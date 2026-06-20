// ROGUES
// SERVER

using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
using UnityEngine;

public class FireborgDamageAura : GenericAbility_Container
{
    [Separator("Damage Aura")]
    public bool m_excludeTargetedActor = true;
    public int m_auraDuration = 1;
    public int m_auraDurationIfSuperheated = 1;
    public bool m_igniteIfNormal = true;
    public bool m_igniteIfSuperheated = true;
    [Separator("Effect on Cast Target")]
    public StandardEffectInfo m_onCastTargetAllyEffect;
    [Separator("Cooldown reduction")]
    public int m_cdrOnUltCast;
    [Separator("Sequences")]
    public GameObject m_auraPersistentSeqPrefab;
    public GameObject m_auraOnTriggerSeqPrefab;
    [Header("-- Superheated versions")]
    public GameObject m_superheatedCastSeqPrefab; // TODO FIREBORG unused, null
    public GameObject m_superheatedPersistentSeqPrefab; // null
    public GameObject m_superheatedOnTriggerSeqPrefab; // null

    private Fireborg_SyncComponent m_syncComp;
    private AbilityMod_FireborgDamageAura m_abilityMod;
    private StandardEffectInfo m_cachedOnCastTargetAllyEffect;

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

    protected override void SetupTargetersAndCachedVars()
    {
        m_syncComp = GetComponent<Fireborg_SyncComponent>();
        SetCachedFields();
        base.SetupTargetersAndCachedVars();
    }

    protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
    {
        base.AddSpecificTooltipTokens(tokens, modAsBase);
        AddTokenInt(tokens, "AuraDuration", string.Empty, m_auraDuration);
        AddTokenInt(tokens, "AuraDurationIfSuperheated", string.Empty, m_auraDurationIfSuperheated);
        AbilityMod.AddToken_EffectInfo(
            tokens,
            m_onCastTargetAllyEffect,
            "OnCastTargetAllyEffect",
            m_onCastTargetAllyEffect);
        AddTokenInt(tokens, "CdrOnUltCast", string.Empty, m_cdrOnUltCast);
    }

    private void SetCachedFields()
    {
        m_cachedOnCastTargetAllyEffect = m_abilityMod != null
            ? m_abilityMod.m_onCastTargetAllyEffectMod.GetModifiedValue(m_onCastTargetAllyEffect)
            : m_onCastTargetAllyEffect;
    }

    public bool ExcludeTargetedActor()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_excludeTargetedActorMod.GetModifiedValue(m_excludeTargetedActor)
            : m_excludeTargetedActor;
    }

    public int GetAuraDuration()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_auraDurationMod.GetModifiedValue(m_auraDuration)
            : m_auraDuration;
    }

    public int GetAuraDurationIfSuperheated()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_auraDurationIfSuperheatedMod.GetModifiedValue(m_auraDurationIfSuperheated)
            : m_auraDurationIfSuperheated;
    }

    public bool IgniteIfNormal()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_igniteIfNormalMod.GetModifiedValue(m_igniteIfNormal)
            : m_igniteIfNormal;
    }

    public bool IgniteIfSuperheated()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_igniteIfSuperheatedMod.GetModifiedValue(m_igniteIfSuperheated)
            : m_igniteIfSuperheated;
    }

    public StandardEffectInfo GetOnCastTargetAllyEffect()
    {
        return m_cachedOnCastTargetAllyEffect ?? m_onCastTargetAllyEffect;
    }

    public int GetCdrOnUltCast()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_cdrOnUltCastMod.GetModifiedValue(m_cdrOnUltCast)
            : m_cdrOnUltCast;
    }

    public override void PreProcessTargetingNumbers(
        ActorData targetActor,
        int currentTargetIndex,
        Dictionary<ActorData, ActorHitContext> actorHitContext,
        ContextVars abilityContext)
    {
        m_syncComp.SetSuperheatedContextVar(abilityContext);
    }

    public override void PostProcessTargetingNumbers(
        ActorData targetActor,
        int currentTargeterIndex,
        Dictionary<ActorData, ActorHitContext> actorHitContext,
        ContextVars abilityContext,
        ActorData caster,
        TargetingNumberUpdateScratch results)
    {
        if (targetActor.GetTeam() == caster.GetTeam())
        {
            StandardEffectInfo onCastTargetAllyEffect = GetOnCastTargetAllyEffect();
            if (!onCastTargetAllyEffect.m_applyEffect || onCastTargetAllyEffect.m_effectData.m_absorbAmount <= 0)
            {
                return;
            }

            BoardSquare targetSquare = Board.Get().GetSquare(Targeter.LastUpdatingGridPos);
            if (targetSquare == null || targetSquare != targetActor.GetCurrentBoardSquare())
            {
                return;
            }

            if (results.m_absorb >= 0)
            {
                results.m_absorb += onCastTargetAllyEffect.m_effectData.m_absorbAmount;
            }
            else
            {
                results.m_absorb = onCastTargetAllyEffect.m_effectData.m_absorbAmount;
            }
        }
        else if (m_excludeTargetedActor)
        {
            BoardSquare targetSquare = Board.Get().GetSquare(Targeter.LastUpdatingGridPos);
            if (targetSquare == null || targetSquare != targetActor.GetCurrentBoardSquare())
            {
                return;
            }

            results.m_damage = 0;
        }
    }

    public override bool ActorCountTowardsEnergyGain(ActorData target, ActorData caster)
    {
        if (!m_excludeTargetedActor || target.GetTeam() == caster.GetTeam())
        {
            return true;
        }

        BoardSquare targetSquare = Board.Get().GetSquare(Targeter.LastUpdatingGridPos);
        return targetSquare == null
               || targetSquare != target.GetCurrentBoardSquare();
    }

    protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
    {
        m_abilityMod = abilityMod as AbilityMod_FireborgDamageAura;
    }

    protected override void GenModImpl_ClearModRef()
    {
        m_abilityMod = null;
    }

#if SERVER
    // custom
    private AbilityAreaShape GetShape()
    {
        return (GetTargetSelectComp() as TargetSelect_Shape)?.m_shape ?? AbilityAreaShape.Five_x_Five_NoCorners;
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

        BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
        bool isSuperheated = m_syncComp.InSuperheatMode();

        ActorHitResults actorHitResult = actorHitResults
            .FirstOrDefault(ahr => ahr.m_hitParameters.Target.GetCurrentBoardSquare() == targetSquare);

        List<ActorHitResults> actualActorHitResults = new List<ActorHitResults>();
        if (actorHitResult != null)
        {
            ActorData hitActor = actorHitResult.m_hitParameters.Target;
            ActorHitResults hitResults = new ActorHitResults(actorHitResult.m_hitParameters);
            FireborgDamageAuraEffect effect = new FireborgDamageAuraEffect(
                AsEffectSource(),
                hitActor.GetCurrentBoardSquare(),
                hitActor,
                caster,
                GetShape(),
                GetOnHitAuthoredData(),
                isSuperheated ? GetAuraDurationIfSuperheated() : GetAuraDuration(),
                ExcludeTargetedActor(),
                isSuperheated ? IgniteIfSuperheated() : IgniteIfNormal(),
                isSuperheated && m_superheatedPersistentSeqPrefab != null
                    ? m_superheatedPersistentSeqPrefab
                    : m_auraPersistentSeqPrefab,
                isSuperheated && m_superheatedOnTriggerSeqPrefab != null
                    ? m_superheatedOnTriggerSeqPrefab
                    : m_auraOnTriggerSeqPrefab,
                m_syncComp);
            hitResults.AddEffect(effect);
            if (hitActor.GetTeam() == caster.GetTeam())
            {
                hitResults.AddStandardEffectInfo(GetOnCastTargetAllyEffect());
            }

            actualActorHitResults.Add(hitResults);
        }

        actorHitResults.Clear();
        positionHitResults.Clear();
        nonActorTargetInfo.Clear();
        actorHitResults.AddRange(actualActorHitResults);
    }

    // custom
    public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
    {
        base.OnExecutedActorHit_Effect(caster, target, results);
        m_syncComp.OnExecutedActorHit_Effect(caster, target, results);

        if (caster.GetTeam() != target.GetTeam() && results.m_hitParameters.Effect is FireborgDamageAuraEffect)
        {
            caster.GetFreelancerStats().AddToValueOfStat(
                FreelancerStats.FireborgStats.FireAuraDamage,
                results.FinalDamage);
        }
    }
#endif
}