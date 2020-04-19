using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SetRegionRequest : WebSocketMessage
	{
		public Region Region;
	}
}
