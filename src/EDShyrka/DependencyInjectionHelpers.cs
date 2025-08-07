using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using EDShyrka.Logging;

namespace EDShyrka
{
    /// <summary>
    /// Extensions methods to ease dependency injection.
    /// </summary>
    public static class DependencyInjectionHelpers
    {
        /// <summary>
        /// Set up the logging for the application.
        /// </summary>
        /// <param name="hostApplicationBuilder">A <see cref="IHostApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IHostApplicationBuilder"/> instance to allow chaining calls.</returns>
        public static IHostApplicationBuilder ConfigureLogging(this IHostApplicationBuilder hostApplicationBuilder)
        {
            hostApplicationBuilder.Logging.ConfigureLogging();
            return hostApplicationBuilder;
        }

        /// <summary>
        /// Set the application settings file.
        /// </summary>
        /// <param name="hostApplicationBuilder">A <see cref="IHostApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IHostApplicationBuilder"/> instance to allow chaining calls.</returns>
        public static IHostApplicationBuilder ConfigureAppSettings(this IHostApplicationBuilder hostApplicationBuilder)
        {
            var configurationBuilder = hostApplicationBuilder.Configuration;
            configurationBuilder.Sources.Clear();
            configurationBuilder.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            return hostApplicationBuilder;
        }

        /// <summary>
        /// Gets a configuration sub-section with the specified key then registers a configuration instance which TOptions will bind against.
        /// </summary>
        /// <param name="hostApplicationBuilder">The <see cref="IHostApplicationBuilder"/> to add the configuration to.</param>
        /// <param name="sectionKey">The key of the configuration section.</param>
        /// <param name="config">The configuration being bound.</param>
        /// <returns>The original <see cref="IServiceCollection"/> instance so that additional calls can be chained.</returns>
        public static IServiceCollection ConfigureSection<TOptions>(this IHostApplicationBuilder hostApplicationBuilder, string sectionKey, IConfiguration config)
            where TOptions : class
        {
            var section = config.GetSection(sectionKey);
            return hostApplicationBuilder.Services.Configure<TOptions>(section);
        }
    }
}
