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
			\u001D,
			\u000E,
			\u0012
		}
	}
}
