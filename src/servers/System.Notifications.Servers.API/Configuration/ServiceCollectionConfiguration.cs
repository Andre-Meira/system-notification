using Microsoft.AspNetCore.Authentication;
using RabbitMQ.Client;
using System.Notifications.Adpater.DataBase.MongoDB.Configurations;
using System.Notifications.Adpater.DataBase.MongoDB.Options;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Configurations;
using System.Notifications.Adpater.OutBound.Email.Configuration;
using System.Notifications.Servers.API.Autentication;

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
        services.AddEmailAdpater();

        services.AddAuthentication(config =>
        {
            config.DefaultScheme = BasicAuthenticationHandler.Schema;
            config.DefaultAuthenticateScheme = BasicAuthenticationHandler.Schema;
        })
        .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationHandler.Schema, null);

        services.AddSignalR();

        return services;
    }
}
