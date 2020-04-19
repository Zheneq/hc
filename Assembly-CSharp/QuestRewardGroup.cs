using System;
using System.Collections.Generic;
using UnityEngine;

public class QuestRewardGroup : MonoBehaviour
{
	public QuestReward[] m_questRewards;

	public void SetupChildren(int questId, int rejectedCount, bool removedQuestCompleteNotification = false)
	{
		QuestTemplate questTemplate = QuestWideData.Get().m_quests[questId - 1];
		QuestRewards questRewards = new QuestRewards();
		questRewards.CurrencyRewards = new List<QuestCurrencyReward>();
		questRewards.ItemRewards = new List<QuestItemReward>();
		questRewards.UnlockRewards = new List<QuestUnlockReward>();
		if (questTemplate.Rewards.UnlockRewards.Count + questTemplate.Rewards.ItemRewards.Count + questTemplate.Rewards.CurrencyRewards.Count > 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestRewardGroup.SetupChildren(int, int, bool)).MethodHandle;
			}
			questRewards.CurrencyRewards.AddRange(questTemplate.Rewards.CurrencyRewards);
			questRewards.ItemRewards.AddRange(questTemplate.Rewards.ItemRewards);
			questRewards.UnlockRewards.AddRange(questTemplate.Rewards.UnlockRewards);
		}
		int i;
		if (questTemplate.ConditionalRewards != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			for (i = 0; i < questTemplate.ConditionalRewards.Length; i++)
			{
				if (QuestWideData.AreConditionsMet(questTemplate.ConditionalRewards[i].Prerequisites.Conditions, questTemplate.ConditionalRewards[i].Prerequisites.LogicStatement, false))
				{
					questRewards.CurrencyRewards.AddRange(questTemplate.ConditionalRewards[i].CurrencyRewards);
					questRewards.ItemRewards.AddRange(questTemplate.ConditionalRewards[i].ItemRewards);
					questRewards.UnlockRewards.AddRange(questTemplate.ConditionalRewards[i].UnlockRewards);
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		i = 0;
		foreach (QuestReward questReward in this.m_questRewards)
		{
			if (i < questRewards.CurrencyRewards.Count)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				questReward.Setup(questRewards.CurrencyRewards[i], rejectedCount);
			}
			else if (i - questRewards.CurrencyRewards.Count < questRewards.UnlockRewards.Count)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				questReward.SetupHack(questRewards.UnlockRewards[i - questRewards.CurrencyRewards.Count].resourceString, 0);
			}
			else if (i - questRewards.CurrencyRewards.Count - questRewards.UnlockRewards.Count < questRewards.ItemRewards.Count)
			{
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(questRewards.ItemRewards[i - questRewards.CurrencyRewards.Count - questRewards.UnlockRewards.Count].ItemTemplateId);
				questReward.SetupHack(itemTemplate, itemTemplate.IconPath, 0);
			}
			else
			{
				questReward.SetupHack(string.Empty, 0);
			}
			if (removedQuestCompleteNotification && questReward.m_ExpUPAnim != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(questReward.m_ExpUPAnim, false, null);
			}
			i++;
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
	}
}
