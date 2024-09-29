namespace System.Notifications.Core.Domain.Notifications.Services;

public interface ISocketNotification
{
    Task<NotificationContext[]> PublishAsync(NotificationContext[] notificationContexts,
        CancellationToken cancellationToken = default);
}

public interface ISmsNotification 
{
    Task<NotificationContext[]> PublishAsync(NotificationContext[] notificationContexts,
        CancellationToken cancellationToken = default);
}

public interface IEmailNotification
{
    Task<NotificationContext[]> PublishAsync(NotificationContext[] notificationContexts,
        CancellationToken cancellationToken = default);
}
