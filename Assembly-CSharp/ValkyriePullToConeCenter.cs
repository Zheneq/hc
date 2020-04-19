using System;
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
	public int m_damage = 0x28;

	public StandardEffectInfo m_effectToEnemies;

	[Header("-- Knockback on Cast")]
	public float m_maxKnockbackDist = 3f;

	public KnockbackType m_knockbackType = KnockbackType.PerpendicularPullToAimDir;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Valkyrie Pull Cone";
		}
		this.Setup();
	}

	private void Setup()
	{
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, this.GetConeLength(), this.GetConeLength(), this.GetConeWidth(), this.GetConeWidth(), AreaEffectUtils.StretchConeStyle.Linear, this.m_coneBackwardOffset, this.GetPenetrateLoS());
		abilityUtil_Targeter_StretchCone.InitKnockbackData(this.GetKnockbackDistance(), this.m_knockbackType, 0f, this.m_knockbackType);
		base.Targeter = abilityUtil_Targeter_StretchCone;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Enemy, this.GetDamage());
		return result;
	}

	private int GetDamage()
	{
		return this.m_damage;
	}

	private StandardEffectInfo GetEffectOnEnemy()
	{
		return this.m_effectToEnemies;
	}

	private float GetConeWidth()
	{
		return this.m_coneAngleWidth;
	}

	private float GetConeLength()
	{
		return this.m_coneLengthInSquares;
	}

	private bool GetPenetrateLoS()
	{
		return this.m_penetratesLoS;
	}

	private float GetKnockbackDistance()
	{
		return this.m_maxKnockbackDist;
	}
}
