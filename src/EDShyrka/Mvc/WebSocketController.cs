using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;
using EDShyrka.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net.WebSockets;

namespace EDShyrka.Mvc
{
	[ApiController]
	[Route("ws")]
	[TypeFilter<WebSocketAuthorizationFilter>]
	public class WebSocketController : ControllerBase
	{
		private readonly ILogger _logger;

		public WebSocketController(ILogger<WebSocketController> logger)
		{
			_logger = logger;
		}

		public async Task Get()
		{
			// accept the client connection
			using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
			_logger.Log(LogLevel.Information, "WebSocket client {clientId} connected", clientId);
			while (webSocket.State == System.Net.WebSockets.WebSocketState.Open)
			{
				var buffer = new Memory<byte>(new byte[1024]);
				var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
				_logger.Log(LogLevel.Information, "WebSocket client sent {Count} bytes in a {MessageType} message (eof:{isEof})", result.Count, result.MessageType, result.EndOfMessage);
				if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
				{
					await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketController", CancellationToken.None);
					_logger.Log(LogLevel.Information, "WebSocket client {clientId} disconnected", clientId);
					break;
				}
				else if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Binary)
				{
					buffer = buffer.Slice(0, result.Count);
					await webSocket.SendAsync(buffer, System.Net.WebSockets.WebSocketMessageType.Binary, true, CancellationToken.None);
					_logger.Log(LogLevel.Information, "Replying to client");
				}

				// keep the connection alive
				await Task.Delay(100);
			}

			await Task.CompletedTask;
		}
	}
}
