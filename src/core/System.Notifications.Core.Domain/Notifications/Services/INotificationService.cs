using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Enums;

namespace System.Notifications.Core.Domain.Notifications.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationContext>> PublishNotificationAsync(
        string eventCode,
        NotificationMessage notificationMessage,
        CancellationToken cancellationToken = default);


    public Task ConfirmReceiptNotifications(Guid[] ids);

    Task SaveNotificationsAsync(NotificationContext[] notifications);

    public Task RepublishPendingNotificationsAsync(Guid userId, Guid outboundId,
        CancellationToken cancellation = default);
}
