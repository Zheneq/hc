using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class SimpleAttachedVFXSequence : Sequence
{
    public class MultiEventExtraParams : IExtraSequenceParams
    {
        public int eventNumberToKeyOffOf;

        public override void XSP_SerializeToStream(IBitStream stream)
        {
            short value = (short)eventNumberToKeyOffOf;
            stream.Serialize(ref value);
        }

        public override void XSP_DeserializeFromStream(IBitStream stream)
        {
            short value = 0;
            stream.Serialize(ref value);
            eventNumberToKeyOffOf = value;
        }
    }

    public class ImpactDelayParams : IExtraSequenceParams
    {
        public float impactDelayTime = -1f;
        public sbyte alternativeImpactAudioIndex = -1;

        public override void XSP_SerializeToStream(IBitStream stream)
        {
            stream.Serialize(ref impactDelayTime);
            stream.Serialize(ref alternativeImpactAudioIndex);
        }

        public override void XSP_DeserializeFromStream(IBitStream stream)
        {
            stream.Serialize(ref impactDelayTime);
            stream.Serialize(ref alternativeImpactAudioIndex);
        }
    }

    public class DelayedImpact
    {
        public float m_timeToSpawnImpact;
        public bool m_lastHit;

        public DelayedImpact(float timeToSpawn, bool lastHit)
        {
            m_timeToSpawnImpact = timeToSpawn;
            m_lastHit = lastHit;
        }
    }

    public enum AudioEventType
    {
        General,
        Pickup
    }

    [Serializable]
    public class HitVFXStatusFilters
    {
        public enum FilterCond
        {
            HasStatus,
            DoesntHaveStatus
        }

        public FilterCond m_condition;
        public StatusType m_status = StatusType.INVALID;
    }

    [Separator("Main FX Prefab")]
    public GameObject m_fxPrefab;
    [JointPopup("FX attach joint (or start position for projectiles).")]
    public JointPopupProperty m_fxJoint;
    public ReferenceModelType m_jointReferenceType;
    [Tooltip(
        "Check if Fx Prefab should stay attached to the joint. If unchecked, the Fx Prefab will start with the joint position and rotation.")]
    public bool m_fxAttachToJoint;
    [Separator("Anim Event -- ( main FX start / stop )", "orange")]
    [Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
    [AnimEventPicker]
    public Object m_startEvent;
    [AnimEventPicker]
    [Tooltip("Animation event (if any) to wait for before stopping the sequence. Search project for EventObjects.")]
    public Object m_stopEvent;
    [Separator("Rotation/Alignment")]
    [Tooltip(
        "Aim the Fx Prefab at the target (character or mouse click). If unchecked, inherits the attach joint transformation.")]
    public bool m_aimAtTarget = true;
    public bool m_useRootOrientation;
    [Separator("Audio Event -- ( on main FX spawn )", "orange")]
    [AudioEvent(false)]
    public string m_audioEvent;
    [Separator("Hit FX (on Targets)")]
    public bool m_playHitReactsWithoutFx;
    [Tooltip("FX at point(s) of impact")]
    public GameObject m_hitFxPrefab;
    [JointPopup("hit FX attach joint (or start position for projectiles).")]
    public JointPopupProperty m_hitFxJoint;
    public bool m_hitFxAttachToJoint;
    [Space(10f)]
    public CameraManager.CameraShakeIntensity m_hitCameraShakeType = CameraManager.CameraShakeIntensity.None;
    [Tooltip("Delay after Start Event before creating Hit Fx Prefab")]
    public float m_hitDelay;
    [Header("-- Orient hit fx to point from caster to target?")]
    public bool m_hitAlignedWithCaster;
    [Header("-- If orienting hit fx, whether to reverse direction (target to caster)")]
    public bool m_hitFxReverseAlignDir;
    [AnimEventPicker]
    [Separator("Anim Events -- ( hit timing )", "orange")]
    public Object m_hitEvent;
    [AnimEventPicker]
    public Object m_lastHitEvent;
    [Tooltip("Amount of time to trigger actual impact after hit react event has been received")]
    public float m_hitImpactDelayTime = -1f;
    [Separator("Audio Event -- ( played per target hit by default )", "orange")]
    [AudioEvent(false)]
    public string m_hitAudioEvent;
    public AudioEventType m_hitAudioEventType;
    [Header("-- Alternative Impact Audio Events, handled per ability, unused otherwise")]
    [AudioEvent(false)]
    public string[] m_alternativeImpactAudioEvents;
    [Separator("Team restrictions for Hit FX on Targets")]
    public HitVFXSpawnTeam m_hitVfxSpawnTeamMode;
    public List<HitVFXStatusFilters> m_hitVfxStatusRequirements = new List<HitVFXStatusFilters>();
    [Separator("Phase-Based Timing")]
    public PhaseTimingParameters m_phaseTimingParameters;

    protected GameObject m_fx;
    protected FriendlyEnemyVFXSelector m_mainFxFoFSelector;

    private List<GameObject> m_hitFx;
    private List<ActorData> m_hitFxAttachedActors = new List<ActorData>();
    private Dictionary<string, float> m_fxAttributes = new Dictionary<string, float>();
    private float m_hitSpawnTime = -1f;
    private bool m_playedHitReact;
    private bool m_spawnAttempted;
    private int m_eventNumberToKeyOffOf = -1;
    private int m_numStartEventsReceived;
    private List<DelayedImpact> m_delayedImpacts = new List<DelayedImpact>();
    private int m_alternativeAudioIndex = -1;

    private bool Finished()
    {
        bool result = false;
        if (!(GetFxPrefab() == null) && !AreFXFinished(m_fx))
        {
            return false;
        }

        if (m_hitFxPrefab == null)
        {
            if (m_playHitReactsWithoutFx && m_playedHitReact)
            {
                result = true;
            }
        }
        else if (m_hitFx != null)
        {
            result = true;
            foreach (GameObject current in m_hitFx)
            {
                if (current != null && current.activeSelf)
                {
                    result = false;
                    break;
                }
            }
        }

        if (m_delayedImpacts.Count > 0)
        {
            result = false;
        }

        return result;
    }

    internal override void Initialize(IExtraSequenceParams[] extraParams)
    {
        foreach (IExtraSequenceParams extraSequenceParams in extraParams)
        {
            MultiEventExtraParams multiEventExtraParams = extraSequenceParams as MultiEventExtraParams;
            if (multiEventExtraParams != null)
            {
                m_eventNumberToKeyOffOf = multiEventExtraParams.eventNumberToKeyOffOf;
            }

            ImpactDelayParams impactDelayParams = extraSequenceParams as ImpactDelayParams;
            if (impactDelayParams != null)
            {
                if (impactDelayParams.impactDelayTime > 0f)
                {
                    m_hitImpactDelayTime = impactDelayParams.impactDelayTime;
                }

                if (impactDelayParams.alternativeImpactAudioIndex >= 0)
                {
                    m_alternativeAudioIndex = impactDelayParams.alternativeImpactAudioIndex;
                }
            }

            if (!(extraSequenceParams is FxAttributeParam))
            {
                continue;
            }

            FxAttributeParam fxAttributeParam = extraSequenceParams as FxAttributeParam;
            if (fxAttributeParam == null || fxAttributeParam.m_paramNameCode == FxAttributeParam.ParamNameCode.None)
            {
                continue;
            }

            string attributeName = fxAttributeParam.GetAttributeName();
            float paramValue = fxAttributeParam.m_paramValue;
            if (fxAttributeParam.m_paramTarget != FxAttributeParam.ParamTarget.MainVfx)
            {
                continue;
            }

            if (!m_fxAttributes.ContainsKey(attributeName))
            {
                m_fxAttributes.Add(attributeName, paramValue);
            }
        }
    }

    internal override void OnTurnStart(int currentTurn)
    {
        m_phaseTimingParameters.OnTurnStart(currentTurn);
    }

    internal override void OnAbilityPhaseStart(AbilityPriority abilityPhase)
    {
        m_phaseTimingParameters.OnAbilityPhaseStart(abilityPhase);
        if (m_startEvent == null
            && !m_spawnAttempted
            && m_phaseTimingParameters.ShouldSpawnSequence(abilityPhase)
            && m_phaseTimingParameters.ShouldSequenceBeActive())
        {
            SpawnFX();
        }

        if (m_phaseTimingParameters.ShouldStopSequence(abilityPhase)
            && m_fx != null)
        {
            StopFX();
        }
    }

    internal override Vector3 GetSequencePos()
    {
        return m_fx != null ? m_fx.transform.position : Vector3.zero;
    }

    public override void FinishSetup()
    {
        if (m_startEvent == null)
        {
            SpawnFX();
        }
    }

    private bool IsHitFXVisibleForActor(ActorData hitTarget)
    {
        bool result = IsHitFXVisibleWrtTeamFilter(hitTarget, m_hitVfxSpawnTeamMode);
        if (result
            && m_hitVfxStatusRequirements != null
            && m_hitVfxStatusRequirements.Count > 0)
        {
            for (int i = 0; i < m_hitVfxStatusRequirements.Count && result; i++)
            {
                HitVFXStatusFilters hitVFXStatusFilters = m_hitVfxStatusRequirements[i];
                if (hitVFXStatusFilters.m_status == StatusType.INVALID)
                {
                    continue;
                }

                bool hasStatus = hitTarget.GetActorStatus().HasStatus(hitVFXStatusFilters.m_status);
                if ((hitVFXStatusFilters.m_condition != HitVFXStatusFilters.FilterCond.HasStatus || hasStatus)
                    && (hitVFXStatusFilters.m_condition != HitVFXStatusFilters.FilterCond.DoesntHaveStatus
                        || !hasStatus))
                {
                    continue;
                }

                result = false;
            }
        }

        return result;
    }

    protected virtual void SetFxRotation()
    {
        if (m_fx != null && m_useRootOrientation && Caster != null)
        {
            m_fx.transform.rotation = Caster.transform.rotation;
        }
    }

    protected virtual GameObject GetFxPrefab()
    {
        return m_fxPrefab;
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnUpdate()
    {
        if (!m_initialized)
        {
            return;
        }

        if (m_fx != null && m_fxAttachToJoint && m_jointReferenceType == ReferenceModelType.Actor
            && ShouldHideForActorIfAttached(Caster))
        {
            SetSequenceVisibility(false);
        }
        else
        {
            ProcessSequenceVisibility();
        }

        if (m_mainFxFoFSelector != null && Caster != null)
        {
            m_mainFxFoFSelector.Setup(Caster.GetTeam());
        }

        SetFxRotation();
        if (m_hitSpawnTime > 0f && GameTime.time > m_hitSpawnTime)
        {
            if (m_hitImpactDelayTime > 0f)
            {
                m_delayedImpacts.Add(new DelayedImpact(GameTime.time + m_hitImpactDelayTime, true));
            }
            else
            {
                SpawnHitFX(true);
            }

            m_hitSpawnTime = -1f;
        }

        for (int num = m_delayedImpacts.Count - 1; num >= 0; num--)
        {
            DelayedImpact delayedImpact = m_delayedImpacts[num];
            if (GameTime.time >= delayedImpact.m_timeToSpawnImpact)
            {
                SpawnHitFX(delayedImpact.m_lastHit);
                m_delayedImpacts.RemoveAt(num);
            }
        }

        if (m_hitFx != null
            && m_hitFx.Count > 0
            && Caster != null
            && m_hitFxAttachToJoint)
        {
            for (int i = 0; i < m_hitFx.Count; i++)
            {
                GameObject hitFx = m_hitFx[i];
                if (hitFx == null)
                {
                    continue;
                }

                if (m_hitAlignedWithCaster)
                {
                    Vector3 forward = hitFx.transform.position - Caster.GetFreePos();
                    forward.y = 0f;
                    if (forward.magnitude > 1E-05f)
                    {
                        forward.Normalize();
                        if (m_hitFxReverseAlignDir)
                        {
                            forward *= -1f;
                        }

                        Quaternion rotation = Quaternion.LookRotation(forward);
                        hitFx.transform.rotation = rotation;
                    }
                }

                if (i >= m_hitFxAttachedActors.Count)
                {
                    continue;
                }

                ActorData actorData = m_hitFxAttachedActors[i];
                if (actorData != null)
                {
                    hitFx.SetActiveIfNeeded(IsActorConsideredVisible(actorData));
                }
            }
        }

        if (Finished() && Source != null && !Source.RemoveAtEndOfTurn)
        {
            MarkForRemoval();
        }
    }

    protected void StopFX()
    {
        if (m_fx != null)
        {
            m_fx.SetActive(false);
        }
    }

    private void SpawnHitFX(bool lastHit)
    {
        m_playedHitReact = true;
        if (m_hitFx == null)
        {
            m_hitFx = new List<GameObject>();
        }

        if (Targets != null)
        {
            if (Targets.Length > 0)
            {
                CameraManager.Get().PlayCameraShake(m_hitCameraShakeType);
            }

            for (int i = 0; i < Targets.Length; i++)
            {
                ActorData actorData = i < Targets.Length ? Targets[i] : null;
                Vector3 targetHitPosition = GetTargetHitPosition(i, m_hitFxJoint);
                Vector3 position = Caster.transform.position;
                if ((position - Targets[i].transform.position).magnitude < 0.1f)
                {
                    position -= Caster.transform.forward * 0.5f;
                }

                Vector3 vector = targetHitPosition - position;
                vector.y = 0f;
                vector.Normalize();
                ActorModelData.ImpulseInfo impulseInfo = new ActorModelData.ImpulseInfo(targetHitPosition, vector);
                Quaternion quaternion;
                if (m_hitAlignedWithCaster)
                {
                    Vector3 forward = m_hitFxReverseAlignDir ? -1f * vector : vector;
                    quaternion = Quaternion.LookRotation(forward);
                }
                else
                {
                    quaternion = Quaternion.identity;
                }

                Quaternion rotation = quaternion;
                bool isVisibleForActor = IsHitFXVisibleForActor(Targets[i]);
                if (m_hitFxPrefab != null && isVisibleForActor)
                {
                    GameObject fxInstance = InstantiateFX(m_hitFxPrefab, targetHitPosition, rotation);
                    if (m_hitFxAttachToJoint && actorData != null)
                    {
                        m_hitFxJoint.Initialize(actorData.gameObject);
                        fxInstance.transform.parent = m_hitFxJoint.m_jointObject.transform;
                        fxInstance.transform.localPosition = Vector3.zero;
                        fxInstance.transform.localRotation = Quaternion.identity;
                    }
                    else
                    {
                        fxInstance.transform.parent = transform;
                    }

                    FriendlyEnemyVFXSelector selector = fxInstance.GetComponent<FriendlyEnemyVFXSelector>();
                    if (selector != null)
                    {
                        selector.Setup(Caster.GetTeam());
                    }

                    m_hitFx.Add(fxInstance);
                    m_hitFxAttachedActors.Add(Targets[i]);
                }

                if (isVisibleForActor)
                {
                    string audioEvent = m_hitAudioEvent;
                    if (m_alternativeAudioIndex >= 0
                        && m_alternativeAudioIndex < m_alternativeImpactAudioEvents.Length)
                    {
                        audioEvent = m_alternativeImpactAudioEvents[m_alternativeAudioIndex];
                    }

                    if ((m_hitAudioEventType != AudioEventType.Pickup || AudioManager.s_pickupAudio)
                        && !string.IsNullOrEmpty(audioEvent))
                    {
                        AudioManager.PostEvent(audioEvent, Targets[i].gameObject);
                    }
                }

                if (Targets[i] != null)
                {
                    if (!lastHit)
                    {
                        Source.OnSequenceHit(this, Targets[i], impulseInfo, ActorModelData.RagdollActivation.None);
                    }
                    else
                    {
                        Source.OnSequenceHit(this, Targets[i], impulseInfo);
                    }
                }
            }
        }

        Source.OnSequenceHit(this, TargetPos);
    }

    protected void SpawnFX(GameObject overrideFxPrefab = null)
    {
        m_spawnAttempted = true;
        if (!m_fxJoint.IsInitialized())
        {
            GameObject referenceModel = GetReferenceModel(Caster, m_jointReferenceType);
            if (referenceModel != null)
            {
                m_fxJoint.Initialize(referenceModel);
            }
        }

        GameObject fxPrefab = overrideFxPrefab;
        if (fxPrefab == null)
        {
            fxPrefab = GetFxPrefab();
        }

        if (fxPrefab != null)
        {
            Vector3 pos;
            if (m_fxJoint.m_jointObject != null
                && m_fxJoint.m_jointObject.transform.localScale != Vector3.zero &&
                m_fxAttachToJoint)
            {
                m_fx = InstantiateFX(fxPrefab);
                AttachToBone(m_fx, m_fxJoint.m_jointObject);
                m_fx.transform.localPosition = Vector3.zero;
                m_fx.transform.localRotation = Quaternion.identity;
                pos = m_fxJoint.m_jointObject.transform.position;
            }
            else
            {
                Vector3 vector = m_fxJoint.m_jointObject != null
                    ? m_fxJoint.m_jointObject.transform.position
                    : transform.position;
                Quaternion rotation = default(Quaternion);
                if (!m_aimAtTarget)
                {
                    rotation = m_fxJoint.m_jointObject != null
                        ? m_fxJoint.m_jointObject.transform.rotation
                        : transform.rotation;
                }
                else
                {
                    Vector3 targetPosition = GetTargetPosition(0);
                    Vector3 lookRotation = targetPosition - vector;
                    lookRotation.y = 0f;
                    lookRotation.Normalize();
                    rotation.SetLookRotation(lookRotation);
                }

                m_fx = InstantiateFX(fxPrefab, vector, rotation);
                pos = vector;
            }

            SetAttribute(m_fx, "abilityAreaLength", (TargetPos - pos).magnitude);
            if (m_fx != null)
            {
                m_mainFxFoFSelector = m_fx.GetComponent<FriendlyEnemyVFXSelector>();
                if (m_mainFxFoFSelector != null && Caster != null)
                {
                    m_mainFxFoFSelector.Setup(Caster.GetTeam());
                }
            }
        }

        if (m_hitEvent == null && m_playHitReactsWithoutFx)
        {
            if (m_hitDelay > 0f)
            {
                m_hitSpawnTime = GameTime.time + m_hitDelay;
            }
            else
            {
                SpawnHitFX(true);
            }
        }

        if (m_fx != null && m_fxAttributes != null)
        {
            foreach (KeyValuePair<string, float> attribute in m_fxAttributes)
            {
                SetAttribute(m_fx, attribute.Key, attribute.Value);
            }
        }

        if (!string.IsNullOrEmpty(m_audioEvent))
        {
            AudioManager.PostEvent(m_audioEvent, Caster.gameObject);
        }
    }

    protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
    {
        if (m_startEvent == parameter)
        {
            if (m_eventNumberToKeyOffOf >= 0 && m_numStartEventsReceived != m_eventNumberToKeyOffOf)
            {
                m_numStartEventsReceived++;
            }
            else
            {
                if (m_eventNumberToKeyOffOf >= 0)
                {
                    m_numStartEventsReceived++;
                }

                SpawnFX();
            }
        }
        else if (m_stopEvent == parameter && m_spawnAttempted)
        {
            StopFX();
        }

        if (m_hitEvent == parameter)
        {
            if (m_hitImpactDelayTime > 0f)
            {
                m_delayedImpacts.Add(new DelayedImpact(GameTime.time + m_hitImpactDelayTime, m_lastHitEvent == null));
            }
            else
            {
                SpawnHitFX(m_lastHitEvent == null);
            }
        }
        else if (m_lastHitEvent == parameter)
        {
            if (m_hitImpactDelayTime > 0f)
            {
                m_delayedImpacts.Add(new DelayedImpact(GameTime.time + m_hitImpactDelayTime, true));
            }
            else
            {
                SpawnHitFX(true);
            }
        }
    }

    private void OnDisable()
    {
        if (m_fx != null)
        {
            m_mainFxFoFSelector = null;
            Destroy(m_fx.gameObject);
            m_fx = null;
        }

        if (m_hitFx != null)
        {
            foreach (GameObject current in m_hitFx)
            {
                Destroy(current.gameObject);
            }

            m_hitFx = null;
        }
    }

    public override string GetSequenceSpecificDescription()
    {
        string desc = string.Empty;
        if (m_fxPrefab == null)
        {
            desc += "<color=yellow>WARNING: </color>No VFX Prefab for <FX Prefab>\n\n";
        }

        if (m_fxJoint != null && string.IsNullOrEmpty(m_fxJoint.m_joint))
        {
            desc += "<color=yellow>WARNING: </color>VFX joint is empty, may not spawn at expected location.\n\n";
        }

        if (m_jointReferenceType != 0)
        {
            desc += "<Joint Reference Type> is <color=cyan>" + m_jointReferenceType + "</color>\n\n";
        }

        if (m_fxJoint != null)
        {
            if (m_fxAttachToJoint)
            {
                desc += "<color=cyan>VFX is attaching to joint (" + m_fxJoint.m_joint + ")</color>\n";
                if (m_aimAtTarget)
                {
                    desc += "<[x] Aim At Target> ignored, attaching to joint\n";
                }

                desc += "\n";
            }
            else
            {
                desc += "VFX spawning at joint (<color=cyan>" + m_fxJoint.m_joint + "</color>), not set to attach.\n\n";
            }
        }

        if (m_useRootOrientation)
        {
            desc += "<[x] Use Root Orientaion> rotation is set to Caster's orientation per update\n\n";
        }

        if (m_hitEvent == null && !m_playHitReactsWithoutFx)
        {
            desc += "Ignoring Gameplay Hits\n";
            desc += "(If need Gameplay Hits, check <[x] Play Hit React Without Fx> or add <Hit Event>)\n\n";
        }
        else
        {
            desc += "<color=cyan>Can do Gameplay Hits</color>\n";
        }

        if (m_lastHitEvent != null)
        {
            desc += "Has <Last Hit Event>, will not trigger ragdoll until that event is fired\n\n";
        }

        if (m_hitEvent != null && m_hitDelay > 0f)
        {
            desc += "<color=yellow>WARNING: </color>Has <Hit Event>, <Hit Delay> will be ignored\n\n";
        }
        else if (m_hitEvent == null && m_hitDelay > 0f)
        {
            desc += "Using <Hit Delay> for timing, Gameplay Hit and <Hit FX Prefab> will spawn " + m_hitDelay
                + " second(s) after VFX spawn\n\n";
        }

        return desc;
    }
}