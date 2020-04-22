using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class AddInventoryItemsResponse : WebSocketResponseMessage
	{
		public List<InventoryItem> Items;
	}
}
