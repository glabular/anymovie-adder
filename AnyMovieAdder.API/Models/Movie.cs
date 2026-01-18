namespace AnyMovieAdder.API.Models;

public sealed class Movie
{
    public required string Title { get; set; }

    public string? Description { get; set; }

    public int ReleaseYear { get; set; }

    public string[] Categories { get; set; } = [];
}
