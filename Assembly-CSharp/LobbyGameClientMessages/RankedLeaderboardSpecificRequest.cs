using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedLeaderboardSpecificRequest : WebSocketMessage
	{
		public enum RequestSpecificationType
		{
			_001D,
			_000E,
			_0012
		}

		public GameType GameType;

		public int GroupSize;

		public RequestSpecificationType Specification;
	}
}
