using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerNavPanel : MonoBehaviour
{
	public _ButtonSwapSprite m_buttonHitBox;

	public _SelectableBtn m_friendMenuToggleBtn;

	public CanvasGroup m_canvasGroup;

	public TextMeshProUGUI m_playerName;

	public Image m_playerImage;

	public TextMeshProUGUI m_playerTitle;

	public Image m_questLevelSlider;

	public Image m_levelSlider;

	public TextMeshProUGUI m_levelText;

	public Animator m_levelUpAnimator;

	public RectTransform m_pressedContainer;

	public RectTransform m_hoverContainer;

	public RectTransform m_newFriendNotificationContainer;

	public _ButtonSwapSprite m_newFriendNotificationBtn;

	public TextMeshProUGUI m_numOnlineFriends;

	public UINavPanelPartyMember m_selfPartyDisplay;

	public UINavPanelPartyMember[] m_partyMemberDisplay;

	public Animator m_GGPackAnimator;

	public Button ggbuttonHitBox;

	public TextMeshProUGUI m_GGPackCount;

	public Animator m_friendAnimator;

	public UITooltipHoverObject friendTooltipObj;

	public Image m_friendBonusOn;

	public RectTransform m_factionContainer;

	public UITooltipHoverObject factionTooltipObj;

	public Image m_factionIcon;

	public Animator m_expBonusAnimator;

	public Button expBonusButtonHitBox;

	public TextMeshProUGUI m_expBonusAmount;

	public Animator m_factionNotification;

	public Image m_factionNotificationGlow;

	public Image m_factionNotificationIcon;

	public TextMeshProUGUI m_factionNotificationText;

	public RectTransform m_container;

	[HideInInspector]
	public string m_playerHandle;

	public RectTransform m_normalLevelContainer;

	public HorizontalLayoutGroup m_tutorialLevelContainer;

	public UITutorialSeasonLevelBar m_tutorialLevelBarPrefab;

	public _ButtonSwapSprite m_levelNumberHitbox;

	private static UIPlayerNavPanel s_instance;

	private const float kSecondsToUpdateExperience = 1f;

	private int m_expPerSecond;

	private int m_curLevel = -1;

	private int m_endLevel = -1;

	private int m_curExp = -1;

	private int m_endExp = -1;

	private int m_expToLevel = -1;

	private string m_currentFactionName;

	private string m_levelTooltipString;

	private bool m_isPressed;

	private bool m_isHover;

	private bool m_receivedDataOnce;

	private float m_failedToGetData;

	private bool m_isVisible;

	private TrustBoostUsedNotification m_pendingTrustNotification;

	private Action<string> OnPlayerTitleChange;

	private Action<CurrencyData> OnBankBalanceChange;

	private Action<FactionCompetitionNotification> OnFactionCompetitionNotification;

	private Action<GameBalanceVars.PlayerRibbon> OnPlayerRibbonChange;

	private DateTime m_freelancerExpBonusTime;

	private bool m_groupedWithFriend;

	private int m_lastFriendCount;

	public static UIPlayerNavPanel Get()
	{
		return UIPlayerNavPanel.s_instance;
	}

	private void Awake()
	{
		UIPlayerNavPanel.s_instance = this;
	}

	public void Start()
	{
		this.m_playerName.text = string.Empty;
		this.m_levelText.text = string.Empty;
		this.m_playerTitle.text = string.Empty;
		this.m_levelTooltipString = string.Empty;
		if (this.m_buttonHitBox != null)
		{
			this.m_buttonHitBox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnProfileClicked);
			this.m_buttonHitBox.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.OnProfileEnter);
			this.m_buttonHitBox.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.OnProfileExit);
			UIEventTriggerUtils.AddListener(this.m_buttonHitBox.gameObject, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.OnProfileDown));
			UITooltipObject component = this.m_buttonHitBox.GetComponent<UITooltipClickObject>();
			TooltipType tooltipType = TooltipType.PlayerGroupMenu;
			if (UIPlayerNavPanel.f__am_cache0 == null)
			{
				UIPlayerNavPanel.f__am_cache0 = delegate(UITooltipBase tooltip)
				{
					if (ClientGameManager.Get().GroupInfo != null)
					{
						if (ClientGameManager.Get().GroupInfo.Members != null)
						{
							List<UpdateGroupMemberData> members = ClientGameManager.Get().GroupInfo.Members;
							for (int i = 0; i < members.Count; i++)
							{
								if (members[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
								{
									UIPlayerPanelGroupMenu uiplayerPanelGroupMenu = tooltip as UIPlayerPanelGroupMenu;
									uiplayerPanelGroupMenu.Setup(members[i]);
									return true;
								}
							}
						}
					}
					return false;
				};
			}
			component.Setup(tooltipType, UIPlayerNavPanel.f__am_cache0, null);
		}
		if (this.ggbuttonHitBox != null)
		{
			UIEventTriggerUtils.AddListener(this.ggbuttonHitBox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnGGButtonClick));
			this.ggbuttonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.GGTooltipSetup), null);
		}
		if (this.expBonusButtonHitBox != null)
		{
			UIEventTriggerUtils.AddListener(this.expBonusButtonHitBox.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnExpBonusButtonClick));
			this.expBonusButtonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.ExpBonusTooltipSetup), null);
		}
		this.m_expBonusAmount.raycastTarget = false;
		if (this.friendTooltipObj != null)
		{
			this.friendTooltipObj.Setup(TooltipType.Titled, new TooltipPopulateCall(this.FriendTooltipSetup), null);
		}
		if (this.factionTooltipObj != null)
		{
			this.factionTooltipObj.Setup(TooltipType.Titled, new TooltipPopulateCall(this.FactionTooltipSetup), null);
			this.m_currentFactionName = StringUtil.TR("FactionNone", "Global");
			UIManager.SetGameObjectActive(this.m_factionIcon, false, null);
		}
		this.m_friendMenuToggleBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnFriendListMenuBtnClicked);
		this.m_newFriendNotificationBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnFriendListMenuBtnClicked);
		ClientGameManager clientGameManager = ClientGameManager.Get();
		this.OnPlayerTitleChange = delegate(string newTitle)
		{
			this.m_playerTitle.text = newTitle;
			UIManager.SetGameObjectActive(this.m_playerTitle, true, null);
		};
		clientGameManager.OnPlayerTitleChange += this.OnPlayerTitleChange;
		this.OnBankBalanceChange = delegate(CurrencyData currencyData)
		{
			if (currencyData.Type == CurrencyType.GGPack)
			{
				if (currencyData.m_Amount > 0)
				{
					this.m_GGPackAnimator.Play("BonusIconDefaultIN");
				}
				else
				{
					this.m_GGPackAnimator.Play("BonusIconDefaultOUT");
				}
			}
			int currentAmount2 = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
			this.m_GGPackCount.text = string.Format("x{0}", currentAmount2);
			this.m_GGPackCount.raycastTarget = false;
			if (currentAmount2 > 0)
			{
				UINewUserFlowManager.OnGGBoostOwned();
			}
		};
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		this.m_GGPackCount.text = string.Format("x{0}", currentAmount);
		this.m_GGPackCount.raycastTarget = false;
		if (currentAmount > 0)
		{
			UINewUserFlowManager.OnGGBoostOwned();
		}
		this.OnFactionCompetitionNotification = delegate(FactionCompetitionNotification notification)
		{
			this.UpdateFactionIcon();
		};
		this.OnPlayerRibbonChange = delegate(GameBalanceVars.PlayerRibbon ribbon)
		{
			this.UpdateFactionIcon();
		};
		clientGameManager.OnBankBalanceChange += this.OnBankBalanceChange;
		clientGameManager.OnAccountDataUpdated += this.OnAccountDataUpdated;
		clientGameManager.OnFactionCompetitionNotification += this.OnFactionCompetitionNotification;
		clientGameManager.OnPlayerRibbonChange += this.OnPlayerRibbonChange;
		clientGameManager.OnTrustBoostUsedNotification += this.OnTrustBoostUsedNotification;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			this.OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		if (ClientGameManager.Get().GroupInfo != null)
		{
			if (ClientGameManager.Get().GroupInfo.Members != null)
			{
				this.NotifyGroupUpdate(ClientGameManager.Get().GroupInfo.Members);
			}
		}
		if (FriendListPanel.Get() != null)
		{
			FriendListPanel.Get().Init();
		}
		this.UpdateFactionIcon();
		this.UpdateExpBonusIcon();
		if (this.m_levelNumberHitbox != null)
		{
			this.m_buttonHitBox.AddSubButton(this.m_levelNumberHitbox);
			this.m_levelNumberHitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnProfileClicked);
			this.m_levelNumberHitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				if (this.m_levelTooltipString.IsNullOrEmpty())
				{
					return false;
				}
				(tooltip as UISimpleTooltip).Setup(this.m_levelTooltipString);
				return true;
			}, null);
		}
	}

	public void NotifyQuestCompleted(QuestCompleteData data)
	{
		UIManager.SetGameObjectActive(this.m_questLevelSlider, true, null);
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
		{
			return;
		}
		clientGameManager.OnPlayerTitleChange -= this.OnPlayerTitleChange;
		clientGameManager.OnBankBalanceChange -= this.OnBankBalanceChange;
		clientGameManager.OnAccountDataUpdated -= this.OnAccountDataUpdated;
		clientGameManager.OnFactionCompetitionNotification -= this.OnFactionCompetitionNotification;
		clientGameManager.OnPlayerRibbonChange -= this.OnPlayerRibbonChange;
		clientGameManager.OnTrustBoostUsedNotification -= this.OnTrustBoostUsedNotification;
	}

	public void NotifyGroupUpdate(List<UpdateGroupMemberData> groupMembers)
	{
		if (groupMembers.Count == 0)
		{
			UIManager.SetGameObjectActive(this.m_selfPartyDisplay.m_PartyLeaderIconAnimator, false, null);
			UIManager.SetGameObjectActive(this.m_selfPartyDisplay.m_ReadyIconAnimator, false, null);
			UIManager.SetGameObjectActive(this.m_selfPartyDisplay.m_IsInGameAnimator, false, null);
		}
		for (int i = 0; i < groupMembers.Count; i++)
		{
			if (groupMembers[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
			{
				this.m_selfPartyDisplay.Setup(groupMembers[i]);
				this.m_selfPartyDisplay.UpdateReadyState(groupMembers[i].IsReady);
				this.m_selfPartyDisplay.SetAsLeader(groupMembers[i].IsLeader);
			}
		}
		int j = 0;
		for (int k = 0; k < groupMembers.Count; k++)
		{
			if (j < this.m_partyMemberDisplay.Length && groupMembers[k].AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
			{
				this.m_partyMemberDisplay[j].Setup(groupMembers[k]);
				this.m_partyMemberDisplay[j].UpdateReadyState(groupMembers[k].IsReady);
				this.m_partyMemberDisplay[j].SetAsLeader(groupMembers[k].IsLeader);
				this.m_partyMemberDisplay[j].SetIsInGame(groupMembers[k].IsInGame);
				j++;
			}
		}
		while (j < this.m_partyMemberDisplay.Length)
		{
			this.m_partyMemberDisplay[j].SetToHidden();
			j++;
		}
		this.UpdateGroupedWithFriendStatus();
	}

	public void UpdateGroupedWithFriendStatus()
	{
		bool flag = this.IsGroupedWithFriend();
		if (flag != this.m_groupedWithFriend)
		{
			if (flag)
			{
				this.m_friendAnimator.Play("BonusIconDefaultIN");
				this.m_groupedWithFriend = true;
			}
			else
			{
				this.m_friendAnimator.Play("BonusIconDefaultOUT");
				this.m_groupedWithFriend = false;
			}
		}
	}

	public void SetVisible(bool visible, bool refreshOnly = false)
	{
		UIManager.SetGameObjectActive(base.gameObject, true, null);
		UIManager.SetGameObjectActive(this.m_container, visible, null);
		if (visible)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().PlayerWallet != null)
				{
					if (ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack) > 0)
					{
						this.m_GGPackAnimator.Play("BonusIconDefaultIN");
						goto IL_139;
					}
				}
			}
			bool flag = false;
			AnimatorClipInfo[] currentAnimatorClipInfo = this.m_GGPackAnimator.GetCurrentAnimatorClipInfo(0);
			if (currentAnimatorClipInfo != null)
			{
				if (currentAnimatorClipInfo.Length > 0)
				{
					if (currentAnimatorClipInfo[0].clip.name != "BonusIconDefaultOUT")
					{
						if (currentAnimatorClipInfo[0].clip.name != "BonusIconDisabled")
						{
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				this.m_GGPackAnimator.Play("BonusIconDefaultOUT");
			}
			IL_139:
			this.UpdateGroupedWithFriendStatus();
		}
		if (!this.m_receivedDataOnce)
		{
			this.m_canvasGroup.alpha = 0f;
		}
		this.m_isVisible = visible;
		UIManager.SetGameObjectActive(this.m_hoverContainer, false, null);
		UIManager.SetGameObjectActive(this.m_pressedContainer, false, null);
		if (!refreshOnly)
		{
			if (UIStorePanel.Get() != null)
			{
				UIStorePanel.Get().CloseStore();
			}
		}
		this.OnEnable();
	}

	public void Update()
	{
		UIManager.SetGameObjectActive(this.m_hoverContainer, this.m_isHover, null);
		UIManager.SetGameObjectActive(this.m_pressedContainer, this.m_isPressed, null);
		if (this.m_failedToGetData > 0f)
		{
			this.m_failedToGetData -= Time.time;
			if (this.m_failedToGetData <= 0f)
			{
				this.OnEnable();
			}
		}
		int num = 0;
		if (FriendListPanel.Get() != null)
		{
			UIManager.SetGameObjectActive(this.m_newFriendNotificationContainer, FriendListPanel.Get().GetNumFriendRequests() > 0, null);
			num = FriendListPanel.Get().GetNumOnlineFriends();
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_newFriendNotificationContainer, false, null);
		}
		this.m_numOnlineFriends.text = num.ToString();
		if (num != this.m_lastFriendCount)
		{
			this.m_lastFriendCount = num;
			this.UpdateGroupedWithFriendStatus();
		}
		this.UpdateExperience();
		this.UpdateExpBonusIconTimer();
	}

	public void OnStoreBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.MenuChoice);
		UIFrontEnd.Get().ToggleStoreVisibility();
	}

	public void OnFriendListMenuBtnClicked(BaseEventData data)
	{
		UIFrontEnd.Get().TogglePlayerFriendListVisibility();
	}

	private void OnGGButtonClick(BaseEventData data)
	{
		UIGGBoostPurchaseScreen.Get().SetVisible(true);
	}

	private bool GGTooltipSetup(UITooltipBase tooltip)
	{
		int num = 0;
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().PlayerWallet != null)
			{
				num = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
			}
		}
		string rightString = string.Empty;
		if (num > 0)
		{
			rightString = string.Format("<color=green>x{0}</color>", num);
		}
		else
		{
			rightString = string.Format("<color=#7f7f7f>x{0}</color>", num);
		}
		string text = StringUtil.TR("GGBoostDescription", "Global");
		if (num == 0)
		{
			text = text + "\n\n" + StringUtil.TR("NoGGBoostsPurchaseInStore", "Global");
		}
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(StringUtil.TR("GGBoosts", "Rewards"), text, rightString);
		return true;
	}

	private bool FriendTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		bool flag = this.IsGroupedWithFriend();
		if (flag)
		{
			uititledTooltip.Setup(StringUtil.TR("PlayWithFriendsBonus", "GameOver"), StringUtil.TR("PlayWithFriendsBonusDesc", "GameOver"), "<color=green>" + StringUtil.TR("Active", "Global") + "</color>");
		}
		else
		{
			uititledTooltip.Setup(StringUtil.TR("PlayWithFriendsBonus", "GameOver"), StringUtil.TR("PlayWithFriendsBonusDesc", "GameOver") + "\n\n" + StringUtil.TR("NotGroupedWithAFriend", "Global"), "<color=#7f7f7f>" + StringUtil.TR("Inactive", "Global") + "</color>");
		}
		return true;
	}

	private bool IsGroupedWithFriend()
	{
		for (int i = 0; i < this.m_partyMemberDisplay.Length; i++)
		{
			UpdateGroupMemberData memberInfo = this.m_partyMemberDisplay[i].GetMemberInfo();
			if (memberInfo != null)
			{
				long accountID = memberInfo.AccountID;
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().FriendList.Friends.ContainsKey(accountID))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void OnProfileEnter(BaseEventData data)
	{
		this.m_isHover = true;
	}

	public void OnProfileExit(BaseEventData data)
	{
		this.m_isHover = false;
		this.m_isPressed = false;
	}

	public void OnProfileDown(BaseEventData data)
	{
		this.m_isPressed = true;
	}

	public void OnProfileClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.button != PointerEventData.InputButton.Left)
		{
			return;
		}
		this.m_isPressed = true;
		UIFrontEnd.Get().TogglePlayerProgressScreenVisibility(true);
	}

	private bool FactionTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(StringUtil.TR("FactionsTitle", "Global"), this.m_currentFactionName, string.Empty);
		return true;
	}

	private void OnExpBonusButtonClick(BaseEventData data)
	{
		UIFrontEnd.Get().m_frontEndNavPanel.CollectionsBtnClicked(null);
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars.StoreItemsForPurchase.Length > 0)
		{
			int itemTemplateId = gameBalanceVars.StoreItemsForPurchase[0].m_itemTemplateId;
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemTemplateId);
			UIStorePanel.Get().SelectItem(itemTemplate);
		}
	}

	private bool ExpBonusTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		string tooltipText = StringUtil.TR("FreelancerExpBonusDesc", "Global");
		if (this.m_freelancerExpBonusTime != DateTime.MinValue)
		{
			DateTime dateTime = ClientGameManager.Get().UtcNow();
			if (this.m_freelancerExpBonusTime > dateTime)
			{
				TimeSpan difference = this.m_freelancerExpBonusTime - dateTime;
				tooltipText = string.Format(StringUtil.TR("FreelancerExpBonusDescWithTime", "Global"), StringUtil.GetTimeDifferenceText(difference, true));
			}
		}
		uititledTooltip.Setup(StringUtil.TR("FreelancerExpBonus", "Global"), tooltipText, string.Empty);
		return true;
	}

	private void OnEnable()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.IsPlayerAccountDataAvailable())
		{
			this.OnAccountDataUpdated(clientGameManager.GetPlayerAccountData());
		}
		else
		{
			if (!this.m_isVisible && this.m_container != null)
			{
				UIManager.SetGameObjectActive(this.m_container, false, null);
			}
			this.m_failedToGetData = 1f;
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData newData)
	{
		if (this.m_container != null)
		{
			if (this.m_isVisible)
			{
				UIManager.SetGameObjectActive(this.m_container, true, null);
			}
			this.m_playerHandle = HydrogenConfig.Get().Ticket.Handle;
			this.m_playerName.text = HydrogenConfig.Get().Ticket.GetFormattedHandle(Mathf.FloorToInt(this.m_playerName.fontSize * 0.7f));
			if (FriendListPanel.Get() != null)
			{
				FriendListPanel.Get().m_playerName.text = HydrogenConfig.Get().Ticket.GetFormattedHandle(Mathf.FloorToInt(FriendListPanel.Get().m_playerName.fontSize * 0.7f));
			}
			this.m_playerTitle.text = GameBalanceVars.Get().GetTitle(newData.AccountComponent.SelectedTitleID, string.Empty, -1);
			UIManager.SetGameObjectActive(this.m_playerTitle, true, null);
			this.m_canvasGroup.alpha = 1f;
			this.m_receivedDataOnce = true;
			if (newData.QuestComponent.SeasonExperience.ContainsKey(newData.QuestComponent.ActiveSeason))
			{
				this.m_endLevel = newData.QuestComponent.GetReactorLevel(SeasonWideData.Get().m_seasons);
				this.m_endExp = newData.QuestComponent.SeasonExperience[newData.QuestComponent.ActiveSeason].XPProgressThroughLevel;
			}
			else
			{
				this.m_endLevel = newData.ExperienceComponent.Level;
				this.m_endExp = newData.ExperienceComponent.XPProgressThroughLevel;
			}
			if (this.m_curLevel < 0)
			{
				this.m_curLevel = this.m_endLevel;
				this.m_curExp = this.m_endExp;
			}
			try
			{
				this.m_expToLevel = SeasonWideData.Get().GetSeasonExperience(newData.QuestComponent.ActiveSeason, newData.QuestComponent.GetSeasonExperienceComponent(newData.QuestComponent.ActiveSeason).Level);
			}
			catch (ArgumentException)
			{
				this.m_expToLevel = 0;
			}
			this.m_expPerSecond = 0;
			if (this.m_endLevel == this.m_curLevel)
			{
				this.m_expPerSecond = this.m_endExp - this.m_curExp;
			}
			else
			{
				this.m_expPerSecond = this.m_expToLevel;
			}
			this.m_expPerSecond = (int)((float)this.m_expPerSecond / 1f);
			if (this.m_expPerSecond < 1)
			{
				this.m_expPerSecond = 1;
			}
			if (this.m_curLevel > this.m_endLevel)
			{
				this.m_curLevel = this.m_endLevel;
				this.m_curExp = this.m_endLevel;
				this.m_expPerSecond = 1;
			}
			UIManager.SetGameObjectActive(this.m_questLevelSlider, true, null);
			if (this.m_curLevel == this.m_endLevel)
			{
				this.m_questLevelSlider.fillAmount = (float)this.m_endExp / (float)this.m_expToLevel;
			}
			else
			{
				this.m_questLevelSlider.fillAmount = 1f;
			}
		}
		this.UpdateFactionIcon();
		this.UpdateExpBonusIcon();
	}

	public bool IsPlayingLevelUpAnim()
	{
		return this.m_levelUpAnimator != null && this.m_levelUpAnimator.gameObject.activeSelf;
	}

	private void UpdateExperience()
	{
		if (this.m_expPerSecond != 0)
		{
			if (!this.IsPlayingLevelUpAnim())
			{
				PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
				int num = (int)((float)this.m_expPerSecond * Time.deltaTime);
				if (num < 1)
				{
					num = 1;
				}
				this.m_curExp += num;
				if (this.m_curLevel <= this.m_endLevel)
				{
					if (this.m_endExp > this.m_expToLevel)
					{
						this.m_expPerSecond = 0;
						goto IL_27F;
					}
				}
				if (this.m_curLevel == this.m_endLevel && this.m_curExp >= this.m_endExp)
				{
					this.m_curExp = this.m_endExp;
					this.m_expPerSecond = 0;
				}
				else if (this.m_curExp >= this.m_expToLevel)
				{
					int num2 = this.m_endLevel - this.m_curLevel;
					int num3 = playerAccountData.QuestComponent.SeasonLevel - num2;
					if (num3 > 0)
					{
						List<RewardUtils.RewardData> nextSeasonLevelRewards = RewardUtils.GetNextSeasonLevelRewards(num3);
						for (int i = 0; i < nextSeasonLevelRewards.Count; i++)
						{
							if (nextSeasonLevelRewards[i].InventoryTemplate == null)
							{
								UINewReward.Get().NotifyNewRewardReceived(nextSeasonLevelRewards[i], CharacterType.None, num3 + 1, -1);
							}
						}
						if (playerAccountData.QuestComponent.SeasonItemRewardsGranted.ContainsKey(num3 + 1))
						{
							List<int> list = playerAccountData.QuestComponent.SeasonItemRewardsGranted[num3 + 1];
							for (int j = 0; j < list.Count; j++)
							{
								UINewReward.Get().NotifySeasonReward(InventoryWideData.Get().GetItemTemplate(list[j]), num3 + 1, 1);
							}
						}
					}
					this.m_curExp = 0;
					this.m_curLevel++;
					int activeSeason = playerAccountData.QuestComponent.ActiveSeason;
					int seasonExperience = SeasonWideData.Get().GetSeasonExperience(activeSeason, playerAccountData.QuestComponent.GetSeasonExperienceComponent(activeSeason).Level);
					this.m_expPerSecond = (this.m_expToLevel = seasonExperience);
					this.m_questLevelSlider.fillAmount = (float)this.m_endExp / (float)this.m_expToLevel;
					UIManager.SetGameObjectActive(this.m_levelUpAnimator, true, null);
				}
				IL_27F:
				this.m_levelSlider.fillAmount = (float)this.m_curExp / (float)this.m_expToLevel;
				UIManager.SetGameObjectActive(this.m_levelSlider, true, null);
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
				Component tutorialLevelContainer = this.m_tutorialLevelContainer;
				bool doActive;
				if (seasonTemplate != null)
				{
					doActive = seasonTemplate.IsTutorial;
				}
				else
				{
					doActive = false;
				}
				UIManager.SetGameObjectActive(tutorialLevelContainer, doActive, null);
				Component normalLevelContainer = this.m_normalLevelContainer;
				bool doActive2;
				if (seasonTemplate != null)
				{
					doActive2 = !seasonTemplate.IsTutorial;
				}
				else
				{
					doActive2 = false;
				}
				UIManager.SetGameObjectActive(normalLevelContainer, doActive2, null);
				if (seasonTemplate != null)
				{
					if (seasonTemplate.IsTutorial)
					{
						int num4 = QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, seasonTemplate.Index) - 1;
						int num5 = playerAccountData.QuestComponent.SeasonLevel - 1;
						this.m_levelText.text = num5 + "/" + num4;
						this.m_levelTooltipString = string.Empty;
						Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards(-1));
						List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(seasonTemplate);
						if (availableSeasonEndRewards.Count > 0)
						{
							queue.Enqueue(availableSeasonEndRewards[0]);
						}
						UITutorialSeasonLevelBar[] componentsInChildren = this.m_tutorialLevelContainer.GetComponentsInChildren<UITutorialSeasonLevelBar>();
						for (int k = 0; k < num4; k++)
						{
							int num6 = k + 1;
							UITutorialSeasonLevelBar uitutorialSeasonLevelBar;
							if (k < componentsInChildren.Length)
							{
								uitutorialSeasonLevelBar = componentsInChildren[k];
							}
							else
							{
								uitutorialSeasonLevelBar = UnityEngine.Object.Instantiate<UITutorialSeasonLevelBar>(this.m_tutorialLevelBarPrefab);
								uitutorialSeasonLevelBar.transform.SetParent(this.m_tutorialLevelContainer.transform);
								uitutorialSeasonLevelBar.transform.localScale = Vector3.one;
								uitutorialSeasonLevelBar.transform.localPosition = Vector3.zero;
							}
							uitutorialSeasonLevelBar.SetFilled(num6 <= num5);
							UIManager.SetGameObjectActive(uitutorialSeasonLevelBar, num6 <= num4, null);
							RewardUtils.RewardData rewardData = null;
							while (queue.Count > 0)
							{
								if (rewardData != null)
								{
									break;
								}
								int num7 = queue.Peek().Level - 1;
								if (num7 < num6)
								{
									queue.Dequeue();
								}
								else
								{
									if (num7 > num6)
									{
										break;
									}
									rewardData = queue.Dequeue();
								}
							}
							uitutorialSeasonLevelBar.SetReward(num6 + 1, rewardData);
						}
					}
					else
					{
						this.m_levelText.text = ClientGameManager.Get().GetDisplayedStatString(this.m_curLevel, playerAccountData.QuestComponent.SeasonLevel, playerAccountData.ExperienceComponent.Wins);
						this.m_levelTooltipString = string.Empty;
						List<int> list2 = new List<int>(playerAccountData.QuestComponent.SeasonExperience.Keys);
						list2.Sort();
						for (int l = 0; l < list2.Count; l++)
						{
							SeasonTemplate seasonTemplate2 = SeasonWideData.Get().GetSeasonTemplate(list2[l]);
							if (seasonTemplate2.IsTutorial)
							{
							}
							else
							{
								if (!this.m_levelTooltipString.IsNullOrEmpty())
								{
									this.m_levelTooltipString += Environment.NewLine;
								}
								this.m_levelTooltipString += string.Format(StringUtil.TR("SeasonLevelDisplay", "Global"), seasonTemplate2.GetDisplayName(), playerAccountData.QuestComponent.SeasonExperience[list2[l]].Level);
							}
						}
					}
				}
				return;
			}
		}
	}

	public bool IsUpdatingExperience()
	{
		return this.m_expPerSecond > 0;
	}

	public void UpdateFactionIcon()
	{
		if (this.m_factionIcon == null)
		{
			return;
		}
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(activeFactionCompetition);
		if (factionCompetition != null)
		{
			Component factionContainer = this.m_factionContainer;
			bool doActive;
			if (factionCompetition.Enabled)
			{
				doActive = factionCompetition.ShouldShowcase;
			}
			else
			{
				doActive = false;
			}
			UIManager.SetGameObjectActive(factionContainer, doActive, null);
			GameBalanceVars.PlayerRibbon currentRibbon = ClientGameManager.Get().GetCurrentRibbon();
			int num;
			if (currentRibbon == null)
			{
				num = -1;
			}
			else
			{
				num = currentRibbon.ID;
			}
			int num2 = num;
			for (int i = 0; i < factionCompetition.Factions.Count; i++)
			{
				for (int j = 0; j < factionCompetition.Factions[i].RibbonIds.Count; j++)
				{
					if (factionCompetition.Factions[i].RibbonIds[j] == num2)
					{
						FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[i].FactionGroupIDToUse);
						this.m_factionIcon.sprite = Resources.Load<Sprite>(factionGroup.OutlinedIconPath);
						this.m_currentFactionName = Faction.GetDisplayName(activeFactionCompetition, i + 1);
						UIManager.SetGameObjectActive(this.m_factionIcon, true, null);
						return;
					}
				}
			}
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_factionContainer, false, null);
		}
		UIManager.SetGameObjectActive(this.m_factionIcon, false, null);
		this.m_currentFactionName = StringUtil.TR("NoCurrentFactionName", "Global");
	}

	public void UpdateExpBonusIcon()
	{
		this.m_freelancerExpBonusTime = DateTime.MinValue;
		AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
		DateTime dateTime = ClientGameManager.Get().UtcNow();
		if (accountComponent.FreelancerExpBonusTime > dateTime)
		{
			this.m_freelancerExpBonusTime = accountComponent.FreelancerExpBonusTime;
			TimeSpan difference = accountComponent.FreelancerExpBonusTime - dateTime;
			this.m_expBonusAmount.text = StringUtil.GetTimeDifferenceTextAbbreviated(difference);
			this.m_expBonusAnimator.Play("BonusIconDefaultIN");
		}
		else if (accountComponent.FreelancerExpBonusGames > 0)
		{
			this.m_expBonusAmount.text = string.Format("x{0}", accountComponent.FreelancerExpBonusGames);
			this.m_expBonusAnimator.Play("BonusIconDefaultIN");
		}
		else
		{
			this.m_expBonusAmount.text = string.Empty;
			this.m_expBonusAnimator.Play("BonusIconDefaultOUT");
		}
	}

	public void UpdateExpBonusIconTimer()
	{
		if (this.m_freelancerExpBonusTime != DateTime.MinValue)
		{
			DateTime dateTime = ClientGameManager.Get().UtcNow();
			if (this.m_freelancerExpBonusTime > dateTime)
			{
				TimeSpan difference = this.m_freelancerExpBonusTime - dateTime;
				this.m_expBonusAmount.text = StringUtil.GetTimeDifferenceTextAbbreviated(difference);
			}
			else
			{
				this.m_freelancerExpBonusTime = DateTime.MinValue;
				this.m_expBonusAmount.text = string.Empty;
				this.m_expBonusAnimator.Play("BonusIconDefaultOUT");
			}
		}
	}

	private void OnTrustBoostUsedNotification(TrustBoostUsedNotification notification)
	{
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(notification.CompetitionIndex);
		if (factionCompetition != null)
		{
			if (notification.FactionIndex < factionCompetition.Factions.Count)
			{
				if (UILootMatrixScreen.Get() != null)
				{
					if (UILootMatrixScreen.Get().IsOpening())
					{
						this.m_pendingTrustNotification = notification;
						return;
					}
				}
				float[] rbga = FactionWideData.Get().GetRBGA(factionCompetition.Factions[notification.FactionIndex]);
				Color color = new Color(rbga[0], rbga[1], rbga[2], rbga[3]);
				this.m_factionNotificationGlow.color = color;
				FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[notification.FactionIndex].FactionGroupIDToUse);
				this.m_factionNotificationIcon.sprite = Resources.Load<Sprite>(factionGroup.IconPath);
				this.m_factionNotificationText.color = color;
				this.m_factionNotificationText.text = "+" + notification.Amount;
				UIManager.SetGameObjectActive(this.m_factionNotification, true, null);
				this.m_pendingTrustNotification = null;
			}
		}
		this.UpdateFactionIcon();
	}

	public void HandlePendingTrustNotifications()
	{
		if (this.m_pendingTrustNotification != null)
		{
			this.OnTrustBoostUsedNotification(this.m_pendingTrustNotification);
		}
	}
}
