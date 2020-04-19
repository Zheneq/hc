using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class JoinGameResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}
