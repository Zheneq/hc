using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ForceMatchmakingQueueNotification : WebSocketMessage
	{
		public enum ActionType
		{
			_001D,
			_000E,
			_0012
		}

		public ActionType Action;

		public GameType GameType;
	}
}
