using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseInventoryItemRequest : WebSocketMessage
	{
		public int InventoryItemID;

		public CurrencyType CurrencyType;
	}
}
