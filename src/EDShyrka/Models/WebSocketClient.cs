using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDShyrka.Models
{
	public class WebSocketClient
	{
		public WebSocketClient(WebSocket webSocket)
		{
			WebSocket = webSocket;
		}

		public WebSocket WebSocket { get; init; }

		public WebSocketState State => WebSocket.State;

		public async Task<byte[]> ReceiveAsync(CancellationToken cancellationToken)
		{
			var buffer = new Memory<byte>(new byte[1024 * 4]);
			var result = await WebSocket.ReceiveAsync(buffer, cancellationToken);
#warning todo : handle result.MessageType == WebSocketMessageType.Close;
			// check count
			// check EndOfMessage
			return buffer.ToArray();
		}

		public async Task SendAsync(byte[] data, CancellationToken cancellationToken)
		{
			var buffer = new ArraySegment<byte>(data);
			await WebSocket.SendAsync(buffer, WebSocketMessageType.Binary, true, cancellationToken);
		}

		private async Task ProcessClientsAsync()
		{
			// Implementation for processing clients goes here
			// This could involve sending messages, handling disconnections, etc.
			var buffer = new byte[1024 * 4];
			while (WebSocket.State == System.Net.WebSockets.WebSocketState.Open)
			{
				var result = await WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
				if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
				{
					await WebSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
				}
				else
				{
					var outgoing = new ArraySegment<byte>(buffer, 0, result.Count);
					await WebSocket.SendAsync(outgoing, result.MessageType, result.EndOfMessage, CancellationToken.None);
				}
			}
			// If the WebSocket is closed, remove it from the list of clients

		}
	}
}
