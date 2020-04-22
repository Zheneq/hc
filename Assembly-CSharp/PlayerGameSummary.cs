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
		int result;
		if (IsInTeamA())
		{
			result = 2;
		}
		else
		{
			result = 3;
		}
		return (GameResult)result;
	}

	public int GetTotalGainedXPAccount()
	{
		return BaseXPGainedAccount + GGXPGainedAccount + PlayWithFriendXPGainedAccount + QuestXPGainedAccount + FirstWinXpGainedAccount + QueueTimeXpGainedAccount + FreelancerOwnedXPAmount;
	}

	public int GetTotalGainedXPCharacter()
	{
		return BaseXPGainedCharacter + GGXPGainedCharacter + ConsumableXPGainedCharacter + PlayWithFriendXPGainedCharacter + QuestXPGainedCharacter + FirstWinXpGainedCharacter + QueueTimeXpGainedCharacter + FreelancerOwnedXPAmount;
	}

	public int GetTotalGainedISO()
	{
		return BaseISOGained + GGISOGained;
	}

	public int GetTotalGainedFreelancerCurrency()
	{
		return BaseFreelancerCurrencyGained + WinFreelancerCurrencyGained + GGFreelancerCurrencyGained + EventBonusFreelancerCurrencyGained;
	}

	public int GetContribution()
	{
		return TotalPlayerDamage + TotalPlayerAbsorb + GetTotalHealingFromAbility();
	}

	public int GetTotalHealingFromAbility()
	{
		List<AbilityGameSummary> abilityGameSummaryList = AbilityGameSummaryList;
		
		return abilityGameSummaryList.Select(((AbilityGameSummary a) => a.TotalHealing)).Sum();
	}

	public float? GetNumLives()
	{
		if (TotalGameTurns > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return Math.Max(1f, NumDeaths + 1);
				}
			}
		}
		return null;
	}

	public float? GetDamageDealtPerTurn()
	{
		if (TotalGameTurns > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return (float)TotalPlayerDamage / (float)TotalGameTurns;
				}
			}
		}
		return null;
	}

	public float? GetTeamEnergyBoostedByMePerTurn()
	{
		if (TotalGameTurns > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return (float)TeamExtraEnergyByEnergizedFromMe / (float)TotalGameTurns;
				}
			}
		}
		return null;
	}

	public float? GetTeamDamageSwingByMePerTurn()
	{
		if (TotalGameTurns > 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return (float)GetTotalTeamDamageAdjustedByMe() / (float)TotalGameTurns;
				}
			}
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return (float)MyOutgoingExtraDamageFromEmpowered / (float)TotalGameTurns;
				}
			}
		}
		return 0f;
	}

	public int GetTotalTeamDamageAdjustedByMe()
	{
		return TeamOutgoingDamageIncreasedByEmpoweredFromMe + TeamIncomingDamageReducedByWeakenedFromMe;
	}

	public string ToPlayerInfoString()
	{
		return $"- - - - - - - - - -\n[User Name] {InGameName}\n[Account Id] {AccountId}\n[Character Played] {CharacterPlayed}\n[Skin] {CharacterSkinIndex}\n[Team] {Team}\n";
	}

	public float? GetTankingPerLife()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return (float)(TotalPlayerDamageReceived + NetDamageAvoidedByEvades + MyIncomingDamageReducedByCover) / numLives.Value;
				}
			}
		}
		return null;
	}

	public float? GetTeamMitigation()
	{
		float num = TeamIncomingDamageReducedByWeakenedFromMe + TotalTeamDamageReceived;
		if (num == 0f)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		float num2 = EffectiveHealing + TotalPlayerAbsorb + TeamIncomingDamageReducedByWeakenedFromMe;
		return num2 / num;
	}

	public float? GetSupportPerTurn()
	{
		float num = EffectiveHealing + TotalPlayerAbsorb;
		float num2 = TotalGameTurns;
		if (num2 == 0f)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return null;
				}
			}
		}
		return num / num2;
	}

	public float? GetDamageDonePerLife()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return (float)TotalPlayerDamage / numLives.Value;
				}
			}
		}
		return null;
	}

	public float? GetNetDamageDodgedPerLife()
	{
		float? numLives = GetNumLives();
		if (numLives.HasValue)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return (float)NetDamageAvoidedByEvades / numLives.Value;
				}
			}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return (float)Math.Max(1, TotalGameTurns) / numLives.Value;
				}
			}
		}
		return null;
	}

	public float? GetDamageTakenPerTurn()
	{
		if (TotalGameTurns > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return (float)TotalPlayerDamageReceived / (float)TotalGameTurns;
				}
			}
		}
		return null;
	}

	public float GetMovementDeniedPerTurn()
	{
		if (TotalGameTurns > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return MovementDeniedByMe / (float)TotalGameTurns;
				}
			}
		}
		return 0f;
	}

	public float? GetStat(StatDisplaySettings.StatType TypeOfStat)
	{
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageDodgeByEvade)
		{
			return GetNetDamageDodgedPerLife();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageReducedByCover)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return GetIncomingDamageMitigatedByCoverPerLife();
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalAssists)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return NumAssists;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalDeaths)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return NumDeaths;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalBadgePoints)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return TotalBadgePoints;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.MovementDenied)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return GetMovementDeniedPerTurn();
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnergyGainPerTurn)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return EnergyGainPerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamagePerTurn)
		{
			return GetDamageDealtPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.NetBoostedOutgoingDamage)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return GetBoostedDamagePerTurn();
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageEfficiency)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return DamageEfficiency;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.KillParticipation)
		{
			return KillParticipation;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EffectiveHealAndAbsorb)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return GetSupportPerTurn();
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamDamageAdjustedByMe)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return GetTeamDamageSwingByMePerTurn();
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe)
		{
			return GetTeamEnergyBoostedByMePerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerLife)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return GetDamageTakenPerLife();
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnemiesSightedPerLife)
		{
			return EnemiesSightedPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTurns)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return TotalGameTurns;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTeamDamageReceived)
		{
			return TotalTeamDamageReceived;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TankingPerLife)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return GetTankingPerLife();
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamMitigation)
		{
			return GetTeamMitigation();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.SupportPerTurn)
		{
			return GetSupportPerTurn();
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageDonePerLife)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return GetDamageDonePerLife();
				}
			}
		}
		switch (TypeOfStat)
		{
		case StatDisplaySettings.StatType.DamageTakenPerTurn:
			return GetDamageTakenPerTurn();
		case StatDisplaySettings.StatType.AvgLifeSpan:
			while (true)
			{
				return GetAvgLifeSpan();
			}
		default:
			return null;
		}
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		if (-1 < FreelancerStatIndex)
		{
			if (FreelancerStatIndex < FreelancerStats.Count)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return FreelancerStats[FreelancerStatIndex];
					}
				}
			}
		}
		return null;
	}
}
