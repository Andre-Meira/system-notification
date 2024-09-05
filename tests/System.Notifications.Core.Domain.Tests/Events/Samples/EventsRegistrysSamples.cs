using System.Notifications.Core.Domain.Events;

namespace System.Notifications.Core.Domain.Tests.Events.Samples;

public class EventsRegistrysSamples
{
    public List<EventsRegistrys> List = new List<EventsRegistrys>();

    public EventsRegistrys OrderEvent = new EventsRegistrys(
            Guid.Parse("EAF28619-32C2-4220-B298-C588D1F9943D"),
            "process-order", "processa ordens",
            "notificação sempre sera lançada quando uma ordem for conlucida");

    public EventsRegistrysSamples()
    {
        List.Add(OrderEvent);
    }
}
