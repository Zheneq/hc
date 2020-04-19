using System;
using System.Collections.Generic;

[Serializable]
public class QuestRewards
{
	public List<QuestCurrencyReward> CurrencyRewards;

	public List<QuestUnlockReward> UnlockRewards;

	public List<QuestItemReward> ItemRewards;
}
