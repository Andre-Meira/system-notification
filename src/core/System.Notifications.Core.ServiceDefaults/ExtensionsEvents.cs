using Microsoft.Extensions.DependencyInjection;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Orders;

namespace System.Notifications.Core.ServiceDefaults;

public static class ExtensionsEvents
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();

        var eventDispatch = serviceProvider.GetRequiredService<EventDispatcherBase>();
        var eventOrder = serviceProvider.GetRequiredService<IOrderEvent>();

        eventDispatch.SubscribeAtEvent<Order>("process-order", eventOrder.ProcessedOrderCreated);
        return services;
    }
}
