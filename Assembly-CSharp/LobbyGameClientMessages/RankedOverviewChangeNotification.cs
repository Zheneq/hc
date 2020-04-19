using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class RankedOverviewChangeNotification : WebSocketMessage
	{
		public GameType GameType;

		public Dictionary<int, PerGroupSizeTierInfo> TierInfoPerGroupSize;
	}
}
