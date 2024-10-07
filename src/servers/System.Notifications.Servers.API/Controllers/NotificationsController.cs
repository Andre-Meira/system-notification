using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Notifications.Core.Domain.Notifications;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Security.Claims;

namespace System.Notifications.Servers.API.Controllers;

[ApiController]
[Route("api/notifications")]
[ApiExplorerSettings(GroupName = "Notifications")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class NotificationsController(INotificationService service) : ControllerBase
{
    public Guid UserId => Guid.Parse(HttpContext.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? Guid.Empty.ToString());

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification(
        [FromQuery] string eventCode,
        [FromBody] NotificationMessage order,
        CancellationToken cancellationToken = default)
    {
        await service.PublishNotificationAsync(eventCode, order);
        return Ok("evento notificado.");
    }

    [HttpPost("mark-read")]
    public async Task<IActionResult> MarkRead(
        [FromBody] Guid[] ids,
        CancellationToken cancellationToken = default)
    {
        await service.ConfirmReceiptNotifications(ids);
        return Ok("notificacao lidas");
    }

    [HttpGet("{outbound}")]
    public Task<NotificationContext[]> GetNotifications(string outbound,
        [FromQuery] int page = 0,
        [FromQuery] int itemsPerPage = 10,
         CancellationToken cancellation = default) 
    => service.GetNotificationsAsync(UserId, outbound, page, itemsPerPage, cancellation);
}
