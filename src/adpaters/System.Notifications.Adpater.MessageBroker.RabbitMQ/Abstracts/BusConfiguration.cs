using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Models;
using System.Notifications.Core.Domain.Abstracts.Exceptions;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;

public static class BusConfiguration
{
    private static IConnection? _connection;

    public static IServiceCollection AddBus(this IServiceCollection services,
        IAsyncConnectionFactory connectionFactory)
    {
        connectionFactory.DispatchConsumersAsync = true;
        _connection = connectionFactory.CreateConnection();
        var channelPublish = _connection.CreateModel();

        services.AddScoped<IPublishContext, PublishContext>(e => new PublishContext(channelPublish));

        return services;
    }

    public static IServiceCollection AddConsumer<TConsumerHandler, IMessage>(
        this IServiceCollection services, Action<IConsumerOptions> consumerOptions)
        where TConsumerHandler : IConsumerHandler<IMessage>
        where IMessage : class
    {
        services.AddScoped(typeof(TConsumerHandler));

        if (_connection is null)
            throw new ArgumentNullException(nameof(_connection));

        IModel model = _connection.CreateModel();

        IConsumerOptions optitons = new ConsumerOptions(model);
        consumerOptions.Invoke(optitons);
        optitons.Validate();

        model.Dispose();

        if (optitons.FaultConfig is not null && optitons.FaultConfig.Consumer is not null)
        {
            var interfaceFault = GetConsumerInterface<TConsumerHandler>(typeof(IConsumerFaultHandler<>));
            services.AddScoped(interfaceFault, optitons.FaultConfig.Consumer);
        }        

        services.AddHostedService((provider) =>
        {
            IServiceScopeFactory providerFactory = provider.GetService<IServiceScopeFactory>()!;
            ILoggerFactory logFactory = provider.GetService<ILoggerFactory>()!;

            var log = logFactory.CreateLogger<ConsumerHandlerBase<IMessage, TConsumerHandler>>();

            var consumerHandlerInstance = Activator.CreateInstance(typeof(ConsumerHandlerBase<IMessage, TConsumerHandler>),
                _connection, providerFactory, optitons, log);

            if (consumerHandlerInstance is null)
            {
                throw new ExceptionDomain("Não foi possivel criar a instancia");
            }

            return (ConsumerHandlerBase<IMessage, TConsumerHandler>)consumerHandlerInstance;
        });

        return services;
    }

    
    private static Type GetConsumerInterface<TConsumerHandler>(Type baseType)
    {
        var consumer = typeof(TConsumerHandler).GetInterface(baseType.Name);

        if (consumer is null)
        {
            throw new ExceptionDomain($"O consumer {typeof(TConsumerHandler).Name}, não possui uma message vinculada");
        }

        return consumer;
    }
}
