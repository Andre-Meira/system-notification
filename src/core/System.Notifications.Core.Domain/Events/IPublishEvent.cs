namespace System.Notifications.Core.Domain.Events;

public interface IPublishEvent
{
    public Task PublishAsync(EventBase @event, CancellationToken cancellationToken);
}
