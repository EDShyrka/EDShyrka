using Avalonia;
using System;
using System.Threading.Tasks;

namespace EDShyrka;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static async Task Main(string[] args)
    {
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
}
