namespace System.Notifications.Core.Domain.Notifications.Repositories;

public interface IOutboundNotificationRepository
{
    public Task SaveChangeAsync(OutboundNotifications notificationContext,
        CancellationToken cancellationToken = default);

    public Task<OutboundNotifications?> GeyByIdAsync(Guid id,
        CancellationToken cancellation = default);

    public Task<OutboundNotifications?> GeyByCodeAsync(string code,
        CancellationToken cancellation = default);
}
