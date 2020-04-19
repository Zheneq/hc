using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerGroupInfoUpdateRequest : WebSocketMessage
	{
		public GameType GameType;
	}
}
