using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Notifications.Servers.API.Autentication;
using System.Security.Claims;
using System.Text;

namespace System.Notifications.Servers.API.Controllers;

[ApiController]
[Route("api/authentication")]
[ApiExplorerSettings(GroupName = "Authorization")]
[Authorize(AuthenticationSchemes = BasicAuthenticationHandler.Schema)]
public class AuthenticationController(IConfiguration configuration) : ControllerBase
{
    public string? UserName => HttpContext.User?.FindFirst(ClaimTypes.Name)?.Value;
    public Guid UserId => Guid.Parse(HttpContext.User!.FindFirst(ClaimTypes.NameIdentifier)!.Value ?? Guid.Empty.ToString());

    [HttpPost("generate-token")]
    public IActionResult GenerateToken()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(configuration["JwtSettings:Key"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
                    new Claim(ClaimTypes.Name, UserName!),
                    new Claim(ClaimTypes.NameIdentifier, UserId.ToString())
            ]),

            Expires = DateTime.Now.AddHours(4),
            Issuer = configuration["JwtSettings:Issuer"]!,
            Audience = configuration["JwtSettings:Audience"]!,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        string BearerToken = tokenHandler.WriteToken(token);

        return Ok(BearerToken); 
    }
}
