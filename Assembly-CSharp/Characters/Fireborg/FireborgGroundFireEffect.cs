// ROGUES
// SERVER
using System.Collections.Generic;

// TODO FIREBORG It used to add a hit animation with null prefab which doesn't play anyway
#if SERVER
// custom
public class FireborgGroundFireEffect : StandardMultiAreaGroundEffect
{
    private readonly Fireborg_SyncComponent m_syncComp;
    
    public FireborgGroundFireEffect(
        EffectSource parent,
        List<GroundAreaInfo> areaInfoList,
        ActorData caster,
        GroundEffectField fieldInfo)
        : base(parent, areaInfoList, caster, fieldInfo)
    {
        m_syncComp = parent.Ability.GetComponent<Fireborg_SyncComponent>();
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        
        foreach (ActorData actorData in GetActorsInShape())
        {
            uint actorIndex = (uint)actorData.ActorIndex;
            if (!m_syncComp.m_actorsInGroundFireOnTurnStart.Contains(actorIndex))
            {
                m_syncComp.m_actorsInGroundFireOnTurnStart.Add(actorIndex);
            }
        }
    }

    protected override void ProcessActorHit(ActorHitResults actorHitResults, bool isReal)
    {
        ActorData hitActor = actorHitResults.m_hitParameters.Target;
        actorHitResults.SetIgnoreTechpointInteractionForHit(true);
        if (hitActor.GetTeam() != Caster.GetTeam() && m_syncComp.GroundFireAddsIgnite)
        {
            FireborgIgnitedEffect fireborgIgnitedEffect = m_syncComp.MakeIgnitedEffect(Parent, Caster, hitActor);
            if (fireborgIgnitedEffect != null)
            {
                actorHitResults.AddEffect(fireborgIgnitedEffect);
            }
        }
        
        // even if we are replacing the old effect, we still need to make sure it doesn't hit once again
        foreach (Effect effect in ServerEffectManager.Get().GetWorldEffectsByCaster(Caster, typeof(FireborgGroundFireEffect)))
        {
            if (effect is FireborgGroundFireEffect fireborgGroundFireEffect)
            {
                fireborgGroundFireEffect.AddActorHitThisTurn(hitActor, isReal);
            }
        }
        m_syncComp.GetActorsHitByGroundFireThisTurn(isReal).Add(hitActor);
    }
}
#endif