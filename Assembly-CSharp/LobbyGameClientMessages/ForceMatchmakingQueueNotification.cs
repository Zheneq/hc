using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ForceMatchmakingQueueNotification : WebSocketMessage
	{
		public ForceMatchmakingQueueNotification.ActionType Action;

		public GameType GameType;

		public enum ActionType
		{
			symbol_001D,
			symbol_000E,
			symbol_0012
		}
	}
}
