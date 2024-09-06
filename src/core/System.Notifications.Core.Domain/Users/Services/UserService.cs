using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Core.Domain.Users.Services;

public sealed class UserService : IUserService
{
    private readonly IUserNotificationRepository _repository;
    private readonly IOutboundNotificationRepository _outboundNotificationRepository;
    private readonly IEventsRepository _eventsRepository;

    public UserService(IUserNotificationRepository repository,
        IOutboundNotificationRepository outboundNotificationRepository,
        IEventsRepository eventsRepository)
    {
        _repository = repository;
        _outboundNotificationRepository = outboundNotificationRepository;
        _eventsRepository = eventsRepository;
    }

    public async Task<Guid> CreateAsync(UserNotificationsModel user, CancellationToken cancellation = default)
    {
        Guid userId = Guid.NewGuid();

        var userNotifications = new UserNotificationsParameters(userId, user.Email, user.Contact);
        UserNotificationSettings[] settingsArray = new UserNotificationSettings[user.Settings.Count];

        for (int index = 0; index < user.Settings.Count; index++)
        {
            UserNotificationSettingsModel userNotificationSettings = user.Settings[index];

            EventsRegistrys? @event = await _eventsRepository.GeyByIdAsync(userNotificationSettings.EventId);
            
            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            OutboundNotifications? outbound = await _outboundNotificationRepository.GeyByIdAsync(userNotificationSettings.OutboundId);

            if (outbound is null)
                throw new ArgumentNullException(nameof(outbound));

            settingsArray[index] = new UserNotificationSettings(@event.Id, outbound.Id, @event.Code, outbound.Code);
        }

        userNotifications.AddRangeSetting(settingsArray.ToList());
        await _repository.SaveChangeAsync(userNotifications, cancellation);

        return userId;
    }
   

    public async Task UpdateAsync(Guid id, UserNotificationsModel user, CancellationToken cancellation = default)
    {
        UserNotificationsParameters? userNotifications = await _repository.GeyByIdAsync(id, cancellation);

        if (userNotifications is null)
            throw new ExceptionDomain("user não encontrado.");
        
        UserNotificationSettings[] settingsArray = new UserNotificationSettings[user.Settings.Count];

        for (int index = 0; index < user.Settings.Count; index++)
        {
            UserNotificationSettingsModel userNotificationSettings = user.Settings[index];

            EventsRegistrys? @event = await _eventsRepository.GeyByIdAsync(userNotificationSettings.EventId);

            if (@event is null)
                throw new ArgumentNullException(nameof(@event));

            OutboundNotifications? outbound = await _outboundNotificationRepository.GeyByIdAsync(userNotificationSettings.OutboundId);

            if (outbound is null)
                throw new ArgumentNullException(nameof(outbound));

            settingsArray[index] = new UserNotificationSettings(@event.Id, outbound.Id, @event.Code, outbound.Code);
        }

        userNotifications.Update(user.Email, user.Contact);
        userNotifications.AddRangeSetting(settingsArray.ToList());

        await _repository.SaveChangeAsync(userNotifications, cancellation);
    }


    public async Task DeleteAsync(Guid userId, CancellationToken cancellation = default)
    {
        UserNotificationsParameters? userNotifications = await _repository.GeyByIdAsync(userId, cancellation);

        if (userNotifications is null)
            throw new ExceptionDomain("user não encontrado.");

        userNotifications.Disable();
        await _repository.SaveChangeAsync(userNotifications, cancellation);
    }


    public Task<UserNotificationsParameters?> FindByIdAsync(Guid userId, CancellationToken cancellation = default)
        => _repository.GeyByIdAsync(userId, cancellation);
}
