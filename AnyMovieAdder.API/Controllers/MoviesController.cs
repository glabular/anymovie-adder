using AnyMovieAdder.API.DTOs;
using AnyMovieAdder.API.Models;
using AnyMovieAdder.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnyMovieAdder.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class MoviesController : ControllerBase
{
    private AnytypeService _anytypeService;

    public MoviesController(AnytypeService anytypeService)
    {
        _anytypeService = anytypeService ?? throw new ArgumentNullException(nameof(anytypeService));
    }

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

        if (!_anytypeService.IsAuthorized)
        {
            return BadRequest("You are not authorized.");
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

    [HttpPost("authorize")]
    public IActionResult Authorize()
    {
        try
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return BadRequest("Authorization header required.");
            }

            var value = authHeader.ToString().Trim();

            if (!value.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest("Authorization must be 'Bearer <key>'.");
            }

            var apiKey = value["Bearer ".Length..].Trim();

            _anytypeService.Authorize(apiKey);

            return Ok("Authorized successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                $"An unexpected error occurred while authorizing:\n{ex.Message}");
        }
    }
}
