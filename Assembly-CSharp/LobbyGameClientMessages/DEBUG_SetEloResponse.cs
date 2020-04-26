using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_SetEloResponse : WebSocketResponseMessage
	{
		public int EloValue;

		public int MatchCount;
	}
}
