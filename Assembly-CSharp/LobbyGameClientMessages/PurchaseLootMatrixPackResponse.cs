using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseLootMatrixPackResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public int LootMatrixPackIndex;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}
