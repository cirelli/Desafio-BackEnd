using System.Text.Json;
using System.Xml.Linq;
using Confluent.Kafka;
using Confluent.Kafka.Admin;
using Microsoft.Extensions.Options;

namespace Notification;

public class NotificationService(IOptions<KafkaSettings> settings)
{
    private const string Topic = "Orders";
    private readonly ClientConfig config = CreateConfig(settings.Value);

    private static ClientConfig CreateConfig(KafkaSettings settings)
    {
        Dictionary<string, string> config = new()
        {
            { "bootstrap.servers", settings.BootstrapServers }
        };

        return new ClientConfig(config);
    }

    public async Task ProduceAsync(string key, object message)
    {
        await CreateTopicMaybeAsync(Topic);

        using var producer = new ProducerBuilder<string, string>(config).Build();

        producer.Produce(Topic, new Message<string, string> { Key = key, Value = JsonSerializer.Serialize(message) },
            (deliveryReport) =>
            {
                if (deliveryReport.Error.Code != ErrorCode.NoError)
                {
                    throw new Exception($"Failed to deliver message: {deliveryReport.Error.Reason}");
                }
            });

        producer.Flush(TimeSpan.FromSeconds(10));
    }

    public async Task Consume(Func<string, string, Task> messageReceivedAction, CancellationToken cancellationToken)
    {
        var consumerConfig = new ConsumerConfig(config)
        {
            GroupId = nameof(NotificationService.Consume),
            //AutoOffsetReset = AutoOffsetReset.Earliest,
            //EnableAutoCommit = false
        };

        using var consumer = new ConsumerBuilder<string, string>(consumerConfig).Build();

        await CreateTopicMaybeAsync(Topic);
        consumer.Subscribe(Topic);
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                ConsumeResult<string, string> cr = consumer.Consume(cancellationToken);
                _ = messageReceivedAction(cr.Message.Key, cr.Message.Value);
            }
        }
        catch (OperationCanceledException)
        {
            
        }
        finally
        {
            consumer.Close();
        }
    }

    private async Task CreateTopicMaybeAsync(string name)
    {
        using var adminClient = new AdminClientBuilder(config).Build();

        try
        {
            await adminClient.CreateTopicsAsync([new TopicSpecification { Name = name, NumPartitions = 1, ReplicationFactor = 1 }]);
        }
        catch (CreateTopicsException e)
        {
            if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
            {
                throw new Exception($"An error occured creating topic {name}: {e.Results[0].Error.Reason}");
            }
        }
    }
}
