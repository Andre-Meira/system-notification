using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Consumers;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Dispatchs;
using System.Notifications.Core.Domain.Events;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Configurations;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddAdapterMessageBrokerRabbiMQ(this IServiceCollection services,
        IAsyncConnectionFactory asyncConnection)
    {
        services.AddBus(asyncConnection);
        services.AddScoped<IPublisEvent, EventDispatchs>();

        services.AddConsumer<EventConsumer, EventBase>(e =>
        {
            e.ConfigureExchangeConsumer(ConstantsRoutings.ExchageEvent, ExchangeType.Topic);
            e.Configure(ConstantsRoutings.ExchageEventConsumer, ExchangeType.Topic, "");
            e.Validate();
        });

        return services;
    }
}
