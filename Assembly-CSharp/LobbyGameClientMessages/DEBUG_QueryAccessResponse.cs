using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DEBUG_QueryAccessResponse : WebSocketResponseMessage
	{
		public string Text;
	}
}
