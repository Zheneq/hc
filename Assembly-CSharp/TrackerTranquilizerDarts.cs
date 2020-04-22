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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_abilityName = "Tranquilizer Darts";
		}
		m_droneTracker = GetComponent<TrackerDroneTrackerComponent>();
		if (m_droneTracker == null)
		{
			while (true)
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
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_droneInfoComp == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_droneInfoComp = GetComponent<TrackerDroneInfoComponent>();
		}
		m_targeterMinAngle = Mathf.Max(0f, m_targeterMinAngle);
		AbilityUtil_Targeter abilityUtil_Targeter2 = base.Targeter = AbilityCommon_FanLaser.CreateTargeter_SingleClick(this, GetLaserCount(), GetLaserTargetingInfo(), m_angleInBetween, m_changeAngleByCursorDistance, m_targeterMinAngle, m_targeterMaxAngle, m_targeterMinInterpDistance, m_targeterMaxInterpDistance);
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
		StandardEffectInfo standardEffectInfo;
		if ((bool)abilityMod_TrackerTranquilizerDarts)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			standardEffectInfo = abilityMod_TrackerTranquilizerDarts.m_enemySingleHitEffectMod.GetModifiedValue(m_enemySingleHitEffect);
		}
		else
		{
			standardEffectInfo = m_enemySingleHitEffect;
		}
		StandardEffectInfo effectInfo = standardEffectInfo;
		StandardEffectInfo effectInfo2 = (!abilityMod_TrackerTranquilizerDarts) ? m_enemyMultiHitEffect : abilityMod_TrackerTranquilizerDarts.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect);
		StandardEffectInfo standardEffectInfo2;
		if ((bool)abilityMod_TrackerTranquilizerDarts)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			standardEffectInfo2 = abilityMod_TrackerTranquilizerDarts.m_allySingleHitEffectMod.GetModifiedValue(m_allySingleHitEffect);
		}
		else
		{
			standardEffectInfo2 = m_allySingleHitEffect;
		}
		StandardEffectInfo effectInfo3 = standardEffectInfo2;
		StandardEffectInfo standardEffectInfo3;
		if ((bool)abilityMod_TrackerTranquilizerDarts)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			standardEffectInfo3 = abilityMod_TrackerTranquilizerDarts.m_allyMultiHitEffectMod.GetModifiedValue(m_allyMultiHitEffect);
		}
		else
		{
			standardEffectInfo3 = m_allyMultiHitEffect;
		}
		StandardEffectInfo effectInfo4 = standardEffectInfo3;
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "Effect_EnemySingleHit");
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "Effect_EnemyMultiHit");
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo3, "Effect_AllySingleHit");
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo4, "Effect_AllyMultiHit");
		TrackerHuntingCrossbow component = GetComponent<TrackerHuntingCrossbow>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			component.m_huntedEffectData.AddTooltipTokens(tokens, "TrackedEffect");
			return;
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
		Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeter, targetActor, currentTargeterIndex, m_laserDamageAmount, m_laserDamageAmount);
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
		LaserTargetingInfo cachedLaserTargetingInfo;
		if ((bool)m_abilityMod)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			cachedLaserTargetingInfo = m_abilityMod.m_laserTargetingInfoMod.GetModifiedValue(m_laserTargetingInfo);
		}
		else
		{
			cachedLaserTargetingInfo = m_laserTargetingInfo;
		}
		m_cachedLaserTargetingInfo = cachedLaserTargetingInfo;
	}

	public LaserTargetingInfo GetLaserTargetingInfo()
	{
		return (m_cachedLaserTargetingInfo == null) ? m_laserTargetingInfo : m_cachedLaserTargetingInfo;
	}

	public StandardEffectInfo GetAllySingleHitEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_allySingleHitEffect;
		}
		else
		{
			result = m_abilityMod.m_allySingleHitEffectMod.GetModifiedValue(m_allySingleHitEffect);
		}
		return result;
	}

	public StandardEffectInfo GetAllyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_allyMultiHitEffect;
		}
		else
		{
			result = m_abilityMod.m_allyMultiHitEffectMod.GetModifiedValue(m_allyMultiHitEffect);
		}
		return result;
	}

	public StandardEffectInfo GetEnemySingleHitEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_enemySingleHitEffect;
		}
		else
		{
			result = m_abilityMod.m_enemySingleHitEffectMod.GetModifiedValue(m_enemySingleHitEffect);
		}
		return result;
	}

	public StandardEffectInfo GetEnemyMultiHitEffect()
	{
		StandardEffectInfo result;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_enemyMultiHitEffect;
		}
		else
		{
			result = m_abilityMod.m_enemyMultiHitEffectMod.GetModifiedValue(m_enemyMultiHitEffect);
		}
		return result;
	}

	private int GetLaserCount()
	{
		int num;
		if (m_abilityMod == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = m_laserCount;
		}
		else
		{
			num = m_abilityMod.m_laserCountMod.GetModifiedValue(m_laserCount);
		}
		int b = num;
		return Mathf.Max(1, b);
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		if (GetLaserCount() > 1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					min = m_targeterMinInterpDistance * Board.Get().squareSize;
					max = m_targeterMaxInterpDistance * Board.Get().squareSize;
					return true;
				}
			}
		}
		return base.HasRestrictedFreePosDistance(aimingActor, targetIndex, targetsSoFar, out min, out max);
	}
}
