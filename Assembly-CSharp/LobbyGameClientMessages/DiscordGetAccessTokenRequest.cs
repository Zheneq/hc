using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DiscordGetAccessTokenRequest : WebSocketMessage
	{
		public string DiscordRpcCode;
	}
}
