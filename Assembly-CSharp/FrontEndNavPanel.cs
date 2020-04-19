using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
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
		return FrontEndNavPanel.s_instance;
	}

	private void Awake()
	{
		FrontEndNavPanel.s_instance = this;
		this.m_PlayBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PlayBtnClicked);
		this.m_CollectionBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CollectionsBtnClicked);
		this.m_CashShopBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CashShopBtnClicked);
		this.m_landingPageBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LandingPageBtnClicked);
		this.m_SeasonBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SeasonsBtnClicked);
		this.m_LootMatrixBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LootMatrixBtnClicked);
		this.m_notificationsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NotificationBtnClicked);
		this.m_menuBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.MenuBtnClicked);
		this.m_exitCustomGamesBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LandingPageBtnClicked);
		this.m_microphoneConnectedBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.MicrophoneClicked);
		this.m_microphoneOfflineBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.MicrophoneClicked);
		this.m_PlayBtn.spriteController.SetSelectableBtn(this.m_PlayBtn);
		this.m_CollectionBtn.spriteController.SetSelectableBtn(this.m_CollectionBtn);
		this.m_CashShopBtn.spriteController.SetSelectableBtn(this.m_CashShopBtn);
		this.m_landingPageBtn.spriteController.SetSelectableBtn(this.m_landingPageBtn);
		this.m_SeasonBtn.spriteController.SetSelectableBtn(this.m_SeasonBtn);
		this.m_LootMatrixBtn.spriteController.SetSelectableBtn(this.m_LootMatrixBtn);
		UIEventTriggerUtils.AddListener(this.m_LimitedModeHitbox.gameObject, EventTriggerType.PointerEnter, delegate(BaseEventData data)
		{
			UIManager.SetGameObjectActive(this.m_LimitedModeTooltip, true, null);
		});
		UIEventTriggerUtils.AddListener(this.m_LimitedModeHitbox.gameObject, EventTriggerType.PointerExit, delegate(BaseEventData data)
		{
			UIManager.SetGameObjectActive(this.m_LimitedModeTooltip, false, null);
		});
		UIManager.SetGameObjectActive(this.m_LimitedModeTooltip, false, null);
		this.m_notificationsBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.PopulateContractsTooltip), null);
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.Awake()).MethodHandle;
			}
			this.OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
			this.OnInventoryDataUpdated(ClientGameManager.Get().GetPlayerAccountData().InventoryComponent);
			this.CheckNewCashShopFeaturedItems();
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_LootMatrixNewContainer.GetComponentInChildren<Animator>(true), false, null);
			UIManager.SetGameObjectActive(this.m_CashShopNewContainer, false, null);
		}
		this.m_PlayBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		this.m_CollectionBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		this.m_CashShopBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		this.m_SeasonBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		this.m_LootMatrixBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		this.m_landingPageBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.TopMenuSelect;
		this.m_menuBtnList.Add(this.m_PlayBtn);
		this.m_menuBtnList.Add(this.m_CollectionBtn);
		this.m_menuBtnList.Add(this.m_CashShopBtn);
		this.m_menuBtnList.Add(this.m_landingPageBtn);
		this.m_menuBtnList.Add(this.m_SeasonBtn);
		this.m_menuBtnList.Add(this.m_LootMatrixBtn);
		this.m_menuBtn.SetSelected(false, false, string.Empty, string.Empty);
	}

	private void Start()
	{
		ClientGameManager.Get().OnLobbyServerReadyNotification += this.HandleLobbyServerReadyNotification;
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
		ClientGameManager.Get().OnInventoryComponentUpdated += this.OnInventoryDataUpdated;
		ClientGameManager.Get().OnLobbyServerClientAccessLevelChange += this.HandleLobbyServerClientAccessLevelChange;
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.HandleLobbyGameplayOverridesChange;
		ClientGameManager.Get().OnBankBalanceChange += this.HandleBankBalanceChange;
		ClientGameManager.Get().OnAlertMissionDataChange += this.HandleAlertMissionDataChange;
		this.HandleLobbyServerClientAccessLevelChange(ClientGameManager.Get().ClientAccessLevel, ClientGameManager.Get().ClientAccessLevel);
		ClientGameManager.Get().QueryPlayerMatchData(new Action<PlayerMatchDataResponse>(this.HandlePlayerMatchDataResponse));
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.Start()).MethodHandle;
			}
			this.OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		int num = 0;
		int value = 0;
		int value2 = 0;
		int num2 = 0;
		if (ClientGameManager.Get() != null)
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
			if (ClientGameManager.Get().PlayerWallet != null)
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
				CurrencyWallet playerWallet = ClientGameManager.Get().PlayerWallet;
				num = playerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
				value = playerWallet.GetCurrentAmount(CurrencyType.ISO);
				value2 = playerWallet.GetCurrentAmount(CurrencyType.RankedCurrency);
				num2 = playerWallet.GetCurrentAmount(CurrencyType.UnlockFreelancerToken);
			}
		}
		this.m_freelancerCurrencyText.text = "<sprite name=credit>" + UIStorePanel.FormatIntToString(num, true);
		UITooltipObject component = this.m_freelancerCurrencyText.GetComponent<UITooltipHoverObject>();
		TooltipType tooltipType = TooltipType.Simple;
		if (FrontEndNavPanel.<>f__am$cache0 == null)
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
			FrontEndNavPanel.<>f__am$cache0 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
				uisimpleTooltip.Setup(StringUtil.TR("FreelancerCurrencyDesc", "Global"));
				return true;
			};
		}
		component.Setup(tooltipType, FrontEndNavPanel.<>f__am$cache0, null);
		this.m_isoText.text = "<sprite name=iso>" + UIStorePanel.FormatIntToString(value, true);
		UITooltipObject component2 = this.m_isoText.GetComponent<UITooltipHoverObject>();
		TooltipType tooltipType2 = TooltipType.Simple;
		if (FrontEndNavPanel.<>f__am$cache1 == null)
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
			FrontEndNavPanel.<>f__am$cache1 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
				uisimpleTooltip.Setup(StringUtil.TR("ISODescription", "Global"));
				return true;
			};
		}
		component2.Setup(tooltipType2, FrontEndNavPanel.<>f__am$cache1, null);
		this.m_prestigeText.text = "<sprite name=rankedCurrency>" + UIStorePanel.FormatIntToString(value2, true);
		UITooltipObject component3 = this.m_prestigeText.GetComponent<UITooltipHoverObject>();
		TooltipType tooltipType3 = TooltipType.Simple;
		if (FrontEndNavPanel.<>f__am$cache2 == null)
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
			FrontEndNavPanel.<>f__am$cache2 = delegate(UITooltipBase tooltip)
			{
				UISimpleTooltip uisimpleTooltip = (UISimpleTooltip)tooltip;
				uisimpleTooltip.Setup(StringUtil.TR("RankedCurrencyDescription", "Global"));
				return true;
			};
		}
		component3.Setup(tooltipType3, FrontEndNavPanel.<>f__am$cache2, null);
		if (num > 0)
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
			UINewUserFlowManager.OnFreelancerCurrencyOwned();
		}
		if (num2 > 0)
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
			UINewUserFlowManager.OnFreelancerTokenOwned();
		}
		UIManager.SetGameObjectActive(this.m_microphoneConnectedBtn, false, null);
		UIManager.SetGameObjectActive(this.m_microphoneOfflineBtn, true, null);
		DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
		discordClientInterface.OnJoined = (Action)Delegate.Combine(discordClientInterface.OnJoined, new Action(this.DiscordOnJoined));
		DiscordClientInterface.Get().OnError += this.DiscordOnError;
		DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
		discordClientInterface2.OnDisconnected = (Action)Delegate.Combine(discordClientInterface2.OnDisconnected, new Action(this.DiscordOnDisconnected));
		this.CheckMicrophoneEnabled();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnLobbyServerReadyNotification -= this.HandleLobbyServerReadyNotification;
			ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdated;
			ClientGameManager.Get().OnInventoryComponentUpdated -= this.OnInventoryDataUpdated;
			ClientGameManager.Get().OnLobbyServerClientAccessLevelChange -= this.HandleLobbyServerClientAccessLevelChange;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.HandleLobbyGameplayOverridesChange;
			ClientGameManager.Get().OnBankBalanceChange -= this.HandleBankBalanceChange;
			ClientGameManager.Get().OnAlertMissionDataChange -= this.HandleAlertMissionDataChange;
		}
		if (DiscordClientInterface.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.OnDestroy()).MethodHandle;
			}
			DiscordClientInterface discordClientInterface = DiscordClientInterface.Get();
			discordClientInterface.OnJoined = (Action)Delegate.Remove(discordClientInterface.OnJoined, new Action(this.DiscordOnJoined));
			DiscordClientInterface.Get().OnError -= this.DiscordOnError;
			DiscordClientInterface discordClientInterface2 = DiscordClientInterface.Get();
			discordClientInterface2.OnDisconnected = (Action)Delegate.Remove(discordClientInterface2.OnDisconnected, new Action(this.DiscordOnDisconnected));
		}
		if (FrontEndNavPanel.s_instance == this)
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
			FrontEndNavPanel.s_instance = null;
		}
	}

	private void Update()
	{
		if (UIFrontendLoadingScreen.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.Update()).MethodHandle;
			}
			if (!UIFrontendLoadingScreen.Get().IsVisible() && base.gameObject.activeInHierarchy)
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
				if (!(Options_UI.Get() == null))
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
					if (Options_UI.Get().IsVisible())
					{
						return;
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
				}
				if (!(KeyBinding_UI.Get() == null))
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
					if (KeyBinding_UI.Get().IsVisible())
					{
						return;
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
				bool flag;
				if (DebugParameters.Get() != null)
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
					flag = DebugParameters.Get().GetParameterAsBool("DebugCamera");
				}
				else
				{
					flag = false;
				}
				if (!flag)
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
					if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Seasons_FE_Nav))
					{
						if (GameManager.Get() != null)
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
							if (GameManager.Get().GameplayOverrides != null && GameManager.Get().GameplayOverrides.EnableSeasons)
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
								if (this.m_SeasonBtn.spriteController.IsClickable())
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
									UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
									this.SeasonsBtnClicked(null);
								}
							}
						}
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Collection_FE_Nav))
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
						if (GameManager.Get() != null)
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
							if (GameManager.Get().GameplayOverrides != null)
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
								if (GameManager.Get().GameplayOverrides.EnableShop)
								{
									UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
									this.CollectionsBtnClicked(null);
								}
							}
						}
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Loot_FE_Nav))
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
						UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
						this.LootMatrixBtnClicked(null);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.RecruitAFriend_FE_Nav))
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
						UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
						this.ReferAFriendBtnClicked(null);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Profile_FE_Nav))
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
						UIFrontEnd.Get().TogglePlayerProgressScreenVisibility(true);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.FreelancerStats_FE_Nav))
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
						UIPlayerProgressPanel uiplayerProgressPanel = UIPlayerProgressPanel.Get();
						uiplayerProgressPanel.SetVisible(true, false);
						uiplayerProgressPanel.NotifyMenuButtonClicked(uiplayerProgressPanel.m_stats);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Match_History_Replays_FE_Nav))
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
						UIPlayerProgressPanel uiplayerProgressPanel2 = UIPlayerProgressPanel.Get();
						uiplayerProgressPanel2.SetVisible(true, false);
						uiplayerProgressPanel2.NotifyMenuButtonClicked(uiplayerProgressPanel2.m_history);
					}
					else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Banner_FE_Nav))
					{
						UIPlayerProgressPanel uiplayerProgressPanel3 = UIPlayerProgressPanel.Get();
						uiplayerProgressPanel3.SetVisible(true, false);
						uiplayerProgressPanel3.NotifyMenuButtonClicked(uiplayerProgressPanel3.m_banner);
					}
					if (GameManager.Get() != null)
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
						if (!GameManager.Get().GameplayOverrides.DisableControlPadInput && !this.m_playMenuCatgeory.IsVisible())
						{
							if (Input.GetButtonDown("GamepadButtonLeftShoulder"))
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
								this.m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
								if (this.m_gamePadHoverBtn == this.m_landingPageBtn)
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
									this.m_gamePadHoverBtn = this.m_LootMatrixBtn;
								}
								else if (this.m_gamePadHoverBtn == this.m_PlayBtn)
								{
									this.m_gamePadHoverBtn = this.m_landingPageBtn;
								}
								else if (this.m_gamePadHoverBtn == this.m_SeasonBtn)
								{
									this.m_gamePadHoverBtn = this.m_PlayBtn;
								}
								else if (this.m_gamePadHoverBtn == this.m_CollectionBtn)
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
									this.m_gamePadHoverBtn = this.m_SeasonBtn;
								}
								else if (this.m_gamePadHoverBtn == this.m_LootMatrixBtn)
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
									this.m_gamePadHoverBtn = this.m_CollectionBtn;
								}
								this.m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
							}
							else if (Input.GetButtonDown("GamepadButtonRightShoulder"))
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
								this.m_gamePadHoverBtn.SetSelected(false, false, string.Empty, string.Empty);
								if (this.m_gamePadHoverBtn == this.m_landingPageBtn)
								{
									this.m_gamePadHoverBtn = this.m_PlayBtn;
								}
								else if (this.m_gamePadHoverBtn == this.m_PlayBtn)
								{
									this.m_gamePadHoverBtn = this.m_SeasonBtn;
								}
								else if (this.m_gamePadHoverBtn == this.m_SeasonBtn)
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
									this.m_gamePadHoverBtn = this.m_CollectionBtn;
								}
								else if (this.m_gamePadHoverBtn == this.m_CollectionBtn)
								{
									this.m_gamePadHoverBtn = this.m_LootMatrixBtn;
								}
								else if (this.m_gamePadHoverBtn == this.m_LootMatrixBtn)
								{
									this.m_gamePadHoverBtn = this.m_landingPageBtn;
								}
								this.m_gamePadHoverBtn.SetSelected(true, false, string.Empty, string.Empty);
							}
							if (Input.GetButtonDown("GamepadButtonA"))
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
								if (this.m_currentNavBtn != this.m_gamePadHoverBtn)
								{
									if (this.m_gamePadHoverBtn == this.m_landingPageBtn)
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
										this.LandingPageBtnClicked(null);
									}
									else if (this.m_gamePadHoverBtn == this.m_PlayBtn)
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
										this.PlayBtnClicked(null);
									}
									else if (this.m_gamePadHoverBtn == this.m_SeasonBtn)
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
										this.SeasonsBtnClicked(null);
									}
									else if (this.m_gamePadHoverBtn == this.m_CollectionBtn)
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
										this.CollectionsBtnClicked(null);
									}
									else if (this.m_gamePadHoverBtn == this.m_LootMatrixBtn)
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
										this.LootMatrixBtnClicked(null);
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public void RefreshUI()
	{
		this.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
		this.CheckSeasonsVisibility();
		this.CheckContractsEnabled();
		this.CheckMicrophoneEnabled();
	}

	private void HandleLobbyServerClientAccessLevelChange(ClientAccessLevel oldLevel, ClientAccessLevel newLevel)
	{
		if (newLevel == ClientAccessLevel.Locked)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.HandleLobbyServerClientAccessLevelChange(ClientAccessLevel, ClientAccessLevel)).MethodHandle;
			}
			if (!this.m_landingPageBtn.isActiveAndEnabled)
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
				if (!this.m_exitCustomGamesBtn.isActiveAndEnabled)
				{
					goto IL_4A;
				}
			}
			this.LandingPageBtnClicked(null);
			IL_4A:
			AppState_GroupCharacterSelect appState_GroupCharacterSelect = AppState_GroupCharacterSelect.Get();
			if (appState_GroupCharacterSelect != null)
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
				if (appState_GroupCharacterSelect.InQueue())
				{
					appState_GroupCharacterSelect.UpdateReadyState(false);
					NavigationBar navigationBar = NavigationBar.Get();
					if (navigationBar != null)
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
						UIManager.SetGameObjectActive(navigationBar.m_cancelBtn, false, null);
						navigationBar.m_cancelBtn.spriteController.SetClickable(false);
						navigationBar.m_searchQueueText.text = string.Empty;
					}
				}
			}
			return;
		}
		Component limitedModeContainer = this.m_LimitedModeContainer;
		bool doActive;
		if (newLevel >= ClientAccessLevel.Full && ClientGameManager.Get() != null)
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
			doActive = !ClientGameManager.Get().HasPurchasedGame;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(limitedModeContainer, doActive, null);
	}

	public void HandleLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		this.RefreshUI();
	}

	public void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
	{
		if (notification.Success)
		{
			this.OnAccountDataUpdated(notification.AccountData);
			ClientGameManager.Get().QueryPlayerMatchData(new Action<PlayerMatchDataResponse>(this.HandlePlayerMatchDataResponse));
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
		bool flag = FrontEndNavPanel.m_lastSeenNumberOfLootMatrices < numLockBoxes;
		Component componentInChildren = this.m_LootMatrixNewContainer.GetComponentInChildren<Animator>(true);
		bool doActive;
		if (numLockBoxes > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.OnInventoryDataUpdated(InventoryComponent)).MethodHandle;
			}
			doActive = flag;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(componentInChildren, doActive, null);
		this.m_NewLootMatrixText.text = numLockBoxes.ToString();
		if (!this.m_NewLootMatrixText)
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
			FrontEndNavPanel.m_lastSeenNumberOfLootMatrices = numLockBoxes;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.OnAccountDataUpdated(PersistedAccountData)).MethodHandle;
			}
			using (Dictionary<int, QuestProgress>.Enumerator enumerator = accountData.QuestComponent.Progress.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, QuestProgress> keyValuePair = enumerator.Current;
					int key = keyValuePair.Key;
					if (QuestWideData.Get().IsDailyQuest(key))
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
						num++;
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
		}
		if (ClientGameManager.Get().AlertMissionsData != null)
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
			if (ClientGameManager.Get().AlertMissionsData.CurrentAlert != null)
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
				UIManager.SetGameObjectActive(this.m_alertActiveIcon, true, null);
				if (ClientGameManager.Get().AlertMissionsData.CurrentAlert.Type == AlertMissionType.Quest)
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
					num++;
				}
				goto IL_112;
			}
		}
		UIManager.SetGameObjectActive(this.m_alertActiveIcon, false, null);
		IL_112:
		this.m_questNotificationNumber.text = num.ToString();
		if (UIFrontEnd.Get() != null)
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
			UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
			UIFrontEnd.Get().m_frontEndNavPanel.CheckContractsEnabled();
		}
		this.CheckNewCashShopFeaturedItems();
	}

	private void HandleAlertMissionDataChange(LobbyAlertMissionDataNotification notification)
	{
		this.OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
	}

	private void HandleBankBalanceChange(CurrencyData newBalance)
	{
		if (newBalance.Type == CurrencyType.FreelancerCurrency)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.HandleBankBalanceChange(CurrencyData)).MethodHandle;
			}
			this.m_freelancerCurrencyText.text = "<sprite name=credit>" + UIStorePanel.FormatIntToString(newBalance.Amount, true);
			if (newBalance.Amount > 0)
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
				UINewUserFlowManager.OnFreelancerCurrencyOwned();
			}
		}
		else if (newBalance.Type == CurrencyType.ISO)
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
			this.m_isoText.text = "<sprite name=iso>" + UIStorePanel.FormatIntToString(newBalance.Amount, true);
		}
		else if (newBalance.Type == CurrencyType.RankedCurrency)
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
			this.m_prestigeText.text = "<sprite name=rankedCurrency>" + UIStorePanel.FormatIntToString(newBalance.Amount, true);
		}
		else if (newBalance.Type == CurrencyType.UnlockFreelancerToken)
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
			if (newBalance.Amount > 0)
			{
				UINewUserFlowManager.OnFreelancerTokenOwned();
			}
		}
	}

	public void SetShopVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_CollectionBtn.spriteController, visible, null);
		CanvasGroup[] componentsInChildren = this.m_CollectionBtn.GetComponentsInChildren<CanvasGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject != this.m_CollectionBtn.gameObject)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.SetShopVisible(bool)).MethodHandle;
				}
				UIManager.SetGameObjectActive(componentsInChildren[i], visible, null);
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!visible && UIStorePanel.Get() != null)
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
			if (UIStorePanel.Get().IsStoreOpen())
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
				this.LandingPageBtnClicked(null);
			}
		}
	}

	public void CheckSeasonsVisibility()
	{
		this.SetSeasonsVisible(UISeasonsPanel.CheckSeasonsVisibility(out this.m_seasonLockoutReason));
	}

	public void SetSeasonsVisible(bool visible)
	{
		this.m_SeasonBtn.spriteController.SetClickable(visible);
		CanvasGroup[] componentsInChildren = this.m_SeasonBtn.GetComponentsInChildren<CanvasGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject != this.m_SeasonBtn.gameObject)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.SetSeasonsVisible(bool)).MethodHandle;
				}
				UIManager.SetGameObjectActive(componentsInChildren[i], visible, null);
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!visible)
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
			if (UISeasonsPanel.Get() != null)
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
				if (UISeasonsPanel.Get().IsVisible())
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
					this.m_SeasonBtn.SetSelected(false, false, string.Empty, string.Empty);
					UISeasonsPanel.Get().SetVisible(false, false, true);
				}
			}
		}
		if (this.m_seasonLockoutReason != SeasonLockoutReason.None)
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
			this.m_SeasonBtn.spriteController.SetForceHovercallback(true);
			this.m_SeasonBtn.spriteController.SetForceExitCallback(true);
			this.m_SeasonBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.PopulateSeasonsTooltip), null);
		}
		else
		{
			this.m_SeasonBtn.spriteController.SetForceHovercallback(false);
			this.m_SeasonBtn.spriteController.SetForceExitCallback(false);
			this.m_SeasonBtn.spriteController.pointerEnterCallback = null;
			this.m_SeasonBtn.spriteController.pointerExitCallback = null;
		}
	}

	private bool PopulateSeasonsTooltip(UITooltipBase tooltip)
	{
		if (this.m_seasonLockoutReason == SeasonLockoutReason.InTutorialSeason)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.PopulateSeasonsTooltip(UITooltipBase)).MethodHandle;
			}
			if (ClientGameManager.Get() != null)
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
				if (ClientGameManager.Get().GetPlayerAccountData() != null)
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
					int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
					int seasonLevel = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.SeasonLevel;
					SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(activeSeason);
					UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
					uititledTooltip.Setup(StringUtil.TR("Locked", "Global"), string.Format(StringUtil.TR("RequiresMatchesPlayed", "Global"), QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, activeSeason) - seasonLevel), string.Empty);
					return true;
				}
			}
		}
		else if (this.m_seasonLockoutReason == SeasonLockoutReason.Disabled)
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
			UITitledTooltip uititledTooltip2 = tooltip as UITitledTooltip;
			uititledTooltip2.Setup(StringUtil.TR("Locked", "Global"), StringUtil.TR("SeasonsDisabled", "Global"), string.Empty);
			return true;
		}
		return false;
	}

	public void CheckContractsEnabled()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		bool flag = false;
		if (clientGameManager != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.CheckContractsEnabled()).MethodHandle;
			}
			if (clientGameManager.IsPlayerAccountDataAvailable())
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
				if (clientGameManager.GetPlayerAccountData().AccountComponent.DailyQuestsAvailable)
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
					flag = true;
				}
			}
		}
		this.m_notificationsBtn.spriteController.SetClickable(flag);
		this.m_notificationsBtn.SetDisabled(!flag);
	}

	public void CheckMicrophoneEnabled()
	{
		UIManager.SetGameObjectActive(this.m_microphoneContainer, DiscordClientInterface.IsEnabled && DiscordClientInterface.IsSdkEnabled, null);
		TMP_Text autoJoinDiscordText = this.m_autoJoinDiscordText;
		string term;
		if (DiscordClientInterface.IsSdkEnabled)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.CheckMicrophoneEnabled()).MethodHandle;
			}
			term = "AutoJoinVoiceBtn";
		}
		else
		{
			term = "JoinDiscordBtn";
		}
		autoJoinDiscordText.text = StringUtil.TR(term, "NewFrontEndScene");
		bool flag = DiscordClientInterface.Get().IsConnected && DiscordClientInterface.Get().ChannelInfo != null;
		if (flag)
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
			this.DiscordOnJoined();
		}
		else
		{
			this.DiscordOnDisconnected();
		}
	}

	private void CheckNewCashShopFeaturedItems()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.CheckNewCashShopFeaturedItems()).MethodHandle;
			}
			if (clientGameManager.IsPlayerAccountDataAvailable())
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
				AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.CashShopFeaturedItemsVersionViewed;
				int uistate = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
				int featuredItemsVersion = StoreWideData.Get().m_featuredItemsVersion;
				UIManager.SetGameObjectActive(this.m_CashShopNewContainer, uistate < featuredItemsVersion, null);
				return;
			}
		}
		UIManager.SetGameObjectActive(this.m_CashShopNewContainer, false, null);
	}

	private bool PopulateContractsTooltip(UITooltipBase tooltip)
	{
		if (this.m_notificationsBtn.spriteController.IsClickable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.PopulateContractsTooltip(UITooltipBase)).MethodHandle;
			}
			return false;
		}
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(StringUtil.TR("Locked", "Global"), string.Format(StringUtil.TR("DailyQuestsUnlockRequirements", "Quests"), new object[0]), string.Empty);
		return true;
	}

	public void SetLootMatrixVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_LootMatrixBtn.spriteController, visible, null);
		CanvasGroup[] componentsInChildren = this.m_LootMatrixBtn.GetComponentsInChildren<CanvasGroup>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (componentsInChildren[i].gameObject != this.m_LootMatrixBtn.gameObject)
			{
				UIManager.SetGameObjectActive(componentsInChildren[i], visible, null);
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.SetLootMatrixVisible(bool)).MethodHandle;
		}
		if (!visible)
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
			if (UILootMatrixScreen.Get().IsVisible)
			{
				this.m_LootMatrixBtn.SetSelected(false, false, string.Empty, string.Empty);
				UILootMatrixScreen.Get().SetVisible(false);
			}
		}
	}

	public void NotifyGroupUpdate()
	{
	}

	public void NotificationBtnClicked(BaseEventData data)
	{
		if (!this.m_notificationsBtn.spriteController.IsClickable())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.NotificationBtnClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		if (UIMainMenu.Get() != null)
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
			if (UIMainMenu.Get().IsOpen())
			{
				UIMainMenu.Get().SetMenuVisible(false, false);
			}
		}
		if (this.m_notificationsBtn.IsSelected())
		{
			QuestListPanel.Get().SetVisible(false, false, false);
			this.m_notificationsBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		else
		{
			QuestListPanel.Get().SetVisible(true, false, false);
			this.m_notificationsBtn.SetSelected(true, false, string.Empty, string.Empty);
		}
	}

	public void MenuBtnClicked(BaseEventData data)
	{
		if (!UIMainMenu.Get().IsOpen())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.MenuBtnClicked(BaseEventData)).MethodHandle;
			}
			if (this.m_notificationsBtn.IsSelected())
			{
				QuestListPanel.Get().SetVisible(false, false, false);
				this.m_notificationsBtn.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		UIMainMenu.Get().SetMenuVisible(!UIMainMenu.Get().IsOpen(), false);
	}

	public void LandingPageBtnClicked(BaseEventData data)
	{
		if (!(UIFrontEnd.Get() == null))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.LandingPageBtnClicked(BaseEventData)).MethodHandle;
			}
			if (!(UIStorePanel.Get() == null) && !(UILootMatrixScreen.Get() == null) && !(UIRAFProgramScreen.Get() == null))
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
				if (!(UIPlayerProgressPanel.Get() == null))
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
					if (UISeasonsPanel.Get() == null)
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
						UIGGBoostPurchaseScreen.Get().SetVisible(false);
						UIPlayerProgressPanel.Get().SetVisible(false, true);
						UIRAFProgramScreen.Get().SetVisible(false);
						if (!UICashShopPanel.Get().IsVisible())
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
							if (!UIStorePanel.Get().IsStoreOpen())
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
								if (!UISeasonsPanel.Get().IsVisible())
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
									if (!UILootMatrixScreen.Get().IsVisible && AppState_LandingPage.Get() == AppState.GetCurrent())
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
										if (this.m_currentNavBtn == this.m_landingPageBtn)
										{
											return;
										}
									}
								}
							}
						}
						if (UIGameSettingsPanel.Get() != null)
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
							UIGameSettingsPanel.Get().SetVisible(false);
						}
						if (this.m_LastTimeNavbuttonClicked == Time.time)
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
							return;
						}
						this.m_LastTimeNavbuttonClicked = Time.time;
						this.SetNavButtonSelected(this.m_landingPageBtn);
						if (AppState_LandingPage.Get() != AppState.GetCurrent())
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
							if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
							{
								AppState_GameTeardown.Get().Enter();
							}
							else
							{
								AppState_LandingPage.Get().Enter(true);
								this.SetPlayMenuCatgeoryVisible(false);
							}
						}
						else
						{
							UIFrontEnd.Get().m_landingPageScreen.SetVisible(true);
						}
						return;
					}
				}
			}
		}
	}

	public void PlayBtnClicked(BaseEventData data)
	{
		if (this.m_LastTimeNavbuttonClicked == Time.time)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.PlayBtnClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		this.m_LastTimeNavbuttonClicked = Time.time;
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false, true);
		UIRAFProgramScreen.Get().SetVisible(false);
		UINewUserFlowManager.OnGameModeButtonDisplayed();
		if (!UICashShopPanel.Get().IsVisible())
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
			if (!UIStorePanel.Get().IsStoreOpen())
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
				if (!UISeasonsPanel.Get().IsVisible())
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
					if (!UILootMatrixScreen.Get().IsVisible)
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
						if (!(AppState_CharacterSelect.Get() == AppState.GetCurrent()))
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
							if (!(AppState_GroupCharacterSelect.Get() == AppState.GetCurrent()))
							{
								goto IL_139;
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
						}
						if (this.m_currentNavBtn == this.m_PlayBtn)
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
							if (!this.m_playMenuCatgeory.IsVisible())
							{
								this.SetPlayMenuCatgeoryVisible(true);
							}
							return;
						}
					}
				}
			}
		}
		IL_139:
		this.SetNavButtonSelected(this.m_PlayBtn);
		this.SetPlayMenuCatgeoryVisible(true);
		UIFrontEnd.Get().m_landingPageScreen.QuickPlayButtonClicked(data);
	}

	public void DoSeasonsBtnClicked(bool setOverview = false, bool displayHighestChapter = true)
	{
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false, true);
		UIRAFProgramScreen.Get().SetVisible(false);
		if (UISeasonsPanel.Get().IsVisible())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.DoSeasonsBtnClicked(bool, bool)).MethodHandle;
			}
			return;
		}
		if (this.m_LastTimeNavbuttonClicked == Time.time)
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
			return;
		}
		this.m_LastTimeNavbuttonClicked = Time.time;
		this.SetNavButtonSelected(this.m_SeasonBtn);
		UISeasonsPanel.Get().SetVisible(true, setOverview, displayHighestChapter);
		UINewUserFlowManager.OnSeasonsTabClicked();
	}

	public void SeasonsBtnClicked(BaseEventData data)
	{
		if (data != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.SeasonsBtnClicked(BaseEventData)).MethodHandle;
			}
			ClientGameManager.Get().SendUIActionNotification("SeasonsBtnClicked");
		}
		this.DoSeasonsBtnClicked(false, true);
	}

	public void LootMatrixBtnClicked(BaseEventData data)
	{
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false, true);
		UIRAFProgramScreen.Get().SetVisible(false);
		if (UILootMatrixScreen.Get().IsVisible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.LootMatrixBtnClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		if (this.m_LastTimeNavbuttonClicked == Time.time)
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
			return;
		}
		this.m_LastTimeNavbuttonClicked = Time.time;
		this.SetNavButtonSelected(this.m_LootMatrixBtn);
		UILootMatrixScreen.Get().SetVisible(true);
		if (int.TryParse(this.m_NewLootMatrixText.text, out FrontEndNavPanel.m_lastSeenNumberOfLootMatrices))
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
			FrontEndNavPanel.m_lastSeenNumberOfLootMatrices = int.Parse(this.m_NewLootMatrixText.text);
		}
		else
		{
			FrontEndNavPanel.m_lastSeenNumberOfLootMatrices = 0;
		}
		UIManager.SetGameObjectActive(this.m_LootMatrixNewContainer.GetComponentInChildren<Animator>(true), false, null);
	}

	public void CollectionsBtnClicked(BaseEventData data)
	{
		if (this.m_LastTimeNavbuttonClicked == Time.time)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.CollectionsBtnClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		this.m_LastTimeNavbuttonClicked = Time.time;
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false, true);
		UIRAFProgramScreen.Get().SetVisible(false);
		if (UIStorePanel.Get().IsStoreOpen())
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
			return;
		}
		this.SetNavButtonSelected(this.m_CollectionBtn);
		if (data != null)
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
			ClientGameManager.Get().SendUIActionNotification("CollectionsBtnClicked");
		}
		UIStorePanel.Get().OpenStore();
	}

	public void ToggleReferAFriend()
	{
		if (UIRAFProgramScreen.Get().IsVisible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.ToggleReferAFriend()).MethodHandle;
			}
			if (this.m_currentNavBtn == this.m_PlayBtn)
			{
				this.SetPlayMenuCatgeoryVisible(true);
			}
			UIRAFProgramScreen.Get().SetVisible(false);
		}
		else
		{
			UIGGBoostPurchaseScreen.Get().SetVisible(false);
			UIPlayerProgressPanel.Get().SetVisible(false, true);
			this.SetPlayMenuCatgeoryVisible(false);
			UIRAFProgramScreen.Get().SetVisible(true);
		}
	}

	public void ReferAFriendBtnClicked(BaseEventData data)
	{
		UIGGBoostPurchaseScreen.Get().SetVisible(false);
		UIPlayerProgressPanel.Get().SetVisible(false, true);
		if (UIRAFProgramScreen.Get().IsVisible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.ReferAFriendBtnClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		this.SetPlayMenuCatgeoryVisible(false);
		UIRAFProgramScreen.Get().SetVisible(true);
	}

	public void CashShopBtnClicked(BaseEventData data)
	{
		if (this.m_LastTimeNavbuttonClicked == Time.time)
		{
			return;
		}
		this.m_LastTimeNavbuttonClicked = Time.time;
		if (UIGGBoostPurchaseScreen.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.CashShopBtnClicked(BaseEventData)).MethodHandle;
			}
			UIGGBoostPurchaseScreen.Get().SetVisible(false);
		}
		if (UIPlayerProgressPanel.Get() != null)
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
			UIPlayerProgressPanel.Get().SetVisible(false, true);
		}
		if (UIRAFProgramScreen.Get() != null)
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
			UIRAFProgramScreen.Get().SetVisible(false);
		}
		if (UICashShopPanel.Get().IsVisible())
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
			return;
		}
		this.SetNavButtonSelected(this.m_CashShopBtn);
		UIManager.SetGameObjectActive(this.m_CashShopNewContainer, false, null);
		UICashShopPanel.Get().SetVisible(true);
	}

	private void MicrophoneClicked(BaseEventData data)
	{
		this.m_voiceListMenu.SetVisible(!this.m_voiceListMenu.IsVisible());
	}

	private void DiscordOnJoined()
	{
		UIManager.SetGameObjectActive(this.m_microphoneConnectedBtn, true, null);
		UIManager.SetGameObjectActive(this.m_microphoneOfflineBtn, false, null);
	}

	private void DiscordOnError(ErrorEventArgs e)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Cancel);
	}

	private void DiscordOnDisconnected()
	{
		UIManager.SetGameObjectActive(this.m_microphoneConnectedBtn, false, null);
		UIManager.SetGameObjectActive(this.m_microphoneOfflineBtn, true, null);
	}

	private void CloseCurrentTabPanel()
	{
		if (this.m_currentNavBtn == this.m_landingPageBtn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.CloseCurrentTabPanel()).MethodHandle;
			}
			UIFrontEnd.Get().m_landingPageScreen.SetVisible(false);
		}
		else if (this.m_currentNavBtn == this.m_PlayBtn)
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
			this.SetPlayMenuCatgeoryVisible(false);
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
				for (;;)
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
					for (;;)
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
						goto IL_145;
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
				UICharacterSelectScreenController.Get().SetVisible(false, false);
			}
			IL_145:
			UICharacterScreen.Get().DoRefreshFunctions(0x80);
		}
		else if (this.m_currentNavBtn == this.m_CollectionBtn)
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
			UIStorePanel.Get().CloseStore();
		}
		else if (this.m_currentNavBtn == this.m_CashShopBtn)
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
			UICashShopPanel.Get().SetVisible(false);
		}
		else if (this.m_currentNavBtn == this.m_SeasonBtn)
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
			UISeasonsPanel.Get().SetVisible(false, false, true);
		}
		else if (this.m_currentNavBtn == this.m_LootMatrixBtn)
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
			UILootMatrixScreen.Get().SetVisible(false);
		}
	}

	public void SetNavButtonSelected(_SelectableBtn btn)
	{
		if (this.m_currentNavBtn == btn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.SetNavButtonSelected(_SelectableBtn)).MethodHandle;
			}
			return;
		}
		if (this.m_currentNavBtn != null)
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
			this.CloseCurrentTabPanel();
			this.m_currentNavBtn.SetSelected(false, false, string.Empty, string.Empty);
		}
		UIManager.SetGameObjectActive(this.m_PlayButtonNoticeContainer, this.m_PlayBtn != btn, null);
		if (btn != null)
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
			btn.SetSelected(true, false, string.Empty, string.Empty);
		}
		this.m_previousNavBtn = this.m_currentNavBtn;
		this.m_currentNavBtn = btn;
		this.m_gamePadHoverBtn = btn;
	}

	public void ReturnToPreviousTab()
	{
		if (this.m_previousNavBtn != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.ReturnToPreviousTab()).MethodHandle;
			}
			this.m_previousNavBtn.spriteController.callback(null);
		}
	}

	public void SetPlayMenuCatgeoryVisible(bool visible)
	{
		this.m_playMenuCatgeory.SetVisible(visible);
	}

	public void NotifyCurrentPanelLoseFocus()
	{
		if (this.m_currentNavBtn == this.m_landingPageBtn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.NotifyCurrentPanelLoseFocus()).MethodHandle;
			}
			UILandingPageScreen.Get().SetVisible(false);
		}
		else if (this.m_currentNavBtn == this.m_PlayBtn)
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
			this.SetPlayMenuCatgeoryVisible(false);
			UICharacterSelectWorldObjects.Get().SetVisible(false);
			UINewUserFlowManager.OnDoneWithReadyButton();
			if (UIRankedModeSelectScreen.Get() != null)
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
				UIRankedModeSelectScreen.Get().SetVisible(false);
			}
			if (AppState.GetCurrent() == AppState_CreateGame.Get())
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
				UICreateGameScreen.Get().SetVisible(false);
			}
			else if (AppState.GetCurrent() == AppState_JoinGame.Get())
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
				UIJoinGameScreen.Get().SetVisible(false);
			}
			else if (AppState.GetCurrent() == AppState_RankModeDraft.Get())
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
				UIRankedModeDraftScreen.Get().m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 0f;
			}
			if (UICharacterSelectScreenController.Get() != null)
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
				if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
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
					if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
					{
						goto IL_18A;
					}
				}
				UICharacterSelectScreenController.Get().SetVisible(false, false);
			}
			IL_18A:
			UICharacterScreen.Get().DoRefreshFunctions(0x80);
		}
		else if (this.m_currentNavBtn == this.m_CollectionBtn)
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
			UIStorePanel.Get().NotifyLoseFocus();
		}
		else if (this.m_currentNavBtn == this.m_SeasonBtn)
		{
			UISeasonsPanel.Get().NotifyLoseFocus();
		}
		else if (this.m_currentNavBtn == this.m_LootMatrixBtn)
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
			UILootMatrixScreen.Get().NotifyLoseFocus();
		}
		else if (this.m_currentNavBtn == this.m_CashShopBtn)
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
			UICashShopPanel.Get().NotifyLoseFocus();
		}
		if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
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
			if (!(AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
			{
				return;
			}
		}
		UICharacterSelectWorldObjects.Get().SetVisible(false);
	}

	public void NotifyCurrentPanelGetFocus()
	{
		if (this.m_currentNavBtn == this.m_landingPageBtn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.NotifyCurrentPanelGetFocus()).MethodHandle;
			}
			UILandingPageScreen.Get().SetVisible(true);
		}
		else if (this.m_currentNavBtn == this.m_PlayBtn)
		{
			if (AppState.GetCurrent() == AppState_CreateGame.Get())
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
				UICreateGameScreen.Get().SetVisible(true);
			}
			else if (AppState.GetCurrent() == AppState_JoinGame.Get())
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
				UIJoinGameScreen.Get().SetVisible(true);
			}
			else if (AppState.GetCurrent() == AppState_RankModeDraft.Get())
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
				UIRankedModeDraftScreen.Get().m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 1f;
			}
			else
			{
				if (GameManager.Get() != null)
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
					if (GameManager.Get().GameInfo != null)
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
						if (GameManager.Get().GameInfo.IsCustomGame && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
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
							UICharacterSelectWorldObjects.Get().SetVisible(true);
							UICharacterSelectScreenController.Get().SetVisible(true, false);
							goto IL_1CF;
						}
					}
				}
				this.SetPlayMenuCatgeoryVisible(true);
				if (!(AppState.GetCurrent() == AppState_CharacterSelect.Get()))
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
					if (!UIMatchStartPanel.Get().IsVisible())
					{
						AppState_GroupCharacterSelect.Get().Enter();
						goto IL_1CF;
					}
					for (;;)
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
			}
			IL_1CF:;
		}
		else if (this.m_currentNavBtn == this.m_CollectionBtn)
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
			UIStorePanel.Get().NotifyGetFocus();
		}
		else if (this.m_currentNavBtn == this.m_SeasonBtn)
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
			UISeasonsPanel.Get().NotifyGetFocus();
		}
		else if (this.m_currentNavBtn == this.m_LootMatrixBtn)
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
			UILootMatrixScreen.Get().NotifyGetFocus();
		}
		else if (this.m_currentNavBtn == this.m_CashShopBtn)
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
			UICashShopPanel.Get().NotifyGetFocus();
		}
	}

	public void ToggleUiForGameStarting(bool shouldShow)
	{
		bool flag;
		if (!AppState.IsInGame())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FrontEndNavPanel.ToggleUiForGameStarting(bool)).MethodHandle;
			}
			if (GameManager.Get().GameInfo != null)
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
				if (GameManager.Get().GameInfo.IsCustomGame)
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
					flag = (GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped);
					goto IL_72;
				}
			}
		}
		flag = false;
		IL_72:
		bool flag2 = flag;
		Component exitCustomGamesBtn = this.m_exitCustomGamesBtn;
		bool doActive;
		if (!shouldShow)
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
			doActive = flag2;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(exitCustomGamesBtn, doActive, null);
		UIManager.SetGameObjectActive(this.m_landingPageBtn, shouldShow, null);
		UIManager.SetGameObjectActive(this.m_PlayBtn, shouldShow, null);
		UIManager.SetGameObjectActive(this.m_CollectionBtn, shouldShow, null);
		UIManager.SetGameObjectActive(this.m_SeasonBtn, shouldShow, null);
		UIManager.SetGameObjectActive(this.m_LootMatrixBtn, shouldShow, null);
		UIManager.SetGameObjectActive(this.m_WatchBtn, false, null);
		UIManager.SetGameObjectActive(this.m_CashShopBtn, shouldShow, null);
		this.SetPlayMenuCatgeoryVisible(shouldShow);
	}
}
