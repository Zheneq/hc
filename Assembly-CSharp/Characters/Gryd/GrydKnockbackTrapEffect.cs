// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class GrydKnockbackTrapEffect : ThiefHiddenTrapEffect
{
    private int m_knockbackAmount;
    private Vector3 m_knockbackDir;

    // added in rogues
    public GrydKnockbackTrapEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        Vector3 shapeFreePos,
        ActorData target,
        ActorData caster,
        GroundEffectField fieldInfo,
        int extraDamagePerTurn,
        int maxExtraDamage,
        int knockbackAmount,
        Vector3 knockbackDir)
        : base(
            parent,
            targetSquare,
            shapeFreePos,
            target,
            caster,
            fieldInfo,
            extraDamagePerTurn,
            maxExtraDamage)
    {
        m_knockbackAmount = knockbackAmount;
        m_knockbackDir = knockbackDir;
        HitPhase = AbilityPriority.Combat_Knockback;
    }

    // added in rogues
    public override void SetupActorHitResults(ref ActorHitResults actorHitRes, BoardSquare targetSquare)
    {
        base.SetupActorHitResults(ref actorHitRes, targetSquare);
        if (m_knockbackAmount > 0)
        {
            List<ActorData> affectableActors =
                m_fieldInfo.GetAffectableActorsInField(TargetSquare, m_shapeFreePos, Caster, null);
            foreach (ActorData target in affectableActors)
            {
                actorHitRes.AddKnockbackData(
                    new KnockbackHitData(
                        target,
                        Caster,
                        KnockbackType.ForwardAlongAimDir,
                        m_knockbackDir,
                        m_shapeFreePos,
                        m_knockbackAmount));
            }
        }
    }
}
# endif