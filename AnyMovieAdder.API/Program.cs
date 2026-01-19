using AnyMovieAdder.API.Services;

namespace AnyMovieAdder.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
                policy.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod()
            );
        });

        builder.Services.AddSingleton<AnytypeService>();
        builder.Services.AddSingleton<ApiKeyStorageService>();

        var app = builder.Build();

        using var scope = app.Services.CreateScope();        
        var anytypeService = scope.ServiceProvider.GetRequiredService<AnytypeService>();
        var apiKeyStorage = scope.ServiceProvider.GetRequiredService<ApiKeyStorageService>();

        if (!anytypeService.IsAuthorized && apiKeyStorage.Exists())
        {
            var key = apiKeyStorage.Load();
            anytypeService.Authorize(key);
        }        

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors();

        app.MapControllers();

        app.Run();
    }
}
