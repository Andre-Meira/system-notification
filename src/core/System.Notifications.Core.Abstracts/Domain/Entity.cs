using System.Notifications.Core.Domain.Abstracts.Exceptions;

namespace System.Notifications.Core.Domain.Abstracts.Domain;

public abstract record Entity : INotificationsDomain
{
    private Guid _id = Guid.Empty;
    public Guid Id => _id;

    private readonly List<NotificationDomain> _notifications = new List<NotificationDomain>();
    
    protected virtual void Create(Guid id = default)
    {
        Validate();

        if (_id.Equals(Guid.Empty) == false)
            throw new ExceptionDomain("já existe um id cadastrado para essa entidade.");

        _id = id == default ? Guid.NewGuid(): id;
    }

    public virtual void Validate()
    {
        bool isInvalid = AnyNotificaion();

        if (isInvalid == false) 
            return;            
        
        throw new ExceptionDomain(_notifications);
    }    

    public bool AnyNotificaion() => _notifications.Any();
    public void AddNotificationDomain(NotificationDomain notification) => _notifications.Add(notification);
    public IReadOnlyCollection<NotificationDomain> AllNotifications() => _notifications;
}
