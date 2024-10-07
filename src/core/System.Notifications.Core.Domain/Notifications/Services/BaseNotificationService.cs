using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Notifications.Enums;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;
using System.Runtime.CompilerServices;

namespace System.Notifications.Core.Domain.Notifications.Services;

public sealed class BaseNotificationService : INotificationService
{
    #region props
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IPublishNotificationChannel _publishNotification;
    private readonly IOutboundNotificationRepository _outboundNotificationRepository;
    private readonly IEventsRepository _eventsRepository;

    public BaseNotificationService(IUserNotificationRepository userNotificationRepository,
        INotificationRepository notificationRepository,
        IPublishNotificationChannel publishNotification,
        IOutboundNotificationRepository outboundNotificationRepository,
        IEventsRepository eventsRepository)
    {
        _userNotificationRepository = userNotificationRepository;
        _notificationRepository = notificationRepository;
        _publishNotification = publishNotification;
        _outboundNotificationRepository = outboundNotificationRepository;
        _eventsRepository = eventsRepository;
    }
    #endregion

    public async Task ConfirmReceiptNotifications(Guid[] ids)
    {
        foreach (var id in ids)
        {
            var context = await _notificationRepository.GetByIdAsync(id);
            if (context == null) continue;

            context.ConfirmReceipt();
            await _notificationRepository.SaveChangeAsync(context);
        }
    }

    public async Task<IEnumerable<NotificationContext>> PublishNotificationAsync(
        string eventCode,
        NotificationMessage notificationMessage,
        CancellationToken cancellationToken = default)
    {
        var notificationContextList = new List<NotificationContext>();

        var userNotificationsParameters = await _userNotificationRepository
            .FindUserByEventCodeAsync(eventCode);

        foreach (UserNotificationsParameters userParameters in userNotificationsParameters)
        {
            foreach (UserNotificationSettings userSettings in userParameters.NotificationSettings)
            {
                var errorList = new List<string>();

                EventsRegistrys? eventRegistry = await _eventsRepository.GetByIdAsync(userSettings.EventId);
                OutboundNotifications? outBound = await _outboundNotificationRepository.GetByIdAsync(userSettings.OutboundNotificationId);

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

        var notificationPublishs = notificationContextList.Where(e => e.Error.Count == 0).ToList();
        var notificationGroup = notificationPublishs.GroupBy(e => e.OutboundNotifications);

        foreach (var notifications in notificationGroup)
        {
            var channelsType = (OutboundNotificationsType)notifications.Key!;
            await _publishNotification.PublishAsync(channelsType, notifications.ToArray(), cancellationToken);
        }

        return notificationContextList;
    }


    public async Task SaveNotificationsAsync(NotificationContext[] notifications)
    {
        foreach (var notification in notifications)
            await _notificationRepository.SaveChangeAsync(notification);
    }

    public async Task<NotificationContext[]> GetNotificationsAsync(Guid userId,
        string outboundCode,
        int page = 1,
        int itemsPerPage = 10,
         CancellationToken cancellationToken = default)
    {
        var outbound = await _outboundNotificationRepository.GetByCodeAsync(outboundCode);

        if (outbound == null)
            throw new ExceptionDomain("outbound not found");

        return await _notificationRepository.GetNotificationsAsync(userId, outbound.Id, page, itemsPerPage);
    }
}
