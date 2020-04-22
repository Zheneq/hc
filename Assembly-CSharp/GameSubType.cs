using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GameSubType
{
	public enum GameLoadScreenInstructions
	{
		Default,
		Extraction,
		OverpoweredUp,
		SupportalCombat,
		LightsOut,
		FourCharacters,
		NewGameLoadScreenInstructions
	}

	public enum SubTypeMods
	{
		Exclusive,
		AntiSocial,
		OverrideFreelancerSelection,
		RankedFreelancerSelection,
		AllowPlayingLockedCharacters,
		BlockQueueMMRUpdate,
		UpdateOnlyCasualQueueMMR,
		NotCheckedByDefault,
		HumansHaveFirstSlots,
		AFKPlayersAbortPreLoadGame,
		CanBeConsolidated,
		NotAllowedForGroups,
		ShowWithAITeammates,
		ControlAllBots,
		StricterMods
	}

	public enum RankedSelectionOrderType
	{
		Random,
		Karma,
		MMR,
		Human,
		Slot,
		Tier,
		FreelancersOwned,
		GroupLeaderTrumps
	}

	private class CCSTSort : IComparer<GameSubType>
	{
		private GameSubType m_original;

		private SubTypeMods? m_pivot;

		private bool m_originalHasPivot;

		private List<string> m_originalMaps;

		internal CCSTSort(GameSubType original, SubTypeMods? pivot)
		{
			m_original = original;
			m_pivot = pivot;
			int originalHasPivot;
			if (pivot.HasValue)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				originalHasPivot = (m_original.HasMod(pivot.Value) ? 1 : 0);
			}
			else
			{
				originalHasPivot = 0;
			}
			m_originalHasPivot = ((byte)originalHasPivot != 0);
			List<GameMapConfig> gameMapConfigs = m_original.GameMapConfigs;
			if (_003C_003Ef__am_0024cache0 == null)
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
				_003C_003Ef__am_0024cache0 = ((GameMapConfig p) => p.IsActive);
			}
			IEnumerable<GameMapConfig> source = gameMapConfigs.Where(_003C_003Ef__am_0024cache0);
			if (_003C_003Ef__am_0024cache1 == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				_003C_003Ef__am_0024cache1 = ((GameMapConfig p) => p.Map);
			}
			m_originalMaps = source.Select(_003C_003Ef__am_0024cache1).ToList();
		}

		public int Compare(GameSubType left, GameSubType right)
		{
			if (m_pivot.HasValue)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				bool flag = m_originalHasPivot == left.HasMod(m_pivot.Value);
				bool flag2 = m_originalHasPivot == right.HasMod(m_pivot.Value);
				if (flag != flag2)
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
							if (flag)
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
			bool flag3 = m_original.LocalizedName == left.LocalizedName;
			bool flag4 = m_original.LocalizedName == right.LocalizedName;
			if (flag3 != flag4)
			{
				while (true)
				{
					switch (5)
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
								switch (2)
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
			IEnumerator enumerator = Enum.GetValues(typeof(SubTypeMods)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					SubTypeMods subTypeMods = (SubTypeMods)enumerator.Current;
					if (m_pivot.HasValue)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (subTypeMods == m_pivot.Value)
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
							continue;
						}
					}
					bool flag5 = left.HasMod(subTypeMods);
					bool flag6 = right.HasMod(subTypeMods);
					if (flag5 != flag6)
					{
						int result3;
						if (flag5 == m_original.HasMod(subTypeMods))
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
							result3 = -1;
						}
						else
						{
							result3 = 1;
						}
						return result3;
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			List<GameMapConfig> gameMapConfigs = left.GameMapConfigs;
			if (_003C_003Ef__am_0024cache2 == null)
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
				_003C_003Ef__am_0024cache2 = ((GameMapConfig p) => p.IsActive);
			}
			IEnumerable<GameMapConfig> source = gameMapConfigs.Where(_003C_003Ef__am_0024cache2);
			if (_003C_003Ef__am_0024cache3 == null)
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
				_003C_003Ef__am_0024cache3 = ((GameMapConfig p) => p.Map);
			}
			int num = source.Select(_003C_003Ef__am_0024cache3).ToList().Intersect(m_originalMaps)
				.Count();
			List<GameMapConfig> gameMapConfigs2 = right.GameMapConfigs;
			if (_003C_003Ef__am_0024cache4 == null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				_003C_003Ef__am_0024cache4 = ((GameMapConfig p) => p.IsActive);
			}
			IEnumerable<GameMapConfig> source2 = gameMapConfigs2.Where(_003C_003Ef__am_0024cache4);
			if (_003C_003Ef__am_0024cache5 == null)
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
				_003C_003Ef__am_0024cache5 = ((GameMapConfig p) => p.Map);
			}
			int num2 = source2.Select(_003C_003Ef__am_0024cache5).ToList().Intersect(m_originalMaps)
				.Count();
			if (num != num2)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						int result4;
						if (num > num2)
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
			return 0;
		}
	}

	public string LocalizedName;

	public List<GameMapConfig> GameMapConfigs;

	public List<SubTypeMods> Mods;

	public GameLoadScreenInstructions InstructionsToDisplay;

	public RequirementCollection Requirements;

	public Rate MaxMatchesGrantingXP;

	public TeamCompositionRules TeamComposition;

	public GameValueOverrides GameOverrides;

	public int TeamAPlayers = -1;

	public int TeamBPlayers = -1;

	public int TeamABots = -1;

	public int TeamBBots = -1;

	public FreelancerRoleBalancingRuleTypes RoleBalancingRule;

	public FreelancerDuplicationRuleTypes DuplicationRule;

	public FreelancerTieBreakerRuleTypes TiebreakerRule;

	public List<RankedSelectionOrderType> RankedSelectionOrder;

	public PersistedStatBucket PersistedStatBucket;

	public GameBalanceVars.GameRewardBucketType RewardBucket = GameBalanceVars.GameRewardBucketType.NoRewards;

	public TimeSpan LoadoutSelectionTimeoutOverride;

	public bool NeedsPreSelectedFreelancer
	{
		get
		{
			int num;
			if (!HasMod(SubTypeMods.OverrideFreelancerSelection))
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = (HasMod(SubTypeMods.RankedFreelancerSelection) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			return num == 0;
		}
	}

	public GameSubType Clone()
	{
		GameSubType gameSubType = (GameSubType)MemberwiseClone();
		gameSubType.GameMapConfigs = new List<GameMapConfig>();
		using (List<GameMapConfig>.Enumerator enumerator = GameMapConfigs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameMapConfig current = enumerator.Current;
				gameSubType.GameMapConfigs.Add(current.Clone());
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (true)
					{
						return gameSubType;
					}
					/*OpCode not supported: LdMemberToken*/;
					return gameSubType;
				}
			}
		}
	}

	public bool HasMod(SubTypeMods mod)
	{
		return !Mods.IsNullOrEmpty() && Mods.Contains(mod);
	}

	public LocalizationPayload GetNameAsPayload()
	{
		object attedLocIdentifier;
		if (LocalizedName.IsNullOrEmpty())
		{
			while (true)
			{
				switch (4)
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
			attedLocIdentifier = "unknown@unknown";
		}
		else
		{
			attedLocIdentifier = LocalizedName;
		}
		return LocalizationPayload.Create((string)attedLocIdentifier);
	}

	public bool IsCharacterAllowed(CharacterType freelancer, IFreelancerSetQueryInterface qi)
	{
		return TeamComposition == null || TeamComposition.IsCharacterAllowed(freelancer, qi);
	}

	public bool IsCharacterAllowedInSlot(CharacterType freelancer, Team team, int slot, IFreelancerSetQueryInterface qi)
	{
		int result;
		if (TeamComposition != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (TeamComposition.IsCharacterAllowedInSlot(freelancer, team, slot, qi) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public FreelancerDuplicationRuleTypes GetResolvedDuplicationRule()
	{
		if (DuplicationRule != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return DuplicationRule;
				}
			}
		}
		if (HasMod(SubTypeMods.RankedFreelancerSelection))
		{
			return FreelancerDuplicationRuleTypes.noneInGame;
		}
		return FreelancerDuplicationRuleTypes.noneInTeam;
	}

	public void ValidateSelf(IFreelancerSetQueryInterface qi, LobbyGameConfig gameConfig)
	{
		if (TeamComposition == null)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			TeamComposition.ValidateSelf(qi, gameConfig, GetResolvedDuplicationRule(), GetNameAsPayload().Term);
			return;
		}
	}

	public static TimeSpan ConformTurnTimeSpanFromSeconds(double totalSeconds)
	{
		double value = Math.Max(1.0, Math.Min(99.9, totalSeconds));
		return TimeSpan.FromSeconds(value);
	}

	public static ushort CalculateClosestSubType(GameSubType original, List<GameSubType> allSubTypesInOrder)
	{
		CCSTSort cCSTSort = new CCSTSort(original, null);
		ushort num = 1;
		ushort result = 0;
		GameSubType gameSubType = null;
		using (List<GameSubType>.Enumerator enumerator = allSubTypesInOrder.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameSubType current = enumerator.Current;
				if (gameSubType == null)
				{
					while (true)
					{
						switch (4)
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
					result = num;
					gameSubType = current;
				}
				else if (cCSTSort.Compare(current, gameSubType) < 0)
				{
					result = num;
					gameSubType = current;
				}
				num = (ushort)(num << 1);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public static ushort CalculatePivotSubTypes(ushort mask, SubTypeMods pivot, List<GameSubType> allSubTypesInOrder)
	{
		Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
		Dictionary<ushort, GameSubType> dictionary2 = new Dictionary<ushort, GameSubType>();
		ushort num = 1;
		using (List<GameSubType>.Enumerator enumerator = allSubTypesInOrder.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameSubType current = enumerator.Current;
				if ((mask & num) == 0)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					dictionary2.Add(num, current);
				}
				else
				{
					dictionary.Add(num, current);
				}
				num = (ushort)(num << 1);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		ushort num2 = 0;
		if (!dictionary2.IsNullOrEmpty())
		{
			foreach (KeyValuePair<ushort, GameSubType> item in dictionary)
			{
				CCSTSort cCSTSort = new CCSTSort(item.Value, pivot);
				ushort num3 = 0;
				GameSubType gameSubType = null;
				foreach (KeyValuePair<ushort, GameSubType> item2 in dictionary2)
				{
					if (gameSubType != null)
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
						if (cCSTSort.Compare(item2.Value, gameSubType) >= 0)
						{
							continue;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					gameSubType = item2.Value;
					num3 = item2.Key;
				}
				num2 = (ushort)(num2 | num3);
			}
			return num2;
		}
		return num2;
	}
}
