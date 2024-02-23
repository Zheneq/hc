using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class BestSolutions
{
	public enum GroupBreakage
	{
		NONE,
		ONE_GROUP,
		MULTIPLE_GROUPS
	}

	public enum EloImbalance
	{
		MINOR,
		ACCEPTABLE,
		MAJOR
	}

	public interface ILegalityValidator
	{
		bool IsLegal(Solution s);
	}

	public class Solution : IComparable<Solution>
	{
		private GroupBreakage m_groupBreakage;

		private bool m_useOtherSortingCriteria;

		private bool m_useFineOtherSortingCriteria;

		private bool m_noobCollision;

		private bool m_expertCollision;

		private float m_AvBEloDifference;

		private float m_largestEloWidth;

		private int m_maxGroupSizeImbalance;

		private int m_groupedMemberCountImbalance;

		private int m_botImbalance;

		private int m_loserImbalance;

		private int m_roleImbalance;

		private int m_willFillImbalance;

		private int m_languageCommonality;

		private bool m_languageImbalance;

		private bool m_regionUniqueness;

		private bool m_hasRoleAssassin;

		private bool m_hasRoleTank;

		private bool m_hasRoleSupport;

		private int m_balanceFailureImbalance;

		public List<int> m_sides;

		private bool m_isLegal;

		public float AbsEloDifference
		{
			get { return Math.Abs(m_AvBEloDifference); }
		}

		public float AvBEloDifference
		{
			get { return m_AvBEloDifference; }
		}

		public bool BreaksGroups
		{
			get { return m_groupBreakage != GroupBreakage.NONE; }
		}

		public bool HasNoobCollisions
		{
			get { return m_noobCollision; }
		}

		public bool HasExpertCollisions
		{
			get { return m_expertCollision; }
		}

		public int MaxGroupSizeImbalance
		{
			get { return m_maxGroupSizeImbalance; }
		}

		public int GroupedMemberCountImbalance
		{
			get { return m_groupedMemberCountImbalance; }
		}

		public GroupBreakage GroupBreakage
		{
			get { return m_groupBreakage; }
		}

		public bool HasRoleImbalance
		{
			get { return m_roleImbalance != 0; }
		}

		public bool HasMissingRoles
		{
			get
			{
				int result;
				if (m_hasRoleTank)
				{
					if (m_hasRoleSupport)
					{
						result = ((!m_hasRoleAssassin) ? 1 : 0);
						goto IL_002f;
					}
				}
				result = 1;
				goto IL_002f;
				IL_002f:
				return (byte)result != 0;
			}
		}

		public bool HasMissingRoleAssassin
		{
			get { return !m_hasRoleAssassin; }
		}

		public bool HasMissingRoleTank
		{
			get { return !m_hasRoleTank; }
		}

		public bool HasMissingRoleSupport
		{
			get { return !m_hasRoleSupport; }
		}

		public int RoleCount
		{
			get
			{
				int num;
				if (m_hasRoleAssassin)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				int num2;
				if (m_hasRoleTank)
				{
					num2 = 1;
				}
				else
				{
					num2 = 0;
				}
				int num3 = num + num2;
				int num4;
				if (m_hasRoleSupport)
				{
					num4 = 1;
				}
				else
				{
					num4 = 0;
				}
				return num3 + num4;
			}
		}

		public bool HasWillFillImbalance
		{
			get { return m_willFillImbalance != 0; }
		}

		public bool HasBotImbalance
		{
			get { return m_botImbalance > 1; }
		}

		public bool HasLoserImbalance
		{
			get { return m_loserImbalance > 1; }
		}

		public bool HasRegionUniqueness
		{
			get { return m_regionUniqueness; }
		}

		public bool HasLanguageImbalance
		{
			get { return m_languageImbalance; }
		}

		public int LanguageCommonality
		{
			get { return m_languageCommonality; }
		}

		public BalanceResults.BalanceFailureLevel BalanceFailureImbalance
		{
			get
			{
				int result;
				if (m_balanceFailureImbalance == 0)
				{
					result = 0;
				}
				else if (m_balanceFailureImbalance != 1)
				{
					result = ((m_balanceFailureImbalance != 2) ? 3 : 2);
				}
				else
				{
					result = 1;
				}
				return (BalanceResults.BalanceFailureLevel)result;
			}
		}

		public EloImbalance EloImbalance
		{
			get
			{
				int result;
				if (m_useFineOtherSortingCriteria)
				{
					result = 0;
				}
				else if (m_useOtherSortingCriteria)
				{
					result = 1;
				}
				else
				{
					result = 2;
				}
				return (EloImbalance)result;
			}
		}

		public Solution(float aVbEloDifference, GroupBreakage groupBreakage, List<int> sides, float maxDifferenceForOtherSortingMethods, int maxGroupSizeImbalance, int groupedMemberCountImbalance, int botImbalance, int loserImbalance, int roleImbalance, bool hasAssassin, bool hasTank, bool hasSupport, bool hasNoobCollision, bool hasExpertCollision, float largestEloWidth, int willFillImbalance, bool regionUniqueness, int languageCommonality, int languageTeamAAdvantage)
		{
			m_groupBreakage = groupBreakage;
			m_AvBEloDifference = aVbEloDifference;
			m_largestEloWidth = largestEloWidth;
			float num = Math.Abs(aVbEloDifference);
			m_useOtherSortingCriteria = (num < maxDifferenceForOtherSortingMethods);
			m_useFineOtherSortingCriteria = (num < maxDifferenceForOtherSortingMethods / 2f);
			m_sides = new List<int>(sides);
			m_maxGroupSizeImbalance = Math.Abs(maxGroupSizeImbalance);
			m_groupedMemberCountImbalance = Math.Abs(groupedMemberCountImbalance);
			m_botImbalance = Math.Abs(botImbalance);
			m_loserImbalance = Math.Abs(loserImbalance);
			m_roleImbalance = Math.Abs(roleImbalance);
			m_willFillImbalance = Math.Abs(willFillImbalance);
			m_hasRoleAssassin = hasAssassin;
			m_hasRoleTank = hasTank;
			m_hasRoleSupport = hasSupport;
			m_noobCollision = hasNoobCollision;
			m_expertCollision = hasExpertCollision;
			int num2 = 0;
			if (maxGroupSizeImbalance != 0)
			{
				int num3 = num2;
				int num4;
				if (maxGroupSizeImbalance > 0)
				{
					num4 = 1;
				}
				else
				{
					num4 = -1;
				}
				num2 = num3 + num4;
			}
			else if (groupedMemberCountImbalance != 0)
			{
				num2 += ((groupedMemberCountImbalance > 0) ? 1 : (-1));
			}
			if (roleImbalance != 0)
			{
				int num5 = num2;
				int num6;
				if (roleImbalance > 0)
				{
					num6 = 1;
				}
				else
				{
					num6 = -1;
				}
				num2 = num5 + num6;
			}
			if (willFillImbalance != 0)
			{
				num2 += ((willFillImbalance > 0) ? 1 : (-1));
			}
			if (botImbalance != 0)
			{
				int num7 = num2;
				int num8;
				if (botImbalance > 0)
				{
					num8 = 1;
				}
				else
				{
					num8 = -1;
				}
				num2 = num7 + num8;
			}
			if (!m_useFineOtherSortingCriteria)
			{
				num2 += ((aVbEloDifference > 0f) ? 1 : (-1));
				if (!m_useOtherSortingCriteria)
				{
					int num9 = num2;
					int num10;
					if (aVbEloDifference > 0f)
					{
						num10 = 1;
					}
					else
					{
						num10 = -1;
					}
					num2 = num9 + num10;
				}
			}
			m_balanceFailureImbalance = Math.Abs(num2);
			m_regionUniqueness = regionUniqueness;
			m_languageCommonality = languageCommonality;
			if (languageTeamAAdvantage > 0)
			{
				num2++;
				m_languageImbalance = true;
			}
			else if (languageTeamAAdvantage < 0)
			{
				num2--;
				m_languageImbalance = true;
			}
			else
			{
				m_languageImbalance = false;
			}
		}

		internal string GetLogString()
		{
			Func<bool, string, string, string> func = (bool isTrue, string writeText, string appendText) => isTrue ? string.Format("{0}{1}{2}", writeText, (writeText.Count() <= 0) ? string.Empty : "+", appendText) : writeText;
			string empty = string.Empty;
			empty = func(m_noobCollision, empty, "noob");
			empty = func(m_expertCollision, empty, "expert");
			if (empty.Count() > 0)
			{
				empty = new StringBuilder().Append(", ").Append(empty).Append(" collision").ToString();
			}
			string empty2 = string.Empty;
			empty2 = func(m_botImbalance != 0, empty2, "bot");
			empty2 = func(m_maxGroupSizeImbalance != 0, empty2, "group");
			bool arg = m_loserImbalance != 0;
			string arg2 = empty2;
			object arg3;
			if (m_loserImbalance > 1)
			{
				arg3 = new StringBuilder().Append("loser(").Append(m_loserImbalance).Append(")").ToString();
			}
			else
			{
				arg3 = "loser";
			}
			empty2 = func(arg, arg2, (string)arg3);
			empty2 = func(m_roleImbalance != 0, empty2, "role");
			empty2 = func(m_willFillImbalance != 0, empty2, "fill");
			empty2 = func(m_languageImbalance, empty2, "lang");
			empty2 = func(m_balanceFailureImbalance != 0, empty2, new StringBuilder().Append("unfair").Append(m_balanceFailureImbalance).ToString());
			if (empty2.Count() > 0)
			{
				empty2 = new StringBuilder().Append(", ").Append(empty2).Append(" imbalance").ToString();
			}
			string text;
			if (!m_useFineOtherSortingCriteria)
			{
				text = new StringBuilder().Append("eloDiff=").Append(AbsEloDifference).ToString();
			}
			else if (m_largestEloWidth > AbsEloDifference)
			{
				text = new StringBuilder().Append("width=").Append(m_largestEloWidth).Append(", eloDiff=").Append(AbsEloDifference).ToString();
			}
			else
			{
				text = new StringBuilder().Append("eloDiff=").Append(AbsEloDifference).Append(", width=").Append(m_largestEloWidth).ToString();
			}
			object[] obj = new object[9]
			{
				(!m_useOtherSortingCriteria) ? "ELO Only" : ((!m_useFineOtherSortingCriteria) ? "flexible" : "fine"),
				null,
				null,
				null,
				null,
				null,
				null,
				null,
				null
			};
			string text2;
			if (m_groupBreakage == GroupBreakage.NONE)
			{
				text2 = string.Empty;
			}
			else
			{
				text2 = new StringBuilder().Append(", ").Append(m_groupBreakage).ToString();
			}
			obj[1] = text2;
			obj[2] = empty;
			obj[3] = empty2;
			object obj2;
			if (m_hasRoleAssassin)
			{
				obj2 = string.Empty;
			}
			else
			{
				obj2 = ", no assassin";
			}
			obj[4] = obj2;
			object obj3;
			if (m_hasRoleTank)
			{
				obj3 = string.Empty;
			}
			else
			{
				obj3 = ", no tank";
			}
			obj[5] = obj3;
			object obj4;
			if (m_hasRoleSupport)
			{
				obj4 = string.Empty;
			}
			else
			{
				obj4 = ", no support";
			}
			obj[6] = obj4;
			obj[7] = text;
			obj[8] = m_languageCommonality;
			return string.Format("solution({0}{1}{2}{3}{4}{5}{6}, {7}, {8} lang)", obj);
		}

		internal void SetLegality(ILegalityValidator validator)
		{
			m_isLegal = validator.IsLegal(this);
		}

		public int CoarseCompareTo(Solution other)
		{
			if (m_isLegal != other.m_isLegal)
			{
				int result;
				if (m_isLegal)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
				return result;
			}
			bool flag = m_groupBreakage == GroupBreakage.MULTIPLE_GROUPS;
			bool flag2 = other.m_groupBreakage == GroupBreakage.MULTIPLE_GROUPS;
			if (flag != flag2)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						int result2;
						if (flag)
						{
							result2 = 1;
						}
						else
						{
							result2 = -1;
						}
						return result2;
					}
					}
				}
			}
			bool flag3 = m_groupBreakage == GroupBreakage.ONE_GROUP;
			bool flag4 = other.m_groupBreakage == GroupBreakage.ONE_GROUP;
			if (flag3 != flag4)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						int result3;
						if (flag3)
						{
							result3 = 1;
						}
						else
						{
							result3 = -1;
						}
						return result3;
					}
					}
				}
			}
			if (m_useOtherSortingCriteria != other.m_useOtherSortingCriteria)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						int result4;
						if (m_useOtherSortingCriteria)
						{
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
			if (m_useOtherSortingCriteria)
			{
				if (m_balanceFailureImbalance != other.m_balanceFailureImbalance)
				{
					while (true)
					{
						return (m_balanceFailureImbalance > other.m_balanceFailureImbalance) ? 1 : (-1);
					}
				}
				if (m_botImbalance != other.m_botImbalance)
				{
					int result5;
					if (m_botImbalance > other.m_botImbalance)
					{
						result5 = 1;
					}
					else
					{
						result5 = -1;
					}
					return result5;
				}
				if (m_maxGroupSizeImbalance != other.m_maxGroupSizeImbalance)
				{
					while (true)
					{
						int result6;
						if (m_maxGroupSizeImbalance > other.m_maxGroupSizeImbalance)
						{
							result6 = 1;
						}
						else
						{
							result6 = -1;
						}
						return result6;
					}
				}
				if (m_noobCollision != other.m_noobCollision)
				{
					while (true)
					{
						int result7;
						if (m_noobCollision)
						{
							result7 = 1;
						}
						else
						{
							result7 = -1;
						}
						return result7;
					}
				}
				if (m_willFillImbalance != other.m_willFillImbalance)
				{
					int result8;
					if (m_willFillImbalance > other.m_willFillImbalance)
					{
						result8 = 1;
					}
					else
					{
						result8 = -1;
					}
					return result8;
				}
				if (m_roleImbalance != other.m_roleImbalance)
				{
					while (true)
					{
						int result9;
						if (m_roleImbalance > other.m_roleImbalance)
						{
							result9 = 1;
						}
						else
						{
							result9 = -1;
						}
						return result9;
					}
				}
				int roleCount = RoleCount;
				int roleCount2 = other.RoleCount;
				if (roleCount != roleCount2)
				{
					while (true)
					{
						int result10;
						if (roleCount > roleCount2)
						{
							result10 = -1;
						}
						else
						{
							result10 = 1;
						}
						return result10;
					}
				}
				if (m_expertCollision != other.m_expertCollision)
				{
					int result11;
					if (m_expertCollision)
					{
						result11 = 1;
					}
					else
					{
						result11 = -1;
					}
					return result11;
				}
				if (m_languageCommonality != other.m_languageCommonality)
				{
					while (true)
					{
						int result12;
						if (m_languageCommonality > other.m_languageCommonality)
						{
							result12 = -1;
						}
						else
						{
							result12 = 1;
						}
						return result12;
					}
				}
				if (m_languageImbalance != other.m_languageImbalance)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
						{
							int result13;
							if (m_languageImbalance)
							{
								result13 = 1;
							}
							else
							{
								result13 = -1;
							}
							return result13;
						}
						}
					}
				}
			}
			return 0;
		}

		public int CompareTo(Solution other)
		{
			int num = CoarseCompareTo(other);
			if (num != 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return num;
					}
				}
			}
			if (m_useOtherSortingCriteria)
			{
				if (m_loserImbalance != other.m_loserImbalance)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							int result;
							if (m_loserImbalance > other.m_loserImbalance)
							{
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
			}
			if (m_useFineOtherSortingCriteria != other.m_useFineOtherSortingCriteria)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
					{
						int result2;
						if (m_useFineOtherSortingCriteria)
						{
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
			if (m_useFineOtherSortingCriteria)
			{
				float num2 = Math.Max(m_largestEloWidth, AbsEloDifference);
				float num3 = Math.Max(other.m_largestEloWidth, other.AbsEloDifference);
				if (num2 != num3)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
						{
							int result3;
							if (num2 < num3)
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
					}
				}
			}
			float absEloDifference = AbsEloDifference;
			float absEloDifference2 = other.AbsEloDifference;
			if (absEloDifference != absEloDifference2)
			{
				int result4;
				if (absEloDifference < absEloDifference2)
				{
					result4 = -1;
				}
				else
				{
					result4 = 1;
				}
				return result4;
			}
			return 0;
		}
	}

	private class ExcuseBucket : IEqualityComparer<ExcuseBucket>
	{
		public long m_id;

		public bool m_isGroup;

		public ExcuseBucket(int playerId, long groupId)
		{
			if (groupId != 0)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_id = groupId;
						m_isGroup = true;
						return;
					}
				}
			}
			m_id = playerId;
			m_isGroup = false;
		}

		public bool Equals(ExcuseBucket x, ExcuseBucket y)
		{
			int result;
			if (x.m_id == y.m_id)
			{
				result = ((x.m_isGroup == y.m_isGroup) ? 1 : 0);
			}
			else
			{
				result = 0;
			}
			return (byte)result != 0;
		}

		public int GetHashCode(ExcuseBucket x)
		{
			return m_isGroup.GetHashCode() ^ m_id.GetHashCode();
		}

		public static int ComputeRolesPresent(int playersOnTeam, Dictionary<ExcuseBucket, List<CharacterRole>> teamRoles)
		{
			int num = Math.Max(0, playersOnTeam - 3);
			int num2 = 0;
			HashSet<CharacterRole> hashSet = new HashSet<CharacterRole>();
			foreach (KeyValuePair<ExcuseBucket, List<CharacterRole>> teamRole in teamRoles)
			{
				List<CharacterRole> roles = teamRole.Value;
				roles.Sort();
				for (int num3 = num; num3 > 0; num3--)
				{
					if (!RolesHaveDups(roles))
					{
						break;
					}
					RemoveOneDup(ref roles);
				}
				for (int i = 0; i < roles.Count(); i++)
				{
					if (roles[i] == CharacterRole.None)
					{
						num2++;
					}
					else
					{
						if (i > 0)
						{
							if (roles[i - 1] == roles[i])
							{
								num2++;
								continue;
							}
						}
						hashSet.Add(roles[i]);
					}
				}
			}
			return Math.Min(hashSet.Count() + num2, 3);
		}

		private static bool RolesHaveDups(List<CharacterRole> roles)
		{
			for (int i = 1; i < roles.Count(); i++)
			{
				if (roles[i - 1] != roles[i])
				{
					continue;
				}
				while (true)
				{
					return true;
				}
			}
			return false;
		}

		private static void RemoveOneDup(ref List<CharacterRole> roles)
		{
			for (int i = 1; i < roles.Count(); i++)
			{
				if (roles[i - 1] != roles[i])
				{
					continue;
				}
				while (true)
				{
					roles.RemoveAt(i - 1);
					return;
				}
			}
			while (true)
			{
				throw new Exception("No dup in list that should have dup");
			}
		}
	}

	public class BalanceResults : IComparable<BalanceResults>
	{
		public enum BalanceFailureLevel
		{
			Fair,
			Minor,
			Major,
			Tragic
		}

		public GroupBreakage GroupBreakage
		{
			get { return (m_solution != null) ? m_solution.GroupBreakage : GroupBreakage.NONE; }
		}

		public EloImbalance EloImbalance
		{
			get { return (m_solution != null) ? m_solution.EloImbalance : EloImbalance.MINOR; }
		}

		public BalanceFailureLevel BalanceFailureImbalance
		{
			get { return (m_solution != null) ? m_solution.BalanceFailureImbalance : BalanceFailureLevel.Fair; }
		}

		public int GroupImbalanceAmount
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = m_solution.MaxGroupSizeImbalance;
				}
				return result;
			}
		}

		public bool WillFillImbalance
		{
			get { return m_solution != null && m_solution.HasWillFillImbalance; }
		}

		public bool BotImbalance
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = (m_solution.HasBotImbalance ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public bool LoserImbalance
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = (m_solution.HasLoserImbalance ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public bool RoleImbalance
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = (m_solution.HasRoleImbalance ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public bool MissingRoles
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = (m_solution.HasMissingRoles ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public bool HasMissingRoleAssassin
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = (m_solution.HasMissingRoleAssassin ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public bool HasMissingRoleTank
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = (m_solution.HasMissingRoleTank ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public bool HasMissingRoleSupport
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = (m_solution.HasMissingRoleSupport ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public int LanguageCompatability
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = m_solution.LanguageCommonality;
				}
				return result;
			}
		}

		public bool LanguageImbalance
		{
			get { return m_solution == null || m_solution.HasLanguageImbalance; }
		}

		public bool RegionUniqueness
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 1;
				}
				else
				{
					result = (m_solution.HasRegionUniqueness ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public bool NoobCollision
		{
			get { return m_solution != null && m_solution.HasNoobCollisions; }
		}

		public bool ExpertCollision
		{
			get
			{
				int result;
				if (m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = (m_solution.HasExpertCollisions ? 1 : 0);
				}
				return (byte)result != 0;
			}
		}

		public float AvBEloDifference
		{
			get
			{
				float result;
				if (m_solution == null)
				{
					result = 0f;
				}
				else
				{
					result = m_solution.AvBEloDifference;
				}
				return result;
			}
		}

		public float AbsEloDifference
		{
			get
			{
				float result;
				if (m_solution == null)
				{
					result = 0f;
				}
				else
				{
					result = m_solution.AbsEloDifference;
				}
				return result;
			}
		}

		private Solution m_solution
		{
			get;
			set;
		}

		private double m_rnd
		{
			get;
			set;
		}

		public bool Success
		{
			get;
			set;
		}

		public LocalizationPayload Error
		{
			get;
			internal set;
		}

		public string SolutionScoreAsString
		{
			get
			{
				object result;
				if (m_solution == null)
				{
					result = "<null>";
				}
				else
				{
					result = m_solution.GetLogString();
				}
				return (string)result;
			}
		}

		public string Status
		{
			get
			{
				string retVal = string.Empty;
				Action<bool, string> action = delegate(bool isTrue, string appendText)
				{
					if (isTrue)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								retVal = string.Format("{0}{1}{2}", retVal, (retVal.Count() <= 0) ? string.Empty : "+", appendText);
								return;
							}
						}
					}
				};
				action(ExpertCollision, "collideE");
				action(NoobCollision, "collideN");
				action(GroupImbalanceAmount > 0, "imbalGp");
				action(BotImbalance, "imbalBt");
				action(LoserImbalance, "imbalLs");
				action(LanguageImbalance, "imbalLng");
				action(RoleImbalance, "imbalRl");
				action(WillFillImbalance, "imbalWf");
				action(MissingRoles, "missingRoles");
				action(GroupBreakage == GroupBreakage.ONE_GROUP, "breakO");
				action(GroupBreakage == GroupBreakage.MULTIPLE_GROUPS, "breakM");
				action(EloImbalance == EloImbalance.ACCEPTABLE, "eloT");
				action(EloImbalance == EloImbalance.MAJOR, "eloM");
				object result;
				if (retVal == string.Empty)
				{
					result = "none";
				}
				else
				{
					result = retVal;
				}
				return (string)result;
			}
		}

		public BalanceResults(double rnd)
		{
			m_solution = null;
			m_rnd = rnd;
			Success = false;
		}

		public BalanceResults(Solution solution, double rnd)
		{
			m_solution = solution;
			m_rnd = rnd;
			Success = true;
		}

		public static BalanceResults MakeError(string term, string context)
		{
			BalanceResults balanceResults = new BalanceResults(0.0);
			balanceResults.Error = LocalizationPayload.Create(term, context);
			balanceResults.Success = false;
			return balanceResults;
		}

		public int ConsolidatedCompareTo(BalanceResults other)
		{
			if (Success != other.Success)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						int result;
						if (Success)
						{
							result = -1;
						}
						else
						{
							result = 1;
						}
						return result;
					}
					}
				}
			}
			if (m_solution == null != (other.m_solution == null))
			{
				int result2;
				if (m_solution != null)
				{
					result2 = -1;
				}
				else
				{
					result2 = 1;
				}
				return result2;
			}
			return m_rnd.CompareTo(other.m_rnd);
		}

		public int CompareTo(BalanceResults other)
		{
			if (Success != other.Success)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						int result;
						if (Success)
						{
							result = -1;
						}
						else
						{
							result = 1;
						}
						return result;
					}
					}
				}
			}
			if (m_solution == null != (other.m_solution == null))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return (m_solution == null) ? 1 : (-1);
					}
				}
			}
			if (m_solution != null)
			{
				int num = m_solution.CoarseCompareTo(other.m_solution);
				if (num != 0)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return num;
						}
					}
				}
			}
			return m_rnd.CompareTo(other.m_rnd);
		}
	}

	internal const int c_numberOfUniqueRoles = 3;

	private List<Solution> m_solutions = new List<Solution>();

	internal FreelancerDuplicationRuleTypes m_duplicationRule;

	private bool NoDupInGame
	{
		get { return m_duplicationRule == FreelancerDuplicationRuleTypes.noneInGame; }
	}

	private bool NoDupInTeam
	{
		get
		{
			if (m_duplicationRule == FreelancerDuplicationRuleTypes.byGameType)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						throw new Exception("Must have resolved duplication rule before using BestSolutions.Resolve");
					}
				}
			}
			int result;
			if (m_duplicationRule != FreelancerDuplicationRuleTypes.noneInGame)
			{
				result = ((m_duplicationRule == FreelancerDuplicationRuleTypes.noneInTeam) ? 1 : 0);
			}
			else
			{
				result = 1;
			}
			return (byte)result != 0;
		}
	}

	public void Register(List<int> potentialTeam, List<ELOProvider> players, float aVbEloDifference, float maxTeamDifference, ILegalityValidator legalityValidator)
	{
		bool hasNoobCollision = false;
		bool hasExpertCollision = false;
		int num = 0;
		int num2 = 0;
		Dictionary<long, int> dictionary = new Dictionary<long, int>();
		Dictionary<long, int> dictionary2 = new Dictionary<long, int>();
		Dictionary<long, ushort> dictionary3 = new Dictionary<long, ushort>();
		Dictionary<CharacterType, bool> dictionary4 = new Dictionary<CharacterType, bool>();
		Dictionary<CharacterType, bool> dictionary5 = new Dictionary<CharacterType, bool>();
		Dictionary<string, int> dictionary6 = new Dictionary<string, int>();
		Dictionary<string, int> dictionary7 = new Dictionary<string, int>();
		Region? region = null;
		bool flag = true;
		int num3 = 0;
		int num4 = 0;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		Dictionary<ExcuseBucket, List<CharacterRole>> dictionary8 = new Dictionary<ExcuseBucket, List<CharacterRole>>();
		Dictionary<ExcuseBucket, List<CharacterRole>> dictionary9 = new Dictionary<ExcuseBucket, List<CharacterRole>>();
		List<float> list = new List<float>();
		List<float> list2 = new List<float>();
		for (int num5 = 0; num5 < players.Count; num5++)
		{
			CharacterType selectedCharacter = players[num5].SelectedCharacter;
			long groupId = players[num5].GroupId;
			bool isCollisionNoob = players[num5].IsCollisionNoob;
			int num6;
			if (num5 != players.Count() - 1)
			{
				num6 = ((potentialTeam[num5] == 1) ? 1 : 0);
			}
			else
			{
				num6 = 1;
			}
			bool flag8 = (byte)num6 != 0;
			if (players[num5].IsNPCBot)
			{
				num += ((!flag8) ? 1 : (-1));
			}
			else if (region.HasValue)
			{
				bool num7 = flag;
				Region value = region.Value;
				Region? region2 = players[num5].Region;
				int num8;
				if (value == region2.GetValueOrDefault())
				{
					num8 = (region2.HasValue ? 1 : 0);
				}
				else
				{
					num8 = 0;
				}
				flag = ((byte)((num7 ? 1 : 0) & num8) != 0);
			}
			else
			{
				region = players[num5].Region;
			}
			ExcuseBucket key = new ExcuseBucket(num5, groupId);
			List<CharacterRole> value3;
			if (flag8)
			{
				if (dictionary6.TryGetValue(players[num5].LanguageCode, out int value2))
				{
					dictionary6[players[num5].LanguageCode] = value2 + 1;
				}
				else
				{
					dictionary6.Add(players[num5].LanguageCode, 1);
				}
				num3 += ((!selectedCharacter.IsValidForHumanGameplay()) ? 1 : 0);
				num2 += players[num5].LossStreak;
				if (!dictionary8.TryGetValue(key, out value3))
				{
					value3 = (dictionary8[key] = new List<CharacterRole>());
				}
				if (players[num5].SelectedRole == CharacterRole.Assassin)
				{
					flag2 = true;
				}
				else if (players[num5].SelectedRole == CharacterRole.Tank)
				{
					flag3 = true;
				}
				else if (players[num5].SelectedRole == CharacterRole.Support)
				{
					flag4 = true;
				}
			}
			else
			{
				if (dictionary7.TryGetValue(players[num5].LanguageCode, out int value4))
				{
					dictionary7[players[num5].LanguageCode] = value4 + 1;
				}
				else
				{
					dictionary7.Add(players[num5].LanguageCode, 1);
				}
				num4 += ((!selectedCharacter.IsValidForHumanGameplay()) ? 1 : 0);
				num2 -= players[num5].LossStreak;
				if (!dictionary9.TryGetValue(key, out value3))
				{
					value3 = (dictionary9[key] = new List<CharacterRole>());
				}
				if (players[num5].SelectedRole == CharacterRole.Assassin)
				{
					flag5 = true;
				}
				else if (players[num5].SelectedRole == CharacterRole.Tank)
				{
					flag6 = true;
				}
				else if (players[num5].SelectedRole == CharacterRole.Support)
				{
					flag7 = true;
				}
			}
			value3.Add(players[num5].SelectedRole);
			Dictionary<CharacterType, bool> dictionary10;
			if (NoDupInTeam)
			{
				if (!flag8)
				{
					if (!NoDupInGame)
					{
						dictionary10 = dictionary5;
						goto IL_040e;
					}
				}
				dictionary10 = dictionary4;
				goto IL_040e;
			}
			goto IL_047f;
			IL_040e:
			Dictionary<CharacterType, bool> dictionary11 = dictionary10;
			if (dictionary11.TryGetValue(selectedCharacter, out bool value5))
			{
				if (!value5)
				{
					hasExpertCollision = true;
					if (isCollisionNoob)
					{
						dictionary11[selectedCharacter] = true;
					}
				}
				else if (isCollisionNoob)
				{
					hasNoobCollision = true;
				}
				else
				{
					hasExpertCollision = true;
				}
			}
			else if (selectedCharacter.IsValidForHumanGameplay())
			{
				dictionary11.Add(selectedCharacter, isCollisionNoob);
			}
			goto IL_047f;
			IL_047f:
			if (groupId != 0)
			{
				if (dictionary3.ContainsKey(groupId))
				{
					Dictionary<long, ushort> dictionary12;
					Dictionary<long, ushort> dictionary13 = dictionary12 = dictionary3;
					long key2;
					long key3 = key2 = groupId;
					ushort num9 = dictionary12[key2];
					int num10;
					if (flag8)
					{
						num10 = 1;
					}
					else
					{
						num10 = 16;
					}
					dictionary13[key3] = (ushort)(num9 | num10);
				}
				else
				{
					int value6;
					if (flag8)
					{
						value6 = 1;
					}
					else
					{
						value6 = 16;
					}
					dictionary3.Add(groupId, (ushort)value6);
				}
				Dictionary<long, int> dictionary14 = (!flag8) ? dictionary2 : dictionary;
				if (dictionary14.ContainsKey(groupId))
				{
					dictionary14[groupId]++;
				}
				else
				{
					dictionary14[groupId] = 1;
				}
			}
			if (flag8)
			{
				list.Add(players[num5].ELO);
			}
			else
			{
				list2.Add(players[num5].ELO);
			}
		}
		while (true)
		{
			int num11;
			if (dictionary.IsNullOrEmpty())
			{
				num11 = 1;
			}
			else
			{
				num11 = dictionary.Select((KeyValuePair<long, int> p) => p.Value).Max();
			}
			int num12 = num11;
			int num13;
			if (dictionary2.IsNullOrEmpty())
			{
				num13 = 1;
			}
			else
			{
				
				num13 = dictionary2.Select(((KeyValuePair<long, int> p) => p.Value)).Max();
			}
			int num14 = num13;
			int maxGroupSizeImbalance = num12 - num14;
			int num15;
			if (dictionary.IsNullOrEmpty())
			{
				num15 = 1;
			}
			else
			{
				
				num15 = dictionary.Select(((KeyValuePair<long, int> p) => p.Value)).Sum();
			}
			int num16 = num15;
			int num17 = dictionary2.IsNullOrEmpty() ? 1 : dictionary2.Select((KeyValuePair<long, int> p) => p.Value).Sum();
			int groupedMemberCountImbalance = num16 - num17;
			float num18 = 0f;
			float num19 = float.MinValue;
			list.Sort();
			using (List<float>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					float num20 = enumerator.Current;
					if (num19 != float.MinValue)
					{
						float num21 = num20 - num19;
						if (num21 > num18)
						{
							num18 = num21;
						}
					}
					num19 = num20;
				}
			}
			num19 = float.MinValue;
			list2.Sort();
			using (List<float>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					float num22 = enumerator2.Current;
					if (num19 != float.MinValue)
					{
						float num23 = num22 - num19;
						if (num23 > num18)
						{
							num18 = num23;
						}
					}
					num19 = num22;
				}
			}
			GroupBreakage groupBreakage = GroupBreakage.NONE;
			
			IEnumerable<KeyValuePair<long, ushort>> source = dictionary3.Where(((KeyValuePair<long, ushort> p) => p.Value == 17));
			
			IEnumerable<long> source2 = source.Select(((KeyValuePair<long, ushort> p) => p.Key));
			if (source2.Count() > 1)
			{
				groupBreakage = GroupBreakage.MULTIPLE_GROUPS;
			}
			else if (source2.Count() == 1)
			{
				long num24 = source2.First();
				float val = float.MaxValue;
				float val2 = float.MinValue;
				using (List<ELOProvider>.Enumerator enumerator3 = players.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						ELOProvider current = enumerator3.Current;
						if (current.GroupId == num24)
						{
							val = Math.Min(val, current.ELO);
						}
						else
						{
							val2 = Math.Max(val2, current.ELO);
						}
					}
				}
				groupBreakage = GroupBreakage.ONE_GROUP;
			}
			
			int playersOnTeam = 1 + potentialTeam.Count(((int p) => p == 1));
			int num25 = ExcuseBucket.ComputeRolesPresent(playersOnTeam, dictionary8);
			
			int playersOnTeam2 = potentialTeam.Count(((int p) => p != 1));
			int num26 = ExcuseBucket.ComputeRolesPresent(playersOnTeam2, dictionary9);
			int roleImbalance = num25 - num26;
			int num27 = num3;
			int num28 = num4;
			if (!flag2)
			{
				if (num27 > 0)
				{
					flag2 = true;
					num27--;
				}
			}
			if (!flag3)
			{
				if (num27 > 0)
				{
					flag3 = true;
					num27--;
				}
			}
			if (!flag4)
			{
				if (num27 > 0)
				{
					flag4 = true;
					num27--;
				}
			}
			if (!flag5)
			{
				if (num28 > 0)
				{
					flag5 = true;
					num28--;
				}
			}
			if (!flag6)
			{
				if (num28 > 0)
				{
					flag6 = true;
					num28--;
				}
			}
			if (!flag7)
			{
				if (num28 > 0)
				{
					flag7 = true;
					num28--;
				}
			}
			int num29;
			if (flag2)
			{
				num29 = (flag5 ? 1 : 0);
			}
			else
			{
				num29 = 0;
			}
			bool hasAssassin = (byte)num29 != 0;
			int num30;
			if (flag3)
			{
				num30 = (flag6 ? 1 : 0);
			}
			else
			{
				num30 = 0;
			}
			bool hasTank = (byte)num30 != 0;
			int num31;
			if (flag4)
			{
				num31 = (flag7 ? 1 : 0);
			}
			else
			{
				num31 = 0;
			}
			bool hasSupport = (byte)num31 != 0;
			int willFillImbalance = num3 - num4;
			int num32;
			if (dictionary6.IsNullOrEmpty())
			{
				num32 = 0;
			}
			else
			{
				Dictionary<string, int>.ValueCollection values = dictionary6.Values;
				
				num32 = values.Where(((int p) => p > 1)).Sum();
			}
			int num33 = num32;
			int num34;
			if (dictionary7.IsNullOrEmpty())
			{
				num34 = 0;
			}
			else
			{
				Dictionary<string, int>.ValueCollection values2 = dictionary7.Values;
				
				num34 = values2.Where(((int p) => p > 1)).Sum();
			}
			int num35 = num34;
			Solution solution = new Solution(aVbEloDifference, groupBreakage, potentialTeam, maxTeamDifference, maxGroupSizeImbalance, groupedMemberCountImbalance, num, num2, roleImbalance, hasAssassin, hasTank, hasSupport, hasNoobCollision, hasExpertCollision, num18, willFillImbalance, flag, num33 + num35, num33 - num35);
			solution.SetLegality(legalityValidator);
			m_solutions.Add(solution);
			return;
		}
	}

	public BalanceResults Resolve(double rnd)
	{
		if (false)
		{
			m_solutions.Sort();
			for (int i = 0; i < m_solutions.Count; i++)
			{
				object[] array = new object[2];
				object obj;
				if (i == 0)
				{
					obj = "best";
				}
				else
				{
					obj = new StringBuilder().Append("#").Append(i).ToString();
				}
				array[0] = obj;
				array[1] = m_solutions[i].GetLogString();
				Log.Warning("{0}: {1}", array);
			}
		}
		else
		{
			Solution solution = m_solutions[0];
			int index = 0;
			for (int j = 1; j < m_solutions.Count; j++)
			{
				if (m_solutions[j].CompareTo(solution) < 0)
				{
					solution = m_solutions[j];
					index = j;
				}
			}
			m_solutions[index] = m_solutions[0];
			m_solutions[0] = solution;
		}
		return new BalanceResults(m_solutions[0], rnd);
	}

	public bool OnTeamA(int playerIndex)
	{
		int result;
		if (playerIndex < m_solutions[0].m_sides.Count())
		{
			result = ((m_solutions[0].m_sides[playerIndex] == 1) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}
}
