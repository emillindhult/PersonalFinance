using System.Security.Claims;
using Application.Users.Delete;
using Application.Users.GetUserInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController(
    ISender sender
) : ControllerBase
{
    [HttpGet("me")]
    public async Task<IActionResult> GetMyInfo(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
        {
            return Unauthorized();
        }

        var query = new GetUserInfoQuery(userId);

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(result.Error.Description);
        }

        return Ok(result.Value);
    }

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteUser(CancellationToken cancellationToken)
    {
        var command = new DeleteUserCommand();

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        Response.Cookies.Delete("refreshToken");

        return NoContent();
    }
}
