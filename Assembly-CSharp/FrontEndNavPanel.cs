using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
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
			while (true)
			{
				switch (3)
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
			OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		int num = 0;
		int value = 0;
		int value2 = 0;
		int num2 = 0;
		if (ClientGameManager.Get() != null)
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
			if (ClientGameManager.Get().PlayerWallet != null)
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
				CurrencyWallet playerWallet = ClientGameManager.Get().PlayerWallet;
				num = playerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
				value = playerWallet.GetCurrentAmount(CurrencyType.ISO);
				value2 = playerWallet.GetCurrentAmount(CurrencyType.RankedCurrency);
				num2 = playerWallet.GetCurrentAmount(CurrencyType.UnlockFreelancerToken);
			}
		}
		m_freelancerCurrencyText.text = "<sprite name=credit>" + UIStorePanel.FormatIntToString(num, true);
		UITooltipHoverObject component = m_freelancerCurrencyText.GetComponent<UITooltipHoverObject>();
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip3 = (UISimpleTooltip)tooltip;
				uISimpleTooltip3.Setup(StringUtil.TR("FreelancerCurrencyDesc", "Global"));
				return true;
			};
		}
		component.Setup(TooltipType.Simple, _003C_003Ef__am_0024cache0);
		m_isoText.text = "<sprite name=iso>" + UIStorePanel.FormatIntToString(value, true);
		UITooltipHoverObject component2 = m_isoText.GetComponent<UITooltipHoverObject>();
		if (_003C_003Ef__am_0024cache1 == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache1 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip2 = (UISimpleTooltip)tooltip;
				uISimpleTooltip2.Setup(StringUtil.TR("ISODescription", "Global"));
				return true;
			};
		}
		component2.Setup(TooltipType.Simple, _003C_003Ef__am_0024cache1);
		m_prestigeText.text = "<sprite name=rankedCurrency>" + UIStorePanel.FormatIntToString(value2, true);
		UITooltipHoverObject component3 = m_prestigeText.GetComponent<UITooltipHoverObject>();
		if (_003C_003Ef__am_0024cache2 == null)
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
			_003C_003Ef__am_0024cache2 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uISimpleTooltip = (UISimpleTooltip)tooltip;
				uISimpleTooltip.Setup(StringUtil.TR("RankedCurrencyDescription", "Global"));
				return true;
			};
		}
		component3.Setup(TooltipType.Simple, _003C_003Ef__am_0024cache2);
		if (num > 0)
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
			UINewUserFlowManager.OnFreelancerCurrencyOwned();
		}
		if (num2 > 0)
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
			switch (5)
			{
			case 0:
				continue;
			}
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (UIFrontendLoadingScreen.Get().IsVisible() || !base.gameObject.activeInHierarchy)
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
				if (!(Options_UI.Get() == null))
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
					if (Options_UI.Get().IsVisible())
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
						break;
					}
				}
				if (!(KeyBinding_UI.Get() == null))
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
					if (KeyBinding_UI.Get().IsVisible())
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
						break;
					}
				}
				int num;
				if (DebugParameters.Get() != null)
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
					switch (3)
					{
					case 0:
						continue;
					}
					if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Seasons_FE_Nav))
					{
						if (GameManager.Get() != null)
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
							if (GameManager.Get().GameplayOverrides != null && GameManager.Get().GameplayOverrides.EnableSeasons)
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
								if (m_SeasonBtn.spriteController.IsClickable())
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
									UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
									SeasonsBtnClicked(null);
								}
							}
						}
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Collection_FE_Nav))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (GameManager.Get() != null)
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
							if (GameManager.Get().GameplayOverrides != null)
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
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
						LootMatrixBtnClicked(null);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.RecruitAFriend_FE_Nav))
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
						UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
						ReferAFriendBtnClicked(null);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Profile_FE_Nav))
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
						UIFrontEnd.Get().TogglePlayerProgressScreenVisibility();
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.FreelancerStats_FE_Nav))
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
						UIPlayerProgressPanel uIPlayerProgressPanel = UIPlayerProgressPanel.Get();
						uIPlayerProgressPanel.SetVisible(true, false);
						uIPlayerProgressPanel.NotifyMenuButtonClicked(uIPlayerProgressPanel.m_stats);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Match_History_Replays_FE_Nav))
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
						switch (5)
						{
						case 0:
							continue;
						}
						if (GameManager.Get().GameplayOverrides.DisableControlPadInput || m_playMenuCatgeory.IsVisible())
						{
							return;
						}
						if (Input.GetButtonDown("GamepadButtonLeftShoulder"))
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
							if (m_gamePadHoverBtn == m_landingPageBtn)
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
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								m_gamePadHoverBtn = m_SeasonBtn;
							}
							else if (m_gamePadHoverBtn == m_LootMatrixBtn)
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
								m_gamePadHoverBtn = m_CollectionBtn;
							}
							m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
						}
						else if (Input.GetButtonDown("GamepadButtonRightShoulder"))
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
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
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
							switch (3)
							{
							case 0:
								continue;
							}
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
									switch (6)
									{
									case 0:
										continue;
									}
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
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						if (!m_landingPageBtn.isActiveAndEnabled)
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
			while (true)
			{
				switch (3)
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
			using (Dictionary<int, QuestProgress>.Enumerator enumerator = accountData.QuestComponent.Progress.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int key = enumerator.Current.Key;
					if (QuestWideData.Get().IsDailyQuest(key))
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
						num++;
					}
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (ClientGameManager.Get().AlertMissionsData != null)
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
			if (ClientGameManager.Get().AlertMissionsData.CurrentAlert != null)
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
				UIManager.SetGameObjectActive(m_alertActiveIcon, true);
				if (ClientGameManager.Get().AlertMissionsData.CurrentAlert.Type == AlertMissionType.Quest)
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_freelancerCurrencyText.text = "<sprite name=credit>" + UIStorePanel.FormatIntToString(newBalance.Amount, true);
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
					m_isoText.text = "<sprite name=iso>" + UIStorePanel.FormatIntToString(newBalance.Amount, true);
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
					m_prestigeText.text = "<sprite name=rankedCurrency>" + UIStorePanel.FormatIntToString(newBalance.Amount, true);
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
			switch (4)
			{
			case 0:
				continue;
			}
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
				UIManager.SetGameObjectActive(componentsInChildren[i], visible);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (visible || !(UIStorePanel.Get() != null))
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
				if (UIStorePanel.Get().IsStoreOpen())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
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
				UIManager.SetGameObjectActive(componentsInChildren[i], visible);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (!visible)
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
				if (UISeasonsPanel.Get() != null)
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
					if (UISeasonsPanel.Get().IsVisible())
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
			while (true)
			{
				switch (6)
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
			if (ClientGameManager.Get() != null)
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
			while (true)
			{
				switch (6)
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
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (clientGameManager.GetPlayerAccountData().AccountComponent.DailyQuestsAvailable)
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
			while (true)
			{
				switch (3)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (visible)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (UIMainMenu.Get() != null)
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
			while (true)
			{
				switch (3)
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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (UIStorePanel.Get() == null || UILootMatrixScreen.Get() == null || UIRAFProgramScreen.Get() == null)
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
				if (UIPlayerProgressPanel.Get() == null)
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
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!UIStorePanel.Get().IsStoreOpen())
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
							if (!UISeasonsPanel.Get().IsVisible())
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
								if (!UILootMatrixScreen.Get().IsVisible && AppState_LandingPage.Get() == AppState.GetCurrent())
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
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
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
				switch (6)
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
		m_LastTimeNavbuttonClicked = Time.time;
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false);
		UIRAFProgramScreen.Get().SetVisible(false);
		UINewUserFlowManager.OnGameModeButtonDisplayed();
		if (!UICashShopPanel.Get().IsVisible())
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
			if (!UIStorePanel.Get().IsStoreOpen())
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
				if (!UISeasonsPanel.Get().IsVisible())
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
					if (!UILootMatrixScreen.Get().IsVisible)
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
						if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
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
							if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
							{
								goto IL_0139;
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			while (true)
			{
				switch (5)
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
			UIGGBoostPurchaseScreen.Get().SetVisible(false);
		}
		if (UIPlayerProgressPanel.Get() != null)
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
			UIPlayerProgressPanel.Get().SetVisible(false);
		}
		if (UIRAFProgramScreen.Get() != null)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							UIJoinGameScreen.Get().SetVisible(false);
						}
						if (UICharacterSelectScreenController.Get() != null)
						{
							if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
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
								if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
								{
									goto IL_0145;
								}
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
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
			switch (3)
			{
			case 0:
				continue;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (m_currentNavBtn != null)
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
			CloseCurrentTabPanel();
			m_currentNavBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		UIManager.SetGameObjectActive(m_PlayButtonNoticeContainer, m_PlayBtn != btn);
		if (btn != null)
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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			UILandingPageScreen.Get().SetVisible(false);
		}
		else
		{
			if (m_currentNavBtn == m_PlayBtn)
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
				SetPlayMenuCatgeoryVisible(false);
				UICharacterSelectWorldObjects.Get().SetVisible(false);
				UINewUserFlowManager.OnDoneWithReadyButton();
				if (UIRankedModeSelectScreen.Get() != null)
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
					UIRankedModeSelectScreen.Get().SetVisible(false);
				}
				if (AppState.GetCurrent() == AppState_CreateGame.Get())
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
					UICreateGameScreen.Get().SetVisible(false);
				}
				else if (AppState.GetCurrent() == AppState_JoinGame.Get())
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
					UIJoinGameScreen.Get().SetVisible(false);
				}
				else if (AppState.GetCurrent() == AppState_RankModeDraft.Get())
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
					UIRankedModeDraftScreen.Get().m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 0f;
				}
				if (UICharacterSelectScreenController.Get() != null)
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
					if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
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
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				UIStorePanel.Get().NotifyLoseFocus();
			}
			else if (m_currentNavBtn == m_SeasonBtn)
			{
				UISeasonsPanel.Get().NotifyLoseFocus();
			}
			else if (m_currentNavBtn == m_LootMatrixBtn)
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
				UILootMatrixScreen.Get().NotifyLoseFocus();
			}
			else if (m_currentNavBtn == m_CashShopBtn)
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
				UICashShopPanel.Get().NotifyLoseFocus();
			}
		}
		goto IL_0247;
		IL_0247:
		if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (GameManager.Get().GameInfo != null)
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
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!UIMatchStartPanel.Get().IsVisible())
				{
					AppState_GroupCharacterSelect.Get().Enter();
					return;
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
			switch (2)
			{
			case 0:
				continue;
			}
			UICashShopPanel.Get().NotifyGetFocus();
			return;
		}
	}

	public void ToggleUiForGameStarting(bool shouldShow)
	{
		int num;
		if (!AppState.IsInGame())
		{
			while (true)
			{
				switch (5)
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
			if (GameManager.Get().GameInfo != null)
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
				if (GameManager.Get().GameInfo.IsCustomGame)
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
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
