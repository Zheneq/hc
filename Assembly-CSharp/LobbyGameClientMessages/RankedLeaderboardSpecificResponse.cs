using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedLeaderboardSpecificResponse : WebSocketResponseMessage
	{
		public List<RankedScoreboardEntry> Entries;

		public LocalizationPayload LocalizedFailure;
	}
}
