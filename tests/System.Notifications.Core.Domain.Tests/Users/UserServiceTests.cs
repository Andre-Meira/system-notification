using Moq;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Tests.Events.Samples;
using System.Notifications.Core.Domain.Tests.Integration;
using System.Notifications.Core.Domain.Tests.Notifications.Samples;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;
using System.Notifications.Core.Domain.Users.Services;

namespace System.Notifications.Core.Domain.Tests.Users;

public class UserServiceTests
{
	private readonly IUserService _userService;
	private readonly UserNotificationsParametersSamples _userNotificationsParametersSamples;

	private static readonly OutboundNotifications outbound = new OutBoundNotificationSamples().Sms;
	private static readonly EventsRegistrys eventsRegistrys = new EventsRegistrysSamples().OrderEvent;

    private readonly List<UserNotificationSettingsRequest> userNotificationsSettings = new List<UserNotificationSettingsRequest>
	{
		new UserNotificationSettingsRequest
        (
			eventsRegistrys.Id,
			outbound.Id
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

		var outboudRepository = new OutboundNotificationRepositoryFixture().OutboundNotificationRepository;
		var eventRegistry = new EventsRepositoryFixture().EventsRepository;

        _userService = new UserService(userMock.Object, outboudRepository, eventRegistry);
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
