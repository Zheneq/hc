using System.Collections.Generic;
using UnityEngine;

public class VisionPowerupInRangeMarkerSequence : Sequence
{
    public class IntervalTimer
    {
        private float m_duration;
        private float m_timeTillEnd;

        public IntervalTimer(float duration, float initialDuration)
        {
            m_duration = duration;
            m_timeTillEnd = initialDuration;
        }

        public void ClearTimeTillEnd()
        {
            m_timeTillEnd = 0f;
        }

        public bool TickTimer(float dt)
        {
            bool result = false;
            m_timeTillEnd -= dt;
            if (m_timeTillEnd <= 0f)
            {
                m_timeTillEnd = m_duration;
                result = true;
            }

            return result;
        }
    }

    [Separator("Main Ping VFX originating form center")]
    public GameObject m_largePingFxPrefab;
    [Separator("Marker FX to indicate actor is in range")]
    public GameObject m_markerFxPrefab;
    [JointPopup("Marker FX attach joint")]
    public JointPopupProperty m_markerFxJoint;
    [Separator("Ping Intervals")]
    public float m_largePingDuration = 5f;
    public float m_markerPingDuration = 1f;
    [Separator("Audio Events", "orange")]
    [AudioEvent(false)]
    public string m_audioEventFriendlyPing;
    [AudioEvent(false)]
    public string m_audioEventFriendlyPingSubsequent;
    [AudioEvent(false)]
    public string m_audioEventEnemyPing;
    [AudioEvent(false)]
    public string m_audioEventEnemyPingSubsequent;
    [AudioEvent(false)]
    public string m_audioEventTargetReveal;

    private bool m_canBeVisibleLastUpdate;
    private GameObject m_largePingFxInst;
    private FriendlyEnemyVFXSelector m_largePingFoFSelector;
    private Dictionary<ActorData, AttachedActorVFXInfo> m_actorToMarkerVfx = new Dictionary<ActorData, AttachedActorVFXInfo>();
    private List<ActorData> m_actorsToProcess = new List<ActorData>();
    private int m_updatingTurn = -1;
    private bool m_emittedLargePingThisTurn;
    private bool m_playedMarkerSfxThisTurn;
    private IntervalTimer m_largePingTimer;
    private IntervalTimer m_markerPingTimer;

    public override void FinishSetup()
    {
        base.FinishSetup();
        if (Caster != null)
        {
            foreach (ActorData actorData in GameFlowData.Get().GetActors())
            {
                if (actorData.GetTeam() != Caster.GetTeam())
                {
                    m_actorToMarkerVfx[actorData] = null;
                    m_actorsToProcess.Add(actorData);
                }
            }

            if (m_largePingFxPrefab != null)
            {
                m_markerFxJoint.Initialize(Caster.gameObject);
                m_largePingFxInst = InstantiateFX(m_largePingFxPrefab);
                m_largePingFxInst.transform.parent = m_markerFxJoint.m_jointObject.transform;
                m_largePingFxInst.transform.localPosition = Vector3.zero;
                m_largePingFxInst.transform.localRotation = Quaternion.identity;
                m_largePingFoFSelector = m_largePingFxInst.GetComponent<FriendlyEnemyVFXSelector>();
                if (m_largePingFoFSelector != null)
                {
                    m_largePingFoFSelector.Setup(Caster.GetTeam());
                }
            }
        }

        m_largePingTimer = new IntervalTimer(m_largePingDuration, 0f);
        m_markerPingTimer = new IntervalTimer(m_markerPingDuration, 0f);
        m_updatingTurn = GameFlowData.Get().CurrentTurn;
    }

    private void OnDisable()
    {
        foreach (ActorData actorData in m_actorsToProcess)
        {
            AttachedActorVFXInfo attachedActorVFXInfo = m_actorToMarkerVfx[actorData];
            if (attachedActorVFXInfo != null)
            {
                attachedActorVFXInfo.DestroyVfx();
            }
        }

        m_actorsToProcess.Clear();
        m_actorToMarkerVfx.Clear();
    }

    internal override void OnTurnStart(int currentTurn)
    {
        base.OnTurnStart(currentTurn);
        m_updatingTurn = GameFlowData.Get().CurrentTurn;
        m_emittedLargePingThisTurn = false;
        m_playedMarkerSfxThisTurn = false;
        m_largePingTimer.ClearTimeTillEnd();
        m_markerPingTimer.ClearTimeTillEnd();
    }

    private void Update()
    {
        if (m_markerFxPrefab == null || GameFlowData.Get() == null)
        {
            return;
        }

        bool isActive = GameFlowData.Get().gameState == GameState.BothTeams_Decision
                    && Caster != null
                    && !Caster.IsDead()
                    && Caster.GetCurrentBoardSquare() != null
                    && GameFlowData.Get().CurrentTurn == m_updatingTurn;
        if (isActive)
        {
            bool isAlly = GameFlowData.Get().activeOwnedActorData != null
                         && GameFlowData.Get().activeOwnedActorData.GetTeam() == Caster.GetTeam();

            float visionRange = Caster.GetSightRange();
            ActorAdditionalVisionProviders actorAdditionalVisionProviders = Caster.GetAdditionalActorVisionProviders();
            if (actorAdditionalVisionProviders != null)
            {
                SyncListVisionProviderInfo visionProviders = actorAdditionalVisionProviders.GetVisionProviders();
                foreach (VisionProviderInfo visionProvider in visionProviders)
                {
                    if (visionProvider.m_actorIndex == Caster.ActorIndex
                        && visionProvider.m_satelliteIndex < 0
                        && visionProvider.m_radius > visionRange)
                    {
                        visionRange = visionProvider.m_radius;
                    }
                }
            }

            bool pingLarge = m_largePingTimer.TickTimer(GameTime.deltaTime);
            bool pingMarker = m_markerPingTimer.TickTimer(GameTime.deltaTime);
            bool isVisible = false;
            foreach (ActorData actorData in m_actorsToProcess)
            {
                BoardSquare currentBoardSquare = actorData.GetCurrentBoardSquare();
                AttachedActorVFXInfo attachedActorVFXInfo = m_actorToMarkerVfx[actorData];
                if (!actorData.IsDead()
                    && currentBoardSquare != null
                    && !actorData.IsInRagdoll()
                    && actorData.IsActorVisibleToClient()
                    && Caster.GetCurrentBoardSquare().HorizontalDistanceOnBoardTo(currentBoardSquare) <= visionRange)
                {
                    if (attachedActorVFXInfo == null)
                    {
                        attachedActorVFXInfo = new AttachedActorVFXInfo(
                            m_markerFxPrefab,
                            actorData,
                            m_markerFxJoint,
                            false,
                            "VisionMarker",
                            AttachedActorVFXInfo.FriendOrFoeVisibility.Both);
                        attachedActorVFXInfo.SetCasterTeam(Caster.GetTeam());
                        m_actorToMarkerVfx[actorData] = attachedActorVFXInfo;
                    }

                    attachedActorVFXInfo.UpdateVisibility(true, !isAlly);
                    if (pingMarker)
                    {
                        attachedActorVFXInfo.RestartEffects();
                    }

                    isVisible = true;
                }
                else if (attachedActorVFXInfo != null)
                {
                    attachedActorVFXInfo.UpdateVisibility(false, false);
                }
            }

            if (m_largePingFxInst != null)
            {
                m_largePingFxInst.SetActiveIfNeeded(true);
                if (IsActorConsideredVisible(Caster))
                {
                    if (m_largePingFoFSelector != null)
                    {
                        m_largePingFoFSelector.Setup(Caster.GetTeam());
                    }

                    if (pingLarge)
                    {
                        foreach (PKFxFX pKFxFX in m_largePingFxInst.GetComponentsInChildren<PKFxFX>())
                        {
                            pKFxFX.TerminateEffect();
                            pKFxFX.StartEffect();
                            if (isAlly)
                            {
                                string audioEvent = !m_emittedLargePingThisTurn
                                    ? m_audioEventFriendlyPing
                                    : m_audioEventFriendlyPingSubsequent;
                                PlayAudioEvent(audioEvent, Caster.gameObject);
                            }
                            else
                            {
                                string audioEvent = !m_emittedLargePingThisTurn
                                    ? m_audioEventEnemyPing
                                    : m_audioEventEnemyPingSubsequent;
                                PlayAudioEvent(audioEvent, Caster.gameObject);  
                            }
                        }

                        m_emittedLargePingThisTurn = true;
                    }
                }
            }

            if (isVisible && !m_playedMarkerSfxThisTurn)
            {
                m_playedMarkerSfxThisTurn = true;
                PlayAudioEvent(m_audioEventTargetReveal, Caster.gameObject);
            }
        }
        else if (m_canBeVisibleLastUpdate)
        {
            foreach (ActorData actor in m_actorsToProcess)
            {
                AttachedActorVFXInfo attachedActorVFXInfo = m_actorToMarkerVfx[actor];
                if (attachedActorVFXInfo != null)
                {
                    attachedActorVFXInfo.UpdateVisibility(false, false);
                }
            }

            if (m_largePingFxInst != null)
            {
                m_largePingFxInst.SetActiveIfNeeded(false);
            }
        }

        m_canBeVisibleLastUpdate = isActive;
    }

    private void PlayAudioEvent(string audioEvent, GameObject sourceObj)
    {
        if (!audioEvent.IsNullOrEmpty())
        {
            AudioManager.PostEvent(audioEvent, sourceObj);
        }
    }
}