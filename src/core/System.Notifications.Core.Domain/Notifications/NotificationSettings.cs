namespace System.Notifications.Core.Domain.Notifications;

public class NotificationSettings
{
    public NotificationSettings(string eventCode, 
        string OutboundCode,
        Guid eventId, 
        Guid outboundNotification)
    {
        EventCode = eventCode;
        EventId = eventId;
        OutboundNotificationCode = OutboundCode;
        OutboundNotificationId = outboundNotification;

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
