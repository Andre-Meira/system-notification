namespace System.Notifications.Core.Domain.Events.Services;

public interface IEventRegistryService
{
    Task<Guid> CreateAsync(EventRegistrysModel eventRegistry, CancellationToken cancellationToken = default);

    Task UpdateAsync(Guid id, EventRegistrysModel eventRegistry, CancellationToken cancellationToken = default);

    public Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    public Task<IEnumerable<EventsRegistrys>> GetAllAsync(CancellationToken cancellationToken = default);
}
