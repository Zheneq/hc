using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithBuffDebuffCone.Start()).MethodHandle;
			}
			this.m_abilityName = "Buff Debuff Cone";
		}
		this.m_sequencePrefab = this.m_castSequencePrefab;
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_DirectionCone(this, this.m_coneAngle, this.m_coneLength, 0f, this.m_conePenetrateLineOfSight, true, this.m_enemyHitEffect.m_applyEffect, this.m_allyHitEffect.m_applyEffect, this.m_casterHitEffect.m_applyEffect, -1, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		this.m_allyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.m_casterHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}
}
