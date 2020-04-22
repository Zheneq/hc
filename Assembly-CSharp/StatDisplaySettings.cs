using System.Linq;

public static class StatDisplaySettings
{
	public interface IPersistatedStatValueSupplier
	{
		float? GetStat(StatType Type);

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

	public static StatType[] GeneralStats = new StatType[4]
	{
		StatType.TotalAssists,
		StatType.TotalDeaths,
		StatType.TotalBadgePoints,
		StatType.EnergyGainPerTurn
	};

	public static StatType[] FirepowerStats = new StatType[4]
	{
		StatType.DamagePerTurn,
		StatType.NetBoostedOutgoingDamage,
		StatType.DamageEfficiency,
		StatType.KillParticipation
	};

	public static StatType[] SupportStats = new StatType[4]
	{
		StatType.EffectiveHealAndAbsorb,
		StatType.TeamDamageAdjustedByMe,
		StatType.TeamExtraEnergyByEnergizedFromMe,
		StatType.IncomingDamageReducedByCover
	};

	public static StatType[] FrontlinerStats = new StatType[4]
	{
		StatType.DamageTakenPerLife,
		StatType.IncomingDamageDodgeByEvade,
		StatType.EnemiesSightedPerLife,
		StatType.MovementDenied
	};

	public static string GetLocalizedName(StatType TypeOfStat)
	{
		return StringUtil.TR_StatName(TypeOfStat);
	}

	public static string GetLocalizedDescription(StatType TypeOfStat)
	{
		return StringUtil.TR_StatDescription(TypeOfStat);
	}

	public static bool IsStatADisplayedStat(StatType TypeOfStat)
	{
		int result;
		if (!GeneralStats.Contains(TypeOfStat))
		{
			if (!FirepowerStats.Contains(TypeOfStat))
			{
				if (!SupportStats.Contains(TypeOfStat))
				{
					result = (FrontlinerStats.Contains(TypeOfStat) ? 1 : 0);
					goto IL_0058;
				}
			}
		}
		result = 1;
		goto IL_0058;
		IL_0058:
		return (byte)result != 0;
	}
}
