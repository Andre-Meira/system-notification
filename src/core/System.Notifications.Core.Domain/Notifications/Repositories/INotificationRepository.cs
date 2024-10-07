using System.Notifications.Core.Domain.Notifications.Enums;
using System.Security.Cryptography;

namespace System.Notifications.Core.Domain.Notifications.Repositories;

public interface INotificationRepository
{
    public Task SaveChangeAsync(NotificationContext notificationContext,
        CancellationToken cancellationToken = default);

    public Task<NotificationContext?> GetByIdAsync(Guid id,
        CancellationToken cancellation = default);

    public Task<NotificationContext[]> GetPendingNotifications(Guid userId,
        Guid outboundId,
        CancellationToken cancellation = default);

    public Task<NotificationContext[]> GetNotificationsAsync(
        Guid userId,
        Guid outboundId,
        int page = 1,
        int itemsPerPage = 10,
         CancellationToken cancellationToken = default);
}
