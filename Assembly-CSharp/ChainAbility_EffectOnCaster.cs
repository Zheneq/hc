// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ChainAbility_EffectOnCaster : Ability
{
	public bool m_applyEffect = true;
	public StandardActorEffectData m_effect;
	[Header("-- Sequences ----------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "CHAIN_ABILITY_EFFECT_ON_CASTER";
		}
		m_sequencePrefab = m_castSequencePrefab;
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetFreePos(),
			new List<ActorData> { caster }.ToArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		if (m_applyEffect)
		{
			actorHitResults.AddEffect(new StandardActorEffect(
				AsEffectSource(), caster.GetCurrentBoardSquare(), caster, caster, m_effect));
		}
		actorHitResults.SetIgnoreTechpointInteractionForHit(true);
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
