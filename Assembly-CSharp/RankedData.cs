using System;
using System.Linq;

[Serializable]
public class RankedData
{
	public string Handle;

	public long AccountId;

	public string EloKeyText;

	public DateTime LastMatch;

	public float Elo;

	public int MatchCount;

	public int Wins;

	public int WinStreak;

	public int Tier;

	public int Points;

	public int DivisionId;

	public int SavedTier;

	public int SavedDivisionId;

	public int YesterdaysTier;

	public int YesterdaysPoints;

	public DateTime LastDecay;

	public override string ToString()
	{
		return $"{Handle}/{EloKeyText}/T{Tier}#{DivisionId}/P{Points}";
	}

	public int GetPlayerFacingTier(MatchmakingQueueConfig matchmakingConfig)
	{
		if (MatchCount >= matchmakingConfig.LeaderboardMinMatchesForRanking)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return Tier;
				}
			}
		}
		return -1;
	}

	public float GetPlayerFacingTierPoints(MatchmakingQueueConfig matchmakingConfig)
	{
		if (MatchCount < matchmakingConfig.LeaderboardMinMatchesForRanking)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return -1f;
				}
			}
		}
		object obj;
		if (matchmakingConfig.LeaderboardTiers.IsNullOrEmpty())
		{
			obj = null;
		}
		else
		{
			obj = matchmakingConfig.LeaderboardTiers.ElementAtOrDefault(Tier - 1);
		}
		TierInfo tierInfo = (TierInfo)obj;
		if (tierInfo != null)
		{
			if (tierInfo.PointType == TierPointsTypes.DerivedFromMMR)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return Elo;
					}
				}
			}
		}
		return Points;
	}

	private void ApplyDecayInternal(IDecayInfo decayInfo, int maxDecent)
	{
		maxDecent--;
		if (maxDecent <= 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					Log.Warning("Stopping ranked decay processing on {0}", ToString());
					return;
				}
			}
		}
		if (!decayInfo.GetDecayAmount(Tier - 1, out int amount, out TimeSpan start))
		{
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		TimeSpan t = decayInfo.UtcNow - (LastMatch + start);
		if (!(t > TimeSpan.Zero))
		{
			return;
		}
		while (true)
		{
			int num = (int)Math.Ceiling(t.TotalDays);
			TimeSpan t2 = LastDecay - (LastMatch + start);
			int num2 = (t2 > TimeSpan.Zero) ? ((int)Math.Ceiling(t2.TotalDays)) : 0;
			int num3 = num - num2;
			int val = 1 + (int)Math.Floor((float)Points / (float)amount);
			if (decayInfo.DoesTierHaveLimitlessLesserNeighborTier(Tier - 1))
			{
				Tier++;
				val = 1;
			}
			int num4 = Math.Min(num3, val);
			num3 -= num4;
			Points -= amount * num4;
			LastDecay = decayInfo.UtcNow;
			if (Points < 0)
			{
				Tier++;
				Points += 100;
				if (Tier == SavedTier)
				{
					int savedDivisionId = SavedDivisionId;
					SavedDivisionId = DivisionId;
					DivisionId = savedDivisionId;
					SavedDivisionId = Tier + 1;
				}
			}
			if (num3 > 0)
			{
				LastDecay -= TimeSpan.FromDays(num3);
				if (decayInfo.GetDecayAmount(Tier - 1, out int _, out TimeSpan _))
				{
					while (true)
					{
						ApplyDecayInternal(decayInfo, maxDecent);
						return;
					}
				}
				return;
			}
			return;
		}
	}

	public void ApplyDecay(IDecayInfo decayInfo)
	{
		if (decayInfo == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					throw new Exception("IMPLEMENT PLEASE");
				}
			}
		}
		if (!decayInfo.IsActive)
		{
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		ApplyDecayInternal(decayInfo, 10);
	}
}
