using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Networking;

public class Neko_SyncComponent : NetworkBehaviour, IForceActorOutlineChecker
{
    [Header("Return disc animation")]
    public int m_animIndexForStartOfDiscReturn;
    public int m_animIndexForPoweredUpDiscReturn;
    [Header("Return disc targeting")]
    public float m_discReturnTripLaserWidthInSquares = 1f;
    public float m_discReturnTripAoeRadiusAtlaserStart = 1f;
    public bool m_showTargetedGlowOnActorsInReturnDiscPath = true;
    public const int c_maxDiscLaserTemplates = 10;
    [Header("Indicator Colors")]
    public Color m_allyDiscIndicatorColor = Color.blue;
    public Color m_enemyDiscIndicatorColor = Color.red;
    public Color m_enlargedDiscEndpointColor = Color.blue;
    public Color m_returnDiscLineColor = Color.white;
    public Color m_fadeoutNonEnlargedDiscLineColor = Color.white;

    private SyncListInt m_boardX = new SyncListInt();
    private SyncListInt m_boardY = new SyncListInt();
    private float m_timeToWaitForValidationRequest;
    private const float c_waitDurationForValidation = 0.3f;

    [SyncVar]
    internal int m_homingActorIndex = -1;
    [SyncVar]
    internal bool m_superDiscActive;
    [SyncVar]
    internal int m_superDiscBoardX;
    [SyncVar]
    internal int m_superDiscBoardY;
    [SyncVar]
    internal int m_numUltConsecUsedTurns;
    internal int m_clientLastDiscBuffTurn = -1;
    internal BoardSquare m_clientDiscBuffTargetSquare;

    private const bool c_homingDiscStartFromCaster = false;
    private ActorData m_actorData;
    private AbilityData m_abilityData;
    private NekoBoomerangDisc m_primaryAbility;
    private NekoHomingDisc m_homingDiscAbility;
    private NekoEnlargeDisc m_enlargeDiscAbility;
    private AbilityData.ActionType m_enlargeDiscActionType = AbilityData.ActionType.INVALID_ACTION;
    private ActorTargeting m_actorTargeting;
    private bool m_showingTargeterTemplate;
    private List<Blaster_SyncComponent.HitAreaIndicatorHighlight> m_laserRangeMarkerForAlly;
    private List<GameObject> m_aoeRadiusMarkers;
    private GameObject m_endAoeMarker;
    private List<MeshRenderer[]> m_aoeMarkerRenderers;
    private List<ActorData> m_actorsTargetedByReturningDiscs = new List<ActorData>();
    private bool m_markedForForceUpdate;
    private bool m_setCasterPosLastFrame;
    private Vector3 m_lastCasterPos = Vector3.zero;

    private static int kListm_boardX = 1782002628;
    private static int kListm_boardY = 1782002629;

    public int Networkm_homingActorIndex
    {
        get { return m_homingActorIndex; }
        [param: In] set { SetSyncVar(value, ref m_homingActorIndex, 4u); }
    }

    public bool Networkm_superDiscActive
    {
        get { return m_superDiscActive; }
        [param: In] set { SetSyncVar(value, ref m_superDiscActive, 8u); }
    }

    public int Networkm_superDiscBoardX
    {
        get { return m_superDiscBoardX; }
        [param: In] set { SetSyncVar(value, ref m_superDiscBoardX, 16u); }
    }

    public int Networkm_superDiscBoardY
    {
        get { return m_superDiscBoardY; }
        [param: In] set { SetSyncVar(value, ref m_superDiscBoardY, 32u); }
    }

    public int Networkm_numUltConsecUsedTurns
    {
        get { return m_numUltConsecUsedTurns; }
        [param: In] set { SetSyncVar(value, ref m_numUltConsecUsedTurns, 64u); }
    }

    static Neko_SyncComponent()
    {
        RegisterSyncListDelegate(typeof(Neko_SyncComponent), kListm_boardX, InvokeSyncListm_boardX);
        RegisterSyncListDelegate(typeof(Neko_SyncComponent), kListm_boardY, InvokeSyncListm_boardY);
        NetworkCRC.RegisterBehaviour("Neko_SyncComponent", 0);
    }

    public static bool HomingDiscStartFromCaster()
    {
        return false;
    }

    public int GetNumActiveDiscs()
    {
        return m_boardX.Count;
    }

    public List<BoardSquare> GetActiveDiscSquares()
    {
        List<BoardSquare> list = new List<BoardSquare>();
        for (int i = 0; i < m_boardX.Count; i++)
        {
            list.Add(GetSquareForDisc(i));
        }

        return list;
    }

    private BoardSquare GetSquareForDisc(int index)
    {
        return Board.Get().GetSquareFromIndex(m_boardX[index], m_boardY[index]);
    }

    public bool ShouldForceShowOutline(ActorData forActor)
    {
        if (NetworkClient.active && GameFlowData.Get().activeOwnedActorData == m_actorData)
        {
            ActorTurnSM actorTurnSM = m_actorData.GetActorTurnSM();
            bool flag = true;
            if (actorTurnSM.CurrentState == TurnStateEnum.TARGETING_ACTION)
            {
                flag = false;
                Ability selectedAbility = m_abilityData.GetSelectedAbility();
                if (selectedAbility != null && selectedAbility.GetRunPriority() == AbilityPriority.Evasion)
                {
                    if (selectedAbility is NekoFlipDash nekoFlipDash)
                    {
                        if (actorTurnSM.GetAbilityTargets().Count >= (nekoFlipDash.ThrowDiscFromStart() ? 1 : 0))
                        {
                            flag = true;
                        }
                    }
                    else
                    {
                        flag = true;
                    }
                }
            }
            return flag && IsActorTargetedByReturningDiscs(forActor);
        }
        return false;
    }

    private void Start()
    {
        m_actorData = GetComponent<ActorData>();
        m_abilityData = m_actorData.GetAbilityData();
        if (m_abilityData != null)
        {
            m_primaryAbility = m_abilityData.GetAbilityOfType<NekoBoomerangDisc>();
            m_homingDiscAbility = m_abilityData.GetAbilityOfType<NekoHomingDisc>();
            m_enlargeDiscAbility = m_abilityData.GetAbilityOfType<NekoEnlargeDisc>();
            m_enlargeDiscActionType = m_abilityData.GetActionTypeOfAbility(m_enlargeDiscAbility);
        }
        m_actorTargeting = m_actorData.GetActorTargeting();
        if (NetworkClient.active)
        {
            m_laserRangeMarkerForAlly = new List<Blaster_SyncComponent.HitAreaIndicatorHighlight>(10);
            m_aoeRadiusMarkers = new List<GameObject>(10);
            m_aoeMarkerRenderers = new List<MeshRenderer[]>();
            for (int i = 0; i < 10; i++)
            {
                Blaster_SyncComponent.HitAreaIndicatorHighlight hitAreaIndicatorHighlight =
                    Blaster_SyncComponent.CreateHitAreaTemplate(
                        m_discReturnTripLaserWidthInSquares, m_returnDiscLineColor, false, 0.15f);
                hitAreaIndicatorHighlight.m_parentObj.SetActive(false);
                m_laserRangeMarkerForAlly.Add(hitAreaIndicatorHighlight);
                GameObject gameObject = HighlightUtils.Get()
                    .CreateDynamicConeMesh(m_discReturnTripAoeRadiusAtlaserStart, 360f, true);
                MeshRenderer[] componentsInChildren = gameObject.GetComponentsInChildren<MeshRenderer>();
                gameObject.SetActive(false);
                m_aoeRadiusMarkers.Add(gameObject);
                m_aoeMarkerRenderers.Add(componentsInChildren);
            }
            m_endAoeMarker = HighlightUtils.Get()
                .CreateDynamicConeMesh(m_discReturnTripAoeRadiusAtlaserStart, 360f, true);
            MeshRenderer[] componentsInChildren2 = m_endAoeMarker.GetComponentsInChildren<MeshRenderer>();
            MeshRenderer[] array = componentsInChildren2;
            foreach (MeshRenderer meshRenderer in array)
            {
                AbilityUtil_Targeter.SetMaterialColor(meshRenderer.materials, m_enlargedDiscEndpointColor, false);
            }

            if (m_actorData != null)
            {
                m_actorData.OnClientQueuedActionChangedDelegates += MarkForForceUpdate;
                m_actorData.OnSelectedAbilityChangedDelegates += OnSelectedAbilityChanged;
                m_actorData.AddForceShowOutlineChecker(this);
                GameFlowData.s_onActiveOwnedActorChange += OnActiveOwnedActorChange;
            }
        }
    }

    private void OnDestroy()
    {
        if (m_actorData != null)
        {
            m_actorData.OnClientQueuedActionChangedDelegates -= MarkForForceUpdate;
            m_actorData.OnSelectedAbilityChangedDelegates -= OnSelectedAbilityChanged;
            m_actorData.RemoveForceShowOutlineChecker(this);
            GameFlowData.s_onActiveOwnedActorChange -= OnActiveOwnedActorChange;
        }
    }

    private Vector3 GetDiscPos(int index)
    {
        if (index < m_boardX.Count)
        {
            BoardSquare squareForDisc = GetSquareForDisc(index);
            if (squareForDisc != null)
            {
                return squareForDisc.ToVector3();
            }
        }
        return Vector3.zero;
    }

    private Vector3 GetCasterPos(out bool hasQueuedEvades)
    {
        hasQueuedEvades = false;
        if (m_actorData == null)
        {
            return Vector3.zero;
        }
        BoardSquare casterSquare = null;
        if (m_actorData.GetCurrentBoardSquare() != null)
        {
            casterSquare = m_actorData.GetCurrentBoardSquare();
            List<AbilityData.AbilityEntry> queuedOrAimingAbilitiesForPhase =
                m_abilityData.GetQueuedOrAimingAbilitiesForPhase(UIQueueListPanel.UIPhase.Evasion);
            foreach (AbilityData.AbilityEntry current in queuedOrAimingAbilitiesForPhase)
            {
                AbilityData.ActionType actionTypeOfAbility = m_abilityData.GetActionTypeOfAbility(current.ability);
                List<AbilityTarget> abilityTargetsInRequest =
                    m_actorTargeting.GetAbilityTargetsInRequest(actionTypeOfAbility);
                if (!abilityTargetsInRequest.IsNullOrEmpty())
                {
                    casterSquare = Board.Get()
                        .GetSquare(abilityTargetsInRequest[abilityTargetsInRequest.Count - 1].GridPos);
                }
                else if (m_actorData.GetActorTurnSM().GetAbilityTargets().Count == current.ability.GetNumTargets() - 1)
                {
                    AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
                    casterSquare = Board.Get().GetSquare(abilityTargetForTargeterUpdate.GridPos);
                }
            }
            hasQueuedEvades = queuedOrAimingAbilitiesForPhase.Count > 0;
        }
        else if (m_actorData.GetMostRecentDeathSquare() != null)
        {
            casterSquare = m_actorData.GetMostRecentDeathSquare();
        }
        return casterSquare != null
            ? casterSquare.ToVector3()
            : Vector3.zero;
    }

    private Vector3 GetHomingActorPos()
    {
        ActorData actorData = m_actorData;
        if (m_homingActorIndex > 0)
        {
            actorData = GameFlowData.Get().FindActorByActorIndex(m_homingActorIndex);
        }
        if (actorData != null)
        {
            BoardSquare square = actorData.GetCurrentBoardSquare();
            if (actorData.IsDead())
            {
                square = actorData.GetMostRecentDeathSquare();
            }
            if (square != null)
            {
                return square.ToVector3();
            }
        }
        return Vector3.zero;
    }

    private bool IsDiscAtPosEnlarged(int discX, int discY, out bool enlargeDiscUsed)
    {
        bool result = false;
        enlargeDiscUsed = false;
        if (m_abilityData != null)
        {
            AbilityTarget abilityTarget = null;
            if (m_abilityData.HasQueuedAction(m_enlargeDiscActionType))
            {
                List<AbilityTarget> abilityTargetsInRequest =
                    m_actorTargeting.GetAbilityTargetsInRequest(m_enlargeDiscActionType);
                if (abilityTargetsInRequest != null && abilityTargetsInRequest.Count > 0)
                {
                    abilityTarget = abilityTargetsInRequest[0];
                }
            }
            else
            {
                Ability selectedAbility = m_abilityData.GetSelectedAbility();
                if (selectedAbility != null && selectedAbility is NekoEnlargeDisc)
                {
                    abilityTarget = AbilityTarget.GetAbilityTargetForTargeterUpdate();
                }
            }
            if (abilityTarget != null)
            {
                enlargeDiscUsed = true;
                if (m_homingActorIndex > 0 && HomingDiscStartFromCaster())
                {
                    return true;
                }

                result = abilityTarget.GridPos.x == discX && abilityTarget.GridPos.y == discY;
            }
        }

        return result;
    }

    private void Update()
    {
        if (!NetworkClient.active)
        {
            return;
        }
        bool setCasterPosLastFrame = m_setCasterPosLastFrame;
        m_setCasterPosLastFrame = false;
        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        bool shouldShowTargeterTemplate = activeOwnedActorData != null
                    && m_actorData.GetCurrentBoardSquare() != null
                    && m_boardX.Count > 0
                    && GameFlowData.Get() != null
                    && GameFlowData.Get().gameState == GameState.BothTeams_Decision;
        if (shouldShowTargeterTemplate)
        {
            int count = m_boardX.Count;
            float losHeight = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
            bool isAlly = activeOwnedActorData.GetTeam() == m_actorData.GetTeam();
            Vector3 casterPos = GetCasterPos(out bool hasQueuedEvades);
            bool isValidatingActionRequest = m_actorData.GetActorTurnSM().CurrentState == TurnStateEnum.VALIDATING_ACTION_REQUEST;
            if (isValidatingActionRequest && setCasterPosLastFrame)
            {
                casterPos = m_lastCasterPos;
                m_timeToWaitForValidationRequest = 0.3f;
            }
            else
            {
                if (m_timeToWaitForValidationRequest > 0f)
                {
                    m_timeToWaitForValidationRequest -= Time.unscaledDeltaTime;
                }
                if (!hasQueuedEvades
                    && m_timeToWaitForValidationRequest > 0f
                    && setCasterPosLastFrame)
                {
                    casterPos = m_lastCasterPos;
                }
            }
            m_setCasterPosLastFrame = true;
            m_lastCasterPos = casterPos;
            Ability selectedAbility = m_abilityData.GetSelectedAbility();
            bool isEvasionAbilitySelected = selectedAbility != null && selectedAbility.GetRunPriority() == AbilityPriority.Evasion;
            bool isNekoEnlargeDiskSelected = selectedAbility != null && selectedAbility is NekoEnlargeDisc;
            ActorData homingActor = m_homingActorIndex < 0
                ? m_actorData
                : GameplayUtils.GetActorOfActorIndex(m_homingActorIndex);
            bool hasHomingActor = homingActor != null;
            if (homingActor != null && homingActor.GetTeam() != m_actorData.GetTeam())
            {
                hasHomingActor = homingActor.GetCurrentBoardSquare() != null && homingActor.IsActorVisibleToClient();
            }

            if (!m_showingTargeterTemplate
                || m_showingTargeterTemplate != hasHomingActor
                || m_markedForForceUpdate
                || isValidatingActionRequest
                || isEvasionAbilitySelected
                || isNekoEnlargeDiskSelected)
            {
                m_markedForForceUpdate = false;
                m_actorsTargetedByReturningDiscs.Clear();
                for (int i = 0; i < m_laserRangeMarkerForAlly.Count; i++)
                {
                    Blaster_SyncComponent.HitAreaIndicatorHighlight hitAreaIndicatorHighlight = m_laserRangeMarkerForAlly[i];
                    GameObject aoeMarker = m_aoeRadiusMarkers[i];
                    MeshRenderer[] aoeMarkerRenderers = m_aoeMarkerRenderers[i];
                    hitAreaIndicatorHighlight.SetVisible(isAlly && i < m_boardX.Count);
                    bool active = (!HomingDiscStartFromCaster() || m_homingActorIndex <= 0) && i < m_boardX.Count;
                    aoeMarker.SetActive(active);
                    if (i >= m_boardX.Count)
                    {
                        continue;
                    }

                    Vector3 startLosPos;
                    Vector3 endLosPos;
                    if (m_homingActorIndex > 0)
                    {
                        startLosPos = HomingDiscStartFromCaster() ? casterPos : GetDiscPos(i);
                        startLosPos.y = losHeight;
                        endLosPos = GetHomingActorPos();
                        endLosPos.y = losHeight;
                    }
                    else
                    {
                        startLosPos = GetDiscPos(i);
                        startLosPos.y = losHeight;
                        endLosPos = casterPos;
                        endLosPos.y = losHeight;
                    }
                    
                    Vector3 startHighlightPos = startLosPos;
                    startHighlightPos.y = HighlightUtils.GetHighlightHeight();
                    if (!isAlly)
                    {
                        startHighlightPos.y += 0.01f;
                    }
                    aoeMarker.transform.position = startHighlightPos;

                    foreach (MeshRenderer meshRenderer in aoeMarkerRenderers)
                    {
                        AbilityUtil_Targeter.SetMaterialColor(
                            meshRenderer.materials,
                            isAlly ? m_allyDiscIndicatorColor : m_enemyDiscIndicatorColor,
                            false);
                    }
                    bool isEndHighlightSet = false;
                    if (hasHomingActor)
                    {
                        float returnDiskLaserWidth = m_discReturnTripLaserWidthInSquares;
                        float aoeStartRadius = m_discReturnTripAoeRadiusAtlaserStart;
                        float returnDiskEndRadius = 0f;
                        if (m_homingActorIndex > 0 && m_homingDiscAbility != null)
                        {
                            returnDiskEndRadius = m_homingDiscAbility.GetDiscReturnEndRadius();
                        }
                        else if (m_homingActorIndex < 0 && m_primaryAbility != null)
                        {
                            returnDiskEndRadius = m_primaryAbility.GetDiscReturnEndRadius();
                        }

                        bool isDiscEnlarged = IsDiscAtPosEnlarged(m_boardX[i], m_boardY[i], out bool enlargeDiscUsed);
                        if (isDiscEnlarged)
                        {
                            returnDiskLaserWidth = m_enlargeDiscAbility.GetLaserWidth();
                            aoeStartRadius = m_enlargeDiscAbility.GetAoeRadius();
                            returnDiskEndRadius = Mathf.Max(returnDiskEndRadius, m_enlargeDiscAbility.GetReturnEndAoeRadius());
                        }

                        if (returnDiskEndRadius > 0f)
                        {
                            Vector3 endHighlightPos = endLosPos;
                            endHighlightPos.y = HighlightUtils.GetHighlightHeight();
                            m_endAoeMarker.transform.position = endHighlightPos;
                            HighlightUtils.Get().AdjustDynamicConeMesh(m_endAoeMarker, returnDiskEndRadius, 360f);
                            isEndHighlightSet = true;
                        }

                        Vector3 adjustedStartPosWithOffset = VectorUtils.GetAdjustedStartPosWithOffset(
                            startLosPos,
                            endLosPos,
                            GameWideData.Get().m_laserInitialOffsetInSquares);
                        adjustedStartPosWithOffset.y = Board.Get().BaselineHeight + 0.01f;
                        Vector3 vec = endLosPos - startLosPos;
                        vec.y = 0f;
                        float distInSquares = vec.magnitude / Board.Get().squareSize;
                        hitAreaIndicatorHighlight.m_color = 
                            count <= 1 
                            || !enlargeDiscUsed 
                            || isDiscEnlarged
                                ? m_returnDiscLineColor
                                : m_fadeoutNonEnlargedDiscLineColor;

                        if (distInSquares > 0f)
                        {
                            hitAreaIndicatorHighlight.SetPose(adjustedStartPosWithOffset, vec.normalized);
                            hitAreaIndicatorHighlight.AdjustSize(returnDiskLaserWidth, distInSquares);
                        }
                        else
                        {
                            hitAreaIndicatorHighlight.SetVisible(false);
                        }
                        UpdateActorsInDiscPath(
                            startLosPos,
                            endLosPos,
                            returnDiskLaserWidth,
                            aoeStartRadius,
                            returnDiskEndRadius,
                            enlargeDiscUsed);
                    }
                    else
                    {
                        hitAreaIndicatorHighlight.SetVisible(false);
                    }

                    isEndHighlightSet = isEndHighlightSet && isAlly;
                    if (isEndHighlightSet != m_endAoeMarker.activeSelf)
                    {
                        m_endAoeMarker.SetActive(isEndHighlightSet);
                    }
                }
            }
        }
        else
        {
            foreach (Blaster_SyncComponent.HitAreaIndicatorHighlight current in m_laserRangeMarkerForAlly)
            {
                current.SetVisible(false);
            }
            foreach (GameObject aoeRadiusMarker in m_aoeRadiusMarkers)
            {
                aoeRadiusMarker.SetActive(false);
            }
            m_endAoeMarker.SetActive(false);
            m_actorsTargetedByReturningDiscs.Clear();
            m_timeToWaitForValidationRequest = 0f;
        }
        m_showingTargeterTemplate = shouldShowTargeterTemplate;
    }

    private void UpdateActorsInDiscPath(Vector3 startLosPos, Vector3 endLosPos, float laserWidth, float aoeStartRadius,
        float aoeEndRadius, bool usingEnlargeDiscAbility)
    {
        if (!m_showTargetedGlowOnActorsInReturnDiscPath)
        {
            return;
        }

        List<Team> opposingTeams = m_actorData.GetEnemyTeamAsList();
        if (usingEnlargeDiscAbility
            && m_enlargeDiscAbility != null
            && m_enlargeDiscAbility.CanIncludeAlliesOnReturn())
        {
            opposingTeams.Add(m_actorData.GetTeam());
        }

        Vector3 dir = endLosPos - startLosPos;
        dir.y = 0f;
        float laserRangeInSquares = dir.magnitude / Board.Get().squareSize;
        List<ActorData> actorsInStartRadius = AreaEffectUtils.GetActorsInRadius(
            startLosPos, aoeStartRadius, true, m_actorData, opposingTeams, null);
        foreach (ActorData actor in actorsInStartRadius)
        {
            if (!m_actorsTargetedByReturningDiscs.Contains(actor))
            {
                m_actorsTargetedByReturningDiscs.Add(actor);
            }
        }

        if (laserRangeInSquares <= 0f)
        {
            return;
        }

        List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
            startLosPos,
            dir,
            laserRangeInSquares,
            laserWidth,
            m_actorData,
            opposingTeams,
            true,
            0,
            true,
            false,
            out _,
            null);
        foreach (ActorData item in actorsInLaser)
        {
            if (!m_actorsTargetedByReturningDiscs.Contains(item))
            {
                m_actorsTargetedByReturningDiscs.Add(item);
            }
        }

        if (aoeEndRadius > 0f)
        {
            List<ActorData> actorsInEndRadius = AreaEffectUtils.GetActorsInRadius(
                endLosPos, aoeEndRadius, true, m_actorData, opposingTeams, null);
            foreach (ActorData actor in actorsInEndRadius)
            {
                if (!m_actorsTargetedByReturningDiscs.Contains(actor))
                {
                    m_actorsTargetedByReturningDiscs.Add(actor);
                }
            }
        }
    }

    public bool IsActorTargetedByReturningDiscs(ActorData actor)
    {
        return m_actorsTargetedByReturningDiscs.Contains(actor);
    }

    public void MarkForForceUpdate()
    {
        m_markedForForceUpdate = true;
        m_timeToWaitForValidationRequest = 0f;
    }

    public void OnSelectedAbilityChanged(Ability ability)
    {
        m_markedForForceUpdate = true;
    }

    private void OnActiveOwnedActorChange(ActorData activeActor)
    {
        m_markedForForceUpdate = true;
    }

    private void UNetVersion()
    {
    }

    protected static void InvokeSyncListm_boardX(NetworkBehaviour obj, NetworkReader reader)
    {
        if (!NetworkClient.active)
        {
            Debug.LogError("SyncList m_boardX called on server.");
            return;
        }

        ((Neko_SyncComponent)obj).m_boardX.HandleMsg(reader);
    }

    protected static void InvokeSyncListm_boardY(NetworkBehaviour obj, NetworkReader reader)
    {
        if (!NetworkClient.active)
        {
            Debug.LogError("SyncList m_boardY called on server.");
            return;
        }
        
        ((Neko_SyncComponent)obj).m_boardY.HandleMsg(reader);
    }

    private void Awake()
    {
        m_boardX.InitializeBehaviour(this, kListm_boardX);
        m_boardY.InitializeBehaviour(this, kListm_boardY);
    }

    public override bool OnSerialize(NetworkWriter writer, bool forceAll)
    {
        if (forceAll)
        {
            SyncListInt.WriteInstance(writer, m_boardX);
            SyncListInt.WriteInstance(writer, m_boardY);
            writer.WritePackedUInt32((uint)m_homingActorIndex);
            writer.Write(m_superDiscActive);
            writer.WritePackedUInt32((uint)m_superDiscBoardX);
            writer.WritePackedUInt32((uint)m_superDiscBoardY);
            writer.WritePackedUInt32((uint)m_numUltConsecUsedTurns);
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
            SyncListInt.WriteInstance(writer, m_boardX);
        }
        if ((syncVarDirtyBits & 2) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }
            SyncListInt.WriteInstance(writer, m_boardY);
        }
        if ((syncVarDirtyBits & 4) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }
            writer.WritePackedUInt32((uint)m_homingActorIndex);
        }
        if ((syncVarDirtyBits & 8) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }
            writer.Write(m_superDiscActive);
        }
        if ((syncVarDirtyBits & 0x10) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }
            writer.WritePackedUInt32((uint)m_superDiscBoardX);
        }
        if ((syncVarDirtyBits & 0x20) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }

            writer.WritePackedUInt32((uint)m_superDiscBoardY);
        }
        if ((syncVarDirtyBits & 0x40) != 0)
        {
            if (!flag)
            {
                writer.WritePackedUInt32(syncVarDirtyBits);
                flag = true;
            }
            writer.WritePackedUInt32((uint)m_numUltConsecUsedTurns);
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
            SyncListInt.ReadReference(reader, m_boardX);
            SyncListInt.ReadReference(reader, m_boardY);
            m_homingActorIndex = (int)reader.ReadPackedUInt32();
            m_superDiscActive = reader.ReadBoolean();
            m_superDiscBoardX = (int)reader.ReadPackedUInt32();
            m_superDiscBoardY = (int)reader.ReadPackedUInt32();
            m_numUltConsecUsedTurns = (int)reader.ReadPackedUInt32();
            return;
        }

        int num = (int)reader.ReadPackedUInt32();
        if ((num & 1) != 0)
        {
            SyncListInt.ReadReference(reader, m_boardX);
        }
        if ((num & 2) != 0)
        {
            SyncListInt.ReadReference(reader, m_boardY);
        }
        if ((num & 4) != 0)
        {
            m_homingActorIndex = (int)reader.ReadPackedUInt32();
        }
        if ((num & 8) != 0)
        {
            m_superDiscActive = reader.ReadBoolean();
        }
        if ((num & 0x10) != 0)
        {
            m_superDiscBoardX = (int)reader.ReadPackedUInt32();
        }
        if ((num & 0x20) != 0)
        {
            m_superDiscBoardY = (int)reader.ReadPackedUInt32();
        }
        if ((num & 0x40) != 0)
        {
            m_numUltConsecUsedTurns = (int)reader.ReadPackedUInt32();
        }
    }
}