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
			\u001D,
			\u000E,
			\u0012,
			\u0015
		}
	}
}
