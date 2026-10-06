using Application.Users.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Users;

[Route("api/[controller]")]
[ApiController]
public sealed class RegisterController(ISender sender)
    : ControllerBase
{
    public sealed record RegisterRequest(
        string Email,
        string FirstName,
        string LastName,
        string Password
    );

    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody] RegisterRequest request,
        CancellationToken cancellationToken
    )
    {
        var command = new RegisterUserCommand(
            request.Email,
            request.FirstName,
            request.LastName,
            request.Password
        );

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        return Ok(result.Value);
    }
}
