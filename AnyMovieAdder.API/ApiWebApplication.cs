using AnyMovieAdder.API.Services;

namespace AnyMovieAdder.API;

public static class ApiWebApplication
{
    public const string HttpsUrl = "https://127.0.0.1:7185";
    public const string HttpUrl = "http://127.0.0.1:5025";

    public static WebApplication Create(string[] args)
    {
        var apiAssembly = typeof(ApiWebApplication).Assembly;
        var assemblyDir = Path.GetDirectoryName(apiAssembly.Location);
        // Single-file host: bundled assemblies may have empty Location; use exe directory (contains wwwroot).
        var contentRoot = !string.IsNullOrEmpty(assemblyDir)
            ? assemblyDir
            : AppContext.BaseDirectory;

        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            ContentRootPath = contentRoot,
            ApplicationName = apiAssembly.GetName().Name,
        });

        builder.WebHost.UseUrls(HttpsUrl, HttpUrl);

        builder.Services.AddControllers();
        builder.Services.AddOpenApi();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod());
        });

        builder.Services.AddSingleton<AnytypeService>();
        builder.Services.AddSingleton<ApiKeyStorageService>();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var anytypeService = scope.ServiceProvider.GetRequiredService<AnytypeService>();
            var apiKeyStorage = scope.ServiceProvider.GetRequiredService<ApiKeyStorageService>();

            if (!anytypeService.IsAuthorized && apiKeyStorage.Exists())
            {
                var key = apiKeyStorage.Load();
                anytypeService.Authorize(key);
            }
        }

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        var defaultFilesOptions = new DefaultFilesOptions();
        defaultFilesOptions.DefaultFileNames.Clear();
        defaultFilesOptions.DefaultFileNames.Add("index.html");
        app.UseDefaultFiles(defaultFilesOptions);
        app.UseStaticFiles();

        app.UseAuthorization();

        app.UseCors();

        app.MapControllers();

        return app;
    }

    public static void RunStandalone(string[] args) => Create(args).Run();
}
