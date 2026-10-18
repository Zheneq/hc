// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class TutorialAttack : Ability
{
    public float m_width = 1f;
    public float m_distance = 15f;
    public bool m_penetrateLineOfSight;
    public int m_damageAmount = 10;
    public int m_knockbackDistance = 3;
    public bool m_knockback = true;

    private void Start()
    {
        Targeter = new AbilityUtil_Targeter_Laser(this, m_width, m_distance, m_penetrateLineOfSight);
    }
    
#if SERVER
    // added in rogues
    private List<ActorData> GetHitTargets(
        List<AbilityTarget> targets,
        ActorData caster,
        out Vector3 zoneEndPoint,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        VectorUtils.LaserCoords laserCoords;
        laserCoords.start = caster.GetLoSCheckPos();
        List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
            laserCoords.start,
            targets[0].AimDirection,
            m_distance,
            m_width,
            caster,
            caster.GetOtherTeams(),
            m_penetrateLineOfSight,
            1,
            false,
            true,
            out laserCoords.end,
            nonActorTargetInfo);
        zoneEndPoint = laserCoords.end;
        return actorsInLaser;
    }

    // added in rogues
    public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        GetHitTargets(targets, caster, out Vector3 targetPos, null);
        return new ServerClientUtils.SequenceStartData(
            AsEffectSource().GetSequencePrefab(),
            targetPos,
            additionalData.m_abilityResults.HitActorsArray(),
            caster,
            additionalData.m_sequenceSource);
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
        List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
        foreach (ActorData target in GetHitTargets(targets, caster, out _, nonActorTargetInfo))
        {
            ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
            ActorHitResults actorHitResults = new ActorHitResults(m_damageAmount, HitActionType.Damage, hitParams);
            if (m_knockback)
            {
                KnockbackHitData knockbackData = new KnockbackHitData(
                    target,
                    caster,
                    KnockbackType.ForwardAlongAimDir,
                    targets[0].AimDirection,
                    caster.GetFreePos(),
                    m_knockbackDistance);
                actorHitResults.AddKnockbackData(knockbackData);
            }

            abilityResults.StoreActorHit(actorHitResults);
        }

        abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
    }
#endif
}