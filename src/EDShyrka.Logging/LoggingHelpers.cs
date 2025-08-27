using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Reflection;

namespace EDShyrka.Logging
{
	/// <summary>
	/// Helper methods for logging related operations.
	/// </summary>
	public static partial class LoggingHelpers
	{
		#region fields
		private static Lazy<AssemblyVersionInfo> _applicationVersion = new(ApplicationVersionFactory);
		#endregion fields

		#region properties
		/// <summary>
		/// Retrieve application version information.
		/// </summary>
		public static AssemblyVersionInfo ApplicationVersion => _applicationVersion.Value;
		#endregion properties

		#region methods
		/// <summary>
		/// Output <see cref="AssemblyVersionInfo"/> to <see cref="ILogger"/> instance.
		/// </summary>
		/// <param name="logger">A <see cref="ILogger"/> instance.</param>
		public static void LogVersionInfo(this ILogger logger, AssemblyVersionInfo versionInfo)
		{
			logger.Log(LogLevel.Information, "{AssemblyName}", versionInfo.AssemblyName);
			logger.Log(LogLevel.Information, "{ProductName} - {Version}", versionInfo.ProductName, versionInfo.Version);
		}

		public static AssemblyVersionInfo GetAssemblyVersion(Assembly assembly)
		{
			var info = FileVersionInfo.GetVersionInfo(assembly.Location);
			return new AssemblyVersionInfo(assembly.FullName!, info.ProductName!, $"v{info.FileVersion}");
		}

		/// <summary>
		/// Creates an <see cref="AssemblyVersionInfo"/> instance representing the version information of the application's entry assembly.
		/// </summary>
		/// <returns>An <see cref="AssemblyVersionInfo"/> containing the version details of the entry assembly.</returns>
		private static AssemblyVersionInfo ApplicationVersionFactory()
		{
			var assembly = Assembly.GetEntryAssembly();
			return GetAssemblyVersion(assembly!);
		}
		#endregion methods
	}

	/// <summary>
	/// Represents version information for an assembly, including its name, product name, version, and configuration.
	/// </summary>
	/// <param name="AssemblyName">The name of the assembly.</param>
	/// <param name="ProductName">The name of the product associated with the assembly.</param>
	/// <param name="Version">The version of the assembly.</param>
	public record AssemblyVersionInfo(string AssemblyName, string ProductName, string Version);
}
