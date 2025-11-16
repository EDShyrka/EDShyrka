using Avalonia;
using Avalonia.Controls;

namespace EDShyrka.UI.Avalonia.Extensions;

/// <summary>
/// Provides attached property and methods for managing frame-based animations on <see cref="Control"/> elements.
/// </summary>
public class FrameAnimator
{
	/// <summary>
	/// Initializes static members of the <see cref="FrameAnimator"/> class.
	/// </summary>
	static FrameAnimator()
	{
		AnimationProperty.Changed.AddClassHandler<Control>(HandleAnimationPropertyChanged);
	}

	/// <summary>
	/// The attached property that holds the <see cref="FramedAnimation"/> instance.
	/// </summary>
	public static readonly AttachedProperty<FramedAnimation> AnimationProperty =
			AvaloniaProperty.RegisterAttached<Control, FramedAnimation>("Animation", typeof(FrameAnimator));

	/// <summary>
	/// Sets the animation for the specified <see cref="AvaloniaObject"/>.
	/// </summary>
	/// <param name="element">The <see cref="AvaloniaObject"/> to which the animation will be applied.</param>
	/// <param name="value">The <see cref="FramedAnimation"/> to set on the specified object.</param>
	public static void SetAnimation(AvaloniaObject element, FramedAnimation value) =>
		element.SetValue(AnimationProperty, value);

	/// <summary>
	/// Retrieves the <see cref="FramedAnimation"/> associated with the specified <see cref="AvaloniaObject"/>.
	/// </summary>
	/// <param name="element">The <see cref="AvaloniaObject"/> from which to retrieve the animation.</param>
	/// <returns>The <see cref="FramedAnimation"/> associated with the specified <paramref name="element"/>.</returns>
	public static FramedAnimation GetAnimation(AvaloniaObject element) =>
		element.GetValue(AnimationProperty);

	/// <summary>
	/// Handle triggered when <see cref="AnimationProperty"/> value changes.
	/// Stop the old animation and start the new one.
	/// </summary>
	/// <param name="control">The control associated with the animation.</param>
	/// <param name="e">The event arguments containing the old and new property values.</param>
	private static void HandleAnimationPropertyChanged(Control control, AvaloniaPropertyChangedEventArgs e)
	{
		if (e.OldValue is FramedAnimation oldAnimation)
		{
			oldAnimation.StopAnimation();
		}
		if (e.NewValue is FramedAnimation newAnimation)
		{
			newAnimation.StartAnimation();
		}
	}
}
