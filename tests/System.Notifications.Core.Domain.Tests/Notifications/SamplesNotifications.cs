using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Tests.Events;

namespace System.Notifications.Core.Domain.Tests.Notifications;

public class SamplesNotifications
{
    public static NotificationContext CreateNotificationSMS()
    {
        Guid userID = Guid.NewGuid();
        OutboundNotifications outbound = new OutboundNotifications(Guid.NewGuid(),
            "SMS", "SMS Service", "envia notificações atraves de SMS");

        EventsRegistrys eventsRegistrys = new EventsRegistrys(Guid.NewGuid(), 
            nameof(SampleEvent.SampleOrder), "Sample Order", "");

        NotificationMessage notification = new NotificationMessage(
            nameof(SampleEvent.SampleOrder), 
            "order publicada com sucesso", 
            "ordem 123 criada", 
            new SampleEvent.SampleOrder("teste"));

        return new NotificationContext(Guid.NewGuid(), userID, outbound, eventsRegistrys, notification);
    }
}
