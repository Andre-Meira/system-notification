using Moq;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Enums;
using System.Notifications.Core.Domain.Tests.Notifications.Samples;

namespace System.Notifications.Core.Domain.Tests.Notifications;

public class OutboundNotificationsTests
{
    [Fact]
    public void Altera_OutboundNotifications_Para_OutboundNotificationsType_SMS()
    {
        OutboundNotificationsType type = (OutboundNotificationsType)new OutBoundNotificationSamples().Sms;
        Assert.Equal(OutboundNotificationsType.Sms, type);
    }

    [Fact]
    public void Altera_OutboundNotifications_Para_OutboundNotificationsType_Email()
    {
        OutboundNotificationsType type = (OutboundNotificationsType)new OutBoundNotificationSamples().Email;
        Assert.Equal(OutboundNotificationsType.Email, type);
    }

    [Fact]
    public void Altera_OutboundNotifications_Para_OutboundNotificationsType_WebSocket()
    {
        OutboundNotificationsType type = (OutboundNotificationsType)new OutBoundNotificationSamples().WebSocket;
        Assert.Equal(OutboundNotificationsType.WebScoket, type);
    }

    [Fact]
    public void Altera_OutboundNotifications_Para_OutboundNotificationsType_Inexistente_Retorna()
    {
        Assert.Throws<NotImplementedException>(() =>
        {
            var _ = (OutboundNotificationsType)new OutboundNotifications(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>());
        });
    }

    [Fact]
    public void Cria_Um_OutboundNotifications()
    {
        var outbound =  new OutboundNotifications(
                It.IsAny<Guid>(),
                "test",
                "test",
                "test");

        Assert.True(outbound.IsEnabled);
        Assert.NotEqual(Guid.Empty, outbound.Id);
        Assert.NotNull(outbound.Name);
        Assert.NotNull(outbound.Description);
        Assert.NotNull(outbound.Code);
    }

    [Fact]
    public void Desativa_Um_OutboundNotifications()
    {
        var outbound = new OutboundNotifications(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>());

        outbound.Disable();
        Assert.False(outbound.IsEnabled);
    }

    [Fact]
    public void Atualiza_Um_OutboundNotifications()
    {
        var outbound = new OutboundNotifications(
                It.IsAny<Guid>(),
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>());

        outbound.Update("test", "test");

        Assert.NotEqual(It.IsAny<string>(), outbound.Name);
        Assert.NotEqual(It.IsAny<string>(), outbound.Description);
    }
}
