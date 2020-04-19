using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class MatchmakingQueueStatusNotification : WebSocketMessage
	{
		public LobbyMatchmakingQueueInfo MatchmakingQueueInfo;
	}
}
