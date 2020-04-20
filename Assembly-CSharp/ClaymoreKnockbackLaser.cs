using System;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreKnockbackLaser : Ability
{
	[Header("-- Only As Indicator For Passive? (If true, the ability cannot be cast)")]
	public bool m_onlyAsIndicatorForPassive;

	[Header("-- Laser Targeting")]
	public float m_laserRange = 4f;

	public float m_laserFullWidth = 5f;

	public float m_laserMiddleWidth = 1f;

	public bool m_penetrateLos;

	public bool m_lengthIgnoreWorldGeo;

	[Header("-- Knockback Params")]
	public KnockbackType m_knockbackType = KnockbackType.PerpendicularAwayFromAimDir;

	public float m_knockbackDistance = 2f;

	[Header("-- Middle Hit Damage/Effect")]
	public int m_middleHitDamage = 0x14;

	public StandardEffectInfo m_middleEnemyHitEffect;

	[Header("-- Knockback Hit Damage/Effect")]
	public int m_knockbackDamage = 0xA;

	public StandardEffectInfo m_knockbackEnemyHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Way of the Waste";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_ClaymoreKnockbackLaser(this, this.GetLaserFullWidth(), this.GetLaserRange(), this.m_penetrateLos, this.m_lengthIgnoreWorldGeo, 0, this.GetLaserMiddleWidth(), this.GetKnockbackDistance(), this.m_knockbackType);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_middleHitDamage);
		this.m_middleEnemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Secondary, this.m_knockbackDamage);
		this.m_knockbackEnemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary);
		return result;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return !this.m_onlyAsIndicatorForPassive;
	}

	private float GetLaserRange()
	{
		return this.m_laserRange;
	}

	private float GetLaserFullWidth()
	{
		return this.m_laserFullWidth;
	}

	private float GetLaserMiddleWidth()
	{
		return this.m_laserMiddleWidth;
	}

	private float GetKnockbackDistance()
	{
		return this.m_knockbackDistance;
	}

	private int GetMiddleHitDamage()
	{
		return this.m_middleHitDamage;
	}

	private StandardEffectInfo GetMiddleEnemyHitEffect()
	{
		return this.m_middleEnemyHitEffect;
	}

	private int GetKnockbackDamage()
	{
		return this.m_knockbackDamage;
	}

	private StandardEffectInfo GetKnockbackEnemyHieEffect()
	{
		return this.m_knockbackEnemyHitEffect;
	}
}
