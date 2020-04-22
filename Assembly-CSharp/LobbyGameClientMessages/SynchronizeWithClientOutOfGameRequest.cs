using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class SynchronizeWithClientOutOfGameRequest : WebSocketMessage
	{
		public string GameServerProcessCode;
	}
}
