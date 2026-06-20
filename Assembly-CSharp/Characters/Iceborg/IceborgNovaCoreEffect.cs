using System.Collections.Generic;

#if SERVER
// custom
public class IceborgNovaCoreEffect : StandardActorEffect
{
    private Iceborg_SyncComponent m_syncComp;
    private bool m_canExplodeThisTurn;
    private bool m_wasHitThisTurn;
    private bool m_wasHitThisTurn_fake;
    private int m_extraEnergyPerExplosion;

    public IceborgNovaCoreEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData target,
        ActorData caster,
        StandardActorEffectData effectData)
        : base(parent, targetSquare, target, caster, effectData)
    {
        m_effectName = "Iceborg Nova Core Target Effect";
        m_syncComp = parent.Ability.GetComponent<Iceborg_SyncComponent>();
        m_canExplodeThisTurn = false; // to trigger the next turn
        m_time.duration = effectData.m_duration;
        ActorData actorData = caster.GetComponent<ActorData>();
        if (actorData != null && actorData.GetAbilityData() != null)
        {
            IceborgNovaOnReact novaOnReactAbility = actorData.GetAbilityData().GetAbilityOfType(typeof(IceborgNovaOnReact)) as IceborgNovaOnReact;
            m_extraEnergyPerExplosion = novaOnReactAbility?.GetExtraEnergyPerNovaCoreTrigger() ?? 0;
        }
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        m_canExplodeThisTurn = true;
        m_wasHitThisTurn = false;
        m_wasHitThisTurn_fake = false;
        if (m_syncComp != null)
        {
            m_syncComp.AddNovaCoreActorIndex(TargetActorIndex);
        }
    }

    public override bool ShouldForceReactToHit(ActorHitResults incomingHit)
    {
        return m_syncComp.m_delayedAoeCanReactToIndirectHits
               || incomingHit.m_hitParameters.Effect is FireborgIgnitedEffect;
    }

    public override void GatherResultsInResponseToActorHit(
        ActorHitResults incomingHit,
        ref List<AbilityResults_Reaction> reactions,
        bool isReal)
    {
        if (!incomingHit.HasDamage
            || !m_canExplodeThisTurn
            || GetWasHitThisTurn(isReal)
            || Target == null 
            || Target.GetCurrentBoardSquare() == null
            || !m_syncComp.m_delayedAoeTriggerOnReact) // for unused manual trigger ability
        {
            return;
        }
        
        SetWasHitThisTurn(true, isReal);
        
        List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
            Target.GetFreePos(),
            m_syncComp.m_delayedAoeRadius,
            false,
            Caster,
            Caster.GetOtherTeams(),
            null);
        
        int energyPerExplosion = m_syncComp.m_delayedAoeEnergyPerExplosion + m_extraEnergyPerExplosion;
        
        List<ActorHitResults> actorHitResultList = new List<ActorHitResults>(actorsInRadius.Count);
        foreach (ActorData hitActor in actorsInRadius)
        {
            ActorHitParameters hitParameters = new ActorHitParameters(hitActor, Target.GetFreePos());
            ActorHitResults actorHitResults = new ActorHitResults(hitParameters);
            actorHitResults.TriggeringHit = incomingHit;
            GenericAbility_Container.ApplyActorHitData(Caster, hitActor, actorHitResults, m_syncComp.m_delayedAoeOnHitData);
            actorHitResults.SetIgnoreTechpointInteractionForHit(true);
            actorHitResults.AddTechPointGainOnCaster(m_syncComp.m_delayedAoeEnergyPerEnemyHit + energyPerExplosion);
            energyPerExplosion = 0;
            actorHitResults.CanBeReactedTo = false;
            if (hitActor == Target)
            {
                EndAllEffectSequences(actorHitResults);
            }
            actorHitResultList.Add(actorHitResults);
        }
        
        AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
        abilityResults_Reaction.SetupGameplayData(
            this,
            actorHitResultList,
            incomingHit.m_reactionDepth,
            isReal);
        abilityResults_Reaction.SetupSequenceData(
            m_syncComp.m_delayedAoeTriggerSeqPrefab,
            Target.GetCurrentBoardSquare(),
            SequenceSource,
            new Sequence.IExtraSequenceParams[]
            {
                new Sequence.FxAttributeParam
                {
                    m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.ScaleControl,
                    m_paramTarget = Sequence.FxAttributeParam.ParamTarget.MainVfx,
                    m_paramValue = 2.8f
                },
                new Sequence.FxAttributeParam
                {
                    m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.ScaleControl,
                    m_paramTarget = Sequence.FxAttributeParam.ParamTarget.ImpactVfx,
                    m_paramValue = 2.8f
                }
            }
            );
        abilityResults_Reaction.SetSequenceCaster(Target); // so that hit effects plays on target, not on Iceborg
        abilityResults_Reaction.SetExtraFlag(ClientReactionResults.ExtraFlags.ClientExecuteOnFirstDamagingHit);
        reactions.Add(abilityResults_Reaction);
    }

    private bool GetWasHitThisTurn(bool isReal)
    {
        return isReal
            ? m_wasHitThisTurn
            : m_wasHitThisTurn_fake;
    }

    private void SetWasHitThisTurn(bool wasHitThisTurn, bool isReal)
    {
        if (isReal)
        {
            m_wasHitThisTurn = wasHitThisTurn;
        }
        else
        {
            m_wasHitThisTurn_fake = wasHitThisTurn;
        }
    }

    public override void OnTurnEnd()
    {
        base.OnTurnEnd();

        if (GetWasHitThisTurn(true))
        {
            Caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.IceborgStats.NumCoresTriggered);
        }
    }
}
#endif