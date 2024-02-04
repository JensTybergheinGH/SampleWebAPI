namespace WebApi.Services
{
	public interface IHubClient
	{
		Task BroadCastMessage();

		Task BroadcastNotification(NotificationMessageModel data);
	}
}