using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Metadata;
using Avalonia.Threading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace UserControls;

public class Frame
{
	[Content]
	public AvaloniaList<Setter> Updates { get; } = [];
}

public class Setter
{
	public string? ResourceName { get; set; }
	public string? PropertyPath { get; set; }
	public object? Value { get; set; }
}

public static class FrameAnimator
{
	public static readonly AttachedProperty<FramedAnimation> FramedAnimationProperty =
			AvaloniaProperty.RegisterAttached<Control, FramedAnimation>(
				"FramedAnimation", typeof(FrameAnimator));

	public static void SetFramedAnimation(AvaloniaObject element, FramedAnimation value) =>
		element.SetValue(FramedAnimationProperty, value);

	public static FramedAnimation GetFramedAnimation(AvaloniaObject element) =>
		element.GetValue(FramedAnimationProperty);

	static FrameAnimator()
	{
		FramedAnimationProperty.Changed.AddClassHandler<Control>((control, args) =>
		{
			var framedAnimation = GetFramedAnimation(control);
			framedAnimation?.StartAnimation(control);
		}); 
	}
}

public class FramedAnimation : AvaloniaObject
{
	private readonly DispatcherTimer _timer;
	private Control _control;
	private int _iteration;

	public FramedAnimation()
	{
		var defaultInterval = TimeSpan.FromMilliseconds(100);
		_timer = new DispatcherTimer(defaultInterval, DispatcherPriority.Render, DispatcherTimerCallback);
	}

	public static readonly StyledProperty<AvaloniaList<Frame>> FramesProperty =
		AvaloniaProperty.Register<FramedAnimation, AvaloniaList<Frame>>(nameof(Steps), []);

	[Content]
	public AvaloniaList<Frame> Steps
	{
		get => GetValue(FramesProperty);
		set => SetValue(FramesProperty, value);
	}

	public static readonly StyledProperty<TimeSpan> IntervalProperty =
		AvaloniaProperty.Register<FramedAnimation, TimeSpan>(nameof(Interval), TimeSpan.FromMilliseconds(100));

	public TimeSpan Interval
	{
		get => GetValue(IntervalProperty);
		set => SetValue(IntervalProperty, value);
	}

	public void StartAnimation(Control control)
	{
		_control = control;
		_iteration = 0;
		_timer.Interval = Interval;
		_timer.Start();
	}

	public void StopAnimation()
	{
		_timer?.Stop();
	}

	private void DispatcherTimerCallback(object? sender, EventArgs e)
	{
		if (Steps.Count == 0) return;

		var step = Steps[_iteration];
		foreach (var update in step.Updates)
		{
			if (_control.Resources.TryGetValue(update.ResourceName, out var resource))
			{
				var propInfo = resource.GetType().GetProperty(update.PropertyPath, BindingFlags.Public | BindingFlags.Instance);
				propInfo?.SetValue(resource, update.Value);
			}
		}

		_iteration = (_iteration + 1) % Steps.Count;
	}
}
