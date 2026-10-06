using Application.Accounts.Create;
using Application.Accounts.Get;
using Application.Accounts.GetById;
using Application.Accounts.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers.Accounts;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountController(
    ISender sender
) : ControllerBase
{
    // GET: api/<AccountController>
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
        {
            return Unauthorized();
        }

        var query = new GetAccountsQuery();

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        return Ok(result.Value);
    }

    // GET api/<AccountController>/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
        {
            return Unauthorized();
        }

        var query = new GetAccountByIdQuery(id);

        var result = await sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        return Ok(result.Value);
    }

    // POST api/<AccountController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] string name, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
        {
            return Unauthorized();
        }

        var command = new CreateAccountCommand(name);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        return Created();
    }

    // PUT api/<AccountController>/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] string name, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out Guid userId))
        {
            return Unauthorized();
        }

        var command = new ChangeAccountNameCommand(id, name);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        return Ok(result.Value);
    }

    // DELETE api/<AccountController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
