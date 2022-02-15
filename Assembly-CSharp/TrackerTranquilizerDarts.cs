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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Tranquilizer Darts";
		}
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			Debug.LogError("No drone tracker component");
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_droneInfoComp == null)
		{
			m_droneInfoComp = GetComponent<TrackerDroneInfoComponent>();
		}
		m_targeterMinAngle = Mathf.Max(0f, m_targeterMinAngle);
		Targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(
			this, GetLaserCount(), GetLaserTargetingInfo(), m_angleInBetween, m_changeAngleByCursorDistance,
			m_targeterMinAngle, m_targeterMaxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserTargetingInfo().range;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_TrackerTranquilizerDarts abilityMod_TrackerTranquilizerDarts = modAsBase as AbilityMod_TrackerTranquilizerDarts;
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TrackerTranquilizerDarts != null
			? abilityMod_TrackerTranquilizerDarts.m_enemySingleHitEffectMod.GetModifiedValue(m_enemySingleHitEffect)
			: m_enemySingleHitEffect, "Effect_EnemySingleHit");
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TrackerTranquilizerDarts != null
			? abilityMod_TrackerTranquilizerDarts.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect)
			: m_enemyMultiHitEffect, "Effect_EnemyMultiHit");
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TrackerTranquilizerDarts != null
			? abilityMod_TrackerTranquilizerDarts.m_allySingleHitEffectMod.GetModifiedValue(m_allySingleHitEffect)
			: m_allySingleHitEffect, "Effect_AllySingleHit");
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_TrackerTranquilizerDarts != null
			? abilityMod_TrackerTranquilizerDarts.m_allyMultiHitEffectMod.GetModifiedValue(m_allyMultiHitEffect)
			: m_allyMultiHitEffect, "Effect_AllyMultiHit");
		TrackerHuntingCrossbow component = GetComponent<TrackerHuntingCrossbow>();
		if (component != null)
		{
			component.m_huntedEffectData.AddTooltipTokens(tokens, "TrackedEffect");
		}
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		m_enemySingleHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		m_enemyMultiHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		AddNameplateValueForOverlap(ref symbolToValue, Targeter, targetActor, currentTargeterIndex, m_laserDamageAmount, m_laserDamageAmount);
		return symbolToValue;
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(fanAngleDegrees, m_targeterMaxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_TrackerTranquilizerDarts))
		{
			m_abilityMod = (abilityMod as AbilityMod_TrackerTranquilizerDarts);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedLaserTargetingInfo = m_abilityMod != null
			? m_abilityMod.m_laserTargetingInfoMod.GetModifiedValue(m_laserTargetingInfo)
			: m_laserTargetingInfo;
	}

	public LaserTargetingInfo GetLaserTargetingInfo()
	{
		return m_cachedLaserTargetingInfo ?? m_laserTargetingInfo;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allySingleHitEffectMod.GetModifiedValue(m_allySingleHitEffect)
			: m_allySingleHitEffect;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyMultiHitEffectMod.GetModifiedValue(m_allyMultiHitEffect)
			: m_allyMultiHitEffect;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemySingleHitEffectMod.GetModifiedValue(m_enemySingleHitEffect)
			: m_enemySingleHitEffect;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect)
			: m_enemyMultiHitEffect;
	}

	private int GetLaserCount()
	{
		int count = m_abilityMod != null
			? m_abilityMod.m_laserCountMod.GetModifiedValue(m_laserCount)
			: m_laserCount;
		return Mathf.Max(1, count);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (GetLaserCount() > 1)
		{
			min = m_targeterMinInterpDistance * Board.Get().squareSize;
			max = m_targeterMaxInterpDistance * Board.Get().squareSize;
			return true;
		}
		else
		{
			return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
		}
	}
}
