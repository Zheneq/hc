using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class ConsumeInventoryItemRequest : WebSocketMessage
	{
		public int ItemId;

		public int ItemCount;

		public bool ToISO;
	}
}
