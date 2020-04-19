using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RejoinGameRequest : WebSocketMessage
	{
		public LobbyGameInfo PreviousGameInfo;

		public bool Accept;
	}
}
