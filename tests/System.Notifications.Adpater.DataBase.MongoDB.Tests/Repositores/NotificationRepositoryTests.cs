using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Tests.Events;

namespace System.Notifications.Adpater.DataBase.MongoDB.Tests.Repositores;

public class NotificationRepositoryTests : IClassFixture<MongoDbFixture>
{
    public static NotificationContext Context => new NotificationContext
        (
            Guid.Parse("8257095D-9052-40EF-900E-85F91AD88176"),
            UserNotificationRepositoryTests.UserNotification.Id,
            OutboundNotificationRepositoryTests.OutboundNotifications,
            EventsRepositoryTests.EventsRegistrys,
            new NotificationMessage(
                nameof(SampleEvent.SampleOrder),
                "order publicada com sucesso",
                "ordem 123 criada",
                new SampleEvent.SampleOrder("teste")
            )
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
        var registry = await _notificationRepository.GeyByIdAsync(Context.Id);

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Uma_Notificao_Inexistente_Retorna_Null()
    {
        var registry = await _notificationRepository.GeyByIdAsync(Guid.NewGuid());
        Assert.Null(registry);
    }
}
