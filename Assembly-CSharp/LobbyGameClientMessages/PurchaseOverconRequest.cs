using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseOverconRequest : WebSocketMessage
	{
		public CurrencyType CurrencyType;

		public int OverconId;
	}
}
