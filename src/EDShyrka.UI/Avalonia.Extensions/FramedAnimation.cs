using Avalonia;
using Avalonia.Collections;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;

namespace EDShyrka.UI.Avalonia.Extensions;

/// <summary>
/// Represents an animation composed of a sequence of frames, where each frame defines updates to properties of target objects.
/// </summary>
public class FramedAnimation : AvaloniaObject
{
	#region fields
	private readonly DispatcherTimer _timer;
	private int _iteration;
	#endregion fields

	#region ctor
	/// <summary>
	/// Initializes a new instance of the <see cref="FramedAnimation"/> class.
	/// </summary>
	public FramedAnimation()
	{
		_timer = new DispatcherTimer(TimeSpan.Zero, DispatcherPriority.Render, DispatcherTimerCallback);
	}
	#endregion ctor

	#region properties
	/// <summary>
	/// Defines the interval between frames in the animation.
	/// </summary>
	public static readonly StyledProperty<TimeSpan> IntervalProperty =
		AvaloniaProperty.Register<FramedAnimation, TimeSpan>(nameof(Interval), TimeSpan.FromMilliseconds(100));

	public TimeSpan Interval
	{
		get => GetValue(IntervalProperty);
		set => SetValue(IntervalProperty, value);
	}

	public static readonly StyledProperty<AvaloniaList<Frame>> FramesProperty =
		AvaloniaProperty.Register<FramedAnimation, AvaloniaList<Frame>>(nameof(Steps), []);

	[Content]
	public AvaloniaList<Frame> Steps
	{
		get => GetValue(FramesProperty);
		set => SetValue(FramesProperty, value);
	}
	#endregion properties

	#region methods
	/// <summary>
	/// Starts the animation.
	/// </summary>
	public void StartAnimation()
	{
		_iteration = 0;
		_timer.Interval = Interval;
		_timer.Start();
	}

	/// <summary>
	/// Stops the currently running animation.
	/// </summary>
	public void StopAnimation()
	{
		_timer.Stop();
	}

	/// <summary>
	/// Handles the timer callback to apply updates to the target objects based on the current step in the sequence.
	/// </summary>
	/// <param name="sender">The source of the timer event.</param>
	/// <param name="e">The event data associated with the timer event.</param>
	private void DispatcherTimerCallback(object? sender, EventArgs e)
	{
		if (Steps.Count == 0) return;

		var step = Steps[_iteration];
		foreach (var update in step.Updates)
		{
			var target = update.Target;
			if (target is AvaloniaObject avaloniaObject
				&& update.Property is AvaloniaProperty avaloniaProperty)
			{
				var convertedValue = ConvertForTargetType(avaloniaProperty.PropertyType, update.Value);
				avaloniaObject.SetValue(avaloniaProperty, convertedValue);
			}
			else if (update.Property is string propertyPath)
			{
				var propertyInfo = target.GetType().GetProperty(propertyPath, BindingFlags.Public | BindingFlags.Instance)!;
				var convertedValue = ConvertForTargetType(propertyInfo.PropertyType, update.Value);
				propertyInfo.SetValue(target, convertedValue);
			}
		}

		_iteration = (_iteration + 1) % Steps.Count;
	}

	/// <summary>
	/// Converts the specified value to the given target type.
	/// </summary>
	/// <param name="targetType">The type to which the value should be converted.</param>
	/// <param name="value">The value to convert.</param>
	/// <returns>The converted value as an instance of the specified <paramref name="targetType"/>,
	/// or the original value if conversion is not possible.</returns>
	private static object? ConvertForTargetType(Type targetType, object? value)
	{
		if (value is null)
			return null;

		if (targetType.IsInstanceOfType(value))
			return value;

		if (value is string s)
		{
			var parseMethod = targetType.GetMethod("Parse", BindingFlags.Public | BindingFlags.Static, new[] { typeof(string) });
			if (parseMethod is not null)
			{
				return parseMethod.Invoke(null, new object[] { s });
			}

			var converter = TypeDescriptor.GetConverter(targetType);
			if (converter is not null && converter.CanConvertFrom(typeof(string)))
			{
				return converter.ConvertFromInvariantString(s);
			}
		}

		try
		{
			return Convert.ChangeType(value, targetType, CultureInfo.InvariantCulture);
		}
		catch
		{
			return value;
		}
	}
	#endregion methods
}
