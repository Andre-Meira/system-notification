using System.Notifications.Core.Domain.Abstracts.Domain;

namespace System.Notifications.Core.Domain.Users;

public record UserNotificationsParameters : Entity
{
    private List<UserNotificationSettings> _notificationSettings;

    public UserNotificationsParameters(Guid id, string emailAddress, string contact)
    {
        EmailAddress = emailAddress;
        Contact = contact;

        IsEnabled = true;
        CreatedAt = DateTime.Now;

        _notificationSettings = new List<UserNotificationSettings>();
        Create(id);
    }

    public string EmailAddress { get; private set; } = null!;

    public string Contact { get; private set; } = null!;

    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; private set; }

    public bool IsEnabled { get; private set; }

    public IReadOnlyCollection<UserNotificationSettings> NotificationSettings => _notificationSettings;

    public void AddRangeSetting(List<UserNotificationSettings> notificationSettings)
    {
        _notificationSettings.AddRange(notificationSettings);
        UpdatedAt = DateTime.Now;
    }

    public void Disable()
    {
        EmailAddress = "";
        Contact = "";
        IsEnabled = false;
        UpdatedAt = DateTime.Now;
    }

    public void Update(string email, string contanct)
    {
        EmailAddress = email;
        Contact = contanct;
        UpdatedAt = DateTime.Now;
    }
}
