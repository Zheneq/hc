using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_PlayerXPChangeRequest : WebSocketMessage
	{
		public int amount;
	}
}
