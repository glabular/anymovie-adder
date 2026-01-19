namespace AnyMovieAdder.API.Services;

public sealed class ApiKeyStorageService
{
    private const string FolderName = "AnyMovieAdder";
    private const string FileName = "Anytype API key.dat";
    private readonly string _filePath;

    public ApiKeyStorageService()
    {
        var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var directoryPath = Path.Combine(appDataPath, FolderName);

        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        _filePath = Path.Combine(directoryPath, FileName);
    }

    /// <summary>
    /// Saves (or overwrites) the API key in a file.
    /// </summary>
    public void Save(string apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new ArgumentException("API key cannot be empty.");
        }

        File.WriteAllText(_filePath, apiKey.Trim());
    }

    /// <summary>
    /// Loads the API key from the file.
    /// </summary>
    public string Load()
    {
        if (!Exists())
        {
            throw new FileNotFoundException("API key file not found.", _filePath);
        }

        return File.ReadAllText(_filePath).Trim();
    }

    /// <summary>
    /// Checks whether the API key file exists.
    /// </summary>
    public bool Exists() => File.Exists(_filePath);
}

