using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RemoveInventoryItemsRequest : WebSocketMessage
	{
		public List<InventoryItem> Items;
	}
}
