using LobbyGameClientMessages;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerProgressAchievements : UIPlayerProgressSubPanel
{
	public UIPlayerProgressDropdownBtn m_categoryDropdownBtn;

	public UIPlayerProgressDropdownBtn m_freelancerDropdownBtn;

	public RectTransform m_categoryDropdownSlot;

	public RectTransform m_freelancerDropdownSlot;

	public ScrollRect m_scrollRect;

	public TextMeshProUGUI m_points;

	public UIPlayerProgressAchievementItem m_itemPrefab;

	public VerticalLayoutGroup[] m_verticalColumns;

	private bool m_initialized;

	private List<UIPlayerProgressAchievementItem> m_achievementItems;

	private CharacterType m_characterType;

	private AchievementType m_achievementType;

	private void Init()
	{
		if (m_initialized)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_initialized = true;
		m_achievementItems = new List<UIPlayerProgressAchievementItem>();
		VerticalLayoutGroup[] verticalColumns = m_verticalColumns;
		foreach (VerticalLayoutGroup verticalLayoutGroup in verticalColumns)
		{
			UIPlayerProgressAchievementItem[] componentsInChildren = verticalLayoutGroup.GetComponentsInChildren<UIPlayerProgressAchievementItem>(true);
			foreach (UIPlayerProgressAchievementItem uIPlayerProgressAchievementItem in componentsInChildren)
			{
				Object.Destroy(uIPlayerProgressAchievementItem.gameObject);
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					goto end_IL_006c;
				}
				continue;
				end_IL_006c:
				break;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Start()
	{
		UIEventTriggerUtils.AddListener(m_scrollRect.verticalScrollbar.gameObject, EventTriggerType.Scroll, OnScroll);
		m_categoryDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenAchievementDropdown(m_achievementType, delegate(int achievementInt)
			{
				m_achievementType = (AchievementType)achievementInt;
				Setup();
			}, m_categoryDropdownSlot);
		};
		m_freelancerDropdownBtn.m_button.spriteController.callback = delegate
		{
			UIPlayerProgressPanel.Get().OpenFreelancerDropdown(m_characterType, delegate(int charTypeInt)
			{
				m_characterType = (CharacterType)charTypeInt;
				Setup();
			}, m_freelancerDropdownSlot, false);
		};
		ClientGameManager.Get().OnQuestCompleteNotification += OnQuestCompleteNotification;
		ClientGameManager.Get().OnQuestProgressChanged += OnQuestProgressChanged;
		ClientGameManager.Get().OnFriendStatusNotification += OnFriendStatusNotification;
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnQuestCompleteNotification -= OnQuestCompleteNotification;
			ClientGameManager.Get().OnQuestProgressChanged -= OnQuestProgressChanged;
			ClientGameManager.Get().OnFriendStatusNotification -= OnFriendStatusNotification;
			return;
		}
	}

	private void OnDisable()
	{
		UIPlayerProgressPanel.Get().HideDropdowns();
	}

	private void OnQuestCompleteNotification(QuestCompleteNotification notification)
	{
		Setup();
	}

	private void OnQuestProgressChanged(QuestProgress[] progresses)
	{
		Setup();
	}

	private void OnFriendStatusNotification(FriendStatusNotification notification)
	{
		Setup();
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}

	public void Setup()
	{
		Init();
		m_categoryDropdownBtn.Setup(StringUtil.TR("AchievementCategory_" + m_achievementType, "Global"));
		UIManager.SetGameObjectActive(m_freelancerDropdownBtn, m_achievementType == AchievementType.Freelancer);
		if (m_achievementType == AchievementType.Freelancer)
		{
			if (m_characterType != 0)
			{
				m_freelancerDropdownBtn.Setup(GameWideData.Get().GetCharacterDisplayName(m_characterType), m_characterType);
			}
			else
			{
				m_freelancerDropdownBtn.Setup(StringUtil.TR("AllFreelancers", "Global"), m_characterType);
			}
		}
		int num = 0;
		int i = 0;
		List<QuestTemplate> quests = QuestWideData.Get().m_quests;
		Dictionary<int, QuestTemplate> dictionary = new Dictionary<int, QuestTemplate>();
		for (int j = 0; j < quests.Count; j++)
		{
			QuestTemplate questTemplate = quests[j];
			if (questTemplate.AchievmentType == AchievementType.None)
			{
				continue;
			}
			if (!questTemplate.Enabled)
			{
				continue;
			}
			if (m_achievementType != 0)
			{
				if (questTemplate.AchievmentType != m_achievementType)
				{
					continue;
				}
			}
			if (m_achievementType == AchievementType.Freelancer)
			{
				if (m_characterType != 0)
				{
					if (!QuestWideData.IsCharacterQuest(questTemplate.Objectives, GameWideData.Get().GetCharacterResourceLink(m_characterType)))
					{
						continue;
					}
				}
			}
			dictionary[questTemplate.Index] = questTemplate;
		}
		quests = new List<QuestTemplate>();
		quests.AddRange(dictionary.Values);
		for (int k = 0; k < quests.Count; k++)
		{
			if (quests[k].AchievementPrevious > 0)
			{
				dictionary.Remove(quests[k].AchievementPrevious);
			}
		}
		bool flag = false;
		Dictionary<int, QuestMetaData> questMetaDatas = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.QuestMetaDatas;
		quests.Clear();
		quests.AddRange(dictionary.Values);
		int questCount = QuestWideData.Get().m_quests.Count;
		quests.Sort((QuestTemplate x, QuestTemplate y) => x.SortOrder * questCount + x.Index - (y.SortOrder * questCount + y.Index));
		for (int l = 0; l < quests.Count; l++)
		{
			QuestTemplate quest = quests[l];
			UIPlayerProgressAchievementItem uIPlayerProgressAchievementItem;
			if (i >= m_achievementItems.Count)
			{
				uIPlayerProgressAchievementItem = Object.Instantiate(m_itemPrefab);
				int num2 = m_achievementItems.Count % m_verticalColumns.Length;
				uIPlayerProgressAchievementItem.transform.SetParent(m_verticalColumns[num2].transform);
				uIPlayerProgressAchievementItem.transform.localPosition = Vector3.zero;
				uIPlayerProgressAchievementItem.transform.localScale = Vector3.one;
				m_achievementItems.Add(uIPlayerProgressAchievementItem);
				uIPlayerProgressAchievementItem.m_selectableBtn.spriteController.RegisterScrollListener(OnScroll);
				flag = true;
			}
			else
			{
				uIPlayerProgressAchievementItem = m_achievementItems[i];
			}
			UIManager.SetGameObjectActive(uIPlayerProgressAchievementItem, true);
			i++;
			num += uIPlayerProgressAchievementItem.Setup(quest, questMetaDatas);
		}
		for (; i < m_achievementItems.Count; i++)
		{
			UIManager.SetGameObjectActive(m_achievementItems[i], false);
			flag = true;
		}
		m_points.text = string.Format(StringUtil.TR("AchievementPoints", "Global"), num);
		if (!flag)
		{
			return;
		}
		while (true)
		{
			m_scrollRect.verticalNormalizedPosition = 1f;
			return;
		}
	}
}
