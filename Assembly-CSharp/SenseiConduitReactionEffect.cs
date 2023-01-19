// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class SenseiConduitReactionEffect : StandardActorEffect
{
	private Ability m_parentAbility;
	private GameObject m_reactionSequencePrefab;
	private int m_reactionHealing;
	private StandardEffectInfo m_reactionEffect;

	public SenseiConduitReactionEffect(
		EffectSource parent,
		ActorData target,
		ActorData caster,
		StandardActorEffectData baseEffectData,
		int reactionHealing,
		StandardEffectInfo reactionEffect,
		GameObject reactionProjectileSequencePrefab)
		: base(parent, target.GetCurrentBoardSquare(), target, caster, baseEffectData)
	{
		m_parentAbility = parent.Ability;
		m_effectName = m_parentAbility.m_abilityName;
		m_reactionHealing = reactionHealing;
		m_reactionEffect = reactionEffect;
		m_reactionSequencePrefab = reactionProjectileSequencePrefab;
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (incomingHit.HasDamage)
		{
			AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
			ActorData caster = incomingHit.m_hitParameters.Caster;
			if (!caster.IsDead())
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, incomingHit.m_hitParameters.Origin));
				actorHitResults.SetBaseHealing(m_reactionHealing);
				actorHitResults.AddStandardEffectInfo(m_reactionEffect);
				abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
				abilityResults_Reaction.SetupSequenceData(m_reactionSequencePrefab, caster.GetCurrentBoardSquare(), SequenceSource);
				reactions.Add(abilityResults_Reaction);
			}
		}
	}
}
#endif
