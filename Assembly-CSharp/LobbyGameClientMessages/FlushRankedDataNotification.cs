using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class FlushRankedDataNotification : WebSocketMessage
	{
		public GameType GameType;
	}
}
