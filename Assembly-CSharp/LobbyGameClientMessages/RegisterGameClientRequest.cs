using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RegisterGameClientRequest : WebSocketMessage
	{
		public LobbySessionInfo SessionInfo;

		public AuthInfo AuthInfo;

		public string SteamUserId;

		public LobbyGameClientSystemInfo SystemInfo;
	}
}
