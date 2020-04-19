using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class PurchaseInventoryItemResponse : WebSocketResponseMessage
	{
		public PurchaseResult Result;

		public CurrencyType CurrencyType;

		public int InventoryItemID;
	}
}
