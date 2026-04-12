using AnyMovieAdder.API;
using Microsoft.Web.WebView2.WinForms;

namespace AnyMovieAdder.Host;

public sealed class MainForm : Form
{
    private readonly WebView2 _webView = new() { Dock = DockStyle.Fill };

    public MainForm()
    {
        Text = "AnyMovie Adder";
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Width = 1200;
        Height = 900;
        StartPosition = FormStartPosition.CenterScreen;

        // Inset WebView2 from the client edge so content is not clipped by the window chrome;
        // extra top padding clears the caption/title bar overlap feel on some DPI setups.
        var host = new Panel
        {
            Dock = DockStyle.Fill
        };
        host.Controls.Add(_webView);
        Controls.Add(host);

        Load += async (_, _) => await LoadWebViewAsync();
    }

    private async Task LoadWebViewAsync()
    {
        try
        {
            await _webView.EnsureCoreWebView2Async();
            _webView.CoreWebView2.Settings.AreDefaultContextMenusEnabled = false;
            _webView.CoreWebView2.Navigate($"{ApiWebApplication.HttpsUrl}/");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
