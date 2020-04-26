using System;

[Serializable]
public class FactionTierConfigOverride
{
	public int CompetitionId;

	public int FactionId;

	public int TierId;

	public long ContributionToComplete;

	public FactionTierConfigOverride Clone()
	{
		return (FactionTierConfigOverride)MemberwiseClone();
	}
}
