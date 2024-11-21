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
        UIEventTriggerUtils.AddListener(
            m_LimitedModeHitbox.gameObject,
            EventTriggerType.PointerEnter,
            delegate { UIManager.SetGameObjectActive(m_LimitedModeTooltip, true); });
        UIEventTriggerUtils.AddListener(
            m_LimitedModeHitbox.gameObject,
            EventTriggerType.PointerExit,
            delegate { UIManager.SetGameObjectActive(m_LimitedModeTooltip, false); });
        UIManager.SetGameObjectActive(m_LimitedModeTooltip, false);
        m_notificationsBtn
            .spriteController
            .GetComponent<UITooltipHoverObject>()
            .Setup(TooltipType.Titled, PopulateContractsTooltip);
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
        m_menuBtn.SetSelected(false);
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
        HandleLobbyServerClientAccessLevelChange(
            ClientGameManager.Get().ClientAccessLevel,
            ClientGameManager.Get().ClientAccessLevel);
        ClientGameManager.Get().QueryPlayerMatchData(HandlePlayerMatchDataResponse);
        if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
        {
            OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
        }

        int freelancerCurrency = 0;
        int isoCurrency = 0;
        int rankedCurrency = 0;
        int freelancerTokens = 0;
        if (ClientGameManager.Get() != null && ClientGameManager.Get().PlayerWallet != null)
        {
            CurrencyWallet playerWallet = ClientGameManager.Get().PlayerWallet;
            freelancerCurrency = playerWallet.GetCurrentAmount(CurrencyType.FreelancerCurrency);
            isoCurrency = playerWallet.GetCurrentAmount(CurrencyType.ISO);
            rankedCurrency = playerWallet.GetCurrentAmount(CurrencyType.RankedCurrency);
            freelancerTokens = playerWallet.GetCurrentAmount(CurrencyType.UnlockFreelancerToken);
        }

        m_freelancerCurrencyText.text = "<sprite name=credit>" + UIStorePanel.FormatIntToString(freelancerCurrency, true);
        m_freelancerCurrencyText.GetComponent<UITooltipHoverObject>().Setup(
            TooltipType.Simple,
            delegate(UITooltipBase tooltip)
            {
                ((UISimpleTooltip)tooltip).Setup(StringUtil.TR("FreelancerCurrencyDesc", "Global"));
                return true;
            });

        m_isoText.text = "<sprite name=iso>" + UIStorePanel.FormatIntToString(isoCurrency, true);
        m_isoText.GetComponent<UITooltipHoverObject>().Setup(
            TooltipType.Simple,
            delegate(UITooltipBase tooltip)
            {
                ((UISimpleTooltip)tooltip).Setup(StringUtil.TR("ISODescription", "Global"));
                return true;
            });

        m_prestigeText.text = "<sprite name=rankedCurrency>" + UIStorePanel.FormatIntToString(rankedCurrency, true);
        m_prestigeText.GetComponent<UITooltipHoverObject>().Setup(
            TooltipType.Simple,
            delegate(UITooltipBase tooltip)
            {
                ((UISimpleTooltip)tooltip).Setup(StringUtil.TR("RankedCurrencyDescription", "Global"));
                return true;
            });

        if (freelancerCurrency > 0)
        {
            UINewUserFlowManager.OnFreelancerCurrencyOwned();
        }

        if (freelancerTokens > 0)
        {
            UINewUserFlowManager.OnFreelancerTokenOwned();
        }

        UIManager.SetGameObjectActive(m_microphoneConnectedBtn, false);
        UIManager.SetGameObjectActive(m_microphoneOfflineBtn, true);
        DiscordClientInterface discord = DiscordClientInterface.Get();
        discord.OnJoined += discord.OnJoined;
        discord.OnError += DiscordOnError;
        discord.OnDisconnected += discord.OnDisconnected;
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

        DiscordClientInterface discord = DiscordClientInterface.Get();
        if (discord != null)
        {
            discord.OnJoined -= discord.OnJoined;
            discord.OnError -= DiscordOnError;
            discord.OnDisconnected -= discord.OnDisconnected;
        }

        if (s_instance == this)
        {
            s_instance = null;
        }
    }

    private void Update()
    {
        if (UIFrontendLoadingScreen.Get() == null
            || UIFrontendLoadingScreen.Get().IsVisible()
            || !gameObject.activeInHierarchy
            || (Options_UI.Get() != null && Options_UI.Get().IsVisible())
            || (KeyBinding_UI.Get() != null && KeyBinding_UI.Get().IsVisible())
            || (DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DebugCamera")))
        {
            return;
        }

        if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Seasons_FE_Nav))
        {
            if (GameManager.Get() != null
                && GameManager.Get().GameplayOverrides != null
                && GameManager.Get().GameplayOverrides.EnableSeasons
                && m_SeasonBtn.spriteController.IsClickable())
            {
                UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
                SeasonsBtnClicked(null);
            }
        }
        else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Collection_FE_Nav))
        {
            if (GameManager.Get() != null
                && GameManager.Get().GameplayOverrides != null
                && GameManager.Get().GameplayOverrides.EnableShop)
            {
                UIFrontEnd.PlaySound(FrontEndButtonSounds.TopMenuSelect);
                CollectionsBtnClicked(null);
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
        else if (InputManager.Get()
                 .IsKeyBindingNewlyHeld(KeyPreference.Match_History_Replays_FE_Nav))
        {
            UIPlayerProgressPanel uIPlayerProgressPanel = UIPlayerProgressPanel.Get();
            uIPlayerProgressPanel.SetVisible(true, false);
            uIPlayerProgressPanel.NotifyMenuButtonClicked(uIPlayerProgressPanel.m_history);
        }
        else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.Banner_FE_Nav))
        {
            UIPlayerProgressPanel uIPlayerProgressPanel = UIPlayerProgressPanel.Get();
            uIPlayerProgressPanel.SetVisible(true, false);
            uIPlayerProgressPanel.NotifyMenuButtonClicked(uIPlayerProgressPanel.m_banner);
        }

        if (GameManager.Get() != null
            && !GameManager.Get().GameplayOverrides.DisableControlPadInput
            && !m_playMenuCatgeory.IsVisible())
        {
            if (Input.GetButtonDown("GamepadButtonLeftShoulder"))
            {
                m_gamePadHoverBtn.SetSelected(false);
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

                m_gamePadHoverBtn.SetSelected(true);
            }
            else if (Input.GetButtonDown("GamepadButtonRightShoulder"))
            {
                m_gamePadHoverBtn.SetSelected(false);
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

                m_gamePadHoverBtn.SetSelected(true);
            }

            if (Input.GetButtonDown("GamepadButtonA") && m_currentNavBtn != m_gamePadHoverBtn)
            {
                if (m_gamePadHoverBtn == m_landingPageBtn)
                {
                    LandingPageBtnClicked(null);
                }
                else if (m_gamePadHoverBtn == m_PlayBtn)
                {
                    PlayBtnClicked(null);
                }
                else if (m_gamePadHoverBtn == m_SeasonBtn)
                {
                    SeasonsBtnClicked(null);
                }
                else if (m_gamePadHoverBtn == m_CollectionBtn)
                {
                    CollectionsBtnClicked(null);
                }
                else if (m_gamePadHoverBtn == m_LootMatrixBtn)
                {
                    LootMatrixBtnClicked(null);
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
            if (m_landingPageBtn.isActiveAndEnabled || m_exitCustomGamesBtn.isActiveAndEnabled)
            {
                LandingPageBtnClicked(null);
            }

            AppState_GroupCharacterSelect appState_GroupCharacterSelect = AppState_GroupCharacterSelect.Get();
            if (appState_GroupCharacterSelect != null && appState_GroupCharacterSelect.InQueue())
            {
                appState_GroupCharacterSelect.UpdateReadyState(false);
                NavigationBar navigationBar = NavigationBar.Get();
                if (navigationBar != null)
                {
                    UIManager.SetGameObjectActive(navigationBar.m_cancelBtn, false);
                    navigationBar.m_cancelBtn.spriteController.SetClickable(false);
                    navigationBar.m_searchQueueText.text = string.Empty;
                }
            }
        }
        else
        {
            bool isLimitedModeActive = newLevel >= ClientAccessLevel.Full
                                       && ClientGameManager.Get() != null
                                       && !ClientGameManager.Get().HasPurchasedGame;
            UIManager.SetGameObjectActive(m_LimitedModeContainer, isLimitedModeActive);
        }
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
        foreach (InventoryItem item in inventoryData.Items)
        {
            if (item.GetTemplate().Type == InventoryItemType.Lockbox)
            {
                numLockBoxes++;
            }
        }

        bool hasNewMatrices = m_lastSeenNumberOfLootMatrices < numLockBoxes;
        Animator componentInChildren = m_LootMatrixNewContainer.GetComponentInChildren<Animator>(true);
        UIManager.SetGameObjectActive(componentInChildren, numLockBoxes > 0 && hasNewMatrices);
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
        int numQuests = 0;
        if (accountData.AccountComponent.DailyQuestsAvailable)
        {
            foreach (int key in accountData.QuestComponent.Progress.Keys)
            {
                if (QuestWideData.Get().IsDailyQuest(key))
                {
                    numQuests++;
                }
            }
        }

        if (ClientGameManager.Get().AlertMissionsData != null
            && ClientGameManager.Get().AlertMissionsData.CurrentAlert != null)
        {
            UIManager.SetGameObjectActive(m_alertActiveIcon, true);
            if (ClientGameManager.Get().AlertMissionsData.CurrentAlert.Type == AlertMissionType.Quest)
            {
                numQuests++;
            }
        }
        else
        {
            UIManager.SetGameObjectActive(m_alertActiveIcon, false);
        }

        m_questNotificationNumber.text = numQuests.ToString();
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
        switch (newBalance.Type)
        {
            case CurrencyType.FreelancerCurrency:
            {
                m_freelancerCurrencyText.text =
                    "<sprite name=credit>" + UIStorePanel.FormatIntToString(newBalance.Amount, true);
                if (newBalance.Amount > 0)
                {
                    UINewUserFlowManager.OnFreelancerCurrencyOwned();
                }

                break;
            }
            case CurrencyType.ISO:
                m_isoText.text = "<sprite name=iso>" + UIStorePanel.FormatIntToString(newBalance.Amount, true);
                break;
            case CurrencyType.RankedCurrency:
                m_prestigeText.text = "<sprite name=rankedCurrency>"
                                      + UIStorePanel.FormatIntToString(newBalance.Amount, true);
                break;
            case CurrencyType.UnlockFreelancerToken when newBalance.Amount > 0:
                UINewUserFlowManager.OnFreelancerTokenOwned();
                break;
        }
    }

    public void SetShopVisible(bool visible)
    {
        UIManager.SetGameObjectActive(m_CollectionBtn.spriteController, visible);
        foreach (CanvasGroup group in m_CollectionBtn.GetComponentsInChildren<CanvasGroup>(true))
        {
            if (group.gameObject != m_CollectionBtn.gameObject)
            {
                UIManager.SetGameObjectActive(group, visible);
            }
        }

        if (!visible
            && UIStorePanel.Get() != null
            && UIStorePanel.Get().IsStoreOpen())
        {
            LandingPageBtnClicked(null);
        }
    }

    public void CheckSeasonsVisibility()
    {
        SetSeasonsVisible(UISeasonsPanel.CheckSeasonsVisibility(out m_seasonLockoutReason));
    }

    public void SetSeasonsVisible(bool visible)
    {
        m_SeasonBtn.spriteController.SetClickable(visible);
        foreach (CanvasGroup group in m_SeasonBtn.GetComponentsInChildren<CanvasGroup>(true))
        {
            if (group.gameObject != m_SeasonBtn.gameObject)
            {
                UIManager.SetGameObjectActive(group, visible);
            }
        }

        if (!visible
            && UISeasonsPanel.Get() != null
            && UISeasonsPanel.Get().IsVisible())
        {
            m_SeasonBtn.SetSelected(false);
            UISeasonsPanel.Get().SetVisible(false);
        }

        if (m_seasonLockoutReason != SeasonLockoutReason.None)
        {
            m_SeasonBtn.spriteController.SetForceHovercallback(true);
            m_SeasonBtn.spriteController.SetForceExitCallback(true);
            m_SeasonBtn.spriteController.GetComponent<UITooltipHoverObject>()
                .Setup(TooltipType.Titled, PopulateSeasonsTooltip);
        }
        else
        {
            m_SeasonBtn.spriteController.SetForceHovercallback(false);
            m_SeasonBtn.spriteController.SetForceExitCallback(false);
            m_SeasonBtn.spriteController.pointerEnterCallback = null;
            m_SeasonBtn.spriteController.pointerExitCallback = null;
        }
    }

    private bool PopulateSeasonsTooltip(UITooltipBase tooltip)
    {
        if (m_seasonLockoutReason == SeasonLockoutReason.InTutorialSeason)
        {
            if (ClientGameManager.Get() != null && ClientGameManager.Get().GetPlayerAccountData() != null)
            {
                int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
                int seasonLevel = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.SeasonLevel;
                SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(activeSeason);
                (tooltip as UITitledTooltip).Setup(
                    StringUtil.TR("Locked", "Global"),
                    string.Format(
                        StringUtil.TR("RequiresMatchesPlayed", "Global"),
                        QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, activeSeason) - seasonLevel));
                return true;
            }
        }
        else if (m_seasonLockoutReason == SeasonLockoutReason.Disabled)
        {
            (tooltip as UITitledTooltip).Setup(
                StringUtil.TR("Locked", "Global"),
                StringUtil.TR("SeasonsDisabled", "Global"));
            return true;
        }

        return false;
    }

    public void CheckContractsEnabled()
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        bool areDailyQuestsAvailable = clientGameManager != null
                                       && clientGameManager.IsPlayerAccountDataAvailable()
                                       && clientGameManager.GetPlayerAccountData().AccountComponent
                                           .DailyQuestsAvailable;
        m_notificationsBtn.spriteController.SetClickable(areDailyQuestsAvailable);
        m_notificationsBtn.SetDisabled(!areDailyQuestsAvailable);
    }

    public void CheckMicrophoneEnabled()
    {
        UIManager.SetGameObjectActive(
            m_microphoneContainer,
            DiscordClientInterface.IsEnabled && DiscordClientInterface.IsSdkEnabled);
        TextMeshProUGUI autoJoinDiscordText = m_autoJoinDiscordText;
        string term = DiscordClientInterface.IsSdkEnabled ? "AutoJoinVoiceBtn" : "JoinDiscordBtn";
        autoJoinDiscordText.text = StringUtil.TR(term, "NewFrontEndScene");
        if (DiscordClientInterface.Get().IsConnected && DiscordClientInterface.Get().ChannelInfo != null)
        {
            DiscordOnJoined();
        }
        else
        {
            DiscordOnDisconnected();
        }
    }

    private void CheckNewCashShopFeaturedItems()
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        if (clientGameManager != null && clientGameManager.IsPlayerAccountDataAvailable())
        {
            AccountComponent.UIStateIdentifier uiState =
                AccountComponent.UIStateIdentifier.CashShopFeaturedItemsVersionViewed;
            int uIState = ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uiState);
            int featuredItemsVersion = StoreWideData.Get().m_featuredItemsVersion;
            UIManager.SetGameObjectActive(m_CashShopNewContainer, uIState < featuredItemsVersion);
        }
        else
        {
            UIManager.SetGameObjectActive(m_CashShopNewContainer, false);
        }
    }

    private bool PopulateContractsTooltip(UITooltipBase tooltip)
    {
        if (m_notificationsBtn.spriteController.IsClickable())
        {
            return false;
        }

        (tooltip as UITitledTooltip).Setup(
            StringUtil.TR("Locked", "Global"),
            string.Format(StringUtil.TR("DailyQuestsUnlockRequirements", "Quests")));
        return true;
    }

    public void SetLootMatrixVisible(bool visible)
    {
        UIManager.SetGameObjectActive(m_LootMatrixBtn.spriteController, visible);
        foreach (CanvasGroup group in m_LootMatrixBtn.GetComponentsInChildren<CanvasGroup>(true))
        {
            if (group.gameObject != m_LootMatrixBtn.gameObject)
            {
                UIManager.SetGameObjectActive(group, visible);
            }
        }

        if (visible)
        {
            return;
        }

        if (UILootMatrixScreen.Get().IsVisible)
        {
            m_LootMatrixBtn.SetSelected(false);
            UILootMatrixScreen.Get().SetVisible(false);
        }
    }

    public void NotifyGroupUpdate()
    {
    }

    public void NotificationBtnClicked(BaseEventData data)
    {
        if (!m_notificationsBtn.spriteController.IsClickable())
        {
            return;
        }

        if (UIMainMenu.Get() != null && UIMainMenu.Get().IsOpen())
        {
            UIMainMenu.Get().SetMenuVisible(false);
        }

        if (m_notificationsBtn.IsSelected())
        {
            QuestListPanel.Get().SetVisible(false);
            m_notificationsBtn.SetSelected(false);
        }
        else
        {
            QuestListPanel.Get().SetVisible(true);
            m_notificationsBtn.SetSelected(true);
        }
    }

    public void MenuBtnClicked(BaseEventData data)
    {
        if (!UIMainMenu.Get().IsOpen() && m_notificationsBtn.IsSelected())
        {
            QuestListPanel.Get().SetVisible(false);
            m_notificationsBtn.SetSelected(false);
        }

        UIMainMenu.Get().SetMenuVisible(!UIMainMenu.Get().IsOpen());
    }

    public void LandingPageBtnClicked(BaseEventData data)
    {
        if (UIFrontEnd.Get() == null
            || UIStorePanel.Get() == null
            || UILootMatrixScreen.Get() == null
            || UIRAFProgramScreen.Get() == null
            || UIPlayerProgressPanel.Get() == null
            || UISeasonsPanel.Get() == null)
        {
            return;
        }

        UIGGBoostPurchaseScreen.Get().SetVisible(false);
        UIPlayerProgressPanel.Get().SetVisible(false);
        UIRAFProgramScreen.Get().SetVisible(false);

        if (!UICashShopPanel.Get().IsVisible()
            && !UIStorePanel.Get().IsStoreOpen()
            && !UISeasonsPanel.Get().IsVisible()
            && !UILootMatrixScreen.Get().IsVisible
            && AppState_LandingPage.Get() == AppState.GetCurrent()
            && m_currentNavBtn == m_landingPageBtn)
        {
            return;
        }

        if (UIGameSettingsPanel.Get() != null)
        {
            UIGameSettingsPanel.Get().SetVisible(false);
        }

        if (m_LastTimeNavbuttonClicked == Time.time)
        {
            return;
        }

        m_LastTimeNavbuttonClicked = Time.time;
        SetNavButtonSelected(m_landingPageBtn);
        if (AppState_LandingPage.Get() != AppState.GetCurrent())
        {
            if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
            {
                AppState_GameTeardown.Get().Enter();
            }
            else
            {
                AppState_LandingPage.Get().Enter(true);
                SetPlayMenuCatgeoryVisible(false);
            }
        }
        else
        {
            UIFrontEnd.Get().m_landingPageScreen.SetVisible(true);
        }
    }

    public void PlayBtnClicked(BaseEventData data)
    {
        if (m_LastTimeNavbuttonClicked == Time.time)
        {
            return;
        }

        m_LastTimeNavbuttonClicked = Time.time;
        UIGGBoostPurchaseScreen.Get().SetVisible(false);
        UIPlayerProgressPanel.Get().SetVisible(false);
        UIRAFProgramScreen.Get().SetVisible(false);
        UINewUserFlowManager.OnGameModeButtonDisplayed();
        if (!UICashShopPanel.Get().IsVisible()
            && !UIStorePanel.Get().IsStoreOpen()
            && !UISeasonsPanel.Get().IsVisible()
            && !UILootMatrixScreen.Get().IsVisible
            && (AppState_CharacterSelect.Get() == AppState.GetCurrent()
                || AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
            && m_currentNavBtn == m_PlayBtn)
        {
            if (!m_playMenuCatgeory.IsVisible())
            {
                SetPlayMenuCatgeoryVisible(true);
            }
        }
        else
        {
            SetNavButtonSelected(m_PlayBtn);
            SetPlayMenuCatgeoryVisible(true);
            UIFrontEnd.Get().m_landingPageScreen.QuickPlayButtonClicked(data);
        }
    }

    public void DoSeasonsBtnClicked(bool setOverview = false, bool displayHighestChapter = true)
    {
        UIGGBoostPurchaseScreen.Get().SetVisible(false);
        UIPlayerProgressPanel.Get().SetVisible(false);
        UIRAFProgramScreen.Get().SetVisible(false);

        if (!UISeasonsPanel.Get().IsVisible() && m_LastTimeNavbuttonClicked != Time.time)
        {
            m_LastTimeNavbuttonClicked = Time.time;
            SetNavButtonSelected(m_SeasonBtn);
            UISeasonsPanel.Get().SetVisible(true, setOverview, displayHighestChapter);
            UINewUserFlowManager.OnSeasonsTabClicked();
        }
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

        if (!UILootMatrixScreen.Get().IsVisible && m_LastTimeNavbuttonClicked != Time.time)
        {
            m_LastTimeNavbuttonClicked = Time.time;
            SetNavButtonSelected(m_LootMatrixBtn);
            UILootMatrixScreen.Get().SetVisible(true);
            m_lastSeenNumberOfLootMatrices = int.TryParse(m_NewLootMatrixText.text, out m_lastSeenNumberOfLootMatrices)
                ? int.Parse(m_NewLootMatrixText.text)
                : 0;
            UIManager.SetGameObjectActive(m_LootMatrixNewContainer.GetComponentInChildren<Animator>(true), false);
        }
    }

    public void CollectionsBtnClicked(BaseEventData data)
    {
        if (m_LastTimeNavbuttonClicked == Time.time)
        {
            return;
        }

        m_LastTimeNavbuttonClicked = Time.time;
        UIGGBoostPurchaseScreen.Get().SetVisible(false);
        UIPlayerProgressPanel.Get().SetVisible(false);
        UIRAFProgramScreen.Get().SetVisible(false);
        if (!UIStorePanel.Get().IsStoreOpen())
        {
            SetNavButtonSelected(m_CollectionBtn);
            if (data != null)
            {
                ClientGameManager.Get().SendUIActionNotification("CollectionsBtnClicked");
            }

            UIStorePanel.Get().OpenStore();
        }
    }

    public void ToggleReferAFriend()
    {
        if (UIRAFProgramScreen.Get().IsVisible)
        {
            if (m_currentNavBtn == m_PlayBtn)
            {
                SetPlayMenuCatgeoryVisible(true);
            }

            UIRAFProgramScreen.Get().SetVisible(false);
        }
        else
        {
            UIGGBoostPurchaseScreen.Get().SetVisible(false);
            UIPlayerProgressPanel.Get().SetVisible(false);
            SetPlayMenuCatgeoryVisible(false);
            UIRAFProgramScreen.Get().SetVisible(true);
        }
    }

    public void ReferAFriendBtnClicked(BaseEventData data)
    {
        UIGGBoostPurchaseScreen.Get().SetVisible(false);
        UIPlayerProgressPanel.Get().SetVisible(false);
        if (!UIRAFProgramScreen.Get().IsVisible)
        {
            SetPlayMenuCatgeoryVisible(false);
            UIRAFProgramScreen.Get().SetVisible(true);
        }
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

        if (!UICashShopPanel.Get().IsVisible())
        {
            SetNavButtonSelected(m_CashShopBtn);
            UIManager.SetGameObjectActive(m_CashShopNewContainer, false);
            UICashShopPanel.Get().SetVisible(true);
        }
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
            UIFrontEnd.Get().m_landingPageScreen.SetVisible(false);
        }
        else if (m_currentNavBtn == m_PlayBtn)
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

            if (UICharacterSelectScreenController.Get() != null
                && (AppState.GetCurrent() == AppState_CharacterSelect.Get()
                    || AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
            {
                UICharacterSelectScreenController.Get().SetVisible(false);
            }

            UICharacterScreen.Get()
                .DoRefreshFunctions((ushort)UICharacterScreen.RefreshFunctionType.RefreshBotSkillPanel);
        }
        else if (m_currentNavBtn == m_CollectionBtn)
        {
            UIStorePanel.Get().CloseStore();
        }
        else if (m_currentNavBtn == m_CashShopBtn)
        {
            UICashShopPanel.Get().SetVisible(false);
        }
        else if (m_currentNavBtn == m_SeasonBtn)
        {
            UISeasonsPanel.Get().SetVisible(false);
        }
        else if (m_currentNavBtn == m_LootMatrixBtn)
        {
            UILootMatrixScreen.Get().SetVisible(false);
        }
    }

    public void SetNavButtonSelected(_SelectableBtn btn)
    {
        if (m_currentNavBtn == btn)
        {
            return;
        }

        if (m_currentNavBtn != null)
        {
            CloseCurrentTabPanel();
            m_currentNavBtn.SetSelected(false);
        }

        UIManager.SetGameObjectActive(m_PlayButtonNoticeContainer, m_PlayBtn != btn);
        if (btn != null)
        {
            btn.SetSelected(true);
        }

        m_previousNavBtn = m_currentNavBtn;
        m_currentNavBtn = btn;
        m_gamePadHoverBtn = btn;
    }

    public void ReturnToPreviousTab()
    {
        if (m_previousNavBtn != null)
        {
            m_previousNavBtn.spriteController.callback(null);
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
        else if (m_currentNavBtn == m_PlayBtn)
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

            if (UICharacterSelectScreenController.Get() != null
                && (AppState.GetCurrent() == AppState_CharacterSelect.Get()
                    || AppState.GetCurrent() == AppState_GroupCharacterSelect.Get()))
            {
                UICharacterSelectScreenController.Get().SetVisible(false);
            }

            UICharacterScreen.Get()
                .DoRefreshFunctions((ushort)UICharacterScreen.RefreshFunctionType.RefreshBotSkillPanel);
        }
        else if (m_currentNavBtn == m_CollectionBtn)
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

        if (AppState.GetCurrent() == AppState_CharacterSelect.Get()
            || AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
        {
            UICharacterSelectWorldObjects.Get().SetVisible(false);
        }
    }

    public void NotifyCurrentPanelGetFocus()
    {
        if (m_currentNavBtn == m_landingPageBtn)
        {
            UILandingPageScreen.Get().SetVisible(true);
        }
        else if (m_currentNavBtn == m_PlayBtn)
        {
            if (AppState.GetCurrent() == AppState_CreateGame.Get())
            {
                UICreateGameScreen.Get().SetVisible(true);
            }
            else if (AppState.GetCurrent() == AppState_JoinGame.Get())
            {
                UIJoinGameScreen.Get().SetVisible(true);
            }
            else if (AppState.GetCurrent() == AppState_RankModeDraft.Get())
            {
                UIRankedModeDraftScreen.Get().m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 1f;
            }
            else if (GameManager.Get() != null
                     && GameManager.Get().GameInfo != null
                     && GameManager.Get().GameInfo.IsCustomGame
                     && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
            {
                UICharacterSelectWorldObjects.Get().SetVisible(true);
                UICharacterSelectScreenController.Get().SetVisible(true);
            }
            else
            {
                SetPlayMenuCatgeoryVisible(true);
                if (AppState.GetCurrent() != AppState_CharacterSelect.Get() && !UIMatchStartPanel.Get().IsVisible())
                {
                    AppState_GroupCharacterSelect.Get().Enter();
                }
                else
                {
                    AppState_CharacterSelect.Get().Enter();
                }
            }
        }
        else if (m_currentNavBtn == m_CollectionBtn)
        {
            UIStorePanel.Get().NotifyGetFocus();
        }
        else if (m_currentNavBtn == m_SeasonBtn)
        {
            UISeasonsPanel.Get().NotifyGetFocus();
        }
        else if (m_currentNavBtn == m_LootMatrixBtn)
        {
            UILootMatrixScreen.Get().NotifyGetFocus();
        }
        else if (m_currentNavBtn == m_CashShopBtn)
        {
            UICashShopPanel.Get().NotifyGetFocus();
        }
    }

    public void ToggleUiForGameStarting(bool shouldShow)
    {
        bool isAssemblingCustomGame = !AppState.IsInGame()
                                      && GameManager.Get().GameInfo != null
                                      && GameManager.Get().GameInfo.IsCustomGame
                                      && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped;
        UIManager.SetGameObjectActive(m_exitCustomGamesBtn, !shouldShow && isAssemblingCustomGame);
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