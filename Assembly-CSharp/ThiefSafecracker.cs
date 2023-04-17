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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Safecracker";
		}
		Targeter = new AbilityUtil_Targeter_KnockbackLaser(
			this,
			m_laserWidth,
			m_laserRange,
			m_laserPenetrateLos,
			-1,
			m_knockbackDistance,
			m_knockbackDistance,
			m_knockbackType,
			false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_damageAmount > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		}
		return numbers;
	}
}
