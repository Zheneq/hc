// SERVER
// ROGUES
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity;
using UnityEngine;
using UnityEngine.Networking;

public class SinglePlayerManager : NetworkBehaviour
{
    private static string m_uniqueNetworkHash;
    private static SinglePlayerManager s_instance;

    [SyncVar(hook = "HookSetCurrentScriptIndex")]
    private int m_currentScriptIndex;
    [SyncVar(hook = "HookSetCanEndTurn")]
    private bool m_canEndTurn = true;
    private bool m_clientCanEndTurn = true;
    private GameObject m_advanceDestinationsHighlight;
    private bool m_pausedTimer;
    private bool m_decisionTimerForceOff;
    private bool m_lockInCancelButtonForceOff;
    private bool m_lockinPhaseDisplayForceOff;
    private bool m_lockinPhaseTextForceOff;
    private bool m_lockinPhaseColorForceOff;
    private bool m_notificationPanelForceOff;

    private bool[] m_teamPlayerIconForceOff = new bool[5];
    private bool[] m_enemyPlayerIconForceOff = new bool[5];

    [HideInInspector] // removed in rogues as useless
    private bool m_errorTriggered;
    private int m_lastTutorialTextState = -1;
    private int m_lastTutorialCameraState = -1;
    private static int kRpcRpcPlayScriptedChat; // removed in rogues

    public int Networkm_currentScriptIndex
    {
        get => m_currentScriptIndex;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetCurrentScriptIndex(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_currentScriptIndex, 1u);
        }
    }

    public bool Networkm_canEndTurn
    {
        get => m_canEndTurn;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetCanEndTurn(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_canEndTurn, 2u);
        }
    }

    static SinglePlayerManager()
    {
        m_uniqueNetworkHash = "FFFFFF24601";
        
        // reactor
        kRpcRpcPlayScriptedChat = 884030896;
        RegisterRpcDelegate(typeof(SinglePlayerManager), kRpcRpcPlayScriptedChat, InvokeRpcRpcPlayScriptedChat);
        NetworkCRC.RegisterBehaviour("SinglePlayerManager", 0);
        // rogues
        // NetworkBehaviour.RegisterRpcDelegate(
        //     typeof(SinglePlayerManager),
        //     "RpcPlayScriptedChat",
        //     new CmdDelegate(InvokeRpcRpcPlayScriptedChat));
    }

    private static GameObject OnSpawn(Vector3 position, NetworkHash128 assetId) // Guid assetId  in rogues
    {
        GameObject gameObject = new GameObject("SinglePlayerManagerGameObject");
        SinglePlayerManager singlePlayerManager = gameObject.AddComponent<SinglePlayerManager>();
        if (!s_instance)
        {
            s_instance = singlePlayerManager;
        }

        return gameObject;
    }

    private static void OnDespawn(GameObject spawned)
    {
        // TODO CLIENT bug
        if (s_instance == spawned)
        {
            s_instance = null;
        }
    }

    // empty in rogues
    public static void RegisterSpawnHandler()
    {
        NetworkHash128 assetId = NetworkHash128.Parse(m_uniqueNetworkHash);
        ClientScene.RegisterSpawnHandler(assetId, OnSpawn, OnDespawn);
    }

    // empty in rogues
    public static void UnregisterSpawnHandler()
    {
        NetworkHash128 assetId = NetworkHash128.Parse(m_uniqueNetworkHash);
        ClientScene.UnregisterSpawnHandler(assetId);
    }

    public static SinglePlayerManager Get()
    {
        if (SinglePlayerCoordinator.Get() == null)
        {
            return null;
        }
        
#if SERVER
        if (NetworkServer.active && s_instance == null)
        {
            GameObject gameObject = new GameObject("SinglePlayerManagerGameObject");
            gameObject.AddComponent<NetworkIdentity>();
            s_instance = gameObject.AddComponent<SinglePlayerManager>();
            // custom
            NetworkHash128 assetId = NetworkHash128.Parse(m_uniqueNetworkHash);
            NetworkServer.Spawn(gameObject, assetId);
            // reactor
            // Guid guid = Guid.Parse(m_uniqueNetworkHash);
            // NetworkServer.Spawn(gameObject, guid);
        }
#endif
        
        return s_instance;
    }

    public void SetScriptIndex(int newIndex)
    {
        while (newIndex > m_currentScriptIndex)
        {
            SinglePlayerState currentState = GetCurrentState();
            if (currentState != null)
            {
                currentState.m_markedForAdvanceState = false;
                currentState.OnExitingState();
            }

            m_errorTriggered = false;
            Networkm_currentScriptIndex = newIndex;
            SinglePlayerState nextState = GetCurrentState();
            if (nextState != null)
            {
                nextState.OnEnteringState();
                bool hasTag = nextState.GetHasTag(SinglePlayerState.SinglePlayerTag.PauseTimer);
                if (NetworkServer.active)
                {
                    if (hasTag != m_pausedTimer)
                    {
                        m_pausedTimer = hasTag;
                        GameFlowData.Get().SetPausedForSinglePlayer(m_pausedTimer);
                    }
                }
            }
#if SERVER
            else if (NetworkServer.active && m_pausedTimer)
            {
                m_pausedTimer = false;
                GameFlowData.Get().SetPausedForSinglePlayer(m_pausedTimer);
            }
#endif

            ActorData localPlayer = GetLocalPlayer();
            if ((bool)localPlayer)
            {
                ActorMovement actorMovement = localPlayer.GetActorMovement();
                if ((bool)actorMovement)
                {
                    actorMovement.UpdateSquaresCanMoveTo();
                }
            }

            UpdateDestinationHighlights();
            RecalcCanEndTurn();
            if (nextState != null && nextState.m_markedForAdvanceState)
            {
                newIndex++;
            }
        }
    }

    public int GetCurrentScriptIndex()
    {
        return m_currentScriptIndex;
    }

    private void HookSetCurrentScriptIndex(int value)
    {
        if (NetworkServer.active)
        {
            return;
        }
        
        SetScriptIndex(value);
    }

    public void SetCanEndTurn(bool canEnd)
    {
        if (!NetworkServer.active)
        {
            return;
        }
        
        Networkm_canEndTurn = canEnd;
    }

    public bool GetCanEndTurnFlag()
    {
        if (NetworkServer.active)
        {
            return m_canEndTurn;
        }

        return m_clientCanEndTurn;
    }

    private void HookSetCanEndTurn(bool value)
    {
        Networkm_canEndTurn = value;
    }

    public void SetDecisionTimerForceOff(bool val)
    {
        m_decisionTimerForceOff = val;
    }

    public bool GetDecisionTimerForceOff()
    {
        return m_decisionTimerForceOff;
    }

    public void SetLockInCancelButtonForceOff(bool val)
    {
        m_lockInCancelButtonForceOff = val;
    }

    public bool GetLockinPhaseDisplayForceOff()
    {
        return m_lockinPhaseDisplayForceOff;
    }

    public void SetLockinPhaseDisplayForceOff(bool val)
    {
        m_lockinPhaseDisplayForceOff = val;
    }

    public bool GetLockinPhaseTextForceOff()
    {
        return m_lockinPhaseTextForceOff;
    }

    public void SetLockinPhaseTextForceOff(bool val)
    {
        m_lockinPhaseTextForceOff = val;
    }

    public bool GetLockinPhaseColorForceOff()
    {
        return m_lockinPhaseColorForceOff;
    }

    public void SetLockinPhaseColorForceOff(bool val)
    {
        m_lockinPhaseColorForceOff = val;
    }

    public bool GetLockInCancelButtonForceOff()
    {
        return m_lockInCancelButtonForceOff;
    }

    public void SetNotificationPanelForceOff(bool val)
    {
        m_notificationPanelForceOff = val;
    }

    public bool GetNotificationPanelForceOff()
    {
        return m_notificationPanelForceOff;
    }

    public void SetTeamPlayerIconForceOff(int index, bool val)
    {
        m_teamPlayerIconForceOff[index] = val;
    }

    public bool GetTeamPlayerIconForceOff(int index)
    {
        return m_teamPlayerIconForceOff[index];
    }

    public void SetEnemyPlayerIconForceOff(int index, bool val)
    {
        m_enemyPlayerIconForceOff[index] = val;
    }

    public bool GetEnemyPlayerIconForceOff(int index)
    {
        return m_enemyPlayerIconForceOff[index];
    }

    public static bool IsCancelDisabled()
    {
        return Get() != null
               && Get().GetCurrentState() != null
               && Get().GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.DisableCancel);
    }

    private void Awake()
    {
        Networkm_currentScriptIndex = -1;
        Networkm_canEndTurn = true;
        m_clientCanEndTurn = true;
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    private void Update()
    {
        SinglePlayerState currentState = GetCurrentState();
        if (currentState == null || currentState.m_advanceAfterSeconds == 0f)
        {
            return;
        }
        
        if (currentState.m_startTime == 0f)
        {
            return;
        }
        
        if (currentState.GetDuration() > currentState.m_advanceAfterSeconds / Time.timeScale)
        {
            AdvanceScript();
        }
    }

    internal void OnDecisionEnd()
    {
        SinglePlayerState currentState = GetCurrentState();
        if (currentState == null)
        {
            return;
        }
        
        if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnDecisionEnd))
        {
            AdvanceScript();
        }
    }

    internal void OnResolutionEnd()
    {
        SinglePlayerState currentState = GetCurrentState();
        if (currentState == null)
        {
            return;
        }
        
        if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnResolutionEnd))
        {
            AdvanceScript();
        }
    }

    public SinglePlayerState GetCurrentState()
    {
        return GetState(m_currentScriptIndex);
    }

    private SinglePlayerState GetState(int scriptIndex)
    {
        if (SinglePlayerCoordinator.Get() != null)
        {
            SinglePlayerState[] script = SinglePlayerCoordinator.Get().m_script;
            if (scriptIndex >= 0 && scriptIndex < script.Length)
            {
                return script[scriptIndex];
            }
        }

        return null;
    }

    public static bool CanEndTurn(ActorData actor)
    {
        return !s_instance
               || actor == null
               || !actor.IsHumanControlled()
               || s_instance.GetCanEndTurnFlag();
    }

    public bool EnableChatter()
    {
        SinglePlayerState currentState = GetCurrentState();
        return currentState == null
               || currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableChatter);
    }

    public bool EnableAutoQueuedAbilitiesForNPCs()
    {
        SinglePlayerState currentState = GetCurrentState();
        return currentState == null
               || currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableAutoQueuedAbilitiesForNpcs);
    }

    public bool EnableHiddenMovementText()
    {
        return GetCurrentState()?.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableHiddenMovementText) ?? true;
    }

    public bool EnableBrush()
    {
        return GetCurrentState()?.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableBrush) ?? true;
    }

    public bool EnableCooldownIndicators()
    {
        return GetCurrentState()?.GetHasTag(SinglePlayerState.SinglePlayerTag.EnableCooldownIndicators) ?? true;
    }

    public bool DisableAdvanceTurn()
    {
        return GetCurrentState()?.GetHasTag(SinglePlayerState.SinglePlayerTag.DisableAdvanceTurn) ?? true;
    }

    public void OnTurnTick()
    {
#if SERVER
        // added in rogues
        if (NetworkServer.active)
        {
            if (SinglePlayerCoordinator.Get() == null)
            {
                return;
            }

            if (m_currentScriptIndex == -1)
            {
                SetScriptIndex(0);
            }

            foreach (ActorData actorData in GameFlowData.Get().GetActors())
            {
                if (actorData.IsHumanControlled())
                {
                    SinglePlayerState currentState = GetCurrentState();
                    if (currentState == null)
                    {
                        break;
                    }

                    currentState.OnTurnTick();
                    if (currentState.m_advanceScriptDestinations != null && currentState.m_advanceScriptDestinations
                            .GetActorsInRegion().Contains(actorData))
                    {
                        currentState.m_markedForAdvanceState = true;
                    }

                    if (currentState.m_minPlayerHitPoints > 0
                        && actorData.HitPoints < currentState.m_minPlayerHitPoints)
                    {
                        actorData.SetHitPoints(actorData.GetMaxHitPoints());
                    }

                    if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnNewTurn))
                    {
                        currentState.m_markedForAdvanceState = true;
                    }

                    if (currentState.m_markedForAdvanceState)
                    {
                        AdvanceScript();
                    }

                    break;
                }
            }
        }
#endif
    }

#if SERVER
    // added in rogues
    private void RespawnActor(ActorData actor)
    {
        if (NetworkServer.active)
        {
            if (SinglePlayerCoordinator.Get() == null)
            {
                return;
            }

            SinglePlayerState currentState = GetCurrentState();
            BoardSquare spawnSquare;
            if (currentState != null && currentState.m_respawnDestinations.m_quads.Length != 0)
            {
                List<BoardSquare> squaresInRegion = currentState.m_respawnDestinations.GetSquaresInRegion();
                if (squaresInRegion.Count > 0)
                {
                    spawnSquare = squaresInRegion[0];
                }
                else
                {
                    spawnSquare = SpawnPointManager.Get().GetSpawnSquare(actor, false);
                }
            }
            else
            {
                spawnSquare = SpawnPointManager.Get().GetSpawnSquare(actor, false);
            }

            actor.GetComponent<ServerActorController>().RespawnOnSquare(spawnSquare);
        }
    }
#endif
    
    public void DebugAdvanceScript() // _001D
    {
        AdvanceScript();
        UITutorialFullscreenPanel.Get().ClearAllPanels();
        UITutorialPanel.Get().ClearAll();
    }

    private void AdvanceScript()
    {
        SetScriptIndex(m_currentScriptIndex + 1);
    }

    public ActorData GetLocalPlayer()
    {
        foreach (ActorData actor in GameFlowData.Get().GetActors())
        {
            if (actor.IsHumanControlled())
            {
                return actor;
            }
        }

        return null;
    }

    public void OnActorDeath(ActorData actor)
    {
        bool advance = false;
        SinglePlayerState currentState = GetCurrentState();
        if (currentState != null)
        {
            ActorData localPlayer = GetLocalPlayer();
            bool advanceForDeath;
            switch (currentState.m_advanceScriptIfActorDies)
            {
                case SinglePlayerState.ActorDiesTrigger.AnyActor:
                {
                    advanceForDeath = true;
                    break;
                }
                case SinglePlayerState.ActorDiesTrigger.ClientActor:
                {
                    advanceForDeath = localPlayer == null || localPlayer == actor;
                    break;
                }
                case SinglePlayerState.ActorDiesTrigger.ClientAlly:
                {
                    advanceForDeath = localPlayer == null || localPlayer.GetTeam() == actor.GetTeam();
                    break;
                }
                case SinglePlayerState.ActorDiesTrigger.ClientEnemy:
                {
                    advanceForDeath = localPlayer == null || localPlayer.GetTeam() != actor.GetTeam();
                    break;
                }
                case SinglePlayerState.ActorDiesTrigger.Never:
                {
                    advanceForDeath = false;
                    break;
                }
                case SinglePlayerState.ActorDiesTrigger.SpawnedNPCs: // case removed in rogues
                {
                    advanceForDeath = NPCCoordinator.IsSpawnedNPC(actor);
                    break;
                }
                case SinglePlayerState.ActorDiesTrigger.Players:
                {
                    advanceForDeath = GameplayUtils.IsPlayerControlled(actor);
                    break;
                }
                default:
                {
                    advanceForDeath = false;
                    break;
                }
            }

            if (advanceForDeath)
            {
                currentState.m_actorDeaths++;
                if (currentState.m_actorDeaths >= currentState.m_advanceScriptIfActorDiesCount)
                {
                    advance = true;
                }
            }
        }

        if (advance)
        {
            AdvanceScript();
        }
    }

    public void OnActorMoveEntered(ActorData actor)
    {
        SinglePlayerState currentState = GetCurrentState();
        ActorData localPlayer = GetLocalPlayer();
        if (localPlayer != actor || currentState == null)
        {
            return;
        }
        
        if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnMoveEntered))
        {
            AdvanceScript();
        }
    }

    public void OnActorAbilitySelected(ActorData actor)
    {
        SinglePlayerState currentState = GetCurrentState();
        ActorData localPlayer = GetLocalPlayer();
        if (localPlayer != actor || currentState == null)
        {
            return;
        }
        
        if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnAbilitySelected))
        {
            AdvanceScript();
        }
    }

    public void OnActorAbilityTargeted(ActorData actor)
    {
        SinglePlayerState currentState = GetCurrentState();
        ActorData localPlayer = GetLocalPlayer();
        if (localPlayer != actor || currentState == null)
        {
            return;
        }
        
        if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnAbilityTargeted))
        {
            AdvanceScript();
        }
    }

    public void OnActorLockInEntered(ActorData actor)
    {
        SinglePlayerState currentState = GetCurrentState();
        ActorData localPlayer = GetLocalPlayer();
        if (localPlayer != actor || currentState == null)
        {
            return;
        }
        
        if (currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnLockInEntered))
        {
            AdvanceScript();
        }
    }

    public void OnTutorialQueueEmpty()
    {
        if (NetworkServer.active)
        {
            SinglePlayerState currentState = GetCurrentState();
            if (currentState != null
                && currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.AdvanceOnTutorialQueueEmpty))
            {
                AdvanceScript();
            }
        }
        else
        {
            ActorData localPlayer = GetLocalPlayer();
            if (localPlayer != null)
            {
                PlayerData component = localPlayer.GetComponent<PlayerData>();
                if (component != null)
                {
                    component.CallCmdTutorialQueueEmpty();
                }
            }
        }
    }

    private void UpdateDestinationHighlights()
    {
        if (m_advanceDestinationsHighlight != null)
        {
            DestroyImmediate(m_advanceDestinationsHighlight);
            m_advanceDestinationsHighlight = null;
        }

        if (GetCurrentState() == null)
        {
            return;
        }
        
        HashSet<BoardSquare> advanceDestinations = GetCurrentState().GetAdvanceDestinations();
        if (advanceDestinations == null || advanceDestinations.Count <= 0)
        {
            return;
        }
        
        m_advanceDestinationsHighlight = HighlightUtils.Get().CreateBoundaryHighlight(advanceDestinations, Color.white);
        if (m_advanceDestinationsHighlight)
        {
            m_advanceDestinationsHighlight.AddComponent<HighlightParent>();
        }
    }

    public void OnActorMovementChanged(ActorData actor)
    {
        if (actor != GetLocalPlayer())
        {
            return;
        }
        
        m_errorTriggered = false;
        RecalcCanEndTurn();
        if (GetCanEndTurnFlag())
        {
            return;
        }

        if (IsMovementAllowed())
        {
            return;
        }

        if (SinglePlayerCoordinator.Get() == null)
        {
            return;
        }
        
        foreach (ActivatableObject activatableObject in SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath)
        {
            if (activatableObject == null)
            {
                Log.Error("Null activation-object in Activations On Forbidden Path.");
            }
            else if (activatableObject.m_sceneObject == null)
            {
                Log.Error("Activation-object has null scene-object in Activations On Forbidden Path.");
            }
            else
            {
                activatableObject.SetIsActive(true);
            }
        }

        if (actor.GetCurrentBoardSquare() != actor.MoveFromBoardSquare)
        {
            m_errorTriggered = true;
        }
    }

    public void OnEndTurnRequested(ActorData requestingActor)
    {
        RecalcCanEndTurn();
        if (!requestingActor.IsHumanControlled())
        {
            return;
        }
        
        if (SinglePlayerCoordinator.Get() != null)
        {
            foreach (ActivatableObject a in SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath)
            {
                if (a != null && a.m_sceneObject != null)
                {
                    a.SetIsActive(false);
                }
            }

            foreach (ActivatableObject a in SinglePlayerCoordinator.Get().m_activationsOnFailedToShootAndMove)
            {
                if (a != null && a.m_sceneObject != null)
                {
                    a.SetIsActive(false);
                }
            }

            foreach (ActivatableObject a in SinglePlayerCoordinator.Get().m_activationsOnFailedToUseAllAbilities)
            {
                if (a != null && a.m_sceneObject != null)
                {
                    a.SetIsActive(false);
                }
            }
        }

        if (GetCanEndTurnFlag())
        {
            return;
        }
        
        if (!IsMovementAllowed())
        {
            if (SinglePlayerCoordinator.Get() != null)
            {
                foreach (ActivatableObject a in SinglePlayerCoordinator.Get().m_activationsOnForbiddenPath)
                {
                    if (a == null)
                    {
                        Log.Error("Null activation-object in Activations On Forbidden Path.");
                    }
                    else if (a.m_sceneObject == null)
                    {
                        Log.Error("Activation-object has null scene-object in Activations On Forbidden Path.");
                    }
                    else
                    {
                        a.SetIsActive(true);
                    }
                }
            }

            return;
        }
        
        if (!IsShootAndMoveAllowed())
        {
            if (SinglePlayerCoordinator.Get() != null)
            {
                foreach (ActivatableObject a in SinglePlayerCoordinator.Get().m_activationsOnFailedToShootAndMove)
                {
                    if (a == null)
                    {
                        Log.Error("Null activation-object in Activations On Failed To Shoot And Move.");
                    }
                    else if (a.m_sceneObject == null)
                    {
                        Log.Error(
                            "Activation-object has null scene-object in Activations On Failed To Shoot And Move.");
                    }
                    else
                    {
                        a.SetIsActive(true);
                    }
                }
            }

            return;
        }

        if (IsMultipleAbilitiesAllowed() || SinglePlayerCoordinator.Get() == null)
        {
            return;
        }
        
        foreach (ActivatableObject a in SinglePlayerCoordinator.Get().m_activationsOnFailedToUseAllAbilities)
        {
            if (a == null)
            {
                Log.Error("Null activation-object in Activations On Failed To Use All Abilities.");
            }
            else if (a.m_sceneObject == null)
            {
                Log.Error("Activation-object has null scene-object in Activations On Failed To Use All Abilities.");
            }
            else
            {
                a.SetIsActive(true);
            }
        }
    }

    public static bool IsActionAllowed(ActorData caster, AbilityData.ActionType action)
    {
        if (s_instance == null || s_instance.GetCurrentState() == null)
        {
            return true;
        }

        if (!caster.IsHumanControlled())
        {
            Ability abilityOfActionType = caster.GetAbilityData().GetAbilityOfActionType(action);
            if (abilityOfActionType == null)
            {
                return true;
            }

            if (AbilityUtils.AbilityHasTag(abilityOfActionType, AbilityTags.AutoQueueIfValid))
            {
                return Get().EnableAutoQueuedAbilitiesForNPCs();
            }
            
            return true;
        }

        SinglePlayerState currentState = s_instance.GetCurrentState();
        if (currentState.m_allowedAbilities.Length == 0)
        {
            return true;
        }

        foreach (int allowed in currentState.m_allowedAbilities)
        {
            if (allowed == (int)action)
            {
                return true;
            }
        }

        return false;
    }

    public static bool IsDestinationAllowed(ActorData mover, BoardSquare square, bool settingWaypoints = true)
    {
        if (s_instance == null || s_instance.GetCurrentState() == null)
        {
            return true;
        }

        if (mover.SpawnerId != -1)
        {
            return true;
        }

        SinglePlayerState currentState = s_instance.GetCurrentState();
        bool inRegion = currentState.m_allowedDestinations.m_quads.Length == 0
                        || currentState.m_allowedDestinations.GetSquaresInRegion().Contains(square);
        bool waypointOk = !currentState.m_onlyAllowWaypointMovement
                          || settingWaypoints;
        return inRegion && waypointOk;
    }

    public static bool IsSquareForbidden(ActorData mover, BoardSquare square)
    {
        if (s_instance == null || s_instance.GetCurrentState() == null)
        {
            return false;
        }

        if (!mover.IsHumanControlled())
        {
            return false;
        }

        if (SinglePlayerCoordinator.Get() == null)
        {
            return false;
        }

        if (SinglePlayerCoordinator.Get().m_forbiddenSquares.m_quads.Length == 0)
        {
            return false;
        }
        
        return SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion().Contains(square);
    }

    public void RecalcCanEndTurn()
    {
        bool canEnd = IsMovementAllowed()
                      && IsShootAndMoveAllowed()
                      && IsMultipleAbilitiesAllowed()
                      && IsMoveToAdvanceScriptDestinationAllowed()
                      && IsRequireDashOk()
                      && !DisableAdvanceTurn();
        if (NetworkServer.active)
        {
            Networkm_canEndTurn = canEnd;
        }
        else
        {
            m_clientCanEndTurn = canEnd;
        }
    }

    private bool IsRequireDashOk()
    {
        if (SinglePlayerCoordinator.Get() == null || GetCurrentState() == null)
        {
            return true;
        }

        if (!GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
        {
            return true;
        }
        
        AbilityData abilityData = GetLocalPlayer().GetAbilityData();
        return abilityData.HasQueuedAbilities() && !abilityData.GetQueuedAbilitiesAllowMovement();
    }

    private bool IsMovementAllowed()
    {
        if (SinglePlayerCoordinator.Get() == null || GetCurrentState() == null)
        {
            return true;
        }
        
        SinglePlayerState currentState = GetCurrentState();
        if (currentState != null && currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.RequireDash))
        {
            return true;
        }
        
        if (currentState != null && currentState.GetHasTag(SinglePlayerState.SinglePlayerTag.RequireChasing))
        {
            bool isChasing = false;
            ActorData localPlayer = GetLocalPlayer();
            
#if SERVER
            // added in rogues
            if (NetworkServer.active && ServerActionBuffer.Get() != null)
            {
                isChasing = ServerActionBuffer.Get().IsChasing(localPlayer);
            }
#endif
            
            if (!NetworkServer.active
                && localPlayer.GetComponent<LineData>() != null)
            {
                isChasing = localPlayer.GetComponent<LineData>().GetIsChasing();
            }

            return isChasing;
        }

        if (SinglePlayerCoordinator.Get().m_forbiddenSquares.m_quads.Length == 0)
        {
            return true;
        }

        ActorData localPlayer2 = GetLocalPlayer();
        bool chasing = false;
        List<GridPos> path = null;
        
#if SERVER
        if (NetworkServer.active && ServerActionBuffer.Get() != null)
        {
            path = ServerActionBuffer.Get().GetGridPosPath(localPlayer2, out chasing);
        }
#endif
        
        if (!NetworkServer.active && localPlayer2.GetComponent<LineData>())
        {
            path = localPlayer2.GetComponent<LineData>().GetGridPosPath();
        }

        if (path != null)
        {
#if SERVER
            // added in rogues
            if (chasing)
            {
                return true;
            }
#endif
            
            foreach (GridPos pos in path)
            {
                if (SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion()
                    .Contains(Board.Get().GetSquare(pos)))
                {
                    return false;
                }
            }

            return true;
        }

        return !SinglePlayerCoordinator.Get().m_forbiddenSquares.GetSquaresInRegion()
            .Contains(localPlayer2.GetCurrentBoardSquare());
    }

    private bool IsMoveToAdvanceScriptDestinationAllowed()
    {
        if (GetCurrentState() == null)
        {
            return true;
        }

        if (!GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireMoveToAdvanceScriptDestination))
        {
            return true;
        }

        BoardSquare item = null;
        ActorData localPlayer = GetLocalPlayer();
        
#if SERVER
        // added in rogues
        if (NetworkServer.active)
        {
            ServerActionBuffer.Get().GatherMovementInfo(localPlayer, out item, out _, out _);
        }
#endif

        if (!NetworkServer.active)
        {
            List<GridPos> gridPosPath = localPlayer.GetComponent<LineData>().GetGridPosPath();
            if (gridPosPath != null && gridPosPath.Count > 0)
            {
                item = Board.Get().GetSquare(gridPosPath[gridPosPath.Count - 1]);
            }
        }

        if (GetCurrentState().GetAdvanceDestinations() != null
            && GetCurrentState().GetAdvanceDestinations().Contains(item))
        {
            return true;
        }

        if (GetCurrentState().GetAdvanceDestinations() == null)
        {
            Log.Error(
                "We have a single player state {0} with the tag RequireMoveToAdvanceScriptDestination and no advance destinations!  Error!  Tell Colin!",
                GetCurrentScriptIndex());
        }

        return false;
    }

    private bool IsShootAndMoveAllowed()
    {
        if (GetCurrentState() == null)
        {
            return true;
        }

        if (!GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireShootAndMove))
        {
            return true;
        }

        ActorData localPlayer = GetLocalPlayer();
        List<GridPos> path = null;
        bool isChasing = false;
        
#if SERVER
        // added in rogues
        if (NetworkServer.active)
        {
            path = ServerActionBuffer.Get().GetGridPosPath(localPlayer, out isChasing);
        }
#endif

        if (!NetworkServer.active)
        {
            path = localPlayer.GetComponent<LineData>().GetGridPosPath();
            isChasing = localPlayer.GetComponent<LineData>().GetIsChasing();
        }

        bool isMoving = path != null && path.Count > 1 && !isChasing;
        return isMoving && localPlayer.GetAbilityData().HasQueuedAbilities();
    }

    private bool IsMultipleAbilitiesAllowed()
    {
        if (GetCurrentState() == null)
        {
            return true;
        }

        if (!GetCurrentState().GetHasTag(SinglePlayerState.SinglePlayerTag.RequireMaxPossibleAbilities))
        {
            return true;
        }

        if (GetCurrentState().m_allowedAbilities.Length == 0)
        {
            Log.Warning(
                "State " + m_currentScriptIndex
                         + " has RequireMaxPossibleAbilities but no specified allowed abilities.  Ignoring RequireMaxPossibleAbilities...");
            return true;
        }

        return GetCurrentState().m_allowedAbilities.Length == GetLocalPlayer().GetAbilityData().GetNumQueuedAbilities();
    }

    public static bool IsAbilitysCurrentAimingAllowed(ActorData aimer)
    {
        if (s_instance == null || s_instance.GetCurrentState() == null || !aimer.IsHumanControlled())
        {
            return true;
        }

        AbilityData component = aimer.GetComponent<AbilityData>();
        Ability selectedAbility = component.GetSelectedAbility();
        if (selectedAbility == null)
        {
            return true;
        }

        SinglePlayerState currentState = s_instance.GetCurrentState();
        bool allTargetersSet;
        if (currentState.m_minAbilityTargetsForAiming == 0)
        {
            allTargetersSet = true;
        }
        else if (selectedAbility != null && selectedAbility.Targeter != null)
        {
            allTargetersSet = selectedAbility.Targeter.GetNumActorsInRange()
                              >= currentState.m_minAbilityTargetsForAiming;
        }
        else
        {
            Log.Warning(
                "Single Player state "
                + currentState.m_stateIndex
                + " cares about MinAbilityTargetsForAiming for a targeter-less ability.  Suggest RequireMaxPossibleAbilities instead.");
            allTargetersSet = currentState.m_minAbilityTargetsForAiming <= 1;
        }

        bool allTargetsAllowed;
        if (currentState.m_allowedTargets.m_quads.Length == 0)
        {
            allTargetsAllowed = true;
        }
        else
        {
            ActorTurnSM actorTurnSM = aimer.GetComponent<ActorTurnSM>();
            int targetSelectionIndex = actorTurnSM.GetTargetSelectionIndex();
            List<BoardSquare> squaresInRegion = currentState.m_allowedTargets.GetSquaresInRegion();
            Ability.TargetingParadigm targetingParadigm = selectedAbility.GetTargetingParadigm(targetSelectionIndex);
            BoardSquare boardSquare;
            if (targetingParadigm == Ability.TargetingParadigm.BoardSquare)
            {
                AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromInterface();
                boardSquare = Board.Get().GetSquare(abilityTarget.GridPos);
            }
            else if (targetingParadigm == Ability.TargetingParadigm.Position)
            {
                AbilityTarget abilityTarget = AbilityTarget.CreateAbilityTargetFromInterface();
                boardSquare = Board.Get().GetSquareFromVec3(abilityTarget.FreePos);
            }
            else
            {
                boardSquare = null;
            }

            allTargetsAllowed = boardSquare == null || squaresInRegion.Contains(boardSquare);
            if (allTargetsAllowed && currentState.m_mustTargetNearCenter)
            {
                AbilityTarget abilityTarget3 = AbilityTarget.CreateAbilityTargetFromInterface();
                Vector3 center = currentState.m_allowedTargets.GetCenter();
                float num2 = 0.45f * Board.Get().squareSize;
                if (Mathf.Abs(center.x - abilityTarget3.FreePos.x) >= num2
                    || Mathf.Abs(center.z - abilityTarget3.FreePos.z) >= num2)
                {
                    allTargetsAllowed = false;
                }
            }
        }

        return allTargetersSet && allTargetsAllowed;
    }

    public void UpdateRightAndLeftClickElements(
        GameObject rightClick,
        TextMeshProUGUI rightClickText,
        GameObject leftClick,
        TextMeshProUGUI leftClickText,
        GameObject shiftRightClick,
        TextMeshProUGUI shiftRightClickText)
    {
        SinglePlayerState currentState = GetCurrentState();
        if (rightClick == null
            || rightClickText == null
            || leftClick == null
            || leftClickText == null
            || shiftRightClick == null
            || shiftRightClickText == null
            || currentState == null
            || currentState.m_leftClickHighlight == null)
        {
            return;
        }

        List<BoardSquare> leftSquares = currentState.m_leftClickHighlight.GetSquaresInRegion();
        if (leftSquares.Count > 0)
        {
            if (!leftClick.activeSelf)
            {
                // reactor
                UIManager.SetGameObjectActive(leftClick, true);
                // rogues
                // leftClick.SetActive(true);
            }

            Canvas c = leftClick.GetComponentInParent<Canvas>();
            RectTransform r = c != null ? c.transform as RectTransform : null;
            Vector2 vp = Camera.main.WorldToViewportPoint(
                new Vector3(leftSquares[0].worldX, 1.5f + currentState.m_leftClickHeight, leftSquares[0].worldY));
            (leftClick.transform as RectTransform).anchoredPosition = new Vector2(
                vp.x * r.sizeDelta.x - r.sizeDelta.x * 0.5f,
                vp.y * r.sizeDelta.y - r.sizeDelta.y * 0.5f);
            string txt = currentState.GetLeftClickText();
            leftClickText.text = txt != string.Empty ? txt : StringUtil.TR("LeftClick", "Tutorial"); // checks old value in rogues
        }
        else if (leftClick.activeSelf)
        {
            // reactor
            UIManager.SetGameObjectActive(leftClick, false);
            // rogues
            // leftClick.SetActive(false);
        }

        List<BoardSquare> rightSquares = currentState.m_rightClickHighlight.GetSquaresInRegion();
        if (rightSquares.Count > 0)
        {
            if (!rightClick.activeSelf)
            {
                // reactor
                UIManager.SetGameObjectActive(rightClick, true);
                // rogues
                // rightClick.SetActive(true);
            }

            Canvas c2 = rightClick.GetComponentInParent<Canvas>();
            RectTransform r2 = c2 != null ? c2.transform as RectTransform : null;
            Vector2 vp2 = Camera.main.WorldToViewportPoint(
                new Vector3(rightSquares[0].worldX, 1.5f + currentState.m_rightClickHeight, rightSquares[0].worldY));
            (rightClick.transform as RectTransform).anchoredPosition = new Vector2(
                vp2.x * r2.sizeDelta.x - r2.sizeDelta.x * 0.5f,
                vp2.y * r2.sizeDelta.y - r2.sizeDelta.y * 0.5f);
            string txt2 = currentState.GetRightClickText();
            rightClickText.text = txt2 != string.Empty ? txt2 : StringUtil.TR("RightClick", "Tutorial"); // checks old value in rogues
        }
        
        bool shiftEnabled = Options_UI.Get() != null && Options_UI.Get().GetShiftClickForMovementWaypoints();
        List<BoardSquare> shiftSquares = currentState.m_shiftRightClickHighlight.GetSquaresInRegion();
        if (shiftSquares.Count > 0)
        {
            if (shiftEnabled)
            {
                if (!shiftRightClick.activeSelf)
                {
                    // reactor
                    UIManager.SetGameObjectActive(shiftRightClick, true);
                    // rogues
                    // shiftRightClick.SetActive(true);
                }

                Canvas c3 = shiftRightClick.GetComponentInParent<Canvas>();
                RectTransform r3 = c3 != null ? c3.transform as RectTransform : null;
                Vector2 vp3 = Camera.main.WorldToViewportPoint(
                    new Vector3(
                        shiftSquares[0].worldX,
                        1.5f + currentState.m_shiftRightClickHeight,
                        shiftSquares[0].worldY));
                (shiftRightClick.transform as RectTransform).anchoredPosition = new Vector2(
                    vp3.x * r3.sizeDelta.x - r3.sizeDelta.x * 0.5f,
                    vp3.y * r3.sizeDelta.y - r3.sizeDelta.y * 0.5f);
                string txt3 = currentState.GetShiftRightClickText();
                shiftRightClickText.text = txt3 != string.Empty ? txt3 : StringUtil.TR("RightClick", "Tutorial"); // checks old value in rogues
            }
            else
            {
                if (!rightClick.activeSelf)
                {
                    // reactor
                    UIManager.SetGameObjectActive(rightClick, true);
                    // rogues
                    // rightClick.SetActive(true);
                }

                Canvas c4 = rightClick.GetComponentInParent<Canvas>();
                RectTransform r4 = c4 != null ? c4.transform as RectTransform : null;
                Vector2 vp4 = Camera.main.WorldToViewportPoint(
                    new Vector3(
                        shiftSquares[0].worldX, 
                        1.5f + currentState.m_rightClickHeight, 
                        shiftSquares[0].worldY));
                (rightClick.transform as RectTransform).anchoredPosition = new Vector2(
                    vp4.x * r4.sizeDelta.x - r4.sizeDelta.x * 0.5f,
                    vp4.y * r4.sizeDelta.y - r4.sizeDelta.y * 0.5f);
                string txt4 = currentState.GetRightClickText();
                rightClickText.text = txt4 != string.Empty ? txt4 : StringUtil.TR("RightClick", "Tutorial"); // checks old value in rogues
            }
        }

        if (rightSquares.Count == 0 && (shiftSquares.Count == 0 || shiftEnabled))
        {
            if (rightClick.activeSelf)
            {
                // reactor
                UIManager.SetGameObjectActive(rightClick, false);
                // rogues
                // rightClick.SetActive(false);
            }
        }

        if (shiftSquares.Count == 0 || !shiftEnabled)
        {
            if (shiftRightClick.activeSelf)
            {
                // reactor
                UIManager.SetGameObjectActive(shiftRightClick, false);
                // rogues
                // shiftRightClick.SetActive(false);
            }
        }
    }

    public void UpdateTutorialError(GameObject panel, TextMeshProUGUI text)
    {
        if (panel == null || text == null)
        {
            return;
        }

        SinglePlayerState currentState = GetCurrentState();
        string errorText = currentState != null ? currentState.GetErrorStringOnForbiddenPath() : string.Empty;
        if (m_errorTriggered && errorText != string.Empty)
        {
            // reactor
            UIManager.SetGameObjectActive(panel, true);
            text.text = errorText;
            // rogues
            // panel.SetActive(true);
            // if (text.text != errorText)
            // {
            //     text.text = errorText;
            // }
        }
        else
        {
            // reactor
            UIManager.SetGameObjectActive(panel, false);
            // rogues
            // panel.SetActive(false);
        }
    }

    public void UpdateTutorialTextElements(
        GameObject panel,
        TextMeshProUGUI text,
        GameObject panel2,
        TextMeshProUGUI text2,
        GameObject panel3,
        TextMeshProUGUI text3,
        GameObject panelCameraMovement,
        TextMeshProUGUI textCameraMovement,
        GameObject panelCameraRotation,
        TextMeshProUGUI textCameraRotation)
    {
        SinglePlayerState currentState = GetCurrentState();
        if (currentState == null
            || panel == null
            || text == null
            || panel2 == null
            || text2 == null
            || panel3 == null
            || text3 == null
            || panelCameraMovement == null
            || textCameraMovement == null
            || panelCameraRotation == null
            || textCameraRotation == null
            || Camera.main == null)
        {
            return;
        }

        bool flag = m_lastTutorialTextState != currentState.m_stateIndex;

        if (currentState.m_tutorialBoxText.m_location == null)
        {
            // reactor
            UIManager.SetGameObjectActive(panel, false);
            // rogues
            // panel.SetActive(false);
        }
        else
        {
            Canvas c = panel.GetComponentInParent<Canvas>();
            RectTransform r = c != null ? c.transform as RectTransform : null;
            if (flag)
            {
                m_lastTutorialTextState = currentState.m_stateIndex;
                text.text = currentState.GetTutorialBoxText();
                // reactor
                UIManager.SetGameObjectActive(panel, true);
                // rogues
                // panel.SetActive(true);
            }

            Vector2 vp = Camera.main.WorldToViewportPoint(currentState.m_tutorialBoxText.m_location.transform.position);
            (panel.transform as RectTransform).anchoredPosition = new Vector2(
                vp.x * r.sizeDelta.x - r.sizeDelta.x * 0.5f,
                vp.y * r.sizeDelta.y - r.sizeDelta.y * 0.5f);
        }

        if (currentState.m_tutorialBoxText2.m_location == null)
        {
            // reactor
            UIManager.SetGameObjectActive(panel2, false);
            // rogues
            // panel2.SetActive(false);
        }
        else
        {
            Canvas c2 = panel2.GetComponentInParent<Canvas>();
            RectTransform r2 = c2 != null ? c2.transform as RectTransform : null;
            if (flag)
            {
                m_lastTutorialTextState = currentState.m_stateIndex;
                text2.text = currentState.GetTutorialBoxText2();
                // reactor
                UIManager.SetGameObjectActive(panel2, true);
                // rogues
                // panel2.SetActive(true);
            }

            Vector2 vp2 =
                Camera.main.WorldToViewportPoint(currentState.m_tutorialBoxText2.m_location.transform.position);
            (panel2.transform as RectTransform).anchoredPosition = new Vector2(
                vp2.x * r2.sizeDelta.x - r2.sizeDelta.x * 0.5f,
                vp2.y * r2.sizeDelta.y - r2.sizeDelta.y * 0.5f);
        }

        if (currentState.m_tutorialBoxText3.m_location == null)
        {
            // reactor
            UIManager.SetGameObjectActive(panel3, false);
            // rogues
            // panel3.SetActive(false);
        }
        else
        {
            Canvas c3 = panel3.GetComponentInParent<Canvas>();
            RectTransform r3 = c3 != null ? c3.transform as RectTransform : null;
            if (flag)
            {
                m_lastTutorialTextState = currentState.m_stateIndex;
                text3.text = currentState.GetTutorialBoxText3();
                // reactor
                UIManager.SetGameObjectActive(panel3, true);
                // rogues
                // panel3.SetActive(true);
            }

            Vector2 vp3 =
                Camera.main.WorldToViewportPoint(currentState.m_tutorialBoxText3.m_location.transform.position);
            (panel3.transform as RectTransform).anchoredPosition = new Vector2(
                vp3.x * r3.sizeDelta.x - r3.sizeDelta.x * 0.5f,
                vp3.y * r3.sizeDelta.y - r3.sizeDelta.y * 0.5f);
        }

        if (currentState.m_tutorialCameraMovementText.m_location == null)
        {
            // reactor
            UIManager.SetGameObjectActive(panelCameraMovement, false);
            // rogues
            // panelCameraMovement.SetActive(false);
        }
        else
        {
            Canvas c4 = panel3.GetComponentInParent<Canvas>();
            RectTransform r4 = c4 != null ? c4.transform as RectTransform : null;
            if (flag)
            {
                m_lastTutorialTextState = currentState.m_stateIndex;
                textCameraMovement.text = currentState.GetTutorialCameraMovementText();
                //reactor
                UIManager.SetGameObjectActive(panelCameraMovement, true);
                // rogues
                // panelCameraMovement.SetActive(true);
            }

            Vector2 vp4 = Camera.main.WorldToViewportPoint(
                currentState.m_tutorialCameraMovementText.m_location.transform.position);
            (panelCameraMovement.transform as RectTransform).anchoredPosition = new Vector2(
                vp4.x * r4.sizeDelta.x - r4.sizeDelta.x * 0.5f,
                vp4.y * r4.sizeDelta.y - r4.sizeDelta.y * 0.5f);
        }

        if (currentState.m_tutorialCameraRotationText.m_location == null)
        {
            // reactor
            UIManager.SetGameObjectActive(panelCameraRotation, false);
            // rogues
            // panelCameraRotation.SetActive(false);
            return;
        }

        Canvas c5 = panel3.GetComponentInParent<Canvas>();
        RectTransform r5 = c5 != null ? c5.transform as RectTransform : null;
        if (flag)
        {
            m_lastTutorialTextState = currentState.m_stateIndex;
            textCameraRotation.text = currentState.GetTutorialCameraRotationText();
            // reactor
            UIManager.SetGameObjectActive(panelCameraRotation, true);
            // rogues
            // panelCameraRotation.SetActive(true);
        }

        Vector2 vp5 =
            Camera.main.WorldToViewportPoint(currentState.m_tutorialCameraRotationText.m_location.transform.position);
        (panelCameraRotation.transform as RectTransform).anchoredPosition = new Vector2(
            vp5.x * r5.sizeDelta.x - r5.sizeDelta.x * 0.5f,
            vp5.y * r5.sizeDelta.y - r5.sizeDelta.y * 0.5f);
    }

    public bool HasPendingCameraUpdate()
    {
        SinglePlayerState currentState = GetCurrentState();
        bool flag = false;
        if (currentState != null)
        {
            if (m_lastTutorialCameraState != currentState.m_stateIndex)
            {
                flag = true;
            }
        }

        if (flag)
        {
            m_lastTutorialCameraState = currentState.m_stateIndex;
            if (currentState.m_cameraRotationTarget == null)
            {
                flag = false;
            }
        }

        return flag;
    }

    public static void ResetUIActivations()
    {
        ActivatableUI activatableUI = new ActivatableUI();
        activatableUI.m_activation = ActivatableUI.ActivationAction.SetActive;
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.Taunt;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelBackground;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelCenterPiece;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus1;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus2;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus3;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus4;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TopDisplayPanelPlayerStatus5;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.ObjectivePanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.NotificationPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.BuffList;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.LockInButtonTutorialTipImage;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.LockInButtonTutorialTipText;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.FadeOutPanel;
        activatableUI.Activate();
        activatableUI.m_activation = ActivatableUI.ActivationAction.ClearActive;
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow1;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow2;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow3;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonGlow4;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip1;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip2;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip3;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.AbilityButtonTutorialTip4;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.LockInButtonTutorialTip;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.CameraControlsTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.EnergyGlow;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.EnergyArrows;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.CombatPhaseTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.DashPhaseTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.PrepPhaseTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.FullScreenPrepPhaseTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.FullScreenDashPhaseTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.FullScreenCombatPhaseTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.CardButtonTutorialTip;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.CardButtonTutorialTip1;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.CardButtonTutorialTip2;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.StatusEffectTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.TeammateTargetingTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.EnergyAndUltimatesTutorialPanel;
        activatableUI.Activate();
        activatableUI.m_uiElement = ActivatableUI.UIElement.FadeOutPanel;
        activatableUI.Activate();
        
        // reactor
        UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar, false);
        UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText, false);
        // rogues
        // HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialBar.gameObject.SetActive(false);
        // HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialText.gameObject.SetActive(false);
    }

    [ClientRpc]
    public void RpcPlayScriptedChat(SinglePlayerScriptedChat chatText)
    {
        UITutorialPanel.Get().QueueDialogue(
            chatText.m_text,
            chatText.m_audioEvent,
            chatText.m_displaySeconds,
            chatText.m_sender);
    }

    private void UNetVersion()
    {
    }

    // changed in rogues
    protected static void InvokeRpcRpcPlayScriptedChat(NetworkBehaviour obj, NetworkReader reader)
    {
        if (!NetworkClient.active)
        {
            Debug.LogError("RPC RpcPlayScriptedChat called on server.");
            return;
        }

        ((SinglePlayerManager)obj).RpcPlayScriptedChat(GeneratedNetworkCode._ReadSinglePlayerScriptedChat_None(reader));
    }

    // changed in rogues
    public void CallRpcPlayScriptedChat(SinglePlayerScriptedChat chatText)
    {
        if (!NetworkServer.active)
        {
            Debug.LogError("RPC Function RpcPlayScriptedChat called on client.");
            return;
        }

        NetworkWriter networkWriter = new NetworkWriter();
        networkWriter.Write((short)0);
        networkWriter.Write((short)2);
        networkWriter.WritePackedUInt32((uint)kRpcRpcPlayScriptedChat);
        networkWriter.Write(GetComponent<NetworkIdentity>().netId);
        GeneratedNetworkCode._WriteSinglePlayerScriptedChat_None(networkWriter, chatText);
        SendRPCInternal(networkWriter, 0, "RpcPlayScriptedChat");
    }

    // changed in rogues
    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        if (forceAll)
        {
            writer.WritePackedUInt32((uint)m_currentScriptIndex);
            writer.Write(m_canEndTurn);
            return true;
        }

        bool flag = false;
        if ((syncVarDirtyBits & 1) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_currentScriptIndex);
        }

        if ((syncVarDirtyBits & 2) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.Write(m_canEndTurn);
        }

        if (!flag)
        {
            writer.WritePackedUInt32(syncVarDirtyBits);
        }

        return flag;
    }

    // changed in rogues
    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if (initialState)
        {
            m_currentScriptIndex = (int)reader.ReadPackedUInt32();
            m_canEndTurn = reader.ReadBoolean();
            return;
        }

        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            HookSetCurrentScriptIndex((int)reader.ReadPackedUInt32());
        }

        if ((num & 2) != 0)
        {
            HookSetCanEndTurn(reader.ReadBoolean());
        }
    }
}
