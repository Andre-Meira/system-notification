namespace System.Notifications.Core.Domain.Users;

public record UserNotificationsRequest(string Email, string Contact, List<UserNotificationSettingsRequest> Settings);

public record UserNotificationSettingsRequest(Guid EventId, Guid OutboundId);