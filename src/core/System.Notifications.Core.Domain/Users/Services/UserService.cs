using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Core.Domain.Users.Services;

public sealed class UserService(IUserNotificationRepository repository) : IUserService
{
    public async Task<Guid> CreateAsync(UserNotificationsRequest user, CancellationToken cancellation = default)
    {
        Guid userId = Guid.NewGuid();

        var userNotifications = new UserNotificationsParameters(userId, user.Email, user.Contact);
        userNotifications.AddRangeSetting(user.Settings);

        await repository.SaveChangeAsync(userNotifications, cancellation);
        return userId;
    }
    

    public async Task DeleteAsync(Guid userId, CancellationToken cancellation = default)
    {
        UserNotificationsParameters? userNotifications = await repository.GeyByIdAsync(userId, cancellation);

        if (userNotifications is null)
            throw new ExceptionDomain("user não encontrado.");

        userNotifications.Disable();
        await repository.SaveChangeAsync(userNotifications, cancellation);
    }


    public async Task UpdateAsync(Guid id, UserNotificationsRequest user, CancellationToken cancellation = default)
    {
        UserNotificationsParameters? userNotifications = await repository.GeyByIdAsync(id, cancellation);

        if (userNotifications is null)
            throw new ExceptionDomain("user não encontrado.");

        userNotifications.Update(user.Email, user.Contact);
        await repository.SaveChangeAsync(userNotifications, cancellation);
    }


    public Task<UserNotificationsParameters?> FindByIdAsync(Guid userId, CancellationToken cancellation = default)
        => repository.GeyByIdAsync(userId, cancellation);
}
