using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Consumers;

internal sealed class EmailConsumer(IEmailNotification emailNotification, IPublishContext publishContext) : IConsumerHandler<NotificationContext[]>
{
    public async Task Consumer(IConsumerContext<NotificationContext[]> context)
    {
        var notifications = await emailNotification.PublishAsync(context.Message);
        await publishContext.PublishDirectMessage(notifications, ConstantsRoutings.ExchangeSaveNotifications);

        context.NotifyConsumed();
    }
}
