using System;
using System.Collections.Generic;
using System.Linq;

internal class MatchmakingQueueDecayInfo : IDecayInfo
{
	public bool IsActive
	{
		get { return !m_leaderboardTiers.IsNullOrEmpty(); }
	}

	public DateTime UtcNow
	{
		get;
		set;
	}

	internal List<TierInfo> m_leaderboardTiers
	{
		get;
		set;
	}

	public bool GetDecayAmount(int tierIndex, out int amount, out TimeSpan start)
	{
		if (!m_leaderboardTiers.IsNullOrEmpty())
		{
			TierInfo tierInfo = m_leaderboardTiers.ElementAtOrDefault(tierIndex);
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
		if (!m_leaderboardTiers.IsNullOrEmpty())
		{
			TierInfo tierInfo = m_leaderboardTiers.ElementAtOrDefault(tierIndex);
			if (tierInfo != null)
			{
				if (tierInfo.PointType != TierPointsTypes.Limitless)
				{
					if (tierInfo.PointType != TierPointsTypes.DerivedFromMMR)
					{
						goto IL_00a7;
					}
				}
				TierInfo tierInfo2 = m_leaderboardTiers.ElementAtOrDefault(tierIndex + 1);
				if (tierInfo2 != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
						{
							int result;
							if (tierInfo2.PointType != TierPointsTypes.Limitless)
							{
								result = ((tierInfo2.PointType == TierPointsTypes.DerivedFromMMR) ? 1 : 0);
							}
							else
							{
								result = 1;
							}
							return (byte)result != 0;
						}
						}
					}
				}
			}
		}
		goto IL_00a7;
		IL_00a7:
		return false;
	}
}
