using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Notifications.Servers.API.Models.Parameters;
using System.Notifications.Core.Domain.Notifications.Services;
using System.Notifications.Core.Domain.Notifications;

namespace System.Notifications.Servers.API.Controllers.Parameters;

[Route("api/[controller]")]
[ApiController]
public class OutboundNotificationsController(IOutboundNotificationService Service) : ControllerBase
{
    [HttpPost("create-outbound")]
    public async Task<IActionResult> Create([FromBody, Required] OutboundNotificationRequestModel outboundNotification)
    {
        Guid id = await Service
            .CreateAsync((OutboundNotificationModel)outboundNotification)
            .ConfigureAwait(false);

        return Ok(new { outboundId = id, message = "saida criado" });
    }

    [HttpPut("update-outbound/{outboundId}")]
    public async Task<IActionResult> Update(
        [FromQuery, Required] Guid outboundId,
        [FromBody, Required] OutboundNotificationRequestModel outboundNotification,
        CancellationToken cancellationToken = default)
    {
        await Service
            .UpdateAsync(outboundId, (OutboundNotificationModel)outboundNotification, cancellationToken)
            .ConfigureAwait(false);

        return Ok(new { outboundId, message = "saida alterado" });
    }

    [HttpDelete("disable-outbound/{outboundId}")]
    public async Task<IActionResult> Disable(
        [FromQuery, Required] Guid outboundId,
        CancellationToken cancellationToken = default)
    {
        await Service.DeleteAsync(outboundId, cancellationToken)
            .ConfigureAwait(false);

        return Ok(new { outboundId, message = "saida desativado" });
    }
}
