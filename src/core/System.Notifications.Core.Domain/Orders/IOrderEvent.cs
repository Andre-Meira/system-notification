namespace System.Notifications.Core.Domain.Orders;

internal interface IOrderEvent
{
    public Task ProcessedOrderCreated(Order order);
}
