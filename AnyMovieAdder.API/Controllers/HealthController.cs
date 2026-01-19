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
        if (!anytypeService.IsAuthorized)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "not ready",
                reason = "unauthorized"
            });
        }

        bool reachable;
        try
        {
            reachable = await anytypeService.PingAsync();
        }
        catch
        {
            reachable = false;
        }

        if (!reachable)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new
            {
                status = "not ready",
                reason = "anytype service unreachable"
            });
        }

        return Ok(new
        {
            status = "ready"
        });
    }
}
