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

    public async Task<IEnumerable<UserNotificationsParameters>> FindUserByEventCodeAsync(string eventCde,
            CancellationToken cancellationToken = default)
    {
        var filter = Builders<UserNotificationsParameters>.Filter
            .ElemMatch(nameof(UserNotificationsParameters.NotificationSettings),
                       Builders<UserNotificationSettings>.Filter.Eq("EventCode", "process-order"));

        var listAsync = await mongoContext.UserNotificationParameter.FindAsync(filter);
        return listAsync.ToList();
    }

    public async Task<IEnumerable<UserNotificationsParameters>> GetAllUsers(CancellationToken cancellation = default)
    {
        return await mongoContext.UserNotificationParameter.AsQueryable().ToListAsync(cancellation);
    }
}
