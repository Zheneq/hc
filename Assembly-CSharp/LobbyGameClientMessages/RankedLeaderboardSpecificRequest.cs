using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedLeaderboardSpecificRequest : WebSocketMessage
	{
		public GameType GameType;

		public int GroupSize;

		public RankedLeaderboardSpecificRequest.RequestSpecificationType Specification;

		public enum RequestSpecificationType
		{
			symbol_001D,
			symbol_000E,
			symbol_0012
		}
	}
}
