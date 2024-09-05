using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Core.Domain.Events.Services;

public sealed class BasicEventConsumer : IEventConsumerService
{
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IPublishNotification _publishNotification;
    private readonly IOutboundNotificationRepository _outboundNotificationRepository;
    private readonly IEventsRepository _eventsRepository;

    public BasicEventConsumer(IUserNotificationRepository userNotificationRepository,
        INotificationRepository notificationRepository,
        IPublishNotification publishNotification,
        IOutboundNotificationRepository outboundNotificationRepository,
        IEventsRepository eventsRepository)
    {
        _userNotificationRepository = userNotificationRepository;
        _notificationRepository = notificationRepository;
        _publishNotification = publishNotification;
        _outboundNotificationRepository = outboundNotificationRepository;
        _eventsRepository = eventsRepository;
    }

    public async Task<IEnumerable<NotificationContext>> PublishEventAsync(
        NotificationMessage notificationMessage, 
        CancellationToken cancellationToken = default)
    {
        var notificationContextList = new List<NotificationContext>();

        IEnumerable<UserNotificationsParameters> userNotificationsParameters = await _userNotificationRepository
            .FindUserByEventCodeAsync(notificationMessage.EventCode);

        foreach (UserNotificationsParameters userParameters in userNotificationsParameters)
        {
            foreach (UserNotificationSettings userSettings in userParameters.NotificationSettings)
            {
                var errorList = new List<string>();

                EventsRegistrys? eventRegistry = await _eventsRepository.GeyByIdAsync(userSettings.EventId);
                OutboundNotifications? outBound = await _outboundNotificationRepository.GeyByIdAsync(userSettings.OutboundNotificationId);

                if (eventRegistry == null)
                    errorList.Add("event registry not implementation");

                if (outBound == null)
                    errorList.Add("event registry not implementation");

                var context = new NotificationContext(Guid.NewGuid(),
                    userParameters.Id,
                    notificationMessage,
                    eventRegistry,
                    outBound);

                context.AddErrors(errorList);
                notificationContextList.Add(context);

                await _notificationRepository.SaveChangeAsync(context);
            }
        }

        await _publishNotification.PublishAsync(notificationContextList, cancellationToken);
        return notificationContextList;
    }
}
