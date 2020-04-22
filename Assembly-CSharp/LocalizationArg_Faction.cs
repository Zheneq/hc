using System;

[Serializable]
public class LocalizationArg_Faction : LocalizationArg
{
	public int m_factionCompetition;

	public int m_factionId;

	public static LocalizationArg_Faction Create(int factionCompetition, int factionId)
	{
		LocalizationArg_Faction localizationArg_Faction = new LocalizationArg_Faction();
		localizationArg_Faction.m_factionCompetition = factionCompetition;
		localizationArg_Faction.m_factionId = factionId;
		return localizationArg_Faction;
	}

	public override string TR()
	{
		return Faction.GetDisplayName(m_factionCompetition, m_factionId);
	}
}
