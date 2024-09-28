using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Tests.Integration;

namespace System.Notifications.Core.Domain.Tests.Notifications;

public class OutboundNotificationServiceTests
{
    private readonly IOutboundNotificationService service;
    private readonly IOutboundNotificationRepository repository;

    public OutboundNotificationServiceTests()
    {
        repository = new OutboundNotificationRepositoryFixture().OutboundNotificationRepository;
        service = new OutboundNotificationService(repository);
    }

    [Fact]
    public async Task Cria_Um_Servico_De_Notificao_Que_Ja_Exite_Retorna_ExceptionDomain()
    {
        var model = new OutboundNotificationModel("SMS",
            "Servico de SMS",
            "Recepera um sms sempre que o evento ocorrer");

        await Assert.ThrowsAsync<ExceptionDomain>(() => service.CreateAsync(model));
    }

    [Fact]
    public async Task Cria_Um_Servico_De_Notificao_Que_Ja_Exite_Retorna_Id()
    {
        var model = new OutboundNotificationModel("TELEGRAM",
            "Telegram",
            "Recepera um sms no telegram");

        Guid id = await service.CreateAsync(model);

        Assert.NotEqual(Guid.Empty, id);

        var outbound = await repository.GetByIdAsync(id);
        Assert.NotNull(outbound);
    }

    [Fact]
    public async Task Atualiza_Um_Servico_De_Notificacao_Que_Ja_Existe_Retorna_Sucesso()
    {
        var outboud = await repository.GetByCodeAsync("SMS");

        var nameService = outboud!.Name;
        var descriptionService = outboud!.Description;

        await service.UpdateAsync(outboud.Id, new OutboundNotificationModel("", "teste", "teste1"));

        var outboudUpdated = await repository.GetByIdAsync(outboud.Id);

        Assert.NotEqual(nameService, outboudUpdated!.Name);
        Assert.NotEqual(descriptionService, outboudUpdated!.Description);
    }


    [Fact]
    public async Task Atualiza_Um_Servico_De_Notificacao_Que_Nao_Exite_Retorna_ExceptionDomain()
    {
        await Assert.ThrowsAsync<ExceptionDomain>(() =>
        {
            return service.UpdateAsync(Guid.NewGuid(), new OutboundNotificationModel("", "teste", "teste1"));
        });
    }


    [Fact]
    public async Task Desativa_Um_Servico_De_Notificacao_Que_Ja_Existe_Retorna_Sucesso()
    {
        var outboud = await repository.GetByCodeAsync("SMS");

        await service.DeleteAsync(outboud!.Id);

        var outboudUpdated = await repository.GetByIdAsync(outboud.Id);
        Assert.False(outboudUpdated!.IsEnabled);
    }


    [Fact]
    public async Task Desativa_Um_Servico_De_Notificacao_Que_Nao_Exite_Retorna_ExceptionDomain()
    {
        await Assert.ThrowsAsync<ExceptionDomain>(() =>
        {
            return service.DeleteAsync(Guid.NewGuid());
        });
    }
}
