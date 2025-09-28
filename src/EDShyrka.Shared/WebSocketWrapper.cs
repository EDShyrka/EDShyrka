using System.Net.WebSockets;

namespace EDShyrka.Shared
{
	public class WebSocketWrapper : IDisposable
	{
		#region fields
		private readonly WebSocket _webSocket;
		private readonly CancellationTokenSource _cancellationTokenSource;
		private readonly Task _listenTask;

		public event RequestReceivedEventHandler? RequestReceived;
		public event DisconnectedEventHandler? Disconnected;
		#endregion fields

		#region ctor
		/// <summary>
		/// Initializes a new instance of the <see cref="WebSocketWrapper"/> class.
		/// </summary>
		/// <param name="webSocket"></param>
		public WebSocketWrapper(WebSocket webSocket)
		{
			_webSocket = webSocket;
			_cancellationTokenSource = new CancellationTokenSource();
			_listenTask = ListenAsync();
		}
		#endregion ctor

		#region properties
		/// <summary>
		/// The task that listens for incoming messages from the WebSocket connection.
		/// This task can be awaited until the web socket is closed.
		/// </summary>
		public Task ListenTask { get => _listenTask; }

		/// <summary>
		/// True if the web socket is connected, false otherwise.
		/// </summary>
		public bool IsConnected { get => _webSocket.State == WebSocketState.Open; }
		#endregion properties

		#region methods
		/// <summary>
		/// Connect a web socket to a server through a uri.
		/// </summary>
		/// <param name="uri">The server uri.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public static async Task<WebSocketWrapper> ConnectAsync(Uri uri)
		{
			var clientWebSocket = new ClientWebSocket();
			//clientWebSocket.Options.
			await clientWebSocket.ConnectAsync(uri, default);

			return new WebSocketWrapper(clientWebSocket);
		}

		/// <summary>
		/// Send a request to the connected web socket.
		/// </summary>
		/// <param name="data">The request data.</param>
		/// <param name="cancellationToken">A token to cancel the operation.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public Task SendAsync(byte[] data, CancellationToken cancellationToken)
		{
			var buffer = new ReadOnlyMemory<byte>(data);
			return SendAsync(buffer, cancellationToken);
		}

		/// <summary>
		/// Send a request to the connected web socket.
		/// </summary>
		/// <param name="data">The request data.</param>
		/// <param name="cancellationToken">A token to cancel the operation.</param>
		/// <returns>A task representing the asynchronous operation.</returns>
		public Task SendAsync(ReadOnlyMemory<byte> data, CancellationToken cancellationToken)
		{
			return _webSocket.SendAsync(data, WebSocketMessageType.Binary, true, cancellationToken).AsTask();
		}

		/// <summary>
		/// Listens for incoming messages from the WebSocket connection and raises events for received data or disconnection.
		/// </summary>
		/// <remarks>
		/// This method continuously listens for messages from the WebSocket connection while the connection remains open. 
		/// When a message is received, the <see cref="RequestReceived"/> event is raised with the received data.  
		/// If the WebSocket connection is closed by the client, the <see cref="Disconnected"/> event is raised.
		/// </remarks>
		/// <returns>A <see cref="Task"/> instance representing the asynchronous job.</returns>
		private async Task ListenAsync()
		{
			var buffer = new byte[1024 * 4];
			while (IsConnected)
			{
				var result = await _webSocket.ReceiveAsync(new Memory<byte>(buffer), _cancellationTokenSource.Token);
				if (result.MessageType == WebSocketMessageType.Close)
				{
					await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
					break;
				}
				var data = buffer.Take(result.Count).ToArray();
				RequestReceived?.Invoke(this, new RequestReceivedEventArgs(this, data));
			}
			Disconnected?.Invoke(this, new DisconnectedEventArgs(this));
		}
		#endregion methods

		#region IDisposable
		/// <summary>
		/// True if the instance is disposed, false otherwise.
		/// </summary>
		private bool _isDisposed;

		/// <summary>
		/// Dispose the instance.
		/// </summary>
		/// <param name="disposing">True is internal resources must be disposed too, false otherwise.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed == false)
			{
				if (disposing)
				{
					// dispose managed objects
					_webSocket.Dispose();
					_cancellationTokenSource.Cancel();
				}

				_isDisposed = true;
			}
		}

		/// <summary>
		/// Public method of IDisposable implementation.
		/// </summary>
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}
		#endregion IDisposable

		#region event handlers
		/// <summary>
		/// Event handler used when a request is received.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="args">The event arguments</param>
		public delegate void RequestReceivedEventHandler(object sender, RequestReceivedEventArgs args);

		/// <summary>
		/// Event arguments sent when a request is received.
		/// </summary>
		/// <remarks>
		/// Initialize a new <see cref="RequestReceived"/> instance.
		/// </remarks>
		/// <param name="webSocketWrapper"></param>
		/// <param name="data">The data received in the request.</param>
		public class RequestReceivedEventArgs(WebSocketWrapper webSocketWrapper, byte[] data)
			: EventArgs
		{
			/// <summary>
			/// The wrapper from which the data was received.
			/// </summary>
			public WebSocketWrapper WebSocketWrapper { get; } = webSocketWrapper;

			/// <summary>
			/// The data received in the request.
			/// </summary>
			public byte[] Data { get; } = data;
		}

		/// <summary>
		/// Event handler used when a web socket is disconnected.
		/// </summary>
		/// <param name="sender">The event sender.</param>
		/// <param name="args">The event arguments</param>
		public delegate void DisconnectedEventHandler(object sender, DisconnectedEventArgs args);

		/// <summary>
		/// Event arguments sent when a web socket is disconnected.
		/// </summary>
		/// <param name="webSocketWrapper"></param>
		public class DisconnectedEventArgs(WebSocketWrapper webSocketWrapper)
			: EventArgs
		{
			/// <summary>
			/// The wrapper where the socket was disconnected.
			/// </summary>
			public WebSocketWrapper WebSocketWrapper { get; } = webSocketWrapper;
		}
		#endregion event handlers
	}
}
