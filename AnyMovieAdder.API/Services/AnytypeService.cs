using AnyMovieAdder.API.Models;
using Anytype.NET;
using Anytype.NET.Models.Requests;

namespace AnyMovieAdder.API.Services;

public sealed class AnytypeService
{
    private AnytypeClient? _client;

    public bool IsAuthorized => _client is not null;

    public void Authorize(string apiKey)
    {
        _client = new AnytypeClient(apiKey);
    }

    public async Task AddMovieAsync(Movie movie)
    {
        var spacesResponse = await _client.Spaces.ListAsync();
        var targetSpaceName = "Наше всё";

        var spaceId = spacesResponse.Spaces
            .FirstOrDefault(s => s.Name.Equals(targetSpaceName))
            ?.Id;

        var propertiesResponse = await _client.Properties.ListAsync(spaceId!);

        var releaseYearPropertyId = propertiesResponse.Properties
            .FirstOrDefault(p => p.Name.Equals("Год выхода"))
            ?.Id;

        var categoryPropertyId = propertiesResponse.Properties
            .FirstOrDefault(p => p.Name.Equals("Категория"))
            ?.Id;

        var categoryPrperty = await _client.Properties.GetByIdAsync(spaceId!, categoryPropertyId);
        var categoryPropertyKey = categoryPrperty.Key;
        var categoryPropertyTags = await _client.Tags.ListAsync(spaceId!, categoryPropertyId);

        var cheiPropertyId = propertiesResponse.Properties
            .FirstOrDefault(p => p.Key.Equals("chei"))
            ?.Id;

        var cheiProperty = await _client.Properties.GetByIdAsync(spaceId!, cheiPropertyId);
        var response = await _client.Tags.ListAsync(spaceId!, cheiPropertyId);

        // Find tag by name.
        var valeraTag = response.Tags
            .FirstOrDefault(t => t.Name.Equals("Валера"));

        var movieTag = categoryPropertyTags.Tags
            .FirstOrDefault(t => t.Name.Equals("Фильм"));

        var animeTag = categoryPropertyTags.Tags
            .FirstOrDefault(t => t.Name.Equals("Аниме"));

        var seriesTag = categoryPropertyTags.Tags
            .FirstOrDefault(t => t.Name.Equals("Сериал"));

        var animationTag = categoryPropertyTags.Tags
            .FirstOrDefault(t => t.Name.Equals("Анимация"));

        var selectedTagIds = new List<string>();

        if (movie.Categories.Contains("movie") && movieTag != null)
        {
            selectedTagIds.Add(movieTag.Id);
        }

        if (movie.Categories.Contains("anime") && animeTag != null)
        {
            selectedTagIds.Add(animeTag.Id);
        }

        if (movie.Categories.Contains("tvshow") && seriesTag != null)
        {
            selectedTagIds.Add(seriesTag.Id);
        }

        if (movie.Categories.Contains("animation") && animationTag != null)
        {
            selectedTagIds.Add(animationTag.Id);
        }

        var valeraTagId = valeraTag?.Id;

        // Get Types.
        var typesResponse = await _client!.Types.ListAsync(spaceId!);

        // Get Movie type ID.
        var movieTypeId = typesResponse.Types
            .FirstOrDefault(t => t.Name.Equals("Movie"))
            ?.Id;

        // List templates for Movie type.
        var templatesResponse = await _client!.Templates.ListAsync(spaceId!, movieTypeId!);

        var targetTemplateName = "Название";
        var templateId = templatesResponse.Templates
            .FirstOrDefault(t => t.Name.Equals(targetTemplateName))
            ?.Id;

        var createObjectRequest = new CreateObjectRequest()
        {
            Name = $"{movie.Title} created when testing API at {DateTime.Now:dd-MM-yyyy HH:mm}",
            TypeKey = movieTypeId!,
            TemplateId = templateId!,
            Properties =
            [
                new
                {
                    key = "description",
                    text = movie.Description
                },
                new
                {
                    key = "chei",
                    multi_select = new List<string> { valeraTagId! }
                },
                new
                {
                    key = "66917458b2bb3b0c70440fdb", // Год выхода
                    text = movie.ReleaseYear.ToString()
                },
                new
                {
                    key = categoryPropertyKey,
                    multi_select = selectedTagIds
                },
            ]
        };

        try
        {
            var createdObject = await _client.Objects.CreateAsync(spaceId!, createObjectRequest);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
