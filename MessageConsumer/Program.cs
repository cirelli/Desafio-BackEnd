using Domain.Interfaces;
using Infraestructure.Context;
using Infraestructure.Repositories;
using MessageConsumer;
using Microsoft.EntityFrameworkCore;
using Notification;

var builder = Host.CreateApplicationBuilder(args);

builder.Services
    .AddDbContext<DataContext>(options =>
    {
        string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new Exception("ConnectionString \"DefaultConnection\" not found!");

        connectionString = Environment.ExpandEnvironmentVariables(connectionString);

        options
#if DEBUG
            .UseLoggerFactory(LoggerFactory.Create(p => p.AddConsole()))
            .EnableSensitiveDataLogging()
#endif
            .UseNpgsql(connectionString);
    })

    .AddAutoMapper(typeof(AutoMapperProfiles.Profiles))

    .Configure<KafkaSettings>(builder.Configuration.GetSection(nameof(KafkaSettings)))

    .AddHostedService<OrdersWorker>()

    .AddScoped<NotificationService>()
    .AddScoped<IRepositoryWrapper, RepositoryWrapper>();

var host = builder.Build();
host.Run();
