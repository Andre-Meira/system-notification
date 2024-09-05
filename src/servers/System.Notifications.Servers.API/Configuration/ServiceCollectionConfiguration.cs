using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Notifications.Adpater.DataBase.MongoDB.Configurations;
using System.Notifications.Adpater.DataBase.MongoDB.Options;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Configurations;
using System.Notifications.Adpater.OutBound.Email.Configuration;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Services;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Orders;

namespace System.Notifications.Servers.API.Configuration;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        var busOptions = configuration.GetSection("Bus").Get<ConnectionFactory>();
        var mongoOptions = configuration.GetSection(MongoOptions.Key).Get<MongoOptions>();

        if (busOptions is null || mongoOptions is null)
        {
            throw new ArgumentNullException();
        }

        services.AddAdpaterMongoDb(mongoOptions);
        services.AddAdapterMessageBrokerRabbiMQ(busOptions);

        services.AddScoped<EventDispatcherBase, DefaultEventDispatcher>();
        services.AddScoped<IEventConsumerService, BasicEventConsumer>();
        services.AddScoped<IPublishNotification, PublishNotification>();
        services.AddScoped<INotificationChannelFactory, NotificationChannelFactory>();

        services.AddScoped<IOrderEvent, BasicOrderConsumerEvent>();

        var serviceProvider = services.BuildServiceProvider();
        
        var eventDispatch = serviceProvider.GetRequiredService<EventDispatcherBase>();
        var eventOrder = serviceProvider.GetRequiredService<IOrderEvent>();

        eventDispatch.SubscribeAtEvent<Order>("process-order", eventOrder.ProcessedOrderCreated);

        services.AddEmailAdpater();

        return services;
    }
}
