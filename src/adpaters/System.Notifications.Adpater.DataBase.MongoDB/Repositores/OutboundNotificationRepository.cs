using MongoDB.Driver;
using System.Notifications.Adpater.DataBase.MongoDB.Contexts;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Repositories;

namespace System.Notifications.Adpater.DataBase.MongoDB.Repositores;

internal sealed class OutboundNotificationRepository(MongoContext mongoContext) : IOutboundNotificationRepository
{
    public async Task<IEnumerable<OutboundNotifications>> GetAllAsync(CancellationToken cancellation = default)
        => await mongoContext.OutboundNotifications.AsQueryable().ToListAsync(cancellation);

    public async Task<OutboundNotifications?> GetByCodeAsync(string code, CancellationToken cancellation = default)
        => await mongoContext.OutboundNotifications.Find(e => e.Code == code)
            .FirstOrDefaultAsync(cancellationToken: cancellation);

    public async Task<OutboundNotifications?> GetByIdAsync(Guid id, CancellationToken cancellation = default)
        => await mongoContext.OutboundNotifications.Find(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken: cancellation);

    public Task SaveChangeAsync(OutboundNotifications notificationContext, CancellationToken cancellationToken = default)
             => mongoContext.OutboundNotifications.ReplaceOneAsync(
                filter: e => e.Id == notificationContext.Id,
                replacement: notificationContext,
                options: new ReplaceOptions { IsUpsert = true },
                cancellationToken: cancellationToken);
}
