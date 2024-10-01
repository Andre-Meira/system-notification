using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Enums;

namespace System.Notifications.Core.Domain.Notifications.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationContext>> PublishNotificationAsync(
        NotificationMessage notificationMessage,
        CancellationToken cancellationToken = default);


    public Task ConfirmReceiptNotifications(Guid[] ids);

    Task SaveNotificationsAsync(NotificationContext[] notifications);

    public Task RepublishPendingNotificationsAsync(Guid userId, OutboundNotificationsType notificationsType,
        CancellationToken cancellation = default);
}
