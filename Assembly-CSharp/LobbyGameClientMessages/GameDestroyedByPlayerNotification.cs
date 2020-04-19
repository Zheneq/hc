using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameDestroyedByPlayerNotification : WebSocketMessage
	{
		public string GameServerProcessCode;

		public GameType GameType;

		public GameResult GameResult;

		public LocalizationPayload LocalizedMessage;
	}
}
