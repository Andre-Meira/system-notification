using MongoDB.Driver;
using System.Notifications.Adpater.DataBase.MongoDB.Contexts;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Repositories;

namespace System.Notifications.Adpater.DataBase.MongoDB.Repositores;

internal class NotificationRepository(MongoContext mongoContext) : INotificationRepository
{
    public async Task<NotificationContext?> GeyByIdAsync(Guid id, CancellationToken cancellation = default)
        => await mongoContext.NotificationContext.Find(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken: cancellation);

    public Task SaveChangeAsync(NotificationContext notificationContext, CancellationToken cancellationToken = default)
        => mongoContext.NotificationContext.ReplaceOneAsync(
                filter: e => e.Id == notificationContext.Id,
                replacement: notificationContext,
                options: new ReplaceOptions { IsUpsert = true },
                cancellationToken: cancellationToken);
}
