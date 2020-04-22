using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerUpdateStatusRequest : WebSocketMessage
	{
		public long AccountId;

		public string StatusString;
	}
}
