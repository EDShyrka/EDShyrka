using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;

namespace EDShyrka.Mvc.Filters
{
	/// <summary>
	/// A filter that asynchronously authorize only requests from a WebSocket.
	/// </summary>
	public class WebSocketAuthorizationFilter : IAsyncAuthorizationFilter
	{
		/// <summary>
		/// Called early in the filter pipeline to confirm request is authorized.
		/// </summary>
		/// <param name="context">The <see cref="AuthorizationFilterContext"/>.</param>
		/// <returns>A <see cref="Task"/> that on completion indicates the filter has executed.</returns>
		public Task OnAuthorizationAsync(AuthorizationFilterContext context)
		{
#if !DEBUG // disabled on DEBUG
            if (HttpContext.WebSockets.IsWebSocketRequest == false)
            {
                context.Result = new Microsoft.AspNetCore.Mvc.StatusCodeResult(StatusCodes.Status403Forbidden);
            }
#endif //!DEBUG
			return Task.CompletedTask;
		}
	}
}
