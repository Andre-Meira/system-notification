using System.Notifications.Core.Domain.Events.Services;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Core.Domain.Orders;

public sealed class BasicOrderConsumerEvent : IOrderEvent
{
    private readonly INotificationService _eventConsumerService;

    public BasicOrderConsumerEvent(INotificationService eventConsumerService)
    {
        _eventConsumerService = eventConsumerService;
    }

    public Task ProcessedOrderCreated(Order order)
    {
        var @event = new NotificationMessage("process-order",
            "Ordem processada com sucesso",
            "Pagamento criado e logo caira para aprovação",
            order);

        return _eventConsumerService.PublishNotificationAsync(@event);
    }
}
