using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DiscordJoinServerRequest : WebSocketMessage
	{
		public ulong DiscordUserId;

		public string DiscordUserAccessToken;

		public DiscordJoinType JoinType;
	}
}
