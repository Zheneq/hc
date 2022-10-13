using System;
using System.Collections.Generic;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CalculateFreelancerStatsResponse : WebSocketResponseMessage
	{
		public Dictionary<StatDisplaySettings.StatType, PercentileInfo> GlobalPercentiles;
		public Dictionary<int, PercentileInfo> FreelancerSpecificPercentiles;
		public LocalizationPayload LocalizedFailure;
	}
}
