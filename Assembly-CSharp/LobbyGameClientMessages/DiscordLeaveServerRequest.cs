using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DiscordLeaveServerRequest : WebSocketMessage
	{
		public DiscordJoinType JoinType;
	}
}
