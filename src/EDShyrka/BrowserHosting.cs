using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.FileProviders;
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
            builder.WebHost.UseKestrelCore().ConfigureKestrel(ConfigureKestrel);

            var app = builder.Build();
            app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(contentRoot) });
            app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(contentRoot), ServeUnknownFileTypes = true });

            app.MapGet("/hello", () => $"{DateTime.Now:hh:mm:ss} - Welcome to EDShyrka !");
            app.UseWebSockets(new WebSocketOptions { });

            cancellationTokenSource = new CancellationTokenSource();
            return app.StartAsync(cancellationTokenSource.Token);
        }

        private static void ConfigureKestrel(KestrelServerOptions options)
        {
            options.ListenAnyIP(12080);
        }
    }
}
