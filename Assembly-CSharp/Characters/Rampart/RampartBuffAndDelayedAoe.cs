// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RampartBuffAndDelayedAoe : Ability
{
	[Header("-- For Self Buff on Cast")]
	public StandardEffectInfo m_selfBuffEffect;
	[Header("-- For Delayed Aoe")]
	public bool m_onlyDoAoeIfFullEnergy;
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Five_x_Five_NoCorners;
	public bool m_penetrateLos;
	public int m_aoeDelayTurns = 1;
	public int m_aoeDamageAmount = 10;
	public StandardEffectInfo m_aoeEnemyHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_aoeMarkerSequencePrefab;
	public GameObject m_aoeDetonateSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Buff and Delayed Aoe";
		}
		m_sequencePrefab = m_castSequencePrefab;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityAreaShape shapeLowEnergy = m_onlyDoAoeIfFullEnergy ? AbilityAreaShape.SingleSquare : m_aoeShape;
		Targeter = new AbilityUtil_Targeter_RampartDelayedAoe(this, shapeLowEnergy, m_aoeShape, m_penetrateLos, m_aoeDelayTurns <= 0, false, m_selfBuffEffect.m_applyEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_selfBuffEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_aoeDamageAmount);
		m_aoeEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(m_castSequencePrefab, targets[0].FreePos, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
		actorHitResults.AddStandardEffectInfo(m_selfBuffEffect);
		if (!m_onlyDoAoeIfFullEnergy || caster.TechPoints == caster.GetMaxTechPoints())
		{
			RampartDelayedAoeEffect effect = new RampartDelayedAoeEffect(AsEffectSource(), caster.GetCurrentBoardSquare(), caster, caster, m_aoeDelayTurns, m_aoeShape, m_aoeDamageAmount, m_aoeEnemyHitEffect, m_penetrateLos, m_aoeMarkerSequencePrefab, m_aoeDetonateSequencePrefab);
			actorHitResults.AddEffect(effect);
			if (m_onlyDoAoeIfFullEnergy)
			{
				actorHitResults.SetTechPointLoss(caster.GetMaxTechPoints());
			}
		}
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
