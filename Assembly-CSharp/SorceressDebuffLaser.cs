using System;
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
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Laser(this, this.GetLaserWidth(), this.GetLaserRange(), this.m_penetrateLineOfSight, -1, this.GetAllyHitEffect().m_applyEffect, this.GetCasterHitEffect().m_applyEffect);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_enemyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Enemy);
		this.m_allyHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.m_casterHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressDebuffLaser abilityMod_SorceressDebuffLaser = modAsBase as AbilityMod_SorceressDebuffLaser;
		StandardEffectInfo effectInfo;
		if (abilityMod_SorceressDebuffLaser)
		{
			effectInfo = abilityMod_SorceressDebuffLaser.m_enemyHitEffectOverride.GetModifiedValue(this.m_enemyHitEffect);
		}
		else
		{
			effectInfo = this.m_enemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EnemyHitEffect", this.m_enemyHitEffect, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_SorceressDebuffLaser)
		{
			effectInfo2 = abilityMod_SorceressDebuffLaser.m_allyHitEffectOverride.GetModifiedValue(this.m_allyHitEffect);
		}
		else
		{
			effectInfo2 = this.m_allyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "AllyHitEffect", this.m_allyHitEffect, true);
		StandardEffectInfo effectInfo3;
		if (abilityMod_SorceressDebuffLaser)
		{
			effectInfo3 = abilityMod_SorceressDebuffLaser.m_casterHitEffectOverride.GetModifiedValue(this.m_casterHitEffect);
		}
		else
		{
			effectInfo3 = this.m_casterHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "CasterHitEffect", this.m_casterHitEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressDebuffLaser))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SorceressDebuffLaser);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private float GetLaserWidth()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_width;
		}
		else
		{
			result = this.m_abilityMod.m_laserWidthMod.GetModifiedValue(this.m_width);
		}
		return result;
	}

	private float GetLaserRange()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_distance;
		}
		else
		{
			result = this.m_abilityMod.m_laserRangeMod.GetModifiedValue(this.m_distance);
		}
		return result;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = this.m_enemyHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(this.m_enemyHitEffect);
		}
		return result;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = this.m_allyHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_allyHitEffectOverride.GetModifiedValue(this.m_allyHitEffect);
		}
		return result;
	}

	private StandardEffectInfo GetCasterHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = this.m_casterHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_casterHitEffectOverride.GetModifiedValue(this.m_casterHitEffect);
		}
		return result;
	}

	private bool HasAdditionalEffectIfHit()
	{
		return this.m_abilityMod != null && this.m_abilityMod.m_additionalEffectOnSelfIfHit.m_applyEffect;
	}

	private int GetEnemyEffectDuration()
	{
		int result = this.m_enemyHitEffect.m_effectData.m_duration;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_enemyEffectDurationMod.GetModifiedValue(this.GetEnemyHitEffect().m_effectData.m_duration);
		}
		return result;
	}

	private int GetAllyEffectDuration()
	{
		int result = this.m_allyHitEffect.m_effectData.m_duration;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_allyEffectDurationMod.GetModifiedValue(this.GetAllyHitEffect().m_effectData.m_duration);
		}
		return result;
	}

	private int GetCasterEffectDuration()
	{
		int result = this.m_casterHitEffect.m_effectData.m_duration;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_casterEffectDurationMod.GetModifiedValue(this.GetCasterHitEffect().m_effectData.m_duration);
		}
		return result;
	}

	private int GetCooldownReduction(int numHit)
	{
		int num = 0;
		if (this.m_abilityMod != null)
		{
			num = this.m_abilityMod.m_cooldownReductionOnNumHit.GetModifiedValue(numHit);
			num += this.m_abilityMod.m_cooldownFlatReduction;
			num = Mathf.Clamp(num, 0, this.m_abilityMod.m_maxCooldownReduction);
		}
		return num;
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> result = new List<Vector3>();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		Vector3 aimDirection = targets[0].AimDirection;
		float maxDistanceInWorld = this.GetLaserRange() * Board.Get().squareSize;
		Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, aimDirection, maxDistanceInWorld, this.m_penetrateLineOfSight, caster, null, true);
		AreaEffectUtils.AddBoxExtremaToList(ref result, travelBoardSquareWorldPositionForLos, laserEndPoint, this.GetLaserWidth());
		return result;
	}
}
