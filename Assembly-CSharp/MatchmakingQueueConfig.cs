using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class MatchmakingQueueConfig
{
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

	public float PercentMaxTeamEloDifference;

	public List<EloKeyFlags> MatchmakingElo;

	public List<QueueEntryExperience> BlockedExperienceEntries;

	public GameType BlockedExperienceAlternativeGameType;

	public MissingLastTurnRankedOutcomeType MissingLastTurnRankingImpact;

	public Dictionary<RelaxCheckpoint, TimeSpan> QueueRelaxOverrides;

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

	public SkillMeasureEnum SkillMeasure;

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

	[JsonIgnore]
	public Dictionary<int, RequirementCollection> GroupSizeRestrictions
	{
		get
		{
			if (GroupRules.IsNullOrEmpty())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
			Dictionary<int, RequirementCollection> dictionary = new Dictionary<int, RequirementCollection>();
			foreach (KeyValuePair<int, GroupSizeSpecification> groupRule in GroupRules)
			{
				dictionary.Add(groupRule.Key, (groupRule.Value != null) ? groupRule.Value.Restrictions : RequirementCollection.Create());
			}
			return dictionary;
		}
	}

	[JsonIgnore]
	public List<int> AllViableGroupSizesAllowed
	{
		get
		{
			if (GroupRules.IsNullOrEmpty())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return null;
					}
				}
			}
			return GroupRules.Keys.ToList();
		}
	}

	[JsonIgnore]
	public bool IsMatchmakingGroupSizeDependant => MatchmakingElo.Contains(EloKeyFlags.GROUP);

	public MatchmakingQueueConfig()
	{
		PercentMaxTeamEloDifference = 6.3f;
		QueueMinEloWidth = 100f;
		QueueMaxEloWidth = 600f;
		QueueEloConeStartTime = TimeSpan.Zero;
		QueueEloConeEndTime = TimeSpan.FromMinutes(4.0);
		QueueSizeWhenEnoughPlayers = 80;
		QueueSizeWhenTooManyPlayers = 1000;
		MaxSecondsToSpendBurningFullQueue = 0.33;
		SkillMeasure = SkillMeasureEnum.HANDICAPPED;
		ShowQueueSize = true;
		ReconsiderMaxWaitEntryWithEveryAddition = false;
		MatchmakingElo = new List<EloKeyFlags>();
		BlockedExperienceEntries = new List<QueueEntryExperience>();
		BlockedExperienceAlternativeGameType = GameType.None;
		LeaderboardExpirationTime = TimeSpan.FromDays(14.0);
		LeaderboardMinMatchesForRanking = 10;
		PlacementKFactor = 40f;
		PlacementPointMultipleOnWin = 1f;
		PlacementPointMultipleOnLoss = 1f;
		MMRDeltaNeededForMaxBonus = 100;
		MaximumELOWhenInPlacement = 1975f;
		PreBanPhaseAnimationLength = TimeSpan.FromSeconds(0.0);
		SelectSubPhaseBan1Timeout = TimeSpan.FromSeconds(60.0);
		SelectSubPhaseBan2Timeout = TimeSpan.FromSeconds(30.0);
		SelectSubPhaseFreelancerSelectTimeout = TimeSpan.FromSeconds(30.0);
		SelectSubPhaseTradeTimeout = TimeSpan.FromSeconds(30.0);
		BotsMasqueradeAsHumans = false;
		Penalties = null;
		GameLeavingPointsToGenerateATicket = 10f;
		GameLeavingPointsRedeemedEveryDay = 0.5f;
		MissingLastTurnRankingImpact = MissingLastTurnRankedOutcomeType.WinsCountAsLosses;
		DebugRunTestHarness = false;
		LastSeasonsMMRToInitialPoints = null;
	}

	public IDecayInfo GetDecayInfo(DateTime utcNow)
	{
		MatchmakingQueueDecayInfo matchmakingQueueDecayInfo = new MatchmakingQueueDecayInfo();
		matchmakingQueueDecayInfo.m_leaderboardTiers = LeaderboardTiers;
		matchmakingQueueDecayInfo.UtcNow = utcNow;
		return matchmakingQueueDecayInfo;
	}

	public IEnumerable<byte> GetAllEloRelivantGroupSizes(byte? maxGroupSize)
	{
		if (!IsMatchmakingGroupSizeDependant)
		{
			yield break;
		}
		if (GroupRules.IsNullOrEmpty())
		{
			if (!maxGroupSize.HasValue)
			{
				maxGroupSize = 4;
			}
			byte groupSize2 = 1;
			if (groupSize2 <= maxGroupSize.Value)
			{
				yield return groupSize2;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			while (true)
			{
				switch (3)
				{
				default:
					yield break;
				case 0:
					break;
				}
			}
		}
		using (Dictionary<int, GroupSizeSpecification>.KeyCollection.Enumerator enumerator = GroupRules.Keys.GetEnumerator())
		{
			if (enumerator.MoveNext())
			{
				int groupSize = enumerator.Current;
				yield return (byte)groupSize;
				/*Error: Unable to find new state assignment for yield return*/;
			}
			while (true)
			{
				switch (1)
				{
				default:
					yield break;
				case 0:
					break;
				}
			}
		}
	}

	public IEnumerable<string> GetAllEloKeysForEachRelivantGroupSize(LobbyGameConfig gameConfig, bool isCasual)
	{
		ELOPlayerKey key = ELOPlayerKey.FromConfig(gameConfig.GameType, this, isCasual);
		if (IsMatchmakingGroupSizeDependant)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					byte maxGroupSize = (byte)gameConfig.MaxGroupSize;
					IEnumerator<byte> enumerator = GetAllEloRelivantGroupSizes(maxGroupSize).GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							byte groupSize = enumerator.Current;
							key.InitializePerCharacter(groupSize);
							yield return key.KeyText;
						}
						while (true)
						{
							switch (4)
							{
							default:
								yield break;
							case 0:
								break;
							}
						}
					}
					finally
					{
						if (enumerator != null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									enumerator.Dispose();
									goto end_IL_011c;
								}
							}
						}
						goto end_IL_011c;
						IL_011f:
						switch (2)
						{
						default:
							goto end_IL_011c;
						case 0:
							goto IL_011f;
						}
						end_IL_011c:;
					}
				}
				}
			}
		}
		yield return key.KeyText;
		/*Error: Unable to find new state assignment for yield return*/;
	}

	public int ModifyRankedPointMovement(int unmodifiedPointMovement, int matchesPlayed)
	{
		if (matchesPlayed <= LeaderboardMinMatchesForRanking)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					float num;
					if (unmodifiedPointMovement < 0)
					{
						num = PlacementPointMultipleOnLoss;
					}
					else
					{
						num = PlacementPointMultipleOnWin;
					}
					float num2 = num;
					float num3 = num2 * (float)unmodifiedPointMovement;
					return (int)Math.Round(num3);
				}
				}
			}
		}
		return unmodifiedPointMovement;
	}
}
