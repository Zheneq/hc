using System;

[Serializable]
public class MatchFreelancerStats : ICloneable, StatDisplaySettings.IPersistatedStatValueSupplier
{
	public PersistedStatBucket PersistedStatBucket
	{
		get;
		set;
	}

	public CharacterType CharacterType
	{
		get;
		set;
	}

	public int ActiveSeason
	{
		get;
		set;
	}

	public float TotalAssists
	{
		get;
		set;
	}

	public float TotalDeaths
	{
		get;
		set;
	}

	public float? TotalBadgePoints
	{
		get;
		set;
	}

	public float EnergyGainPerTurn
	{
		get;
		set;
	}

	public float? DamagePerTurn
	{
		get;
		set;
	}

	public float NetBoostedOutgoingDamage
	{
		get;
		set;
	}

	public float DamageEfficiency
	{
		get;
		set;
	}

	public float KillParticipation
	{
		get;
		set;
	}

	public float TeamDamageAdjustedByMe
	{
		get;
		set;
	}

	public float TeamExtraEnergyByEnergizedFromMe
	{
		get;
		set;
	}

	public float MovementDenied
	{
		get;
		set;
	}

	public float? DamageTakenPerLife
	{
		get;
		set;
	}

	public float IncomingDamageDodgeByEvade
	{
		get;
		set;
	}

	public float IncomingDamageReducedByCover
	{
		get;
		set;
	}

	public float EnemiesSightedPerTurn
	{
		get;
		set;
	}

	public float TotalTurns
	{
		get;
		set;
	}

	public float TotalTeamDamageReceived
	{
		get;
		set;
	}

	public float? TankingPerLife
	{
		get;
		set;
	}

	public float? TeamMitigation
	{
		get;
		set;
	}

	public float? SupportPerTurn
	{
		get;
		set;
	}

	public float? DamageDonePerLife
	{
		get;
		set;
	}

	public float? DamageTakenPerTurn
	{
		get;
		set;
	}

	public float? MMR
	{
		get;
		set;
	}

	public int? Freelancer0
	{
		get;
		set;
	}

	public int? Freelancer1
	{
		get;
		set;
	}

	public int? Freelancer2
	{
		get;
		set;
	}

	public int? Freelancer3
	{
		get;
		set;
	}

	public float? GetStat(StatDisplaySettings.StatType Type)
	{
		switch (Type)
		{
		case StatDisplaySettings.StatType.AvgLifeSpan:
			return TotalTurns / (TotalDeaths + 1f);
		case StatDisplaySettings.StatType.DamageDonePerLife:
			return DamageDonePerLife;
		case StatDisplaySettings.StatType.DamageEfficiency:
			return DamageEfficiency;
		case StatDisplaySettings.StatType.DamagePerTurn:
			return DamagePerTurn;
		case StatDisplaySettings.StatType.DamageTakenPerLife:
			return DamageTakenPerLife;
		case StatDisplaySettings.StatType.DamageTakenPerTurn:
			return DamageTakenPerTurn;
		case StatDisplaySettings.StatType.EffectiveHealAndAbsorb:
			return SupportPerTurn;
		case StatDisplaySettings.StatType.EnemiesSightedPerLife:
			return EnemiesSightedPerTurn;
		case StatDisplaySettings.StatType.EnergyGainPerTurn:
			return EnergyGainPerTurn;
		case StatDisplaySettings.StatType.IncomingDamageDodgeByEvade:
			return IncomingDamageDodgeByEvade / (TotalDeaths + 1f);
		case StatDisplaySettings.StatType.IncomingDamageReducedByCover:
			return IncomingDamageReducedByCover / (TotalDeaths + 1f);
		case StatDisplaySettings.StatType.KillParticipation:
			return KillParticipation;
		case StatDisplaySettings.StatType.MMR:
			if (!MMR.HasValue)
			{
				return null;
			}
			if (!(MMR.Value <= 0f))
			{
				if (MMR.Value != 1500f)
				{
					return MMR;
				}
			}
			return null;
		case StatDisplaySettings.StatType.MovementDenied:
			if (TotalTurns > 0f)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return MovementDenied / TotalTurns;
					}
				}
			}
			return null;
		case StatDisplaySettings.StatType.NetBoostedOutgoingDamage:
			if (TotalTurns > 0f)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return NetBoostedOutgoingDamage / TotalTurns;
					}
				}
			}
			return null;
		case StatDisplaySettings.StatType.SupportPerTurn:
			return SupportPerTurn;
		case StatDisplaySettings.StatType.TankingPerLife:
			return TankingPerLife;
		case StatDisplaySettings.StatType.TeamDamageAdjustedByMe:
			if (TotalTurns > 0f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return TeamDamageAdjustedByMe / TotalTurns;
					}
				}
			}
			return null;
		case StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe:
			if (TotalTurns > 0f)
			{
				return TeamExtraEnergyByEnergizedFromMe / TotalTurns;
			}
			return null;
		case StatDisplaySettings.StatType.TeamMitigation:
			return TeamMitigation;
		case StatDisplaySettings.StatType.TotalAssists:
			return TotalAssists;
		case StatDisplaySettings.StatType.TotalBadgePoints:
			return TotalBadgePoints;
		case StatDisplaySettings.StatType.TotalDeaths:
			return TotalDeaths;
		case StatDisplaySettings.StatType.TotalTeamDamageReceived:
			return TotalTeamDamageReceived;
		case StatDisplaySettings.StatType.TotalTurns:
			return TotalTurns;
		default:
			return null;
		}
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		switch (FreelancerStatIndex)
		{
		case 0:
		{
			int? freelancer3 = Freelancer0;
			float? result;
			if (freelancer3.HasValue)
			{
				result = freelancer3.Value;
			}
			else
			{
				result = null;
			}
			return result;
		}
		case 1:
		{
			int? freelancer2 = Freelancer1;
			return (!freelancer2.HasValue) ? null : new float?(freelancer2.Value);
		}
		case 2:
			while (true)
			{
				int? freelancer4 = Freelancer2;
				float? result2;
				if (freelancer4.HasValue)
				{
					result2 = freelancer4.Value;
				}
				else
				{
					result2 = null;
				}
				return result2;
			}
		case 3:
		{
			int? freelancer = Freelancer3;
			return (!freelancer.HasValue) ? null : new float?(freelancer.Value);
		}
		default:
			Log.Error("Unknown freelancer stat index: {0}", FreelancerStatIndex);
			return null;
		}
	}

	public object Clone()
	{
		return MemberwiseClone();
	}
}
