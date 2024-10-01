using System.Notifications.Core.Domain.Notifications.Enums;

namespace System.Notifications.Core.Domain.Notifications.Repositories;

public interface INotificationRepository
{
    public Task SaveChangeAsync(NotificationContext notificationContext,
        CancellationToken cancellationToken = default);

    public Task<NotificationContext?> GetByIdAsync(Guid id,
        CancellationToken cancellation = default);

    public Task<NotificationContext[]> GetPendingNotifications(Guid userId,
        OutboundNotificationsType notificationsType,
        CancellationToken cancellation = default);
}
