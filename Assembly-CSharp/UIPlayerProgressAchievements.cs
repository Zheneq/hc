using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
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
		if (this.m_initialized)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressAchievements.Init()).MethodHandle;
			}
			return;
		}
		this.m_initialized = true;
		this.m_achievementItems = new List<UIPlayerProgressAchievementItem>();
		foreach (VerticalLayoutGroup verticalLayoutGroup in this.m_verticalColumns)
		{
			foreach (UIPlayerProgressAchievementItem uiplayerProgressAchievementItem in verticalLayoutGroup.GetComponentsInChildren<UIPlayerProgressAchievementItem>(true))
			{
				UnityEngine.Object.Destroy(uiplayerProgressAchievementItem.gameObject);
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

	private void Start()
	{
		UIEventTriggerUtils.AddListener(this.m_scrollRect.verticalScrollbar.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		this.m_categoryDropdownBtn.m_button.spriteController.callback = delegate(BaseEventData data)
		{
			UIPlayerProgressPanel.Get().OpenAchievementDropdown(this.m_achievementType, delegate(int achievementInt)
			{
				this.m_achievementType = (AchievementType)achievementInt;
				this.Setup();
			}, this.m_categoryDropdownSlot);
		};
		this.m_freelancerDropdownBtn.m_button.spriteController.callback = delegate(BaseEventData data)
		{
			UIPlayerProgressPanel.Get().OpenFreelancerDropdown(this.m_characterType, delegate(int charTypeInt)
			{
				this.m_characterType = (CharacterType)charTypeInt;
				this.Setup();
			}, this.m_freelancerDropdownSlot, false, CharacterRole.None);
		};
		ClientGameManager.Get().OnQuestCompleteNotification += this.OnQuestCompleteNotification;
		ClientGameManager.Get().OnQuestProgressChanged += this.OnQuestProgressChanged;
		ClientGameManager.Get().OnFriendStatusNotification += this.OnFriendStatusNotification;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressAchievements.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnQuestCompleteNotification -= this.OnQuestCompleteNotification;
			ClientGameManager.Get().OnQuestProgressChanged -= this.OnQuestProgressChanged;
			ClientGameManager.Get().OnFriendStatusNotification -= this.OnFriendStatusNotification;
		}
	}

	private void OnDisable()
	{
		UIPlayerProgressPanel.Get().HideDropdowns();
	}

	private void OnQuestCompleteNotification(QuestCompleteNotification notification)
	{
		this.Setup();
	}

	private void OnQuestProgressChanged(QuestProgress[] progresses)
	{
		this.Setup();
	}

	private void OnFriendStatusNotification(FriendStatusNotification notification)
	{
		this.Setup();
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	public void Setup()
	{
		this.Init();
		this.m_categoryDropdownBtn.Setup(StringUtil.TR("AchievementCategory_" + this.m_achievementType, "Global"), CharacterType.None);
		UIManager.SetGameObjectActive(this.m_freelancerDropdownBtn, this.m_achievementType == AchievementType.Freelancer, null);
		if (this.m_achievementType == AchievementType.Freelancer)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressAchievements.Setup()).MethodHandle;
			}
			if (this.m_characterType != CharacterType.None)
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
				this.m_freelancerDropdownBtn.Setup(GameWideData.Get().GetCharacterDisplayName(this.m_characterType), this.m_characterType);
			}
			else
			{
				this.m_freelancerDropdownBtn.Setup(StringUtil.TR("AllFreelancers", "Global"), this.m_characterType);
			}
		}
		int num = 0;
		int i = 0;
		List<QuestTemplate> list = QuestWideData.Get().m_quests;
		Dictionary<int, QuestTemplate> dictionary = new Dictionary<int, QuestTemplate>();
		int j = 0;
		while (j < list.Count)
		{
			QuestTemplate questTemplate = list[j];
			if (questTemplate.AchievmentType != AchievementType.None)
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
				if (!questTemplate.Enabled)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				else
				{
					if (this.m_achievementType != AchievementType.None)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (questTemplate.AchievmentType != this.m_achievementType)
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
							goto IL_1AA;
						}
					}
					if (this.m_achievementType == AchievementType.Freelancer)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_characterType != CharacterType.None)
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
							if (!QuestWideData.IsCharacterQuest(questTemplate.Objectives, GameWideData.Get().GetCharacterResourceLink(this.m_characterType)))
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
								goto IL_1AA;
							}
						}
					}
					dictionary[questTemplate.Index] = questTemplate;
				}
			}
			IL_1AA:
			j++;
			continue;
			goto IL_1AA;
		}
		list = new List<QuestTemplate>();
		list.AddRange(dictionary.Values);
		for (int k = 0; k < list.Count; k++)
		{
			if (list[k].AchievementPrevious > 0)
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
				dictionary.Remove(list[k].AchievementPrevious);
			}
		}
		bool flag = false;
		Dictionary<int, QuestMetaData> questMetaDatas = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.QuestMetaDatas;
		list.Clear();
		list.AddRange(dictionary.Values);
		int questCount = QuestWideData.Get().m_quests.Count;
		list.Sort((QuestTemplate x, QuestTemplate y) => x.SortOrder * questCount + x.Index - (y.SortOrder * questCount + y.Index));
		for (int l = 0; l < list.Count; l++)
		{
			QuestTemplate quest = list[l];
			UIPlayerProgressAchievementItem uiplayerProgressAchievementItem;
			if (i >= this.m_achievementItems.Count)
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
				uiplayerProgressAchievementItem = UnityEngine.Object.Instantiate<UIPlayerProgressAchievementItem>(this.m_itemPrefab);
				int num2 = this.m_achievementItems.Count % this.m_verticalColumns.Length;
				uiplayerProgressAchievementItem.transform.SetParent(this.m_verticalColumns[num2].transform);
				uiplayerProgressAchievementItem.transform.localPosition = Vector3.zero;
				uiplayerProgressAchievementItem.transform.localScale = Vector3.one;
				this.m_achievementItems.Add(uiplayerProgressAchievementItem);
				uiplayerProgressAchievementItem.m_selectableBtn.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
				flag = true;
			}
			else
			{
				uiplayerProgressAchievementItem = this.m_achievementItems[i];
			}
			UIManager.SetGameObjectActive(uiplayerProgressAchievementItem, true, null);
			i++;
			num += uiplayerProgressAchievementItem.Setup(quest, questMetaDatas);
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		while (i < this.m_achievementItems.Count)
		{
			UIManager.SetGameObjectActive(this.m_achievementItems[i], false, null);
			flag = true;
			i++;
		}
		this.m_points.text = string.Format(StringUtil.TR("AchievementPoints", "Global"), num);
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_scrollRect.verticalNormalizedPosition = 1f;
		}
	}
}
