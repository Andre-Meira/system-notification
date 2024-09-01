using MongoDB.Driver;
using System.Notifications.Adpater.DataBase.MongoDB.Contexts;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Adpater.DataBase.MongoDB.Repositores;

internal class UserNotificationRepository(MongoContext mongoContext) : IUserNotificationRepository
{
    public async Task<UserNotificationsParameters?> GeyByIdAsync(Guid id, CancellationToken cancellation = default)
        => await mongoContext.UserNotificationParameter.Find(e => e.Id == id)
                .FirstOrDefaultAsync(cancellationToken: cancellation);

    public Task SaveChangeAsync(UserNotificationsParameters userNotificationsParameters, CancellationToken cancellationToken = default)
       => mongoContext.UserNotificationParameter.ReplaceOneAsync(
            filter: e => e.Id == userNotificationsParameters.Id,
            replacement: userNotificationsParameters,
            options: new ReplaceOptions { IsUpsert = true },
            cancellationToken: cancellationToken);
}
