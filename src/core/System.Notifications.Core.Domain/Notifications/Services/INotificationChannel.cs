namespace System.Notifications.Core.Domain.Notifications.Services;

public interface INotificationChannel
{
    public Task PublishAsync(List<NotificationContext> notificationContexts,
        CancellationToken cancellationToken = default);
}
