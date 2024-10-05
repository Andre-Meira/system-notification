using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Consumers;

internal class ConfirmAckNotificationConsumer(INotificationService service) : IConsumerHandler<Guid[]>
{
    public async Task Consumer(IConsumerContext<Guid[]> context)
    {
        await service.ConfirmReceiptNotifications(context.Message);
        context.NotifyConsumed();
    }
}
