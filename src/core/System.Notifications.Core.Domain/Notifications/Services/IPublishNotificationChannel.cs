namespace System.Notifications.Core.Domain.Notifications.Services;

public interface IPublishNotificationChannel
{
    public Task PublishAsync(List<NotificationContext> notificationContexts,
        CancellationToken cancellationToken = default);
}
