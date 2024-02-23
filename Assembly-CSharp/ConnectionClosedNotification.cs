using System.Text;
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
				return new StringBuilder().Append(Code).ToString();
			}
			return new StringBuilder().Append(Code).Append(": ").Append(Reason).ToString();
		}
	}
}
