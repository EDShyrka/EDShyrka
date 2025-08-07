using System.Runtime.InteropServices.JavaScript;

public static partial class JSInterop
{
    [JSImport("getQueryString", "jsInterop")]
    internal static partial string getQueryString();

    [JSImport("getQueryStringParameter", "jsInterop")]
    internal static partial string getQueryStringParameter(string name);

    [JSImport("getQueryStringParameters", "jsInterop")]
    internal static partial string getQueryStringParameters();

    [JSImport("getHostAddress", "jsInterop")]
    internal static partial string getHostAddress();
}
