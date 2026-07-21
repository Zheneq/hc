using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class CaptureTheFlag : NetworkBehaviour, IGameEventListener
{
    public enum CTF_VictoryCondition
    {
        TeamMustBeHoldingFlag,
        TeamMustNotBeHoldingFlag,
        OtherTeamMustBeHoldingFlag,
        OtherTeamMustNotBeHoldingFlag,
        TeamMustHaveCapturedFlag,
        TeamMustNotHaveCapturedFlag,
        OtherTeamMustHaveCapturedFlag,
        OtherTeamMustNotHaveCapturedFlag
    }

    public enum TurninRegionState
    {
        Active,
        Locked,
        Disabled
    }

    public enum TurninType
    {
        FlagHolderMovingIntoCaptureRegion,
        FlagHolderEndingTurnInCaptureRegion,
        FlagHolderSpendingWholeTurnInCaptureRegion,
        CaptureRegionActivatingUnderFlagHolder
    }

    public enum RelationshipToClient
    {
        Neutral,
        Friendly,
        Hostile
    }

    [Serializable]
    public class FlagSpawnData
    {
        public int m_maxActiveSimultaneously;
        public int m_totalMaxSpawns = -1;
        public int m_minTurnsTillFirstSpawn;
        public int m_minTurnsAfterCaptureTillRespawn;
        
        public int NumFlagsSpawned { get; set; }
        public int LastCaptureTurn { get; set; }
    }

    [Serializable]
    public class FlagHolderObjectivePointData
    {
        public float m_pointsPerDamageDealtByFlagHolder;
        public float m_pointsPerDamageTakenByFlagHolder;
        public float m_pointsPerHealingDealtByFlagHolder;
        public float m_pointsPerHealingTakenByFlagHolder;
        public float m_pointsPerAbsorbDealtByFlagHolder;
        public float m_pointsPerAbsorbTakenByFlagHolder;
        public bool m_includeContributionFromNonCharacterAbilities;
        public int m_pointsPerDeathblowByFlagHolder;
        public int m_pointsPerTakedownByFlagHolder;
        public int m_pointsPerDeathOfFlagHolder;
        public int m_pointsPerTurn;
    }

    public static byte s_nextFlagGuid;

    private static CaptureTheFlag s_instance;

    [Header("Locations")]
    public BoardRegion m_flagSpawnsNeutral;
    public BoardRegion m_flagSpawnsTeamA;
    public BoardRegion m_flagSpawnsTeamB;
    public BoardRegion m_flagTurninTeamA;
    public BoardRegion m_flagTurninTeamB;
    public BoardRegion m_flagTurninNeutral;
    public List<BoardRegion> m_potentialFlagTurnins;

    public bool m_potentialTurninsAreTeamSpecific = true;
    public float m_potentialTurninRegion_minDistFromFlag = -1f;
    public float m_potentialTurninRegion_maxDistFromFlag = -1f;
    public float m_potentialTurninRegion_desiredDistFromFlag = -1f;
    public float m_potentialTurninRegion_minDistFromTurnin = -1f;
    public float m_potentialTurninRegion_maxDistFromTurnin = -1f;

    [Header("Visuals")]
    public bool m_autoGenerateTurninVisuals = true;
    public bool m_autoGenerateSpawnLocVisuals;
    public float m_boundaryOscillationSpeed = 3.14159f;
    public float m_boundaryOscillationHeight = 0.05f;
    public Color m_primaryColor_friendly = Color.blue;
    public Color m_primaryColor_hostile = Color.red;
    public Color m_primaryColor_neutral = Color.gray;
    public Color m_secondaryColor_locked = Color.yellow;
    public Color m_neutralFlagColor_idle = Color.gray;
    public Color m_neutralFlagColor_held = Color.white;
    public Color m_friendlyFlagColor_idle = Color.blue;
    public Color m_friendlyFlagColor_held = Color.blue;
    public Color m_hostileFlagColor_idle = Color.red;
    public Color m_hostileFlagColor_held = Color.red;
    public Color m_textColor_positive = Color.blue;
    public Color m_textColor_negative = Color.red;
    public Color m_textColor_neutral = Color.white;
    public float m_timeTillCameraFocusesOntoExtractionPoint = 0.5f;
    public float m_timeTillCameraFocusesOntoExtraction = 0.1f;

    [Header("Capturing Logic")]
    public GameplayRewardForTeam m_rewardToCapturingTeam;
    public GameplayRewardForTeam m_rewardToOtherTeam;
    public int m_disableTurninTeamAUntilTheirScore = -1;
    public int m_disableTurninTeamBUntilTheirScore = -1;
    public int m_disableTurninNeutralUntilAnyScore = -1;
    public int m_numTurnsToLockTurninOnEnable;
    public TurninRegionState m_turninRegionInitialState;
    public List<TurninType> m_turnInRequirements;

    [Header("Spawning Logic")]
    public FlagSpawnData m_neutralFlagSpawningLogic;
    public FlagSpawnData m_teamAFlagSpawningLogic;
    public FlagSpawnData m_teamBFlagSpawningLogic;
    public GameObject m_flagPrefab;
    public StandardEffectInfo m_flagHolderEffect;
    public bool m_flagRevealsHolder = true;
    public StandardEffectInfo m_onDroppedFlagEffect;
    public StandardEffectInfo m_onTurnedInFlagEffect;
    public StandardEffectInfo m_onReturnedFlagEffect;

    [Header("Special movement rules")]
    public bool m_evasionDropsFlags = true;
    public bool m_beingKnockedBackDropsFlags;
    public bool m_evadersCanPickUpFlags;
    public bool m_knockbackedMoversCanPickUpFlags;
    public bool m_disableAllyReturningOwnFlags;
    public bool m_allowFlagJuggling;
    public int m_damageInOneTurnToDropFlag_gross = -1;
    public int m_damageSincePickedUpToDropFlag_gross = -1;
    public int m_damageThesholdIncreaseOnDrop;

    [Header("Sequences")]
    public GameObject m_flagPickedUpSequence;
    public GameObject m_flagDroppedSequence;
    public GameObject m_flagTurnedInSequence;
    public GameObject m_flagReturnedToSpawnSequence;
    public GameObject m_flagBeingHeldSequence;
    public GameObject m_friendlyTurninRegionActivatedSequence;
    public GameObject m_enemyTurninRegionActivatedSequence;
    public GameObject m_neutralTurninRegionActivatedSequence;
    
    public Sprite m_flagIcon;
    public Sprite m_turnInRegionIcon;

    [Header("Objective Points")]
    public FlagHolderObjectivePointData m_objectivePointsData_flagHoldersTeam;
    public FlagHolderObjectivePointData m_objectivePointsData_otherTeam;

    [Header("Strings")]
    public string m_alliedExtractionPointNowActive;
    public string m_alliedExtractionPointNowInactive;
    public string m_alliedExtractionPointNowUnlocking;
    public string m_enemyExtractionPointNowActive;
    public string m_enemyExtractionPointNowInactive;
    public string m_enemyExtractionPointNowUnlocking;
    public string m_neutralExtractionPointNowActive;
    public string m_neutralExtractionPointNowInactive;
    public string m_neutralExtractionPointNowUnlocking;

    private List<CTF_Flag> m_flags;
    private int m_teamACaptures;
    private int m_teamBCaptures;
    
    private GameObject m_autoBoundary_spawn_teamA;
    private GameObject m_autoBoundary_spawn_teamB;
    private GameObject m_autoBoundary_spawn_neutral;
    private GameObject m_autoBoundary_turnin_teamA;
    private GameObject m_autoBoundary_turnin_teamB;
    private GameObject m_autoBoundary_turnin_neutral;
    private float m_autoBoundaryHeight;
    
    private float m_timeToFocusCameraOnTurninTeamA = -1f;
    private float m_timeToFocusCameraOnTurninTeamB = -1f;
    private float m_timeToFocusCameraOnTurninNeutral = -1f;
    private float m_timeToFocusCameraOnExtraction = -1f;

    private BoardSquare m_lastExtractionSquare;

    [SyncVar(hook = "HookSetTurninRegionState_TeamA")]
    private int m_turninRegionState_TeamA;
    [SyncVar(hook = "HookSetTurninRegionState_TeamB")]
    private int m_turninRegionState_TeamB;
    [SyncVar(hook = "HookSetTurninRegionState_Neutral")]
    private int m_turninRegionState_Neutral;
    [SyncVar(hook = "HookSetTurninRegionIndex_TeamA")]
    private int m_turninRegionIndex_TeamA = -1;
    [SyncVar(hook = "HookSetTurninRegionIndex_TeamB")]
    private int m_turninRegionIndex_TeamB = -1;
    [SyncVar(hook = "HookSetTurninRegionIndex_Neutral")]
    private int m_turninRegionIndex_Neutral = -1;
    [SyncVar(hook = "HookSetNumFlagDrops")]
    private int m_numFlagDrops;

    private int m_clientUnresolvedNumFlagDrops;

    [SyncVar]
    private uint m_sequenceSourceId;

    private SequenceSource _sequenceSource;

    private float m_lastFlagCarrierDamageCur;
    private float m_lastFlagCarrierDamageMax = 1f;

    internal SequenceSource SequenceSource
    {
        get
        {
            if (_sequenceSource == null)
            {
                _sequenceSource = new SequenceSource(null, null, m_sequenceSourceId, false);
            }

            return _sequenceSource;
        }
    }

    private TurninRegionState TurninRegionState_TeamA
    {
        get => (TurninRegionState)m_turninRegionState_TeamA;
        set
        {
            if (m_turninRegionState_TeamA != (int)value)
            {
                Networkm_turninRegionState_TeamA = (int)value;
            }
        }
    }

    private TurninRegionState TurninRegionState_TeamB
    {
        get => (TurninRegionState)m_turninRegionState_TeamB;
        set
        {
            if (m_turninRegionState_TeamB != (int)value)
            {
                Networkm_turninRegionState_TeamB = (int)value;
            }
        }
    }

    private TurninRegionState TurninRegionState_Neutral
    {
        get => (TurninRegionState)m_turninRegionState_Neutral;
        set
        {
            if (m_turninRegionState_Neutral != (int)value)
            {
                Networkm_turninRegionState_Neutral = (int)value;
            }
        }
    }

    private BoardRegion FlagTurninRegion_TeamA
    {
        get
        {
            if (TurninRegionState_TeamA != TurninRegionState.Active)
            {
                return null;
            }

            if (m_potentialFlagTurnins == null
                || m_potentialFlagTurnins.Count == 0
                || !m_potentialTurninsAreTeamSpecific)
            {
                return m_flagTurninTeamA;
            }

            if (m_turninRegionIndex_TeamA != -1)
            {
                return m_potentialFlagTurnins[m_turninRegionIndex_TeamA];
            }
            
            return null;
        }
    }

    private BoardRegion FlagTurninRegion_TeamB
    {
        get
        {
            if (TurninRegionState_TeamB != TurninRegionState.Active)
            {
                
                return null;
            }

            if (m_potentialFlagTurnins == null
                || m_potentialFlagTurnins.Count == 0
                || !m_potentialTurninsAreTeamSpecific)
            {
                return m_flagTurninTeamB;
            }

            if (m_turninRegionIndex_TeamB != -1)
            {
                return m_potentialFlagTurnins[m_turninRegionIndex_TeamB];
            }
            
            return null;
        }
    }

    private BoardRegion FlagTurninRegion_Neutral
    {
        get
        {
            if (TurninRegionState_Neutral != TurninRegionState.Active)
            {
                return null;
            }

            if (m_potentialFlagTurnins == null
                || m_potentialFlagTurnins.Count == 0
                || m_potentialTurninsAreTeamSpecific)
            {
                
                return m_flagTurninNeutral;
            }

            if (m_turninRegionIndex_Neutral != -1)
            {
                return m_potentialFlagTurnins[m_turninRegionIndex_Neutral];
            }
            
            return null;
        }
    }

    public int Networkm_turninRegionState_TeamA
    {
        get => m_turninRegionState_TeamA;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive)
            {
                if (!syncVarHookGuard)
                {
                    syncVarHookGuard = true;
                    HookSetTurninRegionState_TeamA(value);
                    syncVarHookGuard = false;
                }
            }

            SetSyncVar(value, ref m_turninRegionState_TeamA, 1u);
        }
    }

    public int Networkm_turninRegionState_TeamB
    {
        get => m_turninRegionState_TeamB;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive)
            {
                if (!syncVarHookGuard)
                {
                    syncVarHookGuard = true;
                    HookSetTurninRegionState_TeamB(value);
                    syncVarHookGuard = false;
                }
            }

            SetSyncVar(value, ref m_turninRegionState_TeamB, 2u);
        }
    }

    public int Networkm_turninRegionState_Neutral
    {
        get => m_turninRegionState_Neutral;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive)
            {
                if (!syncVarHookGuard)
                {
                    syncVarHookGuard = true;
                    HookSetTurninRegionState_Neutral(value);
                    syncVarHookGuard = false;
                }
            }

            SetSyncVar(value, ref m_turninRegionState_Neutral, 4u);
        }
    }

    public int Networkm_turninRegionIndex_TeamA
    {
        get => m_turninRegionIndex_TeamA;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetTurninRegionIndex_TeamA(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_turninRegionIndex_TeamA, 8u);
        }
    }

    public int Networkm_turninRegionIndex_TeamB
    {
        get => m_turninRegionIndex_TeamB;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive)
            {
                if (!syncVarHookGuard)
                {
                    syncVarHookGuard = true;
                    HookSetTurninRegionIndex_TeamB(value);
                    syncVarHookGuard = false;
                }
            }

            SetSyncVar(value, ref m_turninRegionIndex_TeamB, 16u);
        }
    }

    public int Networkm_turninRegionIndex_Neutral
    {
        get => m_turninRegionIndex_Neutral;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive)
            {
                if (!syncVarHookGuard)
                {
                    syncVarHookGuard = true;
                    HookSetTurninRegionIndex_Neutral(value);
                    syncVarHookGuard = false;
                }
            }

            SetSyncVar(value, ref m_turninRegionIndex_Neutral, 32u);
        }
    }

    public int Networkm_numFlagDrops
    {
        get => m_numFlagDrops;
        [param: In]
        set
        {
            if (NetworkServer.localClientActive && !syncVarHookGuard)
            {
                syncVarHookGuard = true;
                HookSetNumFlagDrops(value);
                syncVarHookGuard = false;
            }

            SetSyncVar(value, ref m_numFlagDrops, 64u);
        }
    }

    public uint Networkm_sequenceSourceId
    {
        get => m_sequenceSourceId;
        [param: In]
        set => SetSyncVar(value, ref m_sequenceSourceId, 128u);
    }

    private void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this;
        }
        else
        {
            Log.Error("Multiple CaptureTheFlag components in this scene, remove extraneous ones.");
        }

        if (NetworkServer.active)
        {
            SequenceSource sequenceSource = new SequenceSource(null, null, false);
            Networkm_sequenceSourceId = sequenceSource.RootID;
        }
    }

    private void OnDestroy()
    {
        if (m_autoBoundary_spawn_neutral != null)
        {
            HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_spawn_neutral);
            m_autoBoundary_spawn_neutral = null;
        }

        if (m_autoBoundary_spawn_teamA != null)
        {
            HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_spawn_teamA);
            m_autoBoundary_spawn_teamA = null;
        }

        if (m_autoBoundary_spawn_teamB != null)
        {
            HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_spawn_teamB);
            m_autoBoundary_spawn_teamB = null;
        }

        if (m_autoBoundary_turnin_neutral != null)
        {
            HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_turnin_neutral);
            m_autoBoundary_turnin_neutral = null;
        }

        if (m_autoBoundary_turnin_teamA != null)
        {
            HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_turnin_teamA);
            m_autoBoundary_turnin_teamA = null;
        }

        if (m_autoBoundary_turnin_teamB != null)
        {
            HighlightUtils.DestroyBoundaryHighlightObject(m_autoBoundary_turnin_teamB);
            m_autoBoundary_turnin_teamB = null;
        }

        s_instance = null;
    }

    public static CaptureTheFlag Get()
    {
        return s_instance;
    }

    public void OnNewFlagStarted(CTF_Flag flag)
    {
        if (m_flags.Contains(flag))
        {
            return;
        }

        m_flags.Add(flag);
    }

    public void OnFlagDestroyed(CTF_Flag flag)
    {
        if (!m_flags.Contains(flag))
        {
            return;
        }

        m_flags.Remove(flag);
    }

    private CTF_Flag GetFlagByGuid(byte flagGuid)
    {
        foreach (CTF_Flag flag in m_flags)
        {
            if (flag != null && flag.m_flagGuid == flagGuid)
            {
                return flag;
            }
        }

        return null;
    }

    private List<CTF_Flag> GetFlagsHeldByActor_Server(ActorData actor)
    {
        List<CTF_Flag> list = null;
        foreach (CTF_Flag flag in m_flags)
        {
            if (flag != null && flag.ServerHolderActor == actor)
            {
                if (list == null)
                {
                    list = new List<CTF_Flag>();
                }

                list.Add(flag);
            }
        }

        return list;
    }

    private List<CTF_Flag> GetFlagsHeldByActor_Client(ActorData actor)
    {
        List<CTF_Flag> list = null;
        if (actor == null)
        {
            return null;
        }
        
        foreach (CTF_Flag flag in m_flags)
        {
            if (flag != null && flag.ClientHolderActor == actor)
            {
                if (list == null)
                {
                    list = new List<CTF_Flag>();
                }

                list.Add(flag);
            }
        }

        return list;
    }

    public static CTF_Flag GetMainFlag()
    {
        return s_instance != null
               && s_instance.m_flags != null
               && s_instance.m_flags.Count > 0
            ? s_instance.m_flags[0]
            : null;
    }

    public static BoardSquare GetMainFlagIdleSquare_Server()
    {
        CTF_Flag mainFlag = GetMainFlag();
        return mainFlag != null
            ? mainFlag.ServerIdleSquare
            : null;
    }

    public static BoardSquare GetMainFlagIdleSquare_Client()
    {
        CTF_Flag mainFlag = GetMainFlag();
        return mainFlag != null
            ? mainFlag.ClientIdleSquare
            : null;
    }

    public static ActorData GetMainFlagCarrier_Server()
    {
        CTF_Flag mainFlag = GetMainFlag();
        return mainFlag != null
            ? mainFlag.ServerHolderActor
            : null;
    }

    public static ActorData GetMainFlagCarrier_Client()
    {
        CTF_Flag mainFlag = GetMainFlag();
        return mainFlag != null
            ? mainFlag.ClientHolderActor
            : null;
    }

    public static List<ActorData> GetActorsRevealedByFlags_Client()
    {
        List<ActorData> list = new List<ActorData>();
        
        if (!NetworkClient.active || Get() == null || !Get().m_flagRevealsHolder)
        {
            return list;
        }
        
        foreach (CTF_Flag flag in Get().m_flags)
        {
            if (flag != null
                && flag.ClientHolderActor != null
                && !list.Contains(flag.ClientHolderActor))
            {
                list.Add(flag.ClientHolderActor);
            }
        }

        return list;
    }

    public static bool IsActorRevealedByFlag_Client(ActorData actor)
    {
        return GetActorsRevealedByFlags_Client().Contains(actor);
    }

    public static List<ActorData> GetActorsRevealedByFlags_Server()
    {
        List<ActorData> list = new List<ActorData>();
        
        if (!NetworkServer.active || Get() == null || !Get().m_flagRevealsHolder)
        {
            return list;
        }

        foreach (CTF_Flag flag in Get().m_flags)
        {
            if (flag != null
                && flag.GatheredHolderActor != null
                && !list.Contains(flag.GatheredHolderActor))
            {
                list.Add(flag.GatheredHolderActor);
            }
        }

        return list;
    }

    public static bool IsActorRevealedByFlag_Server(ActorData actor)
    {
        return GetActorsRevealedByFlags_Server().Contains(actor);
    }

    public static BoardRegion GetExtractionRegion()
    {
        return s_instance != null
            ? s_instance.FlagTurninRegion_Neutral
            : null;
    }

    public static BoardRegion GetExtractionRegionOfTeam(Team team)
    {
        if (s_instance == null)
        {
            return null;
        }
        
        switch (team)
        {
            case Team.TeamA:
                return s_instance.FlagTurninRegion_TeamA;
            case Team.TeamB:
                return s_instance.FlagTurninRegion_TeamB;
            default:
                return s_instance.FlagTurninRegion_Neutral;
        }
    }

    public static TurninRegionState GetExtractionRegionState()
    {
        return s_instance != null
            ? s_instance.TurninRegionState_Neutral
            : TurninRegionState.Disabled;
    }

    public static TurninRegionState GetExtractionRegionStateOfTeam(Team team)
    {
        if (s_instance == null)
        {
            return TurninRegionState.Disabled;
        }
        
        switch (team)
        {
            case Team.TeamA:
                return s_instance.TurninRegionState_TeamA;
            case Team.TeamB:
                return s_instance.TurninRegionState_TeamB;
            default:
                return s_instance.TurninRegionState_Neutral;
        }
    }

    private void Start()
    {
        m_flagSpawnsNeutral.Initialize();
        m_flagSpawnsTeamA.Initialize();
        m_flagSpawnsTeamB.Initialize();
        m_flagTurninTeamA.Initialize();
        m_flagTurninTeamB.Initialize();
        m_flagTurninNeutral.Initialize();
        foreach (BoardRegion turnin in m_potentialFlagTurnins)
        {
            turnin.Initialize();
        }

        TurninRegionState_TeamA = m_turninRegionInitialState;
        TurninRegionState_TeamB = m_turninRegionInitialState;
        TurninRegionState_Neutral = m_turninRegionInitialState;
        
        if (NetworkClient.active)
        {
            GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorDamaged_Client);
            GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorHealed_Client);
            GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActorGainedAbsorb_Client);
            GenerateBoundaryVisuals();
        }

        m_flags = new List<CTF_Flag>();
    }

    public void Client_OnActorDeath(ActorData actor)
    {
        bool wasHoldingFlag = false;
        List<ActorData> contributorsDirect = GameFlowData.Get().GetContributorsToKillOnClient(actor, true);
        List<ActorData> contributorsAll = GameFlowData.Get().GetContributorsToKillOnClient(actor);
        List<ActorData> deathblowsBy = new List<ActorData>();
        List<ActorData> takedownsBy = new List<ActorData>();
        
        foreach (CTF_Flag flag in m_flags)
        {
            if (flag.ClientHolderActor == null)
            {
                continue;
            }
            
            if (flag.ClientHolderActor == actor)
            {
                wasHoldingFlag = true;
                flag.OnDropped_Client(actor.GetMostRecentDeathSquare(), -1);
            }

            if (contributorsDirect.Contains(flag.ClientHolderActor))
            {
                deathblowsBy.Add(flag.ClientHolderActor);
            }

            if (contributorsAll.Contains(flag.ClientHolderActor))
            {
                takedownsBy.Add(flag.ClientHolderActor);
            }
        }

        if (wasHoldingFlag)
        {
            if (ObjectivePoints.Get() != null)
            {
                ObjectivePoints.Get().AdjustUnresolvedPoints(
                    m_objectivePointsData_flagHoldersTeam.m_pointsPerDeathOfFlagHolder,
                    actor.GetTeam());
                ObjectivePoints.Get().AdjustUnresolvedPoints(
                    m_objectivePointsData_otherTeam.m_pointsPerDeathOfFlagHolder,
                    actor.GetEnemyTeam());
            }
        }

        foreach (ActorData killer in deathblowsBy)
        {
            if (ObjectivePoints.Get() != null)
            {
                ObjectivePoints.Get().AdjustUnresolvedPoints(
                    m_objectivePointsData_flagHoldersTeam.m_pointsPerDeathblowByFlagHolder,
                    killer.GetTeam());
                ObjectivePoints.Get().AdjustUnresolvedPoints(
                    m_objectivePointsData_otherTeam.m_pointsPerDeathblowByFlagHolder,
                    killer.GetEnemyTeam());
            }
        }

        foreach (ActorData killer in takedownsBy)
        {
            if (ObjectivePoints.Get() != null)
            {
                ObjectivePoints.Get().AdjustUnresolvedPoints(
                    m_objectivePointsData_flagHoldersTeam.m_pointsPerTakedownByFlagHolder,
                    killer.GetTeam());
                ObjectivePoints.Get().AdjustUnresolvedPoints(
                    m_objectivePointsData_otherTeam.m_pointsPerTakedownByFlagHolder,
                    killer.GetEnemyTeam());
            }
        }
    }

    public void ExecuteClientGameModeEvent(ClientGameModeEvent gameModeEvent)
    {
        if (gameModeEvent == null)
        {
            return;
        }

        GameModeEventType eventType = gameModeEvent.m_eventType;
        CTF_Flag flag = GetFlagByGuid(gameModeEvent.m_objectGuid);
        if (flag == null)
        {
            return;
        }

        switch (eventType)
        {
            case GameModeEventType.Ctf_FlagPickedUp:
            {
                flag.OnPickedUp_Client(gameModeEvent.m_primaryActor, gameModeEvent.m_eventGuid);
                return;
            }
            case GameModeEventType.Ctf_FlagDropped:
            {
                flag.OnDropped_Client(gameModeEvent.m_square, gameModeEvent.m_eventGuid);
                m_clientUnresolvedNumFlagDrops++;
                return;
            }
            case GameModeEventType.Ctf_FlagTurnedIn:
            {
                TurnInFlag_Client(flag, gameModeEvent.m_primaryActor, gameModeEvent.m_square, gameModeEvent.m_eventGuid);
                return;
            }
            case GameModeEventType.Ctf_FlagSentToSpawn:
            {
                flag.OnReturned_Client(gameModeEvent.m_primaryActor);
                return;
            }
            default:
            {
                Debug.LogError("CaptureTheFlag trying to handle non-CtF event type " + eventType + ".");
                break;
            }
        }
    }

    private void TurnInFlag_Client(CTF_Flag flag, ActorData capturingActor, BoardSquare captureSquare, int eventGuid)
    {
        Team team = capturingActor.GetTeam();
        if (team == Team.TeamA)
        {
            m_rewardToCapturingTeam.ClientApplyRewardTo(Team.TeamA);
            m_rewardToOtherTeam.ClientApplyRewardTo(Team.TeamB);
        }
        else if (team == Team.TeamB)
        {
            m_rewardToCapturingTeam.ClientApplyRewardTo(Team.TeamB);
            m_rewardToOtherTeam.ClientApplyRewardTo(Team.TeamA);
        }

        m_timeToFocusCameraOnExtraction = Time.time + m_timeTillCameraFocusesOntoExtraction;
        m_lastExtractionSquare = captureSquare;
        flag.OnTurnedIn_Client(capturingActor, eventGuid);
    }

    public void Client_OnFlagHolderChanged(
        ActorData oldHolder,
        ActorData newHolder,
        bool beingTurnedIn,
        bool alreadyTurnedIn)
    {
        if (oldHolder != newHolder && !alreadyTurnedIn)
        {
            UI_CTF_BriefcasePanel.Get().UpdateFlagHolder(oldHolder, newHolder);
        }
    }

    public void Client_OnTurninStateChanged(
        Team turninRegionTeam,
        TurninRegionState prevState,
        TurninRegionState newState)
    {
        if (newState == prevState
            || InterfaceManager.Get() == null
            || GameFlowData.Get() == null
            || !GameFlowData.Get().LocalPlayerData)
        {
            return;
        }
        
        Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
        RelationshipToClient relationship;
        Color color;
        if ((teamViewing == Team.TeamA || teamViewing == Team.TeamB)
            && (turninRegionTeam == Team.TeamA || turninRegionTeam == Team.TeamB))
        {
            if (teamViewing == turninRegionTeam)
            {
                relationship = RelationshipToClient.Friendly;
                color = m_textColor_positive;
            }
            else
            {
                relationship = RelationshipToClient.Hostile;
                color = m_textColor_negative;
            }
        }
        else
        {
            relationship = RelationshipToClient.Neutral;
            color = m_textColor_neutral;
        }

        string alertText = StringUtil.TR(GetTurninStateChangedString(relationship, newState));
        InterfaceManager.Get().DisplayAlert(alertText, color, 5f, true, 1);
    }

    private string GetTurninStateChangedString(RelationshipToClient relationship, TurninRegionState newState)
    {
        switch (relationship)
        {
            case RelationshipToClient.Friendly:
                switch (newState)
                {
                    case TurninRegionState.Active:
                        return m_alliedExtractionPointNowActive;
                    case TurninRegionState.Disabled:
                        return m_alliedExtractionPointNowInactive;
                    case TurninRegionState.Locked:
                        return m_alliedExtractionPointNowUnlocking;
                }

                break;
            case RelationshipToClient.Hostile:
                switch (newState)
                {
                    case TurninRegionState.Active:
                        return m_enemyExtractionPointNowActive;
                    case TurninRegionState.Disabled:
                        return m_enemyExtractionPointNowInactive;
                    case TurninRegionState.Locked:
                        return m_enemyExtractionPointNowUnlocking;
                }

                break;
            case RelationshipToClient.Neutral:
                switch (newState)
                {
                    case TurninRegionState.Active:
                        return m_neutralExtractionPointNowActive;
                    case TurninRegionState.Disabled:
                        return m_neutralExtractionPointNowInactive;
                    case TurninRegionState.Locked:
                        return m_neutralExtractionPointNowUnlocking;
                }

                break;
        }

        Debug.LogWarning(
            "CaptureTheFlag trying to find string for turnin point changed, but failed.  "
            + $"Relationship = {relationship.ToString()}, new state = {newState.ToString()}.  "
            + "Returning empty string...");
        return string.Empty;
    }

    public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
    {
        if (args == null || !NetworkClient.active)
        {
            return;
        }

        switch (eventType)
        {
            case GameEventManager.EventType.ActorDamaged_Client:
            case GameEventManager.EventType.ActorHealed_Client:
            case GameEventManager.EventType.ActorGainedAbsorb_Client:
                OnActorHealthChanged(args, true);
                break;
        }
    }

    private void OnActorHealthChanged(GameEventManager.GameEventArgs args, bool clientMode)
    {
        if (m_evasionDropsFlags && ServerClientUtils.GetCurrentAbilityPhase() == AbilityPriority.Evasion)
        {
            return;
        }

        GameEventManager.ActorHitHealthChangeArgs actorHitHealthChangeArgs =
            args as GameEventManager.ActorHitHealthChangeArgs;
        bool fromCharacterSpecificAbility = actorHitHealthChangeArgs.m_fromCharacterSpecificAbility;
        List<CTF_Flag> flagsHeldByCaster = GetFlagsHeldByActor_Client(actorHitHealthChangeArgs.m_caster);
        if (flagsHeldByCaster != null && flagsHeldByCaster.Count > 0)
        {
            if (actorHitHealthChangeArgs.m_caster != null)
            {
                Team team = actorHitHealthChangeArgs.m_caster.GetTeam();
                Team opposingTeam = actorHitHealthChangeArgs.m_caster.GetEnemyTeam();
                float pointsPerHealthChangeFlagHolderTeam = GetPointsPerHealthChange(
                    m_objectivePointsData_flagHoldersTeam,
                    actorHitHealthChangeArgs.m_type,
                    true,
                    fromCharacterSpecificAbility);
                float pointsPerHealthChangeOtherTeam = GetPointsPerHealthChange(
                    m_objectivePointsData_otherTeam,
                    actorHitHealthChangeArgs.m_type,
                    true,
                    fromCharacterSpecificAbility);
                int pointsFlagHolderTeam = Mathf.RoundToInt(pointsPerHealthChangeFlagHolderTeam * actorHitHealthChangeArgs.m_amount);
                int pointsOtherTeam = Mathf.RoundToInt(pointsPerHealthChangeOtherTeam * actorHitHealthChangeArgs.m_amount);
                AdjustObjectivePoints(pointsFlagHolderTeam, team, clientMode);
                AdjustObjectivePoints(pointsOtherTeam, opposingTeam, clientMode);
            }
        }

        List<CTF_Flag> flagsHeldByTarget = GetFlagsHeldByActor_Client(actorHitHealthChangeArgs.m_target);
        if (flagsHeldByTarget == null)
        {
            return;
        }

        if (flagsHeldByTarget.Count > 0)
        {
            Team team = actorHitHealthChangeArgs.m_target.GetTeam();
            Team opposingTeam = actorHitHealthChangeArgs.m_target.GetEnemyTeam();
            float pointsPerHealthChangeFlagHolderTeam = GetPointsPerHealthChange(
                m_objectivePointsData_flagHoldersTeam,
                actorHitHealthChangeArgs.m_type,
                false,
                fromCharacterSpecificAbility);
            float pointsPerHealthChangeOtherTeam = GetPointsPerHealthChange(
                m_objectivePointsData_otherTeam,
                actorHitHealthChangeArgs.m_type,
                false,
                fromCharacterSpecificAbility);
            int pointsFlagHolderTeam = Mathf.RoundToInt(pointsPerHealthChangeFlagHolderTeam * actorHitHealthChangeArgs.m_amount);
            int pointsOtherTeam = Mathf.RoundToInt(pointsPerHealthChangeOtherTeam * actorHitHealthChangeArgs.m_amount);
            AdjustObjectivePoints(pointsFlagHolderTeam, team, clientMode);
            AdjustObjectivePoints(pointsOtherTeam, opposingTeam, clientMode);
        }
    }

    private float GetPointsPerHealthChange(
        FlagHolderObjectivePointData data,
        GameEventManager.ActorHitHealthChangeArgs.ChangeType healthChangeType,
        bool outgoing,
        bool fromCharacterSpecificAbility)
    {
        if (!fromCharacterSpecificAbility && !data.m_includeContributionFromNonCharacterAbilities)
        {
            return 0f;
        }

        switch (healthChangeType)
        {
            case GameEventManager.ActorHitHealthChangeArgs.ChangeType.Damage:
                return outgoing
                    ? data.m_pointsPerDamageDealtByFlagHolder
                    : data.m_pointsPerDamageTakenByFlagHolder;
            case GameEventManager.ActorHitHealthChangeArgs.ChangeType.Healing when outgoing:
                return data.m_pointsPerHealingDealtByFlagHolder;
            case GameEventManager.ActorHitHealthChangeArgs.ChangeType.Healing:
                return data.m_pointsPerHealingTakenByFlagHolder;
            case GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb when outgoing:
                return data.m_pointsPerAbsorbDealtByFlagHolder;
            case GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb:
                return data.m_pointsPerAbsorbTakenByFlagHolder;
            default:
                return 0f;
        }
    }

    private void AdjustObjectivePoints(int points, Team team, bool clientMode)
    {
        if (points == 0)
        {
            return;
        }

        if (clientMode)
        {
            ObjectivePoints.Get().AdjustUnresolvedPoints(points, team);
        }
        else
        {
            ObjectivePoints.Get().AdjustPoints(points, team);
        }
    }

    public static bool AreCtfVictoryConditionsMetForTeam(CTF_VictoryCondition[] conditions, Team checkTeam)
    {
        if (Get() == null)
        {
            return true;
        }

        if (conditions == null || conditions.Length == 0)
        {
            return true;
        }
        
        if (checkTeam != Team.TeamA && checkTeam != Team.TeamB)
        {
            return true;
        }
        
        bool teamCapturedFlag;
        bool otherTeamCapturedFlag;
        if (checkTeam == Team.TeamA)
        {
            teamCapturedFlag = Get().m_teamACaptures > 0;
            otherTeamCapturedFlag = Get().m_teamBCaptures > 0;
        }
        else
        {
            teamCapturedFlag = Get().m_teamBCaptures > 0;
            otherTeamCapturedFlag = Get().m_teamACaptures > 0;
        }

        bool teamHoldingFlag = false;
        bool otherTeamHoldingFlag = false;
        foreach (CTF_Flag flag in Get().m_flags)
        {
            if (flag.ServerHolderActor == null)
            {
                continue;
            }
            
            if (flag.ServerHolderActor.GetTeam() == checkTeam)
            {
                teamHoldingFlag = true;
            }
            else
            {
                otherTeamHoldingFlag = true;
            }
        }

        foreach (CTF_VictoryCondition condition in conditions)
        {
            switch (condition)
            {
                case CTF_VictoryCondition.TeamMustBeHoldingFlag when !teamHoldingFlag:
                case CTF_VictoryCondition.TeamMustNotBeHoldingFlag when teamHoldingFlag:
                case CTF_VictoryCondition.OtherTeamMustBeHoldingFlag when !otherTeamHoldingFlag:
                case CTF_VictoryCondition.OtherTeamMustNotBeHoldingFlag when otherTeamHoldingFlag:
                case CTF_VictoryCondition.TeamMustHaveCapturedFlag when !teamCapturedFlag:
                case CTF_VictoryCondition.TeamMustNotHaveCapturedFlag when teamCapturedFlag:
                case CTF_VictoryCondition.OtherTeamMustHaveCapturedFlag when !otherTeamCapturedFlag:
                case CTF_VictoryCondition.OtherTeamMustNotHaveCapturedFlag when otherTeamCapturedFlag:
                    return false;
            }
        }

        return true;

    }

    protected void HookSetTurninRegionState_TeamA(int turninRegionState_TeamA)
    {
        TurninRegionState oldState = (TurninRegionState)m_turninRegionState_TeamA;
        Networkm_turninRegionState_TeamA = turninRegionState_TeamA;
        if (FlagTurninRegion_TeamA != null && FlagTurninRegion_TeamA.HasNonZeroArea())
        {
            Client_OnTurninStateChanged(Team.TeamA, oldState, TurninRegionState_TeamA);
        }

        OnTurninChanged_TeamA();
    }

    protected void HookSetTurninRegionState_TeamB(int turninRegionState_TeamB)
    {
        TurninRegionState oldState = (TurninRegionState)m_turninRegionState_TeamB;
        Networkm_turninRegionState_TeamB = turninRegionState_TeamB;
        if (FlagTurninRegion_TeamB != null && FlagTurninRegion_TeamB.HasNonZeroArea())
        {
            Client_OnTurninStateChanged(Team.TeamB, oldState, TurninRegionState_TeamB);
        }

        OnTurninChanged_TeamB();
    }

    protected void HookSetTurninRegionState_Neutral(int turninRegionState_Neutral)
    {
        TurninRegionState oldState = (TurninRegionState)m_turninRegionState_Neutral;
        Networkm_turninRegionState_Neutral = turninRegionState_Neutral;
        if (FlagTurninRegion_Neutral != null && FlagTurninRegion_Neutral.HasNonZeroArea())
        {
            Client_OnTurninStateChanged(Team.Invalid, oldState, TurninRegionState_Neutral);
        }

        OnTurninChanged_Neutral();
    }

    protected void HookSetTurninRegionIndex_TeamA(int turninRegionIndex_TeamA)
    {
        int oldIndex = m_turninRegionIndex_TeamA;
        Networkm_turninRegionIndex_TeamA = turninRegionIndex_TeamA;
        if (oldIndex == -1 && FlagTurninRegion_TeamA != null)
        {
            GenerateFlagTurninVisuals();
            if (TurninRegionState_TeamA != TurninRegionState.Disabled)
            {
                Client_OnTurninStateChanged(Team.TeamA, TurninRegionState.Disabled, TurninRegionState_TeamA);
            }
        }

        OnTurninChanged_TeamA();
    }

    protected void HookSetTurninRegionIndex_TeamB(int turninRegionIndex_TeamB)
    {
        int oldIndex = m_turninRegionIndex_TeamB;
        Networkm_turninRegionIndex_TeamB = turninRegionIndex_TeamB;
        if (oldIndex == -1 && FlagTurninRegion_TeamB != null)
        {
            GenerateFlagTurninVisuals();
            if (TurninRegionState_TeamB != TurninRegionState.Disabled)
            {
                Client_OnTurninStateChanged(Team.TeamB, TurninRegionState.Disabled, TurninRegionState_TeamB);
            }
        }

        OnTurninChanged_TeamB();
    }

    protected void HookSetTurninRegionIndex_Neutral(int turninRegionIndex_Neutral)
    {
        int oldIndex = m_turninRegionIndex_Neutral;
        Networkm_turninRegionIndex_Neutral = turninRegionIndex_Neutral;
        if (oldIndex == -1 && FlagTurninRegion_Neutral != null)
        {
            GenerateFlagTurninVisuals();
            if (TurninRegionState_Neutral != TurninRegionState.Disabled)
            {
                Client_OnTurninStateChanged(Team.Invalid, TurninRegionState.Disabled, TurninRegionState_Neutral);
            }
        }

        OnTurninChanged_Neutral();
    }

    protected void HookSetNumFlagDrops(int numFlagDrops)
    {
        Networkm_numFlagDrops = numFlagDrops;
        m_clientUnresolvedNumFlagDrops = 0;
    }

    private void OnTurninChanged_TeamA()
    {
        bool isRegionActive = TurninRegionState_TeamA == TurninRegionState.Active;
        bool isRegionValid = m_turninRegionIndex_TeamA >= 0;
        bool isPendingExtractionCamera = m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
        bool isPendingTeamCamera = m_timeToFocusCameraOnTurninTeamA >= 0f;
        if (isRegionActive && isRegionValid && isPendingExtractionCamera && !isPendingTeamCamera)
        {
            m_timeToFocusCameraOnTurninTeamA = Time.time + m_timeTillCameraFocusesOntoExtractionPoint;
        }

        if (FlagTurninRegion_TeamA != null && HUD_UI.Get() != null)
        {
            bool isLocalTeamA = GameFlowData.Get().activeOwnedActorData == null
                    || GameFlowData.Get().activeOwnedActorData.GetTeam() == Team.TeamA;

            if (isRegionActive && isRegionValid)
            {
                Team localTeam = isLocalTeamA ? Team.TeamA : Team.TeamB;
                HUD_UI.Get()
                    .m_mainScreenPanel
                    .m_offscreenIndicatorPanel
                    .AddCtfFlagTurnInRegion(FlagTurninRegion_TeamA, localTeam);
            }
            else
            {
                HUD_UI.Get()
                    .m_mainScreenPanel
                    .m_offscreenIndicatorPanel
                    .RemoveCtfFlagTurnInRegion(FlagTurninRegion_TeamA);
            }

        }
    }

    private void OnTurninChanged_TeamB()
    {
        bool isRegionActive = TurninRegionState_TeamB == TurninRegionState.Active;
        bool isRegionValid = m_turninRegionIndex_TeamB >= 0;
        bool isPendingExtractionCamera = m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
        bool isPendingTeamCamera = m_timeToFocusCameraOnTurninTeamB >= 0f;
        if (isRegionActive && isRegionValid && isPendingExtractionCamera && !isPendingTeamCamera)
        {
            m_timeToFocusCameraOnTurninTeamB = Time.time + m_timeTillCameraFocusesOntoExtractionPoint;
        }

        if (FlagTurninRegion_TeamB != null && HUD_UI.Get() != null)
        {
            bool isLocalTeamB = GameFlowData.Get().activeOwnedActorData != null
                    && GameFlowData.Get().activeOwnedActorData.GetTeam() == Team.TeamB;

            if (isRegionActive && isRegionValid)
            {
                Team otherTeam = isLocalTeamB ? Team.TeamA : Team.TeamB;
                HUD_UI.Get()
                    .m_mainScreenPanel
                    .m_offscreenIndicatorPanel
                    .AddCtfFlagTurnInRegion(FlagTurninRegion_TeamB, otherTeam);
            }
            else
            {
                HUD_UI.Get()
                    .m_mainScreenPanel
                    .m_offscreenIndicatorPanel
                    .RemoveCtfFlagTurnInRegion(FlagTurninRegion_TeamB);
            }

        }
    }

    private void OnTurninChanged_Neutral()
    {
        bool isRegionActive = TurninRegionState_Neutral == TurninRegionState.Active;
        bool isRegionValid = m_turninRegionIndex_Neutral >= 0;
        bool isPendingExtractionCamera = m_timeTillCameraFocusesOntoExtractionPoint >= 0f;
        bool isPendingTeamCamera = m_timeToFocusCameraOnTurninNeutral >= 0f;
        if (isRegionActive && isRegionValid && isPendingExtractionCamera && !isPendingTeamCamera)
        {
            m_timeToFocusCameraOnTurninNeutral = Time.time + m_timeTillCameraFocusesOntoExtractionPoint;
        }

        if (FlagTurninRegion_Neutral != null && HUD_UI.Get() != null)
        {
            if (isRegionActive && isRegionValid)
            {
                HUD_UI.Get()
                    .m_mainScreenPanel
                    .m_offscreenIndicatorPanel
                    .AddCtfFlagTurnInRegion(FlagTurninRegion_Neutral);
            }
            else
            {
                HUD_UI.Get()
                    .m_mainScreenPanel
                    .m_offscreenIndicatorPanel
                    .RemoveCtfFlagTurnInRegion(FlagTurninRegion_Neutral);
            }

        }
    }

    private GameObject InstantiateBoundaryObject(BoardRegion region, string boundaryName)
    {
        if (region == null || region.GetSquaresInRegion().Count <= 0)
        {
            return null;
        }
        
        GameObject highlightObject = HighlightUtils.Get().CreateBoundaryHighlight(
            region.GetSquaresInRegion(),
            Color.yellow);
        highlightObject.name = name + " " + boundaryName;
        DontDestroyOnLoad(highlightObject);
        m_autoBoundaryHeight = highlightObject.transform.position.y;
        return highlightObject;
    }

    private void GenerateBoundaryVisuals()
    {
        if (m_autoGenerateSpawnLocVisuals)
        {
            GenerateFlagSpawnBoundaryVisuals();
        }

        if (m_autoGenerateTurninVisuals)
        {
            GenerateFlagTurninVisuals();
        }
    }

    private void GenerateFlagSpawnBoundaryVisuals()
    {
        m_autoBoundary_spawn_neutral = InstantiateBoundaryObject(
            m_flagSpawnsNeutral,
            "Neutral Flag-Spawn Auto-Boundary");
        m_autoBoundary_spawn_teamA = InstantiateBoundaryObject(
            m_flagSpawnsTeamA,
            "TeamA Flag-Spawn Auto-Boundary");
        m_autoBoundary_spawn_teamB = InstantiateBoundaryObject(
            m_flagSpawnsTeamB, 
            "TeamB Flag-Spawn Auto-Boundary");
    }

    private void GenerateFlagTurninVisuals()
    {
        if (m_autoBoundary_turnin_neutral == null && FlagTurninRegion_Neutral != null)
        {
            m_autoBoundary_turnin_neutral = InstantiateBoundaryObject(
                FlagTurninRegion_Neutral,
                "Neutral Turnin Auto-Boundary");
        }

        if (m_autoBoundary_turnin_teamA == null && FlagTurninRegion_TeamA != null)
        {
            m_autoBoundary_turnin_teamA = InstantiateBoundaryObject(
                FlagTurninRegion_TeamA,
                "TeamA Turnin Auto-Boundary");
        }

        if (m_autoBoundary_turnin_teamB == null && FlagTurninRegion_TeamB != null)
        {
            m_autoBoundary_turnin_teamB = InstantiateBoundaryObject(
                FlagTurninRegion_TeamB,
                "TeamB Turnin Auto-Boundary");
        }
    }

    private void SetBoundaryColor(
        GameObject autoBoundary,
        Color mainColor,
        Color secondaryColor,
        float oscillationLevel)
    {
        if (autoBoundary == null)
        {
            return;
        }
        
        float mainAlpha = 1f - oscillationLevel * oscillationLevel;
        float secondaryAlpha = oscillationLevel * oscillationLevel;
        Color color = new Color(
            mainColor.r * mainAlpha + secondaryColor.r * secondaryAlpha,
            mainColor.g * mainAlpha + secondaryColor.g * secondaryAlpha,
            mainColor.b * mainAlpha + secondaryColor.b * secondaryAlpha,
            mainColor.a * mainAlpha + secondaryColor.a * secondaryAlpha);
        
        SetBoundaryColor(autoBoundary, color);
    }

    private void SetBoundaryColor(GameObject autoBoundary, Color color)
    {
        if (autoBoundary != null)
        {
            autoBoundary.GetComponent<Renderer>().material.SetColor("_TintColor", color);
        }
    }

    private Color DetermineSecondaryColor(
        bool condition1,
        Color color1,
        bool condition2,
        Color color2,
        Color fallbackColor)
    {
        return condition1
            ? color1
            : condition2
                ? color2
                : fallbackColor;
    }

    private void AdjustPositionOfObjToOscillation(GameObject obj, float oscillationLevel)
    {
        if (obj == null)
        {
            return;
        }

        float x = obj.transform.position.x;
        float y = m_autoBoundaryHeight + oscillationLevel * m_boundaryOscillationHeight;
        float z = obj.transform.position.z;
        obj.transform.position = new Vector3(x, y, z);
    }

    private void Update()
    {
        float oscillationLevel = (1f - Mathf.Cos(Time.time * m_boundaryOscillationSpeed)) / 2f;
        Team team = GameFlowData.Get() != null && GameFlowData.Get().LocalPlayerData != null
            ? GameFlowData.Get().LocalPlayerData.GetTeamViewing()
            : Team.Invalid;

        Color color;
        Color color2;
        switch (team)
        {
            case Team.TeamA:
                color = m_primaryColor_friendly;
                color2 = m_primaryColor_hostile;
                break;
            case Team.TeamB:
                color = m_primaryColor_hostile;
                color2 = m_primaryColor_friendly;
                break;
            default:
                color = m_primaryColor_neutral;
                color2 = m_primaryColor_neutral;
                break;
        }

        AdjustPositionOfObjToOscillation(m_autoBoundary_spawn_neutral, oscillationLevel);
        SetBoundaryColor(m_autoBoundary_spawn_neutral, m_primaryColor_neutral);
        AdjustPositionOfObjToOscillation(m_autoBoundary_spawn_teamA, oscillationLevel);
        SetBoundaryColor(m_autoBoundary_spawn_teamA, color);
        AdjustPositionOfObjToOscillation(m_autoBoundary_spawn_teamB, oscillationLevel);
        SetBoundaryColor(m_autoBoundary_spawn_teamB, color2);
        if (TurninRegionState_Neutral != TurninRegionState.Disabled)
        {
            AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_neutral, oscillationLevel);
            Color secondaryColor = TurninRegionState_Neutral != TurninRegionState.Locked
                ? m_primaryColor_neutral
                : m_secondaryColor_locked;
            SetBoundaryColor(m_autoBoundary_turnin_neutral, m_primaryColor_neutral, secondaryColor, oscillationLevel);
        }
        else
        {
            AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_neutral, 0f);
            SetBoundaryColor(
                color: new Color(
                    m_primaryColor_neutral.r * 0.5f,
                    m_primaryColor_neutral.g * 0.5f,
                    m_primaryColor_neutral.b * 0.5f,
                    m_primaryColor_neutral.a * 0.5f),
                autoBoundary: m_autoBoundary_turnin_neutral);
        }

        if (TurninRegionState_TeamA != TurninRegionState.Disabled)
        {
            AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_teamA, oscillationLevel);

            Color secondaryColor = TurninRegionState_TeamA == TurninRegionState.Locked
                ? m_secondaryColor_locked
                : color;
            SetBoundaryColor(m_autoBoundary_turnin_teamA, color, secondaryColor, oscillationLevel);
        }
        else
        {
            AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_teamA, 0f);
            SetBoundaryColor(
                color: new Color(color.r * 0.5f, color.g * 0.5f, color.b * 0.5f, color.a * 0.5f),
                autoBoundary: m_autoBoundary_turnin_teamA);
        }

        if (TurninRegionState_TeamB != TurninRegionState.Disabled)
        {
            AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_teamB, oscillationLevel);
            Color secondaryColor =
                TurninRegionState_TeamB != TurninRegionState.Locked ? color2 : m_secondaryColor_locked;
            SetBoundaryColor(m_autoBoundary_turnin_teamB, color2, secondaryColor, oscillationLevel);
        }
        else
        {
            AdjustPositionOfObjToOscillation(m_autoBoundary_turnin_teamB, 0f);
            SetBoundaryColor(
                color: new Color(color2.r * 0.5f, color2.g * 0.5f, color2.b * 0.5f, color2.a * 0.5f),
                autoBoundary: m_autoBoundary_turnin_teamB);
        }

        GetFlagCarrierDamageTillDropProgressForUI(out float cur, out float max);
        UI_CTF_BriefcasePanel uI_CTF_BriefcasePanel = UI_CTF_BriefcasePanel.Get();
        if (uI_CTF_BriefcasePanel != null)
        {
            if (!uI_CTF_BriefcasePanel.m_initialized)
            {
                uI_CTF_BriefcasePanel.Setup(this);
            }

            if (cur != m_lastFlagCarrierDamageCur || max != m_lastFlagCarrierDamageMax)
            {
                if (uI_CTF_BriefcasePanel.UpdateDamageForFlagHolder(cur, max))
                {
                    m_lastFlagCarrierDamageCur = cur;
                    m_lastFlagCarrierDamageMax = max;
                }
            }
        }

        if (m_timeToFocusCameraOnTurninTeamA > 0f && m_timeToFocusCameraOnTurninTeamA <= Time.time)
        {
            if (CameraManager.Get() != null
                && FlagTurninRegion_TeamA != null
                && FlagTurninRegion_TeamA.GetCenterSquare() != null)
            {
                CameraManager.Get().SetTargetObject(
                    FlagTurninRegion_TeamA.GetCenterSquare().gameObject,
                    CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
            }

            m_timeToFocusCameraOnTurninTeamA = -1f;
            CreateTurninRegionActivatedSequence(Team.TeamA, FlagTurninRegion_TeamA.GetCenter());
        }

        if (m_timeToFocusCameraOnTurninTeamB > 0f && m_timeToFocusCameraOnTurninTeamB <= Time.time)
        {
            if (CameraManager.Get() != null
                && FlagTurninRegion_TeamB != null
                && FlagTurninRegion_TeamB.GetCenterSquare() != null)
            {
                CameraManager.Get().SetTargetObject(
                    FlagTurninRegion_TeamB.GetCenterSquare().gameObject,
                    CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
            }

            m_timeToFocusCameraOnTurninTeamB = -1f;
            CreateTurninRegionActivatedSequence(Team.TeamB, FlagTurninRegion_TeamB.GetCenter());
        }

        if (m_timeToFocusCameraOnTurninNeutral > 0f && m_timeToFocusCameraOnTurninNeutral <= Time.time)
        {
            if (CameraManager.Get() != null
                && FlagTurninRegion_Neutral != null
                && FlagTurninRegion_Neutral.GetCenterSquare() != null)
            {
                CameraManager.Get().SetTargetObject(
                    FlagTurninRegion_Neutral.GetCenterSquare().gameObject,
                    CameraManager.CameraTargetReason.CtfTurninRegionSpawned);
            }

            m_timeToFocusCameraOnTurninNeutral = -1f;
            CreateTurninRegionActivatedSequence(Team.Invalid, FlagTurninRegion_Neutral.GetCenter());
        }

        if (m_timeToFocusCameraOnExtraction > 0f && m_timeToFocusCameraOnExtraction <= Time.time)
        {
            if (CameraManager.Get() != null && m_lastExtractionSquare != null)
            {
                CameraManager.Get().SetTargetObject(
                    m_lastExtractionSquare.gameObject,
                    CameraManager.CameraTargetReason.CtfFlagTurnedIn);
            }

            m_timeToFocusCameraOnExtraction = -1f;
            m_timeToFocusCameraOnTurninTeamA = -1f;
            m_timeToFocusCameraOnTurninTeamB = -1f;
            m_timeToFocusCameraOnTurninNeutral = -1f;
            m_lastExtractionSquare = null;
        }
    }

    public void CreateTurninRegionActivatedSequence(Team teamOfTurninRegionActivating, Vector3 centerPos)
    {
        Team team = GameFlowData.Get() != null && GameFlowData.Get().LocalPlayerData != null
            ? GameFlowData.Get().LocalPlayerData.GetTeamViewing()
            : Team.Invalid;

        GameObject sequence;
        if (teamOfTurninRegionActivating != Team.TeamA
            && teamOfTurninRegionActivating != Team.TeamB)
        {
            sequence = m_neutralTurninRegionActivatedSequence;
        }
        else if (team != teamOfTurninRegionActivating
                 && (teamOfTurninRegionActivating != Team.TeamA || team == Team.TeamB))
        {
            sequence = m_enemyTurninRegionActivatedSequence;
        }
        else
        {
            sequence = m_friendlyTurninRegionActivatedSequence;
        }

        if (sequence != null)
        {
            SequenceManager.Get().CreateClientSequences(
                sequence,
                centerPos,
                new ActorData[0],
                null,
                SequenceSource,
                null);
        }
    }

    public static float GetFlagCarrierDamageTillDropProgressForUI(out float cur, out float max)
    {
        CTF_Flag mainFlag = GetMainFlag();
        cur = -1f;
        max = -1f;
        int num = s_instance != null
            ? s_instance.m_numFlagDrops + s_instance.m_clientUnresolvedNumFlagDrops
            : 0;
        if (s_instance == null)
        {
            max = 1f;
            cur = 1f;
        }
        else if (s_instance.m_damageInOneTurnToDropFlag_gross > 0)
        {
            max = s_instance.m_damageInOneTurnToDropFlag_gross + s_instance.m_damageThesholdIncreaseOnDrop * num;
            if (mainFlag != null)
            {
                cur = mainFlag.DamageOnHolderSinceTurnStart_Gross + mainFlag.ClientUnresolvedDamageOnHolder;
            }
            else
            {
                cur = max;
            }
        }
        else if (s_instance.m_damageSincePickedUpToDropFlag_gross > 0)
        {
            max = s_instance.m_damageSincePickedUpToDropFlag_gross + s_instance.m_damageThesholdIncreaseOnDrop * num;
            if (mainFlag != null)
            {
                cur = mainFlag.DamageOnHolderSincePickedUp_Gross + mainFlag.ClientUnresolvedDamageOnHolder;
            }
            else
            {
                cur = max;
            }
        }
        else
        {
            cur = -1f;
            max = -1f;
        }

        return Mathf.Clamp(cur / max, 0f, 1f);
    }

    public static void OnActorDamaged_Client(ActorData actor, int damage)
    {
        CTF_Flag mainFlag = GetMainFlag();
        ActorData mainFlagCarrier = GetMainFlagCarrier_Client();
        if (s_instance != null
            && mainFlag != null
            && mainFlagCarrier != null
            && actor != null
            && mainFlagCarrier == actor)
        {
            mainFlag.ClientUnresolvedDamageOnHolder += damage;
        }
    }

    private void UNetVersion()
    {
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        if (forceAll)
        {
            writer.WritePackedUInt32((uint)m_turninRegionState_TeamA);
            writer.WritePackedUInt32((uint)m_turninRegionState_TeamB);
            writer.WritePackedUInt32((uint)m_turninRegionState_Neutral);
            writer.WritePackedUInt32((uint)m_turninRegionIndex_TeamA);
            writer.WritePackedUInt32((uint)m_turninRegionIndex_TeamB);
            writer.WritePackedUInt32((uint)m_turninRegionIndex_Neutral);
            writer.WritePackedUInt32((uint)m_numFlagDrops);
            writer.WritePackedUInt32(m_sequenceSourceId);
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

            writer.WritePackedUInt32((uint)m_turninRegionState_TeamA);
        }

        if ((syncVarDirtyBits & 2) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_turninRegionState_TeamB);
        }

        if ((syncVarDirtyBits & 4) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_turninRegionState_Neutral);
        }

        if ((syncVarDirtyBits & 8) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_turninRegionIndex_TeamA);
        }

        if ((syncVarDirtyBits & 0x10) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_turninRegionIndex_TeamB);
        }

        if ((syncVarDirtyBits & 0x20) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_turninRegionIndex_Neutral);
        }

        if ((syncVarDirtyBits & 0x40) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_numFlagDrops);
        }

        if ((syncVarDirtyBits & 0x80) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32(m_sequenceSourceId);
        }

        if (!flag)
        {
            writer.WritePackedUInt32(syncVarDirtyBits);
        }

        return flag;
    }

    public override void OnDeserialize(NetworkReader reader, bool initialState)
    {
        if (initialState)
        {
            m_turninRegionState_TeamA = (int)reader.ReadPackedUInt32();
            m_turninRegionState_TeamB = (int)reader.ReadPackedUInt32();
            m_turninRegionState_Neutral = (int)reader.ReadPackedUInt32();
            m_turninRegionIndex_TeamA = (int)reader.ReadPackedUInt32();
            m_turninRegionIndex_TeamB = (int)reader.ReadPackedUInt32();
            m_turninRegionIndex_Neutral = (int)reader.ReadPackedUInt32();
            m_numFlagDrops = (int)reader.ReadPackedUInt32();
            m_sequenceSourceId = reader.ReadPackedUInt32();
            return;
        }

        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            HookSetTurninRegionState_TeamA((int)reader.ReadPackedUInt32());
        }

        if ((num & 2) != 0)
        {
            HookSetTurninRegionState_TeamB((int)reader.ReadPackedUInt32());
        }

        if ((num & 4) != 0)
        {
            HookSetTurninRegionState_Neutral((int)reader.ReadPackedUInt32());
        }

        if ((num & 8) != 0)
        {
            HookSetTurninRegionIndex_TeamA((int)reader.ReadPackedUInt32());
        }

        if ((num & 0x10) != 0)
        {
            HookSetTurninRegionIndex_TeamB((int)reader.ReadPackedUInt32());
        }

        if ((num & 0x20) != 0)
        {
            HookSetTurninRegionIndex_Neutral((int)reader.ReadPackedUInt32());
        }

        if ((num & 0x40) != 0)
        {
            HookSetNumFlagDrops((int)reader.ReadPackedUInt32());
        }

        if ((num & 0x80) == 0)
        {
            return;
        }

        m_sequenceSourceId = reader.ReadPackedUInt32();
    }
}