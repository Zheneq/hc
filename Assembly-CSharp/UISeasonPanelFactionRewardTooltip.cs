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
		UIFactionRewardTooltipListItem[] componentsInChildren = m_factionRewardLayoutGroup.GetComponentsInChildren<UIFactionRewardTooltipListItem>(true);
		m_tooltipItems.AddRange(componentsInChildren);
		UIFactionPersonalReward[] componentsInChildren2 = m_personalRewardLayoutGroup.GetComponentsInChildren<UIFactionPersonalReward>(true);
		m_personalTooltipItems.AddRange(componentsInChildren2);
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
						FactionReward current = enumerator.Current;
						if (num >= m_personalTooltipItems.Count)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							UIFactionPersonalReward uIFactionPersonalReward = Object.Instantiate(m_personalRewardPrefab);
							uIFactionPersonalReward.transform.SetParent(m_personalRewardLayoutGroup.gameObject.transform);
							uIFactionPersonalReward.transform.localPosition = Vector3.zero;
							uIFactionPersonalReward.transform.localScale = Vector3.one;
							uIFactionPersonalReward.transform.localEulerAngles = Vector3.zero;
							m_personalTooltipItems.Add(uIFactionPersonalReward);
						}
						UIManager.SetGameObjectActive(m_personalTooltipItems[num], true);
						m_personalTooltipItems[num].Setup(current, i, currentPersonalLevel - 1 > i);
						num++;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		float num2 = (m_personalRewardLayoutGroup as VerticalLayoutGroup).spacing + m_personalRewardPrefab.GetComponent<LayoutElement>().preferredHeight;
		RectTransform personalRewardTooltipHeightObject = m_personalRewardTooltipHeightObject;
		Vector2 sizeDelta = m_personalRewardTooltipHeightObject.sizeDelta;
		personalRewardTooltipHeightObject.sizeDelta = new Vector2(sizeDelta.x, m_personalTooltipHeightPadding + (float)num * num2);
		for (int j = num; j < m_personalTooltipItems.Count; j++)
		{
			UIManager.SetGameObjectActive(m_personalTooltipItems[j], false);
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public bool SetupFactionReward(Faction factionInfo, int currentLevel, bool isFactionComplete)
	{
		if (factionInfo != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (factionInfo.RewardLootTableID != 0)
			{
				if (currentLevel < 1)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
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
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									if (conditions[k].typeSpecificData3 == i + 1)
									{
										list.Add(lootTableEntry.Index);
									}
								}
							}
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						else if (i == 0)
						{
							list.Add(lootTableEntry.Index);
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					while (m_tooltipItems.Count <= num)
					{
						UIFactionRewardTooltipListItem uIFactionRewardTooltipListItem = Object.Instantiate(m_factionRewardPrefab);
						uIFactionRewardTooltipListItem.transform.SetParent(m_factionRewardLayoutGroup.gameObject.transform);
						uIFactionRewardTooltipListItem.transform.localPosition = Vector3.zero;
						uIFactionRewardTooltipListItem.transform.localScale = Vector3.one;
						uIFactionRewardTooltipListItem.transform.localEulerAngles = Vector3.zero;
						m_tooltipItems.Add(uIFactionRewardTooltipListItem);
					}
					UIManager.SetGameObjectActive(m_tooltipItems[num], true);
					m_tooltipItems[num].Setup(list, i, i < currentLevel);
					num++;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					RectTransform rewardTooltipHeightObject = m_rewardTooltipHeightObject;
					Vector2 sizeDelta = m_rewardTooltipHeightObject.sizeDelta;
					rewardTooltipHeightObject.sizeDelta = new Vector2(sizeDelta.x, m_tooltipHeightPadding + (float)num * m_factionRewardPrefab.GetComponent<LayoutElement>().preferredHeight);
					for (int l = num; l < m_tooltipItems.Count; l++)
					{
						UIManager.SetGameObjectActive(m_tooltipItems[l], false);
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						return true;
					}
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public void SetCommunityFactionVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_CommunityFactionTooltipContainer, visible);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_PersonalTooltipContainer, false);
			return;
		}
	}

	public void SetPersonalRewardVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_PersonalTooltipContainer, visible);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_CommunityFactionTooltipContainer, false);
			return;
		}
	}
}
