using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Publishers;

internal class SocketPublishNotification(IPublishContext _publishContext) : ISocketPublishNotification
{
    public Task PublishAsync(List<NotificationContext> notificationContexts, CancellationToken cancellationToken = default)
        => _publishContext.PublishTopicMessage(notificationContexts.ToArray(),
            ConstantsRoutings.ExchangePublishNotifications, "socket");

}
