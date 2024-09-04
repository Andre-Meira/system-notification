using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Users;

namespace System.Notifications.Core.Domain.Tests.Users;

internal class UserNotificationsParametersSamples
{
    public readonly IList<UserNotificationsParameters> List = new List<UserNotificationsParameters>()
    {
        new UserNotificationsParameters(Guid.Parse("A5CD6946-916E-43ED-BA0F-69EAF7C436F5"), "teste@hotmail.com", "0000000"),
        new UserNotificationsParameters(Guid.Parse("320E50A7-0D83-4859-8AC5-0C4986A322BF"), "teste1@hotmail.com", "0000001"),
        new UserNotificationsParameters(Guid.Parse("7005C012-B135-4887-930D-056C7F577967"), "teste2@hotmail.com", "0000002")
    };
   

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

