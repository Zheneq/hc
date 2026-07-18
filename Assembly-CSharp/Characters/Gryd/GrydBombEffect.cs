// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class GrydBombEffect : Effect
{
    private int m_damageAmount;
    private float m_explosionLaserRange;
    private float m_explosionLaserWidth;
    private bool m_explodeNow;
    private GameObject m_persistentBombSequencePrefab;
    private GameObject m_explodeSequencePrefab;
    private int m_cooldownAfterExplode;
    private Gryd_SyncComponent m_syncComp;

    // added in rogues
    public GrydBombEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData caster,
        int damageAmount,
        float explosionLaserRange,
        float explosionLaserWidth,
        bool explodeFirstTurn,
        int duration,
        GameObject persistentBombSequence,
        GameObject explodeSequence,
        int cooldownAfterExplode)
        : base(parent, targetSquare, null, caster)
    {
        m_damageAmount = damageAmount;
        m_explosionLaserRange = explosionLaserRange;
        m_explosionLaserWidth = explosionLaserWidth;
        m_explodeNow = explodeFirstTurn;
        m_persistentBombSequencePrefab = persistentBombSequence;
        m_explodeSequencePrefab = explodeSequence;
        m_cooldownAfterExplode = cooldownAfterExplode;
        HitPhase = AbilityPriority.Combat_Damage;
        m_time.age = 0;
        m_time.duration = duration;
        m_syncComp = caster.GetComponent<Gryd_SyncComponent>();
    }

    // added in rogues
    public void Detonate()
    {
        m_explodeNow = true;
    }

    // added in rogues
    public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
    {
        return new ServerClientUtils.SequenceStartData(
            m_persistentBombSequencePrefab,
            TargetSquare,
            null,
            Caster,
            SequenceSource);
    }

    // added in rogues
    public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
    {
        if (phaseIndex == HitPhase
            && m_explodeNow
            && Parent.Ability is GrydPlaceOrMoveBomb ability)
        {
            return (int)ability.m_moveBombAnimIndex;
        }

        return base.GetCasterAnimationIndex(phaseIndex);
    }

    // added in rogues
    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        if (!m_explodeNow)
        {
            return new List<ServerClientUtils.SequenceStartData>();
        }

        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_explodeSequencePrefab,
                TargetSquare.ToVector3(),
                Quaternion.LookRotation(new Vector3(m_explosionLaserRange, 0f, 0f)),
                GetHitActors(null).ToArray(),
                Caster,
                SequenceSource),
            new ServerClientUtils.SequenceStartData(
                m_explodeSequencePrefab,
                TargetSquare.ToVector3(),
                Quaternion.LookRotation(new Vector3(-m_explosionLaserRange, 0f, 0f)),
                new ActorData[0],
                Caster,
                SequenceSource),
            new ServerClientUtils.SequenceStartData(
                m_explodeSequencePrefab,
                TargetSquare.ToVector3(),
                Quaternion.LookRotation(new Vector3(0f, 0f, m_explosionLaserRange)),
                new ActorData[0],
                Caster,
                SequenceSource),
            new ServerClientUtils.SequenceStartData(
                m_explodeSequencePrefab,
                TargetSquare.ToVector3(),
                Quaternion.LookRotation(new Vector3(0f, 0f, -m_explosionLaserRange)),
                new ActorData[0],
                Caster,
                SequenceSource)
        };
    }

    // added in rogues
    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return m_explodeNow;
    }

    // added in rogues
    public override void OnAbilityPhaseEnd(AbilityPriority phase)
    {
        base.OnAbilityPhaseEnd(phase);
        if (phase == AbilityPriority.Prep_Defense
            && m_time.age == m_time.duration - 1
            && !Caster.GetAbilityData().HasQueuedAbilityOfType(typeof(GrydPlaceOrMoveBomb))) // , true in rogues
        {
            Detonate();
        }
    }

    // added in rogues
    public override void OnEnd()
    {
        base.OnEnd();
        if (m_syncComp != null)
        {
            m_syncComp.m_bombLocation = GridPos.s_invalid;
        }
    }

    // added in rogues
    public override List<Vector3> CalcPointsOfInterestForCamera()
    {
        List<Vector3> list = new List<Vector3>();
        foreach (ActorData actorData in GetHitActors(null))
        {
            list.Add(actorData.GetFreePos());
        }

        return list;
    }

    // added in rogues
    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        if (!m_explodeNow)
        {
            return;
        }

        List<NonActorTargetInfo> nonActorTargetInfos = new List<NonActorTargetInfo>();
        foreach (ActorData actorData in GetHitActors(nonActorTargetInfos))
        {
            if (actorData == Caster)
            {
                continue;
            }

            ActorHitResults actorHitResults =
                new ActorHitResults(new ActorHitParameters(actorData, TargetSquare.ToVector3()));
            actorHitResults.AddBaseDamage(m_damageAmount);
            effectResults.StoreActorHit(actorHitResults);
        }

        ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
        casterHitResults.AddMiscHitEvent(
            new MiscHitEventData_AddToCasterCooldown(
                Caster.GetAbilityData().GetActionTypeOfAbility(Parent.Ability),
                m_cooldownAfterExplode)
            {
                m_ignoreCooldownMax = true
            });
        effectResults.StoreActorHit(casterHitResults);
        effectResults.StoreNonActorTargetInfo(nonActorTargetInfos);
        if (isReal)
        {
            m_time.age = m_time.duration;
        }
    }

    // added in rogues
    private List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargets)
    {
        Vector3 targetPos = Caster.GetLoSCheckPos(TargetSquare);
        List<ActorData> result;
        if (m_explosionLaserRange <= 0f)
        {
            result = new List<ActorData>();
            if (TargetSquare.OccupantActor != null
                && TargetSquare.OccupantActor.GetTeam() != Caster.GetTeam())
            {
                result.Add(TargetSquare.OccupantActor);
            }
        }
        else
        {
            List<Team> otherTeams = Caster.GetOtherTeams();
            result = AreaEffectUtils.GetActorsInLaser(
                targetPos,
                new Vector3(1f, 0f, 0f),
                m_explosionLaserRange,
                m_explosionLaserWidth,
                Caster,
                otherTeams,
                false,
                0,
                false,
                true,
                out _,
                nonActorTargets);
            result.AddRange(
                AreaEffectUtils.GetActorsInLaser(
                    targetPos,
                    new Vector3(-1f, 0f, 0f),
                    m_explosionLaserRange,
                    m_explosionLaserWidth,
                    Caster,
                    otherTeams,
                    false,
                    0,
                    false,
                    true,
                    out _,
                    nonActorTargets,
                    result));
            result.AddRange(
                AreaEffectUtils.GetActorsInLaser(
                    targetPos,
                    new Vector3(0f, 0f, 1f),
                    m_explosionLaserRange,
                    m_explosionLaserWidth,
                    Caster,
                    otherTeams,
                    false,
                    0,
                    false,
                    true,
                    out _,
                    nonActorTargets,
                    result));
            result.AddRange(
                AreaEffectUtils.GetActorsInLaser(
                    targetPos,
                    new Vector3(0f, 0f, -1f),
                    m_explosionLaserRange,
                    m_explosionLaserWidth,
                    Caster,
                    otherTeams,
                    false,
                    0,
                    false,
                    true,
                    out _,
                    nonActorTargets,
                    result));
        }

        result.Add(Caster);
        return result;
    }
}
#endif