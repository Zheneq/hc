using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RegisterGameClientResponse : WebSocketResponseMessage
	{
		public LobbySessionInfo SessionInfo;

		public AuthInfo AuthInfo;

		public LobbyStatusNotification Status;

		public string DevServerConnectionUrl;

		public LocalizationPayload LocalizedFailure;
	}
}
