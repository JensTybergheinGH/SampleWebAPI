using Microsoft.AspNetCore.SignalR;

namespace WebApi.Services
{
	public class NotificationMessageModel : Hub
	{
		public async Task SendMessageToClients(string message)
		{
			// Broadcast the message to all clients
			await Clients.All.SendAsync("ReceiveMessage", message);
		}
	}
}