using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SyncNotification : WebSocketMessage
	{
		public bool Reply;
	}
}
