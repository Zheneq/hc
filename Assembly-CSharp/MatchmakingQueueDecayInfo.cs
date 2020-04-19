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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MatchmakingQueueDecayInfo.GetDecayAmount(int, int*, TimeSpan*)).MethodHandle;
			}
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MatchmakingQueueDecayInfo.DoesTierHaveLimitlessLesserNeighborTier(int)).MethodHandle;
			}
			TierInfo tierInfo = this.m_leaderboardTiers.ElementAtOrDefault(tierIndex);
			if (tierInfo != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (tierInfo.PointType != TierPointsTypes.Limitless)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (tierInfo.PointType != TierPointsTypes.DerivedFromMMR)
					{
						return false;
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				TierInfo tierInfo2 = this.m_leaderboardTiers.ElementAtOrDefault(tierIndex + 1);
				if (tierInfo2 != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					bool result;
					if (tierInfo2.PointType != TierPointsTypes.Limitless)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
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
