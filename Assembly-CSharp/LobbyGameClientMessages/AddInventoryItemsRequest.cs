using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class AddInventoryItemsRequest : WebSocketMessage
	{
		public List<InventoryItem> Items;
	}
}
