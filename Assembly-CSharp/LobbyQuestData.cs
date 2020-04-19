using System;
using System.Collections.Generic;

[Serializable]
public class LobbyQuestData
{
	public int m_generalSlotCount;

	public int m_questResetHour;

	public DayOfWeek m_questResetDayOfWeek;

	public int m_questBonusPerRejection;

	public int m_questMaxRejectPercentage;

	public int m_notIncludedWeightIncrease;

	public int m_rejectedWeightDecrease;

	public int m_completedWeightDecrease;

	public int m_abandonedWeightDecrease;

	public List<QuestTemplate> m_quests;

	public List<QuestStyleGroup> m_styleGroups;

	public List<QuestPool> m_dailyQuestPools;
}
