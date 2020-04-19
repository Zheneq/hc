using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class AssignGameClientRequest : WebSocketMessage
	{
		public LobbySessionInfo SessionInfo;

		public AuthInfo AuthInfo;

		public int PreferredLobbyServerIndex;
	}
}
