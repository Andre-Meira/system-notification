using Moq;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Tests.Users;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Core.Domain.Tests.Integration;

public class UserNotificationRepositoryFixture
{
    public IUserNotificationRepository UserNotificationRepository { get; }

    public UserNotificationRepositoryFixture()
    {
        var mockUserNotificationRepository = new Mock<IUserNotificationRepository>();

        var listParameters = new UserNotificationsParametersSamples().List;

        mockUserNotificationRepository.Setup(e =>
            e.FindUserByEventCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())
        ).Returns((string code, CancellationToken _) =>
        {
            return Task.FromResult(listParameters.Where(e => e.NotificationSettings.Where(e => e.EventCode == code).Any()));
        });

        mockUserNotificationRepository.Setup(e =>
            e.GetAllUsers(It.IsAny<CancellationToken>())
         )
        .Returns((CancellationToken _) =>
        {
            return Task.FromResult(listParameters.AsEnumerable());
        });

        UserNotificationRepository = mockUserNotificationRepository.Object;
    }
}
