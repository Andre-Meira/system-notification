using Microsoft.Extensions.DependencyInjection;

using System.Notifications.Adpater.DataBase.MongoDB.Contexts;
using System.Notifications.Adpater.DataBase.MongoDB.Options;
using System.Notifications.Adpater.DataBase.MongoDB.Repositores;

using System.Notifications.Core.Domain.Events.Repositories;
using System.Notifications.Core.Domain.Notifications.Repositories;
using System.Notifications.Core.Domain.Users.Repositories;

namespace System.Notifications.Adpater.DataBase.MongoDB.Configurations;

public static class ResolveServices
{
    public static IServiceCollection AddAdpaterMongoDb(this IServiceCollection services, MongoOptions mongoOptions)
    {
        MongoContextConfiguration.RegisterSerializer();
        MongoContextConfiguration.RegisterClassMap();
        services.AddTransient<MongoContext>(e => new(mongoOptions));

        services.AddScoped<IEventsRepository, EventsRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IOutboundNotificationRepository, OutboundNotificationRepository>();
        services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();

        return services;
    }
}
