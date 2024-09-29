using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Notifications.Core.Domain.Events;
using System.Notifications.Core.Domain.Events.Services;
using System.Notifications.Servers.API.Models.Parameters;

namespace System.Notifications.Servers.API.Controllers.Parameters;

[Route("api/[controller]")]
[ApiController]
public class EventsRegistrysController(IEventRegistryService Service) : ControllerBase
{
    [HttpPost("create-event")]
    public async Task<IActionResult> Create(
        [FromBody, Required] EventRegistrysRequestModel eventRegistry)
    {
        Guid id = await Service
            .CreateAsync((EventRegistrysModel)eventRegistry)
            .ConfigureAwait(false);

        return Ok(new { eventId = id, message = "evento criado" });
    }

    [HttpPut("update-event/{eventId}")]
    public async Task<IActionResult> Update(
        Guid eventId,
        [FromBody, Required] EventRegistrysRequestModel eventRegistry,
        CancellationToken cancellationToken = default)
    {
        await Service
            .UpdateAsync(eventId, (EventRegistrysModel)eventRegistry, cancellationToken)
            .ConfigureAwait(false);

        return Ok(new { eventId, message = "evento alterado" });
    }

    [HttpDelete("disable-event/{eventId}")]
    public async Task<IActionResult> Disable(
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        await Service.DeleteAsync(eventId, cancellationToken)
            .ConfigureAwait(false);

        return Ok(new { eventId, message = "evento desativado" });
    }


    [HttpGet]
    public async Task<IActionResult> GettAll(CancellationToken cancellationToken = default)
    {
        return Ok(await Service.GetAllAsync(cancellationToken));
    }
}
