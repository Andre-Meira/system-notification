using Microsoft.AspNetCore.Mvc;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Orders;

namespace System.Notifications.Servers.API.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IPublisEvent _publisEvent;

    public OrderController(IPublisEvent publisEvent)
    {
        _publisEvent = publisEvent;
    }

    [HttpPost("notificar-ordem-processada")]
    public async Task<IActionResult> CreateOrder([FromBody] Order order, CancellationToken cancellationToken = default)
    {
        var @event = new EventBase("process-order", order);
        await _publisEvent.PublishAsync(@event, cancellationToken);

        return Ok("evento notificado.");
    }
}
