using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedTradeResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}
