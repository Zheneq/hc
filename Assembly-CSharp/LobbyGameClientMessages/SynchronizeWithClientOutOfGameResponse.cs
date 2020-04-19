using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SynchronizeWithClientOutOfGameResponse : WebSocketResponseMessage
	{
		public string GameServerProcessCode;
	}
}
