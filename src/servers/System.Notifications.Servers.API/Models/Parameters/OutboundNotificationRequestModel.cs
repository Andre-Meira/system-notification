using System.ComponentModel.DataAnnotations;
using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Servers.API.Models.Parameters;

public class OutboundNotificationRequestModel
{
    [Required]
    public string Code { get; init; } = null!;

    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public string Description { get; init; } = null!;

    public static explicit operator OutboundNotificationModel(OutboundNotificationRequestModel model)
        => new OutboundNotificationModel(model.Code, model.Name, model.Description);
}