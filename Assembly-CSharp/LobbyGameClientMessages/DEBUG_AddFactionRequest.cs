using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_AddFactionRequest : WebSocketMessage
	{
		public int factionId;

		public int amount;

		public long accountID;
	}
}
