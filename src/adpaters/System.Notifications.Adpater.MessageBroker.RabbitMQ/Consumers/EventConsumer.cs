using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Events;
using System.Text.Json;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Consumers;

public class EventConsumer(EventDispatcherBase dispatcherBase) : IConsumerHandler<EventBase>
{
    public async Task Consumer(IConsumerContext<EventBase> context)
    {
        var @object = JsonSerializer.Deserialize<object>(context.Message.EventData.ToString()!);

        if (@object is null)
            throw new ArgumentNullException(nameof(@object));

        await dispatcherBase.PublishEventAsync(context.Message.EventCode, @object);
        context.NotifyConsumed();
    }
}
