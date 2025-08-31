using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace EDShyrka.UI.ViewModels;

public partial class MainViewModel : ViewModelBase
{
	private readonly ObservableCollection<string> _messages = [];
	private ClientWebSocket _clientWebSocket;
	private Task _receiveTask;

	public MainViewModel()
	{
		Greeting = AppHelpers.IsRunningAsWebApp
			? "EDShyrka Browser"
			: "EDShyrka Desktop";
		_clientWebSocket = new ClientWebSocket();
		_receiveTask = Task.Factory.StartNew(Receive, TaskCreationOptions.LongRunning);
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
	private async Task SendMessage()
	{
		var buffer = System.Text.Encoding.UTF8.GetBytes(Message);
		await _clientWebSocket.SendAsync(new Memory<byte>(buffer), WebSocketMessageType.Binary, true, default);
	}

	private async Task Receive()
	{
		Console.WriteLine("Receive started");
		await _clientWebSocket.ConnectAsync(new Uri("ws://localhost:12080/ws"), default);
		Console.WriteLine("Socket connected");

		var buffer = new byte[1024];
		while (_clientWebSocket.State == WebSocketState.Open)
		{
			Console.WriteLine("Waiting for communication");
			var result = await _clientWebSocket.ReceiveAsync(new Memory<byte>(buffer), default);
			Console.WriteLine($"Received {result.Count} bytes");
			if (result.MessageType == WebSocketMessageType.Close)
			{
				await _clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed by the MainViewModel", default);
			Console.WriteLine("Socket closed");
				break;
			}
			else if (result.MessageType == WebSocketMessageType.Binary)
			{
				var message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
				Console.WriteLine("Message received");
				_ = Dispatcher.UIThread.InvokeAsync(() => _messages.Add(message));
			}
			// keep the connection alive
			await Task.Delay(100);
		}
	}
}
