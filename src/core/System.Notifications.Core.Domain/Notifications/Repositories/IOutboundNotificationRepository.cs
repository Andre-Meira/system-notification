namespace System.Notifications.Core.Domain.Notifications.Repositories;

public interface IOutboundNotificationRepository
{
    public Task SaveChangeAsync(OutboundNotifications notificationContext,
        CancellationToken cancellationToken = default);

    public Task<OutboundNotifications?> GetByIdAsync(Guid id,
        CancellationToken cancellation = default);

    public Task<OutboundNotifications?> GetByCodeAsync(string code,
        CancellationToken cancellation = default);

    public Task<IEnumerable<OutboundNotifications>> GetAllAsync(CancellationToken cancellation = default);
}
