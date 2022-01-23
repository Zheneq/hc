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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Resonance Arrow";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		m_targeterMinAngle = Mathf.Max(0f, m_targeterMinAngle);
		LaserTargetingInfo laserTargetingInfo = GetLaserTargetingInfo();
		base.Targeter = new AbilityUtil_Targeter_ThiefFanLaser(this, GetTargeterMinAngle(), GetTargeterMaxAngle(), m_targeterMinInterpDistance, m_targeterMaxInterpDistance, laserTargetingInfo.range, laserTargetingInfo.width, laserTargetingInfo.maxTargets, GetLaserCount(), laserTargetingInfo.penetrateLos, false, false, false, true, 0);
		base.Targeter.SetAffectedGroups(laserTargetingInfo.affectsEnemies, laserTargetingInfo.affectsAllies, false);
	}

	private void SetCachedFields()
	{
		m_cachedLaserTargetingInfo = m_laserTargetingInfo;
		m_cachedEnemySingleEffect = m_enemySingleHitEffect;
		m_cachedEnemyMultiEffect = m_enemyMultiHitEffect;
		m_cachedAllySingleEffect = m_allySingleHitEffect;
		m_cachedAllyMultiEffect = m_allyMultiHitEffect;
	}

	public LaserTargetingInfo GetLaserTargetingInfo()
	{
		LaserTargetingInfo result;
		if (m_cachedLaserTargetingInfo != null)
		{
			result = m_cachedLaserTargetingInfo;
		}
		else
		{
			result = m_laserTargetingInfo;
		}
		return result;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllySingleEffect != null)
		{
			result = m_cachedAllySingleEffect;
		}
		else
		{
			result = m_allySingleHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedAllyMultiEffect != null)
		{
			result = m_cachedAllyMultiEffect;
		}
		else
		{
			result = m_allyMultiHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		return (m_cachedEnemySingleEffect == null) ? m_enemySingleHitEffect : m_cachedEnemySingleEffect;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedEnemyMultiEffect != null)
		{
			result = m_cachedEnemyMultiEffect;
		}
		else
		{
			result = m_enemyMultiHitEffect;
		}
		return result;
	}

	private int GetLaserCount()
	{
		return m_laserCount;
	}

	private float GetTargeterMinAngle()
	{
		return m_targeterMinAngle;
	}

	private float GetTargeterMaxAngle()
	{
		return m_targeterMaxAngle;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		StandardEffectInfo enemySingleHitEffect = m_enemySingleHitEffect;
		StandardEffectInfo enemyMultiHitEffect = m_enemyMultiHitEffect;
		StandardEffectInfo allySingleHitEffect = m_allySingleHitEffect;
		StandardEffectInfo allyMultiHitEffect = m_allyMultiHitEffect;
		AbilityMod.AddToken_EffectInfo(tokens, enemySingleHitEffect, "Effect_EnemySingleHit");
		AbilityMod.AddToken_EffectInfo(tokens, enemyMultiHitEffect, "Effect_EnemyMultiHit");
		AbilityMod.AddToken_EffectInfo(tokens, allySingleHitEffect, "Effect_AllySingleHit");
		AbilityMod.AddToken_EffectInfo(tokens, allyMultiHitEffect, "Effect_AllyMultiHit");
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_enemySingleHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		m_enemyMultiHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		m_allySingleHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		m_allyMultiHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		int absorbAmount = m_allySingleHitEffect.m_effectData.m_absorbAmount;
		int subsequentAmount = m_allyMultiHitEffect.m_effectData.m_absorbAmount - absorbAmount;
		Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeter, targetActor, currentTargeterIndex, absorbAmount, subsequentAmount, AbilityTooltipSymbol.Absorb);
		return symbolToValue;
	}

	private float CalculateFanAngleDegrees(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float value = (currentTarget.FreePos - targetingActor.GetFreePos()).magnitude / Board.Get().squareSize;
		float num = Mathf.Clamp(value, m_targeterMinInterpDistance, m_targeterMaxInterpDistance) - m_targeterMinInterpDistance;
		return Mathf.Max(GetTargeterMinAngle(), GetTargeterMaxAngle() * (1f - num / (m_targeterMaxInterpDistance - m_targeterMinInterpDistance)));
	}

	public float CalculateDistanceFromFanAngleDegrees(float fanAngleDegrees)
	{
		return AbilityCommon_FanLaser.CalculateDistanceFromFanAngleDegrees(fanAngleDegrees, GetTargeterMaxAngle(), m_targeterMinInterpDistance, m_targeterMaxInterpDistance);
	}
}
