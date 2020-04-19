using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonsRewardEntry : MonoBehaviour, IUIDataEntry
{
	public Animator m_seasonRewardAnimator;

	public TextMeshProUGUI[] m_levelLabels;

	public LayoutElement[] m_repeatingRewardContainers;

	public Image[] m_repeatingRewards;

	public UISeasonsRewardIcon[] m_Rewards;

	public CanvasGroup m_canvasGroup;

	public CanvasGroup m_RewardContainer;

	public RectTransform m_previewContainer;

	public _SelectableBtn m_btn;

	public GameObject m_hitBox;

	private bool m_setupRewardList;

	private List<UISeasonRewardDisplayInfo> m_rewardList;

	private List<UISeasonRepeatingRewardInfo> m_repeatingRewardList;

	private int m_setStateOfLevelup;

	private bool m_playingLevelUp;

	private bool m_startPlayingLevelUp;

	private bool m_isAlreadyAwarded;

	private void Start()
	{
		ScrollRect componentInParent = base.gameObject.GetComponentInParent<ScrollRect>();
		if (componentInParent != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.Start()).MethodHandle;
			}
			_MouseEventPasser mouseEventPasser = this.m_hitBox.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(componentInParent);
			for (int i = 0; i < this.m_Rewards.Length; i++)
			{
				_MouseEventPasser mouseEventPasser2 = this.m_Rewards[i].m_hitbox.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser2.AddNewHandler(componentInParent);
			}
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
	}

	private void Update()
	{
		if (this.m_setupRewardList)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.Update()).MethodHandle;
			}
			this.DelaySetupReward(this.m_rewardList);
			this.m_setupRewardList = false;
		}
		if (this.m_setStateOfLevelup > -1)
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
			AnimatorClipInfo[] currentAnimatorClipInfo = this.m_seasonRewardAnimator.GetCurrentAnimatorClipInfo(1);
			if (this.m_setStateOfLevelup == 0)
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
				if (currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip != null)
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
					if (currentAnimatorClipInfo[0].clip.name != "SeasonLevelDisableIDLE")
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
						this.m_seasonRewardAnimator.Play("SeasonLevelDisableIDLE", 1);
					}
				}
				foreach (UISeasonsRewardIcon uiseasonsRewardIcon in this.m_Rewards)
				{
					uiseasonsRewardIcon.PlayLevelUp("SeasonRewardItemDisabledIDLE");
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
			else if (this.m_setStateOfLevelup == 1)
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
				if (currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip != null)
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
					if (currentAnimatorClipInfo[0].clip.name != "SeasonLevelDefaultIDLE")
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
						this.m_seasonRewardAnimator.Play("SeasonLevelDefaultIDLE", 1);
					}
				}
				foreach (UISeasonsRewardIcon uiseasonsRewardIcon2 in this.m_Rewards)
				{
					uiseasonsRewardIcon2.PlayLevelUp("SeasonRewardItemDefaultIDLE");
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
			else if (this.m_setStateOfLevelup == 2 && currentAnimatorClipInfo.Length > 0)
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
				if (currentAnimatorClipInfo[0].clip != null)
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
					if (currentAnimatorClipInfo[0].clip.name != "SeasonLevelDefaultIN")
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
						if (this.m_playingLevelUp)
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
							if (this.m_startPlayingLevelUp)
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
								this.m_canvasGroup.alpha = 1f;
								this.m_startPlayingLevelUp = false;
								this.m_seasonRewardAnimator.Play("SeasonLevelDefaultIN", 1, 0f);
								foreach (UISeasonsRewardIcon uiseasonsRewardIcon3 in this.m_Rewards)
								{
									uiseasonsRewardIcon3.PlayLevelUp("SeasonRewardItemDefaultIN");
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
							else if (currentAnimatorClipInfo[0].clip.name == "SeasonLevelDefaultIDLE")
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
								this.m_setStateOfLevelup = 1;
								this.m_playingLevelUp = false;
							}
						}
					}
				}
			}
		}
	}

	public bool IsPlayingLevelUp()
	{
		bool result;
		if (!this.m_playingLevelUp)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.IsPlayingLevelUp()).MethodHandle;
			}
			result = this.m_startPlayingLevelUp;
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void SetAsNotLevelled()
	{
		if (!this.m_playingLevelUp)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.SetAsNotLevelled()).MethodHandle;
			}
			this.m_setStateOfLevelup = 0;
		}
		this.m_isAlreadyAwarded = false;
	}

	public void SetAsLevelledup()
	{
		this.m_setStateOfLevelup = 1;
		this.m_isAlreadyAwarded = true;
	}

	public void DoLevelUp()
	{
		this.m_playingLevelUp = true;
		this.m_startPlayingLevelUp = true;
		this.m_setStateOfLevelup = 2;
		this.m_isAlreadyAwarded = true;
	}

	private void DelaySetupReward(List<UISeasonRewardDisplayInfo> rewardList)
	{
		this.m_btn.spriteController.ForceSetPointerEntered(false);
		int i = 0;
		for (int j = 0; j < this.m_Rewards.Length; j++)
		{
			UISeasonsRewardIcon uiseasonsRewardIcon = this.m_Rewards[j];
			SeasonReward seasonReward = null;
			while (i < rewardList.Count)
			{
				seasonReward = rewardList[i].GetReward();
				bool flag = true;
				if (seasonReward is SeasonItemReward)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.DelaySetupReward(List<UISeasonRewardDisplayInfo>)).MethodHandle;
					}
					if (!this.m_isAlreadyAwarded)
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
						SeasonItemReward seasonItemReward = seasonReward as SeasonItemReward;
						if (!seasonItemReward.Conditions.IsNullOrEmpty<QuestCondition>())
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
							flag = QuestWideData.AreConditionsMet(seasonItemReward.Conditions, seasonItemReward.LogicStatement, true);
						}
					}
				}
				i++;
				if (flag)
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
					break;
				}
				seasonReward = null;
			}
			if (seasonReward != null)
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
				UIManager.SetGameObjectActive(uiseasonsRewardIcon, true, null);
				if (seasonReward is SeasonItemReward)
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
					QuestItemReward itemReward = (seasonReward as SeasonItemReward).ItemReward;
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemReward.ItemTemplateId);
					uiseasonsRewardIcon.Setup(itemTemplate, new InventoryItem(itemReward.ItemTemplateId, itemReward.Amount, 0));
					UIManager.SetGameObjectActive(uiseasonsRewardIcon.m_ownedCheckmark, InventoryWideData.IsOwned(itemTemplate), null);
				}
				else if (seasonReward is SeasonCurrencyReward)
				{
					QuestCurrencyReward currencyReward = (seasonReward as SeasonCurrencyReward).CurrencyReward;
					InventoryItemTemplate itemTemplate2 = (seasonReward as SeasonCurrencyReward).GetItemTemplate();
					uiseasonsRewardIcon.Setup(itemTemplate2, new InventoryItem(-1, currencyReward.Amount, 0));
					UIManager.SetGameObjectActive(uiseasonsRewardIcon.m_ownedCheckmark, InventoryWideData.IsOwned(itemTemplate2), null);
				}
				else if (seasonReward is SeasonUnlockReward)
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
					QuestUnlockReward unlockReward = (seasonReward as SeasonUnlockReward).UnlockReward;
					InventoryItemTemplate inventoryItemTemplate = new InventoryItemTemplate();
					PurchaseType purchaseType = unlockReward.purchaseType;
					GameBalanceVars.PlayerUnlockable playerUnlockable;
					switch (purchaseType)
					{
					case PurchaseType.BannerBackground:
					case PurchaseType.BannerForeground:
					{
						inventoryItemTemplate.Type = InventoryItemType.BannerID;
						inventoryItemTemplate.TypeSpecificData = unlockReward.typeSpecificData;
						GameBalanceVars.PlayerBanner banner = GameWideData.Get().m_gameBalanceVars.GetBanner(unlockReward.typeSpecificData[0]);
						inventoryItemTemplate.DisplayName = banner.GetBannerName();
						playerUnlockable = banner;
						break;
					}
					default:
					{
						if (purchaseType != PurchaseType.Overcon)
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
							throw new NotImplementedException(unlockReward.purchaseType + " not implement");
						}
						inventoryItemTemplate.Type = InventoryItemType.Overcon;
						inventoryItemTemplate.TypeSpecificData = unlockReward.typeSpecificData;
						GameBalanceVars.OverconUnlockData overconUnlockData = null;
						for (int k = 0; k < GameWideData.Get().m_gameBalanceVars.Overcons.Length; k++)
						{
							if (GameWideData.Get().m_gameBalanceVars.Overcons[k].ID == unlockReward.typeSpecificData[0])
							{
								overconUnlockData = GameWideData.Get().m_gameBalanceVars.Overcons[k];
								IL_319:
								inventoryItemTemplate.DisplayName = overconUnlockData.GetOverconName();
								playerUnlockable = overconUnlockData;
								goto IL_349;
							}
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							goto IL_319;
						}
						break;
					}
					case PurchaseType.ChatEmoji:
					{
						inventoryItemTemplate.Type = InventoryItemType.ChatEmoji;
						inventoryItemTemplate.TypeSpecificData = unlockReward.typeSpecificData;
						GameBalanceVars.ChatEmoticon chatEmoji = GameWideData.Get().m_gameBalanceVars.GetChatEmoji(unlockReward.typeSpecificData[0]);
						inventoryItemTemplate.DisplayName = chatEmoji.GetEmojiName();
						playerUnlockable = chatEmoji;
						break;
					}
					}
					IL_349:
					inventoryItemTemplate.Rarity = playerUnlockable.Rarity;
					inventoryItemTemplate.Enabled = true;
					if (!unlockReward.resourceString.IsNullOrEmpty())
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
						inventoryItemTemplate.IconPath = unlockReward.resourceString;
					}
					uiseasonsRewardIcon.Setup(inventoryItemTemplate, null);
					UIManager.SetGameObjectActive(uiseasonsRewardIcon.m_ownedCheckmark, this.m_setStateOfLevelup > 0, null);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(uiseasonsRewardIcon, false, null);
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int l = 0; l < this.m_repeatingRewards.Length; l++)
		{
			if (l < this.m_repeatingRewardList.Count)
			{
				UIManager.SetGameObjectActive(this.m_repeatingRewardContainers[l], true, null);
				this.m_repeatingRewards[l].sprite = this.m_repeatingRewardList[l].GetDisplaySprite();
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_repeatingRewardContainers[l], false, null);
				this.m_repeatingRewards[l].sprite = null;
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

	public void SetPreviewingLevel(bool isPreviewing)
	{
		if (this.m_previewContainer != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.SetPreviewingLevel(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_previewContainer, isPreviewing, null);
		}
	}

	public void DisplayAsCurrentLevel(bool doDisplay, bool forceAnim)
	{
		this.m_btn.SetSelected(doDisplay, forceAnim, string.Empty, string.Empty);
	}

	public void DoRewardIconFade(bool doFade)
	{
		CanvasGroup rewardContainer = this.m_RewardContainer;
		float alpha;
		if (doFade)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.DoRewardIconFade(bool)).MethodHandle;
			}
			alpha = 0.5f;
		}
		else
		{
			alpha = 1f;
		}
		rewardContainer.alpha = alpha;
	}

	public void SetupReward(List<UISeasonRewardDisplayInfo> rewardList, List<UISeasonRepeatingRewardInfo> repeatingRewards)
	{
		this.m_rewardList = rewardList;
		this.m_repeatingRewardList = repeatingRewards;
		this.m_setupRewardList = true;
	}

	public void SetLevelLabelsText(string text)
	{
		for (int i = 0; i < this.m_levelLabels.Length; i++)
		{
			this.m_levelLabels[i].text = text;
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.SetLevelLabelsText(string)).MethodHandle;
		}
	}

	public void RefreshDisplay()
	{
		if (this.m_rewardList != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonsRewardEntry.RefreshDisplay()).MethodHandle;
			}
			this.DelaySetupReward(this.m_rewardList);
		}
	}

	public float GetHeight()
	{
		return (base.gameObject.transform as RectTransform).sizeDelta.y;
	}
}
