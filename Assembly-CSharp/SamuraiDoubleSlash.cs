using System;
using System.Collections.Generic;
using UnityEngine;

public class SamuraiDoubleSlash : Ability
{
	[Header("-- Targeting")]
	public bool m_penetrateLineOfSight;
	public bool m_coneFirstSlash;
	public bool m_coneSecondSlash;
	public float m_maxAngleBetween = 120f;
	[Header("    Cone(s)")]
	public float m_coneWidthAngle = 180f;
	public float m_coneBackwardOffset;
	public float m_coneLength = 2.5f;
	[Header("    Laser(s)")]
	public float m_laserWidth = 1.5f;
	public float m_laserLength = 2.5f;
	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;
	public int m_overlapExtraDamage = 15;
	public StandardEffectInfo m_targetHitEffect;
	[Header("-- Extra Effect if SelfBuff ability is used on same turn")]
	public StandardEffectInfo m_extraEnemyHitEffectIfSelfBuffed;
	[Header("-- Sequences")]
	public GameObject m_coneCastSequencePrefab;
	public GameObject m_laserCastSequencePrefab;

	private AbilityMod_SamuraiDoubleSlash m_abilityMod;
	private Samurai_SyncComponent m_syncComponent;
	private StandardEffectInfo m_cachedTargetHitEffect;
	private StandardEffectInfo m_cachedExtraEnemyHitEffectIfSelfBuffed;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Double Slash";
		}
		m_syncComponent = ActorData.GetComponent<Samurai_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			if (i == 0 && m_coneFirstSlash || i == 1 && m_coneSecondSlash)
			{
				AbilityUtil_Targeter_DirectionCone targeter = new AbilityUtil_Targeter_DirectionCone(
					this,
					GetConeWidthAngle(),
					GetConeLength(),
					GetConeBackwardOffset(),
					PenetrateLineOfSight(),
					true,
					true,
					false,
					false,
					-1,
					true);
				targeter.SetUseMultiTargetUpdate(true);
				targeter.m_getClampedAimDirection = GetTargeterClampedAimDirection;
				Targeters.Add(targeter);
			}
			else
			{
				AbilityUtil_Targeter_Laser targeter = new AbilityUtil_Targeter_Laser(
					this,
					GetLaserWidth(),
					GetLaserLength(),
					PenetrateLineOfSight());
				targeter.SetUseMultiTargetUpdate(true);
				targeter.m_getClampedAimDirection = GetTargeterClampedAimDirection;
				Targeters.Add(targeter);
			}
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
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
		return Mathf.Max(GetConeLength(), GetLaserLength());
	}

	private void SetCachedFields()
	{
		m_cachedTargetHitEffect = m_abilityMod != null
			? m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect)
			: m_targetHitEffect;
		m_cachedExtraEnemyHitEffectIfSelfBuffed = m_abilityMod != null
			? m_abilityMod.m_extraEnemyHitEffectIfSelfBuffedMod.GetModifiedValue(m_extraEnemyHitEffectIfSelfBuffed)
			: m_extraEnemyHitEffectIfSelfBuffed;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public float GetMaxAngleBetween()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxAngleBetweenMod.GetModifiedValue(m_maxAngleBetween)
			: m_maxAngleBetween;
	}

	public float GetConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
	}

	public float GetConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
			: m_laserWidth;
	}

	public float GetLaserLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserLength)
			: m_laserLength;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetOverlapExtraDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_overlapExtraDamageMod.GetModifiedValue(m_overlapExtraDamage)
			: m_overlapExtraDamage;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return m_cachedTargetHitEffect ?? m_targetHitEffect;
	}

	public StandardEffectInfo GetExtraEnemyHitEffectIfSelfBuffed()
	{
		return m_cachedExtraEnemyHitEffectIfSelfBuffed ?? m_extraEnemyHitEffectIfSelfBuffed;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", "damage in the cone", m_damageAmount);
		AddTokenInt(tokens, "Damage_Overlap", "damage if hit by both cones", m_damageAmount + m_overlapExtraDamage);
		AddTokenInt(tokens, "Cone_Angle", "angle of the damage cone", (int)m_coneWidthAngle);
		AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(m_coneLength));
		AbilityMod.AddToken_EffectInfo(tokens, m_targetHitEffect, "TargetHitEffect", m_targetHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, m_extraEnemyHitEffectIfSelfBuffed, "ExtraEnemyHitEffectIfSelfBuffed", m_extraEnemyHitEffectIfSelfBuffed);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount())
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			AddNameplateValueForOverlap(
				ref symbolToValue,
				Targeters[i],
				targetActor,
				currentTargeterIndex,
				GetDamageAmount(),
				GetOverlapExtraDamage());
		}
		if (m_syncComponent != null && symbolToValue.ContainsKey(AbilityTooltipSymbol.Damage))
		{
			symbolToValue[AbilityTooltipSymbol.Damage] += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
		}
		return symbolToValue;
	}

	public Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, Vector3 prevAimDir)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxAngleBetween = GetMaxAngleBetween();
		if (maxAngleBetween > 0f && maxAngleBetween < 360f)
		{
			aimDir = Vector3.RotateTowards(prevAimDir, aimDir, (float)Math.PI / 180f * maxAngleBetween, 0f);
		}
		return aimDir;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SamuraiDoubleSlash))
		{
			m_abilityMod = abilityMod as AbilityMod_SamuraiDoubleSlash;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
