using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_AddFactionResponse : WebSocketResponseMessage
	{
		public int factionId;

		public int amount;

		public long accountID;
	}
}
