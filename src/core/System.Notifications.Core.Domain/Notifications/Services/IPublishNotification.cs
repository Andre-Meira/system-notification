namespace System.Notifications.Core.Domain.Notifications.Services;

public interface IPublishNotification
{
    public Task PublishAsync(List<NotificationContext> notificationContexts, CancellationToken cancellationToken = default);
}
