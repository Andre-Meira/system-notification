namespace System.Notifications.Core.Domain.Notifications.Repositories;

public interface INotificationRepository
{
    public Task SaveChangeAsync(NotificationContext notificationContext,
        CancellationToken cancellationToken = default);

    public Task<NotificationContext?> GeyByIdAsync(Guid id,
        CancellationToken cancellation = default);
}
