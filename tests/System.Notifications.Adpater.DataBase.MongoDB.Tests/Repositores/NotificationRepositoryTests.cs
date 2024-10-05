using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Enums;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Tests.Events;

namespace System.Notifications.Adpater.DataBase.MongoDB.Tests.Repositores;

public class NotificationRepositoryTests : IClassFixture<MongoDbFixture>
{
    public static NotificationContext Context => new NotificationContext
        (
            Guid.Parse("8257095D-9052-40EF-900E-85F91AD88176"),
            UserNotificationRepositoryTests.UserNotification.Id,
            new NotificationMessage(
                "Ordem publicada",
                "order publicada com sucesso",
                "ordem 123 criada",
                new Dictionary<string, string>
                {
                    { "OrdeID", Guid.NewGuid().ToString() } 
                }
            ),
            EventsRepositoryTests.EventsRegistrys,
            OutboundNotificationRepositoryTests.OutboundNotifications
        );

    private readonly INotificationRepository _notificationRepository;

    public NotificationRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        _notificationRepository = mongoDbFixture.notificationRepository;
    }

    [Fact]
    public async Task Cria_Uma_Nova_Notificacao_Com_Sucesso()
    {
        await _notificationRepository.SaveChangeAsync(Context);
        var registry = await _notificationRepository.GetByIdAsync(Context.Id);

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Uma_Notificao_Inexistente_Retorna_Null()
    {
        var registry = await _notificationRepository.GetByIdAsync(Guid.NewGuid());
        Assert.Null(registry);
    }

    [Fact]
    public async Task Procura_Por_Notificacao_Pendende_Por_Usuario_Retorna_Notificao()
    {
        NotificationContext contextPedente = Context;

        NotificationContext contextNaoPendente =  new NotificationContext
        (
            Guid.Parse("de63ed15-5d0e-41ef-97f9-db016fb394fb"),
            UserNotificationRepositoryTests.UserNotification.Id,
            new NotificationMessage(
                nameof(SampleEvent.SampleOrder),
                "order publicada com sucesso",
                "ordem 123 criada",
                new Dictionary<string, string>
                {
                    { "OrdeID", Guid.NewGuid().ToString() }
                }
            ),
            EventsRepositoryTests.EventsRegistrys,
            OutboundNotificationRepositoryTests.OutboundNotifications
        );
        contextNaoPendente.ConfirmDelivered();

        await _notificationRepository.SaveChangeAsync(contextPedente);
        await _notificationRepository.SaveChangeAsync(contextNaoPendente);

        var a = OutboundNotificationsType.Sms.ToString();

        NotificationContext[] registry = await _notificationRepository.GetPendingNotifications(
            Context.UserNotificationsId, 
            contextPedente.OutboundNotifications!.Id);

        Assert.NotEmpty(registry);
        Assert.Contains(contextPedente.Id, registry.Select(e => e.Id));
    }
}
