using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedLeaderboardOverviewRequest : WebSocketMessage
	{
		public GameType GameType;
	}
}
