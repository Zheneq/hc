using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ConsumeInventoryItemsResponse : WebSocketResponseMessage
	{
		public List<ConsumeInventoryItemResult> Errors;

		public List<InventoryItemWithData> OutputItems;
	}
}
