// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class GrydDetonateBomb : Ability
{
    [Header("-- Sequences")]
    public GameObject m_castSequencePrefab;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Detonate";
        }
    }
#if SERVER
    // added in rogues
    public override void Run(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        foreach (Effect effect in ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(GrydBombEffect)))
        {
            (effect as GrydBombEffect).Detonate();
        }
    }

    // added in rogues
    public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        return new ServerClientUtils.SequenceStartData(
            m_castSequencePrefab,
            caster.GetCurrentBoardSquare(),
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
        base.GatherAbilityResults(targets, caster, ref abilityResults);
    }
#endif
}