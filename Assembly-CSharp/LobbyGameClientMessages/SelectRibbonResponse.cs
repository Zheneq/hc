using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SelectRibbonResponse : WebSocketResponseMessage
	{
		public int CurrentRibbonID;
	}
}
