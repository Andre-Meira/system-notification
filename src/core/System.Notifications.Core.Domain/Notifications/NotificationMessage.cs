namespace System.Notifications.Core.Domain.Notifications;

public record NotificationMessage
{
    public NotificationMessage(string eventCode,
        string message,
        string description,
        object? @event = null)
    {
        Message = message;
        Description = description;
        EventCode = eventCode;
        Event = @event;
    }

    public string Message { get; private set; }

    public string Description { get; private set; }

    public string EventCode { get; init; }

    public object? Event {  get; init; }
}
