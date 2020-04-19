using System;
using System.Collections.Generic;

[Serializable]
public class SeasonChapter
{
	public string DisplayName;

	public bool Hidden;

	public string UnlockChapterString;

	public QuestPrerequisites Prerequisites;

	public List<int> NormalQuests;

	public int NumQuestsToAdvance;

	public List<QuestCurrencyReward> CurrencyRewards;

	public List<QuestUnlockReward> UnlockRewards;

	public List<QuestItemReward> ItemRewards;

	public List<SeasonStorytime> StorytimePanels;
}
