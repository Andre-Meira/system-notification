﻿using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Adpater.DataBase.MongoDB.Tests.Repositores;

public class OutboundNotificationRepositoryTests : IClassFixture<MongoDbFixture>
{
    private readonly IOutboundNotificationRepository _outboundNotification;

    public static OutboundNotifications OutboundNotifications => 
            new OutboundNotifications(Guid.Parse("96627868-708F-4B88-8CDD-8451B287AAB9"),
                "SMS", "SMS Service",
                "envia notificações atraves de SMS");

    public OutboundNotificationRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        _outboundNotification = mongoDbFixture.outboundNotificationRepository;
    }

    [Fact]
    public async Task Cria_Uma_Nova_Saida_De_Notificacao_Com_Sucesso()
    { 
        await _outboundNotification.SaveChangeAsync(OutboundNotifications);
        var registry = await _outboundNotification.GeyByIdAsync(OutboundNotifications.Id);

        Assert.NotNull(registry);
    }

    [Fact]
    public async Task Procura_Uma_Saida_De_Notificacao_Inexistente_Retorna_Null()
    {
        var registry = await _outboundNotification.GeyByIdAsync(Guid.NewGuid());
        Assert.Null(registry);
    }

    [Fact]
    public async Task Procura_Uma_Saida_De_Notificacao_Pelo_Codigo()
    {
        await _outboundNotification.SaveChangeAsync(OutboundNotifications);

        var registry = await _outboundNotification.GeyByCodeAsync("SMS");
        Assert.NotNull(registry);
    }
}