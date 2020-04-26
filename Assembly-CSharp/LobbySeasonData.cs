using System;
using System.Collections.Generic;

[Serializable]
public class LobbySeasonData
{
	public List<SeasonTemplate> m_seasons;

	public List<AlertMission> m_alerts;

	public List<QuestPool> m_seasonQuestPools;
}
