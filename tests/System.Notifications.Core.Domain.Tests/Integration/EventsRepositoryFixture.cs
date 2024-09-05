using Moq;
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
            e.GeyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())
         )
        .Returns((Guid id, CancellationToken _) =>
        {
            return Task.FromResult(listEvents.FirstOrDefault(e => e.Id == id));
        });

        EventsRepository = eventsRepository.Object;
    }
}
