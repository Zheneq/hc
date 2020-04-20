using System;

[Serializable]
public class MatchFreelancerStats : ICloneable, StatDisplaySettings.IPersistatedStatValueSupplier
{
	public PersistedStatBucket PersistedStatBucket { get; set; }

	public CharacterType CharacterType { get; set; }

	public int ActiveSeason { get; set; }

	public float TotalAssists { get; set; }

	public float TotalDeaths { get; set; }

	public float? TotalBadgePoints { get; set; }

	public float EnergyGainPerTurn { get; set; }

	public float? DamagePerTurn { get; set; }

	public float NetBoostedOutgoingDamage { get; set; }

	public float DamageEfficiency { get; set; }

	public float KillParticipation { get; set; }

	public float TeamDamageAdjustedByMe { get; set; }

	public float TeamExtraEnergyByEnergizedFromMe { get; set; }

	public float MovementDenied { get; set; }

	public float? DamageTakenPerLife { get; set; }

	public float IncomingDamageDodgeByEvade { get; set; }

	public float IncomingDamageReducedByCover { get; set; }

	public float EnemiesSightedPerTurn { get; set; }

	public float TotalTurns { get; set; }

	public float TotalTeamDamageReceived { get; set; }

	public float? TankingPerLife { get; set; }

	public float? TeamMitigation { get; set; }

	public float? SupportPerTurn { get; set; }

	public float? DamageDonePerLife { get; set; }

	public float? DamageTakenPerTurn { get; set; }

	public float? MMR { get; set; }

	public int? Freelancer0 { get; set; }

	public int? Freelancer1 { get; set; }

	public int? Freelancer2 { get; set; }

	public int? Freelancer3 { get; set; }

	public float? GetStat(StatDisplaySettings.StatType Type)
	{
		switch (Type)
		{
		case StatDisplaySettings.StatType.TotalAssists:
			return new float?(this.TotalAssists);
		case StatDisplaySettings.StatType.TotalDeaths:
			return new float?(this.TotalDeaths);
		case StatDisplaySettings.StatType.TotalBadgePoints:
			return this.TotalBadgePoints;
		case StatDisplaySettings.StatType.EnergyGainPerTurn:
			return new float?(this.EnergyGainPerTurn);
		case StatDisplaySettings.StatType.DamagePerTurn:
			return this.DamagePerTurn;
		case StatDisplaySettings.StatType.NetBoostedOutgoingDamage:
			if (this.TotalTurns > 0f)
			{
				return new float?(this.NetBoostedOutgoingDamage / this.TotalTurns);
			}
			return null;
		case StatDisplaySettings.StatType.DamageEfficiency:
			return new float?(this.DamageEfficiency);
		case StatDisplaySettings.StatType.KillParticipation:
			return new float?(this.KillParticipation);
		case StatDisplaySettings.StatType.EffectiveHealAndAbsorb:
			return this.SupportPerTurn;
		case StatDisplaySettings.StatType.TeamDamageAdjustedByMe:
			if (this.TotalTurns > 0f)
			{
				return new float?(this.TeamDamageAdjustedByMe / this.TotalTurns);
			}
			return null;
		case StatDisplaySettings.StatType.TeamExtraEnergyByEnergizedFromMe:
			if (this.TotalTurns > 0f)
			{
				return new float?(this.TeamExtraEnergyByEnergizedFromMe / this.TotalTurns);
			}
			return null;
		case StatDisplaySettings.StatType.MovementDenied:
			if (this.TotalTurns > 0f)
			{
				return new float?(this.MovementDenied / this.TotalTurns);
			}
			return null;
		case StatDisplaySettings.StatType.DamageTakenPerLife:
			return this.DamageTakenPerLife;
		case StatDisplaySettings.StatType.IncomingDamageDodgeByEvade:
			return new float?(this.IncomingDamageDodgeByEvade / (this.TotalDeaths + 1f));
		case StatDisplaySettings.StatType.IncomingDamageReducedByCover:
			return new float?(this.IncomingDamageReducedByCover / (this.TotalDeaths + 1f));
		case StatDisplaySettings.StatType.EnemiesSightedPerLife:
			return new float?(this.EnemiesSightedPerTurn);
		case StatDisplaySettings.StatType.TotalTurns:
			return new float?(this.TotalTurns);
		case StatDisplaySettings.StatType.TotalTeamDamageReceived:
			return new float?(this.TotalTeamDamageReceived);
		case StatDisplaySettings.StatType.TankingPerLife:
			return this.TankingPerLife;
		case StatDisplaySettings.StatType.TeamMitigation:
			return this.TeamMitigation;
		case StatDisplaySettings.StatType.SupportPerTurn:
			return this.SupportPerTurn;
		case StatDisplaySettings.StatType.DamageDonePerLife:
			return this.DamageDonePerLife;
		case StatDisplaySettings.StatType.DamageTakenPerTurn:
			return this.DamageTakenPerTurn;
		case StatDisplaySettings.StatType.MMR:
			if (this.MMR == null)
			{
				return null;
			}
			if (this.MMR.Value > 0f)
			{
				if (this.MMR.Value != 1500f)
				{
					return this.MMR;
				}
			}
			return null;
		case StatDisplaySettings.StatType.AvgLifeSpan:
			return new float?(this.TotalTurns / (this.TotalDeaths + 1f));
		default:
			return null;
		}
	}

	public float? GetFreelancerStat(int FreelancerStatIndex)
	{
		if (FreelancerStatIndex == 0)
		{
			int? freelancer = this.Freelancer0;
			float? result;
			if (freelancer != null)
			{
				result = new float?((float)freelancer.Value);
			}
			else
			{
				result = null;
			}
			return result;
		}
		if (FreelancerStatIndex == 1)
		{
			int? freelancer2 = this.Freelancer1;
			return (freelancer2 == null) ? null : new float?((float)freelancer2.Value);
		}
		if (FreelancerStatIndex == 2)
		{
			int? freelancer3 = this.Freelancer2;
			float? result2;
			if (freelancer3 != null)
			{
				result2 = new float?((float)freelancer3.Value);
			}
			else
			{
				result2 = null;
			}
			return result2;
		}
		if (FreelancerStatIndex == 3)
		{
			int? freelancer4 = this.Freelancer3;
			return (freelancer4 == null) ? null : new float?((float)freelancer4.Value);
		}
		Log.Error("Unknown freelancer stat index: {0}", new object[]
		{
			FreelancerStatIndex
		});
		return null;
	}

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
