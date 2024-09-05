using System.Notifications.Core.Domain.Orders;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Adpater.DataBase.MongoDB.Tests.Repositores;

public class UserNotificationRepositoryTests : IClassFixture<MongoDbFixture>
{
    private readonly IUserNotificationRepository _userNotificationRepository;

    public static UserNotificationsParameters UserNotification => new UserNotificationsParameters(
            Guid.Parse("7D1D2BB5-ED97-40BA-9B67-9048E7558018"),
            "test@hotmail.com",
            "00000000000"
        );

    public UserNotificationRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        _userNotificationRepository = mongoDbFixture.userNotificationRepository;
    }

    [Fact]
    public async Task Cria_Uma_Nova_Parametrizacao_De_Notificacao_De_Usuario_Com_Sucesso()
    {
        var userNotifications = UserNotification;

        var userNotificationSettings = new List<UserNotificationSettings>
        {
            new UserNotificationSettings
            (
                EventsRepositoryTests.EventsRegistrys.Id,
                OutboundNotificationRepositoryTests.OutboundNotifications.Id,
                EventsRepositoryTests.EventsRegistrys.Code,
                OutboundNotificationRepositoryTests.OutboundNotifications.Code
            )
        };

        userNotifications.AddRangeSetting(userNotificationSettings);

        await _userNotificationRepository.SaveChangeAsync(userNotifications);
        var registry = await _userNotificationRepository.GeyByIdAsync(userNotifications.Id);

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Uma_Parametrizacao_de_Usuario_Existente_Retorna_Entidade()
    {
        var registry = await _userNotificationRepository.GeyByIdAsync(UserNotification.Id);
        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Uma_Parametrizacao_de_Usuario_Inexistente_Retorna_Null()
    {
        var registry = await _userNotificationRepository.GeyByIdAsync(Guid.NewGuid());
        Assert.Null(registry);
    }

    [Fact]
    public async Task Procura_Uma_Parametrizacao_de_Usuario_Baseado_no_EventCode_Retorna_Lista_de_Usuarios()
    {
        var registry = await _userNotificationRepository.FindUserByEventCodeAsync("process-order");
        Assert.NotEmpty(registry);
    }
}
