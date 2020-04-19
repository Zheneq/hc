using System;
using System.Linq;

public static class StatDisplaySettings
{
	public static StatDisplaySettings.StatType[] GeneralStats = new StatDisplaySettings.StatType[]
	{
		StatDisplaySettings.StatType.TotalAssists,
		StatDisplaySettings.StatType.TotalDeaths,
		StatDisplaySettings.StatType.TotalBadgePoints,
		StatDisplaySettings.StatType.EnergyGainPerTurn
	};

	public static StatDisplaySettings.StatType[] FirepowerStats = new StatDisplaySettings.StatType[]
	{
		StatDisplaySettings.StatType.DamagePerTurn,
		StatDisplaySettings.StatType.NetBoostedOutgoingDamage,
		StatDisplaySettings.StatType.DamageEfficiency,
		StatDisplaySettings.StatType.KillParticipation
	};

	public static StatDisplaySettings.StatType[] SupportStats = new StatDisplaySettings.StatType[]
	{
		StatDisplaySettings.StatType.EffectiveHealAndAbsorb,
		StatDisplaySettings.StatType.TeamDamageAdjustedByMe,
		StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe,
		StatDisplaySettings.StatType.IncomingDamageReducedByCover
	};

	public static StatDisplaySettings.StatType[] FrontlinerStats = new StatDisplaySettings.StatType[]
	{
		StatDisplaySettings.StatType.DamageTakenPerLife,
		StatDisplaySettings.StatType.IncomingDamageDodgeByEvade,
		StatDisplaySettings.StatType.EnemiesSightedPerLife,
		StatDisplaySettings.StatType.MovementDenied
	};

	public static string GetLocalizedName(StatDisplaySettings.StatType TypeOfStat)
	{
		return StringUtil.TR_StatName(TypeOfStat);
	}

	public static string GetLocalizedDescription(StatDisplaySettings.StatType TypeOfStat)
	{
		return StringUtil.TR_StatDescription(TypeOfStat);
	}

	public static bool IsStatADisplayedStat(StatDisplaySettings.StatType TypeOfStat)
	{
		if (!StatDisplaySettings.GeneralStats.Contains(TypeOfStat))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(StatDisplaySettings.IsStatADisplayedStat(StatDisplaySettings.StatType)).MethodHandle;
			}
			if (!StatDisplaySettings.FirepowerStats.Contains(TypeOfStat))
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
				if (!StatDisplaySettings.SupportStats.Contains(TypeOfStat))
				{
					return StatDisplaySettings.FrontlinerStats.Contains(TypeOfStat);
				}
			}
		}
		return true;
	}

	public interface IPersistatedStatValueSupplier
	{
		float? GetStat(StatDisplaySettings.StatType Type);

		float? GetFreelancerStat(int FreelancerStatIndex);
	}

	public enum StatType
	{
		TotalAssists,
		TotalDeaths,
		TotalBadgePoints,
		EnergyGainPerTurn,
		DamagePerTurn,
		NetBoostedOutgoingDamage,
		DamageEfficiency,
		KillParticipation,
		EffectiveHealAndAbsorb,
		TeamDamageAdjustedByMe,
		TeamExtraEnergyByEnergizedFromMe,
		MovementDenied,
		DamageTakenPerLife,
		IncomingDamageDodgeByEvade,
		IncomingDamageReducedByCover,
		EnemiesSightedPerLife,
		TotalTurns,
		TotalTeamDamageReceived,
		TankingPerLife,
		TeamMitigation,
		SupportPerTurn,
		DamageDonePerLife,
		DamageTakenPerTurn,
		MMR,
		AvgLifeSpan,
		SecondsPlayed,
		MatchesWon
	}
}
