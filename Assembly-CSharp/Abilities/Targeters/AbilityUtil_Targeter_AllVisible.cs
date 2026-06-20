using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_AllVisible : AbilityUtil_Targeter
{
    public delegate bool ShouldAddActorDelegate(ActorData potentialActor, ActorData caster);

    public enum DamageOriginType
    {
        CasterPos,
        TargetPos
    }

    public ShouldAddActorDelegate m_shouldAddActorDelegate;

    private DamageOriginType m_damageOriginType;

    public AbilityUtil_Targeter_AllVisible(
        Ability ability,
        bool includeEnemies,
        bool includeAllies,
        bool includeSelf,
        DamageOriginType damageOriginType = DamageOriginType.CasterPos)
        : base(ability)
    {
        m_affectsEnemies = includeEnemies;
        m_affectsAllies = includeAllies;
        m_affectsTargetingActor = includeSelf;
        m_damageOriginType = damageOriginType;
    }

    public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
    {
        ClearActorsInRange();
        if (GameFlowData.Get() == null
            || GameFlowData.Get().activeOwnedActorData == null)
        {
            return;
        }

        List<ActorData> actorsVisibleToActor =
            GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
        foreach (ActorData actorData in actorsVisibleToActor)
        {
            if (actorData.IsDead() || actorData.IgnoreForAbilityHits)
            {
                continue;
            }

            if ((actorData != targetingActor || !m_affectsTargetingActor)
                && (actorData == targetingActor || actorData.GetTeam() != targetingActor.GetTeam() || !m_affectsAllies)
                && (actorData.GetTeam() == targetingActor.GetTeam() || !m_affectsEnemies))
            {
                continue;
            }

            if (m_shouldAddActorDelegate != null && !m_shouldAddActorDelegate(actorData, targetingActor))
            {
                continue;
            }

            Vector3 damageOrigin = m_damageOriginType == DamageOriginType.CasterPos
                ? targetingActor.GetFreePos()
                : actorData.GetFreePos();
            AddActorInRange(actorData, damageOrigin, targetingActor);
        }
    }
}