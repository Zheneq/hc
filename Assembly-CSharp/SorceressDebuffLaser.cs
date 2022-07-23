using System.Collections.Generic;
using UnityEngine;

public class SorceressDebuffLaser : Ability
{
	public bool m_penetrateLineOfSight;
	public float m_width = 1f;
	public float m_distance = 15f;
	[Header("-- Hit Effects")]
	public StandardEffectInfo m_enemyHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_casterHitEffect;
	private AbilityMod_SorceressDebuffLaser m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_Laser(
			this,
			GetLaserWidth(),
			GetLaserRange(),
			m_penetrateLineOfSight,
			-1,
			GetAllyHitEffect().m_applyEffect,
			GetCasterHitEffect().m_applyEffect);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		m_allyHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_casterHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressDebuffLaser abilityMod_SorceressDebuffLaser = modAsBase as AbilityMod_SorceressDebuffLaser;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressDebuffLaser != null
			? abilityMod_SorceressDebuffLaser.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressDebuffLaser != null
			? abilityMod_SorceressDebuffLaser.m_allyHitEffectOverride.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressDebuffLaser != null
			? abilityMod_SorceressDebuffLaser.m_casterHitEffectOverride.GetModifiedValue(m_casterHitEffect)
			: m_casterHitEffect, "CasterHitEffect", m_casterHitEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SorceressDebuffLaser))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_SorceressDebuffLaser;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_width)
			: m_width;
	}

	private float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_distance)
			: m_distance;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHitEffectOverride.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	private StandardEffectInfo GetCasterHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_casterHitEffectOverride.GetModifiedValue(m_casterHitEffect)
			: m_casterHitEffect;
	}

	private bool HasAdditionalEffectIfHit()
	{
		return m_abilityMod != null && m_abilityMod.m_additionalEffectOnSelfIfHit.m_applyEffect;
	}

	private int GetEnemyEffectDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyEffectDurationMod.GetModifiedValue(GetEnemyHitEffect().m_effectData.m_duration)
			: m_enemyHitEffect.m_effectData.m_duration;
	}

	private int GetAllyEffectDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyEffectDurationMod.GetModifiedValue(GetAllyHitEffect().m_effectData.m_duration)
			: m_allyHitEffect.m_effectData.m_duration;
	}

	private int GetCasterEffectDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_casterEffectDurationMod.GetModifiedValue(GetCasterHitEffect().m_effectData.m_duration)
			: m_casterHitEffect.m_effectData.m_duration;
	}

	private int GetCooldownReduction(int numHit)
	{
		return m_abilityMod != null
			? Mathf.Clamp(
				m_abilityMod.m_cooldownReductionOnNumHit.GetModifiedValue(numHit) + m_abilityMod.m_cooldownFlatReduction,
				0,
				m_abilityMod.m_maxCooldownReduction)
			: 0;
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> points = new List<Vector3>();
		float maxDistanceInWorld = GetLaserRange() * Board.Get().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(caster.GetLoSCheckPos(), targets[0].AimDirection, maxDistanceInWorld, m_penetrateLineOfSight, caster);
		AreaEffectUtils.AddBoxExtremaToList(ref points, caster.GetLoSCheckPos(), laserEndPoint, GetLaserWidth());
		return points;
	}
}
