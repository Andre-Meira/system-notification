namespace System.Notifications.Core.Domain.Events;

public class EventBase
{
    public string EventCode { get; init; }
    public object EventData { get; init; }
    public DateTime Created { get; init; }

    public EventBase(string eventCode, object eventData)
    {
        EventCode = eventCode;
        EventData = eventData;

        Created = DateTime.Now;
    }
}
