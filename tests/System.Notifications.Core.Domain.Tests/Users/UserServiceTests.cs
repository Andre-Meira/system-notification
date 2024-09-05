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
	private readonly UserNotificationsParametersSamples _userNotificationsParametersSamples;
	private static readonly OutboundNotifications outbound = new OutboundNotifications(Guid.Parse("96627868-708F-4B88-8CDD-8451B287AAB9"), "SMS", "SMS Service", "");
	private static readonly EventsRegistrys eventsRegistrys = new EventsRegistrys(Guid.Parse("EAF28619-32C2-4220-B298-C588D1F9943D"), "process-order", "processa ordens", "");

    private readonly List<UserNotificationSettings> userNotificationsSettings = new List<UserNotificationSettings>
	{
		new UserNotificationSettings
		(
            eventsRegistrys.Id,
			outbound.Id,
            eventsRegistrys.Code,
            outbound.Code
        )
	};

	public UserServiceTests()
	{
		var userNotificationsParameters = new UserNotificationsParametersSamples();
        _userNotificationsParametersSamples = userNotificationsParameters;

        var userMock = new Mock<IUserNotificationRepository>();
		userMock.Setup(e =>
			e.SaveChangeAsync(It.IsAny<UserNotificationsParameters>(), It.IsAny<CancellationToken>())
		)
		.Callback((UserNotificationsParameters userParameter, CancellationToken _) =>
		{
            userNotificationsParameters.AddOrReplace(userParameter);
		});

		userMock.Setup(e => e.GeyByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
			.Returns((Guid id, CancellationToken _) =>
			{
				return Task.FromResult(userNotificationsParameters.List.FirstOrDefault(e => e.Id == id));
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

	[Fact]
	public async Task Atualiza_Um_Usuario()
	{
		var user = _userNotificationsParametersSamples.List.FirstOrDefault()!;
		var userEmailOriginal = user.EmailAddress;
		var userContactOriginal = user.Contact;

		var userRequest = new UserNotificationsRequest("testeNovo@hotmail.com", "00000121231", userNotificationsSettings);

        await _userService.UpdateAsync(user.Id, userRequest);
        var userChange = _userNotificationsParametersSamples.List.FirstOrDefault(e => e.Id == user.Id)!;


        Assert.True(
            userEmailOriginal.Equals(userChange.EmailAddress) == false &&
            userContactOriginal.Equals(userChange.Contact) == false
		);
    }


	[Fact]
	public async Task Desativa_Usuario()
	{
        var user = _userNotificationsParametersSamples.List.FirstOrDefault()!;

        await _userService.DeleteAsync(user.Id);
        var userChange = _userNotificationsParametersSamples.List.FirstOrDefault(e => e.Id == user.Id)!;

        Assert.True(
            string.IsNullOrEmpty(userChange.Contact) && 
			string.IsNullOrEmpty(userChange.EmailAddress) &&
            userChange.IsEnabled == false
        );
    }
}
