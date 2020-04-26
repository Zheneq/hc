using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class JoinGameRequest : WebSocketMessage
	{
		public string GameServerProcessCode;

		public bool AsSpectator;
	}
}
