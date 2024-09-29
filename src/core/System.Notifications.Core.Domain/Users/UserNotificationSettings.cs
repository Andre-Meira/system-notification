namespace System.Notifications.Core.Domain.Users;

public class UserNotificationSettings
{
    public UserNotificationSettings(
        Guid eventId,
        Guid outboundId,
        string eventCode,
        string outboundCode)
    {
        EventCode = eventCode;
        EventId = eventId;

        OutboundNotificationCode = outboundCode;
        OutboundNotificationId = outboundId;

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
