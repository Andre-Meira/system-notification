using Microsoft.Extensions.DependencyInjection;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Core.Domain.Tests.Notifications;

public class IntegrationTests
{
    public IntegrationTests()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddScoped<ISocketPublishNotification, SocketNotificationTest>();
        serviceCollection.AddScoped<ISmsPublishNotification, SmsNotificationTest>();
        serviceCollection.AddScoped<INotificationChannelFactory, NotificationChannelFactory>();

        serviceCollection.AddScoped<IPublishNotification, BasePublishNotification>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        NotificationChannelFactory = serviceProvider.GetRequiredService<INotificationChannelFactory>();
        PublishNotification = serviceProvider.GetRequiredService<IPublishNotification>();
    }

    public INotificationChannelFactory NotificationChannelFactory { get; private set; }

    public IPublishNotification PublishNotification { get; private set; }
}


internal class SocketNotificationTest : ISocketPublishNotification
{

    public Task PublishAsync(
        List<NotificationContext> notificationContexts,
        CancellationToken cancellationToken = default) => Task.CompletedTask;
}

internal class SmsNotificationTest : ISmsPublishNotification
{
    public Task PublishAsync(
    List<NotificationContext> notificationContexts,
    CancellationToken cancellationToken = default) => Task.CompletedTask;
}

