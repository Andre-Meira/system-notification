using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Notifications.Repositories;

namespace System.Notifications.Core.Domain.Notifications.Services;

public interface IOutboundNotificationService
{
    Task<Guid> CreateAsync(OutboundNotificationModel outboundModel, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid id, OutboundNotificationModel outboundModel, CancellationToken cancellationToken = default);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<OutboundNotifications>> GetAllAsync(CancellationToken cancellation = default);
}