using System;
using LobbyGameClientMessages;

public class AppState_LandingPage : AppState
{
    private static AppState_LandingPage s_instance;

    private UIDialogBox m_messageBox;
    private string m_lastLobbyErrorMessage;
    private bool m_receivedLobbyinfo;
    private bool m_returningFromGroupCharacterSelect;
    private bool m_goToCharacterSelect;

    public bool ReceivedLobbyStatusInfo => m_receivedLobbyinfo;

    public static AppState_LandingPage Get()
    {
        return s_instance;
    }

    public void Enter(bool returningFromGroupCharacterSelect)
    {
        m_returningFromGroupCharacterSelect = returningFromGroupCharacterSelect;
        base.Enter();
    }

    public void Enter(string lastLobbyErrorMessage, bool goToCharacterSelect = false)
    {
        m_lastLobbyErrorMessage = lastLobbyErrorMessage;
        m_goToCharacterSelect = goToCharacterSelect;
        base.Enter();
    }

    public static void Create()
    {
        Create<AppState_LandingPage>();
    }

    private void Awake()
    {
        s_instance = this;
    }

    protected override void OnEnter()
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        UIFrontEnd.Get().m_landingPageScreen.m_inCustomGame = false;
        AudioManager.GetMixerSnapshotManager().SetMix_Menu();
        UIFrontEnd.Get().SetVisible(true);
        UIFrontEnd.Get().EnableFrontendEnvironment(true);
        UIFrontEnd.Get().ShowScreen(FrontEndScreenState.LandingPage);
        UIFrontEnd.Get().m_landingPageScreen.ShowMOTD();
        UIFrontEnd.Get().m_landingPageScreen.SetServerIsLocked(clientGameManager.IsServerLocked);
        UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
        UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
        UIManager.SetGameObjectActive(UISystemMenuPanel.Get(), true);
        clientGameManager.OnConnectedToLobbyServer += HandleConnectedToLobbyServer;
        clientGameManager.OnDisconnectedFromLobbyServer += HandleDisconnectedFromLobbyServer;
        clientGameManager.OnLobbyServerReadyNotification += HandleLobbyServerReadyNotification;
        clientGameManager.OnLobbyStatusNotification += HandleStatusNotification;
        clientGameManager.OnAccountDataUpdated += HandleAccountDataUpdated;
        clientGameManager.OnChatNotification += HandleChatNotification;
        GameManager.Get().OnGameAssembling += HandleGameAssembling;
        GameManager.Get().OnGameSelecting += HandleGameSelecting;
        GameManager.Get().OnGameLaunched += HandleGameLaunched;
        ConnectToLobbyServer();
        if (m_lastLobbyErrorMessage != null && m_messageBox == null)
        {
            UINewUserFlowManager.HideDisplay();
            string lastLobbyErrorMessage = m_lastLobbyErrorMessage;
            m_lastLobbyErrorMessage = null;
            m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(
                string.Empty,
                lastLobbyErrorMessage,
                StringUtil.TR("Ok", "Global"),
                delegate { m_messageBox = null; });
        }

        if (clientGameManager != null && clientGameManager.IsConnectedToLobbyServer
                                      && UILandingPageScreen.Get() == null)
        {
            clientGameManager.SendCheckAccountStatusRequest(HandleCheckAccountStatusResponse);
            clientGameManager.SendCheckRAFStatusRequest(false);
        }

        if (UILoadingScreenPanel.Get() != null)
        {
            UILoadingScreenPanel.Get().SetVisible(false);
        }

        if (clientGameManager.IsPlayerAccountDataAvailable())
        {
            HandleAccountDataUpdated(clientGameManager.GetPlayerAccountData());
        }

        UINewUserFlowManager.HighlightQueued();
        UINewUserFlowManager.OnNavBarDisplayed();
        if (HighlightUtils.Get() != null)
        {
            HighlightUtils.Get().HideCursorHighlights();
        }

        CheckForPreviousGame();
        if (clientGameManager.IsServerLocked)
        {
            m_goToCharacterSelect = false;
        }

        if (m_goToCharacterSelect
            || (!m_returningFromGroupCharacterSelect
                && clientGameManager != null
                && clientGameManager.GroupInfo != null
                && clientGameManager.GroupInfo.InAGroup))
        {
            m_goToCharacterSelect = false;
            UIFrontendLoadingScreen.Get().SetVisible(false);
            AppState_GroupCharacterSelect.Get().Enter();
            UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
            if (UIRankedModeSelectScreen.Get() != null)
            {
                if (ClientGameManager.Get() != null
                    && ClientGameManager.Get().GroupInfo != null
                    && ClientGameManager.Get().GroupInfo.SelectedQueueType == GameType.Ranked)
                {
                    UIFrontEnd.Get().ShowScreen(FrontEndScreenState.RankedModeSelect);
                    UIRankedModeSelectScreen.Get().SetVisible(true);
                }
                else
                {
                    UIFrontEnd.Get().ShowScreen(FrontEndScreenState.GroupCharacterSelect);
                    UIRankedModeSelectScreen.Get().SetVisible(false);
                }
            }
        }
        else if (UIRankedModeSelectScreen.Get() != null)
        {
            UIRankedModeSelectScreen.Get().SetVisible(false);
        }

        m_returningFromGroupCharacterSelect = false;
    }

    protected override void OnLeave()
    {
        if (m_messageBox != null)
        {
            m_messageBox.Close();
            m_messageBox = null;
        }

        ClientGameManager clientGameManager = ClientGameManager.Get();
        clientGameManager.OnConnectedToLobbyServer -= HandleConnectedToLobbyServer;
        clientGameManager.OnDisconnectedFromLobbyServer -= HandleDisconnectedFromLobbyServer;
        clientGameManager.OnLobbyServerReadyNotification -= HandleLobbyServerReadyNotification;
        clientGameManager.OnLobbyStatusNotification -= HandleStatusNotification;
        clientGameManager.OnAccountDataUpdated -= HandleAccountDataUpdated;
        clientGameManager.OnChatNotification -= HandleChatNotification;
        GameManager.Get().OnGameAssembling -= HandleGameAssembling;
        GameManager.Get().OnGameSelecting -= HandleGameSelecting;
        GameManager.Get().OnGameLaunched -= HandleGameLaunched;
    }

    private void HandleGameLaunched(GameType gameType)
    {
        UIFrontEnd.Get().NotifyGameLaunched();
        AppState_GameLoading.Get().Enter(gameType);
    }

    public void ConnectToLobbyServer()
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        if (!clientGameManager.IsConnectedToLobbyServer)
        {
            if (m_messageBox)
            {
                m_messageBox.Close();
                m_messageBox = null;
            }

            if (!m_lastLobbyErrorMessage.IsNullOrEmpty())
            {
                string lastLobbyErrorMessage = m_lastLobbyErrorMessage;
                m_lastLobbyErrorMessage = null;
                UINewUserFlowManager.HideDisplay();
                if (clientGameManager.AllowRelogin)
                {
                    m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(
                        string.Empty,
                        string.Format(StringUtil.TR("PressOkToReconnect", "Global"), lastLobbyErrorMessage),
                        StringUtil.TR("Ok", "Global"),
                        StringUtil.TR("Cancel", "Global"),
                        delegate { ConnectToLobbyServer(); },
                        delegate { AppState_Shutdown.Get().Enter(); });
                    return;
                }

                m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(
                    string.Empty,
                    string.Format(StringUtil.TR("PressOkToExit", "Global"), lastLobbyErrorMessage),
                    StringUtil.TR("Ok", "Global"),
                    delegate { AppState_Shutdown.Get().Enter(); });
                return;
            }

            try
            {
                m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(
                    string.Empty,
                    StringUtil.TR("ConnectingToLobbyServer", "Global"),
                    StringUtil.TR("Cancel", "Global"),
                    delegate { AppState_Shutdown.Get().Enter(); });
                clientGameManager.ConnectToLobbyServer();
            }
            catch (Exception ex)
            {
                if (m_messageBox != null)
                {
                    m_messageBox.Close();
                    m_messageBox = null;
                }

                UINewUserFlowManager.HideDisplay();

                m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(
                    string.Empty,
                    string.Format(StringUtil.TR("FailedToConnectToLobbyServer", "Global"), ex.Message),
                    StringUtil.TR("Ok", "Global"),
                    delegate { AppState_Shutdown.Get().Enter(); });
            }

            return;
        }

        UIFrontEnd.Get().m_landingPageScreen.ShowMOTD();
        UIFrontEnd.Get().m_landingPageScreen.SetServerIsLocked(clientGameManager.IsServerLocked);
    }

    private void Update()
    {
    }

    public void HandleConnectedToLobbyServer(RegisterGameClientResponse response)
    {
        if (m_messageBox)
        {
            m_messageBox.Close();
            m_messageBox = null;
        }

        if (response.Success)
        {
            return;
        }

        if (response.LocalizedFailure != null)
        {
            response.ErrorMessage = response.LocalizedFailure.ToString();
        }

        if (response.ErrorMessage.IsNullOrEmpty())
        {
            response.ErrorMessage = StringUtil.TR("UnknownError", "Global");
        }

        UINewUserFlowManager.HideDisplay();
        string description;
        switch (response.ErrorMessage)
        {
            case "INVALID_PROTOCOL_VERSION":
                description = StringUtil.TR("NotRecentVersionOfTheGame", "Frontend");
                break;
            case "INVALID_IP_ADDRESS":
                description = StringUtil.TR("IPAddressChanged", "Frontend");
                break;
            case "ACCOUNT_BANNED":
                description = StringUtil.TR("AccountBanned", "Frontend");
                break;
            default:
                description = string.Format(
                    StringUtil.TR("FailedToConnectToLobbyServer", "Global"),
                    response.ErrorMessage);
                break;
        }

        m_messageBox = UIDialogPopupManager.OpenOneButtonDialog(
            string.Empty,
            description,
            StringUtil.TR("Ok", "Global"),
            delegate { AppState_Shutdown.Get().Enter(); });
    }

    public void HandleLobbyServerReadyNotification(LobbyServerReadyNotification notification)
    {
        CheckForPreviousGame();
    }

    public void HandleAccountDataUpdated(PersistedAccountData accountData)
    {
        UIFrontEnd.Get().m_landingPageScreen.UpdateMatchData();
    }

    public void HandleStatusNotification(LobbyStatusNotification notification)
    {
        m_receivedLobbyinfo = true;
        UIFrontEnd.Get().m_landingPageScreen.ShowMOTD();
        UIFrontEnd.Get().m_landingPageScreen.SetServerIsLocked(ClientGameManager.Get().IsServerLocked);
        UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
        UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
    }

    private void CheckForPreviousGame()
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        if (clientGameManager != null
            && clientGameManager.IsRegistered
            && clientGameManager.IsReady)
        {
            Log.Info("Checking for previous game");
            clientGameManager.RequestPreviousGameInfo(
                delegate(PreviousGameInfoResponse response)
                {
                    if (GetCurrent() != this)
                    {
                        return;
                    }

                    if (response.PreviousGameInfo != null)
                    {
                        if (!response.PreviousGameInfo.IsQueuedGame && !response.PreviousGameInfo.IsCustomGame)
                        {
                            return;
                        }

                        if (response.PreviousGameInfo.GameConfig.TotalHumanPlayers < 2)
                        {
                            return;
                        }

                        PromptToRejoinGame(response.PreviousGameInfo);
                    }
                });
        }
        else
        {
            Log.Info(
                "Not checking for previous game-- {0}/{1}",
                clientGameManager.IsRegistered,
                clientGameManager.IsReady);
        }
    }

    private void PromptToRejoinGame(LobbyGameInfo previousGameInfo)
    {
        UINewUserFlowManager.HideDisplay();
        m_messageBox = UIDialogPopupManager.OpenTwoButtonDialog(
            StringUtil.TR("Reconnect", "Global"),
            string.Format(
                StringUtil.TR("ReconnectUnderDevelopment", "Global"),
                previousGameInfo.GameConfig.GameType.GetDisplayName()),
            StringUtil.TR("Reconnect", "Global"),
            StringUtil.TR("Cancel", "Global"),
            delegate
            {
                Log.Info("Attempting to reconnect!");
                ClientGameManager.Get().RejoinGame(true);
                m_messageBox = null;
            },
            delegate
            {
                Log.Info("Decided not to reconnect!");
                ClientGameManager.Get().RejoinGame(false);
                m_messageBox = null;
            });
        if (ClientCrashReportDetector.Get().m_crashDialog != null)
        {
            ClientCrashReportDetector.Get().m_crashDialog.gameObject.transform.SetAsLastSibling();
        }
    }

    private void HandleChatNotification(ChatNotification notification)
    {
        if (notification.ConsoleMessageType == ConsoleMessageType.Error
            && notification.LocalizedText != null
            && notification.LocalizedText.ToString() == StringUtil.TR("RejoinGameNoLongerAvailable", "Global"))
        {
            AppState_FrontendLoadingScreen.Get().Enter(null);
        }
    }

    public void OnQuickPlayClicked()
    {
        if (UIMatchStartPanel.Get().IsVisible())
        {
            AppState_CharacterSelect.Get().Enter();
        }
        else
        {
            AppState_GroupCharacterSelect.Get().Enter();
        }
    }

    public void OnTutorial1Clicked()
    {
        AppState_FrontendLoadingScreen.Get().Enter(null, AppState_FrontendLoadingScreen.NextState.GoToTutorial);
    }

    private void HandleGameSelecting()
    {
        AppState_CharacterSelect.Get().Enter();
    }

    private void HandleGameAssembling()
    {
        if (!IsInGame()
            && GameManager.Get().GameInfo != null
            && GameManager.Get().GameInfo.IsCustomGame
            && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
        {
            AppState_CharacterSelect.Get().Enter();
        }
    }

    private void HandleDisconnectedFromLobbyServer(string lastLobbyErrorMessage)
    {
        Get().Enter(lastLobbyErrorMessage);
    }

    private void HandleShowDailyQuests(QuestOfferNotification quests)
    {
        if (QuestOfferPanel.Get() != null)
        {
            QuestOfferPanel.Get().ShowDailyQuests(quests);
        }
    }

    public void HandleCheckAccountStatusResponse(CheckAccountStatusResponse response)
    {
        if (!response.Success || QuestOfferPanel.Get() == null)
        {
            return;
        }

        FactionCompetition factionCompetition = FactionWideData.Get()
            .GetFactionCompetition(FactionWideData.Get().GetCurrentFactionCompetition());
        AccountComponent.UIStateIdentifier uIStateIdentifier = AccountComponent.UIStateIdentifier.NONE;
        if (factionCompetition != null)
        {
            uIStateIdentifier = factionCompetition.UIToDisplayOnLogin;
        }

        if (uIStateIdentifier != AccountComponent.UIStateIdentifier.NONE)
        {
            if (ClientGameManager.Get().GetPlayerAccountData().AccountComponent.GetUIState(uIStateIdentifier) != 0)
            {
                return;
            }

            if (uIStateIdentifier == AccountComponent.UIStateIdentifier.HasSeenFactionWarSeasonTwoChapterTwo)
            {
                UIFactionsIntroduction.Get().SetupIntro(response.QuestOffers);
                ClientGameManager.Get().RequestUpdateUIState(uIStateIdentifier, 1, null);
            }
            else
            {
                Log.Warning("Did not handle to display ui state {0} on log in", uIStateIdentifier);
            }

            return;
        }

        if (response.QuestOffers.OfferDailyQuest
            && (UIFactionsIntroduction.Get() == null || !UIFactionsIntroduction.Get().IsActive()))
        {
            if (!response.QuestOffers.DailyQuestIds.IsNullOrEmpty())
            {
                QuestOfferPanel.Get().ShowDailyQuests(response.QuestOffers);
            }
            else
            {
                Log.Error("CheckForDailyQuestsResponse offered daily quests with no ID's");
            }
        }
    }
}