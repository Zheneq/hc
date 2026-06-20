// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;

#if SERVER
// custom
// TODO FIREBORG focus on it? wait for client enable doesn't help, only breaks everything
public class FireborgIgnitedEffect : StandardActorEffect
{
    private readonly int m_ignitedTriggerDamage;
    private readonly StandardEffectInfo m_ignitedTriggerEffect;
    private readonly int m_ignitedTriggerEnergyOnCaster;

    public FireborgIgnitedEffect(
        EffectSource parent,
        ActorData target,
        ActorData caster,
        StandardActorEffectData data,
        int ignitedTriggerDamage,
        StandardEffectInfo ignitedTriggerEffect,
        int ignitedTriggerEnergyOnCaster)
        : base(parent, null, target, caster, data)
    {
        m_ignitedTriggerDamage = ignitedTriggerDamage;
        m_ignitedTriggerEffect = ignitedTriggerEffect;
        m_ignitedTriggerEnergyOnCaster = ignitedTriggerEnergyOnCaster;
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        if (m_time.age < 1)
        {
            return;
        }
        
        ActorHitResults actorHitResults = BuildMainTargetHitResults()
                                          ?? new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
        actorHitResults.AddBaseDamage(m_ignitedTriggerDamage);
        actorHitResults.AddStandardEffectInfo(m_ignitedTriggerEffect);
        actorHitResults.AddTechPointGainOnCaster(m_ignitedTriggerEnergyOnCaster);
        actorHitResults.SetIgnoreTechpointInteractionForHit(true);
        EndAllEffectSequences(actorHitResults);
        effectResults.StoreActorHit(actorHitResults);
    }

    public override List<Vector3> CalcPointsOfInterestForCamera()
    {
        return new List<Vector3> { Target.GetFreePos() };
    }
}
#endif