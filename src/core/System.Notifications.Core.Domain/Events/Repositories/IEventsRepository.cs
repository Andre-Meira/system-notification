namespace System.Notifications.Core.Domain.Events.Repositories;

public interface IEventsRepository
{
    public Task SaveChangeAsync(EventsRegistrys eventsRegistrys,
        CancellationToken cancellationToken = default);

    public Task<EventsRegistrys?> GetByIdAsync(Guid id,
        CancellationToken cancellation = default);

    public Task<EventsRegistrys?> GetByCodeAsync(string code,
        CancellationToken cancellation = default);


    public Task<IEnumerable<EventsRegistrys>> GetAllAsync(CancellationToken cancellation = default);
}