using Microsoft.AspNetCore.SignalR.Client;
using System.Notifications.Adpater.OutBound.Sockets.Models;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;

namespace System.Notifications.Adpater.OutBound.Sockets.Notifications;

public class BaseSocketNotification : ISocketNotification
{
    private readonly SocketsOptions _socketsOptions;

    public BaseSocketNotification(SocketsOptions socketsOptions)
    {
        _socketsOptions = socketsOptions;
    }

    public async Task<NotificationContext[]> PublishAsync(NotificationContext[] notificationContexts, CancellationToken cancellationToken = default)
    {
        var hub = new HubConnectionBuilder().WithUrl(_socketsOptions.UrlSocket, options =>
        {
            options.Headers.Add("Authorization", _socketsOptions.Token);
        })
        .Build();

        await hub.StartAsync();

        foreach (var notification in notificationContexts)
        {
            await hub.SendAsync("SendNotification", notification);
        }

        return notificationContexts;
    }
}
