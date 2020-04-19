using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameInvitationRequest : WebSocketMessage
	{
		public string InviteeHandle;
	}
}
