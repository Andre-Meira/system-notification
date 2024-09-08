using Microsoft.AspNetCore.Mvc;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Orders;

namespace System.Notifications.Servers.API.Controllers;

[ApiController]
[Route("[controller]")]
public class EventsController(IPublishEvent publish) : ControllerBase
{
    [HttpPost("notificar-ordem-processada")]
    public async Task<IActionResult> CreateOrder([FromBody] Order order, 
        CancellationToken cancellationToken = default)
    {
        var @event = new EventBase("process-order", order);
        await publish.PublishAsync(@event, cancellationToken);

        return Ok("evento notificado.");
    }
}
