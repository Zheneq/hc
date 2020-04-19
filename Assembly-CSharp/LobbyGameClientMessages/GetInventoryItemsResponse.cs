using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class GetInventoryItemsResponse : WebSocketResponseMessage
	{
		public List<InventoryItem> Items;
	}
}
