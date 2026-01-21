using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace EDShyrka.UI.CustomControls;

/// <summary>
/// A control that displays its items one at a time and provide circular navigation.
/// Changing the current item trigger a sliding transition effect.
/// </summary>
[TemplatePart("PART_ContentControl", typeof(TransitioningContentControl))]
public class CircularPageViewer : ItemsControl
{
	#region fields
	private TransitioningContentControl? _contentControl;
	private InnerPageSlide _innerPageTransition = new(TimeSpan.FromMilliseconds(300));
	#endregion fields

	#region CurrentItem dependency property
	/// <summary>
	/// Identifies the CurrentItem dependency property.
	/// </summary>
	public static readonly StyledProperty<object?> CurrentItemProperty =
		AvaloniaProperty.Register<CircularPageViewer, object?>(nameof(CurrentItem));

	/// <summary>
	/// Gets or sets the current item displayed by the control.
	/// </summary>
	public object? CurrentItem
	{
		get => GetValue(CurrentItemProperty);
		set => SetValue(CurrentItemProperty, value);
	}
	#endregion CurrentItem dependency property

	#region IsMouseWheelEnabled dependency property
	/// <summary>
	/// Identifies the IsMouseWheelEnabled dependency property.
	/// </summary>
	public static readonly StyledProperty<bool> IsMouseWheelEnabledProperty =
		AvaloniaProperty.Register<CircularPageViewer, bool>(nameof(IsMouseWheelEnabled), defaultValue: false);

	/// <summary>
	/// Gets or sets the value used to enable the mouse wheel capture for switching the views.
	/// </summary>
	public bool IsMouseWheelEnabled
	{
		get => GetValue(IsMouseWheelEnabledProperty);
		set => SetValue(IsMouseWheelEnabledProperty, value);
	}
	#endregion IsMouseWheelEnabled dependency property!

	#region methods
	/// <summary>
	/// Change the current item with the next one.
	/// </summary>
	public void Next()
	{
		if (Items.Count == 0)
			return;

		var index = Items.IndexOf(CurrentItem) + 1;
		if (index >= Items.Count)
			index = 0;

		_innerPageTransition.Forward = true;
		SetCurrentValue(CurrentItemProperty, Items[index]);
	}

	/// <summary>
	/// Change the current item with the previous one.
	/// </summary>
	public void Previous()
	{
		if (Items.Count == 0)
			return;

		var index = Items.IndexOf(CurrentItem);
		if (index <= 0)
			index = Items.Count;

		_innerPageTransition.Forward = false;
		SetCurrentValue(CurrentItemProperty, Items[index - 1]);
	}

	protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
	{
		base.OnApplyTemplate(e);
		_contentControl = e.NameScope.Find<TransitioningContentControl>("PART_ContentControl");

		if (_contentControl != null)
		{
			_contentControl.PageTransition = _innerPageTransition;
		}

		if (CurrentItem == null && Items.Count > 0)
		{
			SetCurrentValue(CurrentItemProperty, Items[0]);
		}
	}

	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		base.OnPropertyChanged(change);
		if (change.Property == ItemsSourceProperty)
		{
			if (CurrentItem == null && Items.Count > 0)
			{
				SetCurrentValue(CurrentItemProperty, Items.Cast<object>().FirstOrDefault());
			}
		}
	}

	protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
	{
		if (IsMouseWheelEnabled == false)
			return;

		// Scroll down -> Next
		if (e.Delta.Y < 0)
		{
			Next();
		}
		else // Scroll up -> Previous
		{
			Previous();
		}
		e.Handled = true;
	}
	#endregion methods

	#region InnerPageSlide
	private class InnerPageSlide : IPageTransition
	{
		private readonly PageSlide _innerSlide;

		public InnerPageSlide(TimeSpan duration)
		{
			_innerSlide = new PageSlide(duration, PageSlide.SlideAxis.Horizontal);
		}

		public bool Forward { get; set; }

		public Task Start(Visual? from, Visual? to, bool forward, CancellationToken cancellationToken)
		{
			return _innerSlide.Start(from, to, Forward, cancellationToken);
		}
	}
	#endregion InnerPageSlide
}
