using System.Notifications.Core.Domain.Abstracts.Domain;
using System.Notifications.Core.Domain.Notifications.Enums;

namespace System.Notifications.Core.Domain.Notifications;

public record OutboundNotifications : Entity
{
    public OutboundNotifications(Guid id, string code, string name, string description)
    {
        Code = code;
        Name = name;
        Description = description;
        IsEnabled = true;
        Create(id);
    }

    public string Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public bool IsEnabled { get; init; }

    public static explicit operator OutboundNotificationsType(OutboundNotifications outbound)
    {
        return outbound.Code switch
        {
            "SMS" => OutboundNotificationsType.Sms,
            "Email" => OutboundNotificationsType.Email,
            "WebSocket" => OutboundNotificationsType.WebScokets,
            _ => throw new NotImplementedException(outbound.Code)    
        };
    }
}