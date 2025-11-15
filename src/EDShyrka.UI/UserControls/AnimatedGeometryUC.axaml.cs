using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System;

namespace UserControls;

public partial class AnimatedGeometryUC : UserControl
{
	private DispatcherTimer _timer;
	private int _iteration = 0;
	private Color[] _colors01 = { Colors.Red, Colors.Green, Colors.Blue };
	private Color[] _colors02 = { Colors.Blue, Colors.Red, Colors.Green };
	private Color[] _colors03 = { Colors.Green, Colors.Blue, Colors.Red };

	public AnimatedGeometryUC()
	{
		InitializeComponent();
	}

	protected override void OnLoaded(RoutedEventArgs e)
	{
		base.OnLoaded(e);
		StartAnimation();
	}

	protected override void OnUnloaded(RoutedEventArgs e)
	{
		base.OnUnloaded(e);
		_timer.Stop();
	}

	private void StartAnimation()
	{
		var timerInterval = TimeSpan.FromMilliseconds(100);
		_timer = new DispatcherTimer(timerInterval, DispatcherPriority.Render, ProcessAnimation);
		_timer.Start();
	}

	private void ProcessAnimation(object? sender, EventArgs e)
	{
		GetResource<SolidColorBrush>("Brush01").Color = _colors01[_iteration];
		GetResource<SolidColorBrush>("Brush02").Color = _colors02[_iteration];
		GetResource<SolidColorBrush>("Brush03").Color = _colors03[_iteration];

		_iteration = (_iteration + 1) % 3;
	}

	private T GetResource<T>(string name)
	{
		if (Resources.TryGetValue(name, out var resource) == false)
		{
			throw new ArgumentException($"Resource '{name}' not found.", nameof(name));
		}
		if (resource is not T)
		{
			throw new InvalidCastException($"Resource '{name}' is not of type '{typeof(T).FullName}'.");
		}
		return (T)resource!;
	}
}
