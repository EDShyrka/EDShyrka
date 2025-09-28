using EDShyrka.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using EDShyrka.Shared;

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
		private static readonly List<WebSocketWrapper> _clients = [];
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
		public IEnumerable<WebSocketWrapper> Clients => _clients;

		/// <summary>
		/// Registers a new WebSocket client and raises the <see cref="ClientConnected"/> event.
		/// </summary>
		public async Task<WebSocketWrapper> ConnectClient(WebSocketManager webSocketManager)
		{
			var webSocket = await webSocketManager.AcceptWebSocketAsync();
			_logger.Log(LogLevel.Information, "WebSocket connected");

			var client = new WebSocketWrapper(webSocket);
			_clients.Add(client);
			_logger.Log(LogLevel.Information, "client registered");

			client.Disconnected += OnClientDisconnected;
			ClientConnected?.Invoke(this, new ClientConnectedEventArgs(client));

			return client;
		}
		#endregion IClientsManager

		#region methods
		private void OnClientDisconnected(object sender, WebSocketWrapper.DisconnectedEventArgs args)
		{
			_logger.Log(LogLevel.Information, "Client disconnected");
			RemoveClient(args.WebSocketWrapper);
		}

		private void RemoveClient(WebSocketWrapper client)
		{
			client.Disconnected -= OnClientDisconnected;
			_clients.Remove(client);
		}
		#endregion methods
	}
}
