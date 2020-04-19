using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class DiscordJoinServerResponse : WebSocketResponseMessage
	{
		public ulong DiscordServerId;

		public ulong DiscordVoiceChannelId;
	}
}
