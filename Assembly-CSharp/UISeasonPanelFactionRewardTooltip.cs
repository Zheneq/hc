using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonPanelFactionRewardTooltip : UITooltipBase
{
	public RectTransform m_CommunityFactionTooltipContainer;

	public RectTransform m_PersonalTooltipContainer;

	public RectTransform m_rewardTooltipHeightObject;

	public RectTransform m_personalRewardTooltipHeightObject;

	public UIFactionRewardTooltipListItem m_factionRewardPrefab;

	public UIFactionPersonalReward m_personalRewardPrefab;

	public LayoutGroup m_factionRewardLayoutGroup;

	public LayoutGroup m_personalRewardLayoutGroup;

	public float m_tooltipHeightPadding = 20f;

	public float m_personalTooltipHeightPadding = 30f;

	private List<UIFactionRewardTooltipListItem> m_tooltipItems = new List<UIFactionRewardTooltipListItem>();

	private List<UIFactionPersonalReward> m_personalTooltipItems = new List<UIFactionPersonalReward>();

	private void Awake()
	{
		UIFactionRewardTooltipListItem[] componentsInChildren = this.m_factionRewardLayoutGroup.GetComponentsInChildren<UIFactionRewardTooltipListItem>(true);
		this.m_tooltipItems.AddRange(componentsInChildren);
		UIFactionPersonalReward[] componentsInChildren2 = this.m_personalRewardLayoutGroup.GetComponentsInChildren<UIFactionPersonalReward>(true);
		this.m_personalTooltipItems.AddRange(componentsInChildren2);
	}

	public void SetupPersonalReward(int currentPersonalLevel, Faction factionInfo)
	{
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		int num = 0;
		if (gameBalanceVars != null)
		{
			for (int i = 0; i < factionInfo.FactionPlayerProgressInfo.Length; i++)
			{
				List<FactionReward> allRewards = factionInfo.FactionPlayerProgressInfo[i].LevelUpRewards.GetAllRewards();
				using (List<FactionReward>.Enumerator enumerator = allRewards.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						FactionReward reward = enumerator.Current;
						if (num >= this.m_personalTooltipItems.Count)
						{
							UIFactionPersonalReward uifactionPersonalReward = UnityEngine.Object.Instantiate<UIFactionPersonalReward>(this.m_personalRewardPrefab);
							uifactionPersonalReward.transform.SetParent(this.m_personalRewardLayoutGroup.gameObject.transform);
							uifactionPersonalReward.transform.localPosition = Vector3.zero;
							uifactionPersonalReward.transform.localScale = Vector3.one;
							uifactionPersonalReward.transform.localEulerAngles = Vector3.zero;
							this.m_personalTooltipItems.Add(uifactionPersonalReward);
						}
						UIManager.SetGameObjectActive(this.m_personalTooltipItems[num], true, null);
						this.m_personalTooltipItems[num].Setup(reward, i, currentPersonalLevel - 1 > i);
						num++;
					}
				}
			}
		}
		float num2 = (this.m_personalRewardLayoutGroup as VerticalLayoutGroup).spacing + this.m_personalRewardPrefab.GetComponent<LayoutElement>().preferredHeight;
		this.m_personalRewardTooltipHeightObject.sizeDelta = new Vector2(this.m_personalRewardTooltipHeightObject.sizeDelta.x, this.m_personalTooltipHeightPadding + (float)num * num2);
		for (int j = num; j < this.m_personalTooltipItems.Count; j++)
		{
			UIManager.SetGameObjectActive(this.m_personalTooltipItems[j], false, null);
		}
	}

	public bool SetupFactionReward(Faction factionInfo, int currentLevel, bool isFactionComplete)
	{
		if (factionInfo != null)
		{
			if (factionInfo.RewardLootTableID != 0)
			{
				if (currentLevel < 1)
				{
					currentLevel = 1;
				}
				else if (isFactionComplete)
				{
					currentLevel++;
				}
				int num = 0;
				LootTable lootTable = InventoryWideData.Get().GetLootTable(factionInfo.RewardLootTableID);
				for (int i = 0; i < factionInfo.Tiers.Count; i++)
				{
					List<int> list = new List<int>();
					for (int j = 0; j < lootTable.Entries.Count; j++)
					{
						LootTableEntry lootTableEntry = lootTable.Entries[j];
						List<QuestCondition> conditions = lootTableEntry.Prerequisites.Conditions;
						if (conditions.Count > 0)
						{
							for (int k = 0; k < conditions.Count; k++)
							{
								if (conditions[k].ConditionType == QuestConditionType.FactionTierReached)
								{
									if (conditions[k].typeSpecificData3 == i + 1)
									{
										list.Add(lootTableEntry.Index);
									}
								}
							}
						}
						else if (i == 0)
						{
							list.Add(lootTableEntry.Index);
						}
					}
					while (this.m_tooltipItems.Count <= num)
					{
						UIFactionRewardTooltipListItem uifactionRewardTooltipListItem = UnityEngine.Object.Instantiate<UIFactionRewardTooltipListItem>(this.m_factionRewardPrefab);
						uifactionRewardTooltipListItem.transform.SetParent(this.m_factionRewardLayoutGroup.gameObject.transform);
						uifactionRewardTooltipListItem.transform.localPosition = Vector3.zero;
						uifactionRewardTooltipListItem.transform.localScale = Vector3.one;
						uifactionRewardTooltipListItem.transform.localEulerAngles = Vector3.zero;
						this.m_tooltipItems.Add(uifactionRewardTooltipListItem);
					}
					UIManager.SetGameObjectActive(this.m_tooltipItems[num], true, null);
					this.m_tooltipItems[num].Setup(list, i, i < currentLevel);
					num++;
				}
				this.m_rewardTooltipHeightObject.sizeDelta = new Vector2(this.m_rewardTooltipHeightObject.sizeDelta.x, this.m_tooltipHeightPadding + (float)num * this.m_factionRewardPrefab.GetComponent<LayoutElement>().preferredHeight);
				for (int l = num; l < this.m_tooltipItems.Count; l++)
				{
					UIManager.SetGameObjectActive(this.m_tooltipItems[l], false, null);
				}
				return true;
			}
		}
		return false;
	}

	public void SetCommunityFactionVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_CommunityFactionTooltipContainer, visible, null);
		if (visible)
		{
			UIManager.SetGameObjectActive(this.m_PersonalTooltipContainer, false, null);
		}
	}

	public void SetPersonalRewardVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_PersonalTooltipContainer, visible, null);
		if (visible)
		{
			UIManager.SetGameObjectActive(this.m_CommunityFactionTooltipContainer, false, null);
		}
	}
}
