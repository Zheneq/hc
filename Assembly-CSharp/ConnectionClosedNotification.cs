using System;
using WebSocketSharp;

public class ConnectionClosedNotification : WebSocketMessage
{
	public string Reason;

	public CloseStatusCode Code;

	public string Message
	{
		get
		{
			if (this.Reason.IsNullOrEmpty())
			{
				return string.Format("{0}", this.Code);
			}
			return string.Format("{0}: {1}", this.Code, this.Reason);
		}
	}
}
