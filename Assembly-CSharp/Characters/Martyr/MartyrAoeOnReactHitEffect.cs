// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// added in rogues
#if SERVER
public class MartyrAoeOnReactHitEffect : StandardActorEffect
{
	private Martyr_SyncComponent m_syncComp;
	private Passive_Martyr m_passive;
	public float m_aoeRadius; // private in rogues
	public bool m_penetrateLos; // private in rogues
	public int m_damageAmount; // private in rogues
	private StandardEffectInfo m_enemyHitEffect;
	private int m_healOnTarget;
	private int m_energyOnCaster;
	private bool m_includeEffectTarget;
	private GameObject m_onTriggerSequencePrefab;
	private bool m_tempTriggeredInGatherResults;
	private bool m_tempTriggeredInGatherResults_fake;
	private bool m_triggeredFinal;
	private bool m_triggeredFinal_fake;
	private bool m_onlyTriggerOncePerTurn;
	private bool m_wasHitThisTurn;
	private bool m_wasHitThisTurn_fake;

	public MartyrAoeOnReactHitEffect(EffectSource parent, BoardSquare targetSquare, ActorData target, ActorData caster, StandardActorEffectData effectData, Martyr_SyncComponent syncComp, Passive_Martyr passive, float aoeRadius, bool penetrateLos, int damageAmount, StandardEffectInfo enemyHitEffect, int healOnTarget, int energyOnCaster, bool includeEffectTarget, bool onlyTriggerOncePerTurn, GameObject onTriggerSequencePrefab)
		: base(parent, targetSquare, target, caster, effectData)
	{
		m_syncComp = syncComp;
		m_passive = passive;
		m_aoeRadius = aoeRadius;
		m_penetrateLos = penetrateLos;
		m_damageAmount = damageAmount;
		m_enemyHitEffect = enemyHitEffect;
		m_healOnTarget = healOnTarget;
		m_energyOnCaster = energyOnCaster;
		m_includeEffectTarget = includeEffectTarget;
		m_onlyTriggerOncePerTurn = onlyTriggerOncePerTurn;
		m_onTriggerSequencePrefab = onTriggerSequencePrefab;
	}

	public override void OnStart()
	{
		base.OnStart();
		if (m_syncComp != null)
		{
			m_syncComp.AddAoeOnReactActor(Target);
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_syncComp != null)
		{
			m_syncComp.RemoveAoeOnReactActor(Target);
		}
		if (m_passive != null)
		{
			m_passive.m_aoeReactEndedWithoutTriggering |= !m_triggeredFinal;
		}
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_wasHitThisTurn = false;
		m_wasHitThisTurn_fake = false;
	}

	public override void OnAbilityPhaseStart(AbilityPriority phase)
	{
		base.OnAbilityPhaseStart(phase);
		if (phase == AbilityPriority.Prep_Defense)
		{
			m_tempTriggeredInGatherResults = false;
			m_wasHitThisTurn = false;
			m_wasHitThisTurn_fake = false;
		}
		if (phase == AbilityPriority.Combat_Final)
		{
			m_triggeredFinal |= m_tempTriggeredInGatherResults;
			m_triggeredFinal_fake |= m_tempTriggeredInGatherResults_fake;
		}
	}

	public override void GatherResultsInResponseToActorHit(ActorHitResults incomingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (incomingHit.HasDamage && Target != null && Target.GetCurrentBoardSquare() != null)
		{
			if (m_onlyTriggerOncePerTurn && GetWasHitThisTurn(isReal))
			{
				return;
			}
			SetWasHitThisTurn(true, isReal);
			List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(Target.GetFreePos(), m_aoeRadius, m_penetrateLos, Caster, Caster.GetOtherTeams(), null);
			if (!m_includeEffectTarget && actorsInRadius.Contains(Target))
			{
				actorsInRadius.Remove(Target);
			}
			SetTempTriggeredInGatherResults(true, isReal);
			List<ActorHitResults> list = new List<ActorHitResults>();
			AbilityResults_Reaction abilityResults_Reaction = new AbilityResults_Reaction();
			foreach (ActorData target in actorsInRadius)
			{
				ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, Target.GetFreePos()));
				actorHitResults.SetBaseDamage(m_damageAmount);
				actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
				actorHitResults.SetTechPointGainOnCaster(m_energyOnCaster);
				list.Add(actorHitResults);
			}
			bool flag = m_time.duration > 0 && m_time.age >= m_time.duration - 1;
			if (m_healOnTarget > 0 || flag)
			{
				ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
				actorHitResults2.SetBaseHealing(m_healOnTarget);
				if (flag && m_data.m_sequencePrefabs != null && m_data.m_sequencePrefabs.Length != 0)
				{
					for (int i = 0; i < m_data.m_sequencePrefabs.Length; i++)
					{
						actorHitResults2.AddEffectSequenceToEnd(m_data.m_sequencePrefabs[i], m_guid);
					}
				}
				list.Add(actorHitResults2);
			}
			abilityResults_Reaction.SetupGameplayData(this, list, incomingHit.m_reactionDepth, isReal);
			abilityResults_Reaction.SetupSequenceData(m_onTriggerSequencePrefab, Target.GetCurrentBoardSquare(), SequenceSource, null);
			abilityResults_Reaction.SetSequenceCaster(Target);
			if (m_onlyTriggerOncePerTurn)
			{
				abilityResults_Reaction.SetExtraFlag(ClientReactionResults.ExtraFlags.ClientExecuteOnFirstDamagingHit);
			}
			reactions.Add(abilityResults_Reaction);
		}
	}

	private bool GetWasHitThisTurn(bool isReal)
	{
		if (isReal)
		{
			return m_wasHitThisTurn;
		}
		return m_wasHitThisTurn_fake;
	}

	private void SetWasHitThisTurn(bool wasHitThisTurn, bool isReal)
	{
		if (isReal)
		{
			m_wasHitThisTurn = wasHitThisTurn;
			return;
		}
		m_wasHitThisTurn_fake = wasHitThisTurn;
	}

	private bool GetTempTriggeredInGatherResults(bool isReal)
	{
		if (isReal)
		{
			return m_tempTriggeredInGatherResults;
		}
		return m_tempTriggeredInGatherResults_fake;
	}

	private void SetTempTriggeredInGatherResults(bool tempTriggeredInGatherResults, bool isReal)
	{
		if (isReal)
		{
			m_tempTriggeredInGatherResults = tempTriggeredInGatherResults;
			return;
		}
		m_tempTriggeredInGatherResults_fake = tempTriggeredInGatherResults;
	}

	private bool GetTriggeredFinal(bool isReal)
	{
		if (isReal)
		{
			return m_triggeredFinal;
		}
		return m_triggeredFinal_fake;
	}

	private void SetTriggeredFinal(bool triggeredFinal, bool isReal)
	{
		if (isReal)
		{
			m_triggeredFinal = triggeredFinal;
			return;
		}
		m_triggeredFinal_fake = triggeredFinal;
	}
}
#endif
