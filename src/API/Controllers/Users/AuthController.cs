using Application.Users.Login;
using Application.Users.Logout;
using Application.Users.RefreshTokens;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthController(
    ISender sender, 
    ILogger<AuthController> logger
) : ControllerBase
{
    public sealed record LoginRequest(string Email, string Password);

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var command = new LoginUserCommand(request.Email, request.Password);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        Response.Cookies.Append("refreshToken", result.Value.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7),
            }
        );

        return Ok(result.Value.AccessToken);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = Request.Cookies["refreshToken"];

        if (string.IsNullOrEmpty(refreshToken))
        {
            return Unauthorized();
        }

        var command = new RefreshTokenCommand(refreshToken);

        var result = await sender.Send(command);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        Response.Cookies.Append("refreshToken", result.Value.RefreshToken, 
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddDays(7),
            }
        );

        return Ok(result.Value.AccessToken);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var result = await sender.Send(new LogoutCommand());

        if (result.IsFailure)
        {
            logger.LogWarning(result.Error.Code);
            return BadRequest(result.Error.Description);
        }

        Response.Cookies.Delete("refreshToken");

        return NoContent();
    }
}
