using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Events.Services;

public interface IEventConsumerService
{
    Task<IEnumerable<NotificationContext>> PublishEventAsync(
        NotificationMessage notificationMessage,
        CancellationToken cancellationToken = default);
}
