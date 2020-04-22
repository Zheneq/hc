using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameAssignmentNotification : WebSocketMessage
	{
		public LobbyGameInfo GameInfo;

		public LobbyGameplayOverrides GameplayOverrides;

		public LobbyPlayerInfo PlayerInfo;

		public GameResult GameResult;

		public bool Reconnection;

		public bool Observer;
	}
}
