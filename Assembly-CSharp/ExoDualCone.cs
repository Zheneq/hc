﻿// ROGUES
// SERVER
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
		Targeter = new AbilityUtil_Targeter_TricksterCones(
			this,
			GetConeInfo(),
			2,
			GetNumCones,
			GetConeOrigins,
			GetConeDirections,
			GetFreePosForAim,
			false,
			false)
		{
			m_customDamageOriginDelegate = GetDamageOriginForTargeter
		};
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
		return caster.GetFreePos();
	}

	public int GetNumCones()
	{
		return 2;
	}

	private void SetCachedFields()
	{
		m_cachedConeInfo = m_abilityMod != null
			? m_abilityMod.m_coneInfoMod.GetModifiedValue(m_coneInfo)
			: m_coneInfo;
		m_cachedEffectOnHit = m_abilityMod != null
			? m_abilityMod.m_effectOnHitMod.GetModifiedValue(m_effectOnHit)
			: m_effectOnHit;
		m_cachedEffectOnOverlapHit = m_abilityMod != null
			? m_abilityMod.m_effectOnOverlapHitMod.GetModifiedValue(m_effectOnOverlapHit)
			: m_effectOnOverlapHit;
	}

	public ConeTargetingInfo GetConeInfo()
	{
		return m_cachedConeInfo ?? m_coneInfo;
	}

	public float GetLeftConeHorizontalOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_leftConeHorizontalOffsetMod.GetModifiedValue(m_leftConeHorizontalOffset)
			: m_leftConeHorizontalOffset;
	}

	public float GetRightConeHorizontalOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_rightConeHorizontalOffsetMod.GetModifiedValue(m_rightConeHorizontalOffset)
			: m_rightConeHorizontalOffset;
	}

	public float GetConeForwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneForwardOffsetMod.GetModifiedValue(m_coneForwardOffset)
			: m_coneForwardOffset;
	}

	public float GetLeftConeDegreesFromForward()
	{
		return m_abilityMod != null
			? m_abilityMod.m_leftConeDegreesFromForwardMod.GetModifiedValue(m_leftConeDegreesFromForward)
			: m_leftConeDegreesFromForward;
	}

	public float GetRightConeDegreesFromForward()
	{
		return m_abilityMod != null
			? m_abilityMod.m_rightConeDegreesFromForwardMod.GetModifiedValue(m_rightConeDegreesFromForward)
			: m_rightConeDegreesFromForward;
	}

	public bool InterpolateAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_interpolateAngleMod.GetModifiedValue(m_interpolateAngle)
			: m_interpolateAngle;
	}

	public float GetInterpolateMinAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_interpolateMinAngleMod.GetModifiedValue(m_interpolateMinAngle)
			: m_interpolateMinAngle;
	}

	public float GetInterpolateMaxAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_interpolateMaxAngleMod.GetModifiedValue(m_interpolateMaxAngle)
			: m_interpolateMaxAngle;
	}

	public float GetInterpolateMinDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_interpolateMinDistMod.GetModifiedValue(m_interpolateMinDist)
			: m_interpolateMinDist;
	}

	public float GetInterpolateMaxDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_interpolateMaxDistMod.GetModifiedValue(m_interpolateMaxDist)
			: m_interpolateMaxDist;
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetExtraDamageForOverlap()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForOverlapMod.GetModifiedValue(m_extraDamageForOverlap)
			: m_extraDamageForOverlap;
	}

	public int GetExtraDamageForSingleHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit;
	}

	public StandardEffectInfo GetEffectOnHit()
	{
		return m_cachedEffectOnHit ?? m_effectOnHit;
	}

	public StandardEffectInfo GetEffectOnOverlapHit()
	{
		return m_cachedEffectOnOverlapHit ?? m_effectOnOverlapHit;
	}

	public int GetExtraDamageForConsecitiveHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageForConsecitiveHitMod.GetModifiedValue(m_extraDamageForConsecutiveUse)
			: m_extraDamageForConsecutiveUse;
	}

	public int GetExtraEnergyForConsecutiveUse()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyForConsecutiveUseMod.GetModifiedValue(m_extraEnergyForConsecutiveUse)
			: m_extraEnergyForConsecutiveUse;
	}

	public Vector3 GetFreePosForAim(AbilityTarget currentTarget, ActorData caster)
	{
		return caster.GetLoSCheckPos() + currentTarget.AimDirection.normalized;
	}

	public List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		Vector3 casterPos = caster.GetLoSCheckPos();
		Vector3 vector = targeterFreePos - casterPos;
		vector.Normalize();
		Vector3 right = Vector3.Cross(vector, Vector3.up).normalized;
		Vector3 start = casterPos + GetConeForwardOffset() * vector;
		return new List<Vector3>
		{
			start + right * GetRightConeHorizontalOffset(),
			start - right * GetLeftConeHorizontalOffset()
		};
	}

	public List<Vector3> GetConeDirections(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
	{
		Vector3 casterPos = caster.GetLoSCheckPos();
		Vector3 vector = targeterFreePos - casterPos;
		float leftDegrees = GetLeftConeDegreesFromForward();
		float rightDegrees = GetRightConeDegreesFromForward();
		if (InterpolateAngle())
		{
			float angleFromCenter = CalculateAngleFromCenter(currentTarget, ActorData);
			leftDegrees = angleFromCenter;
			rightDegrees = angleFromCenter;
		}
		Vector3 right = Vector3.Cross(vector, Vector3.up).normalized;
		return new List<Vector3>
		{
			Vector3.RotateTowards(vector, right, (float)Math.PI / 180f * rightDegrees, 0f).normalized,
			Vector3.RotateTowards(vector, -1f * right, (float)Math.PI / 180f * leftDegrees, 0f).normalized
		};
	}

	private float CalculateAngleFromCenter(AbilityTarget currentTarget, ActorData targetingActor)
	{
		float interpolateMinAngle = GetInterpolateMinAngle();
		float interpolateMaxAngle = GetInterpolateMaxAngle();
		float interpolateMinDist = GetInterpolateMinDist();
		float interpolateMaxDist = GetInterpolateMaxDist();
		if (interpolateMinDist < interpolateMaxDist && interpolateMinDist > 0f)
		{
			Vector3 vector = currentTarget.FreePos - targetingActor.GetFreePos();
			vector.y = 0f;
			float distInSquares = vector.magnitude / Board.Get().squareSize;
			float distAdd = Mathf.Clamp(distInSquares, interpolateMinDist, interpolateMaxDist) - interpolateMinDist;
			float share = 1f - distAdd / (interpolateMaxDist - interpolateMinDist);
			float range = Mathf.Max(0f, interpolateMaxAngle - interpolateMinAngle);
			return interpolateMinAngle + share * range;
		}
		return interpolateMinAngle;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ExoDualCone abilityMod_ExoDualCone = modAsBase as AbilityMod_ExoDualCone;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_ExoDualCone != null
			? abilityMod_ExoDualCone.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AddTokenInt(tokens, "ExtraDamageForOverlap", string.Empty, abilityMod_ExoDualCone != null
			? abilityMod_ExoDualCone.m_extraDamageForOverlapMod.GetModifiedValue(m_extraDamageForOverlap)
			: m_extraDamageForOverlap);
		AddTokenInt(tokens, "ExtraDamageForSingleHit", string.Empty, abilityMod_ExoDualCone != null
			? abilityMod_ExoDualCone.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit);
		AddTokenInt(tokens, "TotalDamageOverlap", string.Empty, m_damageAmount + m_extraDamageForOverlap);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ExoDualCone != null
			? abilityMod_ExoDualCone.m_effectOnHitMod.GetModifiedValue(m_effectOnHit)
			: m_effectOnHit, "EffectOnHit", m_effectOnHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_ExoDualCone != null
			? abilityMod_ExoDualCone.m_effectOnOverlapHitMod.GetModifiedValue(m_effectOnOverlapHit)
			: m_effectOnOverlapHit, "EffectOnOverlapHit", m_effectOnOverlapHit);
		AddTokenInt(tokens, "ExtraDamageForConsecitiveHit", string.Empty, abilityMod_ExoDualCone != null
			? abilityMod_ExoDualCone.m_extraDamageForConsecitiveHitMod.GetModifiedValue(m_extraDamageForConsecutiveUse)
			: m_extraDamageForConsecutiveUse);
		AddTokenInt(tokens, "ExtraEnergyForConsecutiveUse", string.Empty, abilityMod_ExoDualCone != null
			? abilityMod_ExoDualCone.m_extraEnergyForConsecutiveUseMod.GetModifiedValue(m_extraEnergyForConsecutiveUse)
			: m_extraEnergyForConsecutiveUse);
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
		int damage = GetDamageAmount();
		if (GetExtraDamageForConsecitiveHit() > 0
		    && m_syncComp != null
		    && m_syncComp.UsedBasicAttackLastTurn())
		{
			damage += GetExtraDamageForConsecitiveHit();
		}
		int primaryNum = Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Primary);
		if (primaryNum > 0)
		{
			if (primaryNum == 1)
			{
				damage += GetExtraDamageForSingleHit();
			}
			else if (primaryNum > 1)
			{
				damage += (primaryNum - 1) * GetExtraDamageForOverlap();
			}
			dictionary[AbilityTooltipSymbol.Damage] = damage;
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		return m_syncComp != null
		       && m_syncComp.UsedBasicAttackLastTurn()
		       && GetExtraEnergyForConsecutiveUse() > 0
			? Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) * GetExtraEnergyForConsecutiveUse()
			: 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ExoDualCone))
		{
			m_abilityMod = abilityMod as AbilityMod_ExoDualCone;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
	
#if SERVER
	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_lastBasicAttackUsedTurn = (short)GameFlowData.Get().CurrentTurn;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetHitActorsAndDamage(targets, caster, out List<List<ActorData>> actorsInCones, out List<VectorUtils.LaserCoords> laserCoordsList, null);
		Vector3 freePosForAim = GetFreePosForAim(targets[0], caster);
		List<Vector3> coneDirections = GetConeDirections(targets[0], freePosForAim, caster);
		List<GameObject> prefabs = new List<GameObject>
		{
			m_projectileRightSequencePrefab,
			m_projectileLeftSequencePrefab
		};
		for (int i = 0; i < laserCoordsList.Count; i++)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				prefabs[i],
				laserCoordsList[i].end,
				actorsInCones[i].ToArray(),
				caster,
				additionalData.m_sequenceSource,
				new BlasterStretchConeSequence.ExtraParams
				{
					lengthInSquares = GetConeInfo().m_radiusInSquares,
					angleInDegrees = GetConeInfo().m_widthAngleDeg,
					forwardAngle = VectorUtils.HorizontalAngle_Deg(coneDirections[i])
				}.ToArray()));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		Dictionary<ActorData, int> hitActorsAndDamage = GetHitActorsAndDamage(targets, caster, out List<List<ActorData>> actorsInCones, out _, nonActorTargetInfo);
		foreach (ActorData actorData in hitActorsAndDamage.Keys)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, caster.GetFreePos()));
			actorHitResults.SetBaseDamage(hitActorsAndDamage[actorData]);
			actorHitResults.AddStandardEffectInfo(GetEffectOnHit());
			if (GetEffectOnOverlapHit().m_applyEffect)
			{
				short num = 0;
				for (int i = 0; i < 2; i++)
				{
					if (actorsInCones[i].Contains(actorData))
					{
						num += 1;
					}
				}
				if (num > 1)
				{
					actorHitResults.AddStandardEffectInfo(GetEffectOnOverlapHit());
				}
			}
			if (m_syncComp != null && m_syncComp.UsedBasicAttackLastTurn() && GetExtraEnergyForConsecutiveUse() > 0)
			{
				actorHitResults.SetTechPointGainOnCaster(GetExtraEnergyForConsecutiveUse());
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private Dictionary<ActorData, int> GetHitActorsAndDamage(
		List<AbilityTarget> targets,
		ActorData caster,
		out List<List<ActorData>> actorsInCones,
		out List<VectorUtils.LaserCoords> endPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		actorsInCones = new List<List<ActorData>>();
		endPoints = new List<VectorUtils.LaserCoords>();
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		Dictionary<ActorData, int> dictionary2 = new Dictionary<ActorData, int>();
		Vector3 freePosForAim = GetFreePosForAim(targets[0], caster);
		List<Vector3> coneOrigins = GetConeOrigins(targets[0], freePosForAim, caster);
		List<Vector3> coneDirections = GetConeDirections(targets[0], freePosForAim, caster);
		ConeTargetingInfo coneInfo = GetConeInfo();
		for (int i = 0; i < 2; i++)
		{
			Vector3 vector = coneOrigins[i];
			Vector3 vector2 = coneDirections[i];
			vector2.y = 0f;
			vector2.Normalize();
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vector2);
			List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
				vector,
				coneCenterAngleDegrees,
				coneInfo.m_widthAngleDeg,
				coneInfo.m_radiusInSquares,
				coneInfo.m_backwardsOffset,
				coneInfo.m_penetrateLos,
				caster,
				TargeterUtils.GetRelevantTeams(caster, coneInfo.m_affectsAllies, coneInfo.m_affectsEnemies),
				nonActorTargetInfo);
			actorsInCones.Add(actorsInCone);
			float num = -1f;
			Vector3 end = vector + coneInfo.m_radiusInSquares * Board.Get().squareSize * vector2;
			foreach (ActorData actorData in actorsInCone)
			{
				if (!dictionary2.ContainsKey(actorData))
				{
					dictionary2[actorData] = 1;
				}
				else
				{
					dictionary2[actorData]++;
				}
				float num2 = Vector3.Distance(actorData.GetLoSCheckPos(), caster.GetLoSCheckPos());
				if (num2 > num)
				{
					num = num2;
					end = actorData.GetLoSCheckPos();
				}
			}
			VectorUtils.LaserCoords item;
			item.start = vector;
			item.end = end;
			endPoints.Add(item);
		}
		foreach (ActorData key2 in dictionary2.Keys)
		{
			int num3 = dictionary2[key2];
			int num4 = GetDamageAmount();
			if (GetExtraDamageForConsecitiveHit() > 0 && m_syncComp != null && m_syncComp.UsedBasicAttackLastTurn())
			{
				num4 += GetExtraDamageForConsecitiveHit();
			}
			if (num3 == 1)
			{
				num4 += GetExtraDamageForSingleHit();
			}
			else if (num3 > 1)
			{
				num4 += (num3 - 1) * GetExtraDamageForOverlap();
			}
			dictionary[key2] = num4;
		}
		return dictionary;
	}
#endif
}
