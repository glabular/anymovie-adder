using AnyMovieAdder.API.DTOs;
using AnyMovieAdder.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnyMovieAdder.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class MoviesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> AddMovieAsync([FromBody] AddMovieRequest request)
    {
        if (request is null)
        {
            return BadRequest("Request body is null.");
        }

        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest("Title is required.");
        }

        var movie = new Movie
        {
            Title = request.Title.Trim(),
            Description = request.Description,
            ReleaseYear = request.ReleaseYear,
            Categories = request.Categories ?? []
        };

        await Task.Delay(100);

        return StatusCode(StatusCodes.Status201Created, movie);
    }
}
