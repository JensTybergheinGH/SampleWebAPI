using Newtonsoft.Json;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;

public class WebSocketManager
{
	private readonly ConcurrentDictionary<string, WebSocket> _webSockets = new ConcurrentDictionary<string, WebSocket>();

	public void AddWebSocket(string key, WebSocket webSocket)
	{
		_webSockets.TryAdd(key, webSocket);
	}

	public void RemoveWebSocket(string key)
	{
		_webSockets.TryRemove(key, out _);
	}

	public async Task BroadcastMessageAsync(string messageType, object message)
	{
		var jsonMessage = JsonConvert.SerializeObject(message);

		foreach (var webSocket in _webSockets.Values)
		{
			if (webSocket.State == WebSocketState.Open)
			{
				var buffer = Encoding.UTF8.GetBytes(jsonMessage);
				await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
			}
		}
	}
}