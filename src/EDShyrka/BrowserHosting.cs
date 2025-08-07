using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDShyrka
{
    internal static class BrowserHosting
    {
        public static Task StartAsync(string[] args, out CancellationTokenSource cancellationTokenSource)
        {
            // Set the content root to the wwwroot directory where static files are served from.
            var contentRoot = System.IO.Path.Combine(AppContext.BaseDirectory, @"wwwroot");
            var webApplicationOptions = new WebApplicationOptions { Args = args };
            var builder = WebApplication.CreateBuilder(webApplicationOptions);
            builder.ConfigureAppConfigurationDelegate();
            builder.WebHost.UseKestrelCore().ConfigureKestrel(ConfigureKestrel);

            var app = builder.Build();
            app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(contentRoot) });
            app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(contentRoot), ServeUnknownFileTypes = true });

            app.MapGet("/hello", () => $"{DateTime.Now:hh:mm:ss} - Welcome to EDShyrka !");
            app.UseWebSockets(new WebSocketOptions { });

            cancellationTokenSource = new CancellationTokenSource();
            return app.StartAsync(cancellationTokenSource.Token);
        }

        private static void ConfigureAppConfigurationDelegate(this IHostApplicationBuilder builder)
        {
            var configurationBuilder = builder.Configuration;
            configurationBuilder.Sources.Clear();
            configurationBuilder.AddJsonFile("appsettings.json");
        }

        private static void ConfigureKestrel(WebHostBuilderContext webHostBuilderContext, KestrelServerOptions options)
        {
            var settings = new ServerSettings();
            webHostBuilderContext.Configuration.GetSection("Server").Bind(settings);
            options.ListenAnyIP(settings.ListeningPort);
        }
    }

    public class ServerSettings
    {
        public int ListeningPort { get; set; } = 12080;
    }
}
