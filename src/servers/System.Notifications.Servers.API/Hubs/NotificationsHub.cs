﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Caching.Memory;
using System.Notifications.Adpater.MessageBroker.RabbitMQ;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Core.Domain.Notifications;
using System.Security.Claims;

namespace System.Notifications.Servers.API.Hubs;

[Authorize]
public class NotificationsHub(ILogger<NotificationsHub> logger, IMemoryCache memoryCache, IPublishContext publishContext) : Hub
{
    public string? UserName => Context.User?.FindFirst(ClaimTypes.Name)?.Value;
    public Guid UserId => Guid.Parse(Context.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? Guid.Empty.ToString());
    public bool IsAuthenticated => Context.User?.Identity?.IsAuthenticated ?? false;

    public override Task OnConnectedAsync()
    {
        if (IsAuthenticated == false)
                Context.Abort();

        logger.LogInformation($"Usuario {UserId} logado!!");
        
        memoryCache.GetOrCreate<string>(UserId, options =>
        {
            return Context.ConnectionId;
        });

        return Task.CompletedTask;
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        memoryCache.Remove(UserId);
        return base.OnDisconnectedAsync(exception);
    }

    public async Task SendNotification(NotificationContext context)
    {
        var connetecId = memoryCache.Get<string>(context.UserNotificationsId);

        if (connetecId == null)
            return;

        await Clients.Client(connetecId).SendAsync("ReceiveNotification", context.NotificationMessage.Message, context.Id);
        await publishContext.PublishTopicMessage(context, ConstantsRoutings.ExchangeDeliveryNotifications);
    }

    public async Task AckNotifications(Guid[] guids)
    {
        await publishContext.PublishTopicMessage(guids, ConstantsRoutings.ExchangeAckNotifications);
    }
}