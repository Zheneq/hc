using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestItemExpanded : MonoBehaviour
{
	public GameObject m_defaultState;

	public GameObject m_hoverState;

	public GameObject m_pressedState;

	public GameObject m_selectedState;

	public GameObject m_hitBox;

	public TextMeshProUGUI m_nameText;

	public TextMeshProUGUI m_progressText;

	public TextMeshProUGUI m_longDescription;

	public TextMeshProUGUI m_rewardText;

	public Image m_progressBar;

	public Image m_contractIcon;

	public QuestReward[] m_questRewards;

	public _SelectableBtn m_trashButton;

	public _SelectableBtn m_rejectAbandonButton;

	public _SelectableBtn m_acceptAbandonButton;

	public VerticalLayoutGroup m_verticalLayoutGroup;

	public LayoutElement m_detailTextContainer;

	public LayoutElement m_rewardsContainer;

	public GameObject m_detailsDivider;

	public GameObject m_rewardsDivider;

	private Sprite m_defaultSprite;

	[HideInInspector]
	public int m_questId;

	private int m_lateUpdateCount;

	private void Start()
	{
		m_defaultSprite = m_contractIcon.sprite;
	}

	private void Awake()
	{
		GetComponent<_SelectableBtn>().spriteController.callback = HandleCollapseClick;
		m_trashButton.spriteController.callback = HandleTrashClick;
		m_rejectAbandonButton.spriteController.callback = HandleRejectAbandonClick;
		m_acceptAbandonButton.spriteController.callback = HandleAcceptAbandonClick;
	}

	private void OnEnable()
	{
		if (m_questId <= 0)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			SetQuestId(m_questId);
			return;
		}
	}

	private void HandleCollapseClick(BaseEventData data)
	{
		QuestListPanel.Get().CollapseQuestId(m_questId);
	}

	private void HandleTrashClick(BaseEventData data)
	{
		m_trashButton.SetSelected(!m_trashButton.IsSelected(), false, string.Empty, string.Empty);
	}

	private void HandleRejectAbandonClick(BaseEventData data)
	{
		m_trashButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	private void HandleAcceptAbandonClick(BaseEventData data)
	{
		m_trashButton.SetSelected(false, false, string.Empty, string.Empty);
		ClientGameManager.Get().AbandonDailyQuest(m_questId);
	}

	public void SetQuestId(int questId)
	{
		m_questId = questId;
		if (questId > QuestWideData.Get().m_quests.Count)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Log.Warning("Assigned quest id {0} that is missing data in QuestWideData.", questId);
					return;
				}
			}
		}
		QuestTemplate questTemplate = QuestWideData.Get().m_quests[questId - 1];
		string empty = string.Empty;
		string text = StringUtil.TR_QuestName(questId);
		string text2 = StringUtil.TR_QuestDescription(questId);
		empty = ((!(text != string.Empty)) ? text2 : $"<size=20>{text}</size>\n<#a9a9a9>{text2}");
		m_nameText.text = empty;
		Sprite sprite = (Sprite)Resources.Load(questTemplate.IconFilename, typeof(Sprite));
		if ((bool)sprite)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_contractIcon.sprite = sprite;
		}
		else
		{
			m_contractIcon.sprite = m_defaultSprite;
		}
		int num = 0;
		int num2 = 0;
		int rejectedCount = 0;
		QuestComponent questComponent = null;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			questComponent = playerAccountData.QuestComponent;
			rejectedCount = questComponent.GetRejectedCount(questId);
		}
		string text3 = StringUtil.TR_QuestLongDescription(questId);
		int num3 = 0;
		using (List<QuestObjective>.Enumerator enumerator = questTemplate.Objectives.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestObjective current = enumerator.Current;
				if (current.SuperHidden)
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
					num3++;
					continue;
				}
				int maxCount = current.MaxCount;
				int num4 = 0;
				if (questComponent.Progress[m_questId].ObjectiveProgress.ContainsKey(num3))
				{
					num4 = questComponent.Progress[m_questId].ObjectiveProgress[num3];
				}
				string text4 = StringUtil.TR_QuestObjective(questId, num3 + 1);
				if (num4 == 0)
				{
					if (current.Hidden)
					{
						goto IL_02dd;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (!(text4 == string.Empty))
				{
					if (text3 != string.Empty)
					{
						text3 += "\n";
					}
					if (maxCount != 1)
					{
						text3 = ((num4 != maxCount) ? (text3 + $"    {text4} ({num4}/{maxCount})") : (text3 + $"    <color=white>{text4} ({num4}/{maxCount})</color>"));
					}
					else
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (num4 == 1)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							text3 += $"    <color=white>{text4}</color>";
						}
						else
						{
							text3 += $"    {text4}";
						}
					}
				}
				goto IL_02dd;
				IL_02dd:
				num3++;
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
		m_longDescription.text = text3;
		if (questTemplate.ObjectiveCountType == RequiredObjectiveCountType.SumObjectiveProgress)
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
			num2 = QuestItem.GetRequiredObjectiveCount(questTemplate);
			if (questComponent != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (questComponent.Progress.ContainsKey(questId))
				{
					QuestProgress questProgress = questComponent.Progress[questId];
					using (Dictionary<int, int>.Enumerator enumerator2 = questProgress.ObjectiveProgress.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<int, int> current2 = enumerator2.Current;
							int value = current2.Value;
							int key = current2.Key;
							if (questTemplate.Objectives.Count <= key)
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
							}
							else if (questTemplate.Objectives[key].SuperHidden)
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
							}
							else
							{
								num += value;
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
				}
			}
		}
		else if (questTemplate.Objectives.Count == 1)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num2 = questTemplate.Objectives[0].MaxCount;
			if (questComponent != null)
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
				if (questComponent.Progress.ContainsKey(questId))
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
					QuestProgress questProgress2 = questComponent.Progress[questId];
					if (questProgress2.ObjectiveProgress.ContainsKey(0))
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!questTemplate.Objectives[0].SuperHidden)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							num += questProgress2.ObjectiveProgress[0];
						}
					}
				}
			}
		}
		else
		{
			num2 = QuestItem.GetRequiredObjectiveCount(questTemplate);
			if (questComponent != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (questComponent.Progress.ContainsKey(questId))
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
					QuestProgress questProgress3 = questComponent.Progress[questId];
					using (Dictionary<int, int>.Enumerator enumerator3 = questProgress3.ObjectiveProgress.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<int, int> current3 = enumerator3.Current;
							int key2 = current3.Key;
							if (questTemplate.Objectives.Count > key2)
							{
								if (questTemplate.Objectives[key2].SuperHidden)
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
								}
								else if (questTemplate.Objectives[current3.Key].MaxCount <= current3.Value)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									num++;
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
				}
			}
		}
		num = Mathf.Min(num, num2);
		string text5 = $"{num}/{num2}";
		m_progressText.text = text5;
		m_progressBar.fillAmount = (float)num / (1f * (float)num2);
		int num5 = 0;
		num5 += questTemplate.Rewards.CurrencyRewards.Count;
		num5 += questTemplate.Rewards.UnlockRewards.Count;
		num5 = Mathf.Max(0, Mathf.Min(3, num5));
		int num6 = 0;
		QuestReward[] questRewards = m_questRewards;
		foreach (QuestReward questReward in questRewards)
		{
			if (num6 >= num5)
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
				UIManager.SetGameObjectActive(questReward, false);
			}
			else
			{
				UIManager.SetGameObjectActive(questReward, true);
				if (num6 < questTemplate.Rewards.CurrencyRewards.Count)
				{
					questReward.Setup(questTemplate.Rewards.CurrencyRewards[num6], rejectedCount);
				}
				else
				{
					questReward.SetupHack(questTemplate.Rewards.UnlockRewards[num6 - questTemplate.Rewards.CurrencyRewards.Count].resourceString);
				}
				num6++;
			}
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (num5 == 0)
			{
				UIManager.SetGameObjectActive(m_rewardsContainer, false);
				UIManager.SetGameObjectActive(m_rewardsDivider, false);
			}
			else
			{
				UIManager.SetGameObjectActive(m_rewardsContainer, true);
				UIManager.SetGameObjectActive(m_rewardsDivider, true);
			}
			if (text3.IsNullOrEmpty())
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(m_detailTextContainer, false);
				UIManager.SetGameObjectActive(m_detailsDivider, false);
			}
			else
			{
				UIManager.SetGameObjectActive(m_detailTextContainer, true);
				UIManager.SetGameObjectActive(m_detailsDivider, true);
			}
			m_lateUpdateCount = 5;
			UpdateHeight();
			return;
		}
	}

	private void UpdateHeight()
	{
		if (m_detailTextContainer.gameObject.activeSelf)
		{
			if (m_longDescription.text == null)
			{
				while (true)
				{
					switch (3)
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
				m_longDescription.text = string.Empty;
			}
			m_longDescription.CalculateLayoutInputHorizontal();
			m_longDescription.CalculateLayoutInputVertical();
			float num = 0f;
			if (m_longDescription.text != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_longDescription.text != string.Empty)
				{
					num = m_longDescription.preferredHeight + 20f;
				}
			}
			m_detailTextContainer.minHeight = num;
			m_detailTextContainer.preferredHeight = num;
		}
		m_verticalLayoutGroup.CalculateLayoutInputHorizontal();
		m_verticalLayoutGroup.CalculateLayoutInputVertical();
		try
		{
			RectTransform rectTransform = (RectTransform)base.transform;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			sizeDelta.y = m_verticalLayoutGroup.preferredHeight;
			rectTransform.sizeDelta = sizeDelta;
			LayoutElement component = GetComponent<LayoutElement>();
			component.preferredHeight = m_verticalLayoutGroup.preferredHeight;
			component.minHeight = m_verticalLayoutGroup.preferredHeight;
		}
		catch (Exception ex)
		{
			Log.Info("Exception in UpdateHeight for QuestItemExpanded!" + ex.ToString());
		}
	}

	private void LateUpdate()
	{
		if (m_lateUpdateCount <= 0)
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
			m_lateUpdateCount--;
			UpdateHeight();
			return;
		}
	}
}
