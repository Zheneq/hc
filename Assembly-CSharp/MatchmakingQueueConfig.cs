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
			byte groupSize;
			if (!this.IsMatchmakingGroupSizeDependant)
			{
				yield break;
			}
			if (this.GroupRules.IsNullOrEmpty<KeyValuePair<int, GroupSizeSpecification>>())
			{
				if (maxGroupSize == null)
				{
					maxGroupSize = new byte?(4);
				}
				groupSize = 1;
				if (groupSize > maxGroupSize.Value)
				{
					yield break;
				}
				yield return groupSize;
			}
			Dictionary<int, GroupSizeSpecification>.KeyCollection.Enumerator enumerator = this.GroupRules.Keys.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					int groupSize2 = enumerator.Current;
					yield return (byte)groupSize2;
				}
			}
			finally
			{
				// TODO BUG ? can dispose while iterating?
				((IDisposable)enumerator).Dispose();
			}
			yield break;
		}
	}

	public IEnumerable<string> GetAllEloKeysForEachRelivantGroupSize(LobbyGameConfig gameConfig, bool isCasual)
	{
		bool flag;
		ELOPlayerKey key;
		for (;;)
		{
			flag = false;
			key = ELOPlayerKey.FromConfig(gameConfig.GameType, this, isCasual);
			if (this.IsMatchmakingGroupSizeDependant)
			{
				break;
			}
			yield return key.KeyText;
			continue;
		}
		byte maxGroupSize = (byte)gameConfig.MaxGroupSize;
		IEnumerator<byte> enumerator = this.GetAllEloRelivantGroupSizes(new byte?(maxGroupSize)).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				byte groupSize = enumerator.Current;
				key.InitializePerCharacter(groupSize);
				yield return key.KeyText;
				flag = true;
			}
		}
		finally
		{
			if (!flag && enumerator != null)
			{
				enumerator.Dispose();
			}
		}
		yield break;
	}

	public int ModifyRankedPointMovement(int unmodifiedPointMovement, int matchesPlayed)
	{
		if (matchesPlayed <= this.LeaderboardMinMatchesForRanking)
		{
			float num;
			if (unmodifiedPointMovement < 0)
			{
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
