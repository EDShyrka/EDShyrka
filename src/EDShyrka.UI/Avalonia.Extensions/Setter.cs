using Avalonia;

namespace EDShyrka.UI.Avalonia.Extensions;

/// <summary>
/// Represents a setter that applies a specific value to a property on a target object.
/// </summary>
public class Setter : AvaloniaObject
{
	public object? Target { get; set; }

	public object? Property { get; set; }

	public object? Value { get; set; }
}
