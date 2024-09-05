using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text.Json;

namespace System.Notifications.Core.Domain.Events;

public sealed class DefaultEventDispatcher : EventDispatcherBase
{
    private static readonly ConcurrentDictionary<string, List<Delegate>> _handlers = new();

    public override void SubscribeAtEvent<T>(string eventCode, Func<T, Task> handler) where T : class
    {
        if (!_handlers.TryGetValue(eventCode, out var handlers))
        {
            handlers = new List<Delegate>();
            _handlers[eventCode] = handlers;
        }

        handlers.Add(handler);        
    }

    public override async Task PublishEventAsync<T>(string eventCode, T @event) where T : class
    {
        if (!_handlers.TryGetValue(eventCode, out var handlers) || handlers.Count == 0)
            return;

        Task[] tasks = new Task[handlers.Count];
        await DispatcherStarted(eventCode, @event);

        try
        {
            for (int index = 0; index < handlers.Count; index++)
            {
                var parameter = handlers[index].Method.GetParameters()[0].ParameterType;
                
                var argument = JsonSerializer.Deserialize(@event.ToString()!, parameter);

                ArgumentNullException.ThrowIfNull(argument);

                tasks[index] = ((dynamic)handlers[index])((dynamic)argument);
            }

            await Task.WhenAll(tasks);
            await DispatcherCompleted(eventCode, tasks);
        }
        catch (Exception err)
        {
            await DispatcherFailed(eventCode, err);
            throw;
        }
    }
}

