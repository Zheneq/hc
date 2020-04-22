using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseLootMatrixPackRequest : WebSocketMessage
	{
		public int LootMatrixPackIndex;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}
