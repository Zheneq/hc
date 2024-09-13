// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;

#if SERVER
// custom
public class IceborgDamageAreaEffect : StandardGroundEffect
{
    private int m_damageChangePerTurn;
    private int m_extraDamageOnInitialCast;
    private int m_minDamage;
    private StandardEffectInfo m_effectOnEnemyIfHitPreviousTurn;
    private bool m_applyNovaCoreIfHitPreviousTurn;

    private bool m_doneHitting;
    
    private Iceborg_SyncComponent m_syncComp;

    public IceborgDamageAreaEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        Vector3 shapeFreePos,
        ActorData caster,
        List<ActorData> alreadyHitActors,
        GroundEffectField fieldInfo,
        int damageChangePerTurn,
        int extraDamageOnInitialCast,
        int minDamage,
        StandardEffectInfo effectOnEnemyIfHitPreviousTurn,
        bool applyNovaCoreIfHitPreviousTurn)
        : base(parent, targetSquare, shapeFreePos, null, caster, fieldInfo)
    {
        m_damageChangePerTurn = damageChangePerTurn;
        m_extraDamageOnInitialCast = extraDamageOnInitialCast;
        m_minDamage = minDamage;
        m_effectOnEnemyIfHitPreviousTurn = effectOnEnemyIfHitPreviousTurn;
        m_applyNovaCoreIfHitPreviousTurn = applyNovaCoreIfHitPreviousTurn;
        m_syncComp = parent.Ability.GetComponent<Iceborg_SyncComponent>();
        AddToActorsHitThisTurn(alreadyHitActors);
    }

    public override void OnBeforeGatherEffectResults(AbilityPriority phase)
    {
        if (phase == HitPhase 
            && ServerActionBuffer.Get() != null
            && ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(IceborgDamageArea)))
        {
            m_doneHitting = true;
        }
    }

    public override bool ShouldHitThisTurn()
    {
        return base.ShouldHitThisTurn() && !m_doneHitting;
    }

    public override void SetupActorHitResults(ref ActorHitResults actorHitRes, BoardSquare targetSquare)
    {
        base.SetupActorHitResults(ref actorHitRes, targetSquare);

        ActorData hitActor = actorHitRes.m_hitParameters.Target;
        if (hitActor.GetTeam() == Caster.GetTeam())
        {
            return;
        }
        
        if(m_syncComp.GetTurnsSinceInitialCast() == 0)
        {
            actorHitRes.AddBaseDamage(m_extraDamageOnInitialCast);
        }
        else
        {            
            actorHitRes.AddBaseDamage(m_damageChangePerTurn * m_syncComp.GetTurnsSinceInitialCast());
            if (actorHitRes.BaseDamage < m_minDamage)
            {
                actorHitRes.SetBaseDamage(m_minDamage);
            }

            if (m_syncComp.m_actorsHitByDamageAreaOnPrevTurn.Contains(hitActor))
            {
                actorHitRes.AddStandardEffectInfo(m_effectOnEnemyIfHitPreviousTurn);
            }
            if (m_applyNovaCoreIfHitPreviousTurn)
            {
                actorHitRes.AddEffect(
                    m_syncComp.CreateNovaCoreEffect(
                        Parent,
                        hitActor.GetCurrentBoardSquare(),
                        hitActor,
                        Caster));
            }
        }
    }
    
    public override void OnTurnStart()
    {
        m_syncComp.m_actorsHitByDamageAreaOnPrevTurn = new HashSet<ActorData>(m_actorsHitThisTurn);
        base.OnTurnStart();
        m_syncComp.Networkm_damageAreaCanMoveThisTurn = true;
        // m_shapeFreePos = m_syncComp.m_damageAreaFreePos;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        m_syncComp.m_actorsHitByDamageAreaOnPrevTurn = new HashSet<ActorData>();
    }
}
#endif
