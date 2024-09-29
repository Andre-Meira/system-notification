namespace System.Notifications.Core.Domain.Users;

public record UserNotificationsModel(string Email, string Contact, List<UserNotificationSettingsModel> Settings);

public record UserNotificationSettingsModel(Guid EventId, Guid OutboundId);