using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class PlayerGameSummary : StatDisplaySettings.IPersistatedStatValueSupplier
{
	public long AccountId;
	public string InGameName;
	public CharacterType CharacterPlayed;
	public FreelancerSelectionOnus FreelancerSelectionOnus;
	public string CharacterName;
	public int CharacterSkinIndex;
	public int Team;
	public int TeamSlot;
	public int PlayerId;
	public string PrepCatalystName;
	public string DashCatalystName;
	public string CombatCatalystName;
	public bool PrepCatalystUsed;
	public bool DashCatalystUsed;
	public bool CombatCatalystUsed;
	public int PowerupsCollected;
	public PlayerGameResult PlayerGameResult = PlayerGameResult.NoResult;
	public MatchResultsStats MatchResults;
	public int TotalGameTurns;
	public int TotalPotentialAbsorb;
	public int TotalTechPointsGained;
	public int TotalHealingReceived;
	public int TotalAbsorbReceived;
	public int NumKills;
	public int NumDeaths;
	public int NumAssists;
	public int TotalPlayerDamage;
	public int TotalPlayerHealing;
	public int TotalPlayerAbsorb;
	public int TotalPlayerDamageReceived;
	public int TotalTeamDamageReceived;
	public int NetDamageAvoidedByEvades;
	public int DamageAvoidedByEvades;
	public int DamageInterceptedByEvades;
	public int MyIncomingDamageReducedByCover;
	public int MyOutgoingDamageReducedByCover;
	public int MyOutgoingExtraDamageFromEmpowered;
	public int MyOutgoingReducedDamageFromWeakened;
	public int TeamOutgoingDamageIncreasedByEmpoweredFromMe;
	public int TeamIncomingDamageReducedByWeakenedFromMe;
	public float MovementDeniedByMe;
	public float EnergyGainPerTurn;
	public float DamageEfficiency;
	public float KillParticipation;
	public int EffectiveHealing;
	public int EffectiveHealingFromAbility;
	public int TeamExtraEnergyByEnergizedFromMe;
	public float EnemiesSightedPerTurn;
	public int CharacterSpecificStat;
	public List<int> FreelancerStats = new List<int>();
	public int BaseISOGained;
	public int BaseFreelancerCurrencyGained;
	public int BaseXPGainedAccount;
	public int BaseXPGainedCharacter;
	public int WinFreelancerCurrencyGained;
	public int FirstWinFreelancerCurrencyGained;
	public int EventBonusFreelancerCurrencyGained;
	public int GGPacksSelfUsed;
	public int GGNonSelfCount;
	public int GGISOGained;
	public int GGFreelancerCurrencyGained;
	public int GGXPGainedAccount;
	public int GGXPGainedCharacter;
	public int ConsumableXPGainedCharacter;
	public int PlayWithFriendXPGainedAccount;
	public int PlayWithFriendXPGainedCharacter;
	public int QuestXPGainedAccount;
	public int QuestXPGainedCharacter;
	public int EventBonusXPGainedAccount;
	public int EventBonusXPGainedCharacter;
	public int FirstWinXpGainedAccount;
	public int FirstWinXpGainedCharacter;
	public int QueueTimeXpGainedAccount;
	public int QueueTimeXpGainedCharacter;
	public int FreelancerOwnedXPAmount;
	public int ActorIndex;
	public TimeSpan QueueTime;
	public List<AbilityGameSummary> AbilityGameSummaryList = new List<AbilityGameSummary>();
	public List<int> TimebankUsage = new List<int>();
	public Dictionary<string, float> AccountEloDeltas;
	public Dictionary<string, float> CharacterEloDeltas;
	public int RankedSortKarmaDelta;
	public float UsedMMR;
	public float TotalBadgePoints;

	public bool IsInTeamA()
	{
		return Team == 0;
	}

	public bool IsInTeamB()
	{
		return Team == 1;
	}

	public bool IsSpectator()
	{
		return Team == 3;
	}

	public GameResult ResultForWin()
	{
		return IsInTeamA() ? GameResult.TeamAWon : GameResult.TeamBWon;
	}

	public int GetTotalGainedXPAccount()
	{
		return BaseXPGainedAccount
		       + GGXPGainedAccount
		       + PlayWithFriendXPGainedAccount
		       + QuestXPGainedAccount
		       + FirstWinXpGainedAccount
		       + QueueTimeXpGainedAccount
		       + FreelancerOwnedXPAmount;
	}

	public int GetTotalGainedXPCharacter()
	{
		return BaseXPGainedCharacter
		       + GGXPGainedCharacter
		       + ConsumableXPGainedCharacter
		       + PlayWithFriendXPGainedCharacter
		       + QuestXPGainedCharacter
		       + FirstWinXpGainedCharacter
		       + QueueTimeXpGainedCharacter
		       + FreelancerOwnedXPAmount;
	}

	public int GetTotalGainedISO()
	{
		return BaseISOGained + GGISOGained;
	}

	public int GetTotalGainedFreelancerCurrency()
	{
		return BaseFreelancerCurrencyGained
		       + WinFreelancerCurrencyGained
		       + GGFreelancerCurrencyGained
		       + EventBonusFreelancerCurrencyGained;
	}

	public int GetContribution()
	{
		return TotalPlayerDamage + TotalPlayerAbsorb + GetTotalHealingFromAbility();
	}

	public int GetTotalHealingFromAbility()
	{
		return AbilityGameSummaryList.Select(a => a.TotalHealing).Sum();
	}

	public float? GetNumLives()
	{
		if (TotalGameTurns > 0)
		{
			return Math.Max(1f, NumDeaths + 1);
		}
		return null;
	}

	public float? GetDamageDealtPerTurn()
	{
		if (TotalGameTurns > 0)
		{
			return (float)TotalPlayerDamage / (float)TotalGameTurns;
		}
		return null;
	}

	public float? GetTeamEnergyBoostedByMePerTurn()
	{
		if (TotalGameTurns > 0)
		{
			return (float)TeamExtraEnergyByEnergizedFromMe / (float)TotalGameTurns;
		}
		return null;
	}

	public float? GetTeamDamageSwingByMePerTurn()
	{
		if (TotalGameTurns > 0)
		{
			return (float)GetTotalTeamDamageAdjustedByMe() / (float)TotalGameTurns;
		}
		return null;
	}

	public float? GetDamageTakenPerLife()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			return (float)TotalPlayerDamageReceived / numLives.Value;
		}
		return null;
	}

	public float GetBoostedDamagePerTurn()
	{
		if (TotalGameTurns > 0)
		{
			return (float)MyOutgoingExtraDamageFromEmpowered / (float)TotalGameTurns;
		}
		return 0f;
	}

	public int GetTotalTeamDamageAdjustedByMe()
	{
		return TeamOutgoingDamageIncreasedByEmpoweredFromMe + TeamIncomingDamageReducedByWeakenedFromMe;
	}

	public string ToPlayerInfoString()
	{
		return new StringBuilder().Append("- - - - - - - - - -\n[User Name] ").Append(InGameName).Append("\n[Account Id] ").Append(AccountId).Append("\n[Character Played] ").Append(CharacterPlayed).Append("\n[Skin] ").Append(CharacterSkinIndex).Append("\n[Team] ").Append(Team).Append("\n").ToString();
	}

	public float? GetTankingPerLife()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			return (float)(TotalPlayerDamageReceived + NetDamageAvoidedByEvades + MyIncomingDamageReducedByCover) / numLives.Value;
		}
		return null;
	}

	public float? GetTeamMitigation()
	{
		float potentialTotalDamage = TeamIncomingDamageReducedByWeakenedFromMe + TotalTeamDamageReceived;
		if (potentialTotalDamage == 0f)
		{
			return null;
		}
		float totalMitigation = EffectiveHealing + TotalPlayerAbsorb + TeamIncomingDamageReducedByWeakenedFromMe;
		return totalMitigation / potentialTotalDamage;
	}

	public float? GetSupportPerTurn()
	{
		float support = EffectiveHealing + TotalPlayerAbsorb;
		float turns = TotalGameTurns;
		if (turns == 0f)
		{
			return null;
		}
		return support / turns;
	}

	public float? GetDamageDonePerLife()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			return (float)TotalPlayerDamage / numLives.Value;
		}
		return null;
	}

	public float? GetNetDamageDodgedPerLife()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			return (float)NetDamageAvoidedByEvades / numLives.Value;
		}
		return null;
	}

	public float? GetIncomingDamageMitigatedByCoverPerLife()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			return (float)MyIncomingDamageReducedByCover / numLives.Value;
		}
		return null;
	}

	public float? GetAvgLifeSpan()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			return (float)Math.Max(1, TotalGameTurns) / numLives.Value;
		}
		return null;
	}

	public float? GetDamageTakenPerTurn()
	{
		if (TotalGameTurns > 0)
		{
			return (float)TotalPlayerDamageReceived / (float)TotalGameTurns;
		}
		return null;
	}

	public float GetMovementDeniedPerTurn()
	{
		if (TotalGameTurns > 0)
		{
			return MovementDeniedByMe / (float)TotalGameTurns;
		}
		return 0f;
	}

	public float? GetStat(StatDisplaySettings.StatType TypeOfStat)
	{
		switch (TypeOfStat)
		{
			case StatDisplaySettings.StatType.IncomingDamageDodgeByEvade:
				return GetNetDamageDodgedPerLife();
			case StatDisplaySettings.StatType.IncomingDamageReducedByCover:
				return GetIncomingDamageMitigatedByCoverPerLife();
			case StatDisplaySettings.StatType.TotalAssists:
				return NumAssists;
			case StatDisplaySettings.StatType.TotalDeaths:
				return NumDeaths;
			case StatDisplaySettings.StatType.TotalBadgePoints:
				return TotalBadgePoints;
			case StatDisplaySettings.StatType.MovementDenied:
				return GetMovementDeniedPerTurn();
			case StatDisplaySettings.StatType.EnergyGainPerTurn:
				return EnergyGainPerTurn;
			case StatDisplaySettings.StatType.DamagePerTurn:
				return GetDamageDealtPerTurn();
			case StatDisplaySettings.StatType.NetBoostedOutgoingDamage:
				return GetBoostedDamagePerTurn();
			case StatDisplaySettings.StatType.DamageEfficiency:
				return DamageEfficiency;
			case StatDisplaySettings.StatType.KillParticipation:
				return KillParticipation;
			case StatDisplaySettings.StatType.EffectiveHealAndAbsorb:
				return GetSupportPerTurn();
			case StatDisplaySettings.StatType.TeamDamageAdjustedByMe:
				return GetTeamDamageSwingByMePerTurn();
			case StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe:
				return GetTeamEnergyBoostedByMePerTurn();
			case StatDisplaySettings.StatType.DamageTakenPerLife:
				return GetDamageTakenPerLife();
			case StatDisplaySettings.StatType.EnemiesSightedPerLife:
				return EnemiesSightedPerTurn;
			case StatDisplaySettings.StatType.TotalTurns:
				return TotalGameTurns;
			case StatDisplaySettings.StatType.TotalTeamDamageReceived:
				return TotalTeamDamageReceived;
			case StatDisplaySettings.StatType.TankingPerLife:
				return GetTankingPerLife();
			case StatDisplaySettings.StatType.TeamMitigation:
				return GetTeamMitigation();
			case StatDisplaySettings.StatType.SupportPerTurn:
				return GetSupportPerTurn();
			case StatDisplaySettings.StatType.DamageDonePerLife:
				return GetDamageDonePerLife();
			case StatDisplaySettings.StatType.DamageTakenPerTurn:
				return GetDamageTakenPerTurn();
			case StatDisplaySettings.StatType.AvgLifeSpan:
				return GetAvgLifeSpan();
			default:
				return null;
		}
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		if (-1 < FreelancerStatIndex && FreelancerStatIndex < FreelancerStats.Count)
		{
			return FreelancerStats[FreelancerStatIndex];
		}
		return null;
	}
}
