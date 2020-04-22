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
		int shapeLowEnergy;
		if (m_onlyDoAoeIfFullEnergy)
		{
			shapeLowEnergy = 0;
		}
		else
		{
			shapeLowEnergy = (int)m_aoeShape;
		}
		base.Targeter = new AbilityUtil_Targeter_RampartDelayedAoe(this, (AbilityAreaShape)shapeLowEnergy, m_aoeShape, m_penetrateLos, m_aoeDelayTurns <= 0, false, m_selfBuffEffect.m_applyEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_selfBuffEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_aoeDamageAmount);
		m_aoeEnemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}
}
