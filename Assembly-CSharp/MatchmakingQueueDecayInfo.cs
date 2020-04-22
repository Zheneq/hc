using System;
using System.Collections.Generic;
using System.Linq;

internal class MatchmakingQueueDecayInfo : IDecayInfo
{
	public bool IsActive => !m_leaderboardTiers.IsNullOrEmpty();

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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			TierInfo tierInfo = m_leaderboardTiers.ElementAtOrDefault(tierIndex);
			if (tierInfo != null)
			{
				while (true)
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
					while (true)
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
						goto IL_00a7;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
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
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
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
