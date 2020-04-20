using System;
using System.Collections.Generic;
using System.Linq;

public class BestSolutions
{
	internal const int c_numberOfUniqueRoles = 3;

	private List<BestSolutions.Solution> m_solutions = new List<BestSolutions.Solution>();

	internal FreelancerDuplicationRuleTypes m_duplicationRule;

	public void Register(List<int> potentialTeam, List<ELOProvider> players, float aVbEloDifference, float maxTeamDifference, BestSolutions.ILegalityValidator legalityValidator)
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
		Dictionary<BestSolutions.ExcuseBucket, List<CharacterRole>> dictionary8 = new Dictionary<BestSolutions.ExcuseBucket, List<CharacterRole>>();
		Dictionary<BestSolutions.ExcuseBucket, List<CharacterRole>> dictionary9 = new Dictionary<BestSolutions.ExcuseBucket, List<CharacterRole>>();
		List<float> list = new List<float>();
		List<float> list2 = new List<float>();
		for (int i = 0; i < players.Count; i++)
		{
			CharacterType selectedCharacter = players[i].SelectedCharacter;
			long groupId = players[i].GroupId;
			bool isCollisionNoob = players[i].IsCollisionNoob;
			bool flag8;
			if (i != players.Count<ELOProvider>() - 1)
			{
				flag8 = (potentialTeam[i] == 1);
			}
			else
			{
				flag8 = true;
			}
			bool flag9 = flag8;
			if (players[i].IsNPCBot)
			{
				num += ((!flag9) ? 1 : -1);
			}
			else if (region != null)
			{
				bool flag10 = flag;
				Region value = region.Value;
				Region? region2 = players[i].Region;
				bool flag11;
				if (value == region2.GetValueOrDefault())
				{
					flag11 = (region2 != null);
				}
				else
				{
					flag11 = false;
				}
				flag = (flag10 && flag11);
			}
			else
			{
				region = players[i].Region;
			}
			BestSolutions.ExcuseBucket key = new BestSolutions.ExcuseBucket(i, groupId);
			List<CharacterRole> list3;
			if (flag9)
			{
				int num5;
				if (dictionary6.TryGetValue(players[i].LanguageCode, out num5))
				{
					dictionary6[players[i].LanguageCode] = num5 + 1;
				}
				else
				{
					dictionary6.Add(players[i].LanguageCode, 1);
				}
				num3 += ((!selectedCharacter.IsValidForHumanGameplay()) ? 1 : 0);
				num2 += players[i].LossStreak;
				if (!dictionary8.TryGetValue(key, out list3))
				{
					list3 = new List<CharacterRole>();
					dictionary8[key] = list3;
				}
				if (players[i].SelectedRole == CharacterRole.Assassin)
				{
					flag2 = true;
				}
				else if (players[i].SelectedRole == CharacterRole.Tank)
				{
					flag3 = true;
				}
				else if (players[i].SelectedRole == CharacterRole.Support)
				{
					flag4 = true;
				}
			}
			else
			{
				int num6;
				if (dictionary7.TryGetValue(players[i].LanguageCode, out num6))
				{
					dictionary7[players[i].LanguageCode] = num6 + 1;
				}
				else
				{
					dictionary7.Add(players[i].LanguageCode, 1);
				}
				num4 += ((!selectedCharacter.IsValidForHumanGameplay()) ? 1 : 0);
				num2 -= players[i].LossStreak;
				if (!dictionary9.TryGetValue(key, out list3))
				{
					list3 = new List<CharacterRole>();
					dictionary9[key] = list3;
				}
				if (players[i].SelectedRole == CharacterRole.Assassin)
				{
					flag5 = true;
				}
				else if (players[i].SelectedRole == CharacterRole.Tank)
				{
					flag6 = true;
				}
				else if (players[i].SelectedRole == CharacterRole.Support)
				{
					flag7 = true;
				}
			}
			list3.Add(players[i].SelectedRole);
			if (this.NoDupInTeam)
			{
				if (flag9)
				{
					goto IL_408;
				}
				Dictionary<CharacterType, bool> dictionary10;
				if (this.NoDupInGame)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_408;
					}
				}
				else
				{
					dictionary10 = dictionary5;
				}
				IL_40E:
				Dictionary<CharacterType, bool> dictionary11 = dictionary10;
				bool flag12;
				if (dictionary11.TryGetValue(selectedCharacter, out flag12))
				{
					if (!flag12)
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
					goto IL_47F;
				}
				if (selectedCharacter.IsValidForHumanGameplay())
				{
					dictionary11.Add(selectedCharacter, isCollisionNoob);
					goto IL_47F;
				}
				goto IL_47F;
				IL_408:
				dictionary10 = dictionary4;
				goto IL_40E;
			}
			IL_47F:
			if (groupId != 0L)
			{
				if (dictionary3.ContainsKey(groupId))
				{
					Dictionary<long, ushort> dictionary13;
					Dictionary<long, ushort> dictionary12 = dictionary13 = dictionary3;
					long key3;
					long key2 = key3 = groupId;
					ushort num7 = dictionary13[key3];
					ushort num8;
					if (flag9)
					{
						num8 = 1;
					}
					else
					{
						num8 = 0x10;
					}
					dictionary12[key2] = (ushort)(num7 | num8);
				}
				else
				{
					Dictionary<long, ushort> dictionary14 = dictionary3;
					long key4 = groupId;
					ushort value2;
					if (flag9)
					{
						value2 = 1;
					}
					else
					{
						value2 = 0x10;
					}
					dictionary14.Add(key4, value2);
				}
				Dictionary<long, int> dictionary15 = (!flag9) ? dictionary2 : dictionary;
				if (dictionary15.ContainsKey(groupId))
				{
					Dictionary<long, int> dictionary16;
					long key5;
					(dictionary16 = dictionary15)[key5 = groupId] = dictionary16[key5] + 1;
				}
				else
				{
					dictionary15[groupId] = 1;
				}
			}
			if (flag9)
			{
				list.Add(players[i].ELO);
			}
			else
			{
				list2.Add(players[i].ELO);
			}
		}
		int num9;
		if (dictionary.IsNullOrEmpty<KeyValuePair<long, int>>())
		{
			num9 = 1;
		}
		else
		{
			num9 = (from p in dictionary
			select p.Value).Max();
		}
		int num10 = num9;
		int num11;
		if (dictionary2.IsNullOrEmpty<KeyValuePair<long, int>>())
		{
			num11 = 1;
		}
		else
		{
			IEnumerable<KeyValuePair<long, int>> source = dictionary2;
			
			num11 = source.Select(((KeyValuePair<long, int> p) => p.Value)).Max();
		}
		int num12 = num11;
		int maxGroupSizeImbalance = num10 - num12;
		int num13;
		if (dictionary.IsNullOrEmpty<KeyValuePair<long, int>>())
		{
			num13 = 1;
		}
		else
		{
			IEnumerable<KeyValuePair<long, int>> source2 = dictionary;
			
			num13 = source2.Select(((KeyValuePair<long, int> p) => p.Value)).Sum();
		}
		int num14 = num13;
		int num15;
		if (dictionary2.IsNullOrEmpty<KeyValuePair<long, int>>())
		{
			num15 = 1;
		}
		else
		{
			num15 = (from p in dictionary2
			select p.Value).Sum();
		}
		int num16 = num15;
		int groupedMemberCountImbalance = num14 - num16;
		float num17 = 0f;
		float num18 = float.MinValue;
		list.Sort();
		using (List<float>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				float num19 = enumerator.Current;
				float num20 = num19;
				if (num18 != -3.40282347E+38f)
				{
					float num21 = num20 - num18;
					if (num21 > num17)
					{
						num17 = num21;
					}
				}
				num18 = num20;
			}
		}
		num18 = float.MinValue;
		list2.Sort();
		using (List<float>.Enumerator enumerator2 = list2.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				float num22 = enumerator2.Current;
				float num23 = num22;
				if (num18 != -3.40282347E+38f)
				{
					float num24 = num23 - num18;
					if (num24 > num17)
					{
						num17 = num24;
					}
				}
				num18 = num23;
			}
		}
		BestSolutions.GroupBreakage groupBreakage = BestSolutions.GroupBreakage.NONE;
		IEnumerable<KeyValuePair<long, ushort>> source3 = dictionary3;
		
		IEnumerable<KeyValuePair<long, ushort>> source4 = source3.Where(((KeyValuePair<long, ushort> p) => p.Value == 0x11));
		
		IEnumerable<long> source5 = source4.Select(((KeyValuePair<long, ushort> p) => p.Key));
		if (source5.Count<long>() > 1)
		{
			groupBreakage = BestSolutions.GroupBreakage.MULTIPLE_GROUPS;
		}
		else if (source5.Count<long>() == 1)
		{
			long num25 = source5.First<long>();
			float val = float.MaxValue;
			float val2 = float.MinValue;
			using (List<ELOProvider>.Enumerator enumerator3 = players.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					ELOProvider eloprovider = enumerator3.Current;
					if (eloprovider.GroupId == num25)
					{
						val = Math.Min(val, eloprovider.ELO);
					}
					else
					{
						val2 = Math.Max(val2, eloprovider.ELO);
					}
				}
			}
			groupBreakage = BestSolutions.GroupBreakage.ONE_GROUP;
		}
		int num26 = 1;
		
		int playersOnTeam = num26 + potentialTeam.Count(((int p) => p == 1));
		int num27 = BestSolutions.ExcuseBucket.ComputeRolesPresent(playersOnTeam, dictionary8);
		
		int playersOnTeam2 = potentialTeam.Count(((int p) => p != 1));
		int num28 = BestSolutions.ExcuseBucket.ComputeRolesPresent(playersOnTeam2, dictionary9);
		int roleImbalance = num27 - num28;
		int num29 = num3;
		int num30 = num4;
		if (!flag2)
		{
			if (num29 > 0)
			{
				flag2 = true;
				num29--;
			}
		}
		if (!flag3)
		{
			if (num29 > 0)
			{
				flag3 = true;
				num29--;
			}
		}
		if (!flag4)
		{
			if (num29 > 0)
			{
				flag4 = true;
				num29--;
			}
		}
		if (!flag5)
		{
			if (num30 > 0)
			{
				flag5 = true;
				num30--;
			}
		}
		if (!flag6)
		{
			if (num30 > 0)
			{
				flag6 = true;
				num30--;
			}
		}
		if (!flag7)
		{
			if (num30 > 0)
			{
				flag7 = true;
				num30--;
			}
		}
		bool flag13;
		if (flag2)
		{
			flag13 = flag5;
		}
		else
		{
			flag13 = false;
		}
		bool hasAssassin = flag13;
		bool flag14;
		if (flag3)
		{
			flag14 = flag6;
		}
		else
		{
			flag14 = false;
		}
		bool hasTank = flag14;
		bool flag15;
		if (flag4)
		{
			flag15 = flag7;
		}
		else
		{
			flag15 = false;
		}
		bool hasSupport = flag15;
		int willFillImbalance = num3 - num4;
		int num31;
		if (dictionary6.IsNullOrEmpty<KeyValuePair<string, int>>())
		{
			num31 = 0;
		}
		else
		{
			IEnumerable<int> values = dictionary6.Values;
			
			num31 = values.Where(((int p) => p > 1)).Sum();
		}
		int num32 = num31;
		int num33;
		if (dictionary7.IsNullOrEmpty<KeyValuePair<string, int>>())
		{
			num33 = 0;
		}
		else
		{
			IEnumerable<int> values2 = dictionary7.Values;
			
			num33 = values2.Where(((int p) => p > 1)).Sum();
		}
		int num34 = num33;
		BestSolutions.Solution solution = new BestSolutions.Solution(aVbEloDifference, groupBreakage, potentialTeam, maxTeamDifference, maxGroupSizeImbalance, groupedMemberCountImbalance, num, num2, roleImbalance, hasAssassin, hasTank, hasSupport, hasNoobCollision, hasExpertCollision, num17, willFillImbalance, flag, num32 + num34, num32 - num34);
		solution.SetLegality(legalityValidator);
		this.m_solutions.Add(solution);
	}

	public BestSolutions.BalanceResults Resolve(double rnd)
	{
		bool flag = false;
		if (flag)
		{
			this.m_solutions.Sort();
			for (int i = 0; i < this.m_solutions.Count; i++)
			{
				string message = "{0}: {1}";
				object[] array = new object[2];
				int num = 0;
				object obj;
				if (i == 0)
				{
					obj = "best";
				}
				else
				{
					obj = string.Format("#{0}", i);
				}
				array[num] = obj;
				array[1] = this.m_solutions[i].GetLogString();
				Log.Warning(message, array);
			}
		}
		else
		{
			BestSolutions.Solution solution = this.m_solutions[0];
			int index = 0;
			for (int j = 1; j < this.m_solutions.Count; j++)
			{
				if (this.m_solutions[j].CompareTo(solution) < 0)
				{
					solution = this.m_solutions[j];
					index = j;
				}
			}
			this.m_solutions[index] = this.m_solutions[0];
			this.m_solutions[0] = solution;
		}
		return new BestSolutions.BalanceResults(this.m_solutions[0], rnd);
	}

	public bool OnTeamA(int playerIndex)
	{
		bool result;
		if (playerIndex < this.m_solutions[0].m_sides.Count<int>())
		{
			result = (this.m_solutions[0].m_sides[playerIndex] == 1);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private bool NoDupInGame
	{
		get
		{
			return this.m_duplicationRule == FreelancerDuplicationRuleTypes.noneInGame;
		}
	}

	private bool NoDupInTeam
	{
		get
		{
			if (this.m_duplicationRule == FreelancerDuplicationRuleTypes.byGameType)
			{
				throw new Exception("Must have resolved duplication rule before using BestSolutions.Resolve");
			}
			bool result;
			if (this.m_duplicationRule != FreelancerDuplicationRuleTypes.noneInGame)
			{
				result = (this.m_duplicationRule == FreelancerDuplicationRuleTypes.noneInTeam);
			}
			else
			{
				result = true;
			}
			return result;
		}
	}

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
		bool IsLegal(BestSolutions.Solution s);
	}

	public class Solution : IComparable<BestSolutions.Solution>
	{
		private BestSolutions.GroupBreakage m_groupBreakage;

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

		public Solution(float aVbEloDifference, BestSolutions.GroupBreakage groupBreakage, List<int> sides, float maxDifferenceForOtherSortingMethods, int maxGroupSizeImbalance, int groupedMemberCountImbalance, int botImbalance, int loserImbalance, int roleImbalance, bool hasAssassin, bool hasTank, bool hasSupport, bool hasNoobCollision, bool hasExpertCollision, float largestEloWidth, int willFillImbalance, bool regionUniqueness, int languageCommonality, int languageTeamAAdvantage)
		{
			this.m_groupBreakage = groupBreakage;
			this.m_AvBEloDifference = aVbEloDifference;
			this.m_largestEloWidth = largestEloWidth;
			float num = Math.Abs(aVbEloDifference);
			this.m_useOtherSortingCriteria = (num < maxDifferenceForOtherSortingMethods);
			this.m_useFineOtherSortingCriteria = (num < maxDifferenceForOtherSortingMethods / 2f);
			this.m_sides = new List<int>(sides);
			this.m_maxGroupSizeImbalance = Math.Abs(maxGroupSizeImbalance);
			this.m_groupedMemberCountImbalance = Math.Abs(groupedMemberCountImbalance);
			this.m_botImbalance = Math.Abs(botImbalance);
			this.m_loserImbalance = Math.Abs(loserImbalance);
			this.m_roleImbalance = Math.Abs(roleImbalance);
			this.m_willFillImbalance = Math.Abs(willFillImbalance);
			this.m_hasRoleAssassin = hasAssassin;
			this.m_hasRoleTank = hasTank;
			this.m_hasRoleSupport = hasSupport;
			this.m_noobCollision = hasNoobCollision;
			this.m_expertCollision = hasExpertCollision;
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
				num2 += ((groupedMemberCountImbalance <= 0) ? -1 : 1);
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
				num2 += ((willFillImbalance <= 0) ? -1 : 1);
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
			if (!this.m_useFineOtherSortingCriteria)
			{
				num2 += ((aVbEloDifference <= 0f) ? -1 : 1);
				if (!this.m_useOtherSortingCriteria)
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
			this.m_balanceFailureImbalance = Math.Abs(num2);
			this.m_regionUniqueness = regionUniqueness;
			this.m_languageCommonality = languageCommonality;
			if (languageTeamAAdvantage > 0)
			{
				num2++;
				this.m_languageImbalance = true;
			}
			else if (languageTeamAAdvantage < 0)
			{
				num2--;
				this.m_languageImbalance = true;
			}
			else
			{
				this.m_languageImbalance = false;
			}
		}

		public float AbsEloDifference
		{
			get
			{
				return Math.Abs(this.m_AvBEloDifference);
			}
		}

		public float AvBEloDifference
		{
			get
			{
				return this.m_AvBEloDifference;
			}
		}

		public bool BreaksGroups
		{
			get
			{
				return this.m_groupBreakage != BestSolutions.GroupBreakage.NONE;
			}
		}

		public bool HasNoobCollisions
		{
			get
			{
				return this.m_noobCollision;
			}
		}

		public bool HasExpertCollisions
		{
			get
			{
				return this.m_expertCollision;
			}
		}

		public int MaxGroupSizeImbalance
		{
			get
			{
				return this.m_maxGroupSizeImbalance;
			}
		}

		public int GroupedMemberCountImbalance
		{
			get
			{
				return this.m_groupedMemberCountImbalance;
			}
		}

		public BestSolutions.GroupBreakage GroupBreakage
		{
			get
			{
				return this.m_groupBreakage;
			}
		}

		public bool HasRoleImbalance
		{
			get
			{
				return this.m_roleImbalance != 0;
			}
		}

		public bool HasMissingRoles
		{
			get
			{
				if (this.m_hasRoleTank)
				{
					if (this.m_hasRoleSupport)
					{
						return !this.m_hasRoleAssassin;
					}
				}
				return true;
			}
		}

		public bool HasMissingRoleAssassin
		{
			get
			{
				return !this.m_hasRoleAssassin;
			}
		}

		public bool HasMissingRoleTank
		{
			get
			{
				return !this.m_hasRoleTank;
			}
		}

		public bool HasMissingRoleSupport
		{
			get
			{
				return !this.m_hasRoleSupport;
			}
		}

		public int RoleCount
		{
			get
			{
				int num;
				if (this.m_hasRoleAssassin)
				{
					num = 1;
				}
				else
				{
					num = 0;
				}
				int num2;
				if (this.m_hasRoleTank)
				{
					num2 = 1;
				}
				else
				{
					num2 = 0;
				}
				int num3 = num + num2;
				int num4;
				if (this.m_hasRoleSupport)
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
			get
			{
				return this.m_willFillImbalance != 0;
			}
		}

		public bool HasBotImbalance
		{
			get
			{
				return this.m_botImbalance > 1;
			}
		}

		public bool HasLoserImbalance
		{
			get
			{
				return this.m_loserImbalance > 1;
			}
		}

		public bool HasRegionUniqueness
		{
			get
			{
				return this.m_regionUniqueness;
			}
		}

		public bool HasLanguageImbalance
		{
			get
			{
				return this.m_languageImbalance;
			}
		}

		public int LanguageCommonality
		{
			get
			{
				return this.m_languageCommonality;
			}
		}

		public BestSolutions.BalanceResults.BalanceFailureLevel BalanceFailureImbalance
		{
			get
			{
				BestSolutions.BalanceResults.BalanceFailureLevel result;
				if (this.m_balanceFailureImbalance == 0)
				{
					result = BestSolutions.BalanceResults.BalanceFailureLevel.Fair;
				}
				else if (this.m_balanceFailureImbalance == 1)
				{
					result = BestSolutions.BalanceResults.BalanceFailureLevel.Minor;
				}
				else
				{
					result = ((this.m_balanceFailureImbalance != 2) ? BestSolutions.BalanceResults.BalanceFailureLevel.Tragic : BestSolutions.BalanceResults.BalanceFailureLevel.Major);
				}
				return result;
			}
		}

		public BestSolutions.EloImbalance EloImbalance
		{
			get
			{
				BestSolutions.EloImbalance result;
				if (this.m_useFineOtherSortingCriteria)
				{
					result = BestSolutions.EloImbalance.MINOR;
				}
				else if (this.m_useOtherSortingCriteria)
				{
					result = BestSolutions.EloImbalance.ACCEPTABLE;
				}
				else
				{
					result = BestSolutions.EloImbalance.MAJOR;
				}
				return result;
			}
		}

		internal string GetLogString()
		{
			Func<bool, string, string, string> func = delegate(bool isTrue, string writeText, string appendText)
			{
				if (isTrue)
				{
					return string.Format("{0}{1}{2}", writeText, (writeText.Count<char>() <= 0) ? string.Empty : "+", appendText);
				}
				return writeText;
			};
			string text = string.Empty;
			text = func(this.m_noobCollision, text, "noob");
			text = func(this.m_expertCollision, text, "expert");
			if (text.Count<char>() > 0)
			{
				text = string.Format(", {0} collision", text);
			}
			string text2 = string.Empty;
			text2 = func(this.m_botImbalance != 0, text2, "bot");
			text2 = func(this.m_maxGroupSizeImbalance != 0, text2, "group");
			Func<bool, string, string, string> func2 = func;
			bool arg = this.m_loserImbalance != 0;
			string arg2 = text2;
			string arg3;
			if (this.m_loserImbalance > 1)
			{
				arg3 = string.Format("loser({0})", this.m_loserImbalance);
			}
			else
			{
				arg3 = "loser";
			}
			text2 = func2(arg, arg2, arg3);
			text2 = func(this.m_roleImbalance != 0, text2, "role");
			text2 = func(this.m_willFillImbalance != 0, text2, "fill");
			text2 = func(this.m_languageImbalance, text2, "lang");
			text2 = func(this.m_balanceFailureImbalance != 0, text2, string.Format("unfair{0}", this.m_balanceFailureImbalance));
			if (text2.Count<char>() > 0)
			{
				text2 = string.Format(", {0} imbalance", text2);
			}
			string text3;
			if (!this.m_useFineOtherSortingCriteria)
			{
				text3 = string.Format("eloDiff={0}", this.AbsEloDifference);
			}
			else if (this.m_largestEloWidth > this.AbsEloDifference)
			{
				text3 = string.Format("width={0}, eloDiff={1}", this.m_largestEloWidth, this.AbsEloDifference);
			}
			else
			{
				text3 = string.Format("eloDiff={0}, width={1}", this.AbsEloDifference, this.m_largestEloWidth);
			}
			string format = "solution({0}{1}{2}{3}{4}{5}{6}, {7}, {8} lang)";
			object[] array = new object[9];
			array[0] = ((!this.m_useOtherSortingCriteria) ? "ELO Only" : ((!this.m_useFineOtherSortingCriteria) ? "flexible" : "fine"));
			int num = 1;
			object obj;
			if (this.m_groupBreakage == BestSolutions.GroupBreakage.NONE)
			{
				obj = string.Empty;
			}
			else
			{
				obj = string.Format(", {0}", this.m_groupBreakage);
			}
			array[num] = obj;
			array[2] = text;
			array[3] = text2;
			int num2 = 4;
			object obj2;
			if (this.m_hasRoleAssassin)
			{
				obj2 = string.Empty;
			}
			else
			{
				obj2 = ", no assassin";
			}
			array[num2] = obj2;
			int num3 = 5;
			object obj3;
			if (this.m_hasRoleTank)
			{
				obj3 = string.Empty;
			}
			else
			{
				obj3 = ", no tank";
			}
			array[num3] = obj3;
			int num4 = 6;
			object obj4;
			if (this.m_hasRoleSupport)
			{
				obj4 = string.Empty;
			}
			else
			{
				obj4 = ", no support";
			}
			array[num4] = obj4;
			array[7] = text3;
			array[8] = this.m_languageCommonality;
			return string.Format(format, array);
		}

		internal void SetLegality(BestSolutions.ILegalityValidator validator)
		{
			this.m_isLegal = validator.IsLegal(this);
		}

		public int CoarseCompareTo(BestSolutions.Solution other)
		{
			if (this.m_isLegal != other.m_isLegal)
			{
				int result;
				if (this.m_isLegal)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
				return result;
			}
			bool flag = this.m_groupBreakage == BestSolutions.GroupBreakage.MULTIPLE_GROUPS;
			bool flag2 = other.m_groupBreakage == BestSolutions.GroupBreakage.MULTIPLE_GROUPS;
			if (flag != flag2)
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
			bool flag3 = this.m_groupBreakage == BestSolutions.GroupBreakage.ONE_GROUP;
			bool flag4 = other.m_groupBreakage == BestSolutions.GroupBreakage.ONE_GROUP;
			if (flag3 != flag4)
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
			if (this.m_useOtherSortingCriteria != other.m_useOtherSortingCriteria)
			{
				int result4;
				if (this.m_useOtherSortingCriteria)
				{
					result4 = -1;
				}
				else
				{
					result4 = 1;
				}
				return result4;
			}
			if (this.m_useOtherSortingCriteria)
			{
				if (this.m_balanceFailureImbalance != other.m_balanceFailureImbalance)
				{
					return (this.m_balanceFailureImbalance <= other.m_balanceFailureImbalance) ? -1 : 1;
				}
				if (this.m_botImbalance != other.m_botImbalance)
				{
					int result5;
					if (this.m_botImbalance > other.m_botImbalance)
					{
						result5 = 1;
					}
					else
					{
						result5 = -1;
					}
					return result5;
				}
				if (this.m_maxGroupSizeImbalance != other.m_maxGroupSizeImbalance)
				{
					int result6;
					if (this.m_maxGroupSizeImbalance > other.m_maxGroupSizeImbalance)
					{
						result6 = 1;
					}
					else
					{
						result6 = -1;
					}
					return result6;
				}
				if (this.m_noobCollision != other.m_noobCollision)
				{
					int result7;
					if (this.m_noobCollision)
					{
						result7 = 1;
					}
					else
					{
						result7 = -1;
					}
					return result7;
				}
				if (this.m_willFillImbalance != other.m_willFillImbalance)
				{
					int result8;
					if (this.m_willFillImbalance > other.m_willFillImbalance)
					{
						result8 = 1;
					}
					else
					{
						result8 = -1;
					}
					return result8;
				}
				if (this.m_roleImbalance != other.m_roleImbalance)
				{
					int result9;
					if (this.m_roleImbalance > other.m_roleImbalance)
					{
						result9 = 1;
					}
					else
					{
						result9 = -1;
					}
					return result9;
				}
				int roleCount = this.RoleCount;
				int roleCount2 = other.RoleCount;
				if (roleCount != roleCount2)
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
				if (this.m_expertCollision != other.m_expertCollision)
				{
					int result11;
					if (this.m_expertCollision)
					{
						result11 = 1;
					}
					else
					{
						result11 = -1;
					}
					return result11;
				}
				if (this.m_languageCommonality != other.m_languageCommonality)
				{
					int result12;
					if (this.m_languageCommonality > other.m_languageCommonality)
					{
						result12 = -1;
					}
					else
					{
						result12 = 1;
					}
					return result12;
				}
				if (this.m_languageImbalance != other.m_languageImbalance)
				{
					int result13;
					if (this.m_languageImbalance)
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
			return 0;
		}

		public int CompareTo(BestSolutions.Solution other)
		{
			int num = this.CoarseCompareTo(other);
			if (num != 0)
			{
				return num;
			}
			if (this.m_useOtherSortingCriteria)
			{
				if (this.m_loserImbalance != other.m_loserImbalance)
				{
					int result;
					if (this.m_loserImbalance > other.m_loserImbalance)
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
			if (this.m_useFineOtherSortingCriteria != other.m_useFineOtherSortingCriteria)
			{
				int result2;
				if (this.m_useFineOtherSortingCriteria)
				{
					result2 = -1;
				}
				else
				{
					result2 = 1;
				}
				return result2;
			}
			if (this.m_useFineOtherSortingCriteria)
			{
				float num2 = Math.Max(this.m_largestEloWidth, this.AbsEloDifference);
				float num3 = Math.Max(other.m_largestEloWidth, other.AbsEloDifference);
				if (num2 != num3)
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
			float absEloDifference = this.AbsEloDifference;
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

	private class ExcuseBucket : IEqualityComparer<BestSolutions.ExcuseBucket>
	{
		public long m_id;

		public bool m_isGroup;

		public ExcuseBucket(int playerId, long groupId)
		{
			if (groupId != 0L)
			{
				this.m_id = groupId;
				this.m_isGroup = true;
			}
			else
			{
				this.m_id = (long)playerId;
				this.m_isGroup = false;
			}
		}

		public bool Equals(BestSolutions.ExcuseBucket x, BestSolutions.ExcuseBucket y)
		{
			bool result;
			if (x.m_id == y.m_id)
			{
				result = (x.m_isGroup == y.m_isGroup);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public int GetHashCode(BestSolutions.ExcuseBucket x)
		{
			return this.m_isGroup.GetHashCode() ^ this.m_id.GetHashCode();
		}

		public static int ComputeRolesPresent(int playersOnTeam, Dictionary<BestSolutions.ExcuseBucket, List<CharacterRole>> teamRoles)
		{
			int num = Math.Max(0, playersOnTeam - 3);
			int num2 = 0;
			HashSet<CharacterRole> hashSet = new HashSet<CharacterRole>();
			foreach (KeyValuePair<BestSolutions.ExcuseBucket, List<CharacterRole>> keyValuePair in teamRoles)
			{
				List<CharacterRole> value = keyValuePair.Value;
				value.Sort();
				int i = num;
				while (i > 0)
				{
					if (!BestSolutions.ExcuseBucket.RolesHaveDups(value))
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							goto IL_78;
						}
					}
					else
					{
						BestSolutions.ExcuseBucket.RemoveOneDup(ref value);
						i--;
					}
				}
				IL_78:
				for (int j = 0; j < value.Count<CharacterRole>(); j++)
				{
					if (value[j] == CharacterRole.None)
					{
						num2++;
					}
					else
					{
						if (j > 0)
						{
							if (value[j - 1] == value[j])
							{
								num2++;
								goto IL_D7;
							}
						}
						hashSet.Add(value[j]);
					}
					IL_D7:;
				}
			}
			return Math.Min(hashSet.Count<CharacterRole>() + num2, 3);
		}

		private static bool RolesHaveDups(List<CharacterRole> roles)
		{
			for (int i = 1; i < roles.Count<CharacterRole>(); i++)
			{
				if (roles[i - 1] == roles[i])
				{
					return true;
				}
			}
			return false;
		}

		private unsafe static void RemoveOneDup(ref List<CharacterRole> roles)
		{
			for (int i = 1; i < roles.Count<CharacterRole>(); i++)
			{
				if (roles[i - 1] == roles[i])
				{
					roles.RemoveAt(i - 1);
					return;
				}
			}
			throw new Exception("No dup in list that should have dup");
		}
	}

	public class BalanceResults : IComparable<BestSolutions.BalanceResults>
	{
		public BalanceResults(double rnd)
		{
			this.m_solution = null;
			this.m_rnd = rnd;
			this.Success = false;
		}

		public BalanceResults(BestSolutions.Solution solution, double rnd)
		{
			this.m_solution = solution;
			this.m_rnd = rnd;
			this.Success = true;
		}

		public BestSolutions.GroupBreakage GroupBreakage
		{
			get
			{
				return (this.m_solution != null) ? this.m_solution.GroupBreakage : BestSolutions.GroupBreakage.NONE;
			}
		}

		public BestSolutions.EloImbalance EloImbalance
		{
			get
			{
				return (this.m_solution != null) ? this.m_solution.EloImbalance : BestSolutions.EloImbalance.MINOR;
			}
		}

		public BestSolutions.BalanceResults.BalanceFailureLevel BalanceFailureImbalance
		{
			get
			{
				return (this.m_solution != null) ? this.m_solution.BalanceFailureImbalance : BestSolutions.BalanceResults.BalanceFailureLevel.Fair;
			}
		}

		public int GroupImbalanceAmount
		{
			get
			{
				int result;
				if (this.m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = this.m_solution.MaxGroupSizeImbalance;
				}
				return result;
			}
		}

		public bool WillFillImbalance
		{
			get
			{
				return this.m_solution != null && this.m_solution.HasWillFillImbalance;
			}
		}

		public bool BotImbalance
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = false;
				}
				else
				{
					result = this.m_solution.HasBotImbalance;
				}
				return result;
			}
		}

		public bool LoserImbalance
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = false;
				}
				else
				{
					result = this.m_solution.HasLoserImbalance;
				}
				return result;
			}
		}

		public bool RoleImbalance
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = false;
				}
				else
				{
					result = this.m_solution.HasRoleImbalance;
				}
				return result;
			}
		}

		public bool MissingRoles
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = false;
				}
				else
				{
					result = this.m_solution.HasMissingRoles;
				}
				return result;
			}
		}

		public bool HasMissingRoleAssassin
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = false;
				}
				else
				{
					result = this.m_solution.HasMissingRoleAssassin;
				}
				return result;
			}
		}

		public bool HasMissingRoleTank
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = false;
				}
				else
				{
					result = this.m_solution.HasMissingRoleTank;
				}
				return result;
			}
		}

		public bool HasMissingRoleSupport
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = false;
				}
				else
				{
					result = this.m_solution.HasMissingRoleSupport;
				}
				return result;
			}
		}

		public int LanguageCompatability
		{
			get
			{
				int result;
				if (this.m_solution == null)
				{
					result = 0;
				}
				else
				{
					result = this.m_solution.LanguageCommonality;
				}
				return result;
			}
		}

		public bool LanguageImbalance
		{
			get
			{
				return this.m_solution == null || this.m_solution.HasLanguageImbalance;
			}
		}

		public bool RegionUniqueness
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = true;
				}
				else
				{
					result = this.m_solution.HasRegionUniqueness;
				}
				return result;
			}
		}

		public bool NoobCollision
		{
			get
			{
				return this.m_solution != null && this.m_solution.HasNoobCollisions;
			}
		}

		public bool ExpertCollision
		{
			get
			{
				bool result;
				if (this.m_solution == null)
				{
					result = false;
				}
				else
				{
					result = this.m_solution.HasExpertCollisions;
				}
				return result;
			}
		}

		public float AvBEloDifference
		{
			get
			{
				float result;
				if (this.m_solution == null)
				{
					result = 0f;
				}
				else
				{
					result = this.m_solution.AvBEloDifference;
				}
				return result;
			}
		}

		public float AbsEloDifference
		{
			get
			{
				float result;
				if (this.m_solution == null)
				{
					result = 0f;
				}
				else
				{
					result = this.m_solution.AbsEloDifference;
				}
				return result;
			}
		}

		private BestSolutions.Solution m_solution { get; set; }

		private double m_rnd { get; set; }

		public bool Success { get; set; }

		public LocalizationPayload Error { get; internal set; }

		public static BestSolutions.BalanceResults MakeError(string term, string context)
		{
			return new BestSolutions.BalanceResults(0.0)
			{
				Error = LocalizationPayload.Create(term, context),
				Success = false
			};
		}

		public string SolutionScoreAsString
		{
			get
			{
				string result;
				if (this.m_solution == null)
				{
					result = "<null>";
				}
				else
				{
					result = this.m_solution.GetLogString();
				}
				return result;
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
						retVal = string.Format("{0}{1}{2}", retVal, (retVal.Count<char>() <= 0) ? string.Empty : "+", appendText);
					}
				};
				action(this.ExpertCollision, "collideE");
				action(this.NoobCollision, "collideN");
				action(this.GroupImbalanceAmount > 0, "imbalGp");
				action(this.BotImbalance, "imbalBt");
				action(this.LoserImbalance, "imbalLs");
				action(this.LanguageImbalance, "imbalLng");
				action(this.RoleImbalance, "imbalRl");
				action(this.WillFillImbalance, "imbalWf");
				action(this.MissingRoles, "missingRoles");
				action(this.GroupBreakage == BestSolutions.GroupBreakage.ONE_GROUP, "breakO");
				action(this.GroupBreakage == BestSolutions.GroupBreakage.MULTIPLE_GROUPS, "breakM");
				action(this.EloImbalance == BestSolutions.EloImbalance.ACCEPTABLE, "eloT");
				action(this.EloImbalance == BestSolutions.EloImbalance.MAJOR, "eloM");
				string result;
				if (retVal == string.Empty)
				{
					result = "none";
				}
				else
				{
					result = retVal;
				}
				return result;
			}
		}

		public int ConsolidatedCompareTo(BestSolutions.BalanceResults other)
		{
			if (this.Success != other.Success)
			{
				int result;
				if (this.Success)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
				return result;
			}
			if (this.m_solution == null != (other.m_solution == null))
			{
				int result2;
				if (this.m_solution != null)
				{
					result2 = -1;
				}
				else
				{
					result2 = 1;
				}
				return result2;
			}
			return this.m_rnd.CompareTo(other.m_rnd);
		}

		public int CompareTo(BestSolutions.BalanceResults other)
		{
			if (this.Success != other.Success)
			{
				int result;
				if (this.Success)
				{
					result = -1;
				}
				else
				{
					result = 1;
				}
				return result;
			}
			if (this.m_solution == null != (other.m_solution == null))
			{
				return (this.m_solution == null) ? 1 : -1;
			}
			if (this.m_solution != null)
			{
				int num = this.m_solution.CoarseCompareTo(other.m_solution);
				if (num != 0)
				{
					return num;
				}
			}
			return this.m_rnd.CompareTo(other.m_rnd);
		}

		public enum BalanceFailureLevel
		{
			Fair,
			Minor,
			Major,
			Tragic
		}
	}
}
