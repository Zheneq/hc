using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class MatchmakingQueueToPlayersNotification : WebSocketMessage
	{
		public long AccountId;

		public MatchmakingQueueToPlayersNotification.MatchmakingQueueMessage MessageToSend;

		public GameType GameType;

		public ushort SubTypeMask;

		public enum MatchmakingQueueMessage
		{
			symbol_001D,
			symbol_000E,
			symbol_0012,
			symbol_0015
		}
	}
}
