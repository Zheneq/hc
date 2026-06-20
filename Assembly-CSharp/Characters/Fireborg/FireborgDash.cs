using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
using UnityEngine;

public class FireborgDash : GenericAbility_Container
{
    [Separator("Whether to add ground fire effect")]
    public bool m_addGroundFire = true;
    public int m_groundFireDuration = 1;
    public int m_groundFireDurationIfSuperheated = 1;
    public bool m_igniteIfNormal;
    public bool m_igniteIfSuperheated = true;
    [Separator("Shield per Enemy Hit")]
    public int m_shieldPerEnemyHit;
    public int m_shieldDuration = 1;
    [Separator("Cooldown Reduction")]
    public int m_cdrPerTurnIfLowHealth; // TODO FIREBORG unused, 0
    public int m_lowHealthThresh; // TODO FIREBORG unused, 0
    [Separator("Sequence")]
    public GameObject m_superheatedCastSeqPrefab; // TODO FIREBORG unused, null

    private Fireborg_SyncComponent m_syncComp;
    private AbilityMod_FireborgDash m_abilityMod;

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
        base.SetupTargetersAndCachedVars();
    }

    protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
    {
        base.AddSpecificTooltipTokens(tokens, modAsBase);
        AddTokenInt(tokens, "GroundFireDuration", string.Empty, m_groundFireDuration);
        AddTokenInt(tokens, "GroundFireDurationIfSuperheated", string.Empty, m_groundFireDurationIfSuperheated);
        AddTokenInt(tokens, "ShieldPerEnemyHit", string.Empty, m_shieldPerEnemyHit);
        AddTokenInt(tokens, "ShieldDuration", string.Empty, m_shieldDuration);
        AddTokenInt(tokens, "CdrPerTurnIfLowHealth", string.Empty, m_cdrPerTurnIfLowHealth);
        AddTokenInt(tokens, "LowHealthThresh", string.Empty, m_lowHealthThresh);
    }

    public bool AddGroundFire()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_addGroundFireMod.GetModifiedValue(m_addGroundFire)
            : m_addGroundFire;
    }

    public int GetGroundFireDuration()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_groundFireDurationMod.GetModifiedValue(m_groundFireDuration)
            : m_groundFireDuration;
    }

    public int GetGroundFireDurationIfSuperheated()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_groundFireDurationIfSuperheatedMod.GetModifiedValue(m_groundFireDurationIfSuperheated)
            : m_groundFireDurationIfSuperheated;
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

    public int GetShieldPerEnemyHit()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_shieldPerEnemyHitMod.GetModifiedValue(m_shieldPerEnemyHit)
            : m_shieldPerEnemyHit;
    }

    public int GetShieldDuration()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_shieldDurationMod.GetModifiedValue(m_shieldDuration)
            : m_shieldDuration;
    }

    public int GetCdrPerTurnIfLowHealth()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_cdrPerTurnIfLowHealthMod.GetModifiedValue(m_cdrPerTurnIfLowHealth)
            : m_cdrPerTurnIfLowHealth;
    }

    public int GetLowHealthThresh()
    {
        return m_abilityMod != null
            ? m_abilityMod.m_lowHealthThreshMod.GetModifiedValue(m_lowHealthThresh)
            : m_lowHealthThresh;
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
        IceborgConeOrLaser.SetShieldPerEnemyHitTargetingNumbers(
            targetActor,
            caster,
            GetShieldPerEnemyHit(),
            actorHitContext,
            results);
    }

    public override string GetAccessoryTargeterNumberString(
        ActorData targetActor,
        AbilityTooltipSymbol symbolType,
        int baseValue)
    {
        return AddGroundFire()
               && !m_syncComp.m_actorsInGroundFireOnTurnStart.Contains((uint)targetActor.ActorIndex)
            ? m_syncComp.GetTargetPreviewAccessoryString(symbolType, this, targetActor, ActorData)
            : null;
    }

    protected override void GenModImpl_SetModRef(AbilityMod abilityMod)
    {
        m_abilityMod = abilityMod as AbilityMod_FireborgDash;
    }

    protected override void GenModImpl_ClearModRef()
    {
        m_abilityMod = null;
    }

#if SERVER
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

        bool isSuperheated = m_syncComp.InSuperheatMode();
        bool isIgniting = isSuperheated ? IgniteIfSuperheated() : IgniteIfNormal();

        int enemiesHit = 0;
        foreach (ActorHitResults actorHitResult in actorHitResults)
        {
            ActorData hitActor = actorHitResult.m_hitParameters.Target;

            if (hitActor.GetTeam() == caster.GetTeam())
            {
                continue;
            }

            if (actorHitResult.HasDamage && isIgniting)
            {
                FireborgIgnitedEffect fireborgIgnitedEffect =
                    m_syncComp.MakeIgnitedEffect(AsEffectSource(), caster, hitActor);
                if (fireborgIgnitedEffect != null)
                {
                    actorHitResult.AddEffect(fireborgIgnitedEffect);
                }
            }

            enemiesHit++;
        }

        if (GetShieldPerEnemyHit() > 0 && enemiesHit > 0)
        {
            GetOrAddHitResults(caster, actorHitResults)
                .AddEffect(CreateShieldEffect(this, caster, GetShieldPerEnemyHit() * enemiesHit, GetShieldDuration()));
        }

        if (AddGroundFire())
        {
            TargetSelect_ChargeAoE targetSelect = GetTargetSelectComp() as TargetSelect_ChargeAoE;
            if (targetSelect != null)
            {
                Vector3 endPos = targetSelect.GetNonActorSpecificContext()
                    .GetValueVec3(ContextKeys.s_ChargeEndPos.GetKey());
                BoardSquare centerSquare = Board.Get().GetSquareFromVec3(endPos);
                float radiusAroundEnd = targetSelect.GetRadiusAroundEnd();

                BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
                List<BoardSquare> groundFireSquares =
                    AreaEffectUtils.GetSquaresInRadius(
                            centerSquare,
                            radiusAroundEnd + 0.5f,
                            false,
                            caster)
                        .Concat(
                            AreaEffectUtils.GetSquaresInBox(
                                squareAtPhaseStart.ToVector3(),
                                endPos,
                                targetSelect.GetRangeFromLine(),
                                false,
                                caster))
                        .Concat(new[] { squareAtPhaseStart })
                        .Distinct()
                        .ToList();

                if (groundFireSquares.Count > 0)
                {
                    // can hit evaders with ground fire reaction
                    positionHitResults.Add(
                        m_syncComp.MakeGroundFireEffectResults(
                            this,
                            caster,
                            groundFireSquares,
                            Board.Get().GetSquare(targets[0].GridPos).ToVector3(),
                            isSuperheated ? GetGroundFireDurationIfSuperheated() : GetGroundFireDuration(),
                            ServerAbilityUtils.CurrentlyGatheringRealResults(),
                            out FireborgGroundFireEffect effect,
                            out List<FireborgGroundFireEffect> oldEffectsForRemoval));
                    ActorHitResults casterHitResults = GetOrAddHitResults(caster, actorHitResults);
                    casterHitResults.AddEffect(effect);
                    oldEffectsForRemoval.ForEach(casterHitResults.AddEffectForRemoval);
                }
            }
        }
    }

    // custom
    public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
    {
        base.OnExecutedActorHit_Effect(caster, target, results);
        m_syncComp.OnExecutedActorHit_Effect(caster, target, results);
    }
#endif
}