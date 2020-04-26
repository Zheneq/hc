using System.Collections.Generic;
using UnityEngine;

public class ValkyriePullToConeCenter : Ability
{
	[Header("-- Targeting")]
	public float m_coneAngleWidth = 60f;

	public float m_coneLengthInSquares = 5.5f;

	public float m_coneBackwardOffset;

	public bool m_penetratesLoS;

	[Header("-- Damage & effects")]
	public int m_damage = 40;

	public StandardEffectInfo m_effectToEnemies;

	[Header("-- Knockback on Cast")]
	public float m_maxKnockbackDist = 3f;

	public KnockbackType m_knockbackType = KnockbackType.PerpendicularPullToAimDir;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Valkyrie Pull Cone";
		}
		Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, GetConeLength(), GetConeLength(), GetConeWidth(), GetConeWidth(), AreaEffectUtils.StretchConeStyle.Linear, m_coneBackwardOffset, GetPenetrateLoS());
		abilityUtil_Targeter_StretchCone.InitKnockbackData(GetKnockbackDistance(), m_knockbackType, 0f, m_knockbackType);
		base.Targeter = abilityUtil_Targeter_StretchCone;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamage());
		return numbers;
	}

	private int GetDamage()
	{
		return m_damage;
	}

	private StandardEffectInfo GetEffectOnEnemy()
	{
		return m_effectToEnemies;
	}

	private float GetConeWidth()
	{
		return m_coneAngleWidth;
	}

	private float GetConeLength()
	{
		return m_coneLengthInSquares;
	}

	private bool GetPenetrateLoS()
	{
		return m_penetratesLoS;
	}

	private float GetKnockbackDistance()
	{
		return m_maxKnockbackDist;
	}
}
