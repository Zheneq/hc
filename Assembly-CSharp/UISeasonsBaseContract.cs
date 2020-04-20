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
		return this.m_expanded;
	}

	public void Awake()
	{
		this.Init();
	}

	protected virtual void Init()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.m_initialized = true;
		if (UISeasonsPanel.Get() != null)
		{
			if (base.transform.IsChildOf(UISeasonsPanel.Get().m_QuestChallengeScrollList.transform))
			{
				_MouseEventPasser mouseEventPasser = this.m_btnHitBox.spriteController.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser.AddNewHandler(UISeasonsPanel.Get().m_QuestChallengeScrollList);
			}
		}
		this.m_btnHitBox.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ContractClicked);
		this.m_TrashBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TrashClicked);
		this.m_acceptTrashBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.AcceptTrashClicked);
		this.m_declineTrashBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.DeclineTrashClicked);
		UIManager.SetGameObjectActive(this.m_acceptTrashBtn, false, null);
		UIManager.SetGameObjectActive(this.m_declineTrashBtn, false, null);
		this.m_currentHeight = this.m_layoutElement.minHeight;
		this.m_contractedHeight = this.m_layoutElement.minHeight;
		this.m_progessFilled.fillAmount = 0f;
		UIManager.SetGameObjectActive(this.m_contractedRewardsContainer, false, null);
		UIManager.SetGameObjectActive(this.m_expandedGroup, true, null);
		this.m_headerElement.flexibleHeight = 0f;
		this.m_detailsElement.flexibleHeight = 0f;
		this.m_rewardsElement.flexibleHeight = 0f;
		this.m_abandonElement.flexibleHeight = 0f;
		this.m_btnHitBox.spriteController.AddSubButton(this.m_TrashBtn.spriteController);
		this.m_btnHitBox.spriteController.AddSubButton(this.m_acceptTrashBtn.spriteController);
		this.m_btnHitBox.spriteController.AddSubButton(this.m_declineTrashBtn.spriteController);
		this.m_btnHitBox.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		for (int i = 0; i < this.m_Rewards.Length; i++)
		{
			_SelectableBtn component = this.m_Rewards[i].GetComponent<_SelectableBtn>();
			if (component != null)
			{
				this.m_btnHitBox.spriteController.AddSubButton(component.spriteController);
				component.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ContractClicked);
			}
		}
		if (this.m_singleReward != null)
		{
			_SelectableBtn component2 = this.m_singleReward.GetComponent<_SelectableBtn>();
			if (component2 != null)
			{
				component2.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ContractClicked);
			}
		}
		if (this.m_twoReward != null)
		{
			for (int j = 0; j < this.m_twoReward.Length; j++)
			{
				_SelectableBtn component3 = this.m_twoReward[j].GetComponent<_SelectableBtn>();
				if (component3 != null)
				{
					component3.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ContractClicked);
				}
			}
		}
		if (this.m_threeReward != null)
		{
			for (int k = 0; k < this.m_threeReward.Length; k++)
			{
				_SelectableBtn component4 = this.m_threeReward[k].GetComponent<_SelectableBtn>();
				if (component4 != null)
				{
					component4.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ContractClicked);
				}
			}
		}
		this.m_acceptTrashBtn.SetSelected(false, false, string.Empty, string.Empty);
		this.m_declineTrashBtn.SetSelected(false, false, string.Empty, string.Empty);
		if (this.m_remainingTime != null)
		{
			UIManager.SetGameObjectActive(this.m_remainingTime, false, null);
		}
		Graphic[] componentsInChildren = this.m_expandedGroup.GetComponentsInChildren<Graphic>(true);
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
		num += (this.m_headerElement.transform as RectTransform).rect.height;
		num += (this.m_detailsElement.transform as RectTransform).rect.height + 12f;
		num += (this.m_rewardsElement.transform as RectTransform).rect.height;
		return num + (this.m_abandonElement.transform as RectTransform).rect.height;
	}

	public void SetMouseEventScroll(IScrollHandler handler)
	{
		_MouseEventPasser mouseEventPasser = this.m_singleReward.GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (mouseEventPasser == null)
		{
			mouseEventPasser = this.m_singleReward.GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(handler);
		}
		mouseEventPasser = this.m_twoReward[0].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (mouseEventPasser == null)
		{
			mouseEventPasser = this.m_twoReward[0].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(handler);
		}
		mouseEventPasser = this.m_twoReward[1].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (mouseEventPasser == null)
		{
			mouseEventPasser = this.m_twoReward[1].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(handler);
		}
		mouseEventPasser = this.m_threeReward[0].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (mouseEventPasser == null)
		{
			mouseEventPasser = this.m_threeReward[0].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(handler);
		}
		mouseEventPasser = this.m_threeReward[1].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (mouseEventPasser == null)
		{
			mouseEventPasser = this.m_threeReward[1].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(handler);
		}
		mouseEventPasser = this.m_threeReward[2].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
		if (mouseEventPasser == null)
		{
			mouseEventPasser = this.m_threeReward[2].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(handler);
		}
		for (int i = 0; i < this.m_Rewards.Length; i++)
		{
			mouseEventPasser = this.m_Rewards[i].GetComponent<_SelectableBtn>().spriteController.gameObject.GetComponent<_MouseEventPasser>();
			if (mouseEventPasser == null)
			{
				mouseEventPasser = this.m_Rewards[i].GetComponent<_SelectableBtn>().spriteController.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser.AddNewHandler(handler);
			}
		}
	}

	private void OnScroll(BaseEventData data)
	{
		if (!this.m_scrollRectInit)
		{
			Transform transform = base.transform;
			while (this.m_scrollRect == null)
			{
				if (!(transform != null))
				{
					break;
				}
				this.m_scrollRect = transform.GetComponent<ScrollRect>();
				transform = transform.parent;
			}
			this.m_scrollRectInit = true;
		}
		if (this.m_scrollRect != null)
		{
			this.m_scrollRect.OnScroll((PointerEventData)data);
		}
	}

	public bool IsAnimating()
	{
		if (this.m_isExpandingOrContracting <= 0)
		{
			if (!(HitchDetector.Get() == null))
			{
				return HitchDetector.Get().IsObjectStaggeringOn(base.gameObject);
			}
		}
		return true;
	}

	protected virtual void DoExpand(bool expanded)
	{
	}

	public void SetExpanded(bool expanded, bool force = false)
	{
		this.Init();
		if (this.m_expanded == expanded)
		{
			if (!force)
			{
				return;
			}
		}
		this.UpdateDetailsTextHeight();
		this.DoExpand(expanded);
		this.m_expanded = expanded;
		this.m_isExpandingOrContracting = 2;
		this.m_timeStartChange = Time.time;
		if (this.m_expanded)
		{
			this.m_startLocation = this.m_currentHeight;
			this.m_endLocation = this.GetExpandedHeight();
			this.PlayExpandAnimation();
		}
		else
		{
			this.m_startLocation = this.m_currentHeight;
			this.m_endLocation = this.m_contractedHeight;
			this.SetTrashSelected(false);
			this.PlayContractAnimation();
		}
		for (int i = 0; i < this.m_downArrows.Length; i++)
		{
			this.m_downArrows[i].transform.localScale = new Vector3(1f, (!this.m_expanded) ? 1f : -1f, 1f);
		}
		this.m_singleReward.SetSelectable(!this.m_expanded);
		this.m_twoReward[0].SetSelectable(!this.m_expanded);
		this.m_twoReward[1].SetSelectable(!this.m_expanded);
		this.m_threeReward[0].SetSelectable(!this.m_expanded);
		this.m_threeReward[1].SetSelectable(!this.m_expanded);
		this.m_threeReward[2].SetSelectable(!this.m_expanded);
	}

	public void ContractClicked(BaseEventData data)
	{
		if (this.m_isExpandingOrContracting > 0)
		{
			return;
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonsChallengeClickExpand);
		this.SetExpanded(!this.m_expanded, false);
	}

	public void TrashClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonsChallengeTrashcanClick);
		bool trashSelected = !this.m_TrashBtn.IsSelected();
		this.SetTrashSelected(trashSelected);
	}

	public virtual void AbandonQuest()
	{
		this.m_infoReference = null;
		if (this.m_remainingTime != null)
		{
			this.m_remainingTime.text = string.Empty;
		}
	}

	public void AcceptTrashClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonsChallengeTrashcanYes);
		this.AbandonQuest();
	}

	public void DeclineTrashClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.SeasonsChallengeTrashcanNo);
		this.SetTrashSelected(false);
	}

	protected void SetTrashSelected(bool selected)
	{
		this.m_TrashBtn.SetSelected(selected, false, string.Empty, string.Empty);
		UIManager.SetGameObjectActive(this.m_acceptTrashBtn, selected, null);
		UIManager.SetGameObjectActive(this.m_declineTrashBtn, selected, null);
	}

	private void SetupRewardImages(int numRewards, QuestRewards rewards, int rejectedCount)
	{
		if (numRewards == 1)
		{
			if (rewards.ItemRewards.Count > 0)
			{
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(rewards.ItemRewards[0].ItemTemplateId);
				this.m_singleReward.SetupHack(itemTemplate, itemTemplate.IconPath, rewards.ItemRewards[0].Amount);
				this.m_Rewards[0].SetupHack(itemTemplate, itemTemplate.IconPath, rewards.ItemRewards[0].Amount);
			}
			if (rewards.CurrencyRewards.Count > 0)
			{
				this.m_singleReward.Setup(rewards.CurrencyRewards[0], rejectedCount);
				this.m_Rewards[0].Setup(rewards.CurrencyRewards[0], rejectedCount);
			}
		}
		else if (numRewards == 2)
		{
			int num = 0;
			for (int i = 0; i < rewards.ItemRewards.Count; i++)
			{
				if (num < numRewards)
				{
					InventoryItemTemplate itemTemplate2 = InventoryWideData.Get().GetItemTemplate(rewards.ItemRewards[i].ItemTemplateId);
					this.m_twoReward[num].SetupHack(itemTemplate2, itemTemplate2.IconPath, rewards.ItemRewards[i].Amount);
					this.m_Rewards[num].SetupHack(itemTemplate2, itemTemplate2.IconPath, rewards.ItemRewards[i].Amount);
					num++;
				}
			}
			for (int j = 0; j < rewards.CurrencyRewards.Count; j++)
			{
				if (num < numRewards)
				{
					this.m_twoReward[num].Setup(rewards.CurrencyRewards[j], rejectedCount);
					this.m_Rewards[num].Setup(rewards.CurrencyRewards[j], rejectedCount);
					num++;
				}
			}
		}
		else if (numRewards == 3)
		{
			int num2 = 0;
			for (int k = 0; k < rewards.ItemRewards.Count; k++)
			{
				if (num2 < numRewards)
				{
					InventoryItemTemplate itemTemplate3 = InventoryWideData.Get().GetItemTemplate(rewards.ItemRewards[k].ItemTemplateId);
					this.m_threeReward[num2].SetupHack(itemTemplate3, itemTemplate3.IconPath, rewards.ItemRewards[k].Amount);
					this.m_Rewards[num2].SetupHack(itemTemplate3, itemTemplate3.IconPath, rewards.ItemRewards[k].Amount);
					num2++;
				}
			}
			for (int l = 0; l < rewards.CurrencyRewards.Count; l++)
			{
				if (num2 < numRewards)
				{
					this.m_threeReward[num2].Setup(rewards.CurrencyRewards[l], rejectedCount);
					this.m_Rewards[num2].Setup(rewards.CurrencyRewards[l], rejectedCount);
					num2++;
				}
			}
		}
	}

	private void UpdateDetailsTextHeight()
	{
		float num = 0f;
		if (!this.m_DetailText.text.IsNullOrEmpty())
		{
			num = 7f;
		}
		Vector2 preferredValues = this.m_DetailText.GetPreferredValues();
		this.m_detailsElement.minHeight = preferredValues.y + num;
		this.m_detailsElement.preferredHeight = preferredValues.y + num;
	}

	protected virtual int GetRejectedCount()
	{
		return 0;
	}

	protected void Setup(UIBaseQuestDisplayInfo baseInfo)
	{
		if (this.m_infoReference != null)
		{
			if (this.m_infoReference.Equals(baseInfo))
			{
				return;
			}
		}
		this.Init();
		QuestComponent questComponent = null;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			questComponent = playerAccountData.QuestComponent;
		}
		this.m_infoReference = baseInfo;
		if (this.m_remainingTime != null)
		{
			this.m_remainingTime.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_remainingTime, baseInfo.QuestAbandonDate != DateTime.MinValue, null);
		}
		int index = baseInfo.QuestTemplateRef.Index;
		string text = StringUtil.TR_QuestDescription(index);
		this.m_QuestDescription.text = text;
		this.m_ContractText.text = text;
		string text2 = StringUtil.TR_QuestLongDescription(index);
		int num = 0;
		using (List<QuestObjective>.Enumerator enumerator = baseInfo.QuestTemplateRef.Objectives.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				QuestObjective questObjective = enumerator.Current;
				if (!questObjective.SuperHidden)
				{
					int maxCount = questObjective.MaxCount;
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
					if (num2 != 0)
					{
						goto IL_1F2;
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
							goto IL_1F2;
						}
					}
					IL_2D4:
					num++;
					continue;
					IL_1F2:
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
							if (num2 == 1)
							{
								text2 += string.Format("    <color=white>{0}</color>", text3);
							}
							else
							{
								text2 += string.Format("    {0}", text3);
							}
						}
						else if (num2 == maxCount)
						{
							text2 += string.Format("    <color=white>{0} ({1}/{2})</color>", text3, num2, maxCount);
						}
						else
						{
							text2 += string.Format("    {0} ({1}/{2})", text3, num2, maxCount);
						}
					}
					goto IL_2D4;
				}
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
		this.m_DetailText.text = text2;
		this.UpdateDetailsTextHeight();
		int num3 = baseInfo.QuestRewardsRef.CurrencyRewards.Count + baseInfo.QuestRewardsRef.ItemRewards.Count + baseInfo.QuestRewardsRef.UnlockRewards.Count;
		if (num3 > 0)
		{
			int rejectedCount = this.GetRejectedCount();
			num3 = Mathf.Clamp(num3, 0, this.m_rewardsContainer.Length);
			this.SetupRewardImages(num3, baseInfo.QuestRewardsRef, rejectedCount);
			for (int i = 0; i < this.m_rewardsContainer.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_rewardsContainer[i], i == num3 - 1, null);
			}
			for (int j = 0; j < this.m_Rewards.Length; j++)
			{
				if (j < num3)
				{
					UIManager.SetGameObjectActive(this.m_Rewards[j], true, null);
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_Rewards[j], false, null);
				}
			}
			UIManager.SetGameObjectActive(this.m_contractedRewardsContainer, true, null);
			this.m_rewardsElement.minHeight = this.m_rewardsHeight;
			this.m_rewardsElement.preferredHeight = this.m_rewardsHeight;
		}
		else
		{
			this.m_rewardsElement.minHeight = 30f;
			this.m_rewardsElement.preferredHeight = 30f;
			UIManager.SetGameObjectActive(this.m_contractedRewardsContainer, false, null);
			for (int k = 0; k < this.m_Rewards.Length; k++)
			{
				UIManager.SetGameObjectActive(this.m_Rewards[k], false, null);
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
							KeyValuePair<int, int> keyValuePair = enumerator2.Current;
							int value = keyValuePair.Value;
							int key = keyValuePair.Key;
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
							KeyValuePair<int, int> keyValuePair2 = enumerator3.Current;
							int key2 = keyValuePair2.Key;
							if (baseInfo.QuestTemplateRef.Objectives.Count > key2)
							{
								if (baseInfo.QuestTemplateRef.Objectives[key2].SuperHidden)
								{
								}
								else if (baseInfo.QuestTemplateRef.Objectives[keyValuePair2.Key].MaxCount <= keyValuePair2.Value)
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
		num4 = this.UpdateCurrentProgressValue(num4, num5, questComponent, index);
		this.SetProgress(num4, num5, questComponent, index);
		QuestTemplate questTemplate = QuestWideData.Get().GetQuestTemplate(index);
		Component trashBtn = this.m_TrashBtn;
		bool doActive;
		if (questTemplate != null)
		{
			doActive = !questTemplate.CantManuallyAbandon;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(trashBtn, doActive, null);
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
			UIManager.SetGameObjectActive(this.m_progessFilled, false, null);
			this.m_progessFilled.fillAmount = 1f;
			num = maxProgress;
		}
		else if (maxProgress > 0)
		{
			UIManager.SetGameObjectActive(this.m_progessFilled, true, null);
			this.m_progessFilled.fillAmount = (float)currentProgress / (float)maxProgress;
		}
		else
		{
			this.m_progessFilled.fillAmount = 0f;
		}
		string text = string.Format("{0}/{1}", num, maxProgress);
		this.m_progressText.text = text;
	}

	protected virtual void NotifyDoneAnimating()
	{
	}

	private void Update()
	{
		if (this.IsAnimating())
		{
			if (this.m_timeStartChange == 0f)
			{
				this.SetExpanded(this.m_expanded, true);
			}
			float num = Time.time - this.m_timeStartChange;
			float num2 = num / this.m_timeToChangeHeight;
			this.m_currentHeight = Mathf.Lerp(this.m_startLocation, this.m_endLocation, num2);
			this.m_layoutElement.minHeight = this.m_currentHeight;
			this.m_layoutElement.preferredHeight = this.m_currentHeight;
			if (num2 >= 1f)
			{
				this.m_isExpandingOrContracting--;
				if (this.m_isExpandingOrContracting <= 0)
				{
					this.NotifyDoneAnimating();
				}
			}
		}
		else if (this.m_layoutElement.minHeight != this.m_currentHeight)
		{
			this.m_layoutElement.minHeight = this.m_currentHeight;
			this.m_layoutElement.preferredHeight = this.m_currentHeight;
		}
		if (this.m_remainingTime != null)
		{
			if (this.m_infoReference != null && this.m_infoReference.QuestAbandonDate != DateTime.MinValue)
			{
				TimeSpan timeSpan = this.m_infoReference.QuestAbandonDate.Subtract(ClientGameManager.Get().PacificNow());
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
					arg = string.Format("{0:D2}:{1:D2}", (int)timeSpan.TotalHours, timeSpan.Minutes);
				}
				this.m_remainingTime.text = string.Format(StringUtil.TR("QuestRemainingTime", "Global"), arg);
			}
		}
	}
}
