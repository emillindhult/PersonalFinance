using Application.Categories.Create;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Categories;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CategoryController(
    ISender sender
) : ControllerBase
{
    // GET: api/<CategoryController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<CategoryController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<CategoryController>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CategoryRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCategoryCommand(request.Name, request.Type);

        var result = await sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(result.Error.Description);
        }

        return Created();
    }

    // PUT api/<CategoryController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<CategoryController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
