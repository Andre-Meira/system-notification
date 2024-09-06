using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Notifications.Core.Domain.Users.Services;
using System.Notifications.Servers.API.Models.Parameters;

namespace System.Notifications.Servers.API.Controllers.Parameters;

[Route("api/[controller]")]
[ApiController]
public class UsersNotificationController(IUserService userService) : ControllerBase
{
    [HttpPost("create-user")]
    public async Task<IActionResult> CreateUser([FromBody, Required] UserNotificationModel UserNotificationModel)
    {        
        return Ok(UserNotificationModel);
    }

    [HttpPost("update-user")]
    public async Task<IActionResult> UpdateUser([FromBody, Required] UserNotificationModel UserNotificationModel)
    {
        return Ok(UserNotificationModel);
    }
}
