using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISeasonsBaseContract : MonoBehaviour
{
	public _SelectableBtn m_btnHitBox;

	public LayoutElement m_layoutElement;

	public TextMeshProUGUI m_progressText;

	public Image m_progessFilled;

	public Image[] m_downArrows;

	public Animator m_animationController;

	public bool m_collapseOtherEntries = true;

	public TextMeshProUGUI m_remainingTime;

	[Header("Contracted Rewards")]
	public RectTransform m_contractedRewardsContainer;

	public RectTransform[] m_rewardsContainer;

	public QuestReward m_singleReward;

	public TextMeshProUGUI m_QuestDescription;

	public QuestReward[] m_twoReward = new QuestReward[2];

	public QuestReward[] m_threeReward = new QuestReward[3];

	[Header("Expanded Elements")]
	public VerticalLayoutGroup m_expandedGroup;

	public LayoutElement m_headerElement;

	public LayoutElement m_detailsElement;

	public LayoutElement m_rewardsElement;

	public LayoutElement m_abandonElement;

	public TextMeshProUGUI m_ContractText;

	public TextMeshProUGUI m_DetailText;

	public QuestReward[] m_Rewards;

	public _SelectableBtn m_TrashBtn;

	public _SelectableBtn m_acceptTrashBtn;

	public _SelectableBtn m_declineTrashBtn;

	protected bool m_expanded;

	protected bool m_initialized;

	protected UIBaseQuestDisplayInfo m_infoReference;

	private ScrollRect m_scrollRect;

	private bool m_scrollRectInit;

	private int m_isExpandingOrContracting;

	private float m_contractedHeight;

	private float m_currentHeight;

	private float m_startLocation;

	private float m_endLocation;

	private float m_timeToChangeHeight = 0.25f;

	private float m_timeStartChange;

	private float m_rewardsHeight = 120f;

	public bool IsExpanded()
	{
		return m_expanded;
	}

	public void Awake()
	{
		Init();
	}

	protected virtual void Init()
	{
		if (m_initialized)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_initialized = true;
		if (UISeasonsPanel.Get() != null)
		{
			if (base.transform.IsChildOf(UISeasonsPanel.Get().m_QuestChallengeScrollList.transform))
			{
				_MouseEventPasser mouseEventPasser = m_btnHitBox.spriteController.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser.AddNewHandler(UISeasonsPanel.Get().m_QuestChallengeScrollList);
			}
		}
		m_btnHitBox.spriteController.callback = ContractClicked;
		m_TrashBtn.spriteController.callback = TrashClicked;
		m_acceptTrashBtn.spriteController.callback = AcceptTrashClicked;
		m_declineTrashBtn.spriteController.callback = DeclineTrashClicked;
		UIManager.SetGameObjectActive(m_acceptTrashBtn, false);
		UIManager.SetGameObjectActive(m_declineTrashBtn, false);
		m_currentHeight = m_layoutElement.minHeight;
		m_contractedHeight = m_layoutElement.minHeight;
		m_progessFilled.fillAmount = 0f;
		UIManager.SetGameObjectActive(m_contractedRewardsContainer, false);
		UIManager.SetGameObjectActive(m_expandedGroup, true);
		m_headerElement.flexibleHeight = 0f;
		m_detailsElement.flexibleHeight = 0f;
		m_rewardsElement.flexibleHeight = 0f;
		m_abandonElement.flexibleHeight = 0f;
		m_btnHitBox.spriteController.AddSubButton(m_TrashBtn.spriteController);
		m_btnHitBox.spriteController.AddSubButton(m_acceptTrashBtn.spriteController);
		m_btnHitBox.spriteController.AddSubButton(m_declineTrashBtn.spriteController);
		m_btnHitBox.spriteController.RegisterScrollListener(OnScroll);
		for (int i = 0; i < m_Rewards.Length; i++)
		{
			_SelectableBtn component = m_Rewards[i].GetComponent<_SelectableBtn>();
			if (component != null)
			{
				m_btnHitBox.spriteController.AddSubButton(component.spriteController);
				component.spriteController.callback = ContractClicked;
			}
		}
		if (m_singleReward != null)
		{
			_SelectableBtn component2 = m_singleReward.GetComponent<_SelectableBtn>();
			if (component2 != null)
			{
				component2.spriteController.callback = ContractClicked;
			}
		}
		if (m_twoReward != null)
		{
			for (int j = 0; j < m_twoReward.Length; j++)
			{
				_SelectableBtn component3 = m_twoReward[j].GetComponent<_SelectableBtn>();
				if (component3 != null)
				{
					component3.spriteController.callback = ContractClicked;
				}
			}
		}
		if (m_threeReward != null)
		{
			for (int k = 0; k < m_threeReward.Length; k++)
			{
				_SelectableBtn component4 = m_threeReward[k].GetComponent<_SelectableBtn>();
				if (component4 != null)
				{
					component4.spriteController.callback = ContractClicked;
				}
			}
		}
		m_acceptTrashBtn.SetSelected(false, false, string.Empty, string.Empty);
		m_declineTrashBtn.SetSelected(false, false, string.Empty, string.Empty);
		if (m_remainingTime != null)
		{
			UIManager.SetGameObjectActive(m_remainingTime, false);
		}
		Graphic[] componentsInChildren = m_expandedGroup.GetComponentsInChildren<Graphic>(true);
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			if (componentsInChildren[l].GetComponent<_ButtonSwapSprite>() == null)
			{
				componentsInChildren[l].raycastTarget = false;
			}
		}
		TextMeshProUGUI[] componentsInChildren2 = base.gameObject.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int m = 0; m < componentsInChildren2.Length; m++)
		{
			componentsInChildren2[m].raycastTarget = false;
		}
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	protected virtual void PlayExpandAnimation()
	{
	}

	protected virtual void PlayContractAnimation()
	{
	}

	public virtual float GetExpandedHeight()
	{
		Canvas.ForceUpdateCanvases();
		float num = 15f;
		num += (m_headerElement.transform as RectTransform).rect.height;
		num += (m_detailsElement.transform as RectTransform).rect.height + 12f;
		num += (m_rewardsElement.transform as RectTransform).rect.height;
		return num + (m_abandonElement.transform as RectTransform).rect.height;
	}

	public void SetMouseEventScroll(IScrollHandler handler)
	{
		_MouseEventPasser component = m_singleReward.GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (component == null)
		{
			component = m_singleReward.GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			component.AddNewHandler(handler);
		}
		component = m_twoReward[0].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (component == null)
		{
			component = m_twoReward[0].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			component.AddNewHandler(handler);
		}
		component = m_twoReward[1].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (component == null)
		{
			component = m_twoReward[1].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			component.AddNewHandler(handler);
		}
		component = m_threeReward[0].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (component == null)
		{
			component = m_threeReward[0].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			component.AddNewHandler(handler);
		}
		component = m_threeReward[1].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (component == null)
		{
			component = m_threeReward[1].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			component.AddNewHandler(handler);
		}
		component = m_threeReward[2].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (component == null)
		{
			component = m_threeReward[2].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			component.AddNewHandler(handler);
		}
		for (int i = 0; i < m_Rewards.Length; i++)
		{
			component = m_Rewards[i].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
			if (component == null)
			{
				component = m_Rewards[i].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
				component.AddNewHandler(handler);
			}
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

	private void OnScroll(BaseEventData data)
	{
		if (!m_scrollRectInit)
		{
			Transform transform = base.transform;
			while (m_scrollRect == null)
			{
				if (!(transform != null))
				{
					break;
				}
				m_scrollRect = transform.GetComponent<ScrollRect>();
				transform = transform.parent;
			}
			m_scrollRectInit = true;
		}
		if (!(m_scrollRect != null))
		{
			return;
		}
		while (true)
		{
			m_scrollRect.OnScroll((PointerEventData)data);
			return;
		}
	}

	public bool IsAnimating()
	{
		int result;
		if (m_isExpandingOrContracting <= 0)
		{
			if (!(HitchDetector.Get() == null))
			{
				result = (HitchDetector.Get().IsObjectStaggeringOn(base.gameObject) ? 1 : 0);
				goto IL_004c;
			}
		}
		result = 1;
		goto IL_004c;
		IL_004c:
		return (byte)result != 0;
	}

	protected virtual void DoExpand(bool expanded)
	{
	}

	public void SetExpanded(bool expanded, bool force = false)
	{
		Init();
		if (m_expanded == expanded)
		{
			if (!force)
			{
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
		}
		UpdateDetailsTextHeight();
		DoExpand(expanded);
		m_expanded = expanded;
		m_isExpandingOrContracting = 2;
		m_timeStartChange = Time.time;
		if (m_expanded)
		{
			m_startLocation = m_currentHeight;
			m_endLocation = GetExpandedHeight();
			PlayExpandAnimation();
		}
		else
		{
			m_startLocation = m_currentHeight;
			m_endLocation = m_contractedHeight;
			SetTrashSelected(false);
			PlayContractAnimation();
		}
		for (int i = 0; i < m_downArrows.Length; i++)
		{
			m_downArrows[i].transform.localScale = new Vector3(1f, (!m_expanded) ? 1f : (-1f), 1f);
		}
		while (true)
		{
			m_singleReward.SetSelectable(!m_expanded);
			m_twoReward[0].SetSelectable(!m_expanded);
			m_twoReward[1].SetSelectable(!m_expanded);
			m_threeReward[0].SetSelectable(!m_expanded);
			m_threeReward[1].SetSelectable(!m_expanded);
			m_threeReward[2].SetSelectable(!m_expanded);
			return;
		}
	}

	public void ContractClicked(BaseEventData data)
	{
		if (m_isExpandingOrContracting <= 0)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonsChallengeClickExpand);
			SetExpanded(!m_expanded);
		}
	}

	public void TrashClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonsChallengeTrashcanClick);
		bool trashSelected = !m_TrashBtn.IsSelected();
		SetTrashSelected(trashSelected);
	}

	public virtual void AbandonQuest()
	{
		m_infoReference = null;
		if (!(m_remainingTime != null))
		{
			return;
		}
		while (true)
		{
			m_remainingTime.text = string.Empty;
			return;
		}
	}

	public void AcceptTrashClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonsChallengeTrashcanYes);
		AbandonQuest();
	}

	public void DeclineTrashClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonsChallengeTrashcanNo);
		SetTrashSelected(false);
	}

	protected void SetTrashSelected(bool selected)
	{
		m_TrashBtn.SetSelected(selected, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(m_acceptTrashBtn, selected);
		UIManager.SetGameObjectActive(m_declineTrashBtn, selected);
	}

	private void SetupRewardImages(int numRewards, QuestRewards rewards, int rejectedCount)
	{
		if (numRewards == 1)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (rewards.ItemRewards.Count > 0)
					{
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(rewards.ItemRewards[0].ItemTemplateId);
						m_singleReward.SetupHack(itemTemplate, itemTemplate.IconPath, rewards.ItemRewards[0].Amount);
						m_Rewards[0].SetupHack(itemTemplate, itemTemplate.IconPath, rewards.ItemRewards[0].Amount);
					}
					if (rewards.CurrencyRewards.Count > 0)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								m_singleReward.Setup(rewards.CurrencyRewards[0], rejectedCount);
								m_Rewards[0].Setup(rewards.CurrencyRewards[0], rejectedCount);
								return;
							}
						}
					}
					return;
				}
			}
		}
		switch (numRewards)
		{
		case 2:
			while (true)
			{
				int num2 = 0;
				for (int k = 0; k < rewards.ItemRewards.Count; k++)
				{
					if (num2 < numRewards)
					{
						InventoryItemTemplate itemTemplate3 = InventoryWideData.Get().GetItemTemplate(rewards.ItemRewards[k].ItemTemplateId);
						m_twoReward[num2].SetupHack(itemTemplate3, itemTemplate3.IconPath, rewards.ItemRewards[k].Amount);
						m_Rewards[num2].SetupHack(itemTemplate3, itemTemplate3.IconPath, rewards.ItemRewards[k].Amount);
						num2++;
					}
				}
				while (true)
				{
					for (int l = 0; l < rewards.CurrencyRewards.Count; l++)
					{
						if (num2 < numRewards)
						{
							m_twoReward[num2].Setup(rewards.CurrencyRewards[l], rejectedCount);
							m_Rewards[num2].Setup(rewards.CurrencyRewards[l], rejectedCount);
							num2++;
						}
					}
					while (true)
					{
						switch (5)
						{
						default:
							return;
						case 0:
							break;
						}
					}
				}
			}
		case 3:
		{
			int num = 0;
			for (int i = 0; i < rewards.ItemRewards.Count; i++)
			{
				if (num < numRewards)
				{
					InventoryItemTemplate itemTemplate2 = InventoryWideData.Get().GetItemTemplate(rewards.ItemRewards[i].ItemTemplateId);
					m_threeReward[num].SetupHack(itemTemplate2, itemTemplate2.IconPath, rewards.ItemRewards[i].Amount);
					m_Rewards[num].SetupHack(itemTemplate2, itemTemplate2.IconPath, rewards.ItemRewards[i].Amount);
					num++;
				}
			}
			while (true)
			{
				for (int j = 0; j < rewards.CurrencyRewards.Count; j++)
				{
					if (num < numRewards)
					{
						m_threeReward[num].Setup(rewards.CurrencyRewards[j], rejectedCount);
						m_Rewards[num].Setup(rewards.CurrencyRewards[j], rejectedCount);
						num++;
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
		}
		}
	}

	private void UpdateDetailsTextHeight()
	{
		float num = 0f;
		if (!m_DetailText.text.IsNullOrEmpty())
		{
			num = 7f;
		}
		Vector2 preferredValues = m_DetailText.GetPreferredValues();
		m_detailsElement.minHeight = preferredValues.y + num;
		m_detailsElement.preferredHeight = preferredValues.y + num;
	}

	protected virtual int GetRejectedCount()
	{
		return 0;
	}

	protected void Setup(UIBaseQuestDisplayInfo baseInfo)
	{
		if (m_infoReference != null)
		{
			if (m_infoReference.Equals(baseInfo))
			{
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
		}
		Init();
		QuestComponent questComponent = null;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			questComponent = playerAccountData.QuestComponent;
		}
		m_infoReference = baseInfo;
		if (m_remainingTime != null)
		{
			m_remainingTime.text = string.Empty;
			UIManager.SetGameObjectActive(m_remainingTime, baseInfo.QuestAbandonDate != DateTime.MinValue);
		}
		int index = baseInfo.QuestTemplateRef.Index;
		string text = StringUtil.TR_QuestDescription(index);
		m_QuestDescription.text = text;
		m_ContractText.text = text;
		string text2 = StringUtil.TR_QuestLongDescription(index);
		int num = 0;
		using (List<QuestObjective>.Enumerator enumerator = baseInfo.QuestTemplateRef.Objectives.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestObjective current = enumerator.Current;
				if (current.SuperHidden)
				{
					num++;
					continue;
				}
				int maxCount = current.MaxCount;
				int num2 = 0;
				if (questComponent.Progress.ContainsKey(index))
				{
					if (questComponent.Progress[index].ObjectiveProgress.ContainsKey(num))
					{
						num2 = questComponent.Progress[index].ObjectiveProgress[num];
					}
				}
				else if (questComponent.GetCompletedCount(index) > 0)
				{
					num2 = maxCount;
				}
				string text3 = StringUtil.TR_QuestObjective(index, num + 1);
				if (num2 == 0)
				{
					if (current.Hidden)
					{
						goto IL_02d4;
					}
				}
				if (text3 == string.Empty)
				{
				}
				else
				{
					if (!text2.IsNullOrEmpty())
					{
						text2 += "\n";
					}
					if (maxCount == 1)
					{
						text2 = ((num2 != 1) ? (text2 + $"    {text3}") : (text2 + $"    <color=white>{text3}</color>"));
					}
					else if (num2 == maxCount)
					{
						text2 += $"    <color=white>{text3} ({num2}/{maxCount})</color>";
					}
					else
					{
						text2 += $"    {text3} ({num2}/{maxCount})";
					}
				}
				goto IL_02d4;
				IL_02d4:
				num++;
			}
		}
		if (!questComponent.Progress.ContainsKey(index))
		{
			if (questComponent.GetCompletedCount(index) > 0)
			{
				text2 = string.Empty;
			}
		}
		string text4 = StringUtil.TR_QuestFlavorText(index);
		if (!text4.IsNullOrEmpty())
		{
			if (!text2.IsNullOrEmpty())
			{
				text2 += Environment.NewLine;
			}
			text2 = text2 + "<i>" + text4 + "</i>";
		}
		m_DetailText.text = text2;
		UpdateDetailsTextHeight();
		int num3 = baseInfo.QuestRewardsRef.CurrencyRewards.Count + baseInfo.QuestRewardsRef.ItemRewards.Count + baseInfo.QuestRewardsRef.UnlockRewards.Count;
		if (num3 > 0)
		{
			int rejectedCount = GetRejectedCount();
			num3 = Mathf.Clamp(num3, 0, m_rewardsContainer.Length);
			SetupRewardImages(num3, baseInfo.QuestRewardsRef, rejectedCount);
			for (int i = 0; i < m_rewardsContainer.Length; i++)
			{
				UIManager.SetGameObjectActive(m_rewardsContainer[i], i == num3 - 1);
			}
			for (int j = 0; j < m_Rewards.Length; j++)
			{
				if (j < num3)
				{
					UIManager.SetGameObjectActive(m_Rewards[j], true);
				}
				else
				{
					UIManager.SetGameObjectActive(m_Rewards[j], false);
				}
			}
			UIManager.SetGameObjectActive(m_contractedRewardsContainer, true);
			m_rewardsElement.minHeight = m_rewardsHeight;
			m_rewardsElement.preferredHeight = m_rewardsHeight;
		}
		else
		{
			m_rewardsElement.minHeight = 30f;
			m_rewardsElement.preferredHeight = 30f;
			UIManager.SetGameObjectActive(m_contractedRewardsContainer, false);
			for (int k = 0; k < m_Rewards.Length; k++)
			{
				UIManager.SetGameObjectActive(m_Rewards[k], false);
			}
		}
		int num4 = 0;
		int num5 = 0;
		if (baseInfo.QuestTemplateRef.ObjectiveCountType == RequiredObjectiveCountType.SumObjectiveProgress)
		{
			num5 = QuestItem.GetRequiredObjectiveCount(baseInfo.QuestTemplateRef);
			if (questComponent != null)
			{
				if (questComponent.Progress.ContainsKey(index))
				{
					QuestProgress questProgress = questComponent.Progress[index];
					using (Dictionary<int, int>.Enumerator enumerator2 = questProgress.ObjectiveProgress.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<int, int> current2 = enumerator2.Current;
							int value = current2.Value;
							int key = current2.Key;
							if (baseInfo.QuestTemplateRef.Objectives.Count <= key)
							{
							}
							else if (baseInfo.QuestTemplateRef.Objectives[key].SuperHidden)
							{
							}
							else
							{
								num4 += value;
							}
						}
					}
				}
			}
		}
		else if (baseInfo.QuestTemplateRef.Objectives.Count == 1)
		{
			num5 = baseInfo.QuestTemplateRef.Objectives[0].MaxCount;
			if (questComponent != null)
			{
				if (questComponent.Progress.ContainsKey(index))
				{
					QuestProgress questProgress2 = questComponent.Progress[index];
					if (questProgress2.ObjectiveProgress.ContainsKey(0) && !baseInfo.QuestTemplateRef.Objectives[0].SuperHidden)
					{
						num4 += questProgress2.ObjectiveProgress[0];
					}
				}
			}
		}
		else
		{
			num5 = QuestItem.GetRequiredObjectiveCount(baseInfo.QuestTemplateRef);
			if (questComponent != null)
			{
				if (questComponent.Progress.ContainsKey(index))
				{
					QuestProgress questProgress3 = questComponent.Progress[index];
					using (Dictionary<int, int>.Enumerator enumerator3 = questProgress3.ObjectiveProgress.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							KeyValuePair<int, int> current3 = enumerator3.Current;
							int key2 = current3.Key;
							if (baseInfo.QuestTemplateRef.Objectives.Count > key2)
							{
								if (baseInfo.QuestTemplateRef.Objectives[key2].SuperHidden)
								{
								}
								else if (baseInfo.QuestTemplateRef.Objectives[current3.Key].MaxCount <= current3.Value)
								{
									num4++;
								}
							}
						}
					}
				}
			}
		}
		num4 = Mathf.Min(num4, num5);
		num4 = UpdateCurrentProgressValue(num4, num5, questComponent, index);
		SetProgress(num4, num5, questComponent, index);
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(index);
		_SelectableBtn trashBtn = m_TrashBtn;
		int doActive;
		if (questTemplate != null)
		{
			doActive = ((!questTemplate.CantManuallyAbandon) ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(trashBtn, (byte)doActive != 0);
	}

	protected virtual int UpdateCurrentProgressValue(int currentProgress, int maxProgress, QuestComponent questComponent, int questID)
	{
		return currentProgress;
	}

	protected virtual void SetProgress(int currentProgress, int maxProgress, QuestComponent questComponent, int questID)
	{
		int num = currentProgress;
		if (currentProgress == maxProgress)
		{
			UIManager.SetGameObjectActive(m_progessFilled, false);
			m_progessFilled.fillAmount = 1f;
			num = maxProgress;
		}
		else if (maxProgress > 0)
		{
			UIManager.SetGameObjectActive(m_progessFilled, true);
			m_progessFilled.fillAmount = (float)currentProgress / (float)maxProgress;
		}
		else
		{
			m_progessFilled.fillAmount = 0f;
		}
		string text = $"{num}/{maxProgress}";
		m_progressText.text = text;
	}

	protected virtual void NotifyDoneAnimating()
	{
	}

	private void Update()
	{
		if (IsAnimating())
		{
			if (m_timeStartChange == 0f)
			{
				SetExpanded(m_expanded, true);
			}
			float num = Time.time - m_timeStartChange;
			float num2 = num / m_timeToChangeHeight;
			m_currentHeight = Mathf.Lerp(m_startLocation, m_endLocation, num2);
			m_layoutElement.minHeight = m_currentHeight;
			m_layoutElement.preferredHeight = m_currentHeight;
			if (num2 >= 1f)
			{
				m_isExpandingOrContracting--;
				if (m_isExpandingOrContracting <= 0)
				{
					NotifyDoneAnimating();
				}
			}
		}
		else if (m_layoutElement.minHeight != m_currentHeight)
		{
			m_layoutElement.minHeight = m_currentHeight;
			m_layoutElement.preferredHeight = m_currentHeight;
		}
		if (!(m_remainingTime != null))
		{
			return;
		}
		while (true)
		{
			if (m_infoReference == null || !(m_infoReference.QuestAbandonDate != DateTime.MinValue))
			{
				return;
			}
			TimeSpan timeSpan = m_infoReference.QuestAbandonDate.Subtract(ClientGameManager.Get().PacificNow());
			int num3 = (int)timeSpan.TotalDays;
			string arg;
			if (num3 > 1)
			{
				arg = string.Format(StringUtil.TR("Days", "TimeSpan"), num3);
			}
			else if (num3 == 1)
			{
				arg = StringUtil.TR("Day", "TimeSpan");
			}
			else
			{
				arg = $"{(int)timeSpan.TotalHours:D2}:{timeSpan.Minutes:D2}";
			}
			m_remainingTime.text = string.Format(StringUtil.TR("QuestRemainingTime", "Global"), arg);
			return;
		}
	}
}
