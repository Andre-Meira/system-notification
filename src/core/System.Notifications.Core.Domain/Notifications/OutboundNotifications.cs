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
    public string Name { get; private set; }
    public string Description { get; private set; }

    public bool IsEnabled { get; private set; }

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

    public void Disable() => IsEnabled = false;

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }
}