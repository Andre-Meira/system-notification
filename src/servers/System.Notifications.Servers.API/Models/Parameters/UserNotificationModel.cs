using System.ComponentModel.DataAnnotations;

namespace System.Notifications.Servers.API.Models.Parameters;

public class UserNotificationModel
{
    [Required]
    public string Email { get; init; } = null!;

    [Required]
    public string Contact { get; init; } = null!;

    [Required]
    public List<UserNotificationSettingsModel> Settings { get; init; } = new List<UserNotificationSettingsModel>();
}

public record UserNotificationSettingsModel
{
    [Required]
    public string EventCode { get; init; } = null!;

    [Required]
    public string OutboundNotificationCode { get; init; } = null!;
}
