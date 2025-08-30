using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace EDShyrka.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly ObservableCollection<string> _messages = [];

	public MainViewModel()
	{
		Greeting = AppHelpers.IsRunningAsWebApp
			? "EDShyrka Browser"
			: "EDShyrka Desktop";
	}

	public string Greeting { get; }

	[ObservableProperty]
	private string _message = "Hello, World!";

	public IEnumerable<string> Messages { get => _messages; }

	[RelayCommand]
	private void OpenInBrowser()
	{
		var uriBuilder = new UriBuilder("http", "localhost", 12080);
		Process.Start(new ProcessStartInfo { FileName = uriBuilder.Uri.ToString(), UseShellExecute = true });
	}

	[RelayCommand]
	private void SendMessage()
	{

	}
}
