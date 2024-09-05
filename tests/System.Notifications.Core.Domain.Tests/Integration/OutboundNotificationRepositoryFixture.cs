using Moq;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Tests.Notifications.Samples;

namespace System.Notifications.Core.Domain.Tests.Integration;

public class OutboundNotificationRepositoryFixture
{
    public IOutboundNotificationRepository OutboundNotificationRepository { get; }

    public OutboundNotificationRepositoryFixture()
    {
        var outboundNotificationRepository = new Mock<IOutboundNotificationRepository>();

        var listOutbound = new OutBoundNotificationSamples().List;

        outboundNotificationRepository.Setup(e =>
            e.GeyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())
         )
        .Returns((Guid id, CancellationToken _) =>
        {
            return Task.FromResult(listOutbound.FirstOrDefault(e => e.Id == id));
        });

        OutboundNotificationRepository = outboundNotificationRepository.Object;
    }
}
