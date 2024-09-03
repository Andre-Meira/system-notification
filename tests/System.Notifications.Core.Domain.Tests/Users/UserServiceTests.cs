using Moq;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;
using System.Notifications.Core.Domain.Users.Services;

namespace System.Notifications.Core.Domain.Tests.Users;

public class UserServiceTests
{
	private readonly IUserService _userService;
	private readonly List<UserNotificationSettings> userNotificationsSettings = new List<UserNotificationSettings>
	{
		new UserNotificationSettings
		(
			new OutboundNotifications(Guid.Parse("96627868-708F-4B88-8CDD-8451B287AAB9"), "SMS", "SMS Service", ""),
			new EventsRegistrys(Guid.Parse("EAF28619-32C2-4220-B298-C588D1F9943D"), "process-order", "processa ordens", "")
		)
	};

	public UserServiceTests()
	{
		var userMock = new Mock<IUserNotificationRepository>();
		userMock.Setup(e =>
			e.SaveChangeAsync(It.IsAny<UserNotificationsParameters>(), It.IsAny<CancellationToken>())
		)
		.Callback((UserNotificationsParameters userParameter, CancellationToken _) =>
		{
			UserNotificationsParametersSamples.GetSamples().Add(userParameter);
		});

		_userService = new UserService(userMock.Object);
	}

	[Fact]
	public async Task Cria_Um_Usuario_Valido()
	{


		var userNotification = new UserNotificationsRequest("teste@hotmail.com", "00000", userNotificationsSettings);

		Guid id = await _userService.CreateAsync(userNotification, CancellationToken.None);
		Assert.NotEqual(Guid.Empty, id);
	}
}
