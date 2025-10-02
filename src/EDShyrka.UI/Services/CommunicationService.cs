using EDShyrka.Shared;
using EDShyrka.UI.Models;
using System;
using System.Threading.Tasks;

namespace EDShyrka.UI.Services
{
	public class CommunicationService
	{
		private readonly Uri _serverUri;
		private WebSocketWrapper? _connection;

		public CommunicationService(AppSettings appSettings)
		{
			var builder = new UriBuilder(appSettings.ServerLocation) { Path = "/ws", Scheme = "ws" };
			_serverUri = builder.Uri;
		}

		public async Task<WebSocketWrapper> GetConnection()
		{
			await EnsureConnectionIsValidAsync();
			return _connection!;
		}

		private async Task EnsureConnectionIsValidAsync()
		{
			if (_connection != null)
			{
				if (_connection.IsConnected)
					return;

				_connection.Dispose();
				_connection = null;
			}

			if (_connection == null)
			{
				_connection = await WebSocketWrapper.ConnectAsync(_serverUri);
			}
		}
	}
}
