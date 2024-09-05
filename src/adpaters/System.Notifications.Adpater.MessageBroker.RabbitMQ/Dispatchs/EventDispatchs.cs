using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Events;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Dispatchs;

public class EventDispatchs(IPublishContext publishContext) : IPublisEvent
{
    public Task PublishAsync(EventBase @event, CancellationToken cancellationToken)
        => publishContext.PublishTopicMessage(@event, ConstantsRoutings.ExchageEvent);
    
}
