using System;
using System.Collections.Generic;
using System.Linq;

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
		return this.Team == 0;
	}

	public bool IsInTeamB()
	{
		return this.Team == 1;
	}

	public bool IsSpectator()
	{
		return this.Team == 3;
	}

	public GameResult ResultForWin()
	{
		GameResult result;
		if (this.IsInTeamA())
		{
			result = GameResult.TeamAWon;
		}
		else
		{
			result = GameResult.TeamBWon;
		}
		return result;
	}

	public int GetTotalGainedXPAccount()
	{
		return this.BaseXPGainedAccount + this.GGXPGainedAccount + this.PlayWithFriendXPGainedAccount + this.QuestXPGainedAccount + this.FirstWinXpGainedAccount + this.QueueTimeXpGainedAccount + this.FreelancerOwnedXPAmount;
	}

	public int GetTotalGainedXPCharacter()
	{
		return this.BaseXPGainedCharacter + this.GGXPGainedCharacter + this.ConsumableXPGainedCharacter + this.PlayWithFriendXPGainedCharacter + this.QuestXPGainedCharacter + this.FirstWinXpGainedCharacter + this.QueueTimeXpGainedCharacter + this.FreelancerOwnedXPAmount;
	}

	public int GetTotalGainedISO()
	{
		return this.BaseISOGained + this.GGISOGained;
	}

	public int GetTotalGainedFreelancerCurrency()
	{
		return this.BaseFreelancerCurrencyGained + this.WinFreelancerCurrencyGained + this.GGFreelancerCurrencyGained + this.EventBonusFreelancerCurrencyGained;
	}

	public int GetContribution()
	{
		return this.TotalPlayerDamage + this.TotalPlayerAbsorb + this.GetTotalHealingFromAbility();
	}

	public int GetTotalHealingFromAbility()
	{
		IEnumerable<AbilityGameSummary> abilityGameSummaryList = this.AbilityGameSummaryList;
		
		return abilityGameSummaryList.Select(((AbilityGameSummary a) => a.TotalHealing)).Sum();
	}

	public float? GetNumLives()
	{
		if (this.TotalGameTurns > 0)
		{
			return new float?(Math.Max(1f, (float)(this.NumDeaths + 1)));
		}
		return null;
	}

	public float? GetDamageDealtPerTurn()
	{
		if (this.TotalGameTurns > 0)
		{
			return new float?((float)this.TotalPlayerDamage / (float)this.TotalGameTurns);
		}
		return null;
	}

	public float? GetTeamEnergyBoostedByMePerTurn()
	{
		if (this.TotalGameTurns > 0)
		{
			return new float?((float)this.TeamExtraEnergyByEnergizedFromMe / (float)this.TotalGameTurns);
		}
		return null;
	}

	public float? GetTeamDamageSwingByMePerTurn()
	{
		if (this.TotalGameTurns > 0)
		{
			return new float?((float)this.GetTotalTeamDamageAdjustedByMe() / (float)this.TotalGameTurns);
		}
		return null;
	}

	public float? GetDamageTakenPerLife()
	{
		float? numLives = this.GetNumLives();
		if (numLives != null)
		{
			return new float?((float)this.TotalPlayerDamageReceived / numLives.Value);
		}
		return null;
	}

	public float GetBoostedDamagePerTurn()
	{
		if (this.TotalGameTurns > 0)
		{
			return (float)this.MyOutgoingExtraDamageFromEmpowered / (float)this.TotalGameTurns;
		}
		return 0f;
	}

	public int GetTotalTeamDamageAdjustedByMe()
	{
		return this.TeamOutgoingDamageIncreasedByEmpoweredFromMe + this.TeamIncomingDamageReducedByWeakenedFromMe;
	}

	public string ToPlayerInfoString()
	{
		return string.Format("- - - - - - - - - -\n[User Name] {0}\n[Account Id] {1}\n[Character Played] {2}\n[Skin] {3}\n[Team] {4}\n", new object[]
		{
			this.InGameName,
			this.AccountId,
			this.CharacterPlayed,
			this.CharacterSkinIndex,
			this.Team
		});
	}

	public float? GetTankingPerLife()
	{
		float? numLives = this.GetNumLives();
		if (numLives != null)
		{
			return new float?((float)(this.TotalPlayerDamageReceived + this.NetDamageAvoidedByEvades + this.MyIncomingDamageReducedByCover) / numLives.Value);
		}
		return null;
	}

	public float? GetTeamMitigation()
	{
		float num = (float)(this.TeamIncomingDamageReducedByWeakenedFromMe + this.TotalTeamDamageReceived);
		if (num == 0f)
		{
			return null;
		}
		float num2 = (float)(this.EffectiveHealing + this.TotalPlayerAbsorb + this.TeamIncomingDamageReducedByWeakenedFromMe);
		return new float?(num2 / num);
	}

	public float? GetSupportPerTurn()
	{
		float num = (float)(this.EffectiveHealing + this.TotalPlayerAbsorb);
		float num2 = (float)this.TotalGameTurns;
		if (num2 == 0f)
		{
			return null;
		}
		return new float?(num / num2);
	}

	public float? GetDamageDonePerLife()
	{
		float? numLives = this.GetNumLives();
		if (numLives != null)
		{
			return new float?((float)this.TotalPlayerDamage / numLives.Value);
		}
		return null;
	}

	public float? GetNetDamageDodgedPerLife()
	{
		float? numLives = this.GetNumLives();
		if (numLives != null)
		{
			return new float?((float)this.NetDamageAvoidedByEvades / numLives.Value);
		}
		return null;
	}

	public float? GetIncomingDamageMitigatedByCoverPerLife()
	{
		float? numLives = this.GetNumLives();
		if (numLives != null)
		{
			return new float?((float)this.MyIncomingDamageReducedByCover / numLives.Value);
		}
		return null;
	}

	public float? GetAvgLifeSpan()
	{
		float? numLives = this.GetNumLives();
		if (numLives != null)
		{
			return new float?((float)Math.Max(1, this.TotalGameTurns) / numLives.Value);
		}
		return null;
	}

	public float? GetDamageTakenPerTurn()
	{
		if (this.TotalGameTurns > 0)
		{
			return new float?((float)this.TotalPlayerDamageReceived / (float)this.TotalGameTurns);
		}
		return null;
	}

	public float GetMovementDeniedPerTurn()
	{
		if (this.TotalGameTurns > 0)
		{
			return this.MovementDeniedByMe / (float)this.TotalGameTurns;
		}
		return 0f;
	}

	public float? GetStat(StatDisplaySettings.StatType TypeOfStat)
	{
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageDodgeByEvade)
		{
			return this.GetNetDamageDodgedPerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageReducedByCover)
		{
			return this.GetIncomingDamageMitigatedByCoverPerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalAssists)
		{
			return new float?((float)this.NumAssists);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalDeaths)
		{
			return new float?((float)this.NumDeaths);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalBadgePoints)
		{
			return new float?(this.TotalBadgePoints);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.MovementDenied)
		{
			return new float?(this.GetMovementDeniedPerTurn());
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnergyGainPerTurn)
		{
			return new float?(this.EnergyGainPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamagePerTurn)
		{
			return this.GetDamageDealtPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.NetBoostedOutgoingDamage)
		{
			return new float?(this.GetBoostedDamagePerTurn());
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageEfficiency)
		{
			return new float?(this.DamageEfficiency);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.KillParticipation)
		{
			return new float?(this.KillParticipation);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EffectiveHealAndAbsorb)
		{
			return this.GetSupportPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamDamageAdjustedByMe)
		{
			return this.GetTeamDamageSwingByMePerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe)
		{
			return this.GetTeamEnergyBoostedByMePerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerLife)
		{
			return this.GetDamageTakenPerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnemiesSightedPerLife)
		{
			return new float?(this.EnemiesSightedPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTurns)
		{
			return new float?((float)this.TotalGameTurns);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTeamDamageReceived)
		{
			return new float?((float)this.TotalTeamDamageReceived);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TankingPerLife)
		{
			return this.GetTankingPerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamMitigation)
		{
			return this.GetTeamMitigation();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.SupportPerTurn)
		{
			return this.GetSupportPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageDonePerLife)
		{
			return this.GetDamageDonePerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerTurn)
		{
			return this.GetDamageTakenPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.AvgLifeSpan)
		{
			return this.GetAvgLifeSpan();
		}
		return null;
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		if (-1 < FreelancerStatIndex)
		{
			if (FreelancerStatIndex < this.FreelancerStats.Count)
			{
				return new float?((float)this.FreelancerStats[FreelancerStatIndex]);
			}
		}
		return null;
	}
}
