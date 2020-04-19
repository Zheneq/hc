using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseStoreItemForCashResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public int InventoryItemId;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}
