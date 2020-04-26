using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class MatchmakingQueueAssignmentNotification : WebSocketMessage
	{
		public LobbyMatchmakingQueueInfo MatchmakingQueueInfo;

		public string Reason;
	}
}
