using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ConsumeInventoryItemsRequest : WebSocketMessage
	{
		public List<int> ItemIds;

		public bool ToISO;
	}
}
