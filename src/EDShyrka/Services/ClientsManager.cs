using EDShyrka.Interfaces;
using EDShyrka.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDShyrka.Services
{
	/// <summary>
	/// This class is responsible for managing WebSocket clients.
	/// It is used to keep track of connected clients.
	/// </summary>
	public class ClientsManager : IClientsManager
	{
		#region fields
		private readonly ILogger _logger;
		private static readonly List<WebSocketClient> _clients = [];
		#endregion fields

		#region ctor
		/// <summary>
		/// Initializes a new instance of the <see cref="ClientsManager"/> class.
		/// </summary>
		/// <param name="logger">The <see cref="ILogger"/> instance.</param>
		public ClientsManager(ILogger<ClientsManager> logger)
		{
			_logger = logger;
		}
		#endregion ctor

		#region IClientsManager
		/// <summary>
		/// Occurs when a client successfully establishes a connection.
		/// </summary>
		public event ClientConnectedEventHandler? ClientConnected;

		/// <summary>
		/// Gets the collection of connected WebSocket clients.
		/// </summary>
		public IEnumerable<WebSocketClient> Clients => _clients;

		/// <summary>
		/// Registers a new WebSocket client and raises the <see cref="ClientConnected"/> event.
		/// </summary>
		public async Task<WebSocketClient> ConnectClient(WebSocketManager webSocketManager)
		{
			var webSocket = await webSocketManager.AcceptWebSocketAsync();
			_logger.Log(LogLevel.Information, "WebSocket connected");

			var client = new WebSocketClient(webSocket);
			_clients.Add(client);
			_logger.Log(LogLevel.Information, "client registered");

			client.ClientDisconnected += OnClientDisconnected;
			ClientConnected?.Invoke(this, new ClientConnectedEventArgs(client));

			return client;
		}
		#endregion IClientsManager

		#region methods
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
		#endregion methods
	}
}
