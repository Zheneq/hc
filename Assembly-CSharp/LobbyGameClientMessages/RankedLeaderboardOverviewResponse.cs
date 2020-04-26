using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedLeaderboardOverviewResponse : WebSocketResponseMessage
	{
		public GameType GameType;

		public Dictionary<int, PerGroupSizeTierInfo> TierInfoPerGroupSize;

		public LocalizationPayload LocalizedFailure;
	}
}
