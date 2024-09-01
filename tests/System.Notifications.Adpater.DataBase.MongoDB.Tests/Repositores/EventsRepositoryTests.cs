using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Repositories;

namespace System.Notifications.Adpater.DataBase.MongoDB.Tests.Repositores;

public class EventsRepositoryTests : IClassFixture<MongoDbFixture>
{
    public static EventsRegistrys EventsRegistrys => new EventsRegistrys(
            Guid.Parse("EAF28619-32C2-4220-B298-C588D1F9943D"),
            "process-order", "processa ordens",
            "notificação sempre sera lançada quando uma ordem for conlucida");

    private readonly IEventsRepository _eventsRepository;

    public EventsRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        _eventsRepository = mongoDbFixture.eventsRepository;
    }

    [Fact]
    public async Task Cria_Um_Novo_Evento_Com_Sucesso()
    {
        await _eventsRepository.SaveChangeAsync(EventsRegistrys);
        var registry = await _eventsRepository.GeyByIdAsync(EventsRegistrys.Id);

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Um_Evento_Inexistente_Retorna_Null()
    {
        var registry = await _eventsRepository.GeyByIdAsync(Guid.NewGuid());

        Assert.Null(registry);
    }

    [Fact]
    public async Task Procura_Um_Evento_De_Notificacao_Pelo_Codigo()
    {
        await _eventsRepository.SaveChangeAsync(EventsRegistrys);
        var registry = await _eventsRepository.GeyByCodeAsync("process-order");

        Assert.NotNull(registry);
    }
}
