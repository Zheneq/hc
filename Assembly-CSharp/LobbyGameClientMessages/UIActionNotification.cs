using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class UIActionNotification : WebSocketMessage
	{
		public string Context;
	}
}
