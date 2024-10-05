using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using System.Notifications.Adpater.DataBase.MongoDB.Configurations;
using System.Notifications.Adpater.DataBase.MongoDB.Options;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Configurations;
using System.Notifications.Adpater.OutBound.Email.Configuration;
using System.Notifications.Servers.API.Autentication;
using System.Text;

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

        services.AddAuthentication(configureOptions =>
        {
            configureOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            configureOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, configureOptions =>
            {
                configureOptions.SaveToken = true;
                configureOptions.RequireHttpsMetadata = false;
               
                configureOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = configuration["JwtSettings:Issuer"]!,
                    ValidAudience = configuration["JwtSettings:Audience"]!,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!)),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            })
            .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(BasicAuthenticationHandler.Schema, null);

        services.AddSignalR();

        return services;
    }
}
