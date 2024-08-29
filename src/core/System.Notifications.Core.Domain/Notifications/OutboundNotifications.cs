namespace System.Notifications.Core.Domain.Notifications;

public record OutboundNotifications 
{
    public OutboundNotifications(string code, string name, string description)
    {
        Code = code;
        Name = name;
        Description = description;
        IsEnabled = true;
    }

    public string Code { get; init; }
    public string Name { get; init; }
    public string Description { get; init; }

    public bool IsEnabled { get; init; }
}
