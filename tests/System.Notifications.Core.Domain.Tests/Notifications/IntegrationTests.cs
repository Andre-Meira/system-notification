using Microsoft.Extensions.DependencyInjection;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Tests.Notifications;

public class IntegrationTests
{
    public IntegrationTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ISocketNotification, SocketNotificationTest>();
        serviceCollection.AddScoped<ISmsNotification, SmsNotificationTest>();
        serviceCollection.AddScoped<INotificationChannelFactory, NotificationChannelFactory>();

        serviceCollection.AddScoped<IPublishNotification, PublishNotification>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        NotificationChannelFactory = serviceProvider.GetRequiredService<INotificationChannelFactory>();
        PublishNotification = serviceProvider.GetRequiredService<IPublishNotification>();
    }

    public INotificationChannelFactory NotificationChannelFactory { get; private set; }

    public IPublishNotification PublishNotification { get; private set; }
}


internal class SocketNotificationTest : ISocketNotification
{

    public Task PublishAsync(
        List<NotificationContext> notificationContexts,
        CancellationToken cancellationToken = default) => Task.CompletedTask;
}

internal class SmsNotificationTest : ISmsNotification
{
    public Task PublishAsync(
    List<NotificationContext> notificationContexts,
    CancellationToken cancellationToken = default) => Task.CompletedTask;
}

