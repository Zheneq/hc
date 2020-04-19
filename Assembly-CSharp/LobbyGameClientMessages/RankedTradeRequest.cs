using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedTradeRequest : WebSocketMessage
	{
		public RankedTradeData Trade;
	}
}
