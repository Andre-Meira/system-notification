using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Events.Services;
using System.Notifications.Core.Domain.Tests.Integration;

namespace System.Notifications.Core.Domain.Tests.Events;

public class EventRegistryServiceTests
{
    private IEventRegistryService Service;
    private IEventsRepository Repository;

    public EventRegistryServiceTests()
    {
        Repository = new EventsRepositoryFixture().EventsRepository;
        Service = new EventRegistryService(Repository);
    }

    [Fact]
    public async Task Cria_Um_Evento_Que_Ja_Exite_Retorna_ExceptionDomain()
    {
        var model = new EventRegistrysModel("process-order",
            "processa ordens",
            "notificação sempre sera lançada quando uma ordem for conlucida");

        await Assert.ThrowsAsync<ExceptionDomain>(() => Service.CreateAsync(model));
    }

    [Fact]
    public async Task Cria_Um_Evento_Que_Ja_Exite_Retorna_Id()
    {
        var model = new EventRegistrysModel("order-cancelada",
            "Ordem cancelada",
            "Notifiação sera enviada quando a ordeem for cancelada");

        Guid id = await Service.CreateAsync(model);

        Assert.NotEqual(Guid.Empty, id);

        var outbound = await Repository.GetByIdAsync(id);
        Assert.NotNull(outbound);
    }

    [Fact]
    public async Task Atualiza_Evento_Que_Ja_Existe_Retorna_Sucesso()
    {
        var outboud = await Repository.GetByCodeAsync("process-order");

        var nameService = outboud!.Name;
        var descriptionService = outboud!.Description;

        await Service.UpdateAsync(outboud.Id, new EventRegistrysModel("", "teste", "teste1"));

        var outboudUpdated = await Repository.GetByIdAsync(outboud.Id);

        Assert.NotEqual(nameService, outboudUpdated!.Name);
        Assert.NotEqual(descriptionService, outboudUpdated!.Description);
    }


    [Fact]
    public async Task Atualiza_Evento_Que_Nao_Exite_Retorna_ExceptionDomain()
    {
        await Assert.ThrowsAsync<ExceptionDomain>(() =>
        {
            return Service.UpdateAsync(Guid.NewGuid(), new EventRegistrysModel("", "teste", "teste1"));
        });
    }


    [Fact]
    public async Task Desativa_Evento_Que_Ja_Existe_Retorna_Sucesso()
    {
        var outboud = await Repository.GetByCodeAsync("process-order");

        await Service.DeleteAsync(outboud!.Id);

        var outboudUpdated = await Repository.GetByIdAsync(outboud.Id);
        Assert.False(outboudUpdated!.IsEnabled);
    }


    [Fact]
    public async Task Desativa_Evento_Que_Nao_Exite_Retorna_ExceptionDomain()
    {
        await Assert.ThrowsAsync<ExceptionDomain>(() =>
        {
            return Service.DeleteAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Obtem_uma_Lista_De_Eventos_Retona_List()
    {
        var events = await Service.GetAllAsync();
        Assert.NotEmpty(events);
    }
}
