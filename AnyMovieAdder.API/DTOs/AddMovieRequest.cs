namespace AnyMovieAdder.API.DTOs;

public sealed class AddMovieRequest
{
    public string? Title { get; init; }

    public string? Description { get; init; }

    public int ReleaseYear { get; init; }

    public string[]? Categories { get; init; }
}
