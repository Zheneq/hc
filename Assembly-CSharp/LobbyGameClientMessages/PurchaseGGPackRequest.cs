using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseGGPackRequest : WebSocketMessage
	{
		public int GGPackIndex;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}
