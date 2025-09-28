using EDShyrka.Shared;
using System;
using System.Threading.Tasks;

namespace EDShyrka.UI.Services
{
	public class CommunicationService
	{
		private WebSocketWrapper? _connection;

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
#warning currently hard coded, must be retrieved dynamically
				var builder = new UriBuilder("ws", "localhost", 12080, "/ws");
				_connection = await WebSocketWrapper.ConnectAsync(builder.Uri);
			}
		}
	}
}
