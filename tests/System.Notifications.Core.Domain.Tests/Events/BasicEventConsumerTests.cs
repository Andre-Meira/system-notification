using Moq;
using System.Notifications.Core.Domain.Events.Services;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Orders;
using System.Notifications.Core.Domain.Tests.Integration;

namespace System.Notifications.Core.Domain.Tests.Events;

public class BasicEventConsumerTests
{
    public INotificationService EventConsumerSer { get; }

    public BasicEventConsumerTests()
    {
        var eventsRepositoryFixture = new EventsRepositoryFixture();
        var notificationRepositoryFixture = new NotificationRepositoryFixture();
        var outboundNotificationRepositoryFixture = new OutboundNotificationRepositoryFixture();
        var userNotificationRepositoryFixture = new UserNotificationRepositoryFixture();

        var publishNotification = new Mock<IPublishNotification>();

        EventConsumerSer = new BaseNotificationService
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
        var @event = new NotificationMessage("process-order",
            "Ordem processada com sucesso",
            "Pagamento criado e logo caira para aprovação",
            new Order("teste", "testeee"));

        var notifications = await EventConsumerSer.PublishNotificationAsync(@event);

        var notificationError = notifications.Where(e => e.Error.Any());
        Assert.Empty(notificationError);
    }

    [Fact]
    public async Task Publica_Um_Evento_Que_Nao_Esta_Mapeado_Retorna_Uma_Lista_Vazia()
    {
        var @event = new NotificationMessage("process-order-testes",
            "Ordem processada com sucesso",
            "Pagamento criado e logo caira para aprovação",
            new Order("teste", "testeee"));

        var notifications = await EventConsumerSer.PublishNotificationAsync(@event);
        Assert.Empty(notifications);
    }
}
