using System;
using System.Collections.Generic;

[Serializable]
public class PersistedStats : ICloneable
{
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
		switch (TypeOfStat)
		{
			case StatDisplaySettings.StatType.IncomingDamageDodgeByEvade:
				return NetDamageAvoidedByEvadesPerLife;
			case StatDisplaySettings.StatType.TotalBadgePoints:
				return TotalBadgePoints;
			case StatDisplaySettings.StatType.IncomingDamageReducedByCover:
				return MyIncomingDamageReducedByCoverPerLife;
			case StatDisplaySettings.StatType.TotalAssists:
				return TotalPlayerAssists;
			case StatDisplaySettings.StatType.TotalDeaths:
				return TotalDeaths;
			case StatDisplaySettings.StatType.MovementDenied:
				return MovementDeniedByMePerTurn;
			case StatDisplaySettings.StatType.EnergyGainPerTurn:
				return EnergyGainPerTurn;
			case StatDisplaySettings.StatType.DamagePerTurn:
				return DamagePerTurn;
			case StatDisplaySettings.StatType.NetBoostedOutgoingDamage:
				return BoostedOutgoingDamagePerTurn;
			case StatDisplaySettings.StatType.DamageEfficiency:
				return DamageEfficiency;
			case StatDisplaySettings.StatType.KillParticipation:
				return KillParticipation;
			case StatDisplaySettings.StatType.EffectiveHealAndAbsorb:
				return SupportPerTurn;
			case StatDisplaySettings.StatType.TeamDamageAdjustedByMe:
				return TeamDamageSwingByMePerTurn;
			case StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe:
				return TeamBoostedEnergyByMePerTurn;
			case StatDisplaySettings.StatType.DamageTakenPerLife:
				return DamageTakenPerLife;
			case StatDisplaySettings.StatType.EnemiesSightedPerLife:
				return EnemiesSightedPerTurn;
			case StatDisplaySettings.StatType.TankingPerLife:
				return TankingPerLife;
			case StatDisplaySettings.StatType.DamageDonePerLife:
				return DamageDonePerLife;
			case StatDisplaySettings.StatType.TeamMitigation:
				return TeamMitigation;
			case StatDisplaySettings.StatType.TotalTurns:
				return TotalTurns;
			case StatDisplaySettings.StatType.TotalTeamDamageReceived:
				return TeamDamageReceived;
			case StatDisplaySettings.StatType.SupportPerTurn:
				return SupportPerTurn;
			case StatDisplaySettings.StatType.DamageTakenPerTurn:
				return DamageTakenPerTurn;
			case StatDisplaySettings.StatType.SecondsPlayed:
				return SecondsPlayed;
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
				FreelancerSpecificStats[i].CombineStats(StatsToBeMerged.FreelancerSpecificStats[i]);
			}
		}
	}

	public PersistedStatEntry GetFreelancerStat(int index)
	{
		return FreelancerSpecificStats != null
		       && -1 < index
		       && index < FreelancerSpecificStats.Count
			? FreelancerSpecificStats[index]
			: new PersistedStatEntry();
	}

	public object Clone()
	{
		PersistedStats persistedStats = new PersistedStats
		{
			TotalDeaths = TotalDeaths.GetCopy(),
			TotalPlayerKills = TotalPlayerKills.GetCopy(),
			TotalPlayerAssists = TotalPlayerAssists.GetCopy(),
			TotalPlayerDamage = TotalPlayerDamage.GetCopy(),
			TotalPlayerHealing = TotalPlayerHealing.GetCopy(),
			TotalPlayerAbsorb = TotalPlayerAbsorb.GetCopy(),
			TotalPlayerDamageReceived = TotalPlayerDamageReceived.GetCopy(),
			TotalBadgePoints = TotalBadgePoints.GetCopy(),
			NetDamageAvoidedByEvades = NetDamageAvoidedByEvades.GetCopy(),
			NetDamageAvoidedByEvadesPerLife = NetDamageAvoidedByEvadesPerLife.GetCopy(),
			DamageDodgedByEvades = DamageDodgedByEvades.GetCopy(),
			DamageInterceptedByEvades = DamageInterceptedByEvades.GetCopy(),
			MyIncomingDamageReducedByCover = MyIncomingDamageReducedByCover.GetCopy(),
			MyIncomingDamageReducedByCoverPerLife = MyIncomingDamageReducedByCoverPerLife.GetCopy(),
			MyOutgoingDamageReducedByCover = MyOutgoingDamageReducedByCover.GetCopy(),
			MyOutgoingDamageReducedFromWeakened = MyOutgoingDamageReducedFromWeakened.GetCopy(),
			MyOutgoingExtraDamageFromEmpowered = MyOutgoingExtraDamageFromEmpowered.GetCopy(),
			TeamIncomingDamageReducedByWeakenedFromMe = TeamIncomingDamageReducedByWeakenedFromMe.GetCopy(),
			TeamOutgoingDamageIncreasedByEmpoweredFromMe = TeamOutgoingDamageIncreasedByEmpoweredFromMe.GetCopy(),
			MovementDeniedByMePerTurn = MovementDeniedByMePerTurn.GetCopy(),
			EnergyGainPerTurn = EnergyGainPerTurn.GetCopy(),
			DamagePerTurn = DamagePerTurn.GetCopy(),
			BoostedOutgoingDamagePerTurn = BoostedOutgoingDamagePerTurn.GetCopy(),
			DamageEfficiency = DamageEfficiency.GetCopy(),
			KillParticipation = KillParticipation.GetCopy(),
			EffectiveHealing = EffectiveHealing.GetCopy(),
			TeamDamageAdjustedByMe = TeamDamageAdjustedByMe.GetCopy(),
			TeamDamageSwingByMePerTurn = TeamDamageSwingByMePerTurn.GetCopy(),
			TeamExtraEnergyByEnergizedFromMe = TeamExtraEnergyByEnergizedFromMe.GetCopy(),
			TeamBoostedEnergyByMePerTurn = TeamBoostedEnergyByMePerTurn.GetCopy(),
			TeamDamageReceived = TeamDamageReceived.GetCopy(),
			DamageTakenPerLife = DamageTakenPerLife.GetCopy(),
			EnemiesSightedPerTurn = EnemiesSightedPerTurn.GetCopy(),
			TotalTurns = TotalTurns.GetCopy(),
			TankingPerLife = TankingPerLife.GetCopy(),
			TeamMitigation = TeamMitigation.GetCopy(),
			SupportPerTurn = SupportPerTurn.GetCopy(),
			DamageDonePerLife = DamageDonePerLife.GetCopy(),
			DamageTakenPerTurn = DamageTakenPerTurn.GetCopy(),
			SecondsPlayed = SecondsPlayed.GetCopy(),
			MatchesWon = MatchesWon.GetCopy()
		};
		if (FreelancerSpecificStats == null)
		{
			persistedStats.FreelancerSpecificStats = null;
		}
		else
		{
			persistedStats.FreelancerSpecificStats = new List<PersistedStatEntry>();
			for (int i = 0; i < FreelancerSpecificStats.Count; i++)
			{
				persistedStats.FreelancerSpecificStats.Add((PersistedStatEntry)FreelancerSpecificStats[i].Clone());
			}
		}
		return persistedStats;
	}
}
