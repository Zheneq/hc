using System;
using System.Collections.Generic;
using UnityEngine;

public class ExoDualCone : Ability
{
	[Header("-- Targeting")]
	public ConeTargetingInfo m_coneInfo;

	public float m_leftConeHorizontalOffset;

	public float m_rightConeHorizontalOffset;

	public float m_coneForwardOffset = 0.2f;

	[Space(10f)]
	public float m_leftConeDegreesFromForward;

	public float m_rightConeDegreesFromForward;

	[Header("-- Targeting, if interpolating angle")]
	public bool m_interpolateAngle;

	public float m_interpolateMinAngle;

	public float m_interpolateMaxAngle = 45f;

	public float m_interpolateMinDist = 0.75f;

	public float m_interpolateMaxDist = 3f;

	[Header("-- Damage")]
	public int m_damageAmount;

	public int m_extraDamageForOverlap;

	public int m_extraDamageForSingleHit;

	public StandardEffectInfo m_effectOnHit;

	public StandardEffectInfo m_effectOnOverlapHit;

	[Header("-- Extra Damage for using on consecutive turns (no longer requires hitting)")]
	public int m_extraDamageForConsecutiveUse;

	public int m_extraEnergyForConsecutiveUse;

	[Header("-- Sequences")]
	public GameObject m_projectileRightSequencePrefab;

	public GameObject m_projectileLeftSequencePrefab;

	private AbilityMod_ExoDualCone m_abilityMod;

	private Exo_SyncComponent m_syncComp;

	private ConeTargetingInfo m_cachedConeInfo;

	private StandardEffectInfo m_cachedEffectOnHit;

	private StandardEffectInfo m_cachedEffectOnOverlapHit;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Dual Cones";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Exo_SyncComponent>();
		}
		SetCachedFields();
		AbilityUtil_Targeter_TricksterCones abilityUtil_Targeter_TricksterCones = new AbilityUtil_Targeter_TricksterCones(this, GetConeInfo(), 2, GetNumCones, GetConeOrigins, GetConeDirections, GetFreePosForAim, false, false);
		abilityUtil_Targeter_TricksterCones.m_customDamageOriginDelegate = GetDamageOriginForTargeter;
		base.Targeter = abilityUtil_Targeter_TricksterCones;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeInfo().m_radiusInSquares;
	}

	private Vector3 GetDamageOriginForTargeter(AbilityTarget currentTarget, Vector3 defaultOrigin, ActorData actorToAdd, ActorData caster)
	{
		return caster.GetTravelBoardSquareWorldPosition();
	}

	public int GetNumCones()
	{
		return 2;
	}

	private void SetCachedFields()
	{
		m_cachedConeInfo = ((!m_abilityMod) ? m_coneInfo : m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo));
		StandardEffectInfo cachedEffectOnHit;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnHit = m_abilityMod.m_effectOnHitMod.GetModifiedValue(m_effectOnHit);
		}
		else
		{
			cachedEffectOnHit = m_effectOnHit;
		}
		m_cachedEffectOnHit = cachedEffectOnHit;
		StandardEffectInfo cachedEffectOnOverlapHit;
		if ((bool)m_abilityMod)
		{
			cachedEffectOnOverlapHit = m_abilityMod.m_effectOnOverlapHitMod.GetModifiedValue(m_effectOnOverlapHit);
		}
		else
		{
			cachedEffectOnOverlapHit = m_effectOnOverlapHit;
		}
		m_cachedEffectOnOverlapHit = cachedEffectOnOverlapHit;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		ConeTargetingInfo result;
		if (m_cachedConeInfo != null)
		{
			result = m_cachedConeInfo;
		}
		else
		{
			result = m_coneInfo;
		}
		return result;
	}

	public float GetLeftConeHorizontalOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_leftConeHorizontalOffsetMod.GetModifiedValue(m_leftConeHorizontalOffset);
		}
		else
		{
			result = m_leftConeHorizontalOffset;
		}
		return result;
	}

	public float GetRightConeHorizontalOffset()
	{
		return (!m_abilityMod) ? m_rightConeHorizontalOffset : m_abilityMod.m_rightConeHorizontalOffsetMod.GetModifiedValue(m_rightConeHorizontalOffset);
	}

	public float GetConeForwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneForwardOffsetMod.GetModifiedValue(m_coneForwardOffset);
		}
		else
		{
			result = m_coneForwardOffset;
		}
		return result;
	}

	public float GetLeftConeDegreesFromForward()
	{
		return (!m_abilityMod) ? m_leftConeDegreesFromForward : m_abilityMod.m_leftConeDegreesFromForwardMod.GetModifiedValue(m_leftConeDegreesFromForward);
	}

	public float GetRightConeDegreesFromForward()
	{
		return (!m_abilityMod) ? m_rightConeDegreesFromForward : m_abilityMod.m_rightConeDegreesFromForwardMod.GetModifiedValue(m_rightConeDegreesFromForward);
	}

	public bool InterpolateAngle()
	{
		return (!m_abilityMod) ? m_interpolateAngle : m_abilityMod.m_interpolateAngleMod.GetModifiedValue(m_interpolateAngle);
	}

	public float GetInterpolateMinAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_interpolateMinAngleMod.GetModifiedValue(m_interpolateMinAngle);
		}
		else
		{
			result = m_interpolateMinAngle;
		}
		return result;
	}

	public float GetInterpolateMaxAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_interpolateMaxAngleMod.GetModifiedValue(m_interpolateMaxAngle);
		}
		else
		{
			result = m_interpolateMaxAngle;
		}
		return result;
	}

	public float GetInterpolateMinDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_interpolateMinDistMod.GetModifiedValue(m_interpolateMinDist);
		}
		else
		{
			result = m_interpolateMinDist;
		}
		return result;
	}

	public float GetInterpolateMaxDist()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_interpolateMaxDistMod.GetModifiedValue(m_interpolateMaxDist);
		}
		else
		{
			result = m_interpolateMaxDist;
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

	public int GetExtraDamageForOverlap()
	{
		return (!m_abilityMod) ? m_extraDamageForOverlap : m_abilityMod.m_extraDamageForOverlapMod.GetModifiedValue(m_extraDamageForOverlap);
	}

	public int GetExtraDamageForSingleHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			result = m_extraDamageForSingleHit;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnHit()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnHit != null)
		{
			result = m_cachedEffectOnHit;
		}
		else
		{
			result = m_effectOnHit;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnOverlapHit()
	{
		StandardEffectInfo result;
		if (m_cachedEffectOnOverlapHit != null)
		{
			result = m_cachedEffectOnOverlapHit;
		}
		else
		{
			result = m_effectOnOverlapHit;
		}
		return result;
	}

	public int GetExtraDamageForConsecitiveHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageForConsecitiveHitMod.GetModifiedValue(m_extraDamageForConsecutiveUse);
		}
		else
		{
			result = m_extraDamageForConsecutiveUse;
		}
		return result;
	}

	public int GetExtraEnergyForConsecutiveUse()
	{
		return (!m_abilityMod) ? m_extraEnergyForConsecutiveUse : m_abilityMod.m_extraEnergyForConsecutiveUseMod.GetModifiedValue(m_extraEnergyForConsecutiveUse);
	}

	public Vector3 GetFreePosForAim(AbilityTarget currentTarget, ActorData caster)
	{
		return caster.GetLoSCheckPos() + currentTarget.AimDirection.normalized;
	}

	public List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetLoSCheckPos();
		Vector3 vector = targeterFreePos - travelBoardSquareWorldPositionForLos;
		vector.Normalize();
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		Vector3 a = travelBoardSquareWorldPositionForLos + GetConeForwardOffset() * vector;
		list.Add(a + normalized * GetRightConeHorizontalOffset());
		list.Add(a - normalized * GetLeftConeHorizontalOffset());
		return list;
	}

	public List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetLoSCheckPos();
		Vector3 vector = targeterFreePos - travelBoardSquareWorldPositionForLos;
		float num = GetLeftConeDegreesFromForward();
		float num2 = GetRightConeDegreesFromForward();
		if (InterpolateAngle())
		{
			float num3 = CalculateAngleFromCenter(currentTarget, base.ActorData);
			num = num3;
			num2 = num3;
		}
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		list.Add(Vector3.RotateTowards(vector, normalized, (float)Math.PI / 180f * num2, 0f).normalized);
		list.Add(Vector3.RotateTowards(vector, -1f * normalized, (float)Math.PI / 180f * num, 0f).normalized);
		return list;
	}

	private float CalculateAngleFromCenter(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float interpolateMinAngle = GetInterpolateMinAngle();
		float interpolateMaxAngle = GetInterpolateMaxAngle();
		float interpolateMinDist = GetInterpolateMinDist();
		float interpolateMaxDist = GetInterpolateMaxDist();
		if (interpolateMinDist < interpolateMaxDist && interpolateMinDist > 0f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					Vector3 vector = currentTarget.FreePos - targetingActor.GetTravelBoardSquareWorldPosition();
					vector.y = 0f;
					float value = vector.magnitude / Board.Get().squareSize;
					float num = Mathf.Clamp(value, interpolateMinDist, interpolateMaxDist) - interpolateMinDist;
					float num2 = 1f - num / (interpolateMaxDist - interpolateMinDist);
					float num3 = Mathf.Max(0f, interpolateMaxAngle - interpolateMinAngle);
					return interpolateMinAngle + num2 * num3;
				}
				}
			}
		}
		return interpolateMinAngle;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoDualCone abilityMod_ExoDualCone = modAsBase as AbilityMod_ExoDualCone;
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_ExoDualCone)
		{
			val = abilityMod_ExoDualCone.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			val = m_damageAmount;
		}
		AddTokenInt(tokens, "DamageAmount", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_ExoDualCone)
		{
			val2 = abilityMod_ExoDualCone.m_extraDamageForOverlapMod.GetModifiedValue(m_extraDamageForOverlap);
		}
		else
		{
			val2 = m_extraDamageForOverlap;
		}
		AddTokenInt(tokens, "ExtraDamageForOverlap", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_ExoDualCone)
		{
			val3 = abilityMod_ExoDualCone.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			val3 = m_extraDamageForSingleHit;
		}
		AddTokenInt(tokens, "ExtraDamageForSingleHit", empty3, val3);
		AddTokenInt(tokens, "TotalDamageOverlap", string.Empty, m_damageAmount + m_extraDamageForOverlap);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_ExoDualCone)
		{
			effectInfo = abilityMod_ExoDualCone.m_effectOnHitMod.GetModifiedValue(m_effectOnHit);
		}
		else
		{
			effectInfo = m_effectOnHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnHit", m_effectOnHit);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_ExoDualCone)
		{
			effectInfo2 = abilityMod_ExoDualCone.m_effectOnOverlapHitMod.GetModifiedValue(m_effectOnOverlapHit);
		}
		else
		{
			effectInfo2 = m_effectOnOverlapHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnOverlapHit", m_effectOnOverlapHit);
		AddTokenInt(tokens, "ExtraDamageForConsecitiveHit", string.Empty, (!abilityMod_ExoDualCone) ? m_extraDamageForConsecutiveUse : abilityMod_ExoDualCone.m_extraDamageForConsecitiveHitMod.GetModifiedValue(m_extraDamageForConsecutiveUse));
		string empty4 = string.Empty;
		int val4;
		if ((bool)abilityMod_ExoDualCone)
		{
			val4 = abilityMod_ExoDualCone.m_extraEnergyForConsecutiveUseMod.GetModifiedValue(m_extraEnergyForConsecutiveUse);
		}
		else
		{
			val4 = m_extraEnergyForConsecutiveUse;
		}
		AddTokenInt(tokens, "ExtraEnergyForConsecutiveUse", empty4, val4);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = GetDamageAmount();
		if (GetExtraDamageForConsecitiveHit() > 0)
		{
			if (m_syncComp != null)
			{
				if (m_syncComp.UsedBasicAttackLastTurn())
				{
					num += GetExtraDamageForConsecitiveHit();
				}
			}
		}
		int tooltipSubjectCountOnActor = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
		if (tooltipSubjectCountOnActor > 0)
		{
			if (tooltipSubjectCountOnActor == 1)
			{
				num += GetExtraDamageForSingleHit();
			}
			else if (tooltipSubjectCountOnActor > 1)
			{
				num += (tooltipSubjectCountOnActor - 1) * GetExtraDamageForOverlap();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (m_syncComp != null)
		{
			if (m_syncComp.UsedBasicAttackLastTurn())
			{
				if (GetExtraEnergyForConsecutiveUse() > 0)
				{
					int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
					return visibleActorsCountByTooltipSubject * GetExtraEnergyForConsecutiveUse();
				}
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoDualCone))
		{
			m_abilityMod = (abilityMod as AbilityMod_ExoDualCone);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
