using AnyMovieAdder.API;
using Microsoft.AspNetCore.Builder;
namespace AnyMovieAdder.Host;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        RegisterGlobalExceptionHandlers();

        try
        {
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
        }
        catch (Exception ex)
        {
            ShowError("Startup error", ex.ToString());
            return;
        }

        WebApplication? webApp = null;
        try
        {
            webApp = ApiWebApplication.Create(args);
            webApp.StartAsync().GetAwaiter().GetResult();
        }
        catch (Exception ex)
        {
            ShowError("Web host startup error", ex.ToString());
            return;
        }

        try
        {
            Application.Run(new MainForm());
        }
        catch (Exception ex)
        {
            ShowError("Main window error", ex.ToString());
        }
        finally
        {
            if (webApp is not null)
            {
                try
                {
                    webApp.StopAsync().GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    ShowError("Web host shutdown error", ex.ToString());
                }
            }
        }
    }

    private static void RegisterGlobalExceptionHandlers()
    {
        Application.ThreadException += (_, e) =>
        {
            ShowError("UI thread exception", e.Exception.ToString());
        };

        AppDomain.CurrentDomain.UnhandledException += (_, e) =>
        {
            var text = e.ExceptionObject is Exception ex
                ? ex.ToString()
                : e.ExceptionObject?.ToString() ?? "Unknown error";
            ShowError("Fatal error", text);
        };
    }

    private static void ShowError(string title, string message) =>
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
}
