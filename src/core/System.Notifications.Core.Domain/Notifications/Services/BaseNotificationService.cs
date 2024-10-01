﻿using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Notifications.Enums;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Core.Domain.Notifications.Services;

public sealed class BaseNotificationService : INotificationService
{
    private readonly IUserNotificationRepository _userNotificationRepository;
    private readonly INotificationRepository _notificationRepository;
    private readonly IPublishNotification _publishNotification;
    private readonly IOutboundNotificationRepository _outboundNotificationRepository;
    private readonly IEventsRepository _eventsRepository;

    public BaseNotificationService(IUserNotificationRepository userNotificationRepository,
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
        await _publishNotification.PublishAsync(notificationPublishs, cancellationToken);

        return notificationContextList;
    }

    public async Task RepublishPendingNotificationsAsync(Guid userId, OutboundNotificationsType notificationsType,
        CancellationToken cancellation = default)
    {
        var notifications = await _notificationRepository.GetPendingNotifications(userId, notificationsType);
        await _publishNotification.PublishAsync(notifications.ToList(), cancellation);
    }

    public async Task SaveNotificationsAsync(NotificationContext[] notifications)
    {
        foreach (var notification in notifications)
            await _notificationRepository.SaveChangeAsync(notification);
    }
}