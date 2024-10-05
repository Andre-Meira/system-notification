using Moq;
using System.Notifications.Core.Domain.Abstracts.Exceptions;
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
    #region PROPS    
    private readonly IUserService _userService;
    private readonly UserNotificationsParametersSamples _userNotificationsParametersSamples;

    private static readonly OutboundNotifications outbound = new OutBoundNotificationSamples().Sms;
    private static readonly EventsRegistrys eventsRegistrys = new EventsRegistrysSamples().OrderEvent;

    private readonly List<UserNotificationSettingsModel> userNotificationsSettings = new List<UserNotificationSettingsModel>
    {
        new UserNotificationSettingsModel
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

        userMock.Setup(e => e.GetAllUsers(It.IsAny<CancellationToken>()))
            .Returns((CancellationToken _) =>
            {
                return Task.FromResult(userNotificationsParameters.List.AsEnumerable());
            });

        var outboudRepository = new OutboundNotificationRepositoryFixture().OutboundNotificationRepository;
        var eventRegistry = new EventsRepositoryFixture().EventsRepository;

        _userService = new UserService(userMock.Object, outboudRepository, eventRegistry);
    }
    #endregion

    [Fact]
    public async Task Cria_Um_Usuario_Valido()
    {
        var userNotification = new UserNotificationsModel("teste@hotmail.com", "00000", userNotificationsSettings);

        Guid id = await _userService.CreateAsync(userNotification, CancellationToken.None);
        Assert.NotEqual(Guid.Empty, id);
    }

    [Fact]
    public async Task Cria_Um_Usuario_Com_Saida_De_Notificacao_Inexistente_Retorna_ExceptionDomain()
    {
        var settings = new List<UserNotificationSettingsModel>
        {
            new UserNotificationSettingsModel
            (
                eventsRegistrys.Id,
                Guid.NewGuid()
            )
        };

        var userNotification = new UserNotificationsModel("teste@hotmail.com", "00000", settings);

        await Assert.ThrowsAsync<ExceptionDomain>(async () =>
        {
            await _userService.CreateAsync(userNotification, CancellationToken.None);
        });
    }

    [Fact]
    public async Task Cria_Um_Usuario_Com_Evento_Inexistente_Retorna_ExceptionDomain()
    {
        var settings = new List<UserNotificationSettingsModel>
        {
            new UserNotificationSettingsModel
            (
                Guid.NewGuid(),
                outbound.Id
            )
        };

        var userNotification = new UserNotificationsModel("teste@hotmail.com", "00000", settings);

        await Assert.ThrowsAsync<ExceptionDomain>(async () =>
        {
            await _userService.CreateAsync(userNotification, CancellationToken.None);
        });
    }


    [Fact]
    public async Task Atualiza_Um_Usuario()
    {
        var user = _userNotificationsParametersSamples.List.FirstOrDefault()!;
        var userEmailOriginal = user.EmailAddress;
        var userContactOriginal = user.Contact;

        var userRequest = new UserNotificationsModel("testeNovo@hotmail.com", "00000121231", userNotificationsSettings);

        await _userService.UpdateAsync(user.Id, userRequest);
        var userChange = _userNotificationsParametersSamples.List.FirstOrDefault(e => e.Id == user.Id)!;


        Assert.True(
            userEmailOriginal.Equals(userChange.EmailAddress) == false &&
            userContactOriginal.Equals(userChange.Contact) == false
        );
    }

    [Fact]
    public async Task Atualiza_Um_Usuario_Existente_Retorna_ExceptionDomain()
    {

        var settings = new List<UserNotificationSettingsModel>
        {
            new UserNotificationSettingsModel
            (
                Guid.NewGuid(),
                outbound.Id
            )
        };

        var userRequest = new UserNotificationsModel("testeNovo@hotmail.com", "00000121231", userNotificationsSettings);

        await Assert.ThrowsAsync<ExceptionDomain>(async () =>
        {
            await _userService.UpdateAsync(Guid.NewGuid(), userRequest);
        });
    }

    [Fact]
    public async Task Atualiza_Um_Usuario_Com_Evento_Inexistente_Retorna_ExceptionDomain()
    {
        var user = _userNotificationsParametersSamples.List.FirstOrDefault()!;

        var settings = new List<UserNotificationSettingsModel>
        {
            new UserNotificationSettingsModel
            (
                Guid.NewGuid(),
                outbound.Id
            )
        };

        var userRequest = new UserNotificationsModel("testeNovo@hotmail.com", "00000121231", settings);

        await Assert.ThrowsAsync<ExceptionDomain>(async () =>
        {
            await _userService.UpdateAsync(user.Id, userRequest);
        });
    }


    [Fact]
    public async Task Atualiza_Um_Usuario_Com_Sainda_Inexistente_Retorna_ExceptionDomain()
    {
        var user = _userNotificationsParametersSamples.List.FirstOrDefault()!;

        var settings = new List<UserNotificationSettingsModel>
        {
            new UserNotificationSettingsModel
            (
                eventsRegistrys.Id,
                Guid.NewGuid()
            )
        };

        var userRequest = new UserNotificationsModel("testeNovo@hotmail.com", "00000121231", settings);

        await Assert.ThrowsAsync<ExceptionDomain>(async () =>
        {
            await _userService.UpdateAsync(user.Id, userRequest);
        });
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

    [Fact]
    public async Task Desativa_Usuario_Existente_Retorna_ExceptionDomain()
    {
        await Assert.ThrowsAsync<ExceptionDomain>(async () =>
        {
            await _userService.DeleteAsync(Guid.NewGuid());
        });
    }

    [Fact]
    public async Task Procura_Usuario_Por_Id_Retonar_User()
    {
        var user = _userNotificationsParametersSamples.List.FirstOrDefault()!;
        Assert.NotNull(await _userService.FindByIdAsync(user.Id));
    }

    [Fact]
    public async Task Procura_Usuario_Inexistente_Retonar_null()
    {
        Assert.Null(await _userService.FindByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task Obtem_Uma_Lista_De_Usuario_Retorna_Lista()
    {
        Assert.NotEmpty(await _userService.GettAllUsers());
    }
}