using Microsoft.AspNetCore.SignalR;

namespace WebApi.Services
{
	public class BroadcastHub: Hub<IHubClient>
	{
		public string GetConnectionId() => Context.ConnectionId;
	}
}
