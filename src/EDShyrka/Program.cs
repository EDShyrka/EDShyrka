using Avalonia;
using EDShyrka.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDShyrka;

sealed class Program
{
	private const string _uniqueApplicationIdentifier = "global://EDShyrka/4fcd696e917c68e82a99f556541a350e";
	private static Mutex? _singleInstanceMutex;

	// Initialization code. Don't use any Avalonia, third-party APIs or any
	// SynchronizationContext-reliant code before AppMain is called: things aren't initialized
	// yet and stuff might break.
	[STAThread]
	public static async Task Main(string[] args)
	{
		if (IsSingleInstance() == false)
		{
			LoggingHelpers.LoggerFactory.CreateLogger("EDShyrka")
				.Log(LogLevel.Warning, "Another instance is already running, closing");
			Environment.Exit(0);
		}

		// Start the browser hosting server.
		var hostingTask = BrowserHosting.StartAsync(args, out var hostingCancellationTokenSource);

		// The hosting task is started, now we can run the Avalonia application.
		BuildAvaloniaApp()
			.StartWithClassicDesktopLifetime(args);

		// stop the hosting task gracefully when the application exits.
		hostingCancellationTokenSource.Cancel();
		await hostingTask;
	}

	// Avalonia configuration, don't remove; also used by visual designer.
	public static AppBuilder BuildAvaloniaApp()
		=> AppBuilder.Configure<UI.App>()
			.UsePlatformDetect()
			.WithInterFont()
			.LogToTrace();

	/// <summary>
	/// Check for multiple instances
	/// </summary>
	/// <returns>True if the program can run, false if another instance is already running.</returns>
	public static bool IsSingleInstance()
	{
		try
		{
			// Try to open existing mutex.
			Mutex.OpenExisting(_uniqueApplicationIdentifier);
		}
		catch
		{
			// If exception occurred, there is no such mutex so create it.
			_singleInstanceMutex = new Mutex(true, _uniqueApplicationIdentifier);

			// Only one instance.
			return true;
		}

		// More than one instance.
		return false;
	}
}
