using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_ForceMatchmakingResponse : WebSocketResponseMessage
	{
		public GameType GameType;
	}
}
