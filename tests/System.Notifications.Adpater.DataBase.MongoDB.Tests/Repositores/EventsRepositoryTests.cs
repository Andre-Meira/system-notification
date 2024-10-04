using Moq;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Tests.Events.Samples;

namespace System.Notifications.Adpater.DataBase.MongoDB.Tests.Repositores;

public class EventsRepositoryTests : IClassFixture<MongoDbFixture>
{
    public static EventsRegistrys EventsRegistrys => new EventsRegistrysSamples().OrderEvent;

    private readonly IEventsRepository _eventsRepository;

    public EventsRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        _eventsRepository = mongoDbFixture.eventsRepository;
    }

    [Fact]
    public async Task Cria_Um_Novo_Evento_Com_Sucesso()
    {
        await _eventsRepository.SaveChangeAsync(EventsRegistrys);
        var registry = await _eventsRepository.GetByIdAsync(EventsRegistrys.Id);

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Um_Evento_Inexistente_Retorna_Null()
    {
        var registry = await _eventsRepository.GetByIdAsync(Guid.NewGuid());

        Assert.Null(registry);
    }

    [Fact]
    public async Task Procura_Um_Evento_De_Notificacao_Pelo_Codigo()
    {
        await _eventsRepository.SaveChangeAsync(EventsRegistrys);
        var registry = await _eventsRepository.GetByCodeAsync("process-order");

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Um_Evento_De_Notificacao_Pelo_Codigo_Que_Nao_Existe_Retorna_Null()
    {
        var registry = await _eventsRepository.GetByCodeAsync(It.IsAny<string>());
        Assert.Null(registry);
    }


    [Fact] 
    public async Task Obtem_Uma_Lista_De_Eventos_Retorna_Lista()
    {
        await _eventsRepository.SaveChangeAsync(EventsRegistrys);
        var registry = await _eventsRepository.GetAllAsync();
        Assert.NotEmpty(registry);
    }
}
