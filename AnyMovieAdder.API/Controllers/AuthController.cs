using AnyMovieAdder.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AnyMovieAdder.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly AnytypeService _anytypeService;
    private readonly ApiKeyStorageService _apiKeyStorage;

    public AuthController(AnytypeService anytypeService, ApiKeyStorageService apiKeyStorage)
    {
        _anytypeService = anytypeService ?? throw new ArgumentNullException(nameof(anytypeService));
        _apiKeyStorage = apiKeyStorage ?? throw new ArgumentNullException(nameof(apiKeyStorage));
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

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return BadRequest("API key is missing.");
            }

            _apiKeyStorage.Save(apiKey);
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

