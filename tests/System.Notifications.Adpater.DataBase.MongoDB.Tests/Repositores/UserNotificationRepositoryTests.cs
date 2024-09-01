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
        UserNotification.AddSetting(new UserNotificationSettings
        (
            OutboundNotificationRepositoryTests.OutboundNotifications,
            EventsRepositoryTests.EventsRegistrys
        ));

        _userNotificationRepository = mongoDbFixture.userNotificationRepository;
    }

    [Fact]
    public async Task Cria_Uma_Nova_Saida_De_Notificacao_Com_Sucesso()
    {
        await _userNotificationRepository.SaveChangeAsync(UserNotification);
        var registry = await _userNotificationRepository.GeyByIdAsync(UserNotification.Id);

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Uma_Saida_De_Notificacao_Inexistente_Retorna_Null()
    {
        var registry = await _userNotificationRepository.GeyByIdAsync(Guid.NewGuid());
        Assert.Null(registry);
    }
}
