using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerInfoUpdateRequest : WebSocketMessage
	{
		public LobbyPlayerInfoUpdate PlayerInfoUpdate;

		public GameType? GameType;
	}
}
