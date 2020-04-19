using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseGameRequest : WebSocketMessage
	{
		public int GamePackIndex;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}
