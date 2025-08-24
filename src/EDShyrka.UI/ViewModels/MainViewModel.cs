using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace EDShyrka.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly ILogger _logger;

	private int _counter = 0;

	public MainViewModel(ILogger<MainViewModel> logger)
	{
		_logger = logger;
		if (OperatingSystem.IsBrowser() == false)
		{
			Greeting = "Welcome to EDShyrka Desktop !";
			_clickMeCommand = new RelayCommand(LaunchBrowser);
		}
		else
		{
			Greeting = $"Welcome to EDShyrka Browser ! (host is {JSInterop.getHostAddress()})";
			_clickMeCommand = new RelayCommand(IncrementCounter);
		}
		_logger.Log(LogLevel.Information, "Started");
	}

	private void LaunchBrowser()
	{
		var uriBuilder = new UriBuilder("http", "localhost", 12080);
		Process.Start(new ProcessStartInfo { FileName = uriBuilder.Uri.ToString(), UseShellExecute = true });
		ClickMeCommand = new RelayCommand(IncrementCounter);
	}

	public string Greeting { get; }

	[ObservableProperty]
	private string _labelText = "Hello, World!";


	[ObservableProperty]
	private ICommand _clickMeCommand;

	[RelayCommand]
	public void IncrementCounter()
	{
		LabelText = $"Clicked {++_counter} times !";
		_logger.Log(LogLevel.Information, "Button clicked {count} times", _counter);
	}
}
