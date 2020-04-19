using System;

[Serializable]
public class LocalizationArg_Faction : LocalizationArg
{
	public int m_factionCompetition;

	public int m_factionId;

	public static LocalizationArg_Faction Create(int factionCompetition, int factionId)
	{
		return new LocalizationArg_Faction
		{
			m_factionCompetition = factionCompetition,
			m_factionId = factionId
		};
	}

	public override string TR()
	{
		return Faction.GetDisplayName(this.m_factionCompetition, this.m_factionId);
	}
}
