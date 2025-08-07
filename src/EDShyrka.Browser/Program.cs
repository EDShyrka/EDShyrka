using Avalonia;
using Avalonia.Browser;
using System.Runtime.InteropServices.JavaScript;
using System.Threading.Tasks;

internal sealed partial class Program
{
    private static async Task Main(string[] args)
    {
        await JSHost.ImportAsync("jsInterop", "/interop.js");
        await BuildAvaloniaApp()
            .WithInterFont()
            .StartBrowserAppAsync("out");
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<EDShyrka.UI.App>();
}
