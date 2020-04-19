using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

[Serializable]
public class MatchmakingQueueConfig
{
	public float PercentMaxTeamEloDifference;

	public List<MatchmakingQueueConfig.EloKeyFlags> MatchmakingElo;

	public List<MatchmakingQueueConfig.QueueEntryExperience> BlockedExperienceEntries;

	public GameType BlockedExperienceAlternativeGameType;

	public MatchmakingQueueConfig.MissingLastTurnRankedOutcomeType MissingLastTurnRankingImpact;

	public Dictionary<MatchmakingQueueConfig.RelaxCheckpoint, TimeSpan> QueueRelaxOverrides;

	public float QueueMinEloWidth;

	public float QueueMaxEloWidth;

	public TimeSpan QueueEloConeStartTime;

	public TimeSpan QueueEloConeEndTime;

	public int QueueMaxGroupImbalance;

	public TimeSpan? QueueGroupImbalanceStartTime;

	public TimeSpan? QueueGroupImbalanceEndTime;

	public int QueueSizeWhenEnoughPlayers;

	public int QueueSizeWhenTooManyPlayers;

	public double MaxSecondsToSpendBurningFullQueue;

	public MatchmakingQueueConfig.SkillMeasureEnum SkillMeasure;

	public Dictionary<int, GroupSizeSpecification> GroupRules;

	public bool ShowQueueSize;

	public int MaxWillFillPerTeam;

	public bool ReconsiderMaxWaitEntryWithEveryAddition;

	public int LeaderboardMinMatchesForRanking;

	public float PlacementKFactor;

	public float PlacementPointMultipleOnWin;

	public float PlacementPointMultipleOnLoss;

	public float MaximumELOWhenInPlacement;

	public int MMRDeltaNeededForMaxBonus;

	public TimeSpan LeaderboardExpirationTime;

	public List<TierInfo> LeaderboardTiers;

	public Dictionary<int, int> LastSeasonsMMRToInitialPoints;

	public TimeSpan PreBanPhaseAnimationLength;

	public TimeSpan SelectSubPhaseBan1Timeout;

	public TimeSpan SelectSubPhaseBan2Timeout;

	public TimeSpan SelectSubPhaseFreelancerSelectTimeout;

	public TimeSpan SelectSubPhaseTradeTimeout;

	public bool BotsMasqueradeAsHumans;

	public Penalties Penalties;

	public float GameLeavingPointsToGenerateATicket;

	public float GameLeavingPointsRedeemedEveryDay;

	public bool DebugRunTestHarness;

	public MatchmakingQueueConfig()
	{
		this.PercentMaxTeamEloDifference = 6.3f;
		this.QueueMinEloWidth = 100f;
		this.QueueMaxEloWidth = 600f;
		this.QueueEloConeStartTime = TimeSpan.Zero;
		this.QueueEloConeEndTime = TimeSpan.FromMinutes(4.0);
		this.QueueSizeWhenEnoughPlayers = 0x50;
		this.QueueSizeWhenTooManyPlayers = 0x3E8;
		this.MaxSecondsToSpendBurningFullQueue = 0.33;
		this.SkillMeasure = MatchmakingQueueConfig.SkillMeasureEnum.HANDICAPPED;
		this.ShowQueueSize = true;
		this.ReconsiderMaxWaitEntryWithEveryAddition = false;
		this.MatchmakingElo = new List<MatchmakingQueueConfig.EloKeyFlags>();
		this.BlockedExperienceEntries = new List<MatchmakingQueueConfig.QueueEntryExperience>();
		this.BlockedExperienceAlternativeGameType = GameType.None;
		this.LeaderboardExpirationTime = TimeSpan.FromDays(14.0);
		this.LeaderboardMinMatchesForRanking = 0xA;
		this.PlacementKFactor = 40f;
		this.PlacementPointMultipleOnWin = 1f;
		this.PlacementPointMultipleOnLoss = 1f;
		this.MMRDeltaNeededForMaxBonus = 0x64;
		this.MaximumELOWhenInPlacement = 1975f;
		this.PreBanPhaseAnimationLength = TimeSpan.FromSeconds(0.0);
		this.SelectSubPhaseBan1Timeout = TimeSpan.FromSeconds(60.0);
		this.SelectSubPhaseBan2Timeout = TimeSpan.FromSeconds(30.0);
		this.SelectSubPhaseFreelancerSelectTimeout = TimeSpan.FromSeconds(30.0);
		this.SelectSubPhaseTradeTimeout = TimeSpan.FromSeconds(30.0);
		this.BotsMasqueradeAsHumans = false;
		this.Penalties = null;
		this.GameLeavingPointsToGenerateATicket = 10f;
		this.GameLeavingPointsRedeemedEveryDay = 0.5f;
		this.MissingLastTurnRankingImpact = MatchmakingQueueConfig.MissingLastTurnRankedOutcomeType.WinsCountAsLosses;
		this.DebugRunTestHarness = false;
		this.LastSeasonsMMRToInitialPoints = null;
	}

	[JsonIgnore]
	public Dictionary<int, RequirementCollection> GroupSizeRestrictions
	{
		get
		{
			if (this.GroupRules.IsNullOrEmpty<KeyValuePair<int, GroupSizeSpecification>>())
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(MatchmakingQueueConfig.get_GroupSizeRestrictions()).MethodHandle;
				}
				return null;
			}
			Dictionary<int, RequirementCollection> dictionary = new Dictionary<int, RequirementCollection>();
			foreach (KeyValuePair<int, GroupSizeSpecification> keyValuePair in this.GroupRules)
			{
				dictionary.Add(keyValuePair.Key, (keyValuePair.Value != null) ? keyValuePair.Value.Restrictions : RequirementCollection.Create());
			}
			return dictionary;
		}
	}

	[JsonIgnore]
	public List<int> AllViableGroupSizesAllowed
	{
		get
		{
			if (this.GroupRules.IsNullOrEmpty<KeyValuePair<int, GroupSizeSpecification>>())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MatchmakingQueueConfig.get_AllViableGroupSizesAllowed()).MethodHandle;
				}
				return null;
			}
			return this.GroupRules.Keys.ToList<int>();
		}
	}

	[JsonIgnore]
	public bool IsMatchmakingGroupSizeDependant
	{
		get
		{
			return this.MatchmakingElo.Contains(MatchmakingQueueConfig.EloKeyFlags.GROUP);
		}
	}

	public IDecayInfo GetDecayInfo(DateTime utcNow)
	{
		return new MatchmakingQueueDecayInfo
		{
			m_leaderboardTiers = this.LeaderboardTiers,
			UtcNow = utcNow
		};
	}

	public IEnumerable<byte> GetAllEloRelivantGroupSizes(byte? maxGroupSize)
	{
		for (;;)
		{
			bool flag = false;
			uint num;
			byte groupSize;
			switch (num)
			{
			case 0U:
				if (!this.IsMatchmakingGroupSizeDependant)
				{
					goto IL_18A;
				}
				if (this.GroupRules.IsNullOrEmpty<KeyValuePair<int, GroupSizeSpecification>>())
				{
					if (maxGroupSize == null)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(MatchmakingQueueConfig.<GetAllEloRelivantGroupSizes>c__Iterator0.MoveNext()).MethodHandle;
						}
						maxGroupSize = new byte?(4);
					}
					groupSize = 1;
					goto IL_C0;
				}
				goto IL_E2;
			case 1U:
				groupSize += 1;
				goto IL_C0;
			case 2U:
				goto IL_104;
			}
			goto Block_0;
			IL_C0:
			if (groupSize > maxGroupSize.Value)
			{
				break;
			}
			yield return groupSize;
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
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		goto IL_18A;
		Block_0:
		yield break;
		IL_E2:
		Dictionary<int, GroupSizeSpecification>.KeyCollection.Enumerator enumerator = this.GroupRules.Keys.GetEnumerator();
		try
		{
			IL_104:
			while (enumerator.MoveNext())
			{
				int groupSize2 = enumerator.Current;
				yield return (byte)groupSize2;
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = true;
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
		finally
		{
			bool flag;
			if (flag)
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
			}
			else
			{
				((IDisposable)enumerator).Dispose();
			}
		}
		IL_18A:
		yield break;
	}

	public IEnumerable<string> GetAllEloKeysForEachRelivantGroupSize(LobbyGameConfig gameConfig, bool isCasual)
	{
		bool flag;
		ELOPlayerKey key;
		for (;;)
		{
			flag = false;
			uint num;
			switch (num)
			{
			case 0U:
				key = ELOPlayerKey.FromConfig(gameConfig.GameType, this, isCasual);
				if (this.IsMatchmakingGroupSizeDependant)
				{
					goto IL_5B;
				}
				yield return key.KeyText;
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				continue;
			case 1U:
				goto IL_A8;
			case 2U:
				goto IL_178;
			}
			goto Block_0;
		}
		for (;;)
		{
			IL_5B:
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(MatchmakingQueueConfig.<GetAllEloKeysForEachRelivantGroupSize>c__Iterator1.MoveNext()).MethodHandle;
		}
		byte maxGroupSize = (byte)gameConfig.MaxGroupSize;
		IEnumerator<byte> enumerator = this.GetAllEloRelivantGroupSizes(new byte?(maxGroupSize)).GetEnumerator();
		goto Block_3;
		Block_0:
		yield break;
		Block_3:
		try
		{
			IL_A8:
			while (enumerator.MoveNext())
			{
				byte groupSize = enumerator.Current;
				key.InitializePerCharacter(groupSize);
				yield return key.KeyText;
				flag = true;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		finally
		{
			if (flag)
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
			}
			else if (enumerator != null)
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
				enumerator.Dispose();
			}
		}
		IL_178:
		yield break;
	}

	public int ModifyRankedPointMovement(int unmodifiedPointMovement, int matchesPlayed)
	{
		if (matchesPlayed <= this.LeaderboardMinMatchesForRanking)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MatchmakingQueueConfig.ModifyRankedPointMovement(int, int)).MethodHandle;
			}
			float num;
			if (unmodifiedPointMovement < 0)
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
				num = this.PlacementPointMultipleOnLoss;
			}
			else
			{
				num = this.PlacementPointMultipleOnWin;
			}
			float num2 = num;
			float num3 = num2 * (float)unmodifiedPointMovement;
			return (int)Math.Round((double)num3);
		}
		return unmodifiedPointMovement;
	}

	public enum EloKeyFlags
	{
		GROUP,
		RELATIVE,
		SOFTENED_PUBLIC,
		SOFTENED_INDIVIDUAL,
		QUEUE
	}

	public enum QueueEntryExperience
	{
		NewPlayer,
		Expert,
		MixedGroup
	}

	public enum MissingLastTurnRankedOutcomeType
	{
		WinsCountAsLosses,
		WinsCountAsWins,
		WinsIgnored
	}

	public enum RelaxCheckpoint
	{
		UNDEFINED,
		StartMatchmaking,
		AllowExpertCollision,
		AllowNoobCollision,
		IgnoreEloDifference,
		PadWithBots,
		RelaxCoopDifficultyRequest,
		AllowNoobExpertMixing,
		AllowRoleImbalance,
		AllowMissingRoles,
		AllowMajorTeamEloImbalance,
		AllowRepeatSameOpponents,
		AbandonWillFillParity,
		AbandonRegionUniqueness,
		ConsiderNonDefaultGroupComp,
		MaxTime
	}

	public enum SkillMeasureEnum
	{
		HANDICAPPED,
		ACCOUNT,
		CHARACTER,
		HANDICAPPED_OR_HIGHEST
	}
}
