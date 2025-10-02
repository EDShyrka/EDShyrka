using EDShyrka.Interfaces;
using EDShyrka.Shared;
using EDShyrka.UI;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EDShyrka.Services
{
	/// <summary>
	/// This class is responsible for managing WebSocket clients.
	/// It can register clients, remove them, and manage their connections.
	/// </summary>
	public class CommunicationWorker : BackgroundService
	{
		private readonly ILogger _logger;
		private readonly IClientsManager _clientsManager;

		public CommunicationWorker(ILogger<CommunicationWorker> logger, IClientsManager clientsManager)
		{
			_logger = logger;
			_clientsManager = clientsManager;
			_clientsManager.ClientConnected += OnClientConnected;
		}

		private void OnClientConnected(object sender, ClientConnectedEventArgs args)
		{
			var client = args.Client;
			client.RequestReceived += OnClientRequestReceived;
			client.Disconnected += OnClientDisconnected;
		}

		private void OnClientDisconnected(object sender, WebSocketWrapper.DisconnectedEventArgs args)
		{
			var client = args.WebSocketWrapper;
			client.RequestReceived -= OnClientRequestReceived;
		}

		private void OnClientRequestReceived(object sender, WebSocketWrapper.RequestReceivedEventArgs args)
		{
			//_clientsManager.BroadcastAsync(Encoding.UTF8.GetString(args.Data));
			foreach(var client in _clientsManager.Clients)
				client.SendAsync(new Memory<byte>(args.Data), CancellationToken.None);
		}

		/// <summary>
		/// Triggered when the application host is ready to start the service.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public override Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.Log(LogLevel.Information, "Starting");
			return base.StartAsync(cancellationToken);
		}

		/// <summary>
		/// This method is called when the <see cref="IHostedService"/> starts.
		/// The implementation should return a task that represents the lifetime of the long running operation(s) being performed.
		/// </summary>
		/// <param name="stoppingToken">Triggered when <see cref="IHostedService.StopAsync(CancellationToken)"/> is called.</param>
		/// <returns>A <see cref="Task"/> that represents the long running operations.</returns>
		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.Log(LogLevel.Information, "Running");
			while (stoppingToken.IsCancellationRequested == false)
			{
				await Task.Delay(100);
			}
		}

		/// <summary>
		/// Triggered when the application host is performing a graceful shutdown.
		/// </summary>
		/// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
		/// <returns>A <see cref="Task"/> that represents the asynchronous operation.</returns>
		public override Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.Log(LogLevel.Information, "Stopping");
			return base.StopAsync(cancellationToken);
		}
	}
}
