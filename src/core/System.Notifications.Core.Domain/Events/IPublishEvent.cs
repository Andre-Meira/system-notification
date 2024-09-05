namespace System.Notifications.Core.Domain.Events;

public interface IPublisEvent
{
    public Task PublishAsync(EventBase @event, CancellationToken cancellationToken);
}
