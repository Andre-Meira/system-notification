using System.Notifications.Core.Domain.Tests.Events.Samples;
using System.Notifications.Core.Domain.Tests.Notifications.Samples;
using System.Notifications.Core.Domain.Users;

namespace System.Notifications.Core.Domain.Tests.Users;

internal class UserNotificationsParametersSamples
{
    public readonly IList<UserNotificationsParameters> List = new List<UserNotificationsParameters>();

    public UserNotificationsParametersSamples()
    {
        var smsOutbound = new OutBoundNotificationSamples().Sms;
        var @event = new EventsRegistrysSamples().OrderEvent;

        var userNotificationSettings = new List<UserNotificationSettings>
        {
            new UserNotificationSettings
            (
                @event.Id,
                smsOutbound.Id,
                @event.Code,
                smsOutbound.Code
            )
        };

        var notification = new UserNotificationsParameters(Guid.Parse("A5CD6946-916E-43ED-BA0F-69EAF7C436F5"), "teste@hotmail.com", "0000000");
        var notification2 = new UserNotificationsParameters(Guid.Parse("320E50A7-0D83-4859-8AC5-0C4986A322BF"), "teste1@hotmail.com", "0000001");
        var notification3 = new UserNotificationsParameters(Guid.Parse("7005C012-B135-4887-930D-056C7F577967"), "teste2@hotmail.com", "0000002");

        notification.AddRangeSetting(userNotificationSettings);
        List.Add(notification);

        notification2.AddRangeSetting(userNotificationSettings);
        List.Add(notification2);


        notification3.AddRangeSetting(userNotificationSettings);
        List.Add(notification3);
    }

    public IList<UserNotificationsParameters> AddOrReplace(UserNotificationsParameters value)
    {
        var valueExisted = List.FirstOrDefault(e => e.Id == value.Id);

        if (valueExisted is null)
        {
            List.Add(value);
            return List;
        }

        List.Remove(valueExisted);
        List.Add(value);

        return List;
    }
}

