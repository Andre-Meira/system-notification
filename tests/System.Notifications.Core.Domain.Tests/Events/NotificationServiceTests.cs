using Moq;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Tests.Integration;

namespace System.Notifications.Core.Domain.Tests.Events;

public class NotificationServiceTests
{
    public INotificationService Service { get; }

    public NotificationServiceTests()
    {
        var eventsRepositoryFixture = new EventsRepositoryFixture();
        var notificationRepositoryFixture = new NotificationRepositoryFixture();
        var outboundNotificationRepositoryFixture = new OutboundNotificationRepositoryFixture();
        var userNotificationRepositoryFixture = new UserNotificationRepositoryFixture();

        var publishNotification = new Mock<IPublishNotificationChannel>();

        Service = new BaseNotificationService
            (
                userNotificationRepositoryFixture.UserNotificationRepository,
                notificationRepositoryFixture.NotificationRepository,
                publishNotification.Object,
                outboundNotificationRepositoryFixture.OutboundNotificationRepository,
                eventsRepositoryFixture.EventsRepository
            );
    }

    [Fact]
    public async Task Publica_Um_Evento_Sem_Errors()
    {
        var @event = new NotificationMessage("Ordem Processada",
            "Ordem processada com sucesso",
            "Pagamento criado e logo caira para aprovação");

        var notifications = await Service.PublishNotificationAsync("process-order", @event);

        var notificationError = notifications.Where(e => e.Error.Any());
        Assert.Empty(notificationError);
    }

    [Fact]
    public async Task Publica_Um_Evento_Que_Nao_Esta_Mapeado_Retorna_Uma_Lista_Vazia()
    {
        var @event = new NotificationMessage(
            "Ordem Processada",
            "Ordem processada com sucesso",
            "Pagamento criado e logo caira para aprovação");

        var notifications = await Service.PublishNotificationAsync("process-order-testes", @event);
        Assert.Empty(notifications);
    }
}
