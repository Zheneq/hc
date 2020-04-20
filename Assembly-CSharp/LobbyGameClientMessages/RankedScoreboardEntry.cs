using System;

namespace LobbyGameClientMessages
{
	[Serializable]
	public struct RankedScoreboardEntry : IComparable<RankedScoreboardEntry>
	{
		public string Handle;

		public DateTime LastMatch;

		public long AccountID;

		public int Tier;

		public float TierPoints;

		public int WinCount;

		public int WinStreak;

		public int MatchCount;

		public int InstanceId;

		public int YesterdaysTier;

		public int YesterdaysPoints;

		public int CompareTo(RankedScoreboardEntry other)
		{
			bool flag = this.Tier <= 0;
			bool flag2 = other.Tier <= 0;
			if (flag != flag2)
			{
				int result;
				if (flag)
				{
					result = 1;
				}
				else
				{
					result = -1;
				}
				return result;
			}
			bool flag3;
			if (this.Tier != 1)
			{
				flag3 = (this.Tier == 2);
			}
			else
			{
				flag3 = true;
			}
			bool flag4 = flag3;
			bool flag5 = other.Tier == 1 || other.Tier == 2;
			if (flag4 != flag5)
			{
				int result2;
				if (flag4)
				{
					result2 = -1;
				}
				else
				{
					result2 = 1;
				}
				return result2;
			}
			if (!flag4)
			{
				if (this.Tier != other.Tier)
				{
					int result3;
					if (this.Tier < other.Tier)
					{
						result3 = -1;
					}
					else
					{
						result3 = 1;
					}
					return result3;
				}
			}
			if (this.TierPoints != other.TierPoints)
			{
				int result4;
				if (this.TierPoints > other.TierPoints)
				{
					result4 = -1;
				}
				else
				{
					result4 = 1;
				}
				return result4;
			}
			if (this.WinStreak != other.WinStreak)
			{
				int result5;
				if (this.WinStreak > other.WinStreak)
				{
					result5 = -1;
				}
				else
				{
					result5 = 1;
				}
				return result5;
			}
			if (this.WinCount != other.WinCount)
			{
				int result6;
				if (this.WinCount > other.WinCount)
				{
					result6 = -1;
				}
				else
				{
					result6 = 1;
				}
				return result6;
			}
			if (this.MatchCount != other.MatchCount)
			{
				int result7;
				if (this.MatchCount < other.MatchCount)
				{
					result7 = -1;
				}
				else
				{
					result7 = 1;
				}
				return result7;
			}
			return other.LastMatch.CompareTo(this.LastMatch);
		}

		public static RankedScoreboardEntry? symbol_001D(MatchmakingQueueConfig symbol_001D, RankedData symbol_000E)
		{
			if (symbol_000E != null)
			{
				return new RankedScoreboardEntry?(new RankedScoreboardEntry
				{
					Handle = symbol_000E.Handle,
					Tier = symbol_000E.GetPlayerFacingTier(symbol_001D),
					AccountID = symbol_000E.AccountId,
					TierPoints = symbol_000E.GetPlayerFacingTierPoints(symbol_001D),
					InstanceId = symbol_000E.DivisionId,
					LastMatch = symbol_000E.LastMatch,
					WinCount = symbol_000E.Wins,
					WinStreak = symbol_000E.WinStreak,
					MatchCount = symbol_000E.MatchCount,
					YesterdaysPoints = symbol_000E.YesterdaysPoints,
					YesterdaysTier = symbol_000E.YesterdaysTier
				});
			}
			return null;
		}
	}
}
