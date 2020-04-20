using System;
using System.Collections.Generic;
using System.Linq;

internal class MatchmakingQueueDecayInfo : IDecayInfo
{
	public bool IsActive
	{
		get
		{
			return !this.m_leaderboardTiers.IsNullOrEmpty<TierInfo>();
		}
	}

	public DateTime UtcNow { get; set; }

	public unsafe bool GetDecayAmount(int tierIndex, out int amount, out TimeSpan start)
	{
		if (!this.m_leaderboardTiers.IsNullOrEmpty<TierInfo>())
		{
			TierInfo tierInfo = this.m_leaderboardTiers.ElementAtOrDefault(tierIndex);
			if (tierInfo != null && tierInfo.DecayAmount != 0)
			{
				amount = Math.Abs(tierInfo.DecayAmount);
				start = tierInfo.DecayStart;
				return true;
			}
		}
		amount = 0;
		start = TimeSpan.Zero;
		return false;
	}

	public bool DoesTierHaveLimitlessLesserNeighborTier(int tierIndex)
	{
		if (!this.m_leaderboardTiers.IsNullOrEmpty<TierInfo>())
		{
			TierInfo tierInfo = this.m_leaderboardTiers.ElementAtOrDefault(tierIndex);
			if (tierInfo != null)
			{
				if (tierInfo.PointType != TierPointsTypes.Limitless)
				{
					if (tierInfo.PointType != TierPointsTypes.DerivedFromMMR)
					{
						return false;
					}
				}
				TierInfo tierInfo2 = this.m_leaderboardTiers.ElementAtOrDefault(tierIndex + 1);
				if (tierInfo2 != null)
				{
					bool result;
					if (tierInfo2.PointType != TierPointsTypes.Limitless)
					{
						result = (tierInfo2.PointType == TierPointsTypes.DerivedFromMMR);
					}
					else
					{
						result = true;
					}
					return result;
				}
			}
		}
		return false;
	}

	internal List<TierInfo> m_leaderboardTiers { get; set; }
}
