using Microsoft.Extensions.DependencyInjection;
using System.Notifications.Adpater.OutBound.Email.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.OutBound.Email.Configuration;

public static class ServiceCollectionConfiguration
{
    public static IServiceCollection AddEmailAdpater(this IServiceCollection services)
    {
        services.AddScoped<IEmailNotification, EmailNotificationService>();
        return services;
    }
}
