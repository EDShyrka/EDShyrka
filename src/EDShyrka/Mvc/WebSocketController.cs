using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EDShyrka.Mvc
{
	[ApiController]
	[Route("ws")]
	public class WebSocketController : ControllerBase
	{
		public async Task Get()
		{
			if (HttpContext.WebSockets.IsWebSocketRequest == false)
			{
				Response.StatusCode = StatusCodes.Status403Forbidden;
				return;
			}

			// accept the client connection
			using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
			while(webSocket.State == System.Net.WebSockets.WebSocketState.Open)
			{
				var buffer = new Memory<byte>(new byte[1024]);
				var result = await webSocket.ReceiveAsync(buffer, CancellationToken.None);
				if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Close)
				{
					await webSocket.CloseAsync(System.Net.WebSockets.WebSocketCloseStatus.NormalClosure, "Closed by the WebSocketController", CancellationToken.None);
					break;
				}
				else if (result.MessageType == System.Net.WebSockets.WebSocketMessageType.Binary)
				{
					buffer = buffer.Slice(0, result.Count);
					await webSocket.SendAsync(buffer, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
				}

				// keep the connection alive
				await Task.Delay(100);
			}

			await Task.CompletedTask;
		}
	}
}
