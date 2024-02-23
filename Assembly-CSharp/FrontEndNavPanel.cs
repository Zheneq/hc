using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using WebSocketSharp;

public class FrontEndNavPanel : MonoBehaviour
{
	public _SelectableBtn m_landingPageBtn;

	public _SelectableBtn m_PlayBtn;

	public _SelectableBtn m_CollectionBtn;

	public _SelectableBtn m_CashShopBtn;

	public _SelectableBtn m_SeasonBtn;

	public _SelectableBtn m_LootMatrixBtn;

	public _SelectableBtn m_WatchBtn;

	public _SelectableBtn m_notificationsBtn;

	public _SelectableBtn m_menuBtn;

	public _SelectableBtn m_exitCustomGamesBtn;

	public RectTransform m_microphoneContainer;

	public _SelectableBtn m_microphoneConnectedBtn;

	public _SelectableBtn m_microphoneOfflineBtn;

	public UIVoiceListMenu m_voiceListMenu;

	public RectTransform m_LootMatrixNewContainer;

	public TextMeshProUGUI m_NewLootMatrixText;

	public RectTransform m_CashShopNewContainer;

	public Animator m_animationController;

	public RectTransform m_PlayButtonNoticeContainer;

	public TextMeshProUGUI m_freelancerCurrencyText;

	public TextMeshProUGUI m_isoText;

	public TextMeshProUGUI m_prestigeText;

	public RectTransform m_LimitedModeContainer;

	public Image m_LimitedModeHitbox;

	public RectTransform m_LimitedModeTooltip;

	public UIPlayCategoryMenu m_playMenuCatgeory;

	public RectTransform m_alertActiveIcon;

	public TextMeshProUGUI m_questNotificationNumber;

	public TextMeshProUGUI m_autoJoinDiscordText;

	private static FrontEndNavPanel s_instance;

	private List<_SelectableBtn> m_menuBtnList = new List<_SelectableBtn>();

	private _SelectableBtn m_currentNavBtn;

	private _SelectableBtn m_previousNavBtn;

	private _SelectableBtn m_gamePadHoverBtn;

	private float m_LastTimeNavbuttonClicked = -1f;

	private static int m_lastSeenNumberOfLootMatrices = -1;

	private SeasonLockoutReason m_seasonLockoutReason;

	public static FrontEndNavPanel Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_PlayBtn.spriteController.callback = PlayBtnClicked;
		m_CollectionBtn.spriteController.callback = CollectionsBtnClicked;
		m_CashShopBtn.spriteController.callback = CashShopBtnClicked;
		m_landingPageBtn.spriteController.callback = LandingPageBtnClicked;
		m_SeasonBtn.spriteController.callback = SeasonsBtnClicked;
		m_LootMatrixBtn.spriteController.callback = LootMatrixBtnClicked;
		m_notificationsBtn.spriteController.callback = NotificationBtnClicked;
		m_menuBtn.spriteController.callback = MenuBtnClicked;
		m_exitCustomGamesBtn.spriteController.callback = LandingPageBtnClicked;
		m_microphoneConnectedBtn.spriteController.callback = MicrophoneClicked;
		m_microphoneOfflineBtn.spriteController.callback = MicrophoneClicked;
		m_PlayBtn.spriteController.SetSelectableBtn(m_PlayBtn);
		m_CollectionBtn.spriteController.SetSelectableBtn(m_CollectionBtn);
		m_CashShopBtn.spriteController.SetSelectableBtn(m_CashShopBtn);
		m_landingPageBtn.spriteController.SetSelectableBtn(m_landingPageBtn);
		m_SeasonBtn.spriteController.SetSelectableBtn(m_SeasonBtn);
		m_LootMatrixBtn.spriteController.SetSelectableBtn(m_LootMatrixBtn);
		UIEventTriggerUtils.AddListener(m_LimitedModeHitbox.gameObject, EventTriggerType.PointerEnter, delegate
		{
			UIManager.SetGameObjectActive(m_LimitedModeTooltip, true);
		});
		UIEventTriggerUtils.AddListener(m_LimitedModeHitbox.gameObject, EventTriggerType.PointerExit, delegate
		{
			UIManager.SetGameObjectActive(m_LimitedModeTooltip, false);
		});
		UIManager.SetGameObjectActive(m_LimitedModeTooltip, false);
		m_notificationsBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, PopulateContractsTooltip);
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
			OnInventoryDataUpdated(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent);
			CheckNewCashShopFeaturedItems();
		}
		else
		{
			UIManager.SetGameObjectActive(m_LootMatrixNewContainer.GetComponentInChildren<Animator>(true), false);
			UIManager.SetGameObjectActive(m_CashShopNewContainer, false);
		}
		m_PlayBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		m_CollectionBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		m_CashShopBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		m_SeasonBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		m_LootMatrixBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		m_landingPageBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		m_menuBtnList.Add(m_PlayBtn);
		m_menuBtnList.Add(m_CollectionBtn);
		m_menuBtnList.Add(m_CashShopBtn);
		m_menuBtnList.Add(m_landingPageBtn);
		m_menuBtnList.Add(m_SeasonBtn);
		m_menuBtnList.Add(m_LootMatrixBtn);
		m_menuBtn.SetSelected(false, false, string.Empty, string.Empty);
	}

	private void Start()
	{
		ClientGameManager.Get().OnLobbyServerReadyNotification += HandleLobbyServerReadyNotification;
		ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
		ClientGameManager.Get().OnInventoryComponentUpdated += OnInventoryDataUpdated;
		ClientGameManager.Get().OnLobbyServerClientAccessLevelChange += HandleLobbyServerClientAccessLevelChange;
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += HandleLobbyGameplayOverridesChange;
		ClientGameManager.Get().OnBankBalanceChange += HandleBankBalanceChange;
		ClientGameManager.Get().OnAlertMissionDataChange += HandleAlertMissionDataChange;
		HandleLobbyServerClientAccessLevelChange(ClientGameManager.Get().ClientAccessLevel, ClientGameManager.Get().ClientAccessLevel);
		ClientGameManager.Get().QueryPlayerMatchData(HandlePlayerMatchDataResponse);
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		int num = 0;
		int value = 0;
		int value2 = 0;
		int num2 = 0;
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().PlayerWallet != null)
			{
				CurrencyWallet playerWallet = ClientGameManager.Get().PlayerWallet;
				num = playerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
				value = playerWallet.GetCurrentAmount(CurrencyType.ISO);
				value2 = playerWallet.GetCurrentAmount(CurrencyType.RankedCurrency);
				num2 = playerWallet.GetCurrentAmount(CurrencyType.UnlockFreelancerToken);
			}
		}

		m_freelancerCurrencyText.text = new StringBuilder().Append("<sprite name=credit>").Append(UIStorePanel.FormatIntToString(num, true)).ToString();
		UITooltipHoverObject component = m_freelancerCurrencyText.GetComponent<UITooltipHoverObject>();
		
		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip3 = (UISimpleTooltip)tooltip;
				uISimpleTooltip3.Setup(StringUtil.TR("FreelancerCurrencyDesc", "Global"));
				return true;
			});
		m_isoText.text = new StringBuilder().Append("<sprite name=iso>").Append(UIStorePanel.FormatIntToString(value, true)).ToString();
		UITooltipHoverObject component2 = m_isoText.GetComponent<UITooltipHoverObject>();
		
		component2.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip2 = (UISimpleTooltip)tooltip;
				uISimpleTooltip2.Setup(StringUtil.TR("ISODescription", "Global"));
				return true;
			});
		m_prestigeText.text = new StringBuilder().Append("<sprite name=rankedCurrency>").Append(UIStorePanel.FormatIntToString(value2, true)).ToString();
		UITooltipHoverObject component3 = m_prestigeText.GetComponent<UITooltipHoverObject>();
		
		component3.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
				uISimpleTooltip.Setup(StringUtil.TR("RankedCurrencyDescription", "Global"));
				return true;
			});
		if (num > 0)
		{
			UINewUserFlowManager.OnFreelancerCurrencyOwned();
		}
		if (num2 > 0)
		{
			UINewUserFlowManager.OnFreelancerTokenOwned();
		}
		UIManager.SetGameObjectActive(m_microphoneConnectedBtn, false);
		UIManager.SetGameObjectActive(m_microphoneOfflineBtn, true);
		DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
		discordClientInterface.OnJoined = (Action)Delegate.Combine(discordClientInterface.OnJoined, new Action(DiscordOnJoined));
		DiscordClientInterface.Get().OnError += DiscordOnError;
		DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
		discordClientInterface2.OnDisconnected = (Action)Delegate.Combine(discordClientInterface2.OnDisconnected, new Action(DiscordOnDisconnected));
		CheckMicrophoneEnabled();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnLobbyServerReadyNotification -= HandleLobbyServerReadyNotification;
			ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
			ClientGameManager.Get().OnInventoryComponentUpdated -= OnInventoryDataUpdated;
			ClientGameManager.Get().OnLobbyServerClientAccessLevelChange -= HandleLobbyServerClientAccessLevelChange;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= HandleLobbyGameplayOverridesChange;
			ClientGameManager.Get().OnBankBalanceChange -= HandleBankBalanceChange;
			ClientGameManager.Get().OnAlertMissionDataChange -= HandleAlertMissionDataChange;
		}
		if (DiscordClientInterface.Get() != null)
		{
			DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
			discordClientInterface.OnJoined = (Action)Delegate.Remove(discordClientInterface.OnJoined, new Action(DiscordOnJoined));
			DiscordClientInterface.Get().OnError -= DiscordOnError;
			DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
			discordClientInterface2.OnDisconnected = (Action)Delegate.Remove(discordClientInterface2.OnDisconnected, new Action(DiscordOnDisconnected));
		}
		if (!(s_instance == this))
		{
			return;
		}
		while (true)
		{
			s_instance = null;
			return;
		}
	}

	private void Update()
	{
		if (!(UIFrontendLoadingScreen.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (UIFrontendLoadingScreen.Get().IsVisible() || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			while (true)
			{
				if (!(Options_UI.Get() == null))
				{
					if (Options_UI.Get().IsVisible())
					{
						return;
					}
				}
				if (!(KeyBinding_UI.Get() == null))
				{
					if (KeyBinding_UI.Get().IsVisible())
					{
						return;
					}
				}
				int num;
				if (DebugParameters.Get() != null)
				{
					num = (DebugParameters.Get().GetParameterAsBool("DebugCamera") ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				if (num != 0)
				{
					return;
				}
				while (true)
				{
					if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Seasons_FE_Nav))
					{
						if (GameManager.Get() != null)
						{
							if (GameManager.Get().GameplayOverrides != null && GameManager.Get().GameplayOverrides.EnableSeasons)
							{
								if (m_SeasonBtn.spriteController.IsClickable())
								{
									UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
									SeasonsBtnClicked(null);
								}
							}
						}
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Collection_FE_Nav))
					{
						if (GameManager.Get() != null)
						{
							if (GameManager.Get().GameplayOverrides != null)
							{
								if (GameManager.Get().GameplayOverrides.EnableShop)
								{
									UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
									CollectionsBtnClicked(null);
								}
							}
						}
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Loot_FE_Nav))
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
						LootMatrixBtnClicked(null);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.RecruitAFriend_FE_Nav))
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
						ReferAFriendBtnClicked(null);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Profile_FE_Nav))
					{
						UIFrontEnd.Get().TogglePlayerProgressScreenVisibility();
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.FreelancerStats_FE_Nav))
					{
						UIPlayerProgressPanel uIPlayerProgressPanel = UIPlayerProgressPanel.Get();
						uIPlayerProgressPanel.SetVisible(true, false);
						uIPlayerProgressPanel.NotifyMenuButtonClicked(uIPlayerProgressPanel.m_stats);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Match_History_Replays_FE_Nav))
					{
						UIPlayerProgressPanel uIPlayerProgressPanel2 = UIPlayerProgressPanel.Get();
						uIPlayerProgressPanel2.SetVisible(true, false);
						uIPlayerProgressPanel2.NotifyMenuButtonClicked(uIPlayerProgressPanel2.m_history);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Banner_FE_Nav))
					{
						UIPlayerProgressPanel uIPlayerProgressPanel3 = UIPlayerProgressPanel.Get();
						uIPlayerProgressPanel3.SetVisible(true, false);
						uIPlayerProgressPanel3.NotifyMenuButtonClicked(uIPlayerProgressPanel3.m_banner);
					}
					if (!(GameManager.Get() != null))
					{
						return;
					}
					while (true)
					{
						if (GameManager.Get().GameplayOverrides.DisableControlPadInput || m_playMenuCatgeory.IsVisible())
						{
							return;
						}
						if (Input.GetButtonDown("GamepadButtonLeftShoulder"))
						{
							m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
							if (m_gamePadHoverBtn == m_landingPageBtn)
							{
								m_gamePadHoverBtn = m_LootMatrixBtn;
							}
							else if (m_gamePadHoverBtn == m_PlayBtn)
							{
								m_gamePadHoverBtn = m_landingPageBtn;
							}
							else if (m_gamePadHoverBtn == m_SeasonBtn)
							{
								m_gamePadHoverBtn = m_PlayBtn;
							}
							else if (m_gamePadHoverBtn == m_CollectionBtn)
							{
								m_gamePadHoverBtn = m_SeasonBtn;
							}
							else if (m_gamePadHoverBtn == m_LootMatrixBtn)
							{
								m_gamePadHoverBtn = m_CollectionBtn;
							}
							m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
						}
						else if (Input.GetButtonDown("GamepadButtonRightShoulder"))
						{
							m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
							if (m_gamePadHoverBtn == m_landingPageBtn)
							{
								m_gamePadHoverBtn = m_PlayBtn;
							}
							else if (m_gamePadHoverBtn == m_PlayBtn)
							{
								m_gamePadHoverBtn = m_SeasonBtn;
							}
							else if (m_gamePadHoverBtn == m_SeasonBtn)
							{
								m_gamePadHoverBtn = m_CollectionBtn;
							}
							else if (m_gamePadHoverBtn == m_CollectionBtn)
							{
								m_gamePadHoverBtn = m_LootMatrixBtn;
							}
							else if (m_gamePadHoverBtn == m_LootMatrixBtn)
							{
								m_gamePadHoverBtn = m_landingPageBtn;
							}
							m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
						}
						if (!Input.GetButtonDown("GamepadButtonA"))
						{
							return;
						}
						while (true)
						{
							if (!(m_currentNavBtn != m_gamePadHoverBtn))
							{
								return;
							}
							if (m_gamePadHoverBtn == m_landingPageBtn)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										LandingPageBtnClicked(null);
										return;
									}
								}
							}
							if (m_gamePadHoverBtn == m_PlayBtn)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										PlayBtnClicked(null);
										return;
									}
								}
							}
							if (m_gamePadHoverBtn == m_SeasonBtn)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										SeasonsBtnClicked(null);
										return;
									}
								}
							}
							if (m_gamePadHoverBtn == m_CollectionBtn)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
										CollectionsBtnClicked(null);
										return;
									}
								}
							}
							if (m_gamePadHoverBtn == m_LootMatrixBtn)
							{
								while (true)
								{
									LootMatrixBtnClicked(null);
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

	public void RefreshUI()
	{
		SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
		CheckSeasonsVisibility();
		CheckContractsEnabled();
		CheckMicrophoneEnabled();
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		if (newLevel == ClientAccessLevel.Locked)
		{
			while (true)
			{
				AppState_GroupCharacterSelect appState_GroupCharacterSelect;
				switch (2)
				{
				case 0:
					break;
				default:
					{
						if (!m_landingPageBtn.isActiveAndEnabled)
						{
							if (!m_exitCustomGamesBtn.isActiveAndEnabled)
							{
								goto IL_004a;
							}
						}
						LandingPageBtnClicked(null);
						goto IL_004a;
					}
					IL_004a:
					appState_GroupCharacterSelect = AppState_GroupCharacterSelect.Get();
					if (appState_GroupCharacterSelect != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								if (appState_GroupCharacterSelect.InQueue())
								{
									appState_GroupCharacterSelect.UpdateReadyState(false);
									NavigationBar navigationBar = NavigationBar.Get();
									if (navigationBar != null)
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												UIManager.SetGameObjectActive(navigationBar.m_cancelBtn, false);
												navigationBar.m_cancelBtn.spriteController.SetClickable(false);
												navigationBar.m_searchQueueText.text = string.Empty;
												return;
											}
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
			}
		}
		RectTransform limitedModeContainer = m_LimitedModeContainer;
		int doActive;
		if (newLevel >= ClientAccessLevel.Full && ClientGameManager.Get() != null)
		{
			doActive = ((!ClientGameManager.Get().HasPurchasedGame) ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(limitedModeContainer, (byte)doActive != 0);
	}

	public void HandleLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		RefreshUI();
	}

	public void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		if (notification.Success)
		{
			OnAccountDataUpdated(notification.AccountData);
			ClientGameManager.Get().QueryPlayerMatchData(HandlePlayerMatchDataResponse);
		}
	}

	private void HandlePlayerMatchDataResponse(PlayerMatchDataResponse response)
	{
	}

	private void OnInventoryDataUpdated(InventoryComponent inventoryData)
	{
		int numLockBoxes = 0;
		inventoryData.Items.ForEach(delegate(InventoryItem t)
		{
			InventoryItemTemplate template = t.GetTemplate();
			if (template.Type == InventoryItemType.Lockbox)
			{
				numLockBoxes++;
			}
		});
		bool flag = m_lastSeenNumberOfLootMatrices < numLockBoxes;
		Animator componentInChildren = m_LootMatrixNewContainer.GetComponentInChildren<Animator>(true);
		int doActive;
		if (numLockBoxes > 0)
		{
			doActive = (flag ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(componentInChildren, (byte)doActive != 0);
		m_NewLootMatrixText.text = numLockBoxes.ToString();
		if (!m_NewLootMatrixText)
		{
			m_lastSeenNumberOfLootMatrices = numLockBoxes;
		}
		if (numLockBoxes > 0)
		{
			UINewUserFlowManager.OnHasLootMatrix();
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData accountData)
	{
		int num = 0;
		if (accountData.AccountComponent.DailyQuestsAvailable)
		{
			using (Dictionary<int, QuestProgress>.Enumerator enumerator = accountData.QuestComponent.Progress.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int key = enumerator.Current.Key;
					if (QuestWideData.Get().IsDailyQuest(key))
					{
						num++;
					}
				}
			}
		}
		if (ClientGameManager.Get().AlertMissionsData != null)
		{
			if (ClientGameManager.Get().AlertMissionsData.CurrentAlert != null)
			{
				UIManager.SetGameObjectActive(m_alertActiveIcon, true);
				if (ClientGameManager.Get().AlertMissionsData.CurrentAlert.Type == AlertMissionType.Quest)
				{
					num++;
				}
				goto IL_0112;
			}
		}
		UIManager.SetGameObjectActive(m_alertActiveIcon, false);
		goto IL_0112;
		IL_0112:
		m_questNotificationNumber.text = num.ToString();
		if (UIFrontEnd.Get() != null)
		{
			UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
			UIFrontEnd.Get().m_frontEndNavPanel.CheckContractsEnabled();
		}
		CheckNewCashShopFeaturedItems();
	}

	private void HandleAlertMissionDataChange(LobbyAlertMissionDataNotification notification)
	{
		OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
	}

	private void HandleBankBalanceChange(CurrencyData newBalance)
	{
		if (newBalance.Type == CurrencyType.FreelancerCurrency)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_freelancerCurrencyText.text = new StringBuilder().Append("<sprite name=credit>").Append(UIStorePanel.FormatIntToString(newBalance.Amount, true)).ToString();
					if (newBalance.Amount > 0)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								UINewUserFlowManager.OnFreelancerCurrencyOwned();
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (newBalance.Type == CurrencyType.ISO)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_isoText.text = new StringBuilder().Append("<sprite name=iso>").Append(UIStorePanel.FormatIntToString(newBalance.Amount, true)).ToString();
					return;
				}
			}
		}
		if (newBalance.Type == CurrencyType.RankedCurrency)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					m_prestigeText.text = new StringBuilder().Append("<sprite name=rankedCurrency>").Append(UIStorePanel.FormatIntToString(newBalance.Amount, true)).ToString();
					return;
				}
			}
		}
		if (newBalance.Type != CurrencyType.UnlockFreelancerToken)
		{
			return;
		}
		while (true)
		{
			if (newBalance.Amount > 0)
			{
				UINewUserFlowManager.OnFreelancerTokenOwned();
			}
			return;
		}
	}

	public void SetShopVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_CollectionBtn.spriteController, visible);
		CanvasGroup[] componentsInChildren = m_CollectionBtn.GetComponentsInChildren<CanvasGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject != m_CollectionBtn.gameObject)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], visible);
			}
		}
		while (true)
		{
			if (visible || !(UIStorePanel.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (UIStorePanel.Get().IsStoreOpen())
				{
					while (true)
					{
						LandingPageBtnClicked(null);
						return;
					}
				}
				return;
			}
		}
	}

	public void CheckSeasonsVisibility()
	{
		SetSeasonsVisible(UISeasonsPanel.CheckSeasonsVisibility(out m_seasonLockoutReason));
	}

	public void SetSeasonsVisible(bool visible)
	{
		m_SeasonBtn.spriteController.SetClickable(visible);
		CanvasGroup[] componentsInChildren = m_SeasonBtn.GetComponentsInChildren<CanvasGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject != m_SeasonBtn.gameObject)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], visible);
			}
		}
		while (true)
		{
			if (!visible)
			{
				if (UISeasonsPanel.Get() != null)
				{
					if (UISeasonsPanel.Get().IsVisible())
					{
						m_SeasonBtn.SetSelected(false, false, string.Empty, string.Empty);
						UISeasonsPanel.Get().SetVisible(false);
					}
				}
			}
			if (m_seasonLockoutReason != 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						m_SeasonBtn.spriteController.SetForceHovercallback(true);
						m_SeasonBtn.spriteController.SetForceExitCallback(true);
						m_SeasonBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, PopulateSeasonsTooltip);
						return;
					}
				}
			}
			m_SeasonBtn.spriteController.SetForceHovercallback(false);
			m_SeasonBtn.spriteController.SetForceExitCallback(false);
			m_SeasonBtn.spriteController.pointerEnterCallback = null;
			m_SeasonBtn.spriteController.pointerExitCallback = null;
			return;
		}
	}

	private bool PopulateSeasonsTooltip(UITooltipBase tooltip)
	{
		if (m_seasonLockoutReason == SeasonLockoutReason.InTutorialSeason)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().GetPlayerAccountData() != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
						{
							int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
							int seasonLevel = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.SeasonLevel;
							SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(activeSeason);
							UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
							uITitledTooltip.Setup(StringUtil.TR("Locked", "Global"), string.Format(StringUtil.TR("RequiresMatchesPlayed", "Global"), QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, activeSeason) - seasonLevel), string.Empty);
							return true;
						}
						}
					}
				}
			}
		}
		else if (m_seasonLockoutReason == SeasonLockoutReason.Disabled)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					UITitledTooltip uITitledTooltip2 = tooltip as UITitledTooltip;
					uITitledTooltip2.Setup(StringUtil.TR("Locked", "Global"), StringUtil.TR("SeasonsDisabled", "Global"), string.Empty);
					return true;
				}
				}
			}
		}
		return false;
	}

	public void CheckContractsEnabled()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		bool flag = false;
		if (clientGameManager != null)
		{
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
				if (clientGameManager.GetPlayerAccountData().AccountComponent.DailyQuestsAvailable)
				{
					flag = true;
				}
			}
		}
		m_notificationsBtn.spriteController.SetClickable(flag);
		m_notificationsBtn.SetDisabled(!flag);
	}

	public void CheckMicrophoneEnabled()
	{
		UIManager.SetGameObjectActive(m_microphoneContainer, DiscordClientInterface.IsEnabled && DiscordClientInterface.IsSdkEnabled);
		TextMeshProUGUI autoJoinDiscordText = m_autoJoinDiscordText;
		object term;
		if (DiscordClientInterface.IsSdkEnabled)
		{
			term = "AutoJoinVoiceBtn";
		}
		else
		{
			term = "JoinDiscordBtn";
		}
		autoJoinDiscordText.text = StringUtil.TR((string)term, "NewFrontEndScene");
		if (DiscordClientInterface.Get().IsConnected && DiscordClientInterface.Get().ChannelInfo != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					DiscordOnJoined();
					return;
				}
			}
		}
		DiscordOnDisconnected();
	}

	private void CheckNewCashShopFeaturedItems()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
		{
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.CashShopFeaturedItemsVersionViewed;
						int uIState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
						int featuredItemsVersion = StoreWideData.Get().m_featuredItemsVersion;
						UIManager.SetGameObjectActive(m_CashShopNewContainer, uIState < featuredItemsVersion);
						return;
					}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_CashShopNewContainer, false);
	}

	private bool PopulateContractsTooltip(UITooltipBase tooltip)
	{
		if (m_notificationsBtn.spriteController.IsClickable())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(StringUtil.TR("Locked", "Global"), string.Format(StringUtil.TR("DailyQuestsUnlockRequirements", "Quests")), string.Empty);
		return true;
	}

	public void SetLootMatrixVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_LootMatrixBtn.spriteController, visible);
		CanvasGroup[] componentsInChildren = m_LootMatrixBtn.GetComponentsInChildren<CanvasGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject != m_LootMatrixBtn.gameObject)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], visible);
			}
		}
		while (true)
		{
			if (visible)
			{
				return;
			}
			while (true)
			{
				if (UILootMatrixScreen.Get().IsVisible)
				{
					m_LootMatrixBtn.SetSelected(false, false, string.Empty, string.Empty);
					UILootMatrixScreen.Get().SetVisible(false);
				}
				return;
			}
		}
	}

	public void NotifyGroupUpdate()
	{
	}

	public void NotificationBtnClicked(BaseEventData data)
	{
		if (!m_notificationsBtn.spriteController.IsClickable())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (UIMainMenu.Get() != null)
		{
			if (UIMainMenu.Get().IsOpen())
			{
				UIMainMenu.Get().SetMenuVisible(false);
			}
		}
		if (m_notificationsBtn.IsSelected())
		{
			QuestListPanel.Get().SetVisible(false);
			m_notificationsBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			QuestListPanel.Get().SetVisible(true);
			m_notificationsBtn.SetSelected(true, false, string.Empty, string.Empty);
		}
	}

	public void MenuBtnClicked(BaseEventData data)
	{
		if (!UIMainMenu.Get().IsOpen())
		{
			if (m_notificationsBtn.IsSelected())
			{
				QuestListPanel.Get().SetVisible(false);
				m_notificationsBtn.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		UIMainMenu.Get().SetMenuVisible(!UIMainMenu.Get().IsOpen());
	}

	public void LandingPageBtnClicked(BaseEventData data)
	{
		if (UIFrontEnd.Get() == null)
		{
			return;
		}
		while (true)
		{
			if (UIStorePanel.Get() == null || UILootMatrixScreen.Get() == null || UIRAFProgramScreen.Get() == null)
			{
				return;
			}
			while (true)
			{
				if (UIPlayerProgressPanel.Get() == null)
				{
					return;
				}
				while (true)
				{
					if (UISeasonsPanel.Get() == null)
					{
						while (true)
						{
							switch (6)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
					UIGGBoostPurchaseScreen.Get().SetVisible(false);
					UIPlayerProgressPanel.Get().SetVisible(false);
					UIRAFProgramScreen.Get().SetVisible(false);
					if (!UICashShopPanel.Get().IsVisible())
					{
						if (!UIStorePanel.Get().IsStoreOpen())
						{
							if (!UISeasonsPanel.Get().IsVisible())
							{
								if (!UILootMatrixScreen.Get().IsVisible && AppState_LandingPage.Get() == AppState.GetCurrent())
								{
									if (m_currentNavBtn == m_landingPageBtn)
									{
										return;
									}
								}
							}
						}
					}
					if (UIGameSettingsPanel.Get() != null)
					{
						UIGameSettingsPanel.Get().SetVisible(false);
					}
					if (m_LastTimeNavbuttonClicked == Time.time)
					{
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
					m_LastTimeNavbuttonClicked = Time.time;
					SetNavButtonSelected(m_landingPageBtn);
					if (AppState_LandingPage.Get() != AppState.GetCurrent())
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
								{
									AppState_GameTeardown.Get().Enter();
								}
								else
								{
									AppState_LandingPage.Get().Enter(true);
									SetPlayMenuCatgeoryVisible(false);
								}
								return;
							}
						}
					}
					UIFrontEnd.Get().m_landingPageScreen.SetVisible(true);
					return;
				}
			}
		}
	}

	public void PlayBtnClicked(BaseEventData data)
	{
		if (m_LastTimeNavbuttonClicked == Time.time)
		{
			while (true)
			{
				return;
			}
		}
		m_LastTimeNavbuttonClicked = Time.time;
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false);
		UIRAFProgramScreen.Get().SetVisible(false);
		UINewUserFlowManager.OnGameModeButtonDisplayed();
		if (!UICashShopPanel.Get().IsVisible())
		{
			if (!UIStorePanel.Get().IsStoreOpen())
			{
				if (!UISeasonsPanel.Get().IsVisible())
				{
					if (!UILootMatrixScreen.Get().IsVisible)
					{
						if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
						{
							if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
							{
								goto IL_0139;
							}
						}
						if (m_currentNavBtn == m_PlayBtn)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									if (!m_playMenuCatgeory.IsVisible())
									{
										SetPlayMenuCatgeoryVisible(true);
									}
									return;
								}
							}
						}
					}
				}
			}
		}
		goto IL_0139;
		IL_0139:
		SetNavButtonSelected(m_PlayBtn);
		SetPlayMenuCatgeoryVisible(true);
		UIFrontEnd.Get().m_landingPageScreen.QuickPlayButtonClicked(data);
	}

	public void DoSeasonsBtnClicked(bool setOverview = false, bool displayHighestChapter = true)
	{
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false);
		UIRAFProgramScreen.Get().SetVisible(false);
		if (UISeasonsPanel.Get().IsVisible())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_LastTimeNavbuttonClicked == Time.time)
		{
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		m_LastTimeNavbuttonClicked = Time.time;
		SetNavButtonSelected(m_SeasonBtn);
		UISeasonsPanel.Get().SetVisible(true, setOverview, displayHighestChapter);
		UINewUserFlowManager.OnSeasonsTabClicked();
	}

	public void SeasonsBtnClicked(BaseEventData data)
	{
		if (data != null)
		{
			ClientGameManager.Get().SendUIActionNotification("SeasonsBtnClicked");
		}
		DoSeasonsBtnClicked();
	}

	public void LootMatrixBtnClicked(BaseEventData data)
	{
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false);
		UIRAFProgramScreen.Get().SetVisible(false);
		if (UILootMatrixScreen.Get().IsVisible)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_LastTimeNavbuttonClicked == Time.time)
		{
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		m_LastTimeNavbuttonClicked = Time.time;
		SetNavButtonSelected(m_LootMatrixBtn);
		UILootMatrixScreen.Get().SetVisible(true);
		if (int.TryParse(m_NewLootMatrixText.text, out m_lastSeenNumberOfLootMatrices))
		{
			m_lastSeenNumberOfLootMatrices = int.Parse(m_NewLootMatrixText.text);
		}
		else
		{
			m_lastSeenNumberOfLootMatrices = 0;
		}
		UIManager.SetGameObjectActive(m_LootMatrixNewContainer.GetComponentInChildren<Animator>(true), false);
	}

	public void CollectionsBtnClicked(BaseEventData data)
	{
		if (m_LastTimeNavbuttonClicked == Time.time)
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
		m_LastTimeNavbuttonClicked = Time.time;
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false);
		UIRAFProgramScreen.Get().SetVisible(false);
		if (UIStorePanel.Get().IsStoreOpen())
		{
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
		SetNavButtonSelected(m_CollectionBtn);
		if (data != null)
		{
			ClientGameManager.Get().SendUIActionNotification("CollectionsBtnClicked");
		}
		UIStorePanel.Get().OpenStore();
	}

	public void ToggleReferAFriend()
	{
		if (UIRAFProgramScreen.Get().IsVisible)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (m_currentNavBtn == m_PlayBtn)
					{
						SetPlayMenuCatgeoryVisible(true);
					}
					UIRAFProgramScreen.Get().SetVisible(false);
					return;
				}
			}
		}
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false);
		SetPlayMenuCatgeoryVisible(false);
		UIRAFProgramScreen.Get().SetVisible(true);
	}

	public void ReferAFriendBtnClicked(BaseEventData data)
	{
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false);
		if (UIRAFProgramScreen.Get().IsVisible)
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
		SetPlayMenuCatgeoryVisible(false);
		UIRAFProgramScreen.Get().SetVisible(true);
	}

	public void CashShopBtnClicked(BaseEventData data)
	{
		if (m_LastTimeNavbuttonClicked == Time.time)
		{
			return;
		}
		m_LastTimeNavbuttonClicked = Time.time;
		if (UIGGBoostPurchaseScreen.Get() != null)
		{
			UIGGBoostPurchaseScreen.Get().SetVisible(false);
		}
		if (UIPlayerProgressPanel.Get() != null)
		{
			UIPlayerProgressPanel.Get().SetVisible(false);
		}
		if (UIRAFProgramScreen.Get() != null)
		{
			UIRAFProgramScreen.Get().SetVisible(false);
		}
		if (UICashShopPanel.Get().IsVisible())
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
		SetNavButtonSelected(m_CashShopBtn);
		UIManager.SetGameObjectActive(m_CashShopNewContainer, false);
		UICashShopPanel.Get().SetVisible(true);
	}

	private void MicrophoneClicked(BaseEventData data)
	{
		m_voiceListMenu.SetVisible(!m_voiceListMenu.IsVisible());
	}

	private void DiscordOnJoined()
	{
		UIManager.SetGameObjectActive(m_microphoneConnectedBtn, true);
		UIManager.SetGameObjectActive(m_microphoneOfflineBtn, false);
	}

	private void DiscordOnError(ErrorEventArgs e)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Cancel);
	}

	private void DiscordOnDisconnected()
	{
		UIManager.SetGameObjectActive(m_microphoneConnectedBtn, false);
		UIManager.SetGameObjectActive(m_microphoneOfflineBtn, true);
	}

	private void CloseCurrentTabPanel()
	{
		if (m_currentNavBtn == m_landingPageBtn)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					UIFrontEnd.Get().m_landingPageScreen.SetVisible(false);
					return;
				}
			}
		}
		if (m_currentNavBtn == m_PlayBtn)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					{
						SetPlayMenuCatgeoryVisible(false);
						UICharacterSelectWorldObjects.Get().SetVisible(false);
						UINewUserFlowManager.OnDoneWithReadyButton();
						if (UIRankedModeSelectScreen.Get() != null)
						{
							UIRankedModeSelectScreen.Get().SetVisible(false);
						}
						if (AppState.GetCurrent() == AppState_CreateGame.Get())
						{
							UICreateGameScreen.Get().SetVisible(false);
						}
						else if (AppState.GetCurrent() == AppState_JoinGame.Get())
						{
							UIJoinGameScreen.Get().SetVisible(false);
						}
						if (UICharacterSelectScreenController.Get() != null)
						{
							if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
							{
								if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
								{
									goto IL_0145;
								}
							}
							UICharacterSelectScreenController.Get().SetVisible(false);
						}
						goto IL_0145;
					}
					IL_0145:
					UICharacterScreen.Get().DoRefreshFunctions(128);
					return;
				}
			}
		}
		if (m_currentNavBtn == m_CollectionBtn)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIStorePanel.Get().CloseStore();
					return;
				}
			}
		}
		if (m_currentNavBtn == m_CashShopBtn)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					UICashShopPanel.Get().SetVisible(false);
					return;
				}
			}
		}
		if (m_currentNavBtn == m_SeasonBtn)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					UISeasonsPanel.Get().SetVisible(false);
					return;
				}
			}
		}
		if (!(m_currentNavBtn == m_LootMatrixBtn))
		{
			return;
		}
		while (true)
		{
			UILootMatrixScreen.Get().SetVisible(false);
			return;
		}
	}

	public void SetNavButtonSelected(_SelectableBtn btn)
	{
		if (m_currentNavBtn == btn)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_currentNavBtn != null)
		{
			CloseCurrentTabPanel();
			m_currentNavBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		UIManager.SetGameObjectActive(m_PlayButtonNoticeContainer, m_PlayBtn != btn);
		if (btn != null)
		{
			btn.SetSelected(true, false, string.Empty, string.Empty);
		}
		m_previousNavBtn = m_currentNavBtn;
		m_currentNavBtn = btn;
		m_gamePadHoverBtn = btn;
	}

	public void ReturnToPreviousTab()
	{
		if (!(m_previousNavBtn != null))
		{
			return;
		}
		while (true)
		{
			m_previousNavBtn.spriteController.callback(null);
			return;
		}
	}

	public void SetPlayMenuCatgeoryVisible(bool visible)
	{
		m_playMenuCatgeory.SetVisible(visible);
	}

	public void NotifyCurrentPanelLoseFocus()
	{
		if (m_currentNavBtn == m_landingPageBtn)
		{
			UILandingPageScreen.Get().SetVisible(false);
		}
		else
		{
			if (m_currentNavBtn == m_PlayBtn)
			{
				SetPlayMenuCatgeoryVisible(false);
				UICharacterSelectWorldObjects.Get().SetVisible(false);
				UINewUserFlowManager.OnDoneWithReadyButton();
				if (UIRankedModeSelectScreen.Get() != null)
				{
					UIRankedModeSelectScreen.Get().SetVisible(false);
				}
				if (AppState.GetCurrent() == AppState_CreateGame.Get())
				{
					UICreateGameScreen.Get().SetVisible(false);
				}
				else if (AppState.GetCurrent() == AppState_JoinGame.Get())
				{
					UIJoinGameScreen.Get().SetVisible(false);
				}
				else if (AppState.GetCurrent() == AppState_RankModeDraft.Get())
				{
					UIRankedModeDraftScreen.Get().m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 0f;
				}
				if (UICharacterSelectScreenController.Get() != null)
				{
					if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
					{
						if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
						{
							goto IL_018a;
						}
					}
					UICharacterSelectScreenController.Get().SetVisible(false);
				}
				goto IL_018a;
			}
			if (m_currentNavBtn == m_CollectionBtn)
			{
				UIStorePanel.Get().NotifyLoseFocus();
			}
			else if (m_currentNavBtn == m_SeasonBtn)
			{
				UISeasonsPanel.Get().NotifyLoseFocus();
			}
			else if (m_currentNavBtn == m_LootMatrixBtn)
			{
				UILootMatrixScreen.Get().NotifyLoseFocus();
			}
			else if (m_currentNavBtn == m_CashShopBtn)
			{
				UICashShopPanel.Get().NotifyLoseFocus();
			}
		}
		goto IL_0247;
		IL_0247:
		if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
		{
			if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
			{
				return;
			}
		}
		UICharacterSelectWorldObjects.Get().SetVisible(false);
		return;
		IL_018a:
		UICharacterScreen.Get().DoRefreshFunctions(128);
		goto IL_0247;
	}

	public void NotifyCurrentPanelGetFocus()
	{
		if (m_currentNavBtn == m_landingPageBtn)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UILandingPageScreen.Get().SetVisible(true);
					return;
				}
			}
		}
		if (m_currentNavBtn == m_PlayBtn)
		{
			if (AppState.GetCurrent() == AppState_CreateGame.Get())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						UICreateGameScreen.Get().SetVisible(true);
						return;
					}
				}
			}
			if (AppState.GetCurrent() == AppState_JoinGame.Get())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						UIJoinGameScreen.Get().SetVisible(true);
						return;
					}
				}
			}
			if (AppState.GetCurrent() == AppState_RankModeDraft.Get())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						UIRankedModeDraftScreen.Get().m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 1f;
						return;
					}
				}
			}
			if (GameManager.Get() != null)
			{
				if (GameManager.Get().GameInfo != null)
				{
					if (GameManager.Get().GameInfo.IsCustomGame && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								UICharacterSelectWorldObjects.Get().SetVisible(true);
								UICharacterSelectScreenController.Get().SetVisible(true);
								return;
							}
						}
					}
				}
			}
			SetPlayMenuCatgeoryVisible(true);
			if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
			{
				if (!UIMatchStartPanel.Get().IsVisible())
				{
					AppState_GroupCharacterSelect.Get().Enter();
					return;
				}
			}
			AppState_CharacterSelect.Get().Enter();
			return;
		}
		if (m_currentNavBtn == m_CollectionBtn)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					UIStorePanel.Get().NotifyGetFocus();
					return;
				}
			}
		}
		if (m_currentNavBtn == m_SeasonBtn)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UISeasonsPanel.Get().NotifyGetFocus();
					return;
				}
			}
		}
		if (m_currentNavBtn == m_LootMatrixBtn)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UILootMatrixScreen.Get().NotifyGetFocus();
					return;
				}
			}
		}
		if (!(m_currentNavBtn == m_CashShopBtn))
		{
			return;
		}
		while (true)
		{
			UICashShopPanel.Get().NotifyGetFocus();
			return;
		}
	}

	public void ToggleUiForGameStarting(bool shouldShow)
	{
		int num;
		if (!AppState.IsInGame())
		{
			if (GameManager.Get().GameInfo != null)
			{
				if (GameManager.Get().GameInfo.IsCustomGame)
				{
					num = ((GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped) ? 1 : 0);
					goto IL_0072;
				}
			}
		}
		num = 0;
		goto IL_0072;
		IL_0072:
		bool flag = (byte)num != 0;
		_SelectableBtn exitCustomGamesBtn = m_exitCustomGamesBtn;
		int doActive;
		if (!shouldShow)
		{
			doActive = (flag ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(exitCustomGamesBtn, (byte)doActive != 0);
		UIManager.SetGameObjectActive(m_landingPageBtn, shouldShow);
		UIManager.SetGameObjectActive(m_PlayBtn, shouldShow);
		UIManager.SetGameObjectActive(m_CollectionBtn, shouldShow);
		UIManager.SetGameObjectActive(m_SeasonBtn, shouldShow);
		UIManager.SetGameObjectActive(m_LootMatrixBtn, shouldShow);
		UIManager.SetGameObjectActive(m_WatchBtn, false);
		UIManager.SetGameObjectActive(m_CashShopBtn, shouldShow);
		SetPlayMenuCatgeoryVisible(shouldShow);
	}
}
