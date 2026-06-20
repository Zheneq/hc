using System.Collections.Generic;
using UnityEngine;

#if SERVER
// custom
// In the original, there was a resolution message outside resolve on resolution start,
// with a simple hit animation and zero actor hit results on caster
// (regardless of whether the ability was activated again or not)
public class DinoDashOrShieldEffect : StandardActorEffect
{
    private readonly StandardEffectInfo m_delayedShieldInfo;
    private readonly int m_healIfNoDash;
    private readonly ActorModelData.ActionAnimationType m_noDashShieldAnimIndex;
    private readonly AbilityData.ActionType m_abilityActionType;
    private readonly int m_abilityCooldown;
    private readonly GameObject m_onTriggerSequencePrefab;
    private readonly Passive_Dino m_passive;

    public DinoDashOrShieldEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData target,
        ActorData caster,
        StandardEffectInfo delayedShieldInfo,
        int healIfNoDash,
        ActorModelData.ActionAnimationType noDashShieldAnimIndex,
        AbilityData.ActionType abilityActionType,
        int abilityCooldown,
        GameObject onTriggerSequencePrefab,
        Passive_Dino passive)
        : base(parent, targetSquare, target, caster, StandardActorEffectData.MakeDefault())
    {
        m_delayedShieldInfo = delayedShieldInfo;
        m_healIfNoDash = healIfNoDash;
        m_noDashShieldAnimIndex = noDashShieldAnimIndex;
        m_abilityActionType = abilityActionType;
        m_abilityCooldown = abilityCooldown;
        m_onTriggerSequencePrefab = onTriggerSequencePrefab;
        m_passive = passive;

        HitPhase = AbilityPriority.Prep_Defense;
        m_time.duration = 2;
    }

    public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
    {
        return phaseIndex == HitPhase
               && !Caster.IsDead()
               && ApplyEffect()
            ? (int)m_noDashShieldAnimIndex
            : 0;
    }

    private bool ApplyEffect()
    {
        return !ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(DinoDashOrShield));
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
        if (ApplyEffect())
        {
            SequenceSource source = SequenceSource.GetShallowCopy();
            source.SetWaitForClientEnable(true);
            list.Add(
                new ServerClientUtils.SequenceStartData(
                    m_onTriggerSequencePrefab,
                    TargetSquare,
                    Target.AsArray(),
                    Caster,
                    source));
        }

        return list;
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        if (!ApplyEffect())
        {
            return;
        }

        ActorHitResults hitRes = new ActorHitResults(new ActorHitParameters(Target, Caster.GetFreePos()));
        hitRes.AddStandardEffectInfo(m_delayedShieldInfo);
        hitRes.AddBaseHealing(m_healIfNoDash);
        hitRes.AddMiscHitEvent(
            new MiscHitEventData_OverrideCooldown(
                m_abilityActionType,
                m_abilityCooldown + m_passive.m_dashCooldownAdjust));
        effectResults.StoreActorHit(hitRes);
    }

    public override void OnEnd()
    {
        base.OnEnd();

        m_passive.m_dashCooldownAdjust = 0;
    }
}
#endif