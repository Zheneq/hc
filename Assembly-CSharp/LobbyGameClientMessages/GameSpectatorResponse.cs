using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GameSpectatorResponse : WebSocketResponseMessage
	{
		public string InviteeHandle;

		public LocalizationPayload LocalizedFailure;
	}
}
