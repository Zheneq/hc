using System;
using System.Collections.Generic;

[Serializable]
public class PersistedStats : ICloneable
{
	public PersistedStatEntry TotalDeaths
	{
		get;
		set;
	}

	public PersistedStatEntry TotalPlayerKills
	{
		get;
		set;
	}

	public PersistedStatEntry TotalPlayerAssists
	{
		get;
		set;
	}

	public PersistedStatEntry TotalPlayerDamage
	{
		get;
		set;
	}

	public PersistedStatEntry TotalPlayerHealing
	{
		get;
		set;
	}

	public PersistedStatEntry TotalPlayerAbsorb
	{
		get;
		set;
	}

	public PersistedStatEntry TotalPlayerDamageReceived
	{
		get;
		set;
	}

	public PersistedStatFloatEntry TotalBadgePoints
	{
		get;
		set;
	}

	public PersistedStatEntry NetDamageAvoidedByEvades
	{
		get;
		set;
	}

	public PersistedStatFloatEntry NetDamageAvoidedByEvadesPerLife
	{
		get;
		set;
	}

	public PersistedStatEntry DamageDodgedByEvades
	{
		get;
		set;
	}

	public PersistedStatEntry DamageInterceptedByEvades
	{
		get;
		set;
	}

	public PersistedStatEntry MyIncomingDamageReducedByCover
	{
		get;
		set;
	}

	public PersistedStatFloatEntry MyIncomingDamageReducedByCoverPerLife
	{
		get;
		set;
	}

	public PersistedStatEntry MyOutgoingDamageReducedByCover
	{
		get;
		set;
	}

	public PersistedStatEntry MyOutgoingExtraDamageFromEmpowered
	{
		get;
		set;
	}

	public PersistedStatEntry MyOutgoingDamageReducedFromWeakened
	{
		get;
		set;
	}

	public PersistedStatEntry TeamOutgoingDamageIncreasedByEmpoweredFromMe
	{
		get;
		set;
	}

	public PersistedStatEntry TeamIncomingDamageReducedByWeakenedFromMe
	{
		get;
		set;
	}

	public PersistedStatFloatEntry MovementDeniedByMePerTurn
	{
		get;
		set;
	}

	public PersistedStatFloatEntry EnergyGainPerTurn
	{
		get;
		set;
	}

	public PersistedStatFloatEntry DamagePerTurn
	{
		get;
		set;
	}

	public PersistedStatFloatEntry BoostedOutgoingDamagePerTurn
	{
		get;
		set;
	}

	public PersistedStatFloatEntry DamageEfficiency
	{
		get;
		set;
	}

	public PersistedStatFloatEntry KillParticipation
	{
		get;
		set;
	}

	public PersistedStatEntry EffectiveHealing
	{
		get;
		set;
	}

	public PersistedStatEntry TeamDamageAdjustedByMe
	{
		get;
		set;
	}

	public PersistedStatFloatEntry TeamDamageSwingByMePerTurn
	{
		get;
		set;
	}

	public PersistedStatEntry TeamExtraEnergyByEnergizedFromMe
	{
		get;
		set;
	}

	public PersistedStatFloatEntry TeamBoostedEnergyByMePerTurn
	{
		get;
		set;
	}

	public PersistedStatEntry TeamDamageReceived
	{
		get;
		set;
	}

	public PersistedStatFloatEntry DamageTakenPerLife
	{
		get;
		set;
	}

	public PersistedStatFloatEntry EnemiesSightedPerTurn
	{
		get;
		set;
	}

	public PersistedStatFloatEntry TotalTurns
	{
		get;
		set;
	}

	public PersistedStatFloatEntry TankingPerLife
	{
		get;
		set;
	}

	public PersistedStatFloatEntry TeamMitigation
	{
		get;
		set;
	}

	public PersistedStatFloatEntry SupportPerTurn
	{
		get;
		set;
	}

	public PersistedStatFloatEntry DamageDonePerLife
	{
		get;
		set;
	}

	public PersistedStatFloatEntry DamageTakenPerTurn
	{
		get;
		set;
	}

	public PersistedStatFloatEntry AvgLifeSpan
	{
		get;
		set;
	}

	public PersistedStatFloatEntry SecondsPlayed
	{
		get;
		set;
	}

	public PersistedStatEntry MatchesWon
	{
		get;
		set;
	}

	public List<PersistedStatEntry> FreelancerSpecificStats
	{
		get;
		set;
	}

	public PersistedStats()
	{
		TotalDeaths = new PersistedStatEntry();
		TotalPlayerKills = new PersistedStatEntry();
		TotalPlayerAssists = new PersistedStatEntry();
		TotalPlayerDamage = new PersistedStatEntry();
		TotalPlayerHealing = new PersistedStatEntry();
		TotalPlayerAbsorb = new PersistedStatEntry();
		TotalPlayerDamageReceived = new PersistedStatEntry();
		TotalBadgePoints = new PersistedStatFloatEntry();
		NetDamageAvoidedByEvades = new PersistedStatEntry();
		NetDamageAvoidedByEvadesPerLife = new PersistedStatFloatEntry();
		DamageDodgedByEvades = new PersistedStatEntry();
		DamageInterceptedByEvades = new PersistedStatEntry();
		MyIncomingDamageReducedByCover = new PersistedStatEntry();
		MyIncomingDamageReducedByCoverPerLife = new PersistedStatFloatEntry();
		MyOutgoingDamageReducedByCover = new PersistedStatEntry();
		MyOutgoingExtraDamageFromEmpowered = new PersistedStatEntry();
		MyOutgoingDamageReducedFromWeakened = new PersistedStatEntry();
		TeamOutgoingDamageIncreasedByEmpoweredFromMe = new PersistedStatEntry();
		TeamIncomingDamageReducedByWeakenedFromMe = new PersistedStatEntry();
		MovementDeniedByMePerTurn = new PersistedStatFloatEntry();
		EnergyGainPerTurn = new PersistedStatFloatEntry();
		DamagePerTurn = new PersistedStatFloatEntry();
		BoostedOutgoingDamagePerTurn = new PersistedStatFloatEntry();
		DamageEfficiency = new PersistedStatFloatEntry();
		KillParticipation = new PersistedStatFloatEntry();
		EffectiveHealing = new PersistedStatEntry();
		TeamDamageAdjustedByMe = new PersistedStatEntry();
		TeamDamageSwingByMePerTurn = new PersistedStatFloatEntry();
		TeamExtraEnergyByEnergizedFromMe = new PersistedStatEntry();
		TeamBoostedEnergyByMePerTurn = new PersistedStatFloatEntry();
		TeamDamageReceived = new PersistedStatEntry();
		DamageTakenPerLife = new PersistedStatFloatEntry();
		EnemiesSightedPerTurn = new PersistedStatFloatEntry();
		TotalTurns = new PersistedStatFloatEntry();
		TankingPerLife = new PersistedStatFloatEntry();
		TeamMitigation = new PersistedStatFloatEntry();
		SupportPerTurn = new PersistedStatFloatEntry();
		DamageDonePerLife = new PersistedStatFloatEntry();
		DamageTakenPerTurn = new PersistedStatFloatEntry();
		AvgLifeSpan = new PersistedStatFloatEntry();
		SecondsPlayed = new PersistedStatFloatEntry();
		MatchesWon = new PersistedStatEntry();
		FreelancerSpecificStats = new List<PersistedStatEntry>();
	}

	public IPersistedGameplayStat GetGameplayStat(StatDisplaySettings.StatType TypeOfStat)
	{
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageDodgeByEvade)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return NetDamageAvoidedByEvadesPerLife;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalBadgePoints)
		{
			return TotalBadgePoints;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageReducedByCover)
		{
			return MyIncomingDamageReducedByCoverPerLife;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalAssists)
		{
			return TotalPlayerAssists;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalDeaths)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return TotalDeaths;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.MovementDenied)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return MovementDeniedByMePerTurn;
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
			return DamagePerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.NetBoostedOutgoingDamage)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return BoostedOutgoingDamagePerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageEfficiency)
		{
			while (true)
			{
				switch (1)
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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return KillParticipation;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EffectiveHealAndAbsorb)
		{
			return SupportPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamDamageAdjustedByMe)
		{
			return TeamDamageSwingByMePerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return TeamBoostedEnergyByMePerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerLife)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return DamageTakenPerLife;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnemiesSightedPerLife)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return EnemiesSightedPerTurn;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TankingPerLife)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return TankingPerLife;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageDonePerLife)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return DamageDonePerLife;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamMitigation)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return TeamMitigation;
				}
			}
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTurns)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return TotalTurns;
				}
			}
		}
		switch (TypeOfStat)
		{
		case StatDisplaySettings.StatType.TotalTeamDamageReceived:
			return TeamDamageReceived;
		case StatDisplaySettings.StatType.SupportPerTurn:
			return SupportPerTurn;
		case StatDisplaySettings.StatType.DamageTakenPerTurn:
			return DamageTakenPerTurn;
		case StatDisplaySettings.StatType.SecondsPlayed:
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				return SecondsPlayed;
			}
		case StatDisplaySettings.StatType.MatchesWon:
			return MatchesWon;
		default:
			Log.Warning("Attempting to display a stat that isn't categorized: " + TypeOfStat);
			return null;
		}
	}

	public void CombineStats(PersistedStats StatsToBeMerged)
	{
		TotalDeaths.CombineStats(StatsToBeMerged.TotalDeaths);
		TotalPlayerKills.CombineStats(StatsToBeMerged.TotalPlayerKills);
		TotalPlayerAssists.CombineStats(StatsToBeMerged.TotalPlayerAssists);
		TotalPlayerDamage.CombineStats(StatsToBeMerged.TotalPlayerDamage);
		TotalPlayerHealing.CombineStats(StatsToBeMerged.TotalPlayerHealing);
		TotalPlayerAbsorb.CombineStats(StatsToBeMerged.TotalPlayerAbsorb);
		TotalPlayerDamageReceived.CombineStats(StatsToBeMerged.TotalPlayerDamageReceived);
		TotalBadgePoints.CombineStats(StatsToBeMerged.TotalBadgePoints);
		NetDamageAvoidedByEvades.CombineStats(StatsToBeMerged.NetDamageAvoidedByEvades);
		NetDamageAvoidedByEvadesPerLife.CombineStats(StatsToBeMerged.NetDamageAvoidedByEvadesPerLife);
		DamageDodgedByEvades.CombineStats(StatsToBeMerged.DamageDodgedByEvades);
		DamageInterceptedByEvades.CombineStats(StatsToBeMerged.DamageInterceptedByEvades);
		MyIncomingDamageReducedByCover.CombineStats(StatsToBeMerged.MyIncomingDamageReducedByCover);
		MyIncomingDamageReducedByCoverPerLife.CombineStats(StatsToBeMerged.MyIncomingDamageReducedByCoverPerLife);
		MyOutgoingDamageReducedByCover.CombineStats(StatsToBeMerged.MyOutgoingDamageReducedByCover);
		MyOutgoingExtraDamageFromEmpowered.CombineStats(StatsToBeMerged.MyOutgoingExtraDamageFromEmpowered);
		MyOutgoingDamageReducedFromWeakened.CombineStats(StatsToBeMerged.MyOutgoingDamageReducedFromWeakened);
		TeamOutgoingDamageIncreasedByEmpoweredFromMe.CombineStats(StatsToBeMerged.TeamOutgoingDamageIncreasedByEmpoweredFromMe);
		TeamIncomingDamageReducedByWeakenedFromMe.CombineStats(StatsToBeMerged.TeamIncomingDamageReducedByWeakenedFromMe);
		MovementDeniedByMePerTurn.CombineStats(StatsToBeMerged.MovementDeniedByMePerTurn);
		EnergyGainPerTurn.CombineStats(StatsToBeMerged.EnergyGainPerTurn);
		DamagePerTurn.CombineStats(StatsToBeMerged.DamagePerTurn);
		BoostedOutgoingDamagePerTurn.CombineStats(StatsToBeMerged.BoostedOutgoingDamagePerTurn);
		DamageEfficiency.CombineStats(StatsToBeMerged.DamageEfficiency);
		KillParticipation.CombineStats(StatsToBeMerged.KillParticipation);
		EffectiveHealing.CombineStats(StatsToBeMerged.EffectiveHealing);
		TeamDamageAdjustedByMe.CombineStats(StatsToBeMerged.TeamDamageAdjustedByMe);
		TeamDamageSwingByMePerTurn.CombineStats(StatsToBeMerged.TeamDamageSwingByMePerTurn);
		TeamExtraEnergyByEnergizedFromMe.CombineStats(StatsToBeMerged.TeamExtraEnergyByEnergizedFromMe);
		TeamBoostedEnergyByMePerTurn.CombineStats(StatsToBeMerged.TeamBoostedEnergyByMePerTurn);
		TeamDamageReceived.CombineStats(StatsToBeMerged.TeamDamageReceived);
		DamageTakenPerLife.CombineStats(StatsToBeMerged.DamageTakenPerLife);
		EnemiesSightedPerTurn.CombineStats(StatsToBeMerged.EnemiesSightedPerTurn);
		TotalTurns.CombineStats(StatsToBeMerged.TotalTurns);
		TankingPerLife.CombineStats(StatsToBeMerged.TankingPerLife);
		TeamMitigation.CombineStats(StatsToBeMerged.TeamMitigation);
		SupportPerTurn.CombineStats(StatsToBeMerged.SupportPerTurn);
		DamageDonePerLife.CombineStats(StatsToBeMerged.DamageDonePerLife);
		DamageTakenPerTurn.CombineStats(StatsToBeMerged.DamageTakenPerTurn);
		SecondsPlayed.CombineStats(StatsToBeMerged.SecondsPlayed);
		MatchesWon.CombineStats(StatsToBeMerged.MatchesWon);
		for (int i = 0; i < FreelancerSpecificStats.Count; i++)
		{
			if (i < StatsToBeMerged.FreelancerSpecificStats.Count)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				FreelancerSpecificStats[i].CombineStats(StatsToBeMerged.FreelancerSpecificStats[i]);
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public PersistedStatEntry GetFreelancerStat(int index)
	{
		if (FreelancerSpecificStats != null)
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
			if (-1 < index)
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
				if (index < FreelancerSpecificStats.Count)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return FreelancerSpecificStats[index];
						}
					}
				}
			}
		}
		return new PersistedStatEntry();
	}

	public object Clone()
	{
		PersistedStats persistedStats = new PersistedStats();
		persistedStats.TotalDeaths = TotalDeaths.GetCopy();
		persistedStats.TotalPlayerKills = TotalPlayerKills.GetCopy();
		persistedStats.TotalPlayerAssists = TotalPlayerAssists.GetCopy();
		persistedStats.TotalPlayerDamage = TotalPlayerDamage.GetCopy();
		persistedStats.TotalPlayerHealing = TotalPlayerHealing.GetCopy();
		persistedStats.TotalPlayerAbsorb = TotalPlayerAbsorb.GetCopy();
		persistedStats.TotalPlayerDamageReceived = TotalPlayerDamageReceived.GetCopy();
		persistedStats.TotalBadgePoints = TotalBadgePoints.GetCopy();
		persistedStats.NetDamageAvoidedByEvades = NetDamageAvoidedByEvades.GetCopy();
		persistedStats.NetDamageAvoidedByEvadesPerLife = NetDamageAvoidedByEvadesPerLife.GetCopy();
		persistedStats.DamageDodgedByEvades = DamageDodgedByEvades.GetCopy();
		persistedStats.DamageInterceptedByEvades = DamageInterceptedByEvades.GetCopy();
		persistedStats.MyIncomingDamageReducedByCover = MyIncomingDamageReducedByCover.GetCopy();
		persistedStats.MyIncomingDamageReducedByCoverPerLife = MyIncomingDamageReducedByCoverPerLife.GetCopy();
		persistedStats.MyOutgoingDamageReducedByCover = MyOutgoingDamageReducedByCover.GetCopy();
		persistedStats.MyOutgoingDamageReducedFromWeakened = MyOutgoingDamageReducedFromWeakened.GetCopy();
		persistedStats.MyOutgoingExtraDamageFromEmpowered = MyOutgoingExtraDamageFromEmpowered.GetCopy();
		persistedStats.TeamIncomingDamageReducedByWeakenedFromMe = TeamIncomingDamageReducedByWeakenedFromMe.GetCopy();
		persistedStats.TeamOutgoingDamageIncreasedByEmpoweredFromMe = TeamOutgoingDamageIncreasedByEmpoweredFromMe.GetCopy();
		persistedStats.MovementDeniedByMePerTurn = MovementDeniedByMePerTurn.GetCopy();
		persistedStats.EnergyGainPerTurn = EnergyGainPerTurn.GetCopy();
		persistedStats.DamagePerTurn = DamagePerTurn.GetCopy();
		persistedStats.BoostedOutgoingDamagePerTurn = BoostedOutgoingDamagePerTurn.GetCopy();
		persistedStats.DamageEfficiency = DamageEfficiency.GetCopy();
		persistedStats.KillParticipation = KillParticipation.GetCopy();
		persistedStats.EffectiveHealing = EffectiveHealing.GetCopy();
		persistedStats.TeamDamageAdjustedByMe = TeamDamageAdjustedByMe.GetCopy();
		persistedStats.TeamDamageSwingByMePerTurn = TeamDamageSwingByMePerTurn.GetCopy();
		persistedStats.TeamExtraEnergyByEnergizedFromMe = TeamExtraEnergyByEnergizedFromMe.GetCopy();
		persistedStats.TeamBoostedEnergyByMePerTurn = TeamBoostedEnergyByMePerTurn.GetCopy();
		persistedStats.TeamDamageReceived = TeamDamageReceived.GetCopy();
		persistedStats.DamageTakenPerLife = DamageTakenPerLife.GetCopy();
		persistedStats.EnemiesSightedPerTurn = EnemiesSightedPerTurn.GetCopy();
		persistedStats.TotalTurns = TotalTurns.GetCopy();
		persistedStats.TankingPerLife = TankingPerLife.GetCopy();
		persistedStats.TeamMitigation = TeamMitigation.GetCopy();
		persistedStats.SupportPerTurn = SupportPerTurn.GetCopy();
		persistedStats.DamageDonePerLife = DamageDonePerLife.GetCopy();
		persistedStats.DamageTakenPerTurn = DamageTakenPerTurn.GetCopy();
		persistedStats.SecondsPlayed = SecondsPlayed.GetCopy();
		persistedStats.MatchesWon = MatchesWon.GetCopy();
		if (FreelancerSpecificStats == null)
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
			persistedStats.FreelancerSpecificStats = null;
		}
		else
		{
			persistedStats.FreelancerSpecificStats = new List<PersistedStatEntry>();
			for (int i = 0; i < FreelancerSpecificStats.Count; i++)
			{
				PersistedStatEntry persistedStatEntry = new PersistedStatEntry();
				persistedStatEntry = (PersistedStatEntry)FreelancerSpecificStats[i].Clone();
				persistedStats.FreelancerSpecificStats.Add(persistedStatEntry);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return persistedStats;
	}
}
