using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiWindBlade : Ability
{
	[Header("-- Targeting")]
	public float m_laserWidth = 0.6f;
	public float m_minRangeBeforeBend = 1f;
	public float m_maxRangeBeforeBend = 5.5f;
	public float m_maxTotalRange = 7.5f;
	public float m_maxBendAngle = 45f;
	public bool m_penetrateLoS;
	public int m_maxTargets = 1;
	[Header("-- Damage")]
	public int m_laserDamageAmount = 5;
	public int m_damageChangePerTarget;
	public StandardEffectInfo m_laserHitEffect;
	[Header("-- Shielding per enemy hit on start of Next Turn")]
	public int m_shieldingPerEnemyHitNextTurn;
	public int m_shieldingDuration = 1;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_SamuraiWindBlade m_abilityMod;
	private Samurai_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Wind Blade";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		m_syncComponent = ActorData.GetComponent<Samurai_SyncComponent>();
		ClearTargeters();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_BendingLaser targeter = new AbilityUtil_Targeter_BendingLaser(
				this,
				GetLaserWidth(),
				GetMinRangeBeforeBend(),
				GetMaxRangeBeforeBend(),
				GetMaxTotalRange(),
				GetMaxBendAngle(),
				PenetrateLoS(),
				GetMaxTargets());
			targeter.SetUseMultiTargetUpdate(true);
			Targeters.Add(targeter);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (!Targeters.IsNullOrEmpty())
		{
			AbilityUtil_Targeter_BendingLaser targeter = Targeters[0] as AbilityUtil_Targeter_BendingLaser;
			if (targeter.DidStopShort())
			{
				return 1;
			}
		}
		return 2;
	}

	public override bool ShouldAutoConfirmIfTargetingOnEndTurn()
	{
		return true;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxTotalRange();
	}

	private void SetCachedFields()
	{
		m_cachedLaserHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float GetMinRangeBeforeBend()
	{
		return m_abilityMod != null
			? m_abilityMod.m_minRangeBeforeBendMod.GetModifiedValue(m_minRangeBeforeBend)
			: m_minRangeBeforeBend;
	}

	public float GetMaxRangeBeforeBend()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxRangeBeforeBendMod.GetModifiedValue(m_maxRangeBeforeBend)
			: m_maxRangeBeforeBend;
	}

	public float GetMaxTotalRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTotalRangeMod.GetModifiedValue(m_maxTotalRange)
			: m_maxTotalRange;
	}

	public float GetMaxBendAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxBendAngleMod.GetModifiedValue(m_maxBendAngle)
			: m_maxBendAngle;
	}

	public bool PenetrateLoS()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLoSMod.GetModifiedValue(m_penetrateLoS)
			: m_penetrateLoS;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetLaserDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserDamageAmountMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public int GetDamageChangePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageChangePerTargetMod.GetModifiedValue(m_damageChangePerTarget)
			: m_damageChangePerTarget;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return m_cachedLaserHitEffect ?? m_laserHitEffect;
	}

	public int GetShieldingPerEnemyHitNextTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldingPerEnemyHitNextTurnMod.GetModifiedValue(m_shieldingPerEnemyHitNextTurn)
			: m_shieldingPerEnemyHitNextTurn;
	}

	public int GetShieldingDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_shieldingDurationMod.GetModifiedValue(m_shieldingDuration)
			: m_shieldingDuration;
	}

	public int CalcDamage(int hitOrder)
	{
		int damage = GetLaserDamageAmount();
		if (GetDamageChangePerTarget() > 0 && hitOrder > 0)
		{
			damage += GetDamageChangePerTarget() * hitOrder;
		}
		return damage;
	}

	public int GetHitOrderIndexFromTargeters(ActorData actor, int currentTargetIndex)
	{
		int num = 0;
		if (Targeters != null)
		{
			for (int i = 0; i < Targeters.Count && i <= currentTargetIndex; i++)
			{
				if (Targeters[i] is AbilityUtil_Targeter_BendingLaser targeter)
				{
					foreach (ActorData current in targeter.m_ordererdHitActors)
					{
						if (current == actor)
						{
							return num;
						}
						num++;
					}
				}
			}
		}
		return -1;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "LaserDamageAmount", string.Empty, m_laserDamageAmount);
		AddTokenInt(tokens, "DamageChangePerTarget", string.Empty, m_damageChangePerTarget);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
		AddTokenInt(tokens, "ShieldingPerEnemyHitNextTurn", string.Empty, m_shieldingPerEnemyHitNextTurn);
		AddTokenInt(tokens, "ShieldingDuration", string.Empty, m_shieldingDuration);
	}

	private Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, List<AbilityTarget> targets)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxBendAngle = GetMaxBendAngle();
		Vector3 aimDirection = targets[0].AimDirection;
		if (maxBendAngle > 0f && maxBendAngle < 360f)
		{
			aimDir = Vector3.RotateTowards(aimDirection, aimDir, (float)Math.PI / 180f * maxBendAngle, 0f);
		}
		return aimDir;
	}

	private float GetClampedRangeInSquares(ActorData targetingActor, AbilityTarget currentTarget)
	{
		float dist = (currentTarget.FreePos - targetingActor.GetLoSCheckPos()).magnitude;
		if (dist < GetMinRangeBeforeBend() * Board.Get().squareSize)
		{
			return GetMinRangeBeforeBend();
		}
		if (dist > GetMaxRangeBeforeBend() * Board.Get().squareSize)
		{
			return GetMaxRangeBeforeBend();
		}
		return dist / Board.Get().squareSize;
	}

	private float GetDistanceRemaining(ActorData targetingActor, AbilityTarget previousTarget, out Vector3 bendPos)
	{
		float clampedRangeInSquares = GetClampedRangeInSquares(targetingActor, previousTarget);
		bendPos = targetingActor.GetLoSCheckPos() + previousTarget.AimDirection * clampedRangeInSquares * Board.Get().squareSize;
		return GetMaxTotalRange() - clampedRangeInSquares;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		if (m_laserDamageAmount > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_laserDamageAmount);
		}
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (m_laserDamageAmount > 0)
		{
			int damage = GetLaserDamageAmount();
			if (GetDamageChangePerTarget() > 0)
			{
				int hitOrderIndexFromTargeters = GetHitOrderIndexFromTargeters(targetActor, currentTargeterIndex);
				damage = CalcDamage(hitOrderIndexFromTargeters);
			}
			if (m_syncComponent != null)
			{
				damage += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiWindBlade))
		{
			m_abilityMod = abilityMod as AbilityMod_SamuraiWindBlade;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
