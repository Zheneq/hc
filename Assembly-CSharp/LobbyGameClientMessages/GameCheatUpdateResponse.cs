using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameCheatUpdateResponse : WebSocketResponseMessage
	{
		public GameOptionFlag GameOptionFlags;

		public PlayerGameOptionFlag PlayerGameOptionFlags;
	}
}
