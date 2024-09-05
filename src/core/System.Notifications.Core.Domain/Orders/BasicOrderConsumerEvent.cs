using System.Notifications.Core.Domain.Events.Services;
using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Orders;

public sealed class BasicOrderConsumerEvent : IOrderEvent
{
    private readonly IEventConsumerService _eventConsumerService;

    public BasicOrderConsumerEvent(IEventConsumerService eventConsumerService)
    {
        _eventConsumerService = eventConsumerService;
    } 

    public  Task ProcessedOrderCreated(Order order)
    {
        var @event = new NotificationMessage("process-order",
            "Ordem processada com sucesso",
            "Pagamento criado e logo caira para aprovação",
            order);

        return _eventConsumerService.PublishEventAsync(@event);
    }
}
