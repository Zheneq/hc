using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameStatusNotification : WebSocketMessage
	{
		public string GameServerProcessCode;

		public GameStatus GameStatus;
	}
}
