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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.Start()).MethodHandle;
			}
			this.m_abilityName = "Dual Cones";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		if (this.m_syncComp == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.SetupTargeter()).MethodHandle;
			}
			this.m_syncComp = base.GetComponent<Exo_SyncComponent>();
		}
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_TricksterCones(this, this.GetConeInfo(), 2, new AbilityUtil_Targeter_TricksterCones.GetCurrentNumberOfConesDelegate(this.GetNumCones), new AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate(this.GetConeOrigins), new AbilityUtil_Targeter_TricksterCones.GetConeInfoDelegate(this.GetConeDirections), new AbilityUtil_Targeter_TricksterCones.GetClampedTargetPosDelegate(this.GetFreePosForAim), false, false)
		{
			m_customDamageOriginDelegate = new AbilityUtil_Targeter_TricksterCones.DamageOriginDelegate(this.GetDamageOriginForTargeter)
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetConeInfo().m_radiusInSquares;
	}

	private Vector3 GetDamageOriginForTargeter(AbilityTarget currentTarget, Vector3 defaultOrigin, ActorData actorToAdd, ActorData caster)
	{
		return caster.\u0016();
	}

	public int GetNumCones()
	{
		return 2;
	}

	private void SetCachedFields()
	{
		this.m_cachedConeInfo = ((!this.m_abilityMod) ? this.m_coneInfo : this.m_abilityMod.m_coneInfoMod.GetModifiedValue(this.m_coneInfo));
		StandardEffectInfo cachedEffectOnHit;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.SetCachedFields()).MethodHandle;
			}
			cachedEffectOnHit = this.m_abilityMod.m_effectOnHitMod.GetModifiedValue(this.m_effectOnHit);
		}
		else
		{
			cachedEffectOnHit = this.m_effectOnHit;
		}
		this.m_cachedEffectOnHit = cachedEffectOnHit;
		StandardEffectInfo cachedEffectOnOverlapHit;
		if (this.m_abilityMod)
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
			cachedEffectOnOverlapHit = this.m_abilityMod.m_effectOnOverlapHitMod.GetModifiedValue(this.m_effectOnOverlapHit);
		}
		else
		{
			cachedEffectOnOverlapHit = this.m_effectOnOverlapHit;
		}
		this.m_cachedEffectOnOverlapHit = cachedEffectOnOverlapHit;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		ConeTargetingInfo result;
		if (this.m_cachedConeInfo != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetConeInfo()).MethodHandle;
			}
			result = this.m_cachedConeInfo;
		}
		else
		{
			result = this.m_coneInfo;
		}
		return result;
	}

	public float GetLeftConeHorizontalOffset()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetLeftConeHorizontalOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_leftConeHorizontalOffsetMod.GetModifiedValue(this.m_leftConeHorizontalOffset);
		}
		else
		{
			result = this.m_leftConeHorizontalOffset;
		}
		return result;
	}

	public float GetRightConeHorizontalOffset()
	{
		return (!this.m_abilityMod) ? this.m_rightConeHorizontalOffset : this.m_abilityMod.m_rightConeHorizontalOffsetMod.GetModifiedValue(this.m_rightConeHorizontalOffset);
	}

	public float GetConeForwardOffset()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetConeForwardOffset()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneForwardOffsetMod.GetModifiedValue(this.m_coneForwardOffset);
		}
		else
		{
			result = this.m_coneForwardOffset;
		}
		return result;
	}

	public float GetLeftConeDegreesFromForward()
	{
		return (!this.m_abilityMod) ? this.m_leftConeDegreesFromForward : this.m_abilityMod.m_leftConeDegreesFromForwardMod.GetModifiedValue(this.m_leftConeDegreesFromForward);
	}

	public float GetRightConeDegreesFromForward()
	{
		return (!this.m_abilityMod) ? this.m_rightConeDegreesFromForward : this.m_abilityMod.m_rightConeDegreesFromForwardMod.GetModifiedValue(this.m_rightConeDegreesFromForward);
	}

	public bool InterpolateAngle()
	{
		return (!this.m_abilityMod) ? this.m_interpolateAngle : this.m_abilityMod.m_interpolateAngleMod.GetModifiedValue(this.m_interpolateAngle);
	}

	public float GetInterpolateMinAngle()
	{
		float result;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetInterpolateMinAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_interpolateMinAngleMod.GetModifiedValue(this.m_interpolateMinAngle);
		}
		else
		{
			result = this.m_interpolateMinAngle;
		}
		return result;
	}

	public float GetInterpolateMaxAngle()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetInterpolateMaxAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_interpolateMaxAngleMod.GetModifiedValue(this.m_interpolateMaxAngle);
		}
		else
		{
			result = this.m_interpolateMaxAngle;
		}
		return result;
	}

	public float GetInterpolateMinDist()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetInterpolateMinDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_interpolateMinDistMod.GetModifiedValue(this.m_interpolateMinDist);
		}
		else
		{
			result = this.m_interpolateMinDist;
		}
		return result;
	}

	public float GetInterpolateMaxDist()
	{
		float result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetInterpolateMaxDist()).MethodHandle;
			}
			result = this.m_abilityMod.m_interpolateMaxDistMod.GetModifiedValue(this.m_interpolateMaxDist);
		}
		else
		{
			result = this.m_interpolateMaxDist;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public int GetExtraDamageForOverlap()
	{
		return (!this.m_abilityMod) ? this.m_extraDamageForOverlap : this.m_abilityMod.m_extraDamageForOverlapMod.GetModifiedValue(this.m_extraDamageForOverlap);
	}

	public int GetExtraDamageForSingleHit()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetExtraDamageForSingleHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			result = this.m_extraDamageForSingleHit;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnHit()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnHit != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetEffectOnHit()).MethodHandle;
			}
			result = this.m_cachedEffectOnHit;
		}
		else
		{
			result = this.m_effectOnHit;
		}
		return result;
	}

	public StandardEffectInfo GetEffectOnOverlapHit()
	{
		StandardEffectInfo result;
		if (this.m_cachedEffectOnOverlapHit != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetEffectOnOverlapHit()).MethodHandle;
			}
			result = this.m_cachedEffectOnOverlapHit;
		}
		else
		{
			result = this.m_effectOnOverlapHit;
		}
		return result;
	}

	public int GetExtraDamageForConsecitiveHit()
	{
		int result;
		if (this.m_abilityMod)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetExtraDamageForConsecitiveHit()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageForConsecitiveHitMod.GetModifiedValue(this.m_extraDamageForConsecutiveUse);
		}
		else
		{
			result = this.m_extraDamageForConsecutiveUse;
		}
		return result;
	}

	public int GetExtraEnergyForConsecutiveUse()
	{
		return (!this.m_abilityMod) ? this.m_extraEnergyForConsecutiveUse : this.m_abilityMod.m_extraEnergyForConsecutiveUseMod.GetModifiedValue(this.m_extraEnergyForConsecutiveUse);
	}

	public Vector3 GetFreePosForAim(AbilityTarget currentTarget, ActorData caster)
	{
		return caster.\u0015() + currentTarget.AimDirection.normalized;
	}

	public List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 vector = caster.\u0015();
		Vector3 vector2 = targeterFreePos - vector;
		vector2.Normalize();
		Vector3 normalized = Vector3.Cross(vector2, Vector3.up).normalized;
		Vector3 a = vector + this.GetConeForwardOffset() * vector2;
		list.Add(a + normalized * this.GetRightConeHorizontalOffset());
		list.Add(a - normalized * this.GetLeftConeHorizontalOffset());
		return list;
	}

	public List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 b = caster.\u0015();
		Vector3 vector = targeterFreePos - b;
		float num = this.GetLeftConeDegreesFromForward();
		float num2 = this.GetRightConeDegreesFromForward();
		if (this.InterpolateAngle())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetConeDirections(AbilityTarget, Vector3, ActorData)).MethodHandle;
			}
			float num3 = this.CalculateAngleFromCenter(currentTarget, base.ActorData);
			num = num3;
			num2 = num3;
		}
		Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
		list.Add(Vector3.RotateTowards(vector, normalized, 0.0174532924f * num2, 0f).normalized);
		list.Add(Vector3.RotateTowards(vector, -1f * normalized, 0.0174532924f * num, 0f).normalized);
		return list;
	}

	private float CalculateAngleFromCenter(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float interpolateMinAngle = this.GetInterpolateMinAngle();
		float interpolateMaxAngle = this.GetInterpolateMaxAngle();
		float interpolateMinDist = this.GetInterpolateMinDist();
		float interpolateMaxDist = this.GetInterpolateMaxDist();
		if (interpolateMinDist < interpolateMaxDist && interpolateMinDist > 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.CalculateAngleFromCenter(AbilityTarget, ActorData)).MethodHandle;
			}
			Vector3 vector = currentTarget.FreePos - targetingActor.\u0016();
			vector.y = 0f;
			float value = vector.magnitude / Board.\u000E().squareSize;
			float num = Mathf.Clamp(value, interpolateMinDist, interpolateMaxDist) - interpolateMinDist;
			float num2 = 1f - num / (interpolateMaxDist - interpolateMinDist);
			float num3 = Mathf.Max(0f, interpolateMaxAngle - interpolateMinAngle);
			return interpolateMinAngle + num2 * num3;
		}
		return interpolateMinAngle;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoDualCone abilityMod_ExoDualCone = modAsBase as AbilityMod_ExoDualCone;
		string name = "DamageAmount";
		string empty = string.Empty;
		int val;
		if (abilityMod_ExoDualCone)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.AddSpecificTooltipTokens(List<TooltipTokenEntry>, AbilityMod)).MethodHandle;
			}
			val = abilityMod_ExoDualCone.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			val = this.m_damageAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "ExtraDamageForOverlap";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_ExoDualCone)
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
			val2 = abilityMod_ExoDualCone.m_extraDamageForOverlapMod.GetModifiedValue(this.m_extraDamageForOverlap);
		}
		else
		{
			val2 = this.m_extraDamageForOverlap;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "ExtraDamageForSingleHit";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_ExoDualCone)
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
			val3 = abilityMod_ExoDualCone.m_extraDamageForSingleHitMod.GetModifiedValue(this.m_extraDamageForSingleHit);
		}
		else
		{
			val3 = this.m_extraDamageForSingleHit;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		base.AddTokenInt(tokens, "TotalDamageOverlap", string.Empty, this.m_damageAmount + this.m_extraDamageForOverlap, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_ExoDualCone)
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
			effectInfo = abilityMod_ExoDualCone.m_effectOnHitMod.GetModifiedValue(this.m_effectOnHit);
		}
		else
		{
			effectInfo = this.m_effectOnHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "EffectOnHit", this.m_effectOnHit, true);
		StandardEffectInfo effectInfo2;
		if (abilityMod_ExoDualCone)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			effectInfo2 = abilityMod_ExoDualCone.m_effectOnOverlapHitMod.GetModifiedValue(this.m_effectOnOverlapHit);
		}
		else
		{
			effectInfo2 = this.m_effectOnOverlapHit;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "EffectOnOverlapHit", this.m_effectOnOverlapHit, true);
		base.AddTokenInt(tokens, "ExtraDamageForConsecitiveHit", string.Empty, (!abilityMod_ExoDualCone) ? this.m_extraDamageForConsecutiveUse : abilityMod_ExoDualCone.m_extraDamageForConsecitiveHitMod.GetModifiedValue(this.m_extraDamageForConsecutiveUse), false);
		string name4 = "ExtraEnergyForConsecutiveUse";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_ExoDualCone)
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
			val4 = abilityMod_ExoDualCone.m_extraEnergyForConsecutiveUseMod.GetModifiedValue(this.m_extraEnergyForConsecutiveUse);
		}
		else
		{
			val4 = this.m_extraEnergyForConsecutiveUse;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetDamageAmount());
		return result;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		int num = this.GetDamageAmount();
		if (this.GetExtraDamageForConsecitiveHit() > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			if (this.m_syncComp != null)
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
				if (this.m_syncComp.UsedBasicAttackLastTurn())
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
					num += this.GetExtraDamageForConsecitiveHit();
				}
			}
		}
		int tooltipSubjectCountOnActor = base.Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
		if (tooltipSubjectCountOnActor > 0)
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
			if (tooltipSubjectCountOnActor == 1)
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
				num += this.GetExtraDamageForSingleHit();
			}
			else if (tooltipSubjectCountOnActor > 1)
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
				num += (tooltipSubjectCountOnActor - 1) * this.GetExtraDamageForOverlap();
			}
			dictionary[AbilityTooltipSymbol.Damage] = num;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.m_syncComp != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ExoDualCone.GetAdditionalTechPointGainForNameplateItem(ActorData, int)).MethodHandle;
			}
			if (this.m_syncComp.UsedBasicAttackLastTurn())
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
				if (this.GetExtraEnergyForConsecutiveUse() > 0)
				{
					int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
					return visibleActorsCountByTooltipSubject * this.GetExtraEnergyForConsecutiveUse();
				}
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoDualCone))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ExoDualCone);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}
}
