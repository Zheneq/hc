// SERVER
// ROGUES
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

[Serializable]
public class SinglePlayerState
{
    public enum SinglePlayerTag
    {
        PauseTimer,
        RequireShootAndMove,
        DefaultCinematicCam,
        ForceOnCinematicCam,
        ShutdownOnExit,
        AdvanceOnNewTurn,
        RequireMaxPossibleAbilities,
        RequireMoveToAdvanceScriptDestination,
        ClearCooldownsOnEnter,
        DEPRECATED1,
        AdvanceOnMoveEntered,
        AdvanceOnAbilitySelected,
        AdvanceOnAbilityTargeted,
        AdvanceOnLockInEntered,
        DisableCancel,
        RequireChasing,
        RequireDash,
        TeleportToAdvanceScriptDestination,
        ResetHealthOnEnter,
        ResetEnergyOnEnter,
        EnableChatter,
        EnableAutoQueuedAbilitiesForNpcs,
        EnableHiddenMovementText,
        EnableBrush,
        DisableAdvanceTurn,
        AdvanceOnDecisionEnd,
        AdvanceOnResolutionEnd,
        EnableCooldownIndicators,
        ResetContributionOnEnter,
        AdvanceOnTutorialQueueEmpty
    }

    public enum ActorDiesTrigger
    {
        Never,
        AnyActor,
        SpawnedNPCs, // removed in rogues
        Players,
        ClientActor,
        ClientAlly,
        ClientEnemy
    }

    public string m_stateTitle;
    [Tooltip("-1 to allow no abilities, 0-4 for freelancer abilities, 7-9 for catalysts")]
    public int[] m_allowedAbilities;
    public BoardRegion m_allowedDestinations;
    public BoardRegion m_advanceScriptDestinations;
    public BoardRegion m_respawnDestinations;
    public BoardRegion m_allowedTargets;
    public bool m_mustTargetNearCenter;
    public bool m_onlyAllowWaypointMovement;
    public BoardRegion m_rightClickHighlight;
    public string m_rightClickText;
    public float m_rightClickHeight;
    public BoardRegion m_shiftRightClickHighlight;
    public string m_shiftRightClickText;
    public float m_shiftRightClickHeight;
    public BoardRegion m_leftClickHighlight;
    public string m_leftClickText;
    public float m_leftClickHeight;
    public int m_minAbilityTargetsForAiming;
    public string m_bannerText;
    public TextRegion m_tutorialBoxText;
    public TextRegion m_tutorialBoxText2;
    public TextRegion m_tutorialBoxText3;
    public TextRegion m_tutorialCameraMovementText;
    public TextRegion m_tutorialCameraRotationText;
    public string m_errorStringOnForbiddenPath;
    public GameObject m_cameraRotationTarget;
    public float m_advanceAfterSeconds;
    public int m_minPlayerHitPoints;
    public int m_minAllyHitPoints;

    [AudioEvent(false)]
    public string m_audioEventOnPreEnter;
    [AudioEvent(false)]
    public string m_audioEventOnEnter;
    [AudioEvent(false)]
    public string m_audioEventOnExit;

    public SinglePlayerScriptedChat[] m_chatTextOnEnter;
    public string m_tutorialVideoPreviewOnEnter;

    [HideInInspector]
    public bool m_markedForAdvanceState;
    [HideInInspector]
    public int m_stateIndex = -1;
    [HideInInspector]
    public float m_startTime;

    public SinglePlayerTag[] m_tags;
    public ActivatableObject[] m_activationsOnEnter;
    public ActivatableObject[] m_activationsOnExit;
    public ActivatableUI[] m_uiActivationsOnEnter;
    public ActivatableUI[] m_uiActivationsOnExit;
    public string[] m_npcsToSpawnOnEnter;
    public string[] m_npcsToDespawnOnEnter;
    public ActorDiesTrigger m_advanceScriptIfActorDies;
    public int m_advanceScriptIfActorDiesCount;

    [HideInInspector]
    public int m_actorDeaths;

    private HashSet<BoardSquare> m_advanceDestinationsHashSet;

    public void Initialize() // Initialize(Scene s) in rogues here and in all calls in this function
    {
        m_allowedDestinations.Initialize();
        m_advanceScriptDestinations.Initialize();
        m_respawnDestinations.Initialize();
        m_allowedTargets.Initialize();
        m_rightClickHighlight.Initialize();
        m_shiftRightClickHighlight.Initialize();
        m_leftClickHighlight.Initialize();
    }

    public string GetBannerText()
    {
        if (m_bannerText.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_bannerText);
    }

    public string GetRightClickText()
    {
        if (m_rightClickText.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_rightClickText);
    }

    public string GetShiftRightClickText()
    {
        if (m_shiftRightClickText.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_shiftRightClickText);
    }

    public string GetLeftClickText()
    {
        if (m_leftClickText.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_leftClickText);
    }

    public string GetTutorialBoxText()
    {
        if (m_tutorialBoxText.m_text.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_tutorialBoxText.m_text);
    }

    public string GetTutorialBoxText2()
    {
        if (m_tutorialBoxText2.m_text.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_tutorialBoxText2.m_text);
    }

    public string GetTutorialBoxText3()
    {
        if (m_tutorialBoxText3.m_text.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_tutorialBoxText3.m_text);
    }

    public string GetTutorialCameraMovementText()
    {
        if (m_tutorialCameraMovementText.m_text.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_tutorialCameraMovementText.m_text);
    }

    public string GetTutorialCameraRotationText()
    {
        if (m_tutorialCameraRotationText.m_text.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_tutorialCameraRotationText.m_text);
    }

    public string GetErrorStringOnForbiddenPath()
    {
        if (m_errorStringOnForbiddenPath.IsNullOrEmpty())
        {
            return string.Empty;
        }

        return StringUtil.TR(m_errorStringOnForbiddenPath);
    }

    public float GetDuration()
    {
        return Time.unscaledTime - m_startTime;
    }

    public void OnEnteringState()
    {
        m_startTime = Time.unscaledTime;
        LogState("Entering", false);
        foreach (ActivatableObject activatableObject in m_activationsOnEnter)
        {
            if (activatableObject == null)
            {
                Log.Error("Null activation-object in 'Activations On Enter' for state " + m_stateIndex + ".");
            }
            else if (activatableObject.m_sceneObject == null)
            {
                Log.Error(
                    "Activation-object has null scene-object in 'Activations On Enter' for state " + m_stateIndex
                    + ".");
            }
            else
            {
                activatableObject.Activate();
            }
        }

#if !SERVER
        foreach (ActivatableUI activatableUI in m_uiActivationsOnEnter)
        {
            activatableUI.Activate();
        }

        if (m_audioEventOnPreEnter != string.Empty)
        {
            AudioManager.PostEvent(m_audioEventOnPreEnter);
        }

        if (m_audioEventOnEnter != string.Empty)
        {
            AudioManager.PostEvent(m_audioEventOnEnter);
        }

        if (m_chatTextOnEnter != null && m_chatTextOnEnter.Length > 0)
        {
            foreach (SinglePlayerScriptedChat singlePlayerScriptedChat in m_chatTextOnEnter)
            {
                UITutorialPanel.Get().QueueDialogue(
                    singlePlayerScriptedChat.m_text,
                    singlePlayerScriptedChat.m_audioEvent,
                    singlePlayerScriptedChat.m_displaySeconds,
                    singlePlayerScriptedChat.m_sender);
            }
        }

        if (m_tutorialVideoPreviewOnEnter != string.Empty)
        {
            UITutorialDropdownlist.Get().DisplayPreviewVideo(m_tutorialVideoPreviewOnEnter);
        }

        string bannerText = GetBannerText();
        if (bannerText != string.Empty)
        {
            // reactor
            UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, true);
            UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, true);
            // rogues
            // HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar.gameObject.SetActive(true);
            // HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText.gameObject.SetActive(true);
            HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText.text = bannerText;
        }
        else
        {
            // reactor
            UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, false);
            UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, false);
            // rogues
            // HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar.gameObject.SetActive(false);
            // HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText.gameObject.SetActive(false);
        }

        ChatterManager.Get().EnableChatter = GetHasTag(SinglePlayerTag.EnableChatter);
        if (GetHasTag(SinglePlayerTag.DefaultCinematicCam))
        {
            CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Default;
        }
        else if (GetHasTag(SinglePlayerTag.ForceOnCinematicCam))
        {
            CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Always;
        }
        else
        {
            CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Never;
        }
#endif

        if (GetHasTag(SinglePlayerTag.ClearCooldownsOnEnter))
        {
            GameFlowData.Get().ClearCooldowns();
            GameFlowData.Get().RefillStocks();
        }

#if SERVER
        if (GetHasTag(SinglePlayerTag.ResetHealthOnEnter))
        {
            foreach (ActorData actorData in GameFlowData.Get().GetActors())
            {
                if (actorData.IsHumanControlled())
                {
                    actorData.SetHitPoints(actorData.GetMaxHitPoints());
                }
            }
        }

        if (GetHasTag(SinglePlayerTag.ResetEnergyOnEnter))
        {
            foreach (ActorData actorData in GameFlowData.Get().GetActors())
            {
                if (actorData.IsHumanControlled())
                {
                    actorData.SetTechPoints(0);
                }
            }
        }

        if (GetHasTag(SinglePlayerTag.ResetContributionOnEnter))
        {
            foreach (ActorData actorData in GameFlowData.Get().GetActors())
            {
                if (actorData.IsHumanControlled())
                {
                    actorData.GetComponent<ActorBehavior>().Reset();
                }
            }
        }

        CheckMinHitPoints();
        if (GetHasTag(SinglePlayerTag.TeleportToAdvanceScriptDestination))
        {
            ActorData localPlayer = SinglePlayerManager.Get().GetLocalPlayer();
            List<BoardSquare> squaresInRegion = m_advanceScriptDestinations.GetSquaresInRegion();
            if (squaresInRegion.Count > 0)
            {
                localPlayer.TeleportToBoardSquare(
                    squaresInRegion[0],
                    GetSpawnFacing(squaresInRegion[0].ToVector3()),
                    ActorData.TeleportType.Debug,
                    null);
                CameraManager.Get().SetTargetObject(
                    localPlayer.gameObject,
                    CameraManager.CameraTargetReason.SinglePlayerStateScriptCommand);
            }

            m_markedForAdvanceState = true;
        }

        // custom
        if (NPCCoordinator.Get() != null)
        {
            foreach (string spawnerTitle in m_npcsToSpawnOnEnter)
            {
                NPCCoordinator.Get().Spawn(spawnerTitle);
            }
            foreach (string spawnerTitle in m_npcsToDespawnOnEnter)
            {
                NPCCoordinator.Get().Despawn(spawnerTitle);
            }
        }
#endif
    }

    public void OnExitingState()
    {
        LogState("Exiting", true);
        foreach (ActivatableObject activatableObject in m_activationsOnExit)
        {
            if (activatableObject == null)
            {
                Log.Error(
                    "Null activation-object in 'Activations On Exit' for state " + m_stateIndex + ".  Tell Colin.");
            }
            else if (activatableObject.m_sceneObject == null)
            {
                Log.Error(
                    "Activation-object has null scene-object in 'Activations On Exit' for state " + m_stateIndex
                    + ".  Tell Colin.");
            }
            else
            {
                activatableObject.Activate();
            }
        }

#if !SERVER
        foreach (ActivatableUI activatableUI in m_uiActivationsOnExit)
        {
            activatableUI.Activate();
        }

        if (m_audioEventOnExit != string.Empty)
        {
            AudioManager.PostEvent(m_audioEventOnExit);
        }

        if ((bool)CameraManager.Get())
        {
            CameraManager.Get().m_abilityCinematicState = CameraManager.AbilityCinematicState.Default;
        }
#endif

        if (GetHasTag(SinglePlayerTag.ShutdownOnExit))
        {
            GameManager.Get().StopGame();
            GameFlowData.Get().gameState = GameState.EndingGame;

#if !SERVER
            // reactor
            if (GameManager.Get().GameConfig.GameType != GameType.Tutorial)
            {
                UIGameOverScreen.Get().Setup(GameManager.Get().GameConfig.GameType, GameResult.TeamAWon, 1, 0);
            }
            // rogues
            // UIGameOverScreen.Get().Setup(GameType.Custom, GameResult.TeamAWon, 1, 0);
#endif

            if (AppState.GetCurrent() != AppState_GameTeardown.Get()
                && AppState.GetCurrent() != AppState_FrontendLoadingScreen.Get())
            {
                AppState_GameTeardown.Get().Enter();
            }
        }
    }

    public HashSet<BoardSquare> GetAdvanceDestinations()
    {
        if (m_advanceDestinationsHashSet == null && m_advanceScriptDestinations.m_quads.Length > 0)
        {
            m_advanceDestinationsHashSet = new HashSet<BoardSquare>();
            foreach (BoardSquare square in m_advanceScriptDestinations.GetSquaresInRegion())
            {
                m_advanceDestinationsHashSet.Add(square);
            }
        }

        return m_advanceDestinationsHashSet;
    }

    private void LogState(string action, bool eventLog)
    {
        string timeForLogging = MatchLogger.Get().GetTimeForLogging(false);
#if SERVER
        string time = string.Empty;
        if (NetworkServer.active)
        {
            time = MatchLogger.Get().GetTimeForLogging(true);
        }
#endif

        int currentTurn = GameFlowData.Get().CurrentTurn;
        string name = SceneManager.GetActiveScene().name;
        int num = SinglePlayerCoordinator.Get().m_script.Length - 1;
        string state = m_stateTitle != string.Empty
            ? "state '" + m_stateTitle + "' (state " + m_stateIndex + " of " + num + ")"
            : "state " + m_stateIndex + " of " + num;
        Log.Info(
            $"{HydrogenConfig.Get().Ticket.Handle} SinglePlayer level {name}: {action} {state} on turn {currentTurn}.  Time elapsed: {timeForLogging}.");
        
#if SERVER
        if (eventLog
            && NetworkServer.active
            // reactor
            && GameManager.Get().GameConfig.GameType == GameType.Tutorial)
            // rogues
            // && GameManager.Get().GameMission.IsMissionTagActive(MissionData.s_missionTagTutorial))
        {
            EventLogMessage eventLogMessage = new EventLogMessage("tutorial", "ExitState");
            PlayerDetails playerDetails = null;
            using (Dictionary<Player, PlayerDetails>.Enumerator enumerator =
                   GameFlow.Get().playerDetails.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    KeyValuePair<Player, PlayerDetails> keyValuePair = enumerator.Current;
                    playerDetails = keyValuePair.Value;
                }
            }

            if (playerDetails != null)
            {
                eventLogMessage.AddData("AccountId", playerDetails.m_accountId);
                eventLogMessage.AddData("Handle", playerDetails.m_handle);
            }

            eventLogMessage.AddData("Map", name);
            eventLogMessage.AddData("StateTitle", state);
            eventLogMessage.AddData("StateIndex", m_stateIndex);
            eventLogMessage.AddData("CurrentTurn", currentTurn);
            eventLogMessage.AddData("ElapsedTime", time);
            eventLogMessage.AddData("ElapsedTimeExact", MatchLogger.Get().GetExactTimeForLogging());
            eventLogMessage.AddData("ProcessCode", HydrogenConfig.Get().ProcessCode);
            eventLogMessage.Write();
        }
#endif
    }

    public void OnTurnTick()
    {
        CheckMinHitPoints();
    }
    
    private void CheckMinHitPoints()
    {
#if SERVER
        if (!NetworkServer.active)
        {
            return;
        }
        
        foreach (ActorData actorData in GameFlowData.Get().GetActors())
        {
            if (actorData.IsHumanControlled())
            {
                if (m_minPlayerHitPoints > 0 && actorData.HitPoints < m_minPlayerHitPoints)
                {
                    SinglePlayerScriptedChat[] chatTextOnLowHealth =
                        SinglePlayerCoordinator.Get().m_chatTextOnLowHealth;
                    if (!chatTextOnLowHealth.IsNullOrEmpty())
                    {
                        foreach (SinglePlayerScriptedChat chatText in chatTextOnLowHealth)
                        {
                            SinglePlayerManager.Get().CallRpcPlayScriptedChat(chatText);
                        }
                    }

                    actorData.SetHitPoints(actorData.GetMaxHitPoints());
                }
            }
            else if (actorData.GetTeam() == Team.TeamA
                     && m_minAllyHitPoints > 0
                     && actorData.HitPoints < m_minAllyHitPoints)
            {
                actorData.SetHitPoints(actorData.GetMaxHitPoints());
            }
        }
#endif
    }

    public bool GetHasTag(SinglePlayerTag tag)
    {
        foreach (SinglePlayerTag singlePlayerTag in m_tags)
        {
            if (singlePlayerTag == tag)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3 GetSpawnFacing(Vector3 spawnPosition)
    {
        if (m_cameraRotationTarget == null)
        {
            return new Vector3(1f, 0f, 0f);
        }
        
        Vector3 vector = m_cameraRotationTarget.transform.position - spawnPosition;
        vector.y = 0f;
        
        if (Mathf.Abs(vector.x) > Mathf.Abs(vector.z))
        {
            return vector.x > 0f
                ? new Vector3(1f, 0f, 0f)
                : new Vector3(-1f, 0f, 0f);
        }

        return vector.z > 0f
            ? new Vector3(0f, 0f, 1f)
            : new Vector3(0f, 0f, -1f);
    }
}