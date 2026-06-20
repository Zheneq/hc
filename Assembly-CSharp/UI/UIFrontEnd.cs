using LobbyGameClientMessages;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIFrontEnd : MonoBehaviour
{
    public bool m_enableBuyButtons;
    public float m_baseRingMovingSpeed = 0.75f;
    public float m_cameraMovementSpeed = 1f;
    public float m_cameraRotationSpeed = 5f;
    public float m_panelMovementSpeed = 1f;
    public float m_whisperSoundThreshold = 5f;
    public Camera m_HUDPanelCamera;
    public CameraPositions[] m_cameraPositions;
    public FrontEndScreenState m_currentScreen;
    public CanvasGroup[] m_frontendCanvasContainers;

    public static int s_firstLogInQuestCount = -1;

    private static UIFrontEnd s_instance;
    private Vector3 m_lookAtOffset;
    private float m_rotationStartLength;
    private float m_charCamStartTime;
    private FrontEndScene m_currentCameraPosition;
    private bool m_isStartDrag;
    private bool m_isDragging;
    private bool m_justStoppedDragging;
    private Vector3 m_currentRotationOffset;
    private Vector3 m_startDragPosition;
    private Vector3 m_startRotation;
    private Vector3 m_lastMouseLocation;
    private bool m_isVisible;
    private bool m_attachedHandler;

    public FrontEndNavPanel m_frontEndNavPanel => FrontEndNavPanel.Get();
    public UIPlayerNavPanel m_playerPanel => UIPlayerNavPanel.Get();
    public UILandingPageScreen m_landingPageScreen => UILandingPageScreen.Get();
    public UIJoinGameScreen m_joinGameScreen => UIJoinGameScreen.Get();
    public UICreateGameScreen m_createGameScreen => UICreateGameScreen.Get();
    public UITextConsole m_frontEndChatConsole => UIChatBox.GetChatBox(UIManager.ClientState.InFrontEnd);

    public static UIFrontEnd Get()
    {
        return s_instance;
    }

    public static void PlaySound(FrontEndButtonSounds sound)
    {
        switch (sound)
        {
            case FrontEndButtonSounds.RankFreelancerSelectClick:
                break;
            case FrontEndButtonSounds.SeasonTransitionSeasonPoints:
            case FrontEndButtonSounds.SeasonTransitionReactorPoints:
                break;
            case FrontEndButtonSounds.Back:
                AudioManager.PostEvent("ui/frontend/v1/btn/back");
                break;
            case FrontEndButtonSounds.Cancel:
                AudioManager.PostEvent("ui/frontend/v1/btn/cancel");
                break;
            case FrontEndButtonSounds.Generic:
                AudioManager.PostEvent("ui/frontend/v1/btn/generic");
                break;
            case FrontEndButtonSounds.GenericSmall:
                AudioManager.PostEvent("ui/frontend/v1/btn/generic_small");
                break;
            case FrontEndButtonSounds.GameModeSelect:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
                AudioManager.PostEvent("ui/frontend/v1/btn/gamemode_select");
                break;
            case FrontEndButtonSounds.MenuOpen:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/open");
                break;
            case FrontEndButtonSounds.MenuChoice:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
                break;
            case FrontEndButtonSounds.SubMenuOpen:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/submenu/open");
                break;
            case FrontEndButtonSounds.SelectChoice:
                AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
                break;
            case FrontEndButtonSounds.SelectColorChoice:
                AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
                break;
            case FrontEndButtonSounds.TeamMemberSelect:
                AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
                break;
            case FrontEndButtonSounds.TeamMemberCancel:
                AudioManager.PostEvent("ui/frontend/v1/notify/teammember_cancel");
                break;
            case FrontEndButtonSounds.StartGameReady:
                AudioManager.PostEvent("ui/frontend/btn_ready_start_game");
                break;
            case FrontEndButtonSounds.PlayCategorySelect:
                AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
                break;
            case FrontEndButtonSounds.OptionsChoice:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
                break;
            case FrontEndButtonSounds.TopMenuSelect:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
                break;
            case FrontEndButtonSounds.OptionsOK:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/button");
                break;
            case FrontEndButtonSounds.OptionsCancel:
                AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel");
                break;
            case FrontEndButtonSounds.CharacterSelectCharacter:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/character");
                break;
            case FrontEndButtonSounds.Close:
                AudioManager.PostEvent("ui/frontend/v1/btn/close");
                break;
            case FrontEndButtonSounds.CharacterSelectOpen:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/submenu/open");
                AudioManager.PostEvent("ui/frontend/v1/wnd/charselect/open");
                break;
            case FrontEndButtonSounds.CharacterSelectClose:
                AudioManager.PostEvent("ui/frontend/v1/wnd/charselect/close");
                break;
            case FrontEndButtonSounds.CharacterSelectOptions:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/options");
                break;
            case FrontEndButtonSounds.CharacterSelectOptionsChoice:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/options/choice");
                break;
            case FrontEndButtonSounds.CharacterSelectSkinChoice:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
                break;
            case FrontEndButtonSounds.CharacterSelectModUnlocked:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/unlock");
                break;
            case FrontEndButtonSounds.CharacterSelectModAdd:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
                break;
            case FrontEndButtonSounds.CharacterSelectModClear:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/clear");
                break;
            case FrontEndButtonSounds.CharacterSelectNotifyCharLoaded:
                AudioManager.PostEvent("ui/frontend/v1/notify/charselect/loaded");
                break;
            case FrontEndButtonSounds.StorePurchased:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
                break;
            case FrontEndButtonSounds.StoreCurrencySelect:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
                break;
            case FrontEndButtonSounds.StoreGGPackSelect:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
                break;
            case FrontEndButtonSounds.TutorialPhaseIconAppear:
                AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/appear");
                break;
            case FrontEndButtonSounds.TutorialPhaseIconImpact:
                AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/impact");
                break;
            case FrontEndButtonSounds.TutorialPhaseIconHighlight:
                AudioManager.PostEvent("ui/frontend/v2/menu/tutorial/phase/icon/highlight");
                break;
            case FrontEndButtonSounds.DialogBoxOpen:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/window/appear");
                break;
            case FrontEndButtonSounds.DialogBoxButton:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/button");
                break;
            case FrontEndButtonSounds.GGButtonInGameUsed:
                AudioManager.PostEvent("ui/ingame/ggboost_button");
                break;
            case FrontEndButtonSounds.GGPackUsedNotification:
                AudioManager.PostEvent("ui/ingame/ggboost_button");
                break;
            case FrontEndButtonSounds.GGButtonEndGameUsed:
                AudioManager.PostEvent("ui/endgame/ggboost_button");
                break;
            case FrontEndButtonSounds.NotifyWarning:
                AudioManager.PostEvent("ui/frontend/v2/notify/warning");
                break;
            case FrontEndButtonSounds.MainMenuOpen:
                AudioManager.PostEvent("ui/frontend/v2/menu/mainmenu_open");
                break;
            case FrontEndButtonSounds.MainMenuClose:
                AudioManager.PostEvent("ui/frontend/v2/menu/mainmenu_close");
                break;
            case FrontEndButtonSounds.NotifyMatchFound:
                AudioManager.PostEvent("ui/frontend/v1/notify/match_found");
                break;
            case FrontEndButtonSounds.WhisperMessage:
                AudioManager.PostEvent("ui/frontend/v1/chat/whisper_notify");
                break;
            case FrontEndButtonSounds.LockboxAppear:
                AudioManager.PostEvent("ui/lockbox/appear");
                break;
            case FrontEndButtonSounds.LockboxHit:
                AudioManager.PostEvent("ui/lockbox/hit");
                break;
            case FrontEndButtonSounds.LockboxUnlock:
                AudioManager.PostEvent("ui/lockbox/unlock");
                break;
            case FrontEndButtonSounds.InventoryCraftClick:
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.InventorySalvage:
                AudioManager.PostEvent("ui/lockbox/salvage");
                break;
            case FrontEndButtonSounds.LockboxSelect:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
                break;
            case FrontEndButtonSounds.InventoryFilterSelect:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
                break;
            case FrontEndButtonSounds.CraftButtonClick:
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.SeasonChallengeButtonClick:
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.LockboxUnlockUncommon:
                AudioManager.PostEvent("ui/lockbox/unlock_uncommon");
                break;
            case FrontEndButtonSounds.LockboxUnlockRare:
                AudioManager.PostEvent("ui/lockbox/unlock_rare");
                break;
            case FrontEndButtonSounds.LockboxUnlockEpic:
                AudioManager.PostEvent("ui/lockbox/unlock_epic");
                break;
            case FrontEndButtonSounds.LockboxUnlockLegendary:
                AudioManager.PostEvent("ui/lockbox/unlock_legendary");
                break;
            case FrontEndButtonSounds.LockboxOkCloseButton:
                AudioManager.PostEvent("ui/frontend/v1/btn/options/ok");
                break;
            case FrontEndButtonSounds.LockboxCancelButton:
                AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel");
                break;
            case FrontEndButtonSounds.GeneralGetMoreCredits:
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.GeneralExternalWebsite:
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.InventoryTab:
                AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
                break;
            case FrontEndButtonSounds.InventoryItemSelect:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
                break;
            case FrontEndButtonSounds.InventorySchematicListSelect:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
                break;
            case FrontEndButtonSounds.LockboxOpenClick:
                AudioManager.PostEvent("ui_btn_menu_click");
                AudioManager.PostEvent("ui/lockbox/hit");
                break;
            case FrontEndButtonSounds.InventoryCollectAllClick:
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.InventorySalvageAllClick:
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.InventoryCollect:
                AudioManager.PostEvent("ui/lockbox/collect");
                break;
            case FrontEndButtonSounds.SeasonsChapterTab:
                AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
                break;
            case FrontEndButtonSounds.SeasonsBuyMoreLevels:
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.SeasonsBuyLevelsSelect:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
                break;
            case FrontEndButtonSounds.SeasonsChallengeClickExpand:
                AudioManager.PostEvent("ui/frontend/v1/btn/menu/choice");
                AudioManager.PostEvent("ui/seasons/challenge_button_click_expand");
                break;
            case FrontEndButtonSounds.SeasonsChallengeTrashcanClick:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
                break;
            case FrontEndButtonSounds.SeasonsChallengeTrashcanYes:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
                AudioManager.PostEvent("ui_btn_menu_click");
                break;
            case FrontEndButtonSounds.SeasonsChallengeTrashcanNo:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
                AudioManager.PostEvent("ui/frontend/v1/btn/options/cancel");
                break;
            case FrontEndButtonSounds.ItemCrafting:
                AudioManager.PostEvent("ui/lockbox/craft");
                break;
            case FrontEndButtonSounds.InGameTauntClick:
                AudioManager.PostEvent("ui/ingame/v1/taunt_click");
                break;
            case FrontEndButtonSounds.InGameTauntSelect:
                AudioManager.PostEvent("ui/ingame/v1/taunt_select");
                break;
            case FrontEndButtonSounds.DailyQuestChoice:
                AudioManager.PostEvent("ui/frontend/v1/btn/charselect/mod/add");
                break;
            case FrontEndButtonSounds.DeploymentBegin:
                AudioManager.PostEvent("ui/ingame/notify/match_start");
                break;
            case FrontEndButtonSounds.MaxEnergyReached:
                AudioManager.PostEvent("ui/ingame/v1/energy_max");
                break;
            case FrontEndButtonSounds.FirstTenGamesPregressComplete:
                AudioManager.PostEvent("ui/endgame/firsttengames_progress_complete");
                break;
            case FrontEndButtonSounds.FirstTenGamesProgressIncrement:
                AudioManager.PostEvent("ui/endgame/firsttengames_progress_increment");
                break;
            case FrontEndButtonSounds.HudLockIn:
                AudioManager.PostEvent("ui/ingame/v1/hud/lockin");
                break;
            case FrontEndButtonSounds.RankModeTimerTick:
                AudioManager.PostEvent("ui/frontend/ranked/timer_tick");
                break;
            case FrontEndButtonSounds.RankModeBanPlayer:
                AudioManager.PostEvent("ui/frontend/ranked/ban_player");
                break;
            case FrontEndButtonSounds.RankModePickPlayer:
                AudioManager.PostEvent("ui/frontend/ranked/pick_player");
                break;
            case FrontEndButtonSounds.RankDropdownClick:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
                break;
            case FrontEndButtonSounds.RankDropdownSelect:
                AudioManager.PostEvent("ui/frontend/v2/menu/dialog/option/01");
                break;
            case FrontEndButtonSounds.RankTabClick:
                AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
                break;
            case FrontEndButtonSounds.RankQueueButtonClick:
                AudioManager.PostEvent("ui/frontend/btn_ready_start_game");
                break;
            case FrontEndButtonSounds.RankFreelancerClick:
                AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
                break;
            case FrontEndButtonSounds.RankFreelancerSettingTab:
                AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
                break;
            case FrontEndButtonSounds.RankFreelancerLockin:
                AudioManager.PostEvent("ui/frontend/ranked/lockin");
                break;
            case FrontEndButtonSounds.RankFreelancerSwapClick:
                AudioManager.PostEvent("ui/frontend/v1/notify/teammember_select");
                break;
            case FrontEndButtonSounds.OverconUsed:
                AudioManager.PostEvent("ui/ingame/v1/overcon_generic");
                break;
            case FrontEndButtonSounds.PurchaseComplete:
                AudioManager.PostEvent("ui/frontend/store/purchasecomplete");
                break;
            case FrontEndButtonSounds.ContractsTab:
                AudioManager.PostEvent("ui/frontend/v2/menu/tab/selection");
                break;
            case FrontEndButtonSounds.SeasonTransitionIntro:
                AudioManager.PostEvent("ui/endgame/unlock");
                break;
            case FrontEndButtonSounds.SeasonTransitionRewardDisplay:
                AudioManager.PostEvent("ui/seasons/endseason_SeasonTransitionRewardDisplay");
                break;
            case FrontEndButtonSounds.SeasonTransitionScoreCircle1:
                AudioManager.PostEvent("ui/seasons/endseason_scorecircle_01");
                break;
            case FrontEndButtonSounds.SeasonTransitionScoreCircle2:
                AudioManager.PostEvent("ui/seasons/endseason_scorecircle_02");
                break;
            case FrontEndButtonSounds.EndGameBadgeBasic:
                AudioManager.PostEvent("ui/endgame/badge/basic");
                break;
            case FrontEndButtonSounds.EndGameBadgeAchievement:
                AudioManager.PostEvent("ui/endgame/badge/achievement");
                break;
        }
    }

    public static void PlayLoopingSound(FrontEndButtonSounds sound)
    {
        if (sound == FrontEndButtonSounds.SeasonTransitionSeasonPoints)
        {
            AudioManager.PostEvent("ui/endgame/points/counter_normal_loop", AudioManager.EventAction.PlaySound);
        }
        else if (sound == FrontEndButtonSounds.SeasonTransitionReactorPoints)
        {
            AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop", AudioManager.EventAction.PlaySound);
        }
    }

    public static void StopLoopingSound(FrontEndButtonSounds sound)
    {
        if (sound == FrontEndButtonSounds.SeasonTransitionSeasonPoints)
        {
            AudioManager.PostEvent("ui/endgame/points/counter_normal_loop", AudioManager.EventAction.StopSound);
        }
        else if (sound == FrontEndButtonSounds.SeasonTransitionReactorPoints)
        {
            AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop", AudioManager.EventAction.StopSound);
        }
    }

    public static bool IsMapTypeName(string name)
    {
        switch (name)
        {
            case "Practice":
            case "Tutorial":
            case "Deathmatch":
            case "Testing":
                return true;
            default:
                return false;
        }
    }

    public static string GetSceneDescription(string sceneName)
    {
        if (sceneName.IsNullOrEmpty())
        {
            return null;
        }

        string[] parts = sceneName.Split('_');
        if (parts.Length < 2 || !IsMapTypeName(parts[1]))
        {
            return sceneName;
        }

        return parts.Length == 2
            ? $"{parts[0]} ({parts[1]})"
            : string.Format("{0} {2} ({1})", parts[0], parts[1], parts[2]);
    }

    private void Awake()
    {
        s_instance = this;
        if (gameObject.transform.parent == null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public bool IsLobbyOwner()
    {
        return false;
    }

    public void EnableFrontendEnvironment(bool enableEnvironment)
    {
        if (!enableEnvironment)
        {
            Log.Info(Log.Category.Loading, "PKFxManager.DeepReset leaving frontend");
            PKFxManager.DeepReset();
        }

        UIManager uIManager = UIManager.Get();
        uIManager.SetGameState(enableEnvironment ? UIManager.ClientState.InFrontEnd : UIManager.ClientState.InGame);
    }

    public bool IsProgressScreenOpen()
    {
        return UIPlayerProgressPanel.Get() != null
               && UIPlayerProgressPanel.Get().gameObject != null
               && UIPlayerProgressPanel.Get().gameObject.activeInHierarchy;
    }

    public bool IsStoreOpen()
    {
        return UIStoreViewHeroPage.Get().IsVisible() || UIStorePanel.Get().IsVisible();
    }

    public void TogglePlayerProgressScreenVisibility(bool needToUpdatePlayerProgress = true)
    {
        if (UIMatchStartPanel.Get().IsVisible())
        {
            return;
        }

        bool isVisible = UIPlayerProgressPanel.Get().IsVisible();
        UIPlayerProgressPanel.Get().SetVisible(!isVisible, needToUpdatePlayerProgress);
        if (isVisible)
        {
            UIPlayerProgressPanel.Get().LogPlayerChanges();
        }
    }

    public void ToggleStoreVisibility()
    {
        UIStorePanel.Get().ToggleStore();
        UIPlayerProgressPanel.Get().SetVisible(false);
    }

    public void TogglePlayerFriendListVisibility()
    {
        FriendListPanel.Get().SetVisible(!FriendListPanel.Get().IsVisible());
    }

    public void ShowScreen(FrontEndScreenState newScreen, bool refreshOnly = false)
    {
        m_currentScreen = newScreen;
        if (UICharacterSelectScreenController.Get() != null)
        {
            UICharacterSelectScreenController.Get().SetupReadyButton();
        }

        bool showCharacterSelect = false;
        bool showPlayerPanel = false;
        bool showLandingPage = false;
        bool showJoinGameScreen = false;
        bool showCreateGameScreen = false;
        bool enableChat = false;
        bool changeChatRoom = false;
        switch (newScreen)
        {
            case FrontEndScreenState.LandingPage:
            {
                showLandingPage = true;
                enableChat = true;
                showPlayerPanel = true;
                changeChatRoom = true;
                Get().m_frontEndNavPanel.SetPlayMenuCatgeoryVisible(false);
                break;
            }
            case FrontEndScreenState.CharacterSelect:
            case FrontEndScreenState.GroupCharacterSelect:
            {
                if (UILootMatrixScreen.Get() != null
                    && !UILootMatrixScreen.Get().IsVisible
                    && UIStorePanel.Get() != null
                    && !UIStorePanel.Get().IsVisible())
                {
                    showCharacterSelect = true;
                    showPlayerPanel = true;
                    enableChat = true;
                    changeChatRoom = true;
                }
                else
                {
                    if (!AppState.IsInGame()
                        && GameManager.Get().GameInfo != null
                        && GameManager.Get().GameInfo.IsCustomGame
                        && GameManager.Get().GameInfo.GameStatus != GameStatus.Stopped)
                    {
                        showCharacterSelect = true;
                        UILootMatrixScreen.Get().SetVisible(false);
                    }

                    showPlayerPanel = true;
                    enableChat = true;
                    changeChatRoom = true;
                }

                break;
            }
            case FrontEndScreenState.RankedModeSelect:
            {
                showPlayerPanel = true;
                enableChat = true;
                changeChatRoom = true;
                break;
            }
            case FrontEndScreenState.GameTypeSelect:
            {
                enableChat = true;
                showPlayerPanel = true;
                break;
            }
            case FrontEndScreenState.JoinGame:
            {
                showJoinGameScreen = true;
                m_joinGameScreen.Setup();
                enableChat = true;
                showPlayerPanel = true;
                break;
            }
            case FrontEndScreenState.WaitingForGame:
            {
                enableChat = true;
                showPlayerPanel = true;
                break;
            }
            case FrontEndScreenState.FoundGame:
            {
                enableChat = true;
                break;
            }
            case FrontEndScreenState.CreateGame:
            {
                showCreateGameScreen = true;
                enableChat = true;
                showPlayerPanel = true;
                break;
            }
        }

        if (ClientGameManager.Get().IsServerLocked)
        {
            showPlayerPanel = false;
        }

        if (m_playerPanel != null)
        {
            m_playerPanel.SetVisible(showPlayerPanel, refreshOnly);
        }

        if (m_joinGameScreen != null)
        {
            m_joinGameScreen.SetVisible(showJoinGameScreen);
        }

        if (m_createGameScreen != null)
        {
            m_createGameScreen.SetVisible(showCreateGameScreen);
        }

        if (!refreshOnly)
        {
            if (UIPlayerProgressPanel.Get() != null)
            {
                UIPlayerProgressPanel.Get().SetVisible(false);
            }

            if (UICharacterSelectWorldObjects.Get() != null)
            {
                UICharacterSelectWorldObjects.Get().SetVisible(showCharacterSelect);
            }

            if (m_landingPageScreen != null)
            {
                m_landingPageScreen.SetVisible(showLandingPage);
            }

            if (UICharacterSelectScreen.Get() != null)
            {
                UICharacterSelectScreen.Get().SetVisible(false);
            }

            if (m_frontEndChatConsole != null)
            {
                UIManager.SetGameObjectActive(m_frontEndChatConsole, enableChat);
            }
        }

        if (UICharacterSelectScreenController.Get() != null)
        {
            UICharacterSelectScreenController.Get().SetVisible(showCharacterSelect, refreshOnly);
        }

        if (changeChatRoom && m_frontEndChatConsole != null)
        {
            m_frontEndChatConsole.ChangeChatRoom();
        }
    }

    public bool IsDraggingModel()
    {
        return m_isDragging || m_justStoppedDragging;
    }

    private void Start()
    {
        UIScreenManager.Get().ClearAllPanels();
        m_currentCameraPosition = FrontEndScene.LobbyScreen;
        SetVisible(false);
    }

    private void OnDestroy()
    {
        ClientGameManager clientGameManager = ClientGameManager.Get();
        if (clientGameManager != null)
        {
            clientGameManager.OnLobbyStatusNotification -= HandleLobbyStatusNotification;
        }

        if (s_instance == this)
        {
            Log.Info(string.Concat(GetType(), " OnDestroy, clearing singleton reference"));
            s_instance = null;
        }
    }

    public void HandleLobbyStatusNotification(LobbyStatusNotification notification)
    {
        if (UIPlayerProgressPanel.Get() == null || !UIPlayerProgressPanel.Get().IsVisible())
        {
            ShowScreen(m_currentScreen, true);
        }
    }

    public void ConfirmExit(UIDialogBox boxReference)
    {
        AppState_Shutdown.Get().Enter();
    }

    public void OnExitGameClick(BaseEventData data)
    {
        PlaySound(FrontEndButtonSounds.MenuChoice);
        UIDialogPopupManager.OpenTwoButtonDialog(
            StringUtil.TR("ExitGameTitle", "Global"),
            StringUtil.TR("ExitGamePrompt", "Global"),
            StringUtil.TR("Yes", "Global"),
            StringUtil.TR("No", "Global"),
            ConfirmExit);
    }

    public void OnCreditsClick(BaseEventData data)
    {
        PlaySound(FrontEndButtonSounds.MenuChoice);
        if (UICreditsScreen.Get() != null)
        {
            UICreditsScreen.Get().SetVisible(true);
        }
        else
        {
            UIDialogPopupManager.OpenOneButtonDialog(
                StringUtil.TR("CreditsTitle", "Global"),
                StringUtil.TR("CreditsBody", "Global"),
                StringUtil.TR("Close", "Global"),
                null,
                20);
        }
    }

    public void ConfirmBack(UIDialogBox boxReference)
    {
        if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
        {
            AppState_LandingPage.Get().Enter(true);
            PlaySound(FrontEndButtonSounds.Back);
        }
        else
        {
            AppState_GameTeardown.Get().Enter();
        }
    }

    public void OnBackClick(BaseEventData data)
    {
        PlaySound(FrontEndButtonSounds.MenuChoice);
        if (AppState.GetCurrent() == AppState_GroupCharacterSelect.Get())
        {
            AppState_LandingPage.Get().Enter(true);
        }
        else
        {
            UIDialogPopupManager.OpenTwoButtonDialog(
                StringUtil.TR("LeavingGame", "Global"),
                StringUtil.TR("QuitGamePrompt", "Global"),
                StringUtil.TR("Yes", "Global"),
                StringUtil.TR("No", "Global"),
                ConfirmBack);
        }
    }

    public void Disable()
    {
        EnableFrontendEnvironment(false);
        if (ClientQualityComponentEnabler.OptimizeForMemory())
        {
            Resources.UnloadUnusedAssets();
        }
    }

    public void SetVisible(bool visible)
    {
        m_isVisible = visible;
        if (UI_Persistent.Get() != null)
        {
            UI_Persistent.Get().NotifyFrontEndVisible(visible);
        }
    }

    public void ResetCharacterRotation()
    {
        m_currentRotationOffset = Vector3.zero;
    }

    public Vector3 GetRotationOffset()
    {
        return m_currentRotationOffset;
    }

    public void Update()
    {
        if (!m_attachedHandler && ClientGameManager.Get() != null)
        {
            ClientGameManager.Get().OnLobbyStatusNotification += HandleLobbyStatusNotification;
            m_attachedHandler = true;
        }

        if (s_firstLogInQuestCount != -1
            && QuestOfferPanel.Get() != null
            && !QuestOfferPanel.Get().IsActive()
            && QuestListPanel.Get() != null
            && Get().m_frontEndNavPanel != null)
        {
            s_firstLogInQuestCount = -1;
            Get().m_frontEndNavPanel.NotificationBtnClicked(null);
        }

        if (!m_isVisible
            || UIManager.Get().GetEnvirontmentCamera() == null
            || DebugParameters.Get() != null && DebugParameters.Get().GetParameterAsBool("DebugCamera"))
        {
            return;
        }

        float num = (Time.time - m_charCamStartTime) * m_cameraRotationSpeed;
        float t = 0f;
        if (m_rotationStartLength != 0f)
        {
            t = num / m_rotationStartLength;
        }

        if (m_lookAtOffset != m_cameraPositions[(int)m_currentCameraPosition].rotation)
        {
            m_lookAtOffset = Vector3.Lerp(m_lookAtOffset, m_cameraPositions[(int)m_currentCameraPosition].rotation, t);
        }

        m_justStoppedDragging = false;
        if (UIManager.Get().GetEnvirontmentCamera() != null)
        {
            if (UICharacterSelectWorldObjects.Get() != null
                && UICharacterSelectWorldObjects.Get().m_ringAnimations[0] != null)
            {
                UIActorModelData ringAnimations = UICharacterSelectWorldObjects.Get().m_ringAnimations[0]
                    .GetComponentInChildren<UIActorModelData>();
                if (Input.GetMouseButtonDown(0)
                    && ringAnimations != null
                    && ringAnimations.MousedOver(UIManager.Get().GetEnvirontmentCamera()))
                {
                    m_isStartDrag = true;
                    m_startDragPosition = Input.mousePosition;
                    m_startRotation = m_currentRotationOffset;
                }
            }

            if (!UIUtils.InputFieldHasFocus() && AccountPreferences.DoesApplicationHaveFocus())
            {
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    m_currentRotationOffset -= new Vector3(0f, 5f, 0f);
                }

                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    m_currentRotationOffset += new Vector3(0f, 5f, 0f);
                }
            }
        }

        if (UICharacterStoreAndProgressWorldObjects.Get() != null
            && UICharacterStoreAndProgressWorldObjects.Get().IsVisible())
        {
            if (UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0] != null)
            {
                UIActorModelData ringAnimations = UICharacterStoreAndProgressWorldObjects.Get().m_ringAnimations[0]
                    .GetComponentInChildren<UIActorModelData>();
                if (Input.GetMouseButtonDown(0)
                    && ringAnimations != null
                    && ringAnimations.MousedOver(UIManager.Get().GetEnvirontmentCamera()))
                {
                    m_isStartDrag = true;
                    m_startDragPosition = Input.mousePosition;
                    m_startRotation = m_currentRotationOffset;
                }
            }
        }

        if (m_isStartDrag && (Input.mousePosition - m_startDragPosition).magnitude > 10f)
        {
            m_startDragPosition = Input.mousePosition;
            m_isStartDrag = false;
            m_isDragging = true;
        }

        if (m_isDragging)
        {
            m_lastMouseLocation = Input.mousePosition;
            Vector3 vector = m_startDragPosition - m_lastMouseLocation;
            m_currentRotationOffset = m_startRotation + new Vector3(0f, vector.x / Screen.width * 360f, 0f);
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (m_isDragging)
            {
                m_justStoppedDragging = true;
            }

            m_isDragging = false;
            m_isStartDrag = false;
        }

        if (GameManager.Get() != null && !GameManager.Get().IsControlpadInputDisabled())
        {
            if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickX) != 0f)
            {
                m_currentRotationOffset += new Vector3(
                    0f,
                    ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.RightStickX) * 10f,
                    0f);
            }

            if (UICharacterSelectScreenController.Get() != null)
            {
                // TODO CLIENT uncontrollably switching characters - we can disable gamepad input server-side
                // CameraControls.CameraRotateClockwiseToggled seems to know to take pauses when button is held 
                if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) > 0f)
                {
                    CharacterType characterType = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
                    bool isValid = false;
                    while (!isValid)
                    {
                        characterType++;
                        if (characterType >= CharacterType.Last)
                        {
                            characterType = CharacterType.BattleMonk;
                        }

                        isValid = GameManager.Get().IsCharacterAllowedForPlayers(characterType);
                    }

                    UIManager.Get().HandleNewSceneStateParameter(
                        new UICharacterScreen.CharacterSelectSceneStateParameters
                        {
                            ClientRequestToServerSelectCharacter = characterType
                        });
                }

                if (ControlpadGameplay.Get().GetAxisValue(ControlpadInputValue.DpadX) < 0f)
                {
                    CharacterType characterType = UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay;
                    bool isValid = false;
                    while (!isValid)
                    {
                        characterType--;
                        if (characterType == CharacterType.None)
                        {
                            characterType = CharacterType.Fireborg;
                        }

                        isValid = GameManager.Get().IsCharacterAllowedForPlayers(characterType);
                    }

                    UIManager.Get().HandleNewSceneStateParameter(
                        new UICharacterScreen.CharacterSelectSceneStateParameters
                        {
                            ClientRequestToServerSelectCharacter = characterType
                        });
                }
            }
        }
    }

    public bool CanMenuEscape()
    {
        return !IsProgressScreenOpen()
               && (UIStorePanel.Get() == null || UIStorePanel.Get().CanOpenMenu())
               && AppState_RankModeDraft.Get() != AppState.GetCurrent()
               && !IsChatWindowFocused()
               && (UIDialogPopupManager.Get() == null || !UIDialogPopupManager.Get().IsDialogBoxOpen());
    }

    public bool IsChatWindowFocused()
    {
        return m_frontEndChatConsole != null
               && m_frontEndChatConsole.IsVisible()
               && (EventSystem.current.currentSelectedGameObject == m_frontEndChatConsole.m_textInput.gameObject
                   || m_frontEndChatConsole.InputJustcleared());
    }

    public void NotifyGameLaunched()
    {
    }

    public static UICharacterWorldObjects GetVisibleCharacters()
    {
        if (UICharacterStoreAndProgressWorldObjects.Get() != null
            && UICharacterStoreAndProgressWorldObjects.Get().IsVisible())
        {
            return UICharacterStoreAndProgressWorldObjects.Get();
        }

        if (UICharacterSelectWorldObjects.Get() != null
            && UICharacterSelectWorldObjects.Get().IsVisible())
        {
            return UICharacterSelectWorldObjects.Get();
        }

        return null;
    }
}