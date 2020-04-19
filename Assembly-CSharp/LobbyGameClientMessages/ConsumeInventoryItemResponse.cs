using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ConsumeInventoryItemResponse : WebSocketResponseMessage
	{
		public ConsumeInventoryItemResult Result;

		public List<InventoryItemWithData> OutputItems;
	}
}
