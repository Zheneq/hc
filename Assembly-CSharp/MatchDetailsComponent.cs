using System;

[Serializable]
public class MatchDetailsComponent : ICloneable
{
	public int Deaths { get; set; }

	public int Takedowns { get; set; }

	public int DamageDealt { get; set; }

	public int DamageTaken { get; set; }

	public int Healing { get; set; }

	public int Contribution { get; set; }

	public MatchResultsStats MatchResults { get; set; }

	public int GroupSize { get; set; }

	public string RankedTierLocTag { get; set; }

	public float RankedPoints { get; set; }

	public object Clone()
	{
		return base.MemberwiseClone();
	}
}
