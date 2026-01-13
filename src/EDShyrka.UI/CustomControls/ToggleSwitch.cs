using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data.Converters;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace EDShyrka.UI.CustomControls;

/// <summary>
/// Implement a toggle switch control with two states : 'on' and 'off'.
/// The <see cref="PseudoClassesAttribute"/> attribute link the ':checked' state to this control.
/// </summary>
[PseudoClasses(":checked")]
public class ToggleSwitch : Button
{
	#region ctor
	/// <summary>
	/// Initializes static members of the ToggleSwitch class and registers class handlers for property changes.
	/// </summary>
	static ToggleSwitch()
	{
		IsOnProperty.Changed.AddClassHandler<ToggleSwitch>((x, e) => x.OnIsOnChanged(e));
	}
	#endregion ctor

	#region IsOn dependency property
	/// <summary>
	/// Identifies the IsOn dependency property.
	/// </summary>
	public static readonly StyledProperty<bool> IsOnProperty = AvaloniaProperty.Register<ToggleSwitch, bool>(nameof(IsOn));

	/// <summary>
	/// Gets or sets a value indicating whether the control is in the 'on' state.
	/// True, state is 'on'; False, state is 'off'.
	/// </summary>
	public bool IsOn
	{
		get => GetValue(IsOnProperty);
		set => SetValue(IsOnProperty, value);
	}
	#endregion IsOn dependency property

	#region ThumbBrush dependency property
	/// <summary>
	/// Identifies the ThumbBrush dependency property.
	/// </summary>
	public static readonly StyledProperty<IBrush?> ThumbBrushProperty = AvaloniaProperty.Register<ToggleSwitch, IBrush?>(nameof(ThumbBrush), Brushes.White);

	/// <summary>
	/// Gets or sets the thumb brush used by the template.
	/// </summary>
	public IBrush? ThumbBrush { get => GetValue(ThumbBrushProperty); set => SetValue(ThumbBrushProperty, value); }
	#endregion ThumbBrush dependency property

	#region OnBorderBrush dependency property
	/// <summary>
	/// Identifies the OnBorderBrush dependency property.
	/// </summary>
	public static readonly StyledProperty<IBrush?> OnBorderBrushProperty = AvaloniaProperty.Register<ToggleSwitch, IBrush?>(nameof(OnBorderBrush));

	/// <summary>
	/// Gets or sets the border brush when the toggle switch is in the 'on' state.
	/// </summary>
	public IBrush? OnBorderBrush { get => GetValue(OnBorderBrushProperty); set => SetValue(OnBorderBrushProperty, value); }
	#endregion OnBorderBrush dependency property

	#region OffBorderBrush dependency property
	/// <summary>
	/// Identifies the OffBorderBrush dependency property.
	/// </summary>
	public static readonly StyledProperty<IBrush?> OffBorderBrushProperty = AvaloniaProperty.Register<ToggleSwitch, IBrush?>(nameof(OffBorderBrush));

	/// <summary>
	/// Gets or sets the border brush when the toggle switch is in the 'off' state.
	/// </summary>
	public IBrush? OffBorderBrush { get => GetValue(OffBorderBrushProperty); set => SetValue(OffBorderBrushProperty, value); }
	#endregion OffBorderBrush dependency property

	#region OnBackground dependency property
	/// <summary>
	/// Identifies the OnBackground dependency property.
	/// </summary>
	public static readonly StyledProperty<IBrush?> OnBackgroundProperty = AvaloniaProperty.Register<ToggleSwitch, IBrush?>(nameof(OnBackground));

	/// <summary>
	/// Gets or sets the background brush when the toggle switch is in the 'on' state.
	/// </summary>
	public IBrush? OnBackground { get => GetValue(OnBackgroundProperty); set => SetValue(OnBackgroundProperty, value); }
	#endregion OnBackground dependency property

	#region OffBackground dependency property
	/// <summary>
	/// Identifies the OffBackground dependency property.
	/// </summary>
	public static readonly StyledProperty<IBrush?> OffBackgroundProperty = AvaloniaProperty.Register<ToggleSwitch, IBrush?>(nameof(OffBackground));

	/// <summary>
	/// Gets or sets the background brush when the toggle switch is in the 'off' state.
	/// </summary>
	public IBrush? OffBackground { get => GetValue(OffBackgroundProperty); set => SetValue(OffBackgroundProperty, value); }
	#endregion OffBackground dependency property

	#region OnContent dependency property
	/// <summary>
	/// Identifies the OnContent dependency property.
	/// </summary>
	public static readonly StyledProperty<object?> OnContentProperty = AvaloniaProperty.Register<ToggleSwitch, object?>(nameof(OnContent), "ON");

	/// <summary>
	/// Gets or sets the content when the toggle switch is in the 'on' state.
	/// </summary>
	public object? OnContent { get => GetValue(OnContentProperty); set => SetValue(OnContentProperty, value); }
	#endregion OnContent dependency property

	#region OffContent dependency property
	/// <summary>
	/// Identifies the OffContent dependency property.
	/// </summary>
	public static readonly StyledProperty<object?> OffContentProperty = AvaloniaProperty.Register<ToggleSwitch, object?>(nameof(OffContent), "OFF");

	/// <summary>
	/// Gets or sets the content when the toggle switch is in the 'off' state.
	/// </summary>
	public object? OffContent { get => GetValue(OffContentProperty); set => SetValue(OffContentProperty, value); }
	#endregion OffContent dependency property

	#region OnThumbBrush dependency property
	/// <summary>
	/// Identifies the OnThumbBrush dependency property.
	/// </summary>
	public static readonly StyledProperty<IBrush?> OnThumbBrushProperty =
		AvaloniaProperty.Register<ToggleSwitch, IBrush?>(nameof(OnThumbBrush), Brushes.White);

	/// <summary>
	/// Gets or sets the thumb brush when the toggle switch is in the 'on' state.
	/// </summary>
	public IBrush? OnThumbBrush { get => GetValue(OnThumbBrushProperty); set => SetValue(OnThumbBrushProperty, value); }
	#endregion OnBackground dependency property

	#region OnBackground dependency property
	/// <summary>
	/// Identifies the OnBackground dependency property.
	/// </summary>
	public static readonly StyledProperty<IBrush?> OffThumbBrushProperty =
		AvaloniaProperty.Register<ToggleSwitch, IBrush?>(nameof(OffThumbBrush), Brushes.White);

	/// <summary>
	/// Gets or sets the thumb brush when the toggle switch is in the 'off' state.
	/// </summary>
	public IBrush? OffThumbBrush { get => GetValue(OffThumbBrushProperty); set => SetValue(OffThumbBrushProperty, value); }
	#endregion OnBackground dependency property


	protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
	{
		base.OnApplyTemplate(e);
		UpdatePseudoClasses();
	}

	private void OnIsOnChanged(AvaloniaPropertyChangedEventArgs e)
	{
		UpdatePseudoClasses();
	}

	private void UpdatePseudoClasses()
	{
		// On active ou désactive la pseudo-classe :checked selon IsOn
		PseudoClasses.Set(":checked", IsOn);
	}
}

public class ThumbOffsetConverter : IMultiValueConverter
{
	private static readonly TransformOperations _identity = TransformOperations.Identity;

	public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
	{
		// we expect 2 values : trackWidth and thumbWidth
		if (values.Count >= 2 && values[0] is double trackWidth && values[1] is double thumbWidth)
		{
			//return TransformOperations.Parse($"translate({trackWidth - thumbWidth}px,0)");
			var builder = TransformOperations.CreateBuilder(1);
			builder.AppendTranslate(trackWidth - thumbWidth, 0);
			return builder.Build();
		}
		return _identity;
	}
}
