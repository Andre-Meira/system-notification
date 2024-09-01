using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Core.Domain.Events.Repositories;

public interface IEventsRepository
{
    public Task SaveChangeAsync(EventsRegistrys eventsRegistrys,
        CancellationToken cancellationToken = default);

    public Task<EventsRegistrys?> GeyByIdAsync(Guid id,
        CancellationToken cancellation = default);

    public Task<EventsRegistrys?> GeyByCodeAsync(string code,
        CancellationToken cancellation = default);
}