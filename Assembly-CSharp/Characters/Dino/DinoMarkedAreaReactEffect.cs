using System.Collections.Generic;

#if SERVER
// custom
public class DinoMarkedAreaReactEffect : StandardActorEffect
{
    private readonly int m_energyToAllyOnDamageHit;

    public DinoMarkedAreaReactEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData target,
        ActorData caster,
        int energyToAllyOnDamageHit)
        : base(parent, targetSquare, target, caster, StandardActorEffectData.MakeDefault())
    {
        m_energyToAllyOnDamageHit = energyToAllyOnDamageHit;
        m_time.duration = 1;
    }

    public override void GatherResultsInResponseToActorHit(
        ActorHitResults incomingHit,
        ref List<AbilityResults_Reaction> reactions,
        bool isReal)
    {
        if (!incomingHit.HasDamage)
        {
            return;
        }

        ActorHitParameters hitParameters = new ActorHitParameters(
            incomingHit.m_hitParameters.Caster,
            Target.GetFreePos());
        ActorHitResults actorHitResults = new ActorHitResults(hitParameters);
        actorHitResults.TriggeringHit = incomingHit;
        actorHitResults.CanBeReactedTo = false;
        actorHitResults.AddTechPointGain(m_energyToAllyOnDamageHit);

        AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
        abilityResults_Reaction.SetupGameplayData(
            this,
            new List<ActorHitResults> { actorHitResults },
            incomingHit.m_reactionDepth,
            isReal);
        abilityResults_Reaction.SetupSequenceData(
            SequenceLookup.Get().GetSimpleHitSequencePrefab(),
            Target.GetCurrentBoardSquare(),
            SequenceSource);
        abilityResults_Reaction.SetSequenceCaster(Target);
        reactions.Add(abilityResults_Reaction);
    }
}
#endif