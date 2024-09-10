using Moq;
using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Enums;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Core.Domain.Tests.Notifications;

public class NotificationChannelFactoryTests :  IClassFixture<IntegrationTests>
{
    private readonly INotificationChannelFactory notificationChannelFactory;

    public NotificationChannelFactoryTests(IntegrationTests integrationTest)
    {
        notificationChannelFactory = integrationTest.NotificationChannelFactory;
    }

    [Fact]
    public async Task Cria_Canal_De_Notificacao()
    {
        var channel = notificationChannelFactory.CreateChannel(OutboundNotificationsType.Sms);
        var message = new Mock<List<NotificationContext>>().Object;
        await channel.PublishAsync(message);
    }

    [Fact]
    public void Cria_Canal_De_Notificacao_Nao_Implementado()
    {
        Assert.Throws<ExceptionDomain>(() =>
        {
            notificationChannelFactory.CreateChannel(OutboundNotificationsType.Email);
        });
    }
}
