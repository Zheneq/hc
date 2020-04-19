using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameCheatUpdateRequest : WebSocketMessage
	{
		public GameOptionFlag GameOptionFlags;

		public PlayerGameOptionFlag PlayerGameOptionFlags;
	}
}
