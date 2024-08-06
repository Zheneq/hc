using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
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
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	public void Start()
	{
		m_playerName.text = string.Empty;
		m_levelText.text = string.Empty;
		m_playerTitle.text = string.Empty;
		m_levelTooltipString = string.Empty;
		if (m_buttonHitBox != null)
		{
			m_buttonHitBox.callback = OnProfileClicked;
			m_buttonHitBox.pointerEnterCallback = OnProfileEnter;
			m_buttonHitBox.pointerExitCallback = OnProfileExit;
			UIEventTriggerUtils.AddListener(m_buttonHitBox.gameObject, EventTriggerType.PointerDown, OnProfileDown);
			UITooltipClickObject component = m_buttonHitBox.GetComponent<UITooltipClickObject>();
			
			component.Setup(TooltipType.PlayerGroupMenu, delegate(UITooltipBase tooltip)
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
									while (true)
									{
										switch (2)
										{
										case 0:
											break;
										default:
										{
											UIPlayerPanelGroupMenu uIPlayerPanelGroupMenu = tooltip as UIPlayerPanelGroupMenu;
											uIPlayerPanelGroupMenu.Setup(members[i]);
											return true;
										}
										}
									}
								}
							}
						}
					}
					return false;
				});
		}
		if (ggbuttonHitBox != null)
		{
			UIEventTriggerUtils.AddListener(ggbuttonHitBox.gameObject, EventTriggerType.PointerClick, OnGGButtonClick);
			ggbuttonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, GGTooltipSetup);
		}
		if (expBonusButtonHitBox != null)
		{
			UIEventTriggerUtils.AddListener(expBonusButtonHitBox.gameObject, EventTriggerType.PointerClick, OnExpBonusButtonClick);
			expBonusButtonHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, ExpBonusTooltipSetup);
		}
		m_expBonusAmount.raycastTarget = false;
		if (friendTooltipObj != null)
		{
			friendTooltipObj.Setup(TooltipType.Titled, FriendTooltipSetup);
		}
		if (factionTooltipObj != null)
		{
			factionTooltipObj.Setup(TooltipType.Titled, FactionTooltipSetup);
			m_currentFactionName = StringUtil.TR("FactionNone", "Global");
			UIManager.SetGameObjectActive(m_factionIcon, false);
		}
		m_friendMenuToggleBtn.spriteController.callback = OnFriendListMenuBtnClicked;
		m_newFriendNotificationBtn.callback = OnFriendListMenuBtnClicked;
		ClientGameManager clientGameManager = ClientGameManager.Get();
		OnPlayerTitleChange = delegate(string newTitle)
		{
			m_playerTitle.text = newTitle;
			UIManager.SetGameObjectActive(m_playerTitle, true);
		};
		clientGameManager.OnPlayerTitleChange += OnPlayerTitleChange;
		OnBankBalanceChange = delegate(CurrencyData currencyData)
		{
			if (currencyData.Type == CurrencyType.GGPack)
			{
				if (currencyData.m_Amount > 0)
				{
					m_GGPackAnimator.Play("BonusIconDefaultIN");
				}
				else
				{
					m_GGPackAnimator.Play("BonusIconDefaultOUT");
				}
			}
			int currentAmount2 = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
			m_GGPackCount.text = $"x{currentAmount2}";
			m_GGPackCount.raycastTarget = false;
			if (currentAmount2 > 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						UINewUserFlowManager.OnGGBoostOwned();
						return;
					}
				}
			}
		};
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		m_GGPackCount.text = $"x{currentAmount}";
		m_GGPackCount.raycastTarget = false;
		if (currentAmount > 0)
		{
			UINewUserFlowManager.OnGGBoostOwned();
		}
		OnFactionCompetitionNotification = delegate
		{
			UpdateFactionIcon();
		};
		OnPlayerRibbonChange = delegate
		{
			UpdateFactionIcon();
		};
		clientGameManager.OnBankBalanceChange += OnBankBalanceChange;
		clientGameManager.OnAccountDataUpdated += OnAccountDataUpdated;
		clientGameManager.OnFactionCompetitionNotification += OnFactionCompetitionNotification;
		clientGameManager.OnPlayerRibbonChange += OnPlayerRibbonChange;
		clientGameManager.OnTrustBoostUsedNotification += OnTrustBoostUsedNotification;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		if (ClientGameManager.Get().GroupInfo != null)
		{
			if (ClientGameManager.Get().GroupInfo.Members != null)
			{
				NotifyGroupUpdate(ClientGameManager.Get().GroupInfo.Members);
			}
		}
		if (FriendListPanel.Get() != null)
		{
			FriendListPanel.Get().Init();
		}
		UpdateFactionIcon();
		UpdateExpBonusIcon();
		if (!(m_levelNumberHitbox != null))
		{
			return;
		}
		while (true)
		{
			m_buttonHitBox.AddSubButton(m_levelNumberHitbox);
			m_levelNumberHitbox.callback = OnProfileClicked;
			m_levelNumberHitbox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				if (m_levelTooltipString.IsNullOrEmpty())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				(tooltip as UISimpleTooltip).Setup(m_levelTooltipString);
				return true;
			});
			return;
		}
	}

	public void NotifyQuestCompleted(QuestCompleteData data)
	{
		UIManager.SetGameObjectActive(m_questLevelSlider, true);
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(clientGameManager == null))
		{
			clientGameManager.OnPlayerTitleChange -= OnPlayerTitleChange;
			clientGameManager.OnBankBalanceChange -= OnBankBalanceChange;
			clientGameManager.OnAccountDataUpdated -= OnAccountDataUpdated;
			clientGameManager.OnFactionCompetitionNotification -= OnFactionCompetitionNotification;
			clientGameManager.OnPlayerRibbonChange -= OnPlayerRibbonChange;
			clientGameManager.OnTrustBoostUsedNotification -= OnTrustBoostUsedNotification;
		}
	}

	public void NotifyGroupUpdate(List<UpdateGroupMemberData> groupMembers)
	{
		if (groupMembers.Count == 0)
		{
			UIManager.SetGameObjectActive(m_selfPartyDisplay.m_PartyLeaderIconAnimator, false);
			UIManager.SetGameObjectActive(m_selfPartyDisplay.m_ReadyIconAnimator, false);
			UIManager.SetGameObjectActive(m_selfPartyDisplay.m_IsInGameAnimator, false);
		}
		for (int i = 0; i < groupMembers.Count; i++)
		{
			if (groupMembers[i].AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId)
			{
				m_selfPartyDisplay.Setup(groupMembers[i]);
				m_selfPartyDisplay.UpdateReadyState(groupMembers[i].IsReady);
				m_selfPartyDisplay.SetAsLeader(groupMembers[i].IsLeader);
			}
		}
		while (true)
		{
			int j = 0;
			for (int k = 0; k < groupMembers.Count; k++)
			{
				if (j < m_partyMemberDisplay.Length && groupMembers[k].AccountID != ClientGameManager.Get().GetPlayerAccountData().AccountId)
				{
					m_partyMemberDisplay[j].Setup(groupMembers[k]);
					m_partyMemberDisplay[j].UpdateReadyState(groupMembers[k].IsReady);
					m_partyMemberDisplay[j].SetAsLeader(groupMembers[k].IsLeader);
					m_partyMemberDisplay[j].SetIsInGame(groupMembers[k].IsInGame);
					j++;
				}
			}
			for (; j < m_partyMemberDisplay.Length; j++)
			{
				m_partyMemberDisplay[j].SetToHidden();
			}
			while (true)
			{
				UpdateGroupedWithFriendStatus();
				return;
			}
		}
	}

	public void UpdateGroupedWithFriendStatus()
	{
		bool flag = IsGroupedWithFriend();
		if (flag == m_groupedWithFriend)
		{
			return;
		}
		while (true)
		{
			if (flag)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						m_friendAnimator.Play("BonusIconDefaultIN");
						m_groupedWithFriend = true;
						return;
					}
				}
			}
			m_friendAnimator.Play("BonusIconDefaultOUT");
			m_groupedWithFriend = false;
			return;
		}
	}

	public void SetVisible(bool visible, bool refreshOnly = false)
	{
		UIManager.SetGameObjectActive(base.gameObject, true);
		UIManager.SetGameObjectActive(m_container, visible);
		if (visible)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().PlayerWallet != null)
				{
					if (ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack) > 0)
					{
						m_GGPackAnimator.Play("BonusIconDefaultIN");
						goto IL_0139;
					}
				}
			}
			bool flag = false;
			AnimatorClipInfo[] currentAnimatorClipInfo = m_GGPackAnimator.GetCurrentAnimatorClipInfo(0);
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
				m_GGPackAnimator.Play("BonusIconDefaultOUT");
			}
			goto IL_0139;
		}
		goto IL_013f;
		IL_013f:
		if (!m_receivedDataOnce)
		{
			m_canvasGroup.alpha = 0f;
		}
		m_isVisible = visible;
		UIManager.SetGameObjectActive(m_hoverContainer, false);
		UIManager.SetGameObjectActive(m_pressedContainer, false);
		if (!refreshOnly)
		{
			if (UIStorePanel.Get() != null)
			{
				UIStorePanel.Get().CloseStore();
			}
		}
		OnEnable();
		return;
		IL_0139:
		UpdateGroupedWithFriendStatus();
		goto IL_013f;
	}

	public void Update()
	{
		UIManager.SetGameObjectActive(m_hoverContainer, m_isHover);
		UIManager.SetGameObjectActive(m_pressedContainer, m_isPressed);
		if (m_failedToGetData > 0f)
		{
			m_failedToGetData -= Time.time;
			if (m_failedToGetData <= 0f)
			{
				OnEnable();
			}
		}
		int num = 0;
		if (FriendListPanel.Get() != null)
		{
			UIManager.SetGameObjectActive(m_newFriendNotificationContainer, FriendListPanel.Get().GetNumFriendRequests() > 0);
			num = FriendListPanel.Get().GetNumOnlineFriends();
		}
		else
		{
			UIManager.SetGameObjectActive(m_newFriendNotificationContainer, false);
		}
		m_numOnlineFriends.text = num.ToString();
		if (num != m_lastFriendCount)
		{
			m_lastFriendCount = num;
			UpdateGroupedWithFriendStatus();
		}
		UpdateExperience();
		UpdateExpBonusIconTimer();
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
		string empty = string.Empty;
		if (num > 0)
		{
			empty = $"<color=green>x{num}</color>";
		}
		else
		{
			empty = $"<color=#7f7f7f>x{num}</color>";
		}
		string text = StringUtil.TR("GGBoostDescription", "Global");
		if (num == 0)
		{
			text = text + "\n\n" + StringUtil.TR("NoGGBoostsPurchaseInStore", "Global");
		}
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(StringUtil.TR("GGBoosts", "Rewards"), text, empty);
		return true;
	}

	private bool FriendTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		if (IsGroupedWithFriend())
		{
			uITitledTooltip.Setup(StringUtil.TR("PlayWithFriendsBonus", "GameOver"), StringUtil.TR("PlayWithFriendsBonusDesc", "GameOver"), "<color=green>" + StringUtil.TR("Active", "Global") + "</color>");
		}
		else
		{
			uITitledTooltip.Setup(StringUtil.TR("PlayWithFriendsBonus", "GameOver"), StringUtil.TR("PlayWithFriendsBonusDesc", "GameOver") + "\n\n" + StringUtil.TR("NotGroupedWithAFriend", "Global"), "<color=#7f7f7f>" + StringUtil.TR("Inactive", "Global") + "</color>");
		}
		return true;
	}

	private bool IsGroupedWithFriend()
	{
		for (int i = 0; i < m_partyMemberDisplay.Length; i++)
		{
			UpdateGroupMemberData memberInfo = m_partyMemberDisplay[i].GetMemberInfo();
			if (memberInfo == null)
			{
				continue;
			}
			long accountID = memberInfo.AccountID;
			if (!(ClientGameManager.Get() != null))
			{
				continue;
			}
			if (!ClientGameManager.Get().FriendList.Friends.ContainsKey(accountID))
			{
				continue;
			}
			while (true)
			{
				return true;
			}
		}
		while (true)
		{
			return false;
		}
	}

	public void OnProfileEnter(BaseEventData data)
	{
		m_isHover = true;
	}

	public void OnProfileExit(BaseEventData data)
	{
		m_isHover = false;
		m_isPressed = false;
	}

	public void OnProfileDown(BaseEventData data)
	{
		m_isPressed = true;
	}

	public void OnProfileClicked(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.button != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_isPressed = true;
		UIFrontEnd.Get().TogglePlayerProgressScreenVisibility();
	}

	private bool FactionTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(StringUtil.TR("FactionsTitle", "Global"), m_currentFactionName, string.Empty);
		return true;
	}

	private void OnExpBonusButtonClick(BaseEventData data)
	{
		UIFrontEnd.Get().m_frontEndNavPanel.CollectionsBtnClicked(null);
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars.StoreItemsForPurchase.Length <= 0)
		{
			return;
		}
		while (true)
		{
			int itemTemplateId = gameBalanceVars.StoreItemsForPurchase[0].m_itemTemplateId;
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemTemplateId);
			UIStorePanel.Get().SelectItem(itemTemplate);
			return;
		}
	}

	private bool ExpBonusTooltipSetup(UITooltipBase tooltip)
	{
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		string tooltipText = StringUtil.TR("FreelancerExpBonusDesc", "Global");
		if (m_freelancerExpBonusTime != DateTime.MinValue)
		{
			DateTime dateTime = ClientGameManager.Get().UtcNow();
			if (m_freelancerExpBonusTime > dateTime)
			{
				TimeSpan difference = m_freelancerExpBonusTime - dateTime;
				tooltipText = string.Format(StringUtil.TR("FreelancerExpBonusDescWithTime", "Global"), StringUtil.GetTimeDifferenceText(difference, true));
			}
		}
		uITitledTooltip.Setup(StringUtil.TR("FreelancerExpBonus", "Global"), tooltipText, string.Empty);
		return true;
	}

	private void OnEnable()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager.IsPlayerAccountDataAvailable())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					OnAccountDataUpdated(clientGameManager.GetPlayerAccountData());
					return;
				}
			}
		}
		if (!m_isVisible && m_container != null)
		{
			UIManager.SetGameObjectActive(m_container, false);
		}
		m_failedToGetData = 1f;
	}

	private void OnAccountDataUpdated(PersistedAccountData newData)
	{
		if (m_container != null)
		{
			if (m_isVisible)
			{
				UIManager.SetGameObjectActive(m_container, true);
			}
			m_playerHandle = HydrogenConfig.Get().Ticket.Handle;
			m_playerName.text = HydrogenConfig.Get().Ticket.GetFormattedHandle(Mathf.FloorToInt(m_playerName.fontSize * 0.7f));
			if (FriendListPanel.Get() != null)
			{
				FriendListPanel.Get().m_playerName.text = HydrogenConfig.Get().Ticket.GetFormattedHandle(Mathf.FloorToInt(FriendListPanel.Get().m_playerName.fontSize * 0.7f));
			}
#if !VANILLA && !SERVER
            // Custom titles
            m_playerTitle.text = GameBalanceVars.Get().GetTitle(newData.AccountComponent.SelectedTitleID, newData.Handle, string.Empty);
#else
			m_playerTitle.text = GameBalanceVars.Get().GetTitle(newData.AccountComponent.SelectedTitleID, string.Empty);
#endif
			UIManager.SetGameObjectActive(m_playerTitle, true);
			m_canvasGroup.alpha = 1f;
			m_receivedDataOnce = true;
			if (newData.QuestComponent.SeasonExperience.ContainsKey(newData.QuestComponent.ActiveSeason))
			{
				m_endLevel = newData.QuestComponent.GetReactorLevel(SeasonWideData.Get().m_seasons);
				m_endExp = newData.QuestComponent.SeasonExperience[newData.QuestComponent.ActiveSeason].XPProgressThroughLevel;
			}
			else
			{
				m_endLevel = newData.ExperienceComponent.Level;
				m_endExp = newData.ExperienceComponent.XPProgressThroughLevel;
			}
			if (m_curLevel < 0)
			{
				m_curLevel = m_endLevel;
				m_curExp = m_endExp;
			}
			try
			{
				m_expToLevel = SeasonWideData.Get().GetSeasonExperience(newData.QuestComponent.ActiveSeason, newData.QuestComponent.GetSeasonExperienceComponent(newData.QuestComponent.ActiveSeason).Level);
			}
			catch (ArgumentException)
			{
				m_expToLevel = 0;
			}
			m_expPerSecond = 0;
			if (m_endLevel == m_curLevel)
			{
				m_expPerSecond = m_endExp - m_curExp;
			}
			else
			{
				m_expPerSecond = m_expToLevel;
			}
			m_expPerSecond = (int)((float)m_expPerSecond / 1f);
			if (m_expPerSecond < 1)
			{
				m_expPerSecond = 1;
			}
			if (m_curLevel > m_endLevel)
			{
				m_curLevel = m_endLevel;
				m_curExp = m_endLevel;
				m_expPerSecond = 1;
			}
			UIManager.SetGameObjectActive(m_questLevelSlider, true);
			if (m_curLevel == m_endLevel)
			{
				m_questLevelSlider.fillAmount = (float)m_endExp / (float)m_expToLevel;
			}
			else
			{
				m_questLevelSlider.fillAmount = 1f;
			}
		}
		UpdateFactionIcon();
		UpdateExpBonusIcon();
	}

	public bool IsPlayingLevelUpAnim()
	{
		return m_levelUpAnimator != null && m_levelUpAnimator.gameObject.activeSelf;
	}

	private void UpdateExperience()
	{
		if (m_expPerSecond == 0)
		{
			return;
		}
		while (true)
		{
			if (IsPlayingLevelUpAnim())
			{
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			int num = (int)((float)m_expPerSecond * Time.deltaTime);
			if (num < 1)
			{
				num = 1;
			}
			m_curExp += num;
			if (m_curLevel <= m_endLevel)
			{
				if (m_endExp > m_expToLevel)
				{
					m_expPerSecond = 0;
					goto IL_027f;
				}
			}
			if (m_curLevel == m_endLevel && m_curExp >= m_endExp)
			{
				m_curExp = m_endExp;
				m_expPerSecond = 0;
			}
			else if (m_curExp >= m_expToLevel)
			{
				int num2 = m_endLevel - m_curLevel;
				int num3 = playerAccountData.QuestComponent.SeasonLevel - num2;
				if (num3 > 0)
				{
					List<RewardUtils.RewardData> nextSeasonLevelRewards = RewardUtils.GetNextSeasonLevelRewards(num3);
					for (int i = 0; i < nextSeasonLevelRewards.Count; i++)
					{
						if (nextSeasonLevelRewards[i].InventoryTemplate == null)
						{
							UINewReward.Get().NotifyNewRewardReceived(nextSeasonLevelRewards[i], CharacterType.None, num3 + 1);
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
				m_curExp = 0;
				m_curLevel++;
				int activeSeason = playerAccountData.QuestComponent.ActiveSeason;
				int seasonExperience = SeasonWideData.Get().GetSeasonExperience(activeSeason, playerAccountData.QuestComponent.GetSeasonExperienceComponent(activeSeason).Level);
				m_expPerSecond = (m_expToLevel = seasonExperience);
				m_questLevelSlider.fillAmount = (float)m_endExp / (float)m_expToLevel;
				UIManager.SetGameObjectActive(m_levelUpAnimator, true);
			}
			goto IL_027f;
			IL_027f:
			m_levelSlider.fillAmount = (float)m_curExp / (float)m_expToLevel;
			UIManager.SetGameObjectActive(m_levelSlider, true);
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
			HorizontalLayoutGroup tutorialLevelContainer = m_tutorialLevelContainer;
			int doActive;
			if (seasonTemplate != null)
			{
				doActive = (seasonTemplate.IsTutorial ? 1 : 0);
			}
			else
			{
				doActive = 0;
			}
			UIManager.SetGameObjectActive(tutorialLevelContainer, (byte)doActive != 0);
			RectTransform normalLevelContainer = m_normalLevelContainer;
			int doActive2;
			if (seasonTemplate != null)
			{
				doActive2 = ((!seasonTemplate.IsTutorial) ? 1 : 0);
			}
			else
			{
				doActive2 = 0;
			}
			UIManager.SetGameObjectActive(normalLevelContainer, (byte)doActive2 != 0);
			if (seasonTemplate == null)
			{
				return;
			}
			while (true)
			{
				if (seasonTemplate.IsTutorial)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
						{
							int num4 = QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, seasonTemplate.Index) - 1;
							int num5 = playerAccountData.QuestComponent.SeasonLevel - 1;
							m_levelText.text = num5 + "/" + num4;
							m_levelTooltipString = string.Empty;
							Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards());
							List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(seasonTemplate);
							if (availableSeasonEndRewards.Count > 0)
							{
								queue.Enqueue(availableSeasonEndRewards[0]);
							}
							UITutorialSeasonLevelBar[] componentsInChildren = m_tutorialLevelContainer.GetComponentsInChildren<UITutorialSeasonLevelBar>();
							for (int k = 0; k < num4; k++)
							{
								int num6 = k + 1;
								UITutorialSeasonLevelBar uITutorialSeasonLevelBar;
								if (k < componentsInChildren.Length)
								{
									uITutorialSeasonLevelBar = componentsInChildren[k];
								}
								else
								{
									uITutorialSeasonLevelBar = UnityEngine.Object.Instantiate(m_tutorialLevelBarPrefab);
									uITutorialSeasonLevelBar.transform.SetParent(m_tutorialLevelContainer.transform);
									uITutorialSeasonLevelBar.transform.localScale = Vector3.one;
									uITutorialSeasonLevelBar.transform.localPosition = Vector3.zero;
								}
								uITutorialSeasonLevelBar.SetFilled(num6 <= num5);
								UIManager.SetGameObjectActive(uITutorialSeasonLevelBar, num6 <= num4);
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
								uITutorialSeasonLevelBar.SetReward(num6 + 1, rewardData);
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
				m_levelText.text = ClientGameManager.Get().GetDisplayedStatString(m_curLevel, playerAccountData.QuestComponent.SeasonLevel, playerAccountData.ExperienceComponent.Wins);
				m_levelTooltipString = string.Empty;
				List<int> list2 = new List<int>(playerAccountData.QuestComponent.SeasonExperience.Keys);
				list2.Sort();
				for (int l = 0; l < list2.Count; l++)
				{
					SeasonTemplate seasonTemplate2 = SeasonWideData.Get().GetSeasonTemplate(list2[l]);
					if (seasonTemplate2.IsTutorial)
					{
						continue;
					}
					if (!m_levelTooltipString.IsNullOrEmpty())
					{
						m_levelTooltipString += Environment.NewLine;
					}
					m_levelTooltipString += string.Format(StringUtil.TR("SeasonLevelDisplay", "Global"), seasonTemplate2.GetDisplayName(), playerAccountData.QuestComponent.SeasonExperience[list2[l]].Level);
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

	public bool IsUpdatingExperience()
	{
		return m_expPerSecond > 0;
	}

	public void UpdateFactionIcon()
	{
		if (m_factionIcon == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(activeFactionCompetition);
		if (factionCompetition != null)
		{
			RectTransform factionContainer = m_factionContainer;
			int doActive;
			if (factionCompetition.Enabled)
			{
				doActive = (factionCompetition.ShouldShowcase ? 1 : 0);
			}
			else
			{
				doActive = 0;
			}
			UIManager.SetGameObjectActive(factionContainer, (byte)doActive != 0);
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
						m_factionIcon.sprite = Resources.Load<Sprite>(factionGroup.OutlinedIconPath);
						m_currentFactionName = Faction.GetDisplayName(activeFactionCompetition, i + 1);
						UIManager.SetGameObjectActive(m_factionIcon, true);
						return;
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						goto end_IL_014f;
					}
					continue;
					end_IL_014f:
					break;
				}
			}
		}
		else
		{
			UIManager.SetGameObjectActive(m_factionContainer, false);
		}
		UIManager.SetGameObjectActive(m_factionIcon, false);
		m_currentFactionName = StringUtil.TR("NoCurrentFactionName", "Global");
	}

	public void UpdateExpBonusIcon()
	{
		m_freelancerExpBonusTime = DateTime.MinValue;
		AccountComponent accountComponent = ClientGameManager.Get().GetPlayerAccountData().AccountComponent;
		DateTime dateTime = ClientGameManager.Get().UtcNow();
		if (accountComponent.FreelancerExpBonusTime > dateTime)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					m_freelancerExpBonusTime = accountComponent.FreelancerExpBonusTime;
					TimeSpan difference = accountComponent.FreelancerExpBonusTime - dateTime;
					m_expBonusAmount.text = StringUtil.GetTimeDifferenceTextAbbreviated(difference);
					m_expBonusAnimator.Play("BonusIconDefaultIN");
					return;
				}
				}
			}
		}
		if (accountComponent.FreelancerExpBonusGames > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_expBonusAmount.text = $"x{accountComponent.FreelancerExpBonusGames}";
					m_expBonusAnimator.Play("BonusIconDefaultIN");
					return;
				}
			}
		}
		m_expBonusAmount.text = string.Empty;
		m_expBonusAnimator.Play("BonusIconDefaultOUT");
	}

	public void UpdateExpBonusIconTimer()
	{
		if (!(m_freelancerExpBonusTime != DateTime.MinValue))
		{
			return;
		}
		DateTime dateTime = ClientGameManager.Get().UtcNow();
		if (m_freelancerExpBonusTime > dateTime)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					TimeSpan difference = m_freelancerExpBonusTime - dateTime;
					m_expBonusAmount.text = StringUtil.GetTimeDifferenceTextAbbreviated(difference);
					return;
				}
				}
			}
		}
		m_freelancerExpBonusTime = DateTime.MinValue;
		m_expBonusAmount.text = string.Empty;
		m_expBonusAnimator.Play("BonusIconDefaultOUT");
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
						m_pendingTrustNotification = notification;
						return;
					}
				}
				float[] rBGA = FactionWideData.Get().GetRBGA(factionCompetition.Factions[notification.FactionIndex]);
				Color color = new Color(rBGA[0], rBGA[1], rBGA[2], rBGA[3]);
				m_factionNotificationGlow.color = color;
				FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[notification.FactionIndex].FactionGroupIDToUse);
				m_factionNotificationIcon.sprite = Resources.Load<Sprite>(factionGroup.IconPath);
				m_factionNotificationText.color = color;
				m_factionNotificationText.text = "+" + notification.Amount;
				UIManager.SetGameObjectActive(m_factionNotification, true);
				m_pendingTrustNotification = null;
			}
		}
		UpdateFactionIcon();
	}

	public void HandlePendingTrustNotifications()
	{
		if (m_pendingTrustNotification == null)
		{
			return;
		}
		while (true)
		{
			OnTrustBoostUsedNotification(m_pendingTrustNotification);
			return;
		}
	}
}
