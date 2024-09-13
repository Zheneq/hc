// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;
using UnityEngine.Networking;

// TODO FIREBORG sequence targets a position (not a square, seemingly laser end) and only targets self
public class FireborgReactLasers : GenericAbility_Container
{
    [Serializable]
    public class HitEffectApplySetting
    {
        public bool m_firstNormal;
        public bool m_secondNormal;
        public bool m_firstSuperheated;
        public bool m_secondSuperheated;

        public bool ShouldApply(bool isFirst, bool superheated)
        {
            return isFirst && !superheated && m_firstNormal
                   || !isFirst && !superheated && m_secondNormal
                   || isFirst && superheated && m_firstSuperheated
                   || !isFirst && superheated && m_secondSuperheated;
        }
    }

    [Separator("On Hit Data - For Second Laser", "yellow")]
    public OnHitAuthoredData m_onHitDataForSecondLaser;
    [Separator("When to apply ignite and ground fire effects?")]
    public HitEffectApplySetting m_ignitedApplySetting;
    public HitEffectApplySetting m_groundFireApplySetting;
    [Separator("Extra Shield")]
    public int m_extraShieldIfLowHealth;
    public int m_lowHealthThresh;
    [Header("-- shield per damaging hit, applied on next turn")]
    public int m_shieldPerHitReceivedForNextTurn;
    [Header("-- shield applied on next turn if depleted this turn")]
    public int m_earlyDepleteShieldOnNextTurn;
    [Separator("Sequences")]
    public GameObject m_persistentSeqPrefab; // null
    public GameObject m_onTriggerSeqPrefab;
    public GameObject m_reactionAnimTriggerSeqPrefab;
    [Header("-- Superheated Sequences")]
    public GameObject m_superheatedCastSeqPrefab; // TODO FIREBORG unused, null
    public GameObject m_superheatedOnTriggerSeqPrefab; // TODO FIREBORG unused, null
    public float m_onTriggerProjectileSeqStartDelay;
    [Separator("Animation")]
    public int m_mainLaserAnimationIndex = 4;

    private Fireborg_SyncComponent m_syncComp;
    private AbilityData.ActionType m_myActionType;
    private AbilityMod_FireborgReactLasers m_abilityMod;
    private OnHitAuthoredData m_cachedOnHitDataForSecondLaser;

    public override string GetUsageForEditor()
    {
        return base.GetUsageForEditor() + Fireborg_SyncComponent.GetSuperheatedCvarUsage();
    }

    public override List<string> GetContextNamesForEditor()
    {
        List<string> contextNamesForEditor = base.GetContextNamesForEditor();
        contextNamesForEditor.Add(Fireborg_SyncComponent.s_cvarSuperheated.GetName());
        return contextNamesForEditor;
    }

    public override string GetOnHitDataDesc()
    {
        return base.GetOnHitDataDesc()
               + "-- On Hit Data for Reaction --\n"
               + m_onHitDataForSecondLaser.GetInEditorDesc();
    }

    protected override void SetupTargetersAndCachedVars()
    {
        m_syncComp = GetComponent<Fireborg_SyncComponent>();
        m_myActionType = GetActionTypeOfAbility(this);
        SetCachedFields();
        base.SetupTargetersAndCachedVars();
    }

    protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
    {
        base.AddSpecificTooltipTokens(tokens, modAsBase);
        m_onHitDataForSecondLaser.AddTooltipTokens(tokens);
        AddTokenInt(tokens, "ExtraShieldIfLowHealth", string.Empty, m_extraShieldIfLowHealth);
        AddTokenInt(tokens, "LowHealthThresh", string.Empty, m_lowHealthThresh);
        AddTokenInt(tokens, "ShieldPerHitReceivedForNextTurn", string.Empty, m_shieldPerHitReceivedForNextTurn);
        AddTokenInt(tokens, "EarlyDepleteShieldOnNextTurn", string.Empty, m_earlyDepleteShieldOnNextTurn);
    }

    private void SetCachedFields()
    {
        m_cachedOnHitDataForSecondLaser = m_abilityMod != null
            ? m_abilityMod.m_onHitDataForSecondLaserMod.GetModdedOnHitData(m_onHitDataForSecondLaser)
            : m_onHitDataForSecondLaser;
    }

    public OnHitAuthoredData GetOnHitDataForSecondLaser()
    {
        return m_cachedOnHitDataForSecondLaser ?? m_onHitDataForSecondLaser;
    }

    public int GetExtraShieldIfLowHealth()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_extraShieldIfLowHealthMod.GetModifiedValue(m_extraShieldIfLowHealth)
            : m_extraShieldIfLowHealth;
    }

    public int GetLowHealthThresh()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(m_lowHealthThresh)
            : m_lowHealthThresh;
    }

    public int GetShieldPerHitReceivedForNextTurn()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_shieldPerHitReceivedForNextTurnMod.GetModifiedValue(m_shieldPerHitReceivedForNextTurn)
            : m_shieldPerHitReceivedForNextTurn;
    }

    // TODO FIREBORG unused, always 0 in allowed mods
    public int GetEarlyDepleteShieldOnNextTurn()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_earlyDepleteShieldOnNextTurnMod.GetModifiedValue(m_earlyDepleteShieldOnNextTurn)
            : m_earlyDepleteShieldOnNextTurn;
    }

    public override void PostProcessTargetingNumbers(
        ActorData targetActor,
        int currentTargeterIndex,
        Dictionary<ActorData, ActorHitContext> actorHitContext,
        ContextVars abilityContext,
        ActorData caster,
        TargetingNumberUpdateScratch results)
    {
        if (targetActor == caster
            && GetExtraShieldIfLowHealth() > 0
            && caster.HitPoints < GetLowHealthThresh())
        {
            if (results.m_absorb >= 0)
            {
                results.m_absorb += GetExtraShieldIfLowHealth();
            }
            else
            {
                results.m_absorb = GetExtraShieldIfLowHealth();
            }
        }
    }

    public override string GetAccessoryTargeterNumberString(
        ActorData targetActor,
        AbilityTooltipSymbol symbolType,
        int baseValue)
    {
        return m_groundFireApplySetting.ShouldApply(true, m_syncComp.InSuperheatMode())
               && !m_syncComp.m_actorsInGroundFireOnTurnStart.Contains((uint)targetActor.ActorIndex)
            ? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
            : null;
    }

    protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
    {
        m_abilityMod = abilityMod as AbilityMod_FireborgReactLasers;
    }

    protected override void GenModImpl_ClearModRef()
    {
        m_abilityMod = null;
    }

    public override void OnClientCombatPhasePlayDataReceived(
        List<ClientResolutionAction> resolutionActions,
        ActorData caster)
    {
        if (!NetworkClient.active)
        {
            return;
        }

        ClientResolutionAction directResolutionAction = null;
        ClientResolutionAction reactionResolutionAction = null;
        foreach (ClientResolutionAction resolutionAction in resolutionActions)
        {
            if (directResolutionAction == null
                && resolutionAction.GetSourceAbilityActionType() == m_myActionType
                && resolutionAction.GetCaster() == caster
                && resolutionAction.IsResolutionActionType(ResolutionActionType.EffectAnimation))
            {
                directResolutionAction = resolutionAction;
            }

            if (reactionResolutionAction == null
                && resolutionAction.HasReactionHitByCaster(caster))
            {
                reactionResolutionAction = resolutionAction;
            }
        }

        if (directResolutionAction == null || reactionResolutionAction == null)
        {
            return;
        }

        int playOrderOfClientAction = TheatricsManager.Get()
            .GetPlayOrderOfClientAction(directResolutionAction, AbilityPriority.Combat_Damage);
        int playOrderOfFirstDamagingHitOnActor = TheatricsManager.Get()
            .GetPlayOrderOfFirstDamagingHitOnActor(caster, AbilityPriority.Combat_Damage);

        if (playOrderOfFirstDamagingHitOnActor < 0 || playOrderOfFirstDamagingHitOnActor >= playOrderOfClientAction)
        {
            return;
        }

        directResolutionAction.GetHitResults(
            out Dictionary<ActorData, ClientActorHitResults> actorHitResList,
            out Dictionary<Vector3, ClientPositionHitResults> posHitResList);
        reactionResolutionAction.GetReactionHitResultsByCaster(
            caster,
            out Dictionary<ActorData, ClientActorHitResults> reactionActorHitResults,
            out Dictionary<Vector3, ClientPositionHitResults> reactionPositionHitResults);

        if (actorHitResList == null
            || posHitResList == null
            || reactionActorHitResults == null
            || reactionPositionHitResults == null)
        {
            Debug.LogError(GetType() + " has empty hit results when trying to swap them on client");
            return;
        }

        List<ActorData> directHitActors = new List<ActorData>(actorHitResList.Keys);
        List<Vector3> directHitPositions = new List<Vector3>(posHitResList.Keys);
        foreach (ActorData directHitActor in directHitActors)
        {
            if (!reactionActorHitResults.ContainsKey(directHitActor))
            {
                continue;
            }

            ClientActorHitResults hitRes = actorHitResList[directHitActor];
            actorHitResList[directHitActor] = reactionActorHitResults[directHitActor];
            reactionActorHitResults[directHitActor] = hitRes;
            ClientActorHitResults newReactionHitRes = reactionActorHitResults[directHitActor];
            ClientActorHitResults newDirectHitRes = actorHitResList[directHitActor];
            if (m_syncComp.InSuperheatMode())
            {
                if (m_ignitedApplySetting.m_firstSuperheated
                    && newReactionHitRes.GetNumEffectsToStart() < newDirectHitRes.GetNumEffectsToStart())
                {
                    newReactionHitRes.SwapEffectsToStart(newDirectHitRes);
                }
            }
            else if (!m_ignitedApplySetting.m_firstNormal
                     && newReactionHitRes.GetNumEffectsToStart() > newDirectHitRes.GetNumEffectsToStart())
            {
                newReactionHitRes.SwapEffectsToStart(newDirectHitRes);
            }
            else if (m_ignitedApplySetting.m_firstNormal
                     && newReactionHitRes.GetNumEffectsToStart() < newDirectHitRes.GetNumEffectsToStart())
            {
                newReactionHitRes.SwapEffectsToStart(newDirectHitRes);
            }
        }

        foreach (Vector3 hitPos in directHitPositions)
        {
            if (reactionPositionHitResults.ContainsKey(hitPos))
            {
                ClientPositionHitResults posHitRes = posHitResList[hitPos];
                posHitResList[hitPos] = reactionPositionHitResults[hitPos];
                reactionPositionHitResults[hitPos] = posHitRes;
            }
        }
    }

#if SERVER
    // custom
    public bool IsCasterLowHealth(ActorData caster)
    {
        return GetLowHealthThresh() > 0
               && caster.HitPoints < GetLowHealthThresh();
    }

    // custom
    private float GetLaserWidth()
    {
        return (GetTargetSelectComp() as TargetSelect_Laser)?.GetLaserWidth() ?? 1.25f;
    }

    // custom
    private float GetLaserRange()
    {
        return (GetTargetSelectComp() as TargetSelect_Laser)?.GetLaserRange() ?? 7.5f;
    }

    // custom
    private bool GetIgnoreLos()
    {
        return (GetTargetSelectComp() as TargetSelect_Laser)?.IgnoreLos() ?? false;
    }

    // custom
    protected override void PreProcessForCalcAbilityHits(
        List<AbilityTarget> targets,
        ActorData caster,
        Dictionary<ActorData, ActorHitContext> actorHitContextMap,
        ContextVars abilityContext)
    {
        base.PreProcessForCalcAbilityHits(targets, caster, actorHitContextMap, abilityContext);

        m_syncComp.SetSuperheatedContextVar(abilityContext);
    }

    // custom
    protected override void ProcessGatheredHits(
        List<AbilityTarget> targets,
        ActorData caster,
        AbilityResults abilityResults,
        List<ActorHitResults> actorHitResults,
        List<PositionHitResults> positionHitResults,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        base.ProcessGatheredHits(
            targets,
            caster,
            abilityResults,
            actorHitResults,
            positionHitResults,
            nonActorTargetInfo);

        ActorHitResults casterHitResults = GetOrAddHitResults(caster, actorHitResults);
        casterHitResults.AddEffect(
            new FireborgReactLasersEffect(
                AsEffectSource(),
                caster.GetCurrentBoardSquare(),
                caster,
                caster,
                targets[0].AimDirection,
                GetLaserRange(),
                GetLaserWidth(),
                GetIgnoreLos(),
                GetOnHitAuthoredData(),
                GetOnHitDataForSecondLaser(),
                GetShieldPerHitReceivedForNextTurn(),
                m_ignitedApplySetting,
                m_groundFireApplySetting,
                m_mainLaserAnimationIndex,
                m_persistentSeqPrefab,
                m_onTriggerSeqPrefab,
                m_reactionAnimTriggerSeqPrefab,
                m_onTriggerProjectileSeqStartDelay,
                m_syncComp));

        if (IsCasterLowHealth(caster))
        {
            casterHitResults.AddEffect(CreateShieldEffect(this, caster, GetExtraShieldIfLowHealth(), 1));
        }

        actorHitResults.Clear();
        positionHitResults.Clear();
        nonActorTargetInfo.Clear();
        actorHitResults.Add(casterHitResults);
    }

    // custom
    public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
    {
        base.OnExecutedActorHit_Effect(caster, target, results);
        m_syncComp.OnExecutedActorHit_Effect(caster, target, results);
        
        if (caster.GetTeam() != target.GetTeam() && results.m_hitParameters.Effect is FireborgReactLasersEffect)
        {
            caster.GetFreelancerStats().AddToValueOfStat(
                FreelancerStats.FireborgStats.BlastwaveDamage,
                results.FinalDamage);
        }
    }
#endif
}