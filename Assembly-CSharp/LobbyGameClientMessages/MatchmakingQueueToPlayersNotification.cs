using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class MatchmakingQueueToPlayersNotification : WebSocketMessage
	{
		public enum MatchmakingQueueMessage
		{
			_001D,
			_000E,
			_0012,
			_0015
		}

		public long AccountId;

		public MatchmakingQueueMessage MessageToSend;

		public GameType GameType;

		public ushort SubTypeMask;
	}
}
