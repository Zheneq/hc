using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameSpectatorRequest : WebSocketMessage
	{
		public string InviteeHandle;
	}
}
