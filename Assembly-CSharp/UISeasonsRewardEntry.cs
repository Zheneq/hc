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
		if (!(componentInParent != null))
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
			_MouseEventPasser mouseEventPasser = m_hitBox.AddComponent<_MouseEventPasser>();
			mouseEventPasser.AddNewHandler(componentInParent);
			for (int i = 0; i < m_Rewards.Length; i++)
			{
				_MouseEventPasser mouseEventPasser2 = m_Rewards[i].m_hitbox.gameObject.AddComponent<_MouseEventPasser>();
				mouseEventPasser2.AddNewHandler(componentInParent);
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
	}

	private void Update()
	{
		if (m_setupRewardList)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			DelaySetupReward(m_rewardList);
			m_setupRewardList = false;
		}
		if (m_setStateOfLevelup <= -1)
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
			AnimatorClipInfo[] currentAnimatorClipInfo = m_seasonRewardAnimator.GetCurrentAnimatorClipInfo(1);
			if (m_setStateOfLevelup == 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						if (currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip != null)
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
							if (currentAnimatorClipInfo[0].clip.name != "SeasonLevelDisableIDLE")
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
								m_seasonRewardAnimator.Play("SeasonLevelDisableIDLE", 1);
							}
						}
						UISeasonsRewardIcon[] rewards = m_Rewards;
						foreach (UISeasonsRewardIcon uISeasonsRewardIcon in rewards)
						{
							uISeasonsRewardIcon.PlayLevelUp("SeasonRewardItemDisabledIDLE");
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
					}
				}
			}
			if (m_setStateOfLevelup == 1)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
					{
						if (currentAnimatorClipInfo.Length > 0 && currentAnimatorClipInfo[0].clip != null)
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
							if (currentAnimatorClipInfo[0].clip.name != "SeasonLevelDefaultIDLE")
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
								m_seasonRewardAnimator.Play("SeasonLevelDefaultIDLE", 1);
							}
						}
						UISeasonsRewardIcon[] rewards2 = m_Rewards;
						foreach (UISeasonsRewardIcon uISeasonsRewardIcon2 in rewards2)
						{
							uISeasonsRewardIcon2.PlayLevelUp("SeasonRewardItemDefaultIDLE");
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
					}
				}
			}
			if (m_setStateOfLevelup != 2 || currentAnimatorClipInfo.Length <= 0)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (!(currentAnimatorClipInfo[0].clip != null))
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
					if (!(currentAnimatorClipInfo[0].clip.name != "SeasonLevelDefaultIN"))
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
						if (!m_playingLevelUp)
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
							if (m_startPlayingLevelUp)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
									{
										m_canvasGroup.alpha = 1f;
										m_startPlayingLevelUp = false;
										m_seasonRewardAnimator.Play("SeasonLevelDefaultIN", 1, 0f);
										UISeasonsRewardIcon[] rewards3 = m_Rewards;
										foreach (UISeasonsRewardIcon uISeasonsRewardIcon3 in rewards3)
										{
											uISeasonsRewardIcon3.PlayLevelUp("SeasonRewardItemDefaultIN");
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
									}
								}
							}
							if (currentAnimatorClipInfo[0].clip.name == "SeasonLevelDefaultIDLE")
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									m_setStateOfLevelup = 1;
									m_playingLevelUp = false;
									return;
								}
							}
							return;
						}
					}
				}
			}
		}
	}

	public bool IsPlayingLevelUp()
	{
		int result;
		if (!m_playingLevelUp)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (m_startPlayingLevelUp ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void SetAsNotLevelled()
	{
		if (!m_playingLevelUp)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_setStateOfLevelup = 0;
		}
		m_isAlreadyAwarded = false;
	}

	public void SetAsLevelledup()
	{
		m_setStateOfLevelup = 1;
		m_isAlreadyAwarded = true;
	}

	public void DoLevelUp()
	{
		m_playingLevelUp = true;
		m_startPlayingLevelUp = true;
		m_setStateOfLevelup = 2;
		m_isAlreadyAwarded = true;
	}

	private void DelaySetupReward(List<UISeasonRewardDisplayInfo> rewardList)
	{
		m_btn.spriteController.ForceSetPointerEntered(false);
		int num = 0;
		for (int i = 0; i < m_Rewards.Length; i++)
		{
			UISeasonsRewardIcon uISeasonsRewardIcon = m_Rewards[i];
			SeasonReward seasonReward = null;
			while (num < rewardList.Count)
			{
				seasonReward = rewardList[num].GetReward();
				bool flag = true;
				if (seasonReward is SeasonItemReward)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!m_isAlreadyAwarded)
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
						SeasonItemReward seasonItemReward = seasonReward as SeasonItemReward;
						if (!seasonItemReward.Conditions.IsNullOrEmpty())
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
							flag = QuestWideData.AreConditionsMet(seasonItemReward.Conditions, seasonItemReward.LogicStatement, true);
						}
					}
				}
				num++;
				if (flag)
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
					break;
				}
				seasonReward = null;
			}
			if (seasonReward != null)
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
				UIManager.SetGameObjectActive(uISeasonsRewardIcon, true);
				if (seasonReward is SeasonItemReward)
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
					QuestItemReward itemReward = (seasonReward as SeasonItemReward).ItemReward;
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemReward.ItemTemplateId);
					uISeasonsRewardIcon.Setup(itemTemplate, new InventoryItem(itemReward.ItemTemplateId, itemReward.Amount));
					UIManager.SetGameObjectActive(uISeasonsRewardIcon.m_ownedCheckmark, InventoryWideData.IsOwned(itemTemplate));
				}
				else if (seasonReward is SeasonCurrencyReward)
				{
					QuestCurrencyReward currencyReward = (seasonReward as SeasonCurrencyReward).CurrencyReward;
					InventoryItemTemplate itemTemplate2 = (seasonReward as SeasonCurrencyReward).GetItemTemplate();
					uISeasonsRewardIcon.Setup(itemTemplate2, new InventoryItem(-1, currencyReward.Amount));
					UIManager.SetGameObjectActive(uISeasonsRewardIcon.m_ownedCheckmark, InventoryWideData.IsOwned(itemTemplate2));
				}
				else
				{
					if (!(seasonReward is SeasonUnlockReward))
					{
						continue;
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
					QuestUnlockReward unlockReward = (seasonReward as SeasonUnlockReward).UnlockReward;
					InventoryItemTemplate inventoryItemTemplate = new InventoryItemTemplate();
					GameBalanceVars.PlayerUnlockable playerUnlockable;
					switch (unlockReward.purchaseType)
					{
					default:
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							throw new NotImplementedException(string.Concat(unlockReward.purchaseType, " not implement"));
						}
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
					case PurchaseType.ChatEmoji:
					{
						inventoryItemTemplate.Type = InventoryItemType.ChatEmoji;
						inventoryItemTemplate.TypeSpecificData = unlockReward.typeSpecificData;
						GameBalanceVars.ChatEmoticon chatEmoji = GameWideData.Get().m_gameBalanceVars.GetChatEmoji(unlockReward.typeSpecificData[0]);
						inventoryItemTemplate.DisplayName = chatEmoji.GetEmojiName();
						playerUnlockable = chatEmoji;
						break;
					}
					case PurchaseType.Overcon:
					{
						inventoryItemTemplate.Type = InventoryItemType.Overcon;
						inventoryItemTemplate.TypeSpecificData = unlockReward.typeSpecificData;
						GameBalanceVars.OverconUnlockData overconUnlockData = null;
						int num2 = 0;
						while (true)
						{
							if (num2 < GameWideData.Get().m_gameBalanceVars.Overcons.Length)
							{
								if (GameWideData.Get().m_gameBalanceVars.Overcons[num2].ID == unlockReward.typeSpecificData[0])
								{
									overconUnlockData = GameWideData.Get().m_gameBalanceVars.Overcons[num2];
									break;
								}
								num2++;
								continue;
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
							break;
						}
						inventoryItemTemplate.DisplayName = overconUnlockData.GetOverconName();
						playerUnlockable = overconUnlockData;
						break;
					}
					}
					inventoryItemTemplate.Rarity = playerUnlockable.Rarity;
					inventoryItemTemplate.Enabled = true;
					if (!unlockReward.resourceString.IsNullOrEmpty())
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
						inventoryItemTemplate.IconPath = unlockReward.resourceString;
					}
					uISeasonsRewardIcon.Setup(inventoryItemTemplate, null);
					UIManager.SetGameObjectActive(uISeasonsRewardIcon.m_ownedCheckmark, m_setStateOfLevelup > 0);
				}
			}
			else
			{
				UIManager.SetGameObjectActive(uISeasonsRewardIcon, false);
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			for (int j = 0; j < m_repeatingRewards.Length; j++)
			{
				if (j < m_repeatingRewardList.Count)
				{
					UIManager.SetGameObjectActive(m_repeatingRewardContainers[j], true);
					m_repeatingRewards[j].sprite = m_repeatingRewardList[j].GetDisplaySprite();
				}
				else
				{
					UIManager.SetGameObjectActive(m_repeatingRewardContainers[j], false);
					m_repeatingRewards[j].sprite = null;
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

	public void SetPreviewingLevel(bool isPreviewing)
	{
		if (!(m_previewContainer != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_previewContainer, isPreviewing);
			return;
		}
	}

	public void DisplayAsCurrentLevel(bool doDisplay, bool forceAnim)
	{
		m_btn.SetSelected(doDisplay, forceAnim, string.Empty, string.Empty);
	}

	public void DoRewardIconFade(bool doFade)
	{
		CanvasGroup rewardContainer = m_RewardContainer;
		float alpha;
		if (doFade)
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
		m_rewardList = rewardList;
		m_repeatingRewardList = repeatingRewards;
		m_setupRewardList = true;
	}

	public void SetLevelLabelsText(string text)
	{
		for (int i = 0; i < m_levelLabels.Length; i++)
		{
			m_levelLabels[i].text = text;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	public void RefreshDisplay()
	{
		if (m_rewardList == null)
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
			DelaySetupReward(m_rewardList);
			return;
		}
	}

	public float GetHeight()
	{
		Vector2 sizeDelta = (base.gameObject.transform as RectTransform).sizeDelta;
		return sizeDelta.y;
	}
}
