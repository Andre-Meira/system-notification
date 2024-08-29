namespace System.Notifications.Core.Domain.Abstracts.Domain;

public interface INotificationsDomain
{    
    bool AnyNotificaion();
    void AddNotificationDomain(NotificationDomain notification);
    IReadOnlyCollection<NotificationDomain> AllNotifications();        
}

public sealed record NotificationDomain(string Key, string Value);