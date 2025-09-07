using EDShyrka.Interfaces;
using EDShyrka.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace EDShyrka.Services
{
	/// <summary>
	/// This class is responsible for managing WebSocket clients.
	/// It is used to keep track of connected clients.
	/// </summary>
	public class ClientsManager : IClientsManager
	{
		private readonly ILogger _logger;
		private static readonly List<WebSocketClient> _clients = [];

		public event ClientConnectedEventHandler? ClientConnected;

		/// <summary>
		/// Initializes a new instance of the <see cref="ClientsManager"/> class.
		/// </summary>
		/// <param name="logger">The <see cref="ILogger"/> instance.</param>
		public ClientsManager(ILogger<ClientsManager> logger)
		{
			_logger = logger;
		}

		/// <summary>
		/// Gets the collection of connected WebSocket clients.
		/// </summary>
		public IEnumerable<WebSocketClient> Clients => _clients;

		/// <summary>
		/// Registers a new WebSocket client and raises the <see cref="ClientConnected"/> event.
		/// </summary>
		public void RegisterClient(WebSocketClient client)
		{
			_clients.Add(client);
			client.ClientDisconnected += OnClientDisconnected;
			ClientConnected?.Invoke(this, new ClientConnectedEventArgs(client));
		}

		private void OnClientDisconnected(object sender, WebSocketClientDisconnectedEventArgs args)
		{
			_logger.Log(LogLevel.Information, "Client disconnected");
			RemoveClient(args.Client);
		}

		private void RemoveClient(WebSocketClient client)
		{
			client.ClientDisconnected -= OnClientDisconnected;
			_clients.Remove(client);
		}
	}
}
