namespace System.Notifications.Core.Domain.Events;

public abstract class EventDispatcherBase
{
    public delegate Task EventHandler(string eventCode, object @event);

    public delegate Task EventErrorHandler(string eventCode, Exception exception);

    public static event EventHandler? EventDispatcherStarted;

    public static event EventHandler? EventDispatcherCompleted;

    public static event EventErrorHandler? EventDispatcherFailed;

    public abstract Task PublishEventAsync<T>(string eventCode, T @event) where T : class;

    public abstract void SubscribeAtEvent<T>(string eventCode, Func<T, Task> handler) where T : class;

    protected Task DispatcherStarted(string eventCode, object @event)
    {
        if (EventDispatcherStarted is null)
            return Task.CompletedTask;

        return EventDispatcherStarted.Invoke(eventCode, @event);
    }

    protected Task DispatcherCompleted(string eventCode, object @event)
    {
        if (EventDispatcherCompleted is null) 
            return Task.CompletedTask;

        return EventDispatcherCompleted.Invoke(eventCode, @event);
    }

    protected Task DispatcherFailed(string eventCode, Exception exception)
    {
        if (EventDispatcherFailed is null)
            return Task.CompletedTask;

        return EventDispatcherFailed.Invoke(eventCode, exception);
    }
}