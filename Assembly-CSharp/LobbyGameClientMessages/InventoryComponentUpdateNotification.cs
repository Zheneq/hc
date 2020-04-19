using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class InventoryComponentUpdateNotification : WebSocketResponseMessage
	{
		public InventoryComponent InventoryComponent;
	}
}
