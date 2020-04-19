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
		return string.Format("{0}/{1}/T{2}#{3}/P{4}", new object[]
		{
			this.Handle,
			this.EloKeyText,
			this.Tier,
			this.DivisionId,
			this.Points
		});
	}

	public int GetPlayerFacingTier(MatchmakingQueueConfig matchmakingConfig)
	{
		if (this.MatchCount >= matchmakingConfig.LeaderboardMinMatchesForRanking)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RankedData.GetPlayerFacingTier(MatchmakingQueueConfig)).MethodHandle;
			}
			return this.Tier;
		}
		return -1;
	}

	public float GetPlayerFacingTierPoints(MatchmakingQueueConfig matchmakingConfig)
	{
		if (this.MatchCount < matchmakingConfig.LeaderboardMinMatchesForRanking)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RankedData.GetPlayerFacingTierPoints(MatchmakingQueueConfig)).MethodHandle;
			}
			return -1f;
		}
		TierInfo tierInfo;
		if (matchmakingConfig.LeaderboardTiers.IsNullOrEmpty<TierInfo>())
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
			tierInfo = null;
		}
		else
		{
			tierInfo = matchmakingConfig.LeaderboardTiers.ElementAtOrDefault(this.Tier - 1);
		}
		TierInfo tierInfo2 = tierInfo;
		if (tierInfo2 != null)
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
			if (tierInfo2.PointType == TierPointsTypes.DerivedFromMMR)
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
				return this.Elo;
			}
		}
		return (float)this.Points;
	}

	private void ApplyDecayInternal(IDecayInfo decayInfo, int maxDecent)
	{
		maxDecent--;
		if (maxDecent <= 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RankedData.ApplyDecayInternal(IDecayInfo, int)).MethodHandle;
			}
			Log.Warning("Stopping ranked decay processing on {0}", new object[]
			{
				this.ToString()
			});
			return;
		}
		int num;
		TimeSpan t;
		if (!decayInfo.GetDecayAmount(this.Tier - 1, out num, out t))
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
			return;
		}
		TimeSpan t2 = decayInfo.UtcNow - (this.LastMatch + t);
		if (t2 > TimeSpan.Zero)
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
			int num2 = (int)Math.Ceiling(t2.TotalDays);
			TimeSpan t3 = this.LastDecay - (this.LastMatch + t);
			int num3 = (!(t3 > TimeSpan.Zero)) ? 0 : ((int)Math.Ceiling(t3.TotalDays));
			int num4 = num2 - num3;
			int val = 1 + (int)Math.Floor((double)((float)this.Points / (float)num));
			if (decayInfo.DoesTierHaveLimitlessLesserNeighborTier(this.Tier - 1))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				this.Tier++;
				val = 1;
			}
			int num5 = Math.Min(num4, val);
			num4 -= num5;
			this.Points -= num * num5;
			this.LastDecay = decayInfo.UtcNow;
			if (this.Points < 0)
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
				this.Tier++;
				this.Points += 0x64;
				if (this.Tier == this.SavedTier)
				{
					int savedDivisionId = this.SavedDivisionId;
					this.SavedDivisionId = this.DivisionId;
					this.DivisionId = savedDivisionId;
					this.SavedDivisionId = this.Tier + 1;
				}
			}
			if (num4 > 0)
			{
				this.LastDecay -= TimeSpan.FromDays((double)num4);
				int num6;
				TimeSpan timeSpan;
				if (decayInfo.GetDecayAmount(this.Tier - 1, out num6, out timeSpan))
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
					this.ApplyDecayInternal(decayInfo, maxDecent);
				}
			}
		}
	}

	public void ApplyDecay(IDecayInfo decayInfo)
	{
		if (decayInfo == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RankedData.ApplyDecay(IDecayInfo)).MethodHandle;
			}
			throw new Exception("IMPLEMENT PLEASE");
		}
		if (!decayInfo.IsActive)
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
			return;
		}
		this.ApplyDecayInternal(decayInfo, 0xA);
	}
}
