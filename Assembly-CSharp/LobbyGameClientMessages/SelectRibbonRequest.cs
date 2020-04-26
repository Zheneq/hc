using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SelectRibbonRequest : WebSocketMessage
	{
		public int RibbonID;
	}
}
