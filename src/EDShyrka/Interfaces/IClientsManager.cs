using EDShyrka.Shared;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EDShyrka.Interfaces
{
	/// <summary>
	/// Defines the contract for managing WebSocket clients.
	/// Keep track of connected clients and handle client connection events.
	/// </summary>
	public interface IClientsManager
	{
		/// <summary>
		/// Occurs when a client successfully establishes a connection.
		/// </summary>
		public event ClientConnectedEventHandler ClientConnected;

		/// <summary>
		/// Gets the collection of connected WebSocket clients.
		/// </summary>
		public IEnumerable<WebSocketWrapper> Clients { get; }

		/// <summary>
		/// Registers a new WebSocket client and raises the <see cref="ClientConnected"/> event.
		/// </summary>
		public Task<WebSocketWrapper> ConnectClient(WebSocketManager webSocketManager);
	}

	/// <summary>
	/// Represents the method that will handle the event raised when a client connects to the server.
	/// </summary>
	/// <param name="sender">The source of the event, typically the server instance.</param>
	/// <param name="args">The event data containing information about the connected client.</param>
	public delegate void ClientConnectedEventHandler(object sender, ClientConnectedEventArgs args);

	/// <summary>
	/// Provides data for the event that occurs when a client connects to the server.
	/// </summary>
	public class ClientConnectedEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ClientConnectedEventArgs"/> class.
		/// </summary>
		/// <param name="client">The <see cref="WebSocketClient"/> instance representing the connected client.</param>
		public ClientConnectedEventArgs(WebSocketWrapper client)
		{
			Client = client;
		}

		/// <summary>
		/// Gets the WebSocket client used to manage WebSocket connections.
		/// </summary>
		public WebSocketWrapper Client { get; }
	}
}
