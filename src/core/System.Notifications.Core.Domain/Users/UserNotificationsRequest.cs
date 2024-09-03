namespace System.Notifications.Core.Domain.Users;

public record UserNotificationsRequest(string Email, string Contact, List<UserNotificationSettings> Settings);