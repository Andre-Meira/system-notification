using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Core.Domain.Orders;

public sealed class ProcesserOrderEvents : IOrderEvent
{
    private readonly IPublishNotification _publishNotification;
    private readonly INotificationRepository _notificationRepository;

    public ProcesserOrderEvents(IPublishNotification publishNotification,
        INotificationRepository notificationRepository)
    {
        _publishNotification = publishNotification;
        _notificationRepository = notificationRepository;
    }

    public Task ProcessedOrderCreated(Order order)
    {
        throw new NotImplementedException();
    }
}
