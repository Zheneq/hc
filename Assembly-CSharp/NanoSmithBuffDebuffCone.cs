using System.Collections.Generic;
using UnityEngine;

public class NanoSmithBuffDebuffCone : Ability
{
	[Header("-- Cone Targeting")]
	public float m_coneAngle = 270f;

	public float m_coneLength = 1.5f;

	public bool m_conePenetrateLineOfSight;

	[Header("-- Hit Effects")]
	public StandardEffectInfo m_enemyHitEffect;

	public StandardEffectInfo m_allyHitEffect;

	public StandardEffectInfo m_casterHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Buff Debuff Cone";
		}
		m_sequencePrefab = m_castSequencePrefab;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, m_coneAngle, m_coneLength, 0f, m_conePenetrateLineOfSight, true, m_enemyHitEffect.m_applyEffect, m_allyHitEffect.m_applyEffect, m_casterHitEffect.m_applyEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_allyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_casterHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}
}
