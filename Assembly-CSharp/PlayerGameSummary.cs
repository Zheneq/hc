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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.ResultForWin()).MethodHandle;
			}
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
		if (PlayerGameSummary.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetTotalHealingFromAbility()).MethodHandle;
			}
			PlayerGameSummary.<>f__am$cache0 = ((AbilityGameSummary a) => a.TotalHealing);
		}
		return abilityGameSummaryList.Select(PlayerGameSummary.<>f__am$cache0).Sum();
	}

	public float? GetNumLives()
	{
		if (this.TotalGameTurns > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetNumLives()).MethodHandle;
			}
			return new float?(Math.Max(1f, (float)(this.NumDeaths + 1)));
		}
		return null;
	}

	public float? GetDamageDealtPerTurn()
	{
		if (this.TotalGameTurns > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetDamageDealtPerTurn()).MethodHandle;
			}
			return new float?((float)this.TotalPlayerDamage / (float)this.TotalGameTurns);
		}
		return null;
	}

	public float? GetTeamEnergyBoostedByMePerTurn()
	{
		if (this.TotalGameTurns > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetTeamEnergyBoostedByMePerTurn()).MethodHandle;
			}
			return new float?((float)this.TeamExtraEnergyByEnergizedFromMe / (float)this.TotalGameTurns);
		}
		return null;
	}

	public float? GetTeamDamageSwingByMePerTurn()
	{
		if (this.TotalGameTurns > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetTeamDamageSwingByMePerTurn()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetBoostedDamagePerTurn()).MethodHandle;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetTankingPerLife()).MethodHandle;
			}
			return new float?((float)(this.TotalPlayerDamageReceived + this.NetDamageAvoidedByEvades + this.MyIncomingDamageReducedByCover) / numLives.Value);
		}
		return null;
	}

	public float? GetTeamMitigation()
	{
		float num = (float)(this.TeamIncomingDamageReducedByWeakenedFromMe + this.TotalTeamDamageReceived);
		if (num == 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetTeamMitigation()).MethodHandle;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetSupportPerTurn()).MethodHandle;
			}
			return null;
		}
		return new float?(num / num2);
	}

	public float? GetDamageDonePerLife()
	{
		float? numLives = this.GetNumLives();
		if (numLives != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetDamageDonePerLife()).MethodHandle;
			}
			return new float?((float)this.TotalPlayerDamage / numLives.Value);
		}
		return null;
	}

	public float? GetNetDamageDodgedPerLife()
	{
		float? numLives = this.GetNumLives();
		if (numLives != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetNetDamageDodgedPerLife()).MethodHandle;
			}
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
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetAvgLifeSpan()).MethodHandle;
			}
			return new float?((float)Math.Max(1, this.TotalGameTurns) / numLives.Value);
		}
		return null;
	}

	public float? GetDamageTakenPerTurn()
	{
		if (this.TotalGameTurns > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetDamageTakenPerTurn()).MethodHandle;
			}
			return new float?((float)this.TotalPlayerDamageReceived / (float)this.TotalGameTurns);
		}
		return null;
	}

	public float GetMovementDeniedPerTurn()
	{
		if (this.TotalGameTurns > 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetMovementDeniedPerTurn()).MethodHandle;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetStat(StatDisplaySettings.StatType)).MethodHandle;
			}
			return this.GetIncomingDamageMitigatedByCoverPerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalAssists)
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
			return new float?((float)this.NumAssists);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalDeaths)
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
			return new float?((float)this.NumDeaths);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalBadgePoints)
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
			return new float?(this.TotalBadgePoints);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.MovementDenied)
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
			return new float?(this.GetMovementDeniedPerTurn());
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnergyGainPerTurn)
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
			return new float?(this.EnergyGainPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamagePerTurn)
		{
			return this.GetDamageDealtPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.NetBoostedOutgoingDamage)
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
			return new float?(this.GetBoostedDamagePerTurn());
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageEfficiency)
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
			return new float?(this.DamageEfficiency);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.KillParticipation)
		{
			return new float?(this.KillParticipation);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EffectiveHealAndAbsorb)
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
			return this.GetSupportPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamDamageAdjustedByMe)
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
			return this.GetTeamDamageSwingByMePerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe)
		{
			return this.GetTeamEnergyBoostedByMePerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerLife)
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
			return this.GetDamageTakenPerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnemiesSightedPerLife)
		{
			return new float?(this.EnemiesSightedPerTurn);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTurns)
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
			return new float?((float)this.TotalGameTurns);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTeamDamageReceived)
		{
			return new float?((float)this.TotalTeamDamageReceived);
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TankingPerLife)
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			return this.GetDamageDonePerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerTurn)
		{
			return this.GetDamageTakenPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.AvgLifeSpan)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return this.GetAvgLifeSpan();
		}
		return null;
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		if (-1 < FreelancerStatIndex)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayerGameSummary.GetFreelancerStat(int)).MethodHandle;
			}
			if (FreelancerStatIndex < this.FreelancerStats.Count)
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
				return new float?((float)this.FreelancerStats[FreelancerStatIndex]);
			}
		}
		return null;
	}
}
