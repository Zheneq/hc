using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class GameSubType
{
	public string LocalizedName;

	public List<GameMapConfig> GameMapConfigs;

	public List<GameSubType.SubTypeMods> Mods;

	public GameSubType.GameLoadScreenInstructions InstructionsToDisplay;

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

	public List<GameSubType.RankedSelectionOrderType> RankedSelectionOrder;

	public PersistedStatBucket PersistedStatBucket;

	public GameBalanceVars.GameRewardBucketType RewardBucket = GameBalanceVars.GameRewardBucketType.NoRewards;

	public TimeSpan LoadoutSelectionTimeoutOverride;

	public GameSubType Clone()
	{
		GameSubType gameSubType = (GameSubType)base.MemberwiseClone();
		gameSubType.GameMapConfigs = new List<GameMapConfig>();
		using (List<GameMapConfig>.Enumerator enumerator = this.GameMapConfigs.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameMapConfig gameMapConfig = enumerator.Current;
				gameSubType.GameMapConfigs.Add(gameMapConfig.Clone());
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.Clone()).MethodHandle;
			}
		}
		return gameSubType;
	}

	public bool NeedsPreSelectedFreelancer
	{
		get
		{
			int num;
			if (!this.HasMod(GameSubType.SubTypeMods.OverrideFreelancerSelection))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.get_NeedsPreSelectedFreelancer()).MethodHandle;
				}
				num = (this.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			return num == 0;
		}
	}

	public bool HasMod(GameSubType.SubTypeMods mod)
	{
		return !this.Mods.IsNullOrEmpty<GameSubType.SubTypeMods>() && this.Mods.Contains(mod);
	}

	public LocalizationPayload GetNameAsPayload()
	{
		string attedLocIdentifier;
		if (this.LocalizedName.IsNullOrEmpty())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.GetNameAsPayload()).MethodHandle;
			}
			attedLocIdentifier = "unknown@unknown";
		}
		else
		{
			attedLocIdentifier = this.LocalizedName;
		}
		return LocalizationPayload.Create(attedLocIdentifier);
	}

	public bool IsCharacterAllowed(CharacterType freelancer, IFreelancerSetQueryInterface qi)
	{
		return this.TeamComposition == null || this.TeamComposition.IsCharacterAllowed(freelancer, qi);
	}

	public bool IsCharacterAllowedInSlot(CharacterType freelancer, Team team, int slot, IFreelancerSetQueryInterface qi)
	{
		bool result;
		if (this.TeamComposition != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.IsCharacterAllowedInSlot(CharacterType, Team, int, IFreelancerSetQueryInterface)).MethodHandle;
			}
			result = this.TeamComposition.IsCharacterAllowedInSlot(freelancer, team, slot, qi);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public FreelancerDuplicationRuleTypes GetResolvedDuplicationRule()
	{
		if (this.DuplicationRule != FreelancerDuplicationRuleTypes.byGameType)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.GetResolvedDuplicationRule()).MethodHandle;
			}
			return this.DuplicationRule;
		}
		if (this.HasMod(GameSubType.SubTypeMods.RankedFreelancerSelection))
		{
			return FreelancerDuplicationRuleTypes.noneInGame;
		}
		return FreelancerDuplicationRuleTypes.noneInTeam;
	}

	public void ValidateSelf(IFreelancerSetQueryInterface qi, LobbyGameConfig gameConfig)
	{
		if (this.TeamComposition != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.ValidateSelf(IFreelancerSetQueryInterface, LobbyGameConfig)).MethodHandle;
			}
			this.TeamComposition.ValidateSelf(qi, gameConfig, this.GetResolvedDuplicationRule(), this.GetNameAsPayload().Term);
		}
	}

	public static TimeSpan ConformTurnTimeSpanFromSeconds(double totalSeconds)
	{
		double value = Math.Max(1.0, Math.Min(99.9, totalSeconds));
		return TimeSpan.FromSeconds(value);
	}

	public static ushort CalculateClosestSubType(GameSubType original, List<GameSubType> allSubTypesInOrder)
	{
		GameSubType.CCSTSort ccstsort = new GameSubType.CCSTSort(original, null);
		ushort num = 1;
		ushort result = 0;
		GameSubType gameSubType = null;
		using (List<GameSubType>.Enumerator enumerator = allSubTypesInOrder.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameSubType gameSubType2 = enumerator.Current;
				if (gameSubType == null)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.CalculateClosestSubType(GameSubType, List<GameSubType>)).MethodHandle;
					}
					result = num;
					gameSubType = gameSubType2;
				}
				else if (ccstsort.Compare(gameSubType2, gameSubType) < 0)
				{
					result = num;
					gameSubType = gameSubType2;
				}
				num = (ushort)(num << 1);
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return result;
	}

	public static ushort CalculatePivotSubTypes(ushort mask, GameSubType.SubTypeMods pivot, List<GameSubType> allSubTypesInOrder)
	{
		Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
		Dictionary<ushort, GameSubType> dictionary2 = new Dictionary<ushort, GameSubType>();
		ushort num = 1;
		using (List<GameSubType>.Enumerator enumerator = allSubTypesInOrder.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				GameSubType value = enumerator.Current;
				if ((mask & num) == 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.CalculatePivotSubTypes(ushort, GameSubType.SubTypeMods, List<GameSubType>)).MethodHandle;
					}
					dictionary2.Add(num, value);
				}
				else
				{
					dictionary.Add(num, value);
				}
				num = (ushort)(num << 1);
			}
			for (;;)
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
		if (!dictionary2.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
		{
			foreach (KeyValuePair<ushort, GameSubType> keyValuePair in dictionary)
			{
				GameSubType.CCSTSort ccstsort = new GameSubType.CCSTSort(keyValuePair.Value, new GameSubType.SubTypeMods?(pivot));
				ushort num3 = 0;
				GameSubType gameSubType = null;
				foreach (KeyValuePair<ushort, GameSubType> keyValuePair2 in dictionary2)
				{
					if (gameSubType != null)
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
						if (ccstsort.Compare(keyValuePair2.Value, gameSubType) >= 0)
						{
							continue;
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					gameSubType = keyValuePair2.Value;
					num3 = keyValuePair2.Key;
				}
				num2 |= num3;
			}
		}
		return num2;
	}

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

		private GameSubType.SubTypeMods? m_pivot;

		private bool m_originalHasPivot;

		private List<string> m_originalMaps;

		internal CCSTSort(GameSubType original, GameSubType.SubTypeMods? pivot)
		{
			this.m_original = original;
			this.m_pivot = pivot;
			bool originalHasPivot;
			if (pivot != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.CCSTSort..ctor(GameSubType, GameSubType.SubTypeMods?)).MethodHandle;
				}
				originalHasPivot = this.m_original.HasMod(pivot.Value);
			}
			else
			{
				originalHasPivot = false;
			}
			this.m_originalHasPivot = originalHasPivot;
			IEnumerable<GameMapConfig> gameMapConfigs = this.m_original.GameMapConfigs;
			if (GameSubType.CCSTSort.<>f__am$cache0 == null)
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
				GameSubType.CCSTSort.<>f__am$cache0 = ((GameMapConfig p) => p.IsActive);
			}
			IEnumerable<GameMapConfig> source = gameMapConfigs.Where(GameSubType.CCSTSort.<>f__am$cache0);
			if (GameSubType.CCSTSort.<>f__am$cache1 == null)
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
				GameSubType.CCSTSort.<>f__am$cache1 = ((GameMapConfig p) => p.Map);
			}
			this.m_originalMaps = source.Select(GameSubType.CCSTSort.<>f__am$cache1).ToList<string>();
		}

		public int Compare(GameSubType left, GameSubType right)
		{
			if (this.m_pivot != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(GameSubType.CCSTSort.Compare(GameSubType, GameSubType)).MethodHandle;
				}
				bool flag = this.m_originalHasPivot == left.HasMod(this.m_pivot.Value);
				bool flag2 = this.m_originalHasPivot == right.HasMod(this.m_pivot.Value);
				if (flag != flag2)
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
					int result;
					if (flag)
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
						result = 1;
					}
					else
					{
						result = -1;
					}
					return result;
				}
			}
			bool flag3 = this.m_original.LocalizedName == left.LocalizedName;
			bool flag4 = this.m_original.LocalizedName == right.LocalizedName;
			if (flag3 != flag4)
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
				int result2;
				if (flag3)
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
					result2 = -1;
				}
				else
				{
					result2 = 1;
				}
				return result2;
			}
			IEnumerator enumerator = Enum.GetValues(typeof(GameSubType.SubTypeMods)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					GameSubType.SubTypeMods subTypeMods = (GameSubType.SubTypeMods)obj;
					if (this.m_pivot != null)
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
						if (subTypeMods == this.m_pivot.Value)
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
							continue;
						}
					}
					bool flag5 = left.HasMod(subTypeMods);
					bool flag6 = right.HasMod(subTypeMods);
					if (flag5 != flag6)
					{
						int result3;
						if (flag5 == this.m_original.HasMod(subTypeMods))
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
							result3 = -1;
						}
						else
						{
							result3 = 1;
						}
						return result3;
					}
				}
				for (;;)
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
			IEnumerable<GameMapConfig> gameMapConfigs = left.GameMapConfigs;
			if (GameSubType.CCSTSort.<>f__am$cache2 == null)
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
				GameSubType.CCSTSort.<>f__am$cache2 = ((GameMapConfig p) => p.IsActive);
			}
			IEnumerable<GameMapConfig> source = gameMapConfigs.Where(GameSubType.CCSTSort.<>f__am$cache2);
			if (GameSubType.CCSTSort.<>f__am$cache3 == null)
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
				GameSubType.CCSTSort.<>f__am$cache3 = ((GameMapConfig p) => p.Map);
			}
			int num = source.Select(GameSubType.CCSTSort.<>f__am$cache3).ToList<string>().Intersect(this.m_originalMaps).Count<string>();
			IEnumerable<GameMapConfig> gameMapConfigs2 = right.GameMapConfigs;
			if (GameSubType.CCSTSort.<>f__am$cache4 == null)
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
				GameSubType.CCSTSort.<>f__am$cache4 = ((GameMapConfig p) => p.IsActive);
			}
			IEnumerable<GameMapConfig> source2 = gameMapConfigs2.Where(GameSubType.CCSTSort.<>f__am$cache4);
			if (GameSubType.CCSTSort.<>f__am$cache5 == null)
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
				GameSubType.CCSTSort.<>f__am$cache5 = ((GameMapConfig p) => p.Map);
			}
			int num2 = source2.Select(GameSubType.CCSTSort.<>f__am$cache5).ToList<string>().Intersect(this.m_originalMaps).Count<string>();
			if (num != num2)
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
				int result4;
				if (num > num2)
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
}
