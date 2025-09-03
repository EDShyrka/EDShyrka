using EDShyrka.Models;
using System;
using System.Collections.Generic;

namespace EDShyrka.Interfaces
{
	public interface IClientsManager
	{
		public event ClientConnectedEventHandler ClientConnected;

		public void RegisterClient(WebSocketClient client);

		public IEnumerable<WebSocketClient> Clients { get; }
	}

	public delegate void ClientConnectedEventHandler(object sender, ClientConnectedEventArgs args);

	public class ClientConnectedEventArgs : EventArgs
	{
		public WebSocketClient Client { get; }

		public ClientConnectedEventArgs(WebSocketClient client)
		{
			Client = client;
		}
	}
}
