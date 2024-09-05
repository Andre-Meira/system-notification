using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Events;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Consumers;

public class EventConsumer(EventDispatcherBase dispatcherBase) : IConsumerHandler<EventBase>
{
    public async Task Consumer(IConsumerContext<EventBase> context)
    {
        await dispatcherBase.PublishEventAsync(context.Message.EventCode, context.Message.EventData);
        context.NotifyConsumed();
    }
}
