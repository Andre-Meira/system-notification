using Moq;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Repositories;

namespace System.Notifications.Core.Domain.Tests.Integration;

public class NotificationRepositoryFixture
{
    public INotificationRepository NotificationRepository { get; }

    public List<NotificationContext> NotificationContexts => new List<NotificationContext>();

    public NotificationRepositoryFixture()
    {
        var notificationRepository = new Mock<INotificationRepository>();

        notificationRepository.Setup(e =>
            e.SaveChangeAsync(It.IsAny<NotificationContext>(), It.IsAny<CancellationToken>())
        )
        .Callback((NotificationContext context, CancellationToken _) =>
        {
            NotificationContexts.Add(context);
        });

        NotificationRepository = notificationRepository.Object;
    }
}
