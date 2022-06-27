// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class BattleMonkThornsEffect : StandardActorEffect
{
	private Ability m_parentAbility;
	private GameObject m_reactionSequencePrefab;
	private int m_reactionDamage;
	private StandardEffectInfo m_reactionEffect;
	private List<ActorData> m_actorsReactedToThisTurn = new List<ActorData>();
	private List<ActorData> m_actorsReactedToThisTurnFake = new List<ActorData>();

	public BattleMonkThornsEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData baseEffectData, int reactionDamage, StandardEffectInfo reactionEffect, GameObject reactionProjectileSequencePrefab) : base(parent, targetSquare, target, caster, baseEffectData)
	{
		m_parentAbility = parent.Ability;
		m_effectName = m_parentAbility.m_abilityName;
		m_reactionDamage = reactionDamage;
		m_reactionEffect = reactionEffect;
		m_reactionSequencePrefab = reactionProjectileSequencePrefab;
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_actorsReactedToThisTurn.Clear();
		m_actorsReactedToThisTurnFake.Clear();
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		base.OnAbilityPhaseStart(phase);
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_actorsReactedToThisTurn.Clear();
			m_actorsReactedToThisTurnFake.Clear();
		}
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (incomingHit.HasDamage)
		{
			AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
			ActorData caster = incomingHit.m_hitParameters.Caster;
			if (!caster.IsDead())
			{
				bool isGatheringResults = true;
				if (isReal)
				{
					if (m_actorsReactedToThisTurn.Contains(caster))
					{
						isGatheringResults = false;
					}
					else
					{
						m_actorsReactedToThisTurn.Add(caster);
					}
				}
				else if (m_actorsReactedToThisTurnFake.Contains(caster))
				{
					isGatheringResults = false;
				}
				else
				{
					m_actorsReactedToThisTurnFake.Add(caster);
				}
				if (isGatheringResults)
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, incomingHit.m_hitParameters.Origin));
					actorHitResults.SetBaseDamage(m_reactionDamage);
					actorHitResults.AddStandardEffectInfo(m_reactionEffect);
					abilityResults_Reaction.SetupGameplayData(this, actorHitResults, incomingHit.m_reactionDepth, null, isReal, incomingHit);
					abilityResults_Reaction.SetupSequenceData(m_reactionSequencePrefab, caster.GetCurrentBoardSquare(), SequenceSource, null);
					abilityResults_Reaction.SetExtraFlag(ClientReactionResults.ExtraFlags.TriggerOnFirstDamageIfReactOnAttacker);
					reactions.Add(abilityResults_Reaction);
				}
			}
		}
	}
}
#endif
