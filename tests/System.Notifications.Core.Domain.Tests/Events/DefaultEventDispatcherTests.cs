using System.Notifications.Core.Domain.Events;
namespace System.Notifications.Core.Domain.Tests.Events;

public class DefaultEventDispatcherTests
{
    private readonly EventDispatcherBase _eventDispatcher;

    public DefaultEventDispatcherTests()
    {
        _eventDispatcher = new DefaultEventDispatcher();
    }

    [Fact]
    public async Task Publica_evento_e_valida_se_foi_entregue()
    {
        bool eventPublished = false;

        _eventDispatcher.SubscribeAtEvent<SampleEvent.SampleOrder>("order", e =>
            {
                eventPublished = true;
                return Task.CompletedTask;
            });

        var @object = new SampleEvent.SampleOrder("test");
        await _eventDispatcher.PublishEventAsync("order", @object);

        Assert.True(eventPublished);
    }

    [Fact]
    public async Task Publica_Evento_E_Valida_Se_EventDispatcherStarted_Foi_Executado()
    {
        bool eventPublished = false;

        _eventDispatcher.SubscribeAtEvent<SampleEvent.SampleOrder>("order", e => Task.CompletedTask);
        EventDispatcherBase.EventDispatcherStarted += (string eventCode, object @event) =>
        {
            eventPublished = true;
            return Task.CompletedTask;
        };

        var @object = new SampleEvent.SampleOrder("test");
        await _eventDispatcher.PublishEventAsync("order", @object);

        Assert.True(eventPublished);
    }

    [Fact]
    public async Task Publica_Evento_E_Valida_Se_EventDispatcherCompleted_Foi_Executado()
    {
        bool eventPublished = false;

        _eventDispatcher.SubscribeAtEvent<SampleEvent.SampleOrder>("order", e => Task.CompletedTask);
        EventDispatcherBase.EventDispatcherCompleted += (string eventCode, object @event) =>
        {
            eventPublished = true;
            return Task.CompletedTask;
        };

        var @object = new SampleEvent.SampleOrder("test");
        await _eventDispatcher.PublishEventAsync("order", @object);

        Assert.True(eventPublished);
    }


    [Fact]
    public async Task Publica_Evento_E_Valida_Se_EventDispatcherFailed_Foi_Executado()
    {
        bool eventPublished = false;

        _eventDispatcher.SubscribeAtEvent<SampleEvent.SampleOrder>("order", e => Task.CompletedTask);

        EventDispatcherBase.EventDispatcherFailed += (string eventCode, Exception exception) =>
        {
            eventPublished = true;
            return Task.CompletedTask;
        };

        try
        {
            object @object = new SampleEvent.SampleOrder2(2);
            await _eventDispatcher.PublishEventAsync("order", @object);
        }
        catch (Exception) { }
        finally { Assert.True(eventPublished); }
    }

    [Fact]
    public async Task Publica_Evento_E_Valida_Se_EventDispatcherFailed_Nao_Foi_Executado()
    {
        bool eventPublished = false;

        _eventDispatcher.SubscribeAtEvent<SampleEvent.SampleOrder>("order", e => Task.CompletedTask);

        try
        {
            object @object = new SampleEvent.SampleOrder2(2);
            await _eventDispatcher.PublishEventAsync("order", @object);
        }
        catch (Exception) { }
        finally { Assert.False(eventPublished); }
    }

    [Fact]
    public async Task Publica_Evento_Que_Nao_Tem_Assinatura_Retona_False()
    {
        bool eventPublished = false;
        
        EventDispatcherBase.EventDispatcherStarted += (string eventCode, object @event) =>
        {
            eventPublished = true;
            return Task.CompletedTask;
        };

        var @object = new SampleEvent.SampleOrder("test");
        await _eventDispatcher.PublishEventAsync("teste", @object);

        Assert.False(eventPublished);
    }
}
