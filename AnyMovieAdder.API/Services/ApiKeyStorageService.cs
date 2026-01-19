namespace AnyMovieAdder.API.Services;

public sealed class ApiKeyStorageService
{
    private const string FileName = "Anytype API key.dat";

    /// <summary>
    /// Saves (or overwrites) the API key in a file.
    /// </summary>
    public void Save(string apiKey)
    {
        File.WriteAllText(FileName, apiKey.Trim());
    }

    /// <summary>
    /// Loads the API key from the file.
    /// </summary>
    public string Load()
    {
        if (!File.Exists(FileName))
        {
            throw new InvalidOperationException("API key file not found.");
        }

        return File.ReadAllText(FileName).Trim();
    }

    /// <summary>
    /// Checks whether the API key file exists.
    /// </summary>
    public bool Exists() => File.Exists(FileName);
}

