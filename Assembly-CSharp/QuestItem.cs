using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuestItem : MonoBehaviour
{
	public GameObject m_emptyState;

	public GameObject m_defaultState;

	public GameObject m_hoverState;

	public GameObject m_pressedState;

	public GameObject m_selectedState;

	public GameObject m_hitBox;

	public GameObject m_expandArrow;

	public TextMeshProUGUI m_nameText;

	public TextMeshProUGUI m_progressText;

	public TextMeshProUGUI m_rewardText;

	public GameObject[] m_rewardStates;

	public Image m_progressBar;

	public Image m_contractIcon;

	public Animator m_animatorController;

	private Sprite m_defaultSprite;

	[HideInInspector]
	public int m_questId;

	[HideInInspector]
	public QuestItemState m_questItemState;

	private bool m_isAnimatingOut;

	private void Start()
	{
		m_defaultSprite = m_contractIcon.sprite;
	}

	private void Awake()
	{
		_SelectableBtn component = GetComponent<_SelectableBtn>();
		if (component != null)
		{
			GetComponent<_SelectableBtn>().spriteController.callback = HandleExpandClick;
		}
		m_questItemState = QuestItemState.None;
		ClientGameManager.Get().OnAccountDataUpdated += HandleAccountDataUpdated;
	}

	private void OnDestroy()
	{
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= HandleAccountDataUpdated;
			return;
		}
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		if (m_questId <= 0 || m_questItemState == QuestItemState.Finished)
		{
			return;
		}
		while (true)
		{
			SetQuestId(m_questId);
			return;
		}
	}

	private void CallToClickAnim()
	{
		m_isAnimatingOut = false;
		QuestCompletePanel.Get().RemoveQuestCompleteNotification(m_questId);
	}

	private void HandleExpandClick(BaseEventData data)
	{
		if (m_questItemState == QuestItemState.Finished)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (!m_isAnimatingOut)
					{
						m_isAnimatingOut = true;
						UIAnimationEventManager.Get().PlayAnimation(m_animatorController, "contractItemNewOUT", CallToClickAnim, string.Empty);
					}
					return;
				}
			}
		}
		QuestListPanel.Get().ExpandQuestId(m_questId);
	}

	public void SetQuestId(int questId)
	{
		int rejectedCount = 0;
		QuestComponent questComponent = null;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			questComponent = playerAccountData.QuestComponent;
			if (questComponent != null)
			{
				rejectedCount = questComponent.GetRejectedCount(questId);
			}
		}
		SetQuestId(questId, rejectedCount);
	}

	public void SetQuestId(int questId, int rejectedCount, bool questComplete = false, bool removedQuestCompleteNotification = false)
	{
		m_questId = questId;
		if (questId > QuestWideData.Get().m_quests.Count)
		{
			Log.Warning("Assigned quest id {0} that is missing data in QuestWideData.", questId);
			return;
		}
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questId);
		string empty = string.Empty;
		string text = StringUtil.TR_QuestName(questId);
		string text2 = StringUtil.TR_QuestDescription(questId);
		if (text != string.Empty)
		{
			empty = $"<size=20>{text}</size>\n<#a9a9a9>{text2}";
		}
		else
		{
			empty = text2;
		}
		m_nameText.text = empty;
		Sprite sprite = (Sprite)Resources.Load(questTemplate.IconFilename, typeof(Sprite));
		if ((bool)sprite)
		{
			m_contractIcon.sprite = sprite;
		}
		else
		{
			m_contractIcon.sprite = m_defaultSprite;
		}
		int currentProgress = 0;
		int maxProgress = 0;
		GetQuestProgress(questId, out currentProgress, out maxProgress);
		if (questComplete)
		{
			currentProgress = maxProgress;
		}
		string text3 = $"{currentProgress}/{maxProgress}";
		m_progressText.text = text3;
		m_progressBar.fillAmount = (float)currentProgress / (1f * (float)maxProgress);
		int num = 0;
		num += questTemplate.Rewards.CurrencyRewards.Count;
		num += questTemplate.Rewards.UnlockRewards.Count;
		num += questTemplate.Rewards.ItemRewards.Count;
		if (questTemplate.ConditionalRewards != null)
		{
			for (int i = 0; i < questTemplate.ConditionalRewards.Length; i++)
			{
				if (QuestWideData.AreConditionsMet(questTemplate.ConditionalRewards[i].Prerequisites.Conditions, questTemplate.ConditionalRewards[i].Prerequisites.LogicStatement))
				{
					num += questTemplate.ConditionalRewards[i].CurrencyRewards.Count;
					num += questTemplate.ConditionalRewards[i].UnlockRewards.Count;
					num += questTemplate.ConditionalRewards[i].ItemRewards.Count;
				}
			}
		}
		num = Mathf.Max(0, Mathf.Min(3, num));
		if (num == 0)
		{
			UIManager.SetGameObjectActive(m_rewardText, false);
		}
		else
		{
			UIManager.SetGameObjectActive(m_rewardText, true);
		}
		for (int j = 0; j < m_rewardStates.Length; j++)
		{
			if (j == num - 1)
			{
				UIManager.SetGameObjectActive(m_rewardStates[j], true);
				QuestRewardGroup component = m_rewardStates[j].GetComponent<QuestRewardGroup>();
				if (component != null)
				{
					component.SetupChildren(questId, rejectedCount, removedQuestCompleteNotification);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(m_rewardStates[j], false);
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public static bool GetQuestProgress(int questId, out int currentProgress, out int maxProgress)
	{
		currentProgress = 0;
		maxProgress = 0;
		if (questId > QuestWideData.Get().m_quests.Count)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questId);
		QuestComponent questComponent = null;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			questComponent = playerAccountData.QuestComponent;
		}
		if (questTemplate.ObjectiveCountType == RequiredObjectiveCountType.SumObjectiveProgress)
		{
			maxProgress = GetRequiredObjectiveCount(questTemplate);
			if (questComponent != null)
			{
				if (questComponent.Progress != null)
				{
					if (questComponent.Progress.ContainsKey(questId))
					{
						QuestProgress questProgress = questComponent.Progress[questId];
						foreach (KeyValuePair<int, int> item in questProgress.ObjectiveProgress)
						{
							int key = item.Key;
							int value = item.Value;
							if (questTemplate.Objectives.Count > key)
							{
								if (!questTemplate.Objectives[key].SuperHidden)
								{
									currentProgress += value;
								}
							}
						}
					}
				}
			}
		}
		else if (questTemplate.Objectives.Count == 1)
		{
			maxProgress = questTemplate.Objectives[0].MaxCount;
			if (maxProgress == 1)
			{
				QuestCondition questCondition = null;
				using (List<QuestTrigger>.Enumerator enumerator2 = questTemplate.Objectives[0].Triggers.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator2.MoveNext())
						{
							break;
						}
						QuestTrigger current2 = enumerator2.Current;
						if (current2.Conditions.Count != 1)
						{
							break;
						}
						if (questCondition == null)
						{
							questCondition = current2.Conditions[0];
						}
						else if (!questCondition.Equals(current2.Conditions[0]))
						{
							questCondition = null;
							break;
						}
					}
				}
				if (questCondition != null)
				{
					QuestConditionType conditionType = questCondition.ConditionType;
					if (conditionType != QuestConditionType.HasAccountLevel)
					{
						if (conditionType == QuestConditionType.HasFriendCountOrMore)
						{
							maxProgress = questCondition.typeSpecificData;
							currentProgress = ClientGameManager.Get().FriendList.Friends.Values.Where((FriendInfo x) => x.FriendStatus == FriendStatus.Friend).Count();
						}
					}
					else
					{
						maxProgress = questCondition.typeSpecificData;
						if (ClientGameManager.Get() != null && ClientGameManager.Get().IsPlayerAccountDataAvailable())
						{
							currentProgress = ClientGameManager.Get().GetPlayerAccountData().GetReactorLevel(SeasonWideData.Get().m_seasons);
						}
						else
						{
							currentProgress = 0;
						}
					}
				}
			}
			else if (questComponent != null)
			{
				if (questComponent.Progress.ContainsKey(questId))
				{
					QuestProgress questProgress2 = questComponent.Progress[questId];
					if (questProgress2.ObjectiveProgress.ContainsKey(0))
					{
						currentProgress += questProgress2.ObjectiveProgress[0];
					}
				}
			}
		}
		else
		{
			maxProgress = GetRequiredObjectiveCount(questTemplate);
			if (questComponent != null && questComponent.Progress.ContainsKey(questId))
			{
				QuestProgress questProgress3 = questComponent.Progress[questId];
				foreach (KeyValuePair<int, int> item2 in questProgress3.ObjectiveProgress)
				{
					if (item2.Key < questTemplate.Objectives.Count && questTemplate.Objectives[item2.Key].MaxCount <= item2.Value && !questTemplate.Objectives[item2.Key].SuperHidden)
					{
						currentProgress++;
					}
				}
			}
		}
		currentProgress = Mathf.Min(currentProgress, maxProgress);
		if (questComponent != null)
		{
			if (!questComponent.Progress.ContainsKey(questId))
			{
				if (questComponent.GetCompletedCount(questId) > 0)
				{
					currentProgress = maxProgress;
				}
			}
		}
		return true;
	}

	public void SetState(QuestItemState newState)
	{
		if (m_questItemState == newState)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_questItemState = newState;
		if (m_questItemState == QuestItemState.Empty)
		{
			UIManager.SetGameObjectActive(base.gameObject, true);
			UIManager.SetGameObjectActive(m_emptyState, true);
			UIManager.SetGameObjectActive(m_defaultState, false);
			UIManager.SetGameObjectActive(m_hoverState, false);
			UIManager.SetGameObjectActive(m_pressedState, false);
			UIManager.SetGameObjectActive(m_selectedState, false);
			UIManager.SetGameObjectActive(m_hitBox, false);
			return;
		}
		if (m_questItemState == QuestItemState.Filled)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(base.gameObject, true);
					UIManager.SetGameObjectActive(m_emptyState, false);
					UIManager.SetGameObjectActive(m_defaultState, true);
					UIManager.SetGameObjectActive(m_hoverState, false);
					UIManager.SetGameObjectActive(m_pressedState, false);
					UIManager.SetGameObjectActive(m_selectedState, false);
					UIManager.SetGameObjectActive(m_hitBox, true);
					return;
				}
			}
		}
		if (m_questItemState == QuestItemState.Expanded)
		{
			UIManager.SetGameObjectActive(base.gameObject, false);
			UIManager.SetGameObjectActive(m_emptyState, false);
			UIManager.SetGameObjectActive(m_defaultState, false);
			UIManager.SetGameObjectActive(m_hoverState, false);
			UIManager.SetGameObjectActive(m_pressedState, false);
			UIManager.SetGameObjectActive(m_selectedState, false);
			UIManager.SetGameObjectActive(m_hitBox, true);
		}
		else
		{
			if (m_questItemState != QuestItemState.Finished)
			{
				return;
			}
			while (true)
			{
				UIManager.SetGameObjectActive(base.gameObject, true);
				UIManager.SetGameObjectActive(m_emptyState, false);
				UIManager.SetGameObjectActive(m_defaultState, true);
				UIManager.SetGameObjectActive(m_hoverState, false);
				UIManager.SetGameObjectActive(m_pressedState, false);
				UIManager.SetGameObjectActive(m_selectedState, false);
				UIManager.SetGameObjectActive(m_hitBox, true);
				return;
			}
		}
	}

	public static int GetRequiredObjectiveCount(QuestTemplate questTemplate)
	{
		int num = 0;
		if (questTemplate.ObjectiveCountType == RequiredObjectiveCountType.SumObjectiveProgress)
		{
			using (List<QuestObjective>.Enumerator enumerator = questTemplate.Objectives.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestObjective current = enumerator.Current;
					num += current.MaxCount;
				}
			}
		}
		else
		{
			num = questTemplate.Objectives.Count;
		}
		if (questTemplate.RequiredObjectiveCount != 0)
		{
			num = questTemplate.RequiredObjectiveCount;
		}
		if (questTemplate.CosmeticRequiredObjectiveCount != 0)
		{
			num = questTemplate.CosmeticRequiredObjectiveCount;
		}
		return num;
	}
}
