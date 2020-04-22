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
			bool flag = Tier <= 0;
			bool flag2 = other.Tier <= 0;
			if (flag != flag2)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						int result;
						if (flag)
						{
							while (true)
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
					}
				}
			}
			int num;
			if (Tier != 1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num = ((Tier == 2) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag3 = (byte)num != 0;
			bool flag4 = other.Tier == 1 || other.Tier == 2;
			if (flag3 != flag4)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						int result2;
						if (flag3)
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
							result2 = -1;
						}
						else
						{
							result2 = 1;
						}
						return result2;
					}
					}
				}
			}
			if (!flag3)
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
				if (Tier != other.Tier)
				{
					int result3;
					if (Tier < other.Tier)
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
						result3 = -1;
					}
					else
					{
						result3 = 1;
					}
					return result3;
				}
			}
			if (TierPoints != other.TierPoints)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						int result4;
						if (TierPoints > other.TierPoints)
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
							result4 = -1;
						}
						else
						{
							result4 = 1;
						}
						return result4;
					}
					}
				}
			}
			if (WinStreak != other.WinStreak)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						int result5;
						if (WinStreak > other.WinStreak)
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
							result5 = -1;
						}
						else
						{
							result5 = 1;
						}
						return result5;
					}
					}
				}
			}
			if (WinCount != other.WinCount)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						int result6;
						if (WinCount > other.WinCount)
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
							result6 = -1;
						}
						else
						{
							result6 = 1;
						}
						return result6;
					}
					}
				}
			}
			if (MatchCount != other.MatchCount)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						int result7;
						if (MatchCount < other.MatchCount)
						{
							while (true)
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
					}
				}
			}
			return other.LastMatch.CompareTo(LastMatch);
		}

		public static RankedScoreboardEntry? _001D(MatchmakingQueueConfig _001D, RankedData _000E)
		{
			if (_000E != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						RankedScoreboardEntry value = default(RankedScoreboardEntry);
						value.Handle = _000E.Handle;
						value.Tier = _000E.GetPlayerFacingTier(_001D);
						value.AccountID = _000E.AccountId;
						value.TierPoints = _000E.GetPlayerFacingTierPoints(_001D);
						value.InstanceId = _000E.DivisionId;
						value.LastMatch = _000E.LastMatch;
						value.WinCount = _000E.Wins;
						value.WinStreak = _000E.WinStreak;
						value.MatchCount = _000E.MatchCount;
						value.YesterdaysPoints = _000E.YesterdaysPoints;
						value.YesterdaysTier = _000E.YesterdaysTier;
						return value;
					}
					}
				}
			}
			return null;
		}
	}
}
