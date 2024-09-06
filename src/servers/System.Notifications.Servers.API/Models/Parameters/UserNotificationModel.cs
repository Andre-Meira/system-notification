using System.ComponentModel.DataAnnotations;
using System.Notifications.Core.Domain.Users;

namespace System.Notifications.Servers.API.Models.Parameters;

public class UserNotificationModel
{
	[Required]
	public string Email { get; init; } = null!;

	[Required]
	public string Contact { get; init; } = null!;

	[Required]
	public List<UserNotificationSettingsModel> Settings { get; init; } = new List<UserNotificationSettingsModel>();

	public static explicit operator UserNotificationsRequest(UserNotificationModel model)
	{
		UserNotificationSettingsRequest[] settingsArray = new UserNotificationSettingsRequest[model.Settings.Count];

		for (var index = 0; index < model.Settings.Count; index++)
			settingsArray[index] = (UserNotificationSettingsRequest)model.Settings[index];

		return new UserNotificationsRequest(model.Email, model.Contact, settingsArray.ToList());
	}
}

public record UserNotificationSettingsModel
{
	[Required]
	public Guid EventId { get; init; }

	[Required]
	public Guid OutboundNotificationId { get; init; }

	public static explicit operator UserNotificationSettingsRequest(UserNotificationSettingsModel model)
		=> new UserNotificationSettingsRequest(model.EventId, model.OutboundNotificationId);
}
