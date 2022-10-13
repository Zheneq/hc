using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public class CalculateFreelancerStatsRequest : WebSocketMessage, StatDisplaySettings.IPersistatedStatValueSupplier
	{
		public PersistedStatBucket PersistedStatBucket;
		public CharacterType CharacterType;
		public PersistedStats PersistedStats;
		public MatchFreelancerStats MatchFreelancerStats;

		public float? GetStat(StatDisplaySettings.StatType Type)
		{
			if (PersistedStats != null)
			{
				return PersistedStats.GetGameplayStat(Type).Average();
			}
			if (MatchFreelancerStats != null)
			{
				return MatchFreelancerStats.GetStat(Type);
			}
			return null;
		}

		public float? GetFreelancerStat(int FreelancerStatIndex)
		{
			if (PersistedStats != null)
			{
				return PersistedStats.GetFreelancerStat(FreelancerStatIndex).Average();
			}
			if (MatchFreelancerStats != null)
			{
				return MatchFreelancerStats.GetFreelancerStat(FreelancerStatIndex);
			}
			return null;
		}
	}
}
