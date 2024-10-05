using System.Notifications.Core.Domain.Notifications.Enums;

namespace System.Notifications.Core.Domain.Notifications.Services;

public interface IPublishNotificationChannel
{
    public Task PublishAsync(
        OutboundNotificationsType outboundNotificationsType,
        NotificationContext[] notificationContexts,
        CancellationToken cancellationToken = default);
}
