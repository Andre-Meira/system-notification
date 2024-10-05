namespace System.Notifications.Servers.API.Controllers;

//[ApiController]
//[Route("api/[controller]")]
//[ApiExplorerSettings(GroupName = "Events")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
//public class EventsController(IPublishEvent publish) : ControllerBase
//{
//    [HttpPost("notificar-ordem-processada")]
//    public async Task<IActionResult> CreateOrder([FromBody] Order order,
//        CancellationToken cancellationToken = default)
//    {
//        var @event = new EventBase("process-order", order);
//        await publish.PublishAsync(@event, cancellationToken);

//        return Ok("evento notificado.");
//    }
//}
