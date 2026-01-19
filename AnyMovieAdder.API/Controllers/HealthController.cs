using AnyMovieAdder.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnyMovieAdder.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HealthController : ControllerBase
{
    [HttpGet("live")]
    public IActionResult Live()
    {
        return Ok(new
        {
            status = "alive",
            timestamp = DateTime.UtcNow
        });
    }

    [HttpGet("ready")]
    public async Task<IActionResult> Ready([FromServices] AnytypeService anytypeService)
    {
        var reachable = anytypeService.IsAuthorized && await anytypeService.PingAsync();

        return reachable
            ? Ok()
            : StatusCode(StatusCodes.Status503ServiceUnavailable);
    }
}
