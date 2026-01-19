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
    private readonly AnytypeService _anytypeService;

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

        await _anytypeService.AddMovieAsync(movie);

        return StatusCode(StatusCodes.Status201Created, movie);
    }
}
