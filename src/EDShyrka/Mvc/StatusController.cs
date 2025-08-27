using EDShyrka.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EDShyrka.Mvc
{
	[ApiController]
	[Route("status")]
	public class StatusController : ControllerBase
	{
		private readonly ILogger<StatusController> _logger;



		public StatusController(ILogger<StatusController> logger)
		{
			_logger = logger;
		}

		[HttpGet]
		public async Task Get()
		{
			// log version, uptime, memory consumption
			_logger.LogVersionInfo(LoggingHelpers.ApplicationVersion);
			var uptime = DateTime.Now - GetStartTime();
			_logger.Log(LogLevel.Information, "Uptime: {Uptime}", uptime);
			var allocatedMemory = GC.GetTotalAllocatedBytes();
			_logger.Log(LogLevel.Information, "Allocated Memory: {Memory} bytes", allocatedMemory);

			var sb = new StringBuilder()
				.AppendLine($"{DateTime.Now:hh:mm:ss} - Welcome to EDShyrka !")
				.AppendLine($"Uptime: {uptime}")
				.AppendLine($"Allocated Memory: {allocatedMemory} bytes");
			await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(sb.ToString()));
		}

		private DateTime GetStartTime()
		{
			return Process.GetCurrentProcess().StartTime;
		}
	}
}
