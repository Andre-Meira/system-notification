using Moq;
using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Tests.Notifications.Samples;

namespace System.Notifications.Adpater.DataBase.MongoDB.Tests.Repositores;

public class OutboundNotificationRepositoryTests : IClassFixture<MongoDbFixture>
{
    private readonly IOutboundNotificationRepository _outboundNotification;

    public static OutboundNotifications OutboundNotifications => new OutBoundNotificationSamples().Sms;

    public OutboundNotificationRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        _outboundNotification = mongoDbFixture.outboundNotificationRepository;
    }

    [Fact]
    public async Task Cria_Uma_Nova_Saida_De_Notificacao_Com_Sucesso()
    {
        await _outboundNotification.SaveChangeAsync(OutboundNotifications);
        var registry = await _outboundNotification.GetByIdAsync(OutboundNotifications.Id);

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Uma_Saida_De_Notificacao_Inexistente_Retorna_Null()
    {
        var registry = await _outboundNotification.GetByIdAsync(Guid.NewGuid());
        Assert.Null(registry);
    }

    [Fact]
    public async Task Procura_Uma_Saida_De_Notificacao_Pelo_Codigo()
    {
        await _outboundNotification.SaveChangeAsync(OutboundNotifications);
        var registry = await _outboundNotification.GetByCodeAsync("SMS");
        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Uma_Saida_De_Notificacao_Pelo_Codigo_Que_Nao_Existe_Retorna_Null()
    {
        var registry = await _outboundNotification.GetByCodeAsync(It.IsAny<string>());
        Assert.Null(registry);
    }

    [Fact]
    public async Task Obtem_Uma_Lista_De_Saidas_Retorna_Lista()
    {
        await _outboundNotification.SaveChangeAsync(OutboundNotifications);
        var registry = await _outboundNotification.GetAllAsync();

        Assert.NotEmpty(registry);
    }
}