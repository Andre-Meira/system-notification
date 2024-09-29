using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Publishers;

public class EmailPublishNotification(IPublishContext _publishContext) : IEmailPublishNotification
{
    public async Task PublishAsync(List<NotificationContext> notificationContexts, 
        CancellationToken cancellationToken = default)
    {
        await _publishContext.PublishTopicMessage(notificationContexts.ToArray(),
            ConstantsRoutings.ExchangePublishNotifications, "email");
    }
}
