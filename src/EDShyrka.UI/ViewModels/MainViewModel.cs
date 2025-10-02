using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EDShyrka.Shared;
using EDShyrka.UI.Models;
using EDShyrka.UI.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace EDShyrka.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly AppSettings _appSettings;
	private readonly CommunicationService _communicationService;
	private readonly ObservableCollection<string> _messages = [];

	public MainViewModel(AppSettings appSettings,CommunicationService communicationService)
	{
		_appSettings = appSettings;
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
		Dispatcher.UIThread.Post(() => _messages.Add($"received: {message}"));
	}

	public string Greeting { get; }

	[ObservableProperty]
	private string _message = "Hello, EDShyrka !";

	public IEnumerable<string> Messages { get => _messages; }

	[ObservableProperty]
	private bool _isLandingGearDeployed;

	[RelayCommand]
	private void OpenInBrowser()
	{
		Process.Start(new ProcessStartInfo { FileName = _appSettings.ServerLocation, UseShellExecute = true });
	}

	[RelayCommand]
	private async Task ToggleLandingGear(string parameter)
	{
		var buffer = System.Text.Encoding.UTF8.GetBytes(parameter);
		var connection = await _communicationService.GetConnection();
		_ = connection.SendAsync(buffer, default);
	}

	[RelayCommand]
	private async Task SendMessage()
	{
		var message = Message;
		Dispatcher.UIThread.Post(() => _messages.Add($"Sending [{message}] to [{_appSettings.ServerLocation}]"));
		var buffer = System.Text.Encoding.UTF8.GetBytes(message);
		var connection = await _communicationService.GetConnection();
		_ = connection.SendAsync(buffer, default);
	}

}
