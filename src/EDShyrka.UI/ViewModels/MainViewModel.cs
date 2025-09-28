using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EDShyrka.Shared;
using EDShyrka.UI.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EDShyrka.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly CommunicationService _communicationService;
	private readonly ObservableCollection<string> _messages = [];

	public MainViewModel(CommunicationService communicationService)
	{
		_communicationService = communicationService;
		Greeting = AppHelpers.IsRunningAsWebApp
			? "EDShyrka Browser"
			: "EDShyrka Desktop";
		_communicationService.GetConnection().ContinueWith(t => RegisterConnection(t.Result));
	}

	private void RegisterConnection(WebSocketWrapper wrapper)
	{
		wrapper.RequestReceived += OnRequestReceived;

	}

	private void OnRequestReceived(object sender, WebSocketWrapper.RequestReceivedEventArgs args)
	{
		var message = System.Text.Encoding.UTF8.GetString(args.Data);
		Dispatcher.UIThread.Post(() => _messages.Add(message));
	}

	public string Greeting { get; }

	[ObservableProperty]
	private string _message = "Hello, World!";

	public IEnumerable<string> Messages { get => _messages; }

	[ObservableProperty]
	private bool _isLandingGearDeployed;

	[RelayCommand]
	private void OpenInBrowser()
	{
		var uriBuilder = new UriBuilder("http", "localhost", 12080);
		Process.Start(new ProcessStartInfo { FileName = uriBuilder.Uri.ToString(), UseShellExecute = true });
	}

	[RelayCommand]
	private async void ToggleLandingGear(string parameter)
	{
		var buffer = System.Text.Encoding.UTF8.GetBytes(parameter);
		var connection = await _communicationService.GetConnection();
		connection.SendAsync(buffer, default);
	}

	[RelayCommand]
	private async Task SendMessage()
	{
		var buffer = System.Text.Encoding.UTF8.GetBytes(Message);
		var connection = await _communicationService.GetConnection();
		connection.SendAsync(buffer, default);
	}

}
