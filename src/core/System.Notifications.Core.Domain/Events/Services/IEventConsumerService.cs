using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Events.Services;

public interface IEventConsumerService
{
    Task<IEnumerable<NotificationContext>> PublishNotificationAsync(
        NotificationMessage notificationMessage,
        CancellationToken cancellationToken = default);
}
