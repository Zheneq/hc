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
		this.m_defaultSprite = this.m_contractIcon.sprite;
	}

	private void Awake()
	{
		base.GetComponent<_SelectableBtn>().spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleCollapseClick);
		this.m_trashButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleTrashClick);
		this.m_rejectAbandonButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleRejectAbandonClick);
		this.m_acceptAbandonButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HandleAcceptAbandonClick);
	}

	private void OnEnable()
	{
		if (this.m_questId > 0)
		{
			this.SetQuestId(this.m_questId);
		}
	}

	private void HandleCollapseClick(BaseEventData data)
	{
		QuestListPanel.Get().CollapseQuestId(this.m_questId);
	}

	private void HandleTrashClick(BaseEventData data)
	{
		this.m_trashButton.SetSelected(!this.m_trashButton.IsSelected(), false, string.Empty, string.Empty);
	}

	private void HandleRejectAbandonClick(BaseEventData data)
	{
		this.m_trashButton.SetSelected(false, false, string.Empty, string.Empty);
	}

	private void HandleAcceptAbandonClick(BaseEventData data)
	{
		this.m_trashButton.SetSelected(false, false, string.Empty, string.Empty);
		ClientGameManager.Get().AbandonDailyQuest(this.m_questId, null);
	}

	public void SetQuestId(int questId)
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
		QuestTemplate questTemplate = QuestWideData.Get().m_quests[questId - 1];
		string text = string.Empty;
		string text2 = StringUtil.TR_QuestName(questId);
		string text3 = StringUtil.TR_QuestDescription(questId);
		if (text2 != string.Empty)
		{
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
			this.m_contractIcon.sprite = sprite;
		}
		else
		{
			this.m_contractIcon.sprite = this.m_defaultSprite;
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
		string text4 = StringUtil.TR_QuestLongDescription(questId);
		int num3 = 0;
		using (List<QuestObjective>.Enumerator enumerator = questTemplate.Objectives.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestObjective questObjective = enumerator.Current;
				if (!questObjective.SuperHidden)
				{
					int maxCount = questObjective.MaxCount;
					int num4 = 0;
					if (questComponent.Progress[this.m_questId].ObjectiveProgress.ContainsKey(num3))
					{
						num4 = questComponent.Progress[this.m_questId].ObjectiveProgress[num3];
					}
					string text5 = StringUtil.TR_QuestObjective(questId, num3 + 1);
					if (num4 != 0)
					{
						goto IL_202;
					}
					if (!questObjective.Hidden)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							goto IL_202;
						}
					}
					IL_2DD:
					num3++;
					continue;
					IL_202:
					if (!(text5 == string.Empty))
					{
						if (text4 != string.Empty)
						{
							text4 += "\n";
						}
						if (maxCount == 1)
						{
							if (num4 == 1)
							{
								text4 += string.Format("    <color=white>{0}</color>", text5);
							}
							else
							{
								text4 += string.Format("    {0}", text5);
							}
						}
						else if (num4 == maxCount)
						{
							text4 += string.Format("    <color=white>{0} ({1}/{2})</color>", text5, num4, maxCount);
						}
						else
						{
							text4 += string.Format("    {0} ({1}/{2})", text5, num4, maxCount);
						}
					}
					goto IL_2DD;
				}
				num3++;
			}
		}
		this.m_longDescription.text = text4;
		if (questTemplate.ObjectiveCountType == RequiredObjectiveCountType.SumObjectiveProgress)
		{
			num2 = QuestItem.GetRequiredObjectiveCount(questTemplate);
			if (questComponent != null)
			{
				if (questComponent.Progress.ContainsKey(questId))
				{
					QuestProgress questProgress = questComponent.Progress[questId];
					using (Dictionary<int, int>.Enumerator enumerator2 = questProgress.ObjectiveProgress.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<int, int> keyValuePair = enumerator2.Current;
							int value = keyValuePair.Value;
							int key = keyValuePair.Key;
							if (questTemplate.Objectives.Count <= key)
							{
							}
							else if (questTemplate.Objectives[key].SuperHidden)
							{
							}
							else
							{
								num += value;
							}
						}
					}
				}
			}
		}
		else if (questTemplate.Objectives.Count == 1)
		{
			num2 = questTemplate.Objectives[0].MaxCount;
			if (questComponent != null)
			{
				if (questComponent.Progress.ContainsKey(questId))
				{
					QuestProgress questProgress2 = questComponent.Progress[questId];
					if (questProgress2.ObjectiveProgress.ContainsKey(0))
					{
						if (!questTemplate.Objectives[0].SuperHidden)
						{
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
				if (questComponent.Progress.ContainsKey(questId))
				{
					QuestProgress questProgress3 = questComponent.Progress[questId];
					using (Dictionary<int, int>.Enumerator enumerator3 = questProgress3.ObjectiveProgress.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<int, int> keyValuePair2 = enumerator3.Current;
							int key2 = keyValuePair2.Key;
							if (questTemplate.Objectives.Count > key2)
							{
								if (questTemplate.Objectives[key2].SuperHidden)
								{
								}
								else if (questTemplate.Objectives[keyValuePair2.Key].MaxCount <= keyValuePair2.Value)
								{
									num++;
								}
							}
						}
					}
				}
			}
		}
		num = Mathf.Min(num, num2);
		string text6 = string.Format("{0}/{1}", num, num2);
		this.m_progressText.text = text6;
		this.m_progressBar.fillAmount = (float)num / (1f * (float)num2);
		int num5 = 0;
		num5 += questTemplate.Rewards.CurrencyRewards.Count;
		num5 += questTemplate.Rewards.UnlockRewards.Count;
		num5 = Mathf.Max(0, Mathf.Min(3, num5));
		int num6 = 0;
		foreach (QuestReward questReward in this.m_questRewards)
		{
			if (num6 >= num5)
			{
				UIManager.SetGameObjectActive(questReward, false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(questReward, true, null);
				if (num6 < questTemplate.Rewards.CurrencyRewards.Count)
				{
					questReward.Setup(questTemplate.Rewards.CurrencyRewards[num6], rejectedCount);
				}
				else
				{
					questReward.SetupHack(questTemplate.Rewards.UnlockRewards[num6 - questTemplate.Rewards.CurrencyRewards.Count].resourceString, 0);
				}
				num6++;
			}
		}
		if (num5 == 0)
		{
			UIManager.SetGameObjectActive(this.m_rewardsContainer, false, null);
			UIManager.SetGameObjectActive(this.m_rewardsDivider, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_rewardsContainer, true, null);
			UIManager.SetGameObjectActive(this.m_rewardsDivider, true, null);
		}
		if (text4.IsNullOrEmpty())
		{
			UIManager.SetGameObjectActive(this.m_detailTextContainer, false, null);
			UIManager.SetGameObjectActive(this.m_detailsDivider, false, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_detailTextContainer, true, null);
			UIManager.SetGameObjectActive(this.m_detailsDivider, true, null);
		}
		this.m_lateUpdateCount = 5;
		this.UpdateHeight();
	}

	private void UpdateHeight()
	{
		if (this.m_detailTextContainer.gameObject.activeSelf)
		{
			if (this.m_longDescription.text == null)
			{
				this.m_longDescription.text = string.Empty;
			}
			this.m_longDescription.CalculateLayoutInputHorizontal();
			this.m_longDescription.CalculateLayoutInputVertical();
			float num = 0f;
			if (this.m_longDescription.text != null)
			{
				if (this.m_longDescription.text != string.Empty)
				{
					num = this.m_longDescription.preferredHeight + 20f;
				}
			}
			this.m_detailTextContainer.minHeight = num;
			this.m_detailTextContainer.preferredHeight = num;
		}
		this.m_verticalLayoutGroup.CalculateLayoutInputHorizontal();
		this.m_verticalLayoutGroup.CalculateLayoutInputVertical();
		try
		{
			RectTransform rectTransform = (RectTransform)base.transform;
			Vector2 sizeDelta = rectTransform.sizeDelta;
			sizeDelta.y = this.m_verticalLayoutGroup.preferredHeight;
			rectTransform.sizeDelta = sizeDelta;
			LayoutElement component = base.GetComponent<LayoutElement>();
			component.preferredHeight = this.m_verticalLayoutGroup.preferredHeight;
			component.minHeight = this.m_verticalLayoutGroup.preferredHeight;
		}
		catch (Exception ex)
		{
			Log.Info("Exception in UpdateHeight for QuestItemExpanded!" + ex.ToString(), new object[0]);
		}
	}

	private void LateUpdate()
	{
		if (this.m_lateUpdateCount > 0)
		{
			this.m_lateUpdateCount--;
			this.UpdateHeight();
		}
	}
}
