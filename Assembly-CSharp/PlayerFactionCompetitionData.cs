using System;
using System.Collections.Generic;

[Serializable]
public class PlayerFactionCompetitionData
{
	public int CompetitionID;

	public Dictionary<int, FactionPlayerData> Factions;
}
