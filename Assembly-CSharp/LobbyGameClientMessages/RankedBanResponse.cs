using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedBanResponse : WebSocketResponseMessage
	{
		public LocalizationPayload LocalizedFailure;
	}
}
