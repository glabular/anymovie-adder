using AnyMovieAdder.API;

namespace AnyMovieAdder.Host;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        var webApp = ApiWebApplication.Create(args);
        webApp.StartAsync().GetAwaiter().GetResult();
        try
        {
            Application.Run(new MainForm());
        }
        finally
        {
            webApp.StopAsync().GetAwaiter().GetResult();
        }
    }
}
