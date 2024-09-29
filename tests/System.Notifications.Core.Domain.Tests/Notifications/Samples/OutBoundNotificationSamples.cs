using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Tests.Notifications.Samples;

public class OutBoundNotificationSamples
{
    public List<OutboundNotifications> List = new List<OutboundNotifications>();

    public OutboundNotifications Sms = new OutboundNotifications(
                Guid.Parse("96627868-708F-4B88-8CDD-8451B287AAB9"),
                "SMS", "SMS Service",
                "envia notificações atraves de SMS");

    public OutboundNotifications Email = new OutboundNotifications(
                Guid.Parse("E8171280-D07B-4816-8600-6A49C8A2F246"),
                "EMAIL", "Email ",
                "envia notificações atraves de Email");

    public OutboundNotifications WebSocket = new OutboundNotifications(
                Guid.Parse("7DA62CF2-4164-487F-B911-6266311E4E51"),
                "WEBSOCKET", "WebSocket Service",
                "envia notificações atraves de WebSocket");


    public OutBoundNotificationSamples()
    {
        List.Add(Sms);
        List.Add(Email);
        List.Add(WebSocket);
    }
}
