using WebSocketSharp;

public class ConnectionClosedNotification : WebSocketMessage
{
	public string Reason;

	public CloseStatusCode Code;

	public string Message
	{
		get
		{
			if (Reason.IsNullOrEmpty())
			{
				return $"{Code}";
			}
			return $"{Code}: {Reason}";
		}
	}
}
