using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Notifications.Core.Domain.Users;
using System.Notifications.Core.Domain.Users.Services;
using System.Notifications.Servers.API.Models.Parameters;

namespace System.Notifications.Servers.API.Controllers.Parameters;

[ApiController]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Parameters")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class UsersNotificationController(IUserService userService) : ControllerBase
{
    [HttpPost("create-user")]
    public async Task<IActionResult> Create([FromBody, Required] UserNotificationRequestModel UserNotificationModel)
    {
        Guid id = await userService
            .CreateAsync((UserNotificationsModel)UserNotificationModel)
            .ConfigureAwait(false);

        return Ok(new { userId = id, message = "usuario criado" });
    }

    [HttpPut("update-user/{userId}")]
    public async Task<IActionResult> Update(Guid userId,
        [FromBody, Required] UserNotificationRequestModel UserNotificationModel,
        CancellationToken cancellationToken = default)
    {
        await userService
            .UpdateAsync(userId, (UserNotificationsModel)UserNotificationModel, cancellationToken)
            .ConfigureAwait(false);

        return Ok(new { userId, message = "usuario alterado" });
    }

    [HttpDelete("disable-user/{userId}")]
    public async Task<IActionResult> Disable(Guid userId,
        CancellationToken cancellationToken = default)
    {
        await userService.DeleteAsync(userId, cancellationToken)
            .ConfigureAwait(false);

        return Ok(new { userId, message = "usuario desativado" });
    }

    [HttpGet]
    public async Task<IActionResult> GettAll(CancellationToken cancellationToken = default)
    {
        return Ok(await userService.GettAllUsers(cancellationToken));
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> FindById(Guid userId, CancellationToken cancellationToken = default)
    {
        return Ok(await userService.FindByIdAsync(userId, cancellationToken));
    }
}
