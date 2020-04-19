using System;
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

	public int m_aoeDamageAmount = 0xA;

	public StandardEffectInfo m_aoeEnemyHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	public GameObject m_aoeMarkerSequencePrefab;

	public GameObject m_aoeDetonateSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Buff and Delayed Aoe";
		}
		this.m_sequencePrefab = this.m_castSequencePrefab;
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityAreaShape shapeLowEnergy;
		if (this.m_onlyDoAoeIfFullEnergy)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RampartBuffAndDelayedAoe.SetupTargeter()).MethodHandle;
			}
			shapeLowEnergy = AbilityAreaShape.SingleSquare;
		}
		else
		{
			shapeLowEnergy = this.m_aoeShape;
		}
		base.Targeter = new AbilityUtil_Targeter_RampartDelayedAoe(this, shapeLowEnergy, this.m_aoeShape, this.m_penetrateLos, this.m_aoeDelayTurns <= 0, false, this.m_selfBuffEffect.m_applyEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_selfBuffEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.m_aoeDamageAmount);
		this.m_aoeEnemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		return result;
	}
}
