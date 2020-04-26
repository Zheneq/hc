using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameInvitationResponse : WebSocketResponseMessage
	{
		public string InviteeHandle;

		public LocalizationPayload LocalizedFailure;
	}
}
