using System.Notifications.Core.Domain.Abstracts.Domain;

namespace System.Notifications.Core.Domain.Users;

public record UserNotificationsParameters : Entity
{
    private readonly List<UserNotificationSettings> _notificationSettings;

    public UserNotificationsParameters(string emailAddress, string contact)
    {
        EmailAddress = emailAddress;
        Contact = contact;

        IsEnabled = true;
        CreatedAt = DateTime.Now;

        _notificationSettings = new List<UserNotificationSettings>();
    }

    public string EmailAddress { get; private set; } = null!;
    
    public string Contact {  get; private set; } = null!;

    public DateTime CreatedAt { get; init; } 
    public DateTime? UpdatedAt { get; private set; }

    public bool IsEnabled { get; private set; }

    public IReadOnlyCollection<UserNotificationSettings> NotificationSettings => _notificationSettings;

    public void AddSenttings(UserNotificationSettings notificationSettings)
    {        
        _notificationSettings.Add(notificationSettings);
        UpdatedAt = DateTime.Now;
    }    
}
