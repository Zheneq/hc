using System;
using System.Collections.Generic;

[Serializable]
public class PersistedStats : ICloneable
{
	public PersistedStats()
	{
		this.TotalDeaths = new PersistedStatEntry();
		this.TotalPlayerKills = new PersistedStatEntry();
		this.TotalPlayerAssists = new PersistedStatEntry();
		this.TotalPlayerDamage = new PersistedStatEntry();
		this.TotalPlayerHealing = new PersistedStatEntry();
		this.TotalPlayerAbsorb = new PersistedStatEntry();
		this.TotalPlayerDamageReceived = new PersistedStatEntry();
		this.TotalBadgePoints = new PersistedStatFloatEntry();
		this.NetDamageAvoidedByEvades = new PersistedStatEntry();
		this.NetDamageAvoidedByEvadesPerLife = new PersistedStatFloatEntry();
		this.DamageDodgedByEvades = new PersistedStatEntry();
		this.DamageInterceptedByEvades = new PersistedStatEntry();
		this.MyIncomingDamageReducedByCover = new PersistedStatEntry();
		this.MyIncomingDamageReducedByCoverPerLife = new PersistedStatFloatEntry();
		this.MyOutgoingDamageReducedByCover = new PersistedStatEntry();
		this.MyOutgoingExtraDamageFromEmpowered = new PersistedStatEntry();
		this.MyOutgoingDamageReducedFromWeakened = new PersistedStatEntry();
		this.TeamOutgoingDamageIncreasedByEmpoweredFromMe = new PersistedStatEntry();
		this.TeamIncomingDamageReducedByWeakenedFromMe = new PersistedStatEntry();
		this.MovementDeniedByMePerTurn = new PersistedStatFloatEntry();
		this.EnergyGainPerTurn = new PersistedStatFloatEntry();
		this.DamagePerTurn = new PersistedStatFloatEntry();
		this.BoostedOutgoingDamagePerTurn = new PersistedStatFloatEntry();
		this.DamageEfficiency = new PersistedStatFloatEntry();
		this.KillParticipation = new PersistedStatFloatEntry();
		this.EffectiveHealing = new PersistedStatEntry();
		this.TeamDamageAdjustedByMe = new PersistedStatEntry();
		this.TeamDamageSwingByMePerTurn = new PersistedStatFloatEntry();
		this.TeamExtraEnergyByEnergizedFromMe = new PersistedStatEntry();
		this.TeamBoostedEnergyByMePerTurn = new PersistedStatFloatEntry();
		this.TeamDamageReceived = new PersistedStatEntry();
		this.DamageTakenPerLife = new PersistedStatFloatEntry();
		this.EnemiesSightedPerTurn = new PersistedStatFloatEntry();
		this.TotalTurns = new PersistedStatFloatEntry();
		this.TankingPerLife = new PersistedStatFloatEntry();
		this.TeamMitigation = new PersistedStatFloatEntry();
		this.SupportPerTurn = new PersistedStatFloatEntry();
		this.DamageDonePerLife = new PersistedStatFloatEntry();
		this.DamageTakenPerTurn = new PersistedStatFloatEntry();
		this.AvgLifeSpan = new PersistedStatFloatEntry();
		this.SecondsPlayed = new PersistedStatFloatEntry();
		this.MatchesWon = new PersistedStatEntry();
		this.FreelancerSpecificStats = new List<PersistedStatEntry>();
	}

	public PersistedStatEntry TotalDeaths { get; set; }

	public PersistedStatEntry TotalPlayerKills { get; set; }

	public PersistedStatEntry TotalPlayerAssists { get; set; }

	public PersistedStatEntry TotalPlayerDamage { get; set; }

	public PersistedStatEntry TotalPlayerHealing { get; set; }

	public PersistedStatEntry TotalPlayerAbsorb { get; set; }

	public PersistedStatEntry TotalPlayerDamageReceived { get; set; }

	public PersistedStatFloatEntry TotalBadgePoints { get; set; }

	public PersistedStatEntry NetDamageAvoidedByEvades { get; set; }

	public PersistedStatFloatEntry NetDamageAvoidedByEvadesPerLife { get; set; }

	public PersistedStatEntry DamageDodgedByEvades { get; set; }

	public PersistedStatEntry DamageInterceptedByEvades { get; set; }

	public PersistedStatEntry MyIncomingDamageReducedByCover { get; set; }

	public PersistedStatFloatEntry MyIncomingDamageReducedByCoverPerLife { get; set; }

	public PersistedStatEntry MyOutgoingDamageReducedByCover { get; set; }

	public PersistedStatEntry MyOutgoingExtraDamageFromEmpowered { get; set; }

	public PersistedStatEntry MyOutgoingDamageReducedFromWeakened { get; set; }

	public PersistedStatEntry TeamOutgoingDamageIncreasedByEmpoweredFromMe { get; set; }

	public PersistedStatEntry TeamIncomingDamageReducedByWeakenedFromMe { get; set; }

	public PersistedStatFloatEntry MovementDeniedByMePerTurn { get; set; }

	public PersistedStatFloatEntry EnergyGainPerTurn { get; set; }

	public PersistedStatFloatEntry DamagePerTurn { get; set; }

	public PersistedStatFloatEntry BoostedOutgoingDamagePerTurn { get; set; }

	public PersistedStatFloatEntry DamageEfficiency { get; set; }

	public PersistedStatFloatEntry KillParticipation { get; set; }

	public PersistedStatEntry EffectiveHealing { get; set; }

	public PersistedStatEntry TeamDamageAdjustedByMe { get; set; }

	public PersistedStatFloatEntry TeamDamageSwingByMePerTurn { get; set; }

	public PersistedStatEntry TeamExtraEnergyByEnergizedFromMe { get; set; }

	public PersistedStatFloatEntry TeamBoostedEnergyByMePerTurn { get; set; }

	public PersistedStatEntry TeamDamageReceived { get; set; }

	public PersistedStatFloatEntry DamageTakenPerLife { get; set; }

	public PersistedStatFloatEntry EnemiesSightedPerTurn { get; set; }

	public PersistedStatFloatEntry TotalTurns { get; set; }

	public PersistedStatFloatEntry TankingPerLife { get; set; }

	public PersistedStatFloatEntry TeamMitigation { get; set; }

	public PersistedStatFloatEntry SupportPerTurn { get; set; }

	public PersistedStatFloatEntry DamageDonePerLife { get; set; }

	public PersistedStatFloatEntry DamageTakenPerTurn { get; set; }

	public PersistedStatFloatEntry AvgLifeSpan { get; set; }

	public PersistedStatFloatEntry SecondsPlayed { get; set; }

	public PersistedStatEntry MatchesWon { get; set; }

	public List<PersistedStatEntry> FreelancerSpecificStats { get; set; }

	public IPersistedGameplayStat GetGameplayStat(StatDisplaySettings.StatType TypeOfStat)
	{
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageDodgeByEvade)
		{
			return this.NetDamageAvoidedByEvadesPerLife;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalBadgePoints)
		{
			return this.TotalBadgePoints;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.IncomingDamageReducedByCover)
		{
			return this.MyIncomingDamageReducedByCoverPerLife;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalAssists)
		{
			return this.TotalPlayerAssists;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalDeaths)
		{
			return this.TotalDeaths;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.MovementDenied)
		{
			return this.MovementDeniedByMePerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnergyGainPerTurn)
		{
			return this.EnergyGainPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamagePerTurn)
		{
			return this.DamagePerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.NetBoostedOutgoingDamage)
		{
			return this.BoostedOutgoingDamagePerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageEfficiency)
		{
			return this.DamageEfficiency;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.KillParticipation)
		{
			return this.KillParticipation;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EffectiveHealAndAbsorb)
		{
			return this.SupportPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamDamageAdjustedByMe)
		{
			return this.TeamDamageSwingByMePerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe)
		{
			return this.TeamBoostedEnergyByMePerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerLife)
		{
			return this.DamageTakenPerLife;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.EnemiesSightedPerLife)
		{
			return this.EnemiesSightedPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TankingPerLife)
		{
			return this.TankingPerLife;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageDonePerLife)
		{
			return this.DamageDonePerLife;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TeamMitigation)
		{
			return this.TeamMitigation;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTurns)
		{
			return this.TotalTurns;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.TotalTeamDamageReceived)
		{
			return this.TeamDamageReceived;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.SupportPerTurn)
		{
			return this.SupportPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.DamageTakenPerTurn)
		{
			return this.DamageTakenPerTurn;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.SecondsPlayed)
		{
			return this.SecondsPlayed;
		}
		if (TypeOfStat == StatDisplaySettings.StatType.MatchesWon)
		{
			return this.MatchesWon;
		}
		Log.Warning("Attempting to display a stat that isn't categorized: " + TypeOfStat, new object[0]);
		return null;
	}

	public void CombineStats(PersistedStats StatsToBeMerged)
	{
		this.TotalDeaths.CombineStats(StatsToBeMerged.TotalDeaths);
		this.TotalPlayerKills.CombineStats(StatsToBeMerged.TotalPlayerKills);
		this.TotalPlayerAssists.CombineStats(StatsToBeMerged.TotalPlayerAssists);
		this.TotalPlayerDamage.CombineStats(StatsToBeMerged.TotalPlayerDamage);
		this.TotalPlayerHealing.CombineStats(StatsToBeMerged.TotalPlayerHealing);
		this.TotalPlayerAbsorb.CombineStats(StatsToBeMerged.TotalPlayerAbsorb);
		this.TotalPlayerDamageReceived.CombineStats(StatsToBeMerged.TotalPlayerDamageReceived);
		this.TotalBadgePoints.CombineStats(StatsToBeMerged.TotalBadgePoints);
		this.NetDamageAvoidedByEvades.CombineStats(StatsToBeMerged.NetDamageAvoidedByEvades);
		this.NetDamageAvoidedByEvadesPerLife.CombineStats(StatsToBeMerged.NetDamageAvoidedByEvadesPerLife);
		this.DamageDodgedByEvades.CombineStats(StatsToBeMerged.DamageDodgedByEvades);
		this.DamageInterceptedByEvades.CombineStats(StatsToBeMerged.DamageInterceptedByEvades);
		this.MyIncomingDamageReducedByCover.CombineStats(StatsToBeMerged.MyIncomingDamageReducedByCover);
		this.MyIncomingDamageReducedByCoverPerLife.CombineStats(StatsToBeMerged.MyIncomingDamageReducedByCoverPerLife);
		this.MyOutgoingDamageReducedByCover.CombineStats(StatsToBeMerged.MyOutgoingDamageReducedByCover);
		this.MyOutgoingExtraDamageFromEmpowered.CombineStats(StatsToBeMerged.MyOutgoingExtraDamageFromEmpowered);
		this.MyOutgoingDamageReducedFromWeakened.CombineStats(StatsToBeMerged.MyOutgoingDamageReducedFromWeakened);
		this.TeamOutgoingDamageIncreasedByEmpoweredFromMe.CombineStats(StatsToBeMerged.TeamOutgoingDamageIncreasedByEmpoweredFromMe);
		this.TeamIncomingDamageReducedByWeakenedFromMe.CombineStats(StatsToBeMerged.TeamIncomingDamageReducedByWeakenedFromMe);
		this.MovementDeniedByMePerTurn.CombineStats(StatsToBeMerged.MovementDeniedByMePerTurn);
		this.EnergyGainPerTurn.CombineStats(StatsToBeMerged.EnergyGainPerTurn);
		this.DamagePerTurn.CombineStats(StatsToBeMerged.DamagePerTurn);
		this.BoostedOutgoingDamagePerTurn.CombineStats(StatsToBeMerged.BoostedOutgoingDamagePerTurn);
		this.DamageEfficiency.CombineStats(StatsToBeMerged.DamageEfficiency);
		this.KillParticipation.CombineStats(StatsToBeMerged.KillParticipation);
		this.EffectiveHealing.CombineStats(StatsToBeMerged.EffectiveHealing);
		this.TeamDamageAdjustedByMe.CombineStats(StatsToBeMerged.TeamDamageAdjustedByMe);
		this.TeamDamageSwingByMePerTurn.CombineStats(StatsToBeMerged.TeamDamageSwingByMePerTurn);
		this.TeamExtraEnergyByEnergizedFromMe.CombineStats(StatsToBeMerged.TeamExtraEnergyByEnergizedFromMe);
		this.TeamBoostedEnergyByMePerTurn.CombineStats(StatsToBeMerged.TeamBoostedEnergyByMePerTurn);
		this.TeamDamageReceived.CombineStats(StatsToBeMerged.TeamDamageReceived);
		this.DamageTakenPerLife.CombineStats(StatsToBeMerged.DamageTakenPerLife);
		this.EnemiesSightedPerTurn.CombineStats(StatsToBeMerged.EnemiesSightedPerTurn);
		this.TotalTurns.CombineStats(StatsToBeMerged.TotalTurns);
		this.TankingPerLife.CombineStats(StatsToBeMerged.TankingPerLife);
		this.TeamMitigation.CombineStats(StatsToBeMerged.TeamMitigation);
		this.SupportPerTurn.CombineStats(StatsToBeMerged.SupportPerTurn);
		this.DamageDonePerLife.CombineStats(StatsToBeMerged.DamageDonePerLife);
		this.DamageTakenPerTurn.CombineStats(StatsToBeMerged.DamageTakenPerTurn);
		this.SecondsPlayed.CombineStats(StatsToBeMerged.SecondsPlayed);
		this.MatchesWon.CombineStats(StatsToBeMerged.MatchesWon);
		for (int i = 0; i < this.FreelancerSpecificStats.Count; i++)
		{
			if (i < StatsToBeMerged.FreelancerSpecificStats.Count)
			{
				this.FreelancerSpecificStats[i].CombineStats(StatsToBeMerged.FreelancerSpecificStats[i]);
			}
		}
	}

	public PersistedStatEntry GetFreelancerStat(int index)
	{
		if (this.FreelancerSpecificStats != null)
		{
			if (-1 < index)
			{
				if (index < this.FreelancerSpecificStats.Count)
				{
					return this.FreelancerSpecificStats[index];
				}
			}
		}
		return new PersistedStatEntry();
	}

	public object Clone()
	{
		PersistedStats persistedStats = new PersistedStats();
		persistedStats.TotalDeaths = this.TotalDeaths.GetCopy();
		persistedStats.TotalPlayerKills = this.TotalPlayerKills.GetCopy();
		persistedStats.TotalPlayerAssists = this.TotalPlayerAssists.GetCopy();
		persistedStats.TotalPlayerDamage = this.TotalPlayerDamage.GetCopy();
		persistedStats.TotalPlayerHealing = this.TotalPlayerHealing.GetCopy();
		persistedStats.TotalPlayerAbsorb = this.TotalPlayerAbsorb.GetCopy();
		persistedStats.TotalPlayerDamageReceived = this.TotalPlayerDamageReceived.GetCopy();
		persistedStats.TotalBadgePoints = this.TotalBadgePoints.GetCopy();
		persistedStats.NetDamageAvoidedByEvades = this.NetDamageAvoidedByEvades.GetCopy();
		persistedStats.NetDamageAvoidedByEvadesPerLife = this.NetDamageAvoidedByEvadesPerLife.GetCopy();
		persistedStats.DamageDodgedByEvades = this.DamageDodgedByEvades.GetCopy();
		persistedStats.DamageInterceptedByEvades = this.DamageInterceptedByEvades.GetCopy();
		persistedStats.MyIncomingDamageReducedByCover = this.MyIncomingDamageReducedByCover.GetCopy();
		persistedStats.MyIncomingDamageReducedByCoverPerLife = this.MyIncomingDamageReducedByCoverPerLife.GetCopy();
		persistedStats.MyOutgoingDamageReducedByCover = this.MyOutgoingDamageReducedByCover.GetCopy();
		persistedStats.MyOutgoingDamageReducedFromWeakened = this.MyOutgoingDamageReducedFromWeakened.GetCopy();
		persistedStats.MyOutgoingExtraDamageFromEmpowered = this.MyOutgoingExtraDamageFromEmpowered.GetCopy();
		persistedStats.TeamIncomingDamageReducedByWeakenedFromMe = this.TeamIncomingDamageReducedByWeakenedFromMe.GetCopy();
		persistedStats.TeamOutgoingDamageIncreasedByEmpoweredFromMe = this.TeamOutgoingDamageIncreasedByEmpoweredFromMe.GetCopy();
		persistedStats.MovementDeniedByMePerTurn = this.MovementDeniedByMePerTurn.GetCopy();
		persistedStats.EnergyGainPerTurn = this.EnergyGainPerTurn.GetCopy();
		persistedStats.DamagePerTurn = this.DamagePerTurn.GetCopy();
		persistedStats.BoostedOutgoingDamagePerTurn = this.BoostedOutgoingDamagePerTurn.GetCopy();
		persistedStats.DamageEfficiency = this.DamageEfficiency.GetCopy();
		persistedStats.KillParticipation = this.KillParticipation.GetCopy();
		persistedStats.EffectiveHealing = this.EffectiveHealing.GetCopy();
		persistedStats.TeamDamageAdjustedByMe = this.TeamDamageAdjustedByMe.GetCopy();
		persistedStats.TeamDamageSwingByMePerTurn = this.TeamDamageSwingByMePerTurn.GetCopy();
		persistedStats.TeamExtraEnergyByEnergizedFromMe = this.TeamExtraEnergyByEnergizedFromMe.GetCopy();
		persistedStats.TeamBoostedEnergyByMePerTurn = this.TeamBoostedEnergyByMePerTurn.GetCopy();
		persistedStats.TeamDamageReceived = this.TeamDamageReceived.GetCopy();
		persistedStats.DamageTakenPerLife = this.DamageTakenPerLife.GetCopy();
		persistedStats.EnemiesSightedPerTurn = this.EnemiesSightedPerTurn.GetCopy();
		persistedStats.TotalTurns = this.TotalTurns.GetCopy();
		persistedStats.TankingPerLife = this.TankingPerLife.GetCopy();
		persistedStats.TeamMitigation = this.TeamMitigation.GetCopy();
		persistedStats.SupportPerTurn = this.SupportPerTurn.GetCopy();
		persistedStats.DamageDonePerLife = this.DamageDonePerLife.GetCopy();
		persistedStats.DamageTakenPerTurn = this.DamageTakenPerTurn.GetCopy();
		persistedStats.SecondsPlayed = this.SecondsPlayed.GetCopy();
		persistedStats.MatchesWon = this.MatchesWon.GetCopy();
		if (this.FreelancerSpecificStats == null)
		{
			persistedStats.FreelancerSpecificStats = null;
		}
		else
		{
			persistedStats.FreelancerSpecificStats = new List<PersistedStatEntry>();
			for (int i = 0; i < this.FreelancerSpecificStats.Count; i++)
			{
				PersistedStatEntry item = new PersistedStatEntry();
				item = (PersistedStatEntry)this.FreelancerSpecificStats[i].Clone();
				persistedStats.FreelancerSpecificStats.Add(item);
			}
		}
		return persistedStats;
	}
}
