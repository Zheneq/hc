using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DiscordGetRpcTokenResponse : WebSocketResponseMessage
	{
		public string DiscordClientId;

		public string DiscordRpcToken;

		public string DiscordRpcOrigin;
	}
}
