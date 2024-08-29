using System.Collections.Concurrent;

namespace System.Notifications.Core.Domain.Abstracts.Domain;

public interface IEventDispatcher
{
    void Subscribe<T>(Func<T,Task> handler) where T : class;
    Task PublishAsync<T>(T @event) where T : DomainEvents;
}

public class EventDispatcher : IEventDispatcher
{
    private readonly ConcurrentDictionary<Type, List<Delegate>> _handlers = new();  

    public void Subscribe<T>(Func<T, Task> handler) where T : class
    {
        if (!_handlers.TryGetValue(typeof(T), out var handlers))
        {
            handlers = new List<Delegate>();
            _handlers[typeof(T)] = handlers;
        }

        handlers.Add(handler);
    }

    public async Task PublishAsync<T>(T @event) where T : DomainEvents
    {
        if (!_handlers.TryGetValue(typeof(T), out var handlers))
        {
            return;
        }

        foreach (var handler in handlers)
        {
            await ((dynamic)handler)((dynamic)@event);
        }
    }
}

public abstract record DomainEvents
{
    public Guid Id { get; init; }
    public DateTime CreatedAt { get; init; }

    public DomainEvents()
    {
        Id = Guid.NewGuid();
        CreatedAt = DateTime.Now;
    }

    public string Name => GetType().Name;
}
