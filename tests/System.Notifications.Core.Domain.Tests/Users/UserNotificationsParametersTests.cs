using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Users;

namespace System.Notifications.Core.Domain.Tests.Users;

public class UserNotificationsParametersTests
{   
   public void Desativa_Usuario() 
   {
        Guid guid = Guid.NewGuid();
        var userNotifications = new UserNotificationsParameters(guid, "teste@hotmail.com", "00000000000");
        userNotifications.Disable();

        Assert.True(
            userNotifications.IsEnabled == false && 
            string.IsNullOrEmpty(userNotifications.EmailAddress) &&
            string.IsNullOrEmpty(userNotifications.Contact)
            );
   }

    
    public void Atualiza_Usuario()
    {
        Guid guid = Guid.NewGuid();
        string email = "teste@hotmail.com";
        string contact = "00000000000";

        var userNotifications = new UserNotificationsParameters(guid, email, contact);
        userNotifications.Update("teste1@hotmail.com", "11111111111");

        Assert.True(
            email.Equals(userNotifications.EmailAddress) == false &&
            contact.Equals(userNotifications.Contact) == false
            );
    }


    public void Criar_Usuario_Com_As_Configuracoes_De_Notificacoes()
    {
        var outbound = new OutboundNotifications(Guid.Parse("96627868-708F-4B88-8CDD-8451B287AAB9"), "SMS", "SMS Service", "");
        var eventRegistry = new EventsRegistrys(Guid.Parse("EAF28619-32C2-4220-B298-C588D1F9943D"), "process-order", "processa ordens", "");

        var userNotificationsSettings = new List<UserNotificationSettings>
        {
            new UserNotificationSettings
            (
                eventRegistry.Id,
                outbound.Id,
                eventRegistry.Code,
                outbound.Code
            )
        };

        Guid guid = Guid.NewGuid();
        var userNotifications = new UserNotificationsParameters(guid, "", "");
        userNotifications.AddRangeSetting(userNotificationsSettings);

        Assert.True(userNotifications.NotificationSettings.Any());
    }
}
