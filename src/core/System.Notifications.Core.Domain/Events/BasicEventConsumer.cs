using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Core.Domain.Events;

public abstract class BasicEventConsumer 
{
    private readonly IUserNotificationRepository _notificationRepository;

    protected BasicEventConsumer(IUserNotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

}
