using System;

public class ServerMessageBroadcast
{
	public TimeSpan TimeInAdvance;

	public ServerMessage Message;

	public ServerMessageBroadcast()
	{
		this.Message = new ServerMessage();
	}
}
