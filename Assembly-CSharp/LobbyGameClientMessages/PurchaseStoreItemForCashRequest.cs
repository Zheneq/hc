using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseStoreItemForCashRequest : WebSocketMessage
	{
		public int InventoryTemplateId;

		public long PaymentMethodId;

		public string AccountCurrency;
	}
}
