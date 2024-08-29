using System.Notifications.Core.Domain.Abstracts.Domain;

namespace System.Notifications.Core.Domain.Events;

public record EventsRegistrys : Entity
{
    public EventsRegistrys(string code, string name, string description)
    {
        Code = code;
        Name = name;
        Description = description;

        CreatedAt = DateTime.UtcNow;
        IsEnabled = true;
    }

    public string Code { get; init; }
    public string Name { get; private set; }
    public string Description { get; private set; }

    public DateTime CreatedAt { get; private set; } 
    public DateTime? UpdatedAt { get; private set; }

    public bool IsEnabled { get; private set; }
}
