using System;
using System.Collections.Generic;

[Serializable]
public class FactionCompetitionConfigOverride
{
	public int Index;

	public string InternalName;

	public List<FactionTierConfigOverride> FactionTierConfigs;

	public FactionCompetitionConfigOverride Clone()
	{
		return (FactionCompetitionConfigOverride)base.MemberwiseClone();
	}
}
