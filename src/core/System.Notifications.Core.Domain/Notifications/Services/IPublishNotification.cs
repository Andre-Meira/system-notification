using System.Notifications.Core.Domain.Notifications.Enums;

namespace System.Notifications.Core.Domain.Notifications.Services;

public interface IPublishNotification
{
    public Task PublishAsync(List<NotificationContext> notificationContexts, CancellationToken cancellationToken = default);
}


public sealed class PublishNotification : IPublishNotification
{
    private readonly INotificationChannelFactory _notificationChannelFactory;

    public PublishNotification(INotificationChannelFactory notificationChannelFactory)
    {
        _notificationChannelFactory = notificationChannelFactory;
    }

    public async Task PublishAsync(List<NotificationContext> notificationContexts,
        CancellationToken cancellationToken = default)
    {
        var notificationGroup = notificationContexts.GroupBy(e => e.OutboundNotifications);

        foreach (var notifications in notificationGroup)
        {
            var channelsType = (OutboundNotificationsType)notifications.Key!;
            INotificationChannel channel = _notificationChannelFactory.CreateChannel(channelsType);

            List<NotificationContext> notificationGrouped = notifications.ToList();
            await channel.PublishAsync(notificationGrouped, cancellationToken);
        }
    }
}
