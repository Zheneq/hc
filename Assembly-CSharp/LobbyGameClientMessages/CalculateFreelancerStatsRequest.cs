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
			if (this.PersistedStats != null)
			{
				return new float?(this.PersistedStats.GetGameplayStat(Type).Average());
			}
			if (this.MatchFreelancerStats != null)
			{
				return this.MatchFreelancerStats.GetStat(Type);
			}
			return null;
		}

		public float? GetFreelancerStat(int FreelancerStatIndex)
		{
			if (this.PersistedStats != null)
			{
				return new float?(this.PersistedStats.GetFreelancerStat(FreelancerStatIndex).Average());
			}
			if (this.MatchFreelancerStats != null)
			{
				return this.MatchFreelancerStats.GetFreelancerStat(FreelancerStatIndex);
			}
			return null;
		}
	}
}
