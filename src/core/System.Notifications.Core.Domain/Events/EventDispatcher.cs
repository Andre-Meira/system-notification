using System.Collections.Concurrent;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;

namespace System.Notifications.Core.Domain.Events;

public class EventDispatcher
{
    private readonly ConcurrentDictionary<string, List<Delegate>> _handlers = new();

    public void Subscribe<T>(string eventCode, Func<T, Task> handler) where T : class
    {
        if (!_handlers.TryGetValue(eventCode, out var handlers))
        {
            handlers = new List<Delegate>();
            _handlers[eventCode] = handlers;
        }

        handlers.Add(handler);
    }

    public async Task PublishAsync<T>(string eventCode, T @event) where T : class
    {
        if (!_handlers.TryGetValue(eventCode, out var handlers))
        {
            return;
        }

        Task[] tasks = new Task[handlers.Count];

        for (int index = 0; index <= _handlers.Count; index++)        
        {
            var parameter = handlers[index].Method.GetParameters()[0].ParameterType;

            string objectString = JsonSerializer.Serialize(@event);
            var argument = JsonSerializer.Deserialize(objectString, parameter);

            if (argument == null)
                throw new ArgumentNullException(nameof(argument));

            tasks[index] = ((dynamic)handlers[index])((dynamic)argument);
        }

        await Task.WhenAll(tasks);
    }
}