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
			\u001D,
			\u000E,
			\u0012
		}
	}
}
