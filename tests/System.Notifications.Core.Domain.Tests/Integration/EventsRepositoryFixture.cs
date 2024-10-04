using Moq;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Tests.Events.Samples;

namespace System.Notifications.Core.Domain.Tests.Integration;

public class EventsRepositoryFixture
{
    public IEventsRepository EventsRepository { get; }

    public EventsRepositoryFixture()
    {
        var eventsRepository = new Mock<IEventsRepository>();

        var listEvents = new EventsRegistrysSamples().List;

        eventsRepository.Setup(e =>
            e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())
         )
        .Returns((Guid id, CancellationToken _) =>
        {
            return Task.FromResult(listEvents.FirstOrDefault(e => e.Id == id));
        });

        eventsRepository.Setup(e =>
            e.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
         )
        .Returns((string code, CancellationToken _) =>
        {
            return Task.FromResult(listEvents.FirstOrDefault(e => e.Code == code));
        });

        eventsRepository.Setup(e =>
            e.GetAllAsync(It.IsAny<CancellationToken>())
         )
        .Returns((CancellationToken _) =>
        {
            return Task.FromResult(listEvents.AsEnumerable());
        });

        eventsRepository.Setup(e =>
            e.SaveChangeAsync(It.IsAny<EventsRegistrys>(), It.IsAny<CancellationToken>())
         )
        .Callback((EventsRegistrys @event, CancellationToken _) =>
        {
            listEvents.Add(@event);
        });

        EventsRepository = eventsRepository.Object;
    }
}
