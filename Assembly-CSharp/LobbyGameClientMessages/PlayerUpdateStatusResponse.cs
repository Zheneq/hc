using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PlayerUpdateStatusResponse : WebSocketResponseMessage
	{
		public long AccountId;

		public string StatusString;
	}
}
