using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Users;

namespace System.Notifications.Core.Domain.Tests.Users;

internal class UserNotificationsParametersSamples
{
    public static List<UserNotificationsParameters> GetSamples()
    {
        var userNotificationsSettings = new List<UserNotificationSettings>
        {
            new UserNotificationSettings
            (
                new OutboundNotifications(Guid.Parse("96627868-708F-4B88-8CDD-8451B287AAB9"), "SMS", "SMS Service", ""),
                new EventsRegistrys(Guid.Parse("EAF28619-32C2-4220-B298-C588D1F9943D"), "process-order", "processa ordens", "")
            )
        };

        var notification = new UserNotificationsParameters(Guid.NewGuid(), "teste@hotmail.com", "0000000");
        notification.AddRangeSetting(userNotificationsSettings);

        var notification1 = new UserNotificationsParameters(Guid.NewGuid(), "teste1@hotmail.com", "0000001");
        notification1.AddRangeSetting(userNotificationsSettings);

        var notification2 = new UserNotificationsParameters(Guid.NewGuid(), "teste2@hotmail.com", "0000002");
        notification2.AddRangeSetting(userNotificationsSettings);


        return new List<UserNotificationsParameters>
        {
            notification, notification1, notification2
        }; 
    }
}
