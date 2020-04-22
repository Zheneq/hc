using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_QueryAccessRequest : WebSocketMessage
	{
		public string Target;
	}
}
