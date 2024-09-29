using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Notifications.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationContext>> PublishNotificationAsync(
        NotificationMessage notificationMessage,
        CancellationToken cancellationToken = default);


    Task SaveNotificationsAsync(NotificationContext[] notifications);
}
