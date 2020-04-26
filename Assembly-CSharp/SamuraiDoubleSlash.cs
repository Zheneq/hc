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
		m_syncComponent = base.ActorData.GetComponent<Samurai_SyncComponent>();
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			if (i == 0)
			{
				if (m_coneFirstSlash)
				{
					goto IL_004b;
				}
			}
			if (i == 1)
			{
				if (m_coneSecondSlash)
				{
					goto IL_004b;
				}
			}
			AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, GetLaserWidth(), GetLaserLength(), PenetrateLineOfSight());
			abilityUtil_Targeter_Laser.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_Laser.m_getClampedAimDirection = GetTargeterClampedAimDirection;
			base.Targeters.Add(abilityUtil_Targeter_Laser);
			continue;
			IL_004b:
			AbilityUtil_Targeter_DirectionCone abilityUtil_Targeter_DirectionCone = new AbilityUtil_Targeter_DirectionCone(this, GetConeWidthAngle(), GetConeLength(), GetConeBackwardOffset(), PenetrateLineOfSight(), true, true, false, false, -1, true);
			abilityUtil_Targeter_DirectionCone.SetUseMultiTargetUpdate(true);
			abilityUtil_Targeter_DirectionCone.m_getClampedAimDirection = GetTargeterClampedAimDirection;
			base.Targeters.Add(abilityUtil_Targeter_DirectionCone);
		}
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
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
		m_cachedTargetHitEffect = ((!m_abilityMod) ? m_targetHitEffect : m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect));
		StandardEffectInfo cachedExtraEnemyHitEffectIfSelfBuffed;
		if ((bool)m_abilityMod)
		{
			cachedExtraEnemyHitEffectIfSelfBuffed = m_abilityMod.m_extraEnemyHitEffectIfSelfBuffedMod.GetModifiedValue(m_extraEnemyHitEffectIfSelfBuffed);
		}
		else
		{
			cachedExtraEnemyHitEffectIfSelfBuffed = m_extraEnemyHitEffectIfSelfBuffed;
		}
		m_cachedExtraEnemyHitEffectIfSelfBuffed = cachedExtraEnemyHitEffectIfSelfBuffed;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		else
		{
			result = m_penetrateLineOfSight;
		}
		return result;
	}

	public float GetMaxAngleBetween()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxAngleBetweenMod.GetModifiedValue(m_maxAngleBetween);
		}
		else
		{
			result = m_maxAngleBetween;
		}
		return result;
	}

	public float GetConeWidthAngle()
	{
		return (!m_abilityMod) ? m_coneWidthAngle : m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public float GetConeLength()
	{
		return (!m_abilityMod) ? m_coneLength : m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength);
	}

	public float GetLaserWidth()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserWidthMod.GetModifiedValue(m_laserWidth);
		}
		else
		{
			result = m_laserWidth;
		}
		return result;
	}

	public float GetLaserLength()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_laserLengthMod.GetModifiedValue(m_laserLength);
		}
		else
		{
			result = m_laserLength;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public int GetOverlapExtraDamage()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_overlapExtraDamageMod.GetModifiedValue(m_overlapExtraDamage);
		}
		else
		{
			result = m_overlapExtraDamage;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedTargetHitEffect != null)
		{
			result = m_cachedTargetHitEffect;
		}
		else
		{
			result = m_targetHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetExtraEnemyHitEffectIfSelfBuffed()
	{
		StandardEffectInfo result;
		if (m_cachedExtraEnemyHitEffectIfSelfBuffed != null)
		{
			result = m_cachedExtraEnemyHitEffectIfSelfBuffed;
		}
		else
		{
			result = m_extraEnemyHitEffectIfSelfBuffed;
		}
		return result;
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
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount()));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		for (int i = 0; i <= currentTargeterIndex; i++)
		{
			Ability.AddNameplateValueForOverlap(ref symbolToValue, base.Targeters[i], targetActor, currentTargeterIndex, GetDamageAmount(), GetOverlapExtraDamage());
		}
		if (m_syncComponent != null)
		{
			if (symbolToValue.ContainsKey(AbilityTooltipSymbol.Damage))
			{
				symbolToValue[AbilityTooltipSymbol.Damage] += m_syncComponent.CalcExtraDamageFromSelfBuffAbility();
			}
		}
		return symbolToValue;
	}

	public Vector3 GetTargeterClampedAimDirection(Vector3 aimDir, Vector3 prevAimDir)
	{
		aimDir.y = 0f;
		aimDir.Normalize();
		float maxAngleBetween = GetMaxAngleBetween();
		if (maxAngleBetween > 0f)
		{
			if (maxAngleBetween < 360f)
			{
				aimDir = Vector3.RotateTowards(prevAimDir, aimDir, (float)Math.PI / 180f * maxAngleBetween, 0f);
			}
		}
		return aimDir;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SamuraiDoubleSlash))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_SamuraiDoubleSlash);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
