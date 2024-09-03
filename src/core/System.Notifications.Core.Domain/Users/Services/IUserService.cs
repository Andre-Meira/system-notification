namespace System.Notifications.Core.Domain.Users.Services;

public interface IUserService
{
    public Task<Guid> CreateAsync(UserNotificationsRequest user, CancellationToken cancellation = default);

    public Task UpdateAsync(Guid id, UserNotificationsRequest user, CancellationToken cancellation = default);

    public Task DeleteAsync(Guid userId, CancellationToken cancellation = default);

    public Task<UserNotificationsParameters?> FindByIdAsync(Guid userId, CancellationToken cancellation = default);
}