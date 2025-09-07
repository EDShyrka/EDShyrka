using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;

namespace EDShyrka.Models
{
	public class WebSocketClient : IDisposable
	{
		#region fields
		private const int _bufferSize = 4096;
		private readonly Memory<byte> _buffer = new Memory<byte>();
		private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

		public event WebSocketClientRequestReceivedEventHandler? RequestReceived;
		public event WebSocketClientDisconnectedEventHandler? ClientDisconnected;
		#endregion fields

		#region ctor
		public WebSocketClient(WebSocket webSocket)
		{
			WebSocket = webSocket;
			Listener = ListenAsync(_cancellationTokenSource.Token);
		}
		#endregion ctor

		#region properties
		public WebSocket WebSocket { get; init; }

		public WebSocketState State => WebSocket.State;

		public Task Listener { get; private set; }
		#endregion properties

		#region methods
		public Task SendAsync(byte[] data, CancellationToken cancellationToken)
		{
			var buffer = new ReadOnlyMemory<byte>(data);
			return SendAsync(buffer, cancellationToken);
		}

		public Task SendAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
		{
			return WebSocket.SendAsync(data, WebSocketMessageType.Binary, true, cancellationToken).AsTask();
		}

		/// <summary>
		/// Listens for incoming messages from the WebSocket connection and raises events for received data or disconnection.
		/// </summary>
		/// <remarks>
		/// This method continuously listens for messages from the WebSocket connection while the connection remains open. 
		/// When a message is received, the <see cref="RequestReceived"/> event is raised with the received data.  
		/// If the WebSocket connection is closed by the client, the <see cref="ClientDisconnected"/> event is raised.
		/// </remarks>
		/// <param name="cancellationToken">A token that can be used to cancel the listening operation.
		/// If cancellation is requested, the method will terminate early.</param>
		/// <returns>A <see cref="Task"/> instance representing the asynchronous job.</returns>
		private async Task ListenAsync(CancellationToken cancellationToken)
		{
			// Implementation for processing clients goes here
			// This could involve sending messages, handling disconnections, etc.
			var buffer = new byte[1024 * 4];
			while (WebSocket.State == WebSocketState.Open)
			{
				var result = await WebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
				if (result.MessageType == WebSocketMessageType.Close)
				{
					await WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
					break;
				}
				var data = buffer.Take(result.Count).ToArray();
				RequestReceived?.Invoke(this, new WebSocketClientRequestReceivedEventArgs(this, data));
			}
			ClientDisconnected?.Invoke(this, new WebSocketClientDisconnectedEventArgs(this));
		}
		#endregion methods

		#region IDisposable
		private bool _isDisposed;

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed == false)
			{
				if (disposing)
				{
					// dispose managed objects
					WebSocket.Dispose();
					_cancellationTokenSource.Cancel();
				}

				_isDisposed = true;
			}
		}

		public void Dispose()
		{
			// Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion IDisposable
	}

	public delegate void WebSocketClientRequestReceivedEventHandler(object sender, WebSocketClientRequestReceivedEventArgs args);

	public class WebSocketClientRequestReceivedEventArgs : EventArgs
	{
		public WebSocketClient Client { get; }

		public byte[] Data { get; }

		public WebSocketClientRequestReceivedEventArgs(WebSocketClient client, byte[] data)
		{
			Client = client;
			Data = data;
		}
	}

	public delegate void WebSocketClientDisconnectedEventHandler(object sender, WebSocketClientDisconnectedEventArgs args);

	public class WebSocketClientDisconnectedEventArgs : EventArgs
	{
		public WebSocketClient Client { get; }

		public WebSocketClientDisconnectedEventArgs(WebSocketClient client)
		{
			Client = client;
		}
	}
}
