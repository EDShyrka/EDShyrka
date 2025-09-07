using EDShyrka.Interfaces;
using EDShyrka.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EDShyrka.Mvc
{
	[ApiController]
	[Route("ws")]
	[TypeFilter<WebSocketAuthorizationFilter>]
	public class WebSocketController : ControllerBase
	{
		#region fields
		private readonly ILogger _logger;
		private readonly IClientsManager _clientsManager;
		#endregion fields

		#region ctor
		public WebSocketController(ILogger<WebSocketController> logger, IClientsManager clientsManager)
		{
			_logger = logger;
			_clientsManager = clientsManager;
		}
		#endregion ctor

		#region methods
		public async Task Get()
		{
			_logger.Log(LogLevel.Information, "Incoming WebSocket client");
			using var client = await _clientsManager.ConnectClient(HttpContext.WebSockets);
			await client.Listener;
		}
		#endregion methods
	}
}
