using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public class UIScreenManager : MonoBehaviour
{
    public float m_chatDisplayTime = 1f;
    public float m_chatRecentChatDisplayTime = 20f;

    private static UIScreenManager s_instance;

    private Regex m_screenshotNameRegex = new Regex("[S,s]creenshot\\d{8}.png");
    private bool m_HideHUD;
    private bool m_HideHUDDebug;
    private bool m_wasInGroup;
    private bool m_SetHandler;
    private float normalXPLoopStartTime;
    private float ggXPLoopStartTime;

    private const float timeOutXPLoopSound = 20f;
    private const string screenshotsFolder = "Screenshots";

    public void EndAllLoopSounds()
    {
        EndNormalXPLoop();
        EndGGXPLoop();
    }

    public void PlayNormalXPLoop(bool endLoop = false)
    {
        if (normalXPLoopStartTime < 0f)
        {
            AudioManager.PostEvent("ui/endgame/points/counter_normal_loop");
            normalXPLoopStartTime = Time.time;
        }

        if (endLoop)
        {
            EndNormalXPLoop();
        }
    }

    public void EndNormalXPLoop()
    {
        normalXPLoopStartTime = -1f;
        if (UISounds.GetUISounds() != null)
        {
            UISounds.GetUISounds().Stop("ui/endgame/points/counter_normal_loop");
        }
    }

    public void PlayGGBoostXPLoop(bool endLoop = false)
    {
        if (ggXPLoopStartTime < 0f)
        {
            AudioManager.PostEvent("ui/endgame/points/counter_ggboost_loop");
            ggXPLoopStartTime = Time.time;
        }

        if (endLoop)
        {
            EndGGXPLoop();
        }
    }

    public void EndGGXPLoop()
    {
        ggXPLoopStartTime = -1f;
        if (UISounds.GetUISounds() != null)
        {
            UISounds.GetUISounds().Stop("ui/endgame/points/counter_ggboost_loop");
        }
    }

    private void Awake()
    {
        normalXPLoopStartTime = -1f;
        ggXPLoopStartTime = -1f;
        m_wasInGroup = false;
        s_instance = this;
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    public void HandleGroupUpdateNotification()
    {
        bool joinedGroupNonDraft = false;
        if (!m_wasInGroup && ClientGameManager.Get().GroupInfo.InAGroup)
        {
            if (AppState_RankModeDraft.Get() != AppState.GetCurrent())
            {
                joinedGroupNonDraft = true;
            }

            AppState_GroupCharacterSelect.Get().NotifyJoinedNewGroup();
        }
        else if (m_wasInGroup)
        {
            if (!ClientGameManager.Get().GroupInfo.InAGroup)
            {
                AppState_GroupCharacterSelect.Get().NotifyDroppedGroup();
            }
        }

        m_wasInGroup = ClientGameManager.Get().GroupInfo.InAGroup;
        if (UIPlayCategoryMenu.Get() != null)
        {
            UIPlayCategoryMenu.Get().UpdateGroupInfo();
        }

        if (UIFrontEnd.Get() != null)
        {
            if (UIFrontEnd.Get().m_playerPanel != null)
            {
                UIFrontEnd.Get().m_playerPanel.NotifyGroupUpdate(ClientGameManager.Get().GroupInfo.Members);
            }

            if (UIFrontEnd.Get().m_frontEndNavPanel != null)
            {
                UIFrontEnd.Get().m_frontEndNavPanel.NotifyGroupUpdate();
            }

            if (UICharacterSelectScreenController.Get() != null)
            {
                UICharacterSelectScreenController.Get().NotifyGroupUpdate();
                UICharacterSelectScreenController.Get().UpdateReadyButton();
            }

            if (AppState.GetCurrent() != AppState_GroupCharacterSelect.Get())
            {
                GameStatus status = GameManager.Get() != null ? GameManager.Get().GameStatus : GameStatus.None;
                if ((status < GameStatus.LoadoutSelecting || status > GameStatus.Started) && joinedGroupNonDraft)
                {
                    UIFrontEnd.Get().m_frontEndNavPanel.PlayBtnClicked(null);
                }
            }
        }
    }

    public bool GetHideHUDCompletely()
    {
        return m_HideHUDDebug;
    }

    public void SetHUDHide(bool visible, bool nameplateVisible, bool hideNameplateText = false, bool hideChat = false)
    {
        bool newHidden = !visible;
        if (m_HideHUD != newHidden)
        {
            m_HideHUD = newHidden;
            if (!m_HideHUDDebug && HUD_UI.Get() != null)
            {
                HUD_UI.Get().SetMainElementsVisible(!m_HideHUD, hideChat);
            }
        }

        Log.Info("HEALTHBARCHECK: HIDE " + m_HideHUDDebug);
        if (!m_HideHUDDebug && HUD_UI.Get() != null)
        {
            Log.Info(
                "HEALTHBARCHECK: pos "
                + (HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.transform as RectTransform)
                .localPosition);
            Log.Info(
                "HEALTHBARCHECK: size "
                + (HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.transform as RectTransform)
                .sizeDelta);
            CanvasGroup nameplate = HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetComponent<CanvasGroup>();
            nameplate.alpha = nameplateVisible ? 1f : 0f;
            HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetTextVisible(!hideNameplateText);
            HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCombatTextVisible(!hideNameplateText);
        }
    }

    public void SetHUDHideDebug(
        bool visible,
        bool nameplateVisible,
        bool hideNameplateText = false,
        bool hideChat = false)
    {
        m_HideHUDDebug = !visible;
        if (HUD_UI.Get() != null)
        {
            HUD_UI.Get().SetMainElementsVisible(!m_HideHUDDebug, hideChat);
            CanvasGroup nameplate = HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.GetComponent<CanvasGroup>();
            nameplate.alpha = nameplateVisible ? 1f : 0f;
            HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetTextVisible(!hideNameplateText);
            HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.SetCombatTextVisible(!hideNameplateText);
        }

        if (UIFrontEnd.Get() != null)
        {
            foreach (CanvasGroup canvasGroup in UIFrontEnd.Get().m_frontendCanvasContainers)
            {
                SetCanvasGroupForVis(canvasGroup, visible);
            }

            if (UICharacterSelectScreenController.Get() != null
                && UICharacterSelectScreenController.Get().buttonContainer != null)
            {
                UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().buttonContainer, visible);
            }

            if (UICharacterSelectScreen.Get() != null)
            {
                UIManager.SetGameObjectActive(UICharacterSelectScreen.Get().transform.parent, visible);
            }

            if (UICharacterScreen.Get() != null)
            {
                UICharacterScreen.Get().DoRefreshFunctions(ushort.MaxValue);
            }

            if (UIChatBox.Get() != null)
            {
                UIManager.SetGameObjectActive(UIChatBox.Get(), visible);
            }

            if (UICharacterSelectWorldObjects.Get() != null)
            {
                if (UICharacterSelectWorldObjects.Get().m_objectsToHideForToggleUI != null)
                {
                    foreach (GameObject current in UICharacterSelectWorldObjects.Get().m_objectsToHideForToggleUI)
                    {
                        if (current != null)
                        {
                            UIManager.SetGameObjectActive(current, visible);
                        }
                    }
                }

                UICharacterSelectRing[] ringAnimations = UICharacterSelectWorldObjects.Get().m_ringAnimations;
                if (ringAnimations != null)
                {
                    foreach (UICharacterSelectRing ringAnimation in ringAnimations)
                    {
                        if (ringAnimation != null)
                        {
                            ringAnimation.PlayAnimation("ReadyOut");
                        }
                    }
                }
            }

            if (NavigationBar.Get() != null)
            {
                UIManager.SetGameObjectActive(NavigationBar.Get(), visible);
            }
        }
    }

    private void SetCanvasGroupForVis(CanvasGroup canvasGroup, bool visible)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.blocksRaycasts = visible;
            canvasGroup.interactable = visible;
        }
    }

    public static UIScreenManager Get()
    {
        return s_instance;
    }

    public void ClearAllPanels()
    {
        if (UIGameOverScreen.Get() != null)
        {
            UIGameOverScreen.Get().SetVisible(false);
        }

        if (UIActorDebugPanel.Get() != null)
        {
            UIManager.SetGameObjectActive(UIActorDebugPanel.Get(), false);
            UIActorDebugPanel.Get().Reset();
        }

        if (UIDebugMenu.Get() != null)
        {
            UIManager.SetGameObjectActive(UIDebugMenu.Get().m_container, false);
        }

        if (UILoadingScreenPanel.Get() != null)
        {
            UILoadingScreenPanel.Get().SetVisible(false);
        }
    }

    private void UpdateXPLoopTime()
    {
        if (normalXPLoopStartTime > 0f && Time.time - normalXPLoopStartTime >= timeOutXPLoopSound)
        {
            normalXPLoopStartTime = -1f;
            EndNormalXPLoop();
        }

        if (ggXPLoopStartTime > 0f && Time.time - ggXPLoopStartTime >= timeOutXPLoopSound)
        {
            ggXPLoopStartTime = -1f;
            EndGGXPLoop();
        }
    }

    private void UpdateEscapeKeyHit()
    {
        if (UIFrontEnd.Get() == null
            || UIFrontEnd.Get().m_frontEndNavPanel == null
            || !UIFrontEnd.Get().m_frontEndNavPanel.gameObject.activeInHierarchy
            || !Input.GetKeyDown(KeyCode.Escape)
            || !UIFrontEnd.Get().CanMenuEscape())
        {
            return;
        }

        if (Options_UI.Get().IsVisible())
        {
            Options_UI.Get().ToggleOptions();
        }
        else if (KeyBinding_UI.Get().IsVisible())
        {
            if (!KeyBinding_UI.Get().IsSettingKeybindCommand())
            {
                KeyBinding_UI.Get().ToggleKeybinds();
            }
        }
        else if (QuestListPanel.Get().IsVisible())
        {
            QuestListPanel.Get().SetVisible(false);
        }
        else if (UILandingPageFullScreenMenus.Get().IsActive())
        {
            UILandingPageFullScreenMenus.Get().CloseMenu();
        }
        else if (AppState.GetCurrent() != AppState_FullScreenMovie.Get())
        {
            UIFrontEnd.Get().m_frontEndNavPanel.MenuBtnClicked(null);
        }
    }

    private void UpdateToggleHUDKey()
    {
        if (InputManager.Get() == null)
        {
            return;
        }

        if (GameFlowData.Get() != null)
        {
            GameState gameState = GameFlowData.Get().gameState;
            if (gameState == GameState.BothTeams_Decision || gameState == GameState.BothTeams_Resolve)
            {
                if (HUD_UI.Get() != null
                    && InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleHUD))
                {
                    bool newHidden = !m_HideHUDDebug;
                    SetHUDHideDebug(!newHidden, !newHidden, false, true);
                    LineData.SetAllowMovementLinesVisibleForHud(!newHidden);
                }

                if (HUD_UI.Get() != null
                    && InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleHUDExceptNameplates))
                {
                    bool newHidden = !m_HideHUDDebug;
                    SetHUDHideDebug(!newHidden, true, newHidden);
                    GameFlowData.Get().activeOwnedActorData.GetActorController()
                        .SetMovementDistanceLinesVisible(!newHidden);
                }
            }
        }
        else if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.ToggleHUD))
        {
            bool newHidden = !m_HideHUDDebug;
            SetHUDHideDebug(!newHidden, !newHidden);
        }

        if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.TakeScreenShot))
        {
            if (!Directory.Exists(screenshotsFolder))
            {
                Directory.CreateDirectory(screenshotsFolder);
            }

            int index = 0;
            foreach (FileInfo fileInfo in new DirectoryInfo(screenshotsFolder).GetFiles())
            {
                Match match = m_screenshotNameRegex.Match(fileInfo.Name);
                if (match != null)
                {
                    Regex regex = new Regex("\\d{8}");
                    Match match2 = regex.Match(match.Value);
                    int lastIndex = int.Parse(match2.Value);
                    if (index <= lastIndex)
                    {
                        index = lastIndex + 1;
                    }
                }
            }

            int superSize = 1;
            if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
            {
                superSize = 4;
            }

            Application.CaptureScreenshot(
                string.Format(
                    Application.dataPath + "/../ScreenShots/Screenshot{0}.png",
                    FormatNumberForScreenshotIndex(index)),
                superSize);
        }
    }

    private void UpdateCheckSetHandler()
    {
        if (ClientGameManager.Get() != null && !m_SetHandler)
        {
            m_SetHandler = true;
            ClientGameManager.Get().OnGroupUpdateNotification += HandleGroupUpdateNotification;
        }
    }

    private void Update()
    {
        UpdateXPLoopTime();
        UpdateEscapeKeyHit();
        UpdateToggleHUDKey();
        UpdateCheckSetHandler();
    }

    public string FormatNumberForScreenshotIndex(int index)
    {
        string text = index.ToString();
        while (text.Length < 8)
        {
            text = "0" + text;
        }

        return text;
    }

    public void TryLoadAndSetupInGameUI()
    {
        if (ClientGameManager.Get().InGameUIActivated)
        {
            Log.Info("OnHUD_UILoaded called, UI already activated.");
            return;
        }

        Log.Info("OnHUD_UILoaded called.");
        if (HUD_UI.Get() != null)
        {
            Log.Info("HEALTHBARCHECK: Entrance success");
            if (HUD_UI.Get().m_textConsole != null)
            {
                UIManager.SetGameObjectActive(HUD_UI.Get().m_textConsole, true);
            }

            if (HUD_UI.Get().m_mainScreenPanel != null)
            {
                HUD_UI.Get().m_mainScreenPanel.NotifyStartGame();
                HUD_UI.Get().m_mainScreenPanel.SetVisible(true);
                HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.ProcessTeams();
            }

            ClientGameManager.Get().InGameUIActivated = true;
        }
    }
}