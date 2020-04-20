using System;
using System.Collections.Generic;
using UnityEngine;

public class TrackerTranquilizerDarts : Ability
{
	[Header("-- Laser Info --------------------------------------")]
	public int m_laserCount = 5;

	public float m_angleInBetween = 10f;

	[Header("-- Targeting Properties --")]
	public bool m_changeAngleByCursorDistance;

	public float m_targeterMinAngle;

	public float m_targeterMaxAngle = 180f;

	public float m_targeterMinInterpDistance = 0.5f;

	public float m_targeterMaxInterpDistance = 4f;

	public LaserTargetingInfo m_laserTargetingInfo;

	[Header("-- On Hit --")]
	public int m_laserDamageAmount = 1;

	public int m_laserEnergyDamageAmount;

	public float m_laserEnergyGainPerHit;

	[Header("-- Enemy Single Hit Effect")]
	public StandardEffectInfo m_enemySingleHitEffect;

	[Header("-- Enemy Multi Hit Effect")]
	public StandardEffectInfo m_enemyMultiHitEffect;

	[Header("-- Ally Single Hit Effect")]
	public StandardEffectInfo m_allySingleHitEffect;

	[Header("-- Ally Multi Hit Effect")]
	public StandardEffectInfo m_allyMultiHitEffect;

	[Header("-- Whether to apply <Tracked> Effect")]
	public bool m_applyTrackedEffect = true;

	private TrackerDroneTrackerComponent m_droneTracker;

	private TrackerDroneInfoComponent m_droneInfoComp;

	private AbilityMod_TrackerTranquilizerDarts m_abilityMod;

	private LaserTargetingInfo m_cachedLaserTargetingInfo;

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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.Start()).MethodHandle;
			}
			this.m_abilityName = "Tranquilizer Darts";
		}
		this.m_droneTracker = base.GetComponent<TrackerDroneTrackerComponent>();
		if (this.m_droneTracker == null)
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
			Debug.LogError("No drone tracker component");
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		if (this.m_droneInfoComp == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.SetupTargeter()).MethodHandle;
			}
			this.m_droneInfoComp = base.GetComponent<TrackerDroneInfoComponent>();
		}
		this.m_targeterMinAngle = Mathf.Max(0f, this.m_targeterMinAngle);
		AbilityUtil_Targeter targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(this, this.GetLaserCount(), this.GetLaserTargetingInfo(), this.m_angleInBetween, this.m_changeAngleByCursorDistance, this.m_targeterMinAngle, this.m_targeterMaxAngle, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance);
		base.Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserTargetingInfo().range;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerTranquilizerDarts abilityMod_TrackerTranquilizerDarts = modAsBase as AbilityMod_TrackerTranquilizerDarts;
		StandardEffectInfo standardEffectInfo;
		if (abilityMod_TrackerTranquilizerDarts)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			standardEffectInfo = abilityMod_TrackerTranquilizerDarts.m_enemySingleHitEffectMod.GetModifiedValue(this.m_enemySingleHitEffect);
		}
		else
		{
			standardEffectInfo = this.m_enemySingleHitEffect;
		}
		StandardEffectInfo effectInfo = standardEffectInfo;
		StandardEffectInfo effectInfo2 = (!abilityMod_TrackerTranquilizerDarts) ? this.m_enemyMultiHitEffect : abilityMod_TrackerTranquilizerDarts.m_enemyMultiHitEffectMod.GetModifiedValue(this.m_enemyMultiHitEffect);
		StandardEffectInfo standardEffectInfo2;
		if (abilityMod_TrackerTranquilizerDarts)
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
			standardEffectInfo2 = abilityMod_TrackerTranquilizerDarts.m_allySingleHitEffectMod.GetModifiedValue(this.m_allySingleHitEffect);
		}
		else
		{
			standardEffectInfo2 = this.m_allySingleHitEffect;
		}
		StandardEffectInfo effectInfo3 = standardEffectInfo2;
		StandardEffectInfo standardEffectInfo3;
		if (abilityMod_TrackerTranquilizerDarts)
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
			standardEffectInfo3 = abilityMod_TrackerTranquilizerDarts.m_allyMultiHitEffectMod.GetModifiedValue(this.m_allyMultiHitEffect);
		}
		else
		{
			standardEffectInfo3 = this.m_allyMultiHitEffect;
		}
		StandardEffectInfo effectInfo4 = standardEffectInfo3;
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "Effect_EnemySingleHit", null, true);
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "Effect_EnemyMultiHit", null, true);
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "Effect_AllySingleHit", null, true);
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "Effect_AllyMultiHit", null, true);
		TrackerHuntingCrossbow component = base.GetComponent<TrackerHuntingCrossbow>();
		if (component != null)
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
			component.m_huntedEffectData.AddTooltipTokens(tokens, "TrackedEffect", false, null);
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_laserDamageAmount);
		this.m_enemySingleHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		this.m_enemyMultiHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		Ability.AddNameplateValueForOverlap(ref result, base.Targeter, targetActor, currentTargeterIndex, this.m_laserDamageAmount, this.m_laserDamageAmount, AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary);
		return result;
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(fanAngleDegrees, this.m_targeterMaxAngle, this.m_targeterMinInterpDistance, this.m_targeterMaxInterpDistance);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerTranquilizerDarts))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_TrackerTranquilizerDarts);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo cachedLaserTargetingInfo;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.SetCachedFields()).MethodHandle;
			}
			cachedLaserTargetingInfo = this.m_abilityMod.m_laserTargetingInfoMod.GetModifiedValue(this.m_laserTargetingInfo);
		}
		else
		{
			cachedLaserTargetingInfo = this.m_laserTargetingInfo;
		}
		this.m_cachedLaserTargetingInfo = cachedLaserTargetingInfo;
	}

	public LaserTargetingInfo GetLaserTargetingInfo()
	{
		return (this.m_cachedLaserTargetingInfo == null) ? this.m_laserTargetingInfo : this.m_cachedLaserTargetingInfo;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.GetAllySingleHitEffect()).MethodHandle;
			}
			result = this.m_allySingleHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_allySingleHitEffectMod.GetModifiedValue(this.m_allySingleHitEffect);
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.GetAllyMultiHitEffect()).MethodHandle;
			}
			result = this.m_allyMultiHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_allyMultiHitEffectMod.GetModifiedValue(this.m_allyMultiHitEffect);
		}
		return result;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.GetEnemySingleHitEffect()).MethodHandle;
			}
			result = this.m_enemySingleHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_enemySingleHitEffectMod.GetModifiedValue(this.m_enemySingleHitEffect);
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.GetEnemyMultiHitEffect()).MethodHandle;
			}
			result = this.m_enemyMultiHitEffect;
		}
		else
		{
			result = this.m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(this.m_enemyMultiHitEffect);
		}
		return result;
	}

	private int GetLaserCount()
	{
		int num;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.GetLaserCount()).MethodHandle;
			}
			num = this.m_laserCount;
		}
		else
		{
			num = this.m_abilityMod.m_laserCountMod.GetModifiedValue(this.m_laserCount);
		}
		int b = num;
		return Mathf.Max(1, b);
	}

	public unsafe override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (this.GetLaserCount() > 1)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TrackerTranquilizerDarts.HasRestrictedFreePosDistance(ActorData, int, List<AbilityTarget>, float*, float*)).MethodHandle;
			}
			min = this.m_targeterMinInterpDistance * Board.Get().squareSize;
			max = this.m_targeterMaxInterpDistance * Board.Get().squareSize;
			return true;
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}
}
