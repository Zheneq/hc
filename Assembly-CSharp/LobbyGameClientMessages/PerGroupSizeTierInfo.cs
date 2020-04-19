using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct PerGroupSizeTierInfo
	{
		public Dictionary<int, RankedTierInfo> PerTierInfo;

		public RankedScoreboardEntry? OurEntry;
	}
}
