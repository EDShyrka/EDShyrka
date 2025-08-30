using Avalonia;
using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows;

namespace EDShyrka.UI
{
    /// <summary>
    /// Helpers for global application management.
    /// </summary>
    public static class AppHelpers
    {
        private static readonly Lazy<string> _versionProvider = new(VersionProviderFactory);

        private static string VersionProviderFactory()
        {
            if (IsDesignMode)
                return "EDShyrka.UI - design mode v00.00.00.00000";

            var assembly = Assembly.GetExecutingAssembly();
            var info = FileVersionInfo.GetVersionInfo(assembly.Location);
            return $"{info.ProductName} - v{info.FileVersion}";
        }

        /// <summary>
        /// Application's version information to display.
        /// </summary>
        public static string VersionInfo { get => _versionProvider.Value; }

        /// <summary>
        /// Returns the display name of the assembly that contains the code that is currently executing.
        /// </summary>
        public static string ExecutingAssemblyFullName { get => Assembly.GetExecutingAssembly().FullName; }

        /// <summary>
        /// True if the WPF application is in design mode, false otherwise.
        /// </summary>
        public static bool IsDesignMode { get => Application.Current is App == false; }

		/// <summary>
		/// True when the application is running on the host as a desktop application, false otherwise.
		/// </summary>
		public static bool IsRunningAsDesktopApp { get => IsRunningAsWebApp == false; }

		/// <summary>
		/// True when the application is running in a browser, false otherwise.
		/// </summary>
		public static bool IsRunningAsWebApp { get => OperatingSystem.IsBrowser(); }
    }
}
