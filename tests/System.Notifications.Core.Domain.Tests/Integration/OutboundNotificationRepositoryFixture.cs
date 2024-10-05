using Moq;
using System.Notifications.Core.Domain.Notifications;
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
            e.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>())
         )
        .Returns((Guid id, CancellationToken _) =>
        {
            return Task.FromResult(listOutbound.FirstOrDefault(e => e.Id == id));
        });

        outboundNotificationRepository.Setup(e =>
            e.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
         )
        .Returns((string code, CancellationToken _) =>
        {
            return Task.FromResult(listOutbound.FirstOrDefault(e => e.Code == code));
        });

        outboundNotificationRepository.Setup(e =>
            e.GetAllAsync(It.IsAny<CancellationToken>())
         )
        .Returns((CancellationToken _) =>
        {
            return Task.FromResult(listOutbound.AsEnumerable());
        });

        outboundNotificationRepository.Setup(e =>
            e.SaveChangeAsync(It.IsAny<OutboundNotifications>(), It.IsAny<CancellationToken>())
         )
        .Callback((OutboundNotifications notification, CancellationToken _) =>
        {
            listOutbound.Add(notification);
        });

        OutboundNotificationRepository = outboundNotificationRepository.Object;
    }
}
