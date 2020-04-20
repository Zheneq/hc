using System;
using System.Collections.Generic;
using UnityEngine;

public class ThiefSafecracker : Ability
{
	public int m_damageAmount = 5;

	public float m_laserRange = 5f;

	public float m_laserWidth = 0.5f;

	public bool m_laserPenetrateLos = true;

	public int m_returnDelay = 1;

	public int m_returnEffectAnimationIndex = 1;

	public float m_knockbackDistance = 2f;

	public KnockbackType m_knockbackType = KnockbackType.PerpendicularAwayFromAimDir;

	public GameObject m_returnSequencePrefab;

	public GameObject m_groundSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Safecracker";
		}
		base.Targeter = new AbilityUtil_Targeter_KnockbackLaser(this, this.m_laserWidth, this.m_laserRange, this.m_laserPenetrateLos, -1, this.m_knockbackDistance, this.m_knockbackDistance, this.m_knockbackType, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.m_damageAmount > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_damageAmount);
		}
		return result;
	}
}
