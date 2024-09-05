using System.Linq.Expressions;

namespace System.Notifications.Core.Domain.Users.Repositories;

public interface IUserNotificationRepository
{
    public Task SaveChangeAsync(UserNotificationsParameters userNotificationsParameters,
        CancellationToken cancellationToken = default);

    public Task<UserNotificationsParameters?> GeyByIdAsync(Guid id, 
        CancellationToken cancellation = default);

    public Task<List<UserNotificationsParameters>> FindUserByEventCodeAsync(string code,
            CancellationToken cancellationToken = default);
}
