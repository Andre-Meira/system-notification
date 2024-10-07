using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Enums;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Publishers;

public class PublishNotificationChannel(IPublishContext publishContext) : IPublishNotificationChannel
{
    public async Task PublishAsync(
        OutboundNotificationsType outboundNotificationsType, 
        NotificationContext[] notificationContexts, 
        CancellationToken cancellationToken = default)
    {
        await publishContext.PublishTopicMessage(notificationContexts,
            ConstantsRoutings.ExchangePublishNotifications,
            outboundNotificationsType.ToString());
    }
}
