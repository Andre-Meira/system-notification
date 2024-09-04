using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Users;

public class UserNotificationSettings
{ 
    public UserNotificationSettings(OutboundNotifications outbound, EventsRegistrys eventsRegistrys)
    {
        EventCode = eventsRegistrys.Code;
        EventId = eventsRegistrys.Id;

        OutboundNotificationCode = outbound.Code;
        OutboundNotificationId = outbound.Id;

        CreatedAt = DateTime.Now;
        IsEnabled = true;
    }

    public string EventCode { get; init; }
    public string OutboundNotificationCode { get; init; }

    public Guid EventId { get; init; }
    public Guid OutboundNotificationId { get; init; }

    public DateTime CreatedAt { get; init; }

    public bool IsEnabled { get; init; }
}
