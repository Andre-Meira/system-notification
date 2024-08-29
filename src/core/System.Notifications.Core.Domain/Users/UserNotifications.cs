using System.Notifications.Core.Domain.Abstracts.Domain;
using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Users;

public record UserNotifications : Entity
{
    private readonly List<NotificationSettings> _notificationSettings;

    public UserNotifications(string emailAddress, string contact)
    {
        EmailAddress = emailAddress;
        Contact = contact;

        IsEnabled = true;
        CreatedAt = DateTime.Now;

        _notificationSettings = new List<NotificationSettings>();
    }

    public string EmailAddress { get; private set; } = null!;
    
    public string Contact {  get; private set; } = null!;

    public DateTime CreatedAt { get; init; } 
    public DateTime? UpdatedAt { get; private set; }

    public bool IsEnabled { get; private set; }

    public IReadOnlyCollection<NotificationSettings> NotificationSettings => _notificationSettings;

    public void AddSenttings(NotificationSettings notificationSettings)
    {        
        _notificationSettings.Add(notificationSettings);
        UpdatedAt = DateTime.Now;
    }    
}
