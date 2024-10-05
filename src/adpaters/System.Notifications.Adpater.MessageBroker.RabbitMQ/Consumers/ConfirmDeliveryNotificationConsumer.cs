using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Consumers
{
    internal class ConfirmDeliveryNotificationConsumer(INotificationService service) : IConsumerHandler<NotificationContext>
    {
        public async Task Consumer(IConsumerContext<NotificationContext> context)
        {
            context.Message.ConfirmDelivered();
            await service.SaveNotificationsAsync([context.Message]);

            context.NotifyConsumed();
        }
    }
}
