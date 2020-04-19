using System;
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
		this.m_defaultSprite = this.m_contractIcon.sprite;
	}

	private void Awake()
	{
		_SelectableBtn component = base.GetComponent<_SelectableBtn>();
		if (component != null)
		{
			base.GetComponent<_SelectableBtn>().spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleExpandClick);
		}
		this.m_questItemState = QuestItemState.None;
		ClientGameManager.Get().OnAccountDataUpdated += this.HandleAccountDataUpdated;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestItem.OnDestroy()).MethodHandle;
			}
			ClientGameManager.Get().OnAccountDataUpdated -= this.HandleAccountDataUpdated;
		}
	}

	private void HandleAccountDataUpdated(PersistedAccountData accountData)
	{
		if (this.m_questId > 0 && this.m_questItemState != QuestItemState.Finished)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestItem.HandleAccountDataUpdated(PersistedAccountData)).MethodHandle;
			}
			this.SetQuestId(this.m_questId);
		}
	}

	private void CallToClickAnim()
	{
		this.m_isAnimatingOut = false;
		QuestCompletePanel.Get().RemoveQuestCompleteNotification(this.m_questId);
	}

	private void HandleExpandClick(BaseEventData data)
	{
		if (this.m_questItemState == QuestItemState.Finished)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestItem.HandleExpandClick(BaseEventData)).MethodHandle;
			}
			if (!this.m_isAnimatingOut)
			{
				this.m_isAnimatingOut = true;
				UIAnimationEventManager.Get().PlayAnimation(this.m_animatorController, "contractItemNewOUT", new UIAnimationEventManager.AnimationDoneCallback(this.CallToClickAnim), string.Empty, 0, 0f, true, false, null, null);
			}
		}
		else
		{
			QuestListPanel.Get().ExpandQuestId(this.m_questId);
		}
	}

	public void SetQuestId(int questId)
	{
		int rejectedCount = 0;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestItem.SetQuestId(int)).MethodHandle;
			}
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			QuestComponent questComponent = playerAccountData.QuestComponent;
			if (questComponent != null)
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
				rejectedCount = questComponent.GetRejectedCount(questId);
			}
		}
		this.SetQuestId(questId, rejectedCount, false, false);
	}

	public void SetQuestId(int questId, int rejectedCount, bool questComplete = false, bool removedQuestCompleteNotification = false)
	{
		this.m_questId = questId;
		if (questId > QuestWideData.Get().m_quests.Count)
		{
			Log.Warning("Assigned quest id {0} that is missing data in QuestWideData.", new object[]
			{
				questId
			});
			return;
		}
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questId);
		string text = string.Empty;
		string text2 = StringUtil.TR_QuestName(questId);
		string text3 = StringUtil.TR_QuestDescription(questId);
		if (text2 != string.Empty)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestItem.SetQuestId(int, int, bool, bool)).MethodHandle;
			}
			text = string.Format("<size=20>{0}</size>\n<#a9a9a9>{1}", text2, text3);
		}
		else
		{
			text = text3;
		}
		this.m_nameText.text = text;
		Sprite sprite = (Sprite)Resources.Load(questTemplate.IconFilename, typeof(Sprite));
		if (sprite)
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
			this.m_contractIcon.sprite = sprite;
		}
		else
		{
			this.m_contractIcon.sprite = this.m_defaultSprite;
		}
		int num = 0;
		int num2 = 0;
		QuestItem.GetQuestProgress(questId, out num, out num2);
		if (questComplete)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num = num2;
		}
		string text4 = string.Format("{0}/{1}", num, num2);
		this.m_progressText.text = text4;
		this.m_progressBar.fillAmount = (float)num / (1f * (float)num2);
		int num3 = 0;
		num3 += questTemplate.Rewards.CurrencyRewards.Count;
		num3 += questTemplate.Rewards.UnlockRewards.Count;
		num3 += questTemplate.Rewards.ItemRewards.Count;
		if (questTemplate.ConditionalRewards != null)
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
			for (int i = 0; i < questTemplate.ConditionalRewards.Length; i++)
			{
				if (QuestWideData.AreConditionsMet(questTemplate.ConditionalRewards[i].Prerequisites.Conditions, questTemplate.ConditionalRewards[i].Prerequisites.LogicStatement, false))
				{
					num3 += questTemplate.ConditionalRewards[i].CurrencyRewards.Count;
					num3 += questTemplate.ConditionalRewards[i].UnlockRewards.Count;
					num3 += questTemplate.ConditionalRewards[i].ItemRewards.Count;
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
		num3 = Mathf.Max(0, Mathf.Min(3, num3));
		if (num3 == 0)
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
			UIManager.SetGameObjectActive(this.m_rewardText, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_rewardText, true, null);
		}
		for (int j = 0; j < this.m_rewardStates.Length; j++)
		{
			if (j == num3 - 1)
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
				UIManager.SetGameObjectActive(this.m_rewardStates[j], true, null);
				QuestRewardGroup component = this.m_rewardStates[j].GetComponent<QuestRewardGroup>();
				if (component != null)
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
					component.SetupChildren(questId, rejectedCount, removedQuestCompleteNotification);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_rewardStates[j], false, null);
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public unsafe static bool GetQuestProgress(int questId, out int currentProgress, out int maxProgress)
	{
		currentProgress = 0;
		maxProgress = 0;
		if (questId > QuestWideData.Get().m_quests.Count)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestItem.GetQuestProgress(int, int*, int*)).MethodHandle;
			}
			return false;
		}
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(questId);
		QuestComponent questComponent = null;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			questComponent = playerAccountData.QuestComponent;
		}
		if (questTemplate.ObjectiveCountType == RequiredObjectiveCountType.SumObjectiveProgress)
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
			maxProgress = QuestItem.GetRequiredObjectiveCount(questTemplate);
			if (questComponent != null)
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
				if (questComponent.Progress != null)
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
					if (questComponent.Progress.ContainsKey(questId))
					{
						QuestProgress questProgress = questComponent.Progress[questId];
						foreach (KeyValuePair<int, int> keyValuePair in questProgress.ObjectiveProgress)
						{
							int key = keyValuePair.Key;
							int value = keyValuePair.Value;
							if (questTemplate.Objectives.Count > key)
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
								if (!questTemplate.Objectives[key].SuperHidden)
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			maxProgress = questTemplate.Objectives[0].MaxCount;
			if (maxProgress == 1)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				QuestCondition questCondition = null;
				using (List<QuestTrigger>.Enumerator enumerator2 = questTemplate.Objectives[0].Triggers.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						QuestTrigger questTrigger = enumerator2.Current;
						if (questTrigger.Conditions.Count != 1)
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
						}
						else
						{
							if (questCondition == null)
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
								questCondition = questTrigger.Conditions[0];
								continue;
							}
							if (questCondition.Equals(questTrigger.Conditions[0]))
							{
								continue;
							}
							questCondition = null;
						}
						goto IL_25C;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				IL_25C:
				if (questCondition != null)
				{
					QuestConditionType conditionType = questCondition.ConditionType;
					if (conditionType != QuestConditionType.HasAccountLevel)
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
						if (conditionType == QuestConditionType.HasFriendCountOrMore)
						{
							maxProgress = questCondition.typeSpecificData;
							currentProgress = (from x in ClientGameManager.Get().FriendList.Friends.Values
							where x.FriendStatus == FriendStatus.Friend
							select x).Count<FriendInfo>();
						}
					}
					else
					{
						maxProgress = questCondition.typeSpecificData;
						if (ClientGameManager.Get() != null && ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				for (;;)
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
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					QuestProgress questProgress2 = questComponent.Progress[questId];
					if (questProgress2.ObjectiveProgress.ContainsKey(0))
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
						currentProgress += questProgress2.ObjectiveProgress[0];
					}
				}
			}
		}
		else
		{
			maxProgress = QuestItem.GetRequiredObjectiveCount(questTemplate);
			if (questComponent != null && questComponent.Progress.ContainsKey(questId))
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
				QuestProgress questProgress3 = questComponent.Progress[questId];
				foreach (KeyValuePair<int, int> keyValuePair2 in questProgress3.ObjectiveProgress)
				{
					if (keyValuePair2.Key < questTemplate.Objectives.Count && questTemplate.Objectives[keyValuePair2.Key].MaxCount <= keyValuePair2.Value && !questTemplate.Objectives[keyValuePair2.Key].SuperHidden)
					{
						currentProgress++;
					}
				}
			}
		}
		currentProgress = Mathf.Min(currentProgress, maxProgress);
		if (questComponent != null)
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
			if (!questComponent.Progress.ContainsKey(questId))
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
				if (questComponent.GetCompletedCount(questId) > 0)
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
					currentProgress = maxProgress;
				}
			}
		}
		return true;
	}

	public void SetState(QuestItemState newState)
	{
		if (this.m_questItemState == newState)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestItem.SetState(QuestItemState)).MethodHandle;
			}
			return;
		}
		this.m_questItemState = newState;
		if (this.m_questItemState == QuestItemState.Empty)
		{
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			UIManager.SetGameObjectActive(this.m_emptyState, true, null);
			UIManager.SetGameObjectActive(this.m_defaultState, false, null);
			UIManager.SetGameObjectActive(this.m_hoverState, false, null);
			UIManager.SetGameObjectActive(this.m_pressedState, false, null);
			UIManager.SetGameObjectActive(this.m_selectedState, false, null);
			UIManager.SetGameObjectActive(this.m_hitBox, false, null);
		}
		else if (this.m_questItemState == QuestItemState.Filled)
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
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			UIManager.SetGameObjectActive(this.m_emptyState, false, null);
			UIManager.SetGameObjectActive(this.m_defaultState, true, null);
			UIManager.SetGameObjectActive(this.m_hoverState, false, null);
			UIManager.SetGameObjectActive(this.m_pressedState, false, null);
			UIManager.SetGameObjectActive(this.m_selectedState, false, null);
			UIManager.SetGameObjectActive(this.m_hitBox, true, null);
		}
		else if (this.m_questItemState == QuestItemState.Expanded)
		{
			UIManager.SetGameObjectActive(base.gameObject, false, null);
			UIManager.SetGameObjectActive(this.m_emptyState, false, null);
			UIManager.SetGameObjectActive(this.m_defaultState, false, null);
			UIManager.SetGameObjectActive(this.m_hoverState, false, null);
			UIManager.SetGameObjectActive(this.m_pressedState, false, null);
			UIManager.SetGameObjectActive(this.m_selectedState, false, null);
			UIManager.SetGameObjectActive(this.m_hitBox, true, null);
		}
		else if (this.m_questItemState == QuestItemState.Finished)
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
			UIManager.SetGameObjectActive(base.gameObject, true, null);
			UIManager.SetGameObjectActive(this.m_emptyState, false, null);
			UIManager.SetGameObjectActive(this.m_defaultState, true, null);
			UIManager.SetGameObjectActive(this.m_hoverState, false, null);
			UIManager.SetGameObjectActive(this.m_pressedState, false, null);
			UIManager.SetGameObjectActive(this.m_selectedState, false, null);
			UIManager.SetGameObjectActive(this.m_hitBox, true, null);
		}
	}

	public static int GetRequiredObjectiveCount(QuestTemplate questTemplate)
	{
		int num = 0;
		if (questTemplate.ObjectiveCountType == RequiredObjectiveCountType.SumObjectiveProgress)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(QuestItem.GetRequiredObjectiveCount(QuestTemplate)).MethodHandle;
			}
			using (List<QuestObjective>.Enumerator enumerator = questTemplate.Objectives.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QuestObjective questObjective = enumerator.Current;
					num += questObjective.MaxCount;
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
			}
		}
		else
		{
			num = questTemplate.Objectives.Count;
		}
		if (questTemplate.RequiredObjectiveCount != 0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num = questTemplate.RequiredObjectiveCount;
		}
		if (questTemplate.CosmeticRequiredObjectiveCount != 0)
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
			num = questTemplate.CosmeticRequiredObjectiveCount;
		}
		return num;
	}
}
