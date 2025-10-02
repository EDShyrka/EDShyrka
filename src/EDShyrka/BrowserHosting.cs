using EDShyrka.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDShyrka
{
    internal class BrowserHosting
    {
		public ServerSettings ServerSettings { get; } = new();

		public Task StartAsync(string[] args, out CancellationTokenSource cancellationTokenSource)
        {
            // Set the content root to the wwwroot directory where static files are served from.
            var contentRoot = System.IO.Path.Combine(AppContext.BaseDirectory, @"wwwroot");
            var webApplicationOptions = new WebApplicationOptions { Args = args };
            var builder = WebApplication.CreateBuilder(webApplicationOptions);

			var configurationBuilder = builder.Configuration;
			configurationBuilder.Sources.Clear();
			configurationBuilder.SetBasePath(AppContext.BaseDirectory);
			configurationBuilder.AddJsonFile("appsettings.json");
			configurationBuilder.GetSection("Server").Bind(ServerSettings);

			builder.WebHost.UseKestrelCore().ConfigureKestrel(ConfigureKestrel);
			builder.Services.AddControllers();
            builder.Services.AddSingleton<Interfaces.IClientsManager, Services.ClientsManager>();
			builder.ConfigureLogging();
            builder.Services.AddHostedService<Services.CommunicationWorker>();

            var app = builder.Build();
            app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(contentRoot) });
            app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(contentRoot), ServeUnknownFileTypes = true });
			app.UseWebSockets(new WebSocketOptions { });
			app.MapControllers();

			cancellationTokenSource = new CancellationTokenSource();
            return app.StartAsync(cancellationTokenSource.Token);
        }

		private void ConfigureKestrel(WebHostBuilderContext webHostBuilderContext, KestrelServerOptions options)
		{
			options.ListenAnyIP(ServerSettings.ListeningPort);
		}
	}
}
