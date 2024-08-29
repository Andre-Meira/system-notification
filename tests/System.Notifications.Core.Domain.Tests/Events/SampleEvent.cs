namespace System.Notifications.Core.Domain.Tests.Events;

public record SampleEvent
{
    public record SampleOrder(string Name);
    public record SampleOrder2(string Name);
}
