using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;

namespace EDShyrka.Logging
{
    public static partial class LoggingHelpers
    {
        #region fields
        /// <summary>
        /// NLog configuration filename.
        /// </summary>
        public const string NLogConfigFilename = "NLog.config";

        private static readonly Lazy<NLogLoggerProvider> _loggerProvider;
        private static readonly Lazy<NLogLoggerFactory> _loggerFactory;
        #endregion fields

        #region ctor
        static LoggingHelpers()
        {
            _loggerProvider = new Lazy<NLogLoggerProvider>(CreateProvider);
            _loggerFactory = new Lazy<NLogLoggerFactory>(CreateFactory);
        }
        #endregion ctor

        #region properties
        /// <summary>
        /// <see cref="ILoggerProvider"/> instance.
        /// </summary>
        public static ILoggerProvider LoggerProvider { get => _loggerProvider.Value; }

        /// <summary>
        /// <see cref="ILoggerFactory"/> instance.
        /// </summary>
        public static ILoggerFactory LoggerFactory { get => _loggerFactory.Value; }
        #endregion properties

        #region methods
        /// <summary>
        /// Method used by <see cref="Microsoft.Extensions.Hosting.IHostBuilder"/> to setup NLog logger.
        /// </summary>
        /// <param name="logging"></param>
        public static void ConfigureLogging(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.ClearProviders().AddProvider(LoggerProvider);
        }

        private static NLogLoggerProvider CreateProvider()
        {
            var provider = new NLogLoggerProvider();
            provider.LogFactory.Setup().LoadConfigurationFromFile(NLogConfigFilename);
            return provider;
        }

        private static NLogLoggerFactory CreateFactory()
        {
            return new NLogLoggerFactory(_loggerProvider.Value);
        }
        #endregion methods
    }
}
