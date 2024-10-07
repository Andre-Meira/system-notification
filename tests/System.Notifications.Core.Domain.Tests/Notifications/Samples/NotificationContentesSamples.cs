using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Tests.Events;

namespace System.Notifications.Core.Domain.Tests.Notifications.Samples;

public class NotificationContentesSamples
{
    public static NotificationContext CreateNotificationSMS()
    {
        Guid userID = Guid.NewGuid();
        OutboundNotifications outbound = new OutboundNotifications(Guid.NewGuid(),
            "SMS", "SMS Service", "envia notificações atraves de SMS");

        EventsRegistrys eventsRegistrys = new EventsRegistrys(Guid.NewGuid(),
            nameof(SampleEvent.SampleOrder), "Sample Order", "");

        NotificationMessage notification = new NotificationMessage(
            "Ordem Publicada",
            "order publicada com sucesso",
            "ordem 123 criada",
            new Dictionary<string, string> { { "OrdeID", Guid.NewGuid().ToString() } });

        return new NotificationContext(Guid.NewGuid(), userID, notification, eventsRegistrys, outbound);
    }
}
