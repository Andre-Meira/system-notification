﻿using System.Notifications.Core.Domain.Notifications.Enums;

namespace System.Notifications.Core.Domain.Notifications.Services;

public interface INotificationChannelFactory
{
    INotificationChannel CreateChannel(OutboundNotificationsType channels);
}

public sealed class NotificationChannelFactory : INotificationChannelFactory
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationChannelFactory(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public INotificationChannel CreateChannel(OutboundNotificationsType channel)
    {
        return channel switch 
        {
            OutboundNotificationsType.Email => GetService<IEmailNotification>(),
            OutboundNotificationsType.Sms => GetService<ISmsNotification>(),
            OutboundNotificationsType.WebScokets => GetService<ISocketNotification>(),
            _ => throw new ArgumentException("Canal de notificação não suportado")
        };
    }

    private T GetService<T>()
    {
        var service = _serviceProvider.GetService(typeof(T));

        if (service is null)
            throw new ArgumentException($"notificação nao implementada {nameof(T)}");

        return (T)service;
    }
}