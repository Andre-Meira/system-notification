namespace System.Notifications.Core.Domain.Orders;

public interface IOrderEvent
{
    public Task ProcessedOrderCreated(Order order);
}
