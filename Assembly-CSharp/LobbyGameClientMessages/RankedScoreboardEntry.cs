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
					RuntimeMethodHandle runtimeMethodHandle = methodof(RankedScoreboardEntry.CompareTo(RankedScoreboardEntry)).MethodHandle;
				}
				int result;
				if (flag)
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
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				int result2;
				if (flag4)
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
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.Tier != other.Tier)
				{
					int result3;
					if (this.Tier < other.Tier)
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
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				int result4;
				if (this.TierPoints > other.TierPoints)
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				int result5;
				if (this.WinStreak > other.WinStreak)
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				int result6;
				if (this.WinCount > other.WinCount)
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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				int result7;
				if (this.MatchCount < other.MatchCount)
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

		public static RankedScoreboardEntry? \u001D(MatchmakingQueueConfig \u001D, RankedData \u000E)
		{
			if (\u000E != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(RankedScoreboardEntry.\u001D(MatchmakingQueueConfig, RankedData)).MethodHandle;
				}
				return new RankedScoreboardEntry?(new RankedScoreboardEntry
				{
					Handle = \u000E.Handle,
					Tier = \u000E.GetPlayerFacingTier(\u001D),
					AccountID = \u000E.AccountId,
					TierPoints = \u000E.GetPlayerFacingTierPoints(\u001D),
					InstanceId = \u000E.DivisionId,
					LastMatch = \u000E.LastMatch,
					WinCount = \u000E.Wins,
					WinStreak = \u000E.WinStreak,
					MatchCount = \u000E.MatchCount,
					YesterdaysPoints = \u000E.YesterdaysPoints,
					YesterdaysTier = \u000E.YesterdaysTier
				});
			}
			return null;
		}
	}
}
