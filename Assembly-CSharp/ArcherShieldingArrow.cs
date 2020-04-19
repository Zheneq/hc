using System;
using System.Collections.Generic;
using UnityEngine;

public class ArcherShieldingArrow : Ability
{
	[Header("-- Targeting Properties --")]
	public int m_laserCount = 2;

	public float m_targeterMinAngle;

	public float m_targeterMaxAngle = 100f;

	public float m_targeterMinInterpDistance = 0.5f;

	public float m_targeterMaxInterpDistance = 4f;

	public LaserTargetingInfo m_laserTargetingInfo;

	[Header("-- On Hit --")]
	public float m_laserEnergyGainPerHit;

	[Header("-- Enemy Single Hit Effect")]
	public StandardEffectInfo m_enemySingleHitEffect;

	[Header("-- Enemy Multi Hit Effect")]
	public StandardEffectInfo m_enemyMultiHitEffect;

	[Header("-- Ally Single Hit Effect")]
	public StandardEffectInfo m_allySingleHitEffect;

	[Header("-- Ally Multi Hit Effect")]
	public StandardEffectInfo m_allyMultiHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private LaserTargetingInfo m_cachedLaserTargetingInfo;

	private StandardEffectInfo m_cachedEnemySingleEffect;

	private StandardEffectInfo m_cachedEnemyMultiEffect;

	private StandardEffectInfo m_cachedAllySingleEffect;

	private StandardEffectInfo m_cachedAllyMultiEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldingArrow.Start()).MethodHandle;
			}
			this.m_abilityName = "Resonance Arrow";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		this.m_targeterMinAngle = Mathf.Max(0f, this.m_targeterMinAngle);
		LaserTargetingInfo laserTargetingInfo = this.GetLaserTargetingInfo();
		base.Targeter = new AbilityUtil_Targeter_ThiefFanLaser(this, this.GetTargeterMinAngle(), this.GetTargeterMaxAngle(), this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance, laserTargetingInfo.range, laserTargetingInfo.width, laserTargetingInfo.maxTargets, this.GetLaserCount(), laserTargetingInfo.penetrateLos, false, false, false, true, 0, 0f, 0f);
		base.Targeter.SetAffectedGroups(laserTargetingInfo.affectsEnemies, laserTargetingInfo.affectsAllies, false);
	}

	private void SetCachedFields()
	{
		this.m_cachedLaserTargetingInfo = this.m_laserTargetingInfo;
		this.m_cachedEnemySingleEffect = this.m_enemySingleHitEffect;
		this.m_cachedEnemyMultiEffect = this.m_enemyMultiHitEffect;
		this.m_cachedAllySingleEffect = this.m_allySingleHitEffect;
		this.m_cachedAllyMultiEffect = this.m_allyMultiHitEffect;
	}

	public LaserTargetingInfo GetLaserTargetingInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserTargetingInfo != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldingArrow.GetLaserTargetingInfo()).MethodHandle;
			}
			result = this.m_cachedLaserTargetingInfo;
		}
		else
		{
			result = this.m_laserTargetingInfo;
		}
		return result;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllySingleEffect != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldingArrow.GetAllySingleHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllySingleEffect;
		}
		else
		{
			result = this.m_allySingleHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedAllyMultiEffect != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldingArrow.GetAllyMultiHitEffect()).MethodHandle;
			}
			result = this.m_cachedAllyMultiEffect;
		}
		else
		{
			result = this.m_allyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		return (this.m_cachedEnemySingleEffect == null) ? this.m_enemySingleHitEffect : this.m_cachedEnemySingleEffect;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedEnemyMultiEffect != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherShieldingArrow.GetEnemyMultiHitEffect()).MethodHandle;
			}
			result = this.m_cachedEnemyMultiEffect;
		}
		else
		{
			result = this.m_enemyMultiHitEffect;
		}
		return result;
	}

	private int GetLaserCount()
	{
		return this.m_laserCount;
	}

	private float GetTargeterMinAngle()
	{
		return this.m_targeterMinAngle;
	}

	private float GetTargeterMaxAngle()
	{
		return this.m_targeterMaxAngle;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		StandardEffectInfo enemySingleHitEffect = this.m_enemySingleHitEffect;
		StandardEffectInfo enemyMultiHitEffect = this.m_enemyMultiHitEffect;
		StandardEffectInfo allySingleHitEffect = this.m_allySingleHitEffect;
		StandardEffectInfo allyMultiHitEffect = this.m_allyMultiHitEffect;
		AbilityMod.AddToken_EffectInfo(tokens, enemySingleHitEffect, "Effect_EnemySingleHit", null, true);
		AbilityMod.AddToken_EffectInfo(tokens, enemyMultiHitEffect, "Effect_EnemyMultiHit", null, true);
		AbilityMod.AddToken_EffectInfo(tokens, allySingleHitEffect, "Effect_AllySingleHit", null, true);
		AbilityMod.AddToken_EffectInfo(tokens, allyMultiHitEffect, "Effect_AllyMultiHit", null, true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_enemySingleHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		this.m_enemyMultiHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		this.m_allySingleHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		this.m_allyMultiHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Ally);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		int absorbAmount = this.m_allySingleHitEffect.m_effectData.m_absorbAmount;
		int subsequentAmount = this.m_allyMultiHitEffect.m_effectData.m_absorbAmount - absorbAmount;
		Ability.AddNameplateValueForOverlap(ref result, base.Targeter, targetActor, currentTargeterIndex, absorbAmount, subsequentAmount, AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Primary);
		return result;
	}

	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float value = (currentTarget.FreePos - targetingActor.\u0016()).magnitude / Board.\u000E().squareSize;
		float num = Mathf.Clamp(value, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance) - this.m_targeterMinInterpDistance;
		return Mathf.Max(this.GetTargeterMinAngle(), this.GetTargeterMaxAngle() * (1f - num / (this.m_targeterMaxInterpDistance - this.m_targeterMinInterpDistance)));
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(fanAngleDegrees, this.GetTargeterMaxAngle(), this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance);
	}
}
