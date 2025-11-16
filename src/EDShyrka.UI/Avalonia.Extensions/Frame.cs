using Avalonia;
using Avalonia.Collections;
using Avalonia.Metadata;

namespace EDShyrka.UI.Avalonia.Extensions;

/// <summary>
/// Represents a frame that contains a collection of updates to be applied.
/// </summary>
public class Frame : AvaloniaObject
{
	/// <summary>
	/// Gets the collection of updates that define property changes to be applied.
	/// </summary>
	[Content]
	public AvaloniaList<Setter> Updates { get; } = [];
}
