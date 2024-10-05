﻿using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Consumers;
using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Configurations;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddAdapterMessageBrokerRabbiMQ(this IServiceCollection services,
        IAsyncConnectionFactory asyncConnection)
    {
        services.AddBus(asyncConnection);

        return services;
    }

    public static IServiceCollection AddAdapterConsumerMessageBrokerRabbiMQ(this IServiceCollection services,
        IAsyncConnectionFactory asyncConnection)
    {
        services.AddAdapterMessageBrokerRabbiMQ(asyncConnection);

        services.AddConsumer<EmailConsumer, NotificationContext[]>(e =>
        {
            e.ConfigureExchangeConsumer(ConstantsRoutings.ExchangePublishNotifications, ExchangeType.Topic);
            e.Configure(ConstantsRoutings.ExchageEmailConsumer, ExchangeType.Topic, "email");
            e.Validate();
        });

        services.AddConsumer<WebSocketConsumer, NotificationContext[]>(e =>
        {
            e.ConfigureExchangeConsumer(ConstantsRoutings.ExchangePublishNotifications, ExchangeType.Topic);
            e.Configure(ConstantsRoutings.ExchageSocketConsumer, ExchangeType.Topic, "socket");
            e.Validate();
        });

        services.AddConsumer<SaveNotificationsConsumer, NotificationContext[]>(e =>
        {
            e.ConfigureExchangeConsumer(ConstantsRoutings.ExchangeSaveNotifications, ExchangeType.Direct);
            e.Configure(ConstantsRoutings.ExchangeSaveNotificationsConsumer, ExchangeType.Direct);
            e.Validate();
        });

        services.AddConsumer<ConfirmDeliveryNotificationConsumer, NotificationContext>(e =>
        {
            e.ConfigureExchangeConsumer(ConstantsRoutings.ExchangeDeliveryNotifications, ExchangeType.Topic);
            e.Configure(ConstantsRoutings.ExchangeDeliveryNotificationsConsumer, ExchangeType.Topic);
            e.Validate();
        });

        services.AddConsumer<ConfirmAckNotificationConsumer, Guid[]>(e =>
        {
            e.ConfigureExchangeConsumer(ConstantsRoutings.ExchangeAckNotifications, ExchangeType.Topic);
            e.Configure(ConstantsRoutings.ExchangeAckNotificationsConsumer, ExchangeType.Topic);
            e.Validate();
        });

        return services;
    }
}
