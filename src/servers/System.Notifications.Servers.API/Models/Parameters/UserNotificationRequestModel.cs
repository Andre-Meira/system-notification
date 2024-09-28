using System.ComponentModel.DataAnnotations;
using System.Notifications.Core.Domain.Users;

namespace System.Notifications.Servers.API.Models.Parameters;

public class UserNotificationRequestModel
{
    [Required]
    public string Email { get; init; } = null!;

    [Required]
    public string Contact { get; init; } = null!;

    [Required]
    public List<UserNotificationSettingsRequestModel> Settings { get; init; } = new List<UserNotificationSettingsRequestModel>();

    public static explicit operator UserNotificationsModel(UserNotificationRequestModel model)
    {
        UserNotificationSettingsModel[] settingsArray = new UserNotificationSettingsModel[model.Settings.Count];

        for (var index = 0; index < model.Settings.Count; index++)
            settingsArray[index] = (UserNotificationSettingsModel)model.Settings[index];

        return new UserNotificationsModel(model.Email, model.Contact, settingsArray.ToList());
    }
}

public record UserNotificationSettingsRequestModel
{
    [Required]
    public Guid EventId { get; init; }

    [Required]
    public Guid OutboundNotificationId { get; init; }

    public static explicit operator UserNotificationSettingsModel(UserNotificationSettingsRequestModel model)
        => new UserNotificationSettingsModel(model.EventId, model.OutboundNotificationId);
}
