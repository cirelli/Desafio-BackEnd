using System.Text.Json;
using Domain.Entities;
using Domain.Interfaces;
using Notification;

namespace MessageConsumer
{
    public class OrdersWorker(IServiceProvider Services,
                              ILogger<OrdersWorker> Logger)
        : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation($"{nameof(OrdersWorker)} is running.");

            using IServiceScope scope = Services.CreateScope();

            var notificationService = scope.ServiceProvider
                .GetRequiredService<NotificationService>();

            _ = notificationService.Consume(ProcessMessage, stoppingToken);

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            Logger.LogInformation($"{nameof(OrdersWorker)} is stopping.");

            await base.StopAsync(stoppingToken);
        }

        private async Task ProcessMessage(string key, string message)
        {
            Logger.LogInformation("Message received! key: {key}, message: {message}", key, message);

            try
            {
            using IServiceScope scope = Services.CreateScope();
            IRepositoryWrapper repositoryWrapper = scope.ServiceProvider.GetRequiredService<IRepositoryWrapper>();

            await SaveToDbAsync(repositoryWrapper, key, message);

            Task task = key switch
            {
                NotificationKeys.OrderCreated => OrderCreatedProcessorAsync(repositoryWrapper, message),
                _ => UnknownMessageProcessorAsync(key)
            };

            await task.WaitAsync(CancellationToken.None);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Exception");
            }
        }

        private static async Task SaveToDbAsync(IRepositoryWrapper repositoryWrapper, string key, string message)
        {
            repositoryWrapper.Message.Create(new Message { Key = key, Value = message });
            await repositoryWrapper.SaveAsync(CancellationToken.None);
        }

        private Task UnknownMessageProcessorAsync(string key)
        {
            Logger.LogWarning("Unknown message key \"{key}\"", key);
            return Task.CompletedTask;
        }

        private static async Task OrderCreatedProcessorAsync(IRepositoryWrapper repositoryWrapper, string message)
        {
            Order order = JsonSerializer.Deserialize<Order>(message)!;

            var driverIds = await repositoryWrapper.Driver.GetAllIdAvailableAsync(CancellationToken.None);

            foreach (var driverId in driverIds)
            {
                var notification = new Domain.Entities.Notification()
                {
                    DriverId = driverId,
                    OrderId = order.Id
                };
                repositoryWrapper.Notification.Create(notification);
            }

            await repositoryWrapper.SaveAsync(CancellationToken.None);
        }
    }
}
