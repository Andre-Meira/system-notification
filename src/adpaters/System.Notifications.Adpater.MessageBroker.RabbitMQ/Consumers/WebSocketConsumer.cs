using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Consumers;

internal sealed class WebSocketConsumer(ISocketNotification socketNotification) : IConsumerHandler<NotificationContext[]>
{
    public async Task Consumer(IConsumerContext<NotificationContext[]> context)
    {
        await socketNotification.PublishAsync(context.Message);
        context.NotifyConsumed();
    }
}
