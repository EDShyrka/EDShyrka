using System;
using System.Threading.Tasks;
using Avalonia;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using System.Diagnostics;

namespace EDShyrka;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
        _ = StartBrowserHosting(args);

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    private static Task StartBrowserHosting(string[] args)
    {
        try
        {
            //var contentRoot = System.IO.Path.Combine(AppContext.BaseDirectory, @"../../../../EDShyrka.Browser/bin/Release/net9.0-browser/publish/wwwroot");
            var contentRoot = System.IO.Path.Combine(AppContext.BaseDirectory, @"wwwroot");
            var webApplicationOptions = new WebApplicationOptions { Args = args };
            var builder = WebApplication.CreateBuilder(webApplicationOptions);
            builder.WebHost
                .UseKestrelCore()
                .ConfigureKestrel(ConfigureKestrel);

            var app = builder.Build();
            app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(contentRoot) });
            app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(contentRoot), ServeUnknownFileTypes = true });
            app.MapGet("/hello", () => $"{DateTime.Now:hh:mm:ss} - Welcome to EDShyrka !");
            //app.UseWebSockets()
            return app.StartAsync();

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting browser hosting: {ex.Message}");
            Debugger.Break();
            return Task.CompletedTask;
        }
    }

    private static void ConfigureKestrel(Microsoft.AspNetCore.Server.Kestrel.Core.KestrelServerOptions options)
    {
        options.ListenAnyIP(12080);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<UI.App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
