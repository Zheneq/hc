// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

// custom
#if SERVER
public class ScampDelayedAoeEffect : StandardActorEffect
{
    private readonly float m_radius;
    private readonly OnHitAuthoredData m_onHitData;
    private readonly int m_extraDamageIfShieldDownForm;
    private readonly float m_subseqTurnDamageMultiplier; // TODO SCAMP unused, always -1
    private readonly bool m_subseqTurnNoEnergyGain; // TODO SCAMP unused, always false
    private readonly int m_animIndexOnTrigger;
    private readonly GameObject m_onTriggerSequencePrefab;

    private readonly Scamp_SyncComponent m_syncComp;
    private readonly Passive_Scamp m_passive;

    public ScampDelayedAoeEffect(
        EffectSource parent,
        ActorData target,
        ActorData caster,
        StandardActorEffectData data,
        float radius,
        OnHitAuthoredData onHitData,
        int extraDamageIfShieldDownForm,
        float subseqTurnDamageMultiplier,
        bool subseqTurnNoEnergyGain,
        int animIndexOnTrigger,
        GameObject onTriggerSequencePrefab,
        Scamp_SyncComponent syncComp,
        Passive_Scamp passive)
        : base(parent, null, target, caster, data)
    {
        m_radius = radius;
        m_onHitData = onHitData;
        m_extraDamageIfShieldDownForm = extraDamageIfShieldDownForm;
        m_subseqTurnDamageMultiplier = subseqTurnDamageMultiplier;
        m_subseqTurnNoEnergyGain = subseqTurnNoEnergyGain;
        m_animIndexOnTrigger = animIndexOnTrigger;
        m_onTriggerSequencePrefab = onTriggerSequencePrefab;
        m_syncComp = syncComp;
        m_passive = passive;
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_onTriggerSequencePrefab,
                Target.GetCurrentBoardSquare(),
                m_effectResults.HitActorsArray(),
                Target,
                SequenceSource,
                AbilityCommon_LayeredRings.GetAdjustableRingSequenceParams(m_radius))
        };
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        GenericAbility_Container ability = Parent.Ability as GenericAbility_Container;
        if (ability == null)
        {
            return;
        }

        List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
        List<ActorData> hitActors = ability.GetHitActors(
            new List<AbilityTarget>
            {
                AbilityTarget.CreateSimpleAbilityTarget(Target)
            },
            Caster,
            nonActorTargetInfo);

        Dictionary<ActorData, ActorHitContext> actorContext = ability.GetTargetSelectComp().GetActorHitContextMap();
        ContextVars abilityContext = new ContextVars();
        abilityContext.SetValue(
            ScampDelayedAoe.s_cvarMissingShields.GetKey(),
            m_passive.GetMaxSuitShield() - m_syncComp.m_suitShieldingOnTurnStart);
        bool endedSequence = false;
        foreach (ActorData hitActor in hitActors)
        {
            ActorHitResults actorHitResults =
                new ActorHitResults(new ActorHitParameters(hitActor, Target.GetFreePos()));
            GenericAbility_Container.ApplyActorHitData(
                Caster,
                hitActor,
                actorHitResults,
                m_onHitData,
                actorContext[hitActor],
                abilityContext);
            if (!m_syncComp.m_suitWasActiveOnTurnStart)
            {
                actorHitResults.AddBaseDamage(m_extraDamageIfShieldDownForm);
            }

            if (!endedSequence)
            {
                EndAllEffectSequences(actorHitResults);
                endedSequence = true;
            }

            effectResults.StoreActorHit(actorHitResults);
        }

        if (!endedSequence)
        {
            ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
            EndAllEffectSequences(casterHitResults);
            effectResults.StoreActorHit(casterHitResults);
        }

        effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
    }

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return Target != null && HitPhase == phaseIndex;
    }

    public override ActorData GetActorAnimationActor()
    {
        return Target;
    }

    public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
    {
        return HitPhase == phaseIndex ? m_animIndexOnTrigger : 0;
    }
}
#endif