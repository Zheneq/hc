using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedHoverClickResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}
