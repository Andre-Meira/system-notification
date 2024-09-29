using System.Notifications.Core.Domain.Abstracts.Exceptions;
using System.Notifications.Core.Domain.Events.Repositories;

namespace System.Notifications.Core.Domain.Events.Services;

public class EventRegistryService(IEventsRepository eventsRepository) : IEventRegistryService
{
    public async Task<Guid> CreateAsync(EventRegistrysModel eventRegistry,
        CancellationToken cancellationToken = default)
    {
        Guid idEvent = Guid.NewGuid();

        EventsRegistrys? eventExist = await eventsRepository
            .GetByCodeAsync(eventRegistry.Code, cancellationToken);

        if (eventExist is not null)
            throw new ExceptionDomain($"O evento {eventExist.Code} já exite.");

        EventsRegistrys newEventRegistrys = new EventsRegistrys(idEvent,
            eventRegistry.Code,
            eventRegistry.Name,
            eventRegistry.Description);

        await eventsRepository.SaveChangeAsync(newEventRegistrys);
        return idEvent;
    }


    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        EventsRegistrys? @event = await eventsRepository.GetByIdAsync(id, cancellationToken);

        if (@event is null)
            throw new ExceptionDomain($"O evento não foi encontrado.");

        @event.Disable();
        await eventsRepository.SaveChangeAsync(@event);
    }

    public Task<IEnumerable<EventsRegistrys>> GetAllAsync(CancellationToken cancellationToken = default)
        => eventsRepository.GetAllAsync(cancellationToken);

    public async Task UpdateAsync(Guid id, EventRegistrysModel eventRegistry,
        CancellationToken cancellationToken = default)
    {
        EventsRegistrys? @event = await eventsRepository.GetByIdAsync(id, cancellationToken);

        if (@event is null)
            throw new ExceptionDomain($"O evento não foi encontrado.");

        @event.Upadate(eventRegistry.Name, eventRegistry.Description);
        await eventsRepository.SaveChangeAsync(@event);
    }
}
