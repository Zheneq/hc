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
			m_originalHasPivot = pivot.HasValue && m_original.HasMod(pivot.Value);
			m_originalMaps = m_original.GameMapConfigs
				.Where(p => p.IsActive)
				.Select(p => p.Map)
				.ToList();
		}

		public int Compare(GameSubType left, GameSubType right)
		{
			if (m_pivot.HasValue)
			{
				bool leftSamePivot = m_originalHasPivot == left.HasMod(m_pivot.Value);
				bool rightSamePivot = m_originalHasPivot == right.HasMod(m_pivot.Value);
				if (leftSamePivot != rightSamePivot)
				{
					return leftSamePivot ? 1 : -1;
				}
			}
			bool leftSameName = m_original.LocalizedName == left.LocalizedName;
			bool rightSameName = m_original.LocalizedName == right.LocalizedName;
			if (leftSameName != rightSameName)
			{
				return leftSameName ? -1 : 1;
			}
			foreach (SubTypeMods subTypeMods in Enum.GetValues(typeof(SubTypeMods)))
			{
				if (!m_pivot.HasValue || subTypeMods != m_pivot.Value)
				{
					bool leftHasMod = left.HasMod(subTypeMods);
					bool rightHasMod = right.HasMod(subTypeMods);
					if (leftHasMod != rightHasMod)
					{
						return leftHasMod == m_original.HasMod(subTypeMods) ? -1 : 1;
					}
				}
			}
			int leftActiveOriginalMapNum = left.GameMapConfigs
				.Where(p => p.IsActive)
				.Select(p => p.Map)
				.ToList()
				.Intersect(m_originalMaps)
				.Count();
			int rightActiveOriginalMapNum = right.GameMapConfigs
				.Where(p => p.IsActive)
				.Select(p => p.Map)
				.ToList()
				.Intersect(m_originalMaps)
				.Count();
			if (leftActiveOriginalMapNum != rightActiveOriginalMapNum)
			{
				return leftActiveOriginalMapNum > rightActiveOriginalMapNum ? -1 : 1;
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
			return !HasMod(SubTypeMods.OverrideFreelancerSelection)
			       && !HasMod(SubTypeMods.RankedFreelancerSelection);
		}
	}

	public GameSubType Clone()
	{
		GameSubType gameSubType = (GameSubType)MemberwiseClone();
		gameSubType.GameMapConfigs = new List<GameMapConfig>();
		foreach (GameMapConfig config in GameMapConfigs)
		{
			gameSubType.GameMapConfigs.Add(config.Clone());
		}
		return gameSubType;
	}

	public bool HasMod(SubTypeMods mod)
	{
		return !Mods.IsNullOrEmpty() && Mods.Contains(mod);
	}

	public LocalizationPayload GetNameAsPayload()
	{
		string attedLocIdentifier = LocalizedName.IsNullOrEmpty()
			? "unknown@unknown"
			: LocalizedName;
		return LocalizationPayload.Create(attedLocIdentifier);
	}

	public bool IsCharacterAllowed(CharacterType freelancer, IFreelancerSetQueryInterface qi)
	{
		return TeamComposition == null || TeamComposition.IsCharacterAllowed(freelancer, qi);
	}

	public bool IsCharacterAllowedInSlot(CharacterType freelancer, Team team, int slot, IFreelancerSetQueryInterface qi)
	{
		return TeamComposition == null || TeamComposition.IsCharacterAllowedInSlot(freelancer, team, slot, qi);
	}

	public FreelancerDuplicationRuleTypes GetResolvedDuplicationRule()
	{
		if (DuplicationRule != FreelancerDuplicationRuleTypes.byGameType)
		{
			return DuplicationRule;
		}
		if (HasMod(SubTypeMods.RankedFreelancerSelection))
		{
			return FreelancerDuplicationRuleTypes.noneInGame;
		}
		return FreelancerDuplicationRuleTypes.noneInTeam;
	}

	public void ValidateSelf(IFreelancerSetQueryInterface qi, LobbyGameConfig gameConfig)
	{
		if (TeamComposition != null) TeamComposition.ValidateSelf(qi, gameConfig, GetResolvedDuplicationRule(), GetNameAsPayload().Term);
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
		foreach (GameSubType subType in allSubTypesInOrder)
		{
			if (gameSubType == null || cCSTSort.Compare(subType, gameSubType) < 0)
			{
				result = num;
				gameSubType = subType;
			}
			num = (ushort)(num << 1);
		}
		return result;
	}

	public static ushort CalculatePivotSubTypes(ushort mask, SubTypeMods pivot, List<GameSubType> allSubTypesInOrder)
	{
		Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
		Dictionary<ushort, GameSubType> dictionary2 = new Dictionary<ushort, GameSubType>();
		ushort num = 1;
		foreach (GameSubType subType in allSubTypesInOrder)
		{
			if ((mask & num) == 0)
			{
				dictionary2.Add(num, subType);
			}
			else
			{
				dictionary.Add(num, subType);
			}
			num = (ushort)(num << 1);
		}
		if (dictionary2.IsNullOrEmpty())
		{
			return 0;
		}
		ushort num2 = 0;
		foreach (KeyValuePair<ushort, GameSubType> item in dictionary)
		{
			CCSTSort cCSTSort = new CCSTSort(item.Value, pivot);
			ushort num3 = 0;
			GameSubType gameSubType = null;
			foreach (KeyValuePair<ushort, GameSubType> item2 in dictionary2)
			{
				if (gameSubType == null || cCSTSort.Compare(item2.Value, gameSubType) < 0)
				{
					gameSubType = item2.Value;
					num3 = item2.Key;
				}
			}
			num2 = (ushort)(num2 | num3);
		}
		return num2;
	}
}
