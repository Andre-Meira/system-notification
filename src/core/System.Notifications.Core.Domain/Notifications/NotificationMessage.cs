namespace System.Notifications.Core.Domain.Notifications;

public record NotificationMessage
{
    public NotificationMessage(string title,
        string message,
        string description,
        Dictionary<string, string>? arguments = null)
    {
        Message = message;
        Description = description;
        Title = title;
        Arguments = arguments;
    }

    public string Message { get; init; }
    public string Description { get; init; }
    public string Title { get; init; }
    public Dictionary<string, string>? Arguments { get; init; }
}
