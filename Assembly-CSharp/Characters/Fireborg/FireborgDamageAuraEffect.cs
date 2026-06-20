// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// custom
public class FireborgDamageAuraEffect : Effect
{
    private readonly AbilityAreaShape m_shape;
    private readonly OnHitAuthoredData m_onHitData;
    private readonly bool m_excludeTargetedActor;
    private readonly bool m_ignite;
    private readonly GameObject m_auraPersistentSeqPrefab;
    private readonly GameObject m_auraOnTriggerSeqPrefab;

    private readonly Fireborg_SyncComponent m_syncComp;

    public FireborgDamageAuraEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData target,
        ActorData caster,
        AbilityAreaShape shape,
        OnHitAuthoredData onHitData,
        int duration,
        bool excludeTargetedActor,
        bool ignite,
        GameObject auraPersistentSeqPrefab,
        GameObject auraOnTriggerSeqPrefab,
        Fireborg_SyncComponent syncComp)
        : base(parent, targetSquare, target, caster)
    {
        m_shape = shape;
        m_onHitData = onHitData;
        m_excludeTargetedActor = excludeTargetedActor;
        m_ignite = ignite;
        m_auraPersistentSeqPrefab = auraPersistentSeqPrefab;
        m_auraOnTriggerSeqPrefab = auraOnTriggerSeqPrefab;
        m_syncComp = syncComp;
        m_time.duration = duration;
        HitPhase = AbilityPriority.Combat_Damage;
    }

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return phaseIndex == HitPhase;
    }

    public override ActorData GetActorAnimationActor()
    {
        return !Target.IsDead() ? Target : base.GetActorAnimationActor();
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
    {
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_auraPersistentSeqPrefab,
                TargetSquare,
                Target.AsArray(),
                Caster,
                SequenceSource)
        };
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        SequenceSource sequenceSource = SequenceSource.GetShallowCopy();
        sequenceSource.SetWaitForClientEnable(true);
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_auraOnTriggerSeqPrefab,
                Target.GetCurrentBoardSquare(),
                m_effectResults.HitActorsArray(),
                Target,
                sequenceSource)
        };
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        bool needToEndSequence = m_time.age + 1 == m_time.duration;
        
        List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
        foreach (ActorData hitActor in GetHitActors(nonActorTargetInfo))
        {
            ActorHitParameters hitParameters = new ActorHitParameters(hitActor, Target.GetFreePos());
            ActorHitResults actorHitResults = new ActorHitResults(hitParameters);
            GenericAbility_Container.ApplyActorHitData(Caster, hitActor, actorHitResults, m_onHitData);
            if (m_ignite)
            {
                FireborgIgnitedEffect ignitedEffect = m_syncComp.MakeIgnitedEffect(Parent, Caster, hitActor);
                if (ignitedEffect != null)
                {
                    actorHitResults.AddEffect(ignitedEffect);
                }
            }
            if (needToEndSequence)
            {
                actorHitResults.AddEffectSequenceToEnd(m_auraPersistentSeqPrefab, m_guid);
                needToEndSequence = false;
            }

            effectResults.StoreActorHit(actorHitResults);
        }
        effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);

        if (needToEndSequence)
        {
            ActorHitParameters hitParameters = new ActorHitParameters(Target, Target.GetFreePos());
            ActorHitResults actorHitResults = new ActorHitResults(hitParameters);
            actorHitResults.AddEffectSequenceToEnd(m_auraPersistentSeqPrefab, m_guid);
            effectResults.StoreActorHit(actorHitResults);
        }
    }

    public List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargetInfo = null)
    {
        List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
            m_shape,
            Target.GetFreePos(),
            Target.GetCurrentBoardSquare(),
            false,
            Caster,
            Caster.GetOtherTeams(),
            nonActorTargetInfo);

        if (m_excludeTargetedActor)
        {
            actorsInShape.Remove(Target);
        }

        return actorsInShape;
    }

    public override List<Vector3> CalcPointsOfInterestForCamera()
    {
        List<Vector3> list = new List<Vector3>();
        if (Target != null && Target.GetCurrentBoardSquare() != null)
        {
            list.Add(Target.GetFreePos());
        }

        ActorData[] array = m_effectResults.HitActorsArray();
        if (array.Length != 0)
        {
            list.Add(array[0].GetFreePos());
        }

        return list;
    }

    public int GetBaseDamage()
    {
        return m_onHitData.GetFirstDamageValue();
    }
}
#endif