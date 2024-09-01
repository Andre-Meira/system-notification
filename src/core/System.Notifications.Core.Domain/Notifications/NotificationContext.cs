using System.Notifications.Core.Domain.Abstracts.Domain;
using System.Notifications.Core.Domain.Events;

namespace System.Notifications.Core.Domain.Notifications;

public record NotificationContext : Entity 
{
    public NotificationContext(
        Guid id,
        Guid userNotificationsId,
        OutboundNotifications outboundNotifications,
        EventsRegistrys eventsRegistrys,
        NotificationMessage notificationMessage)
    {
        UserNotificationsId = userNotificationsId;
        OutboundNotifications = outboundNotifications;
        EventsRegistrys = eventsRegistrys;
        NotificationMessage = notificationMessage;

        CreatedAt = DateTime.UtcNow;
        Create(id);
    }

    public Guid UserNotificationsId { get; init; }

    public OutboundNotifications OutboundNotifications { get; init; }

    public EventsRegistrys EventsRegistrys { get; init; }

    public NotificationMessage NotificationMessage { get; init; }

    public bool IsDelivered { get; private set; } = false;
    public bool IsConfirmed { get; private set; } = false; 

    public DateTime CreatedAt {  get; init; }
    public DateTime? DeliveredAt { get; private set; }
    public DateTime? ConfirmedAt { get; private set; }

    public void ConfirmDelivered()
    {
        IsConfirmed = true;
        ConfirmedAt = DateTime.UtcNow;
    }

    public void ConfirmReceipt()
    {
        IsConfirmed = true;
        ConfirmedAt = DateTime.UtcNow;
    }
}
