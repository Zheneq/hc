using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DiscordGetAccessTokenResponse : WebSocketResponseMessage
	{
		public string DiscordAccessToken;
	}
}
