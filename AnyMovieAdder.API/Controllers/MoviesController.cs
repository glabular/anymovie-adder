using AnyMovieAdder.API.DTOs;
using AnyMovieAdder.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnyMovieAdder.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class MoviesController : ControllerBase
{
    [HttpPost("add")]
    public async Task<IActionResult> AddMovieAsync([FromBody] AddMovieRequest request)
    {
        await Task.Delay(100);

        return Ok();
    }
}
