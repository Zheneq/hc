using System;

public class ServerMessageBroadcast
{
	public TimeSpan TimeInAdvance;

	public ServerMessage Message;

	public ServerMessageBroadcast()
	{
		Message = new ServerMessage();
	}
}
