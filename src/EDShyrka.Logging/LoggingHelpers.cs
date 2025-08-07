using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace EDShyrka.Logging
{
    /// <summary>
    /// Helper methods for logging related operations.
    /// </summary>
    public static partial class LoggingHelpers
    {
        /// <summary>
        /// Output application version information to <see cref="ILogger"/> instance.
        /// <typeparamref name="TSource"/> is used to locate the assembly where version informatio nmust be extracted.
        /// </summary>
        /// <param name="logger">A <see cref="ILogger"/> instance.</param>
        public static void LogApplicationVersion<TSource>(this ILogger logger)
        {
            var assembly = typeof(TSource).Assembly;
            logger.Log(LogLevel.Information, "{AssemblyName}", assembly.FullName);
#if DEBUG
            var configuration = "Debug";
#else
            var configuration = "Release";
#endif //DEBUG
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = $"v{info.FileVersion}";
            logger.Log(LogLevel.Information, "{ProductName} - {Version} {Configuration}", info.ProductName, version, configuration);
        }

        /// <summary>
        /// Output application version information to <see cref="ILogger"/> instance.
        /// <typeparamref name="TSource"/> is used to locate the assembly where version informatio nmust be extracted.
        /// </summary>
        /// <param name="logger">A <see cref="ILogger"/> instance.</param>
        public static void LogApplicationVersion<TSource>(this ILogger<TSource> logger)
        {
            LogApplicationVersion<TSource>((ILogger)logger);
        }
    }
}
