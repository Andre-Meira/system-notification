using System.Notifications.Core.Domain.Events;
using System.Text.Json;

namespace System.Notifications.Core.Domain.Tests.Events;

public class DispatcherEventsTests
{
    private readonly EventDispatcher eventDispatcher;

    public DispatcherEventsTests()
    {
        eventDispatcher = new EventDispatcher();
        eventDispatcher.Subscribe<SampleEvent.SampleOrder>("order", ProcessaTask);
        eventDispatcher.Subscribe<SampleEvent.SampleOrder>("order", ProcessaTask2);
    }

    [Fact]
    public async Task DisparaEventoComSucessoAsync()
    {
        object @object = new SampleEvent.SampleOrder2("a");

        var a = JsonSerializer.Serialize(@object);
        var t1 = JsonSerializer.Deserialize<object>(a);

        await eventDispatcher.PublishAsync<object>("order", t1);
    }

    private Task ProcessaTask(SampleEvent.SampleOrder eventContext)
    {
        Console.WriteLine("Test");
        return Task.CompletedTask;  
    }

    private Task ProcessaTask2(SampleEvent.SampleOrder eventContext)
    {
        Console.WriteLine("Test");
        return Task.CompletedTask;
    }
}
