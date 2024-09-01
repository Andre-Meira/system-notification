using Microsoft.Extensions.DependencyInjection;
using System.Notifications.Adpater.DataBase.MongoDB.Configurations;
using System.Notifications.Adpater.DataBase.MongoDB.Options;
using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Users.Repositories;
using Testcontainers.MongoDb;

namespace System.Notifications.Adpater.DataBase.MongoDB.Tests;


public class MongoDbFixture : IAsyncLifetime
{
    public readonly IEventsRepository eventsRepository;
    public readonly INotificationRepository notificationRepository;
    public readonly IOutboundNotificationRepository outboundNotificationRepository;
    public readonly IUserNotificationRepository userNotificationRepository;

    public readonly IServiceProvider ServiceProvider;

    public MongoDbFixture()
    {
        var services = new ServiceCollection();
        services.AddAdpaterMongoDb(new MongoOptions
        {
            Connection = "mongodb://root:root@localhost:27017/",
            DatabaseName = "system-notification-tests"
        });

        ServiceProvider = services.BuildServiceProvider();

        eventsRepository = ServiceProvider.GetRequiredService<IEventsRepository>();
        notificationRepository = ServiceProvider.GetRequiredService<INotificationRepository>();
        outboundNotificationRepository = ServiceProvider.GetRequiredService<IOutboundNotificationRepository>();
        userNotificationRepository = ServiceProvider.GetRequiredService<IUserNotificationRepository>();
        
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    public Task InitializeAsync()
    {
        return Task.CompletedTask;
    }
}