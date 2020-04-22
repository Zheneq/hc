using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_AccountCurrencyChangeRequest : WebSocketMessage
	{
		public CurrencyType currency;

		public int amount;
	}
}
