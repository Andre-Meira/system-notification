using System.ComponentModel.DataAnnotations;
using System.Notifications.Core.Domain.Events;

namespace System.Notifications.Servers.API.Models.Parameters;

public record EventRegistrysRequestModel
{
    [Required]
    public string Code { get; init; } = null!;

    [Required]
    public string Name { get; init; } = null!;

    [Required]
    public string Description { get; init; } = null!;

    public static explicit operator EventRegistrysModel(EventRegistrysRequestModel model)
        => new EventRegistrysModel(model.Code, model.Name, model.Description);
}
