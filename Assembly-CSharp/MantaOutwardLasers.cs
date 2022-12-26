// ROGUES
// SERVER
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class MantaOutwardLasers : Ability
{
	[Header("-- Targeting")]
	public int m_numLasers = 5;
	public float m_totalAngleForLaserFan = 288f;
	public float m_width = 1f;
	public float m_maxDistancePerBounce = 15f;
	public float m_maxTotalDistance = 50f;
	public int m_maxBounces = 1;
	public int m_maxTargetsHit = 1;
	[Header("-- Damage")]
	public int m_damageAmount = 20;
	public int m_damageAmountForAdditionalHits = 10;
	public int m_bonusDamagePerBounce;
	public int m_techPointGainPerLaserHit;
	public StandardEffectInfo m_effectOnEnemy;
	public StandardEffectInfo m_effectForMultiHitsOnEnemy;
	[Tooltip("For when we want to apply 2 statuses that have different durations")]
	public StandardEffectInfo m_additionalEffectForMultiHitsOnEnemy;
	[Header("-- Sequences")]
	public GameObject m_projectileSequence;

	private StandardEffectInfo m_cachedEffectData;
	private StandardEffectInfo m_cachedMultiHitEffectData;
	private StandardEffectInfo m_cachedAdditionalMultiHitEffectData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Fissure Nova";
		}
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedEffectData = m_effectOnEnemy;
		m_cachedMultiHitEffectData = m_effectForMultiHitsOnEnemy;
		m_cachedAdditionalMultiHitEffectData = m_additionalEffectForMultiHitsOnEnemy;
	}

	public float GetFanAngle()
	{
		return m_totalAngleForLaserFan;
	}

	public int GetLaserCount()
	{
		return m_numLasers;
	}

	public int GetMaxBounces()
	{
		return m_maxBounces;
	}

	public int GetMaxTargetHits()
	{
		return m_maxTargetsHit;
	}

	public float GetLaserWidth()
	{
		return m_width;
	}

	public float GetDistancePerBounce()
	{
		return m_maxDistancePerBounce;
	}

	public float GetMaxTotalDistance()
	{
		return m_maxTotalDistance;
	}

	private StandardEffectInfo GetEnemyEffectData()
	{
		return m_cachedEffectData ?? m_effectOnEnemy;
	}

	private StandardEffectInfo GetMultiHitEnemyEffectData()
	{
		return m_cachedMultiHitEffectData ?? m_effectForMultiHitsOnEnemy;
	}

	private StandardEffectInfo GetAdditionalMultiHitEnemyEffectData()
	{
		return m_cachedAdditionalMultiHitEffectData ?? m_additionalEffectForMultiHitsOnEnemy;
	}

	public int GetBaseDamage()
	{
		return m_damageAmount;
	}

	public int GetDamageForAdditionalHit()
	{
		return m_damageAmountForAdditionalHits;
	}

	public int GetBonusDamagePerBounce()
	{
		return m_bonusDamagePerBounce;
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_FanOfBouncingLasers(
			this,
			GetFanAngle(),
			GetDistancePerBounce(),
			GetMaxTotalDistance(),
			GetLaserWidth(),
			GetMaxBounces(),
			GetMaxTargetHits(),
			GetLaserCount());
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetBaseDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		ReadOnlyCollection<AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext> hitActorContext =
			(Targeters[currentTargeterIndex] as AbilityUtil_Targeter_FanOfBouncingLasers).GetHitActorContext();
		foreach (AbilityUtil_Targeter_FanOfBouncingLasers.HitActorContext hit in hitActorContext)
		{
			if (hit.actor == targetActor)
			{
				int bonusDamage = GetBonusDamagePerBounce() * hit.segmentIndex;
				int firstHitDamage = GetBaseDamage() + bonusDamage;
				int additionalHitDamage = GetDamageForAdditionalHit() + bonusDamage;
				if (dictionary.ContainsKey(AbilityTooltipSymbol.Damage))
				{
					dictionary[AbilityTooltipSymbol.Damage] += additionalHitDamage;
				}
				else
				{
					dictionary[AbilityTooltipSymbol.Damage] = firstHitDamage;
				}
			}
		}
		return dictionary;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		return m_techPointGainPerLaserHit > 0
			? m_techPointGainPerLaserHit * Targeter.GetTooltipSubjectCountTotalWithDuplicates(AbilityTooltipSubject.Primary)
			: 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, modAsBase != null ? 0 : m_damageAmount);
		AddTokenInt(tokens, "DamageAdditionalHit", string.Empty, modAsBase != null ? 0 : m_damageAmountForAdditionalHits);
		AddTokenInt(tokens, "BonusDamagePerBounce", string.Empty, modAsBase != null ? 0 : m_bonusDamagePerBounce);
		AddTokenInt(tokens, "NumLasers", string.Empty, modAsBase != null ? 0 : m_numLasers);
		AddTokenInt(tokens, "MaxBounces", string.Empty, modAsBase != null ? 0 : m_maxBounces);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, modAsBase != null ? 0 : m_maxTargetsHit);
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		for (int i = 0; i < m_numLasers; i++)
		{
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets =
				FindLaserTargets(targets[0], i, caster, out List<Vector3> laserEndPoints, out _, null);
			if (i > 0)
			{
				for (int j = 0; j < laserEndPoints.Count; j++)
				{
					Vector3 value = laserEndPoints[j];
					value.y += 0.1f * i;
					laserEndPoints[j] = value;
				}
			}

			ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
				m_projectileSequence,
				caster.GetCurrentBoardSquare(),
				laserTargets.Keys.ToArray(),
				caster,
				additionalData.m_sequenceSource,
				new BouncingShotSequence.ExtraParams
				{
					laserTargets = laserTargets,
					segmentPts = laserEndPoints,
					doPositionHitOnBounce = true
				}.ToArray());
			list.Add(item);
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Dictionary<ActorData, ActorHitResults> dictionary = new Dictionary<ActorData, ActorHitResults>();
		Dictionary<ActorData, int> dictionary2 = new Dictionary<ActorData, int>();
		int num = 0;
		List<Barrier> list = new List<Barrier>();
		for (int i = 0; i < m_numLasers; i++)
		{
			List<List<NonActorTargetInfo>> list2 = new List<List<NonActorTargetInfo>>();
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets =
				FindLaserTargets(targets[0], i, caster, out List<Vector3> laserEndPoints, out List<ActorData> orderedHitActors, list2);
			foreach (ActorData actorData in orderedHitActors)
			{
				Vector3 segmentOrigin = laserTargets[actorData].m_segmentOrigin;
				int endpointIndex = laserTargets[actorData].m_endpointIndex;
				bool flag = dictionary.ContainsKey(actorData);
				int damage;
				if (flag)
				{
					damage = GetDamageForAdditionalHit();
					dictionary2[actorData]++;
				}
				else
				{
					damage = GetBaseDamage();
					dictionary2[actorData] = 1;
				}
				int num3 = GetBonusDamagePerBounce() * endpointIndex;
				damage += num3;
				if (flag)
				{
					dictionary[actorData].AddBaseDamage(damage);
				}
				else
				{
					ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, segmentOrigin));
					actorHitResults.SetBaseDamage(damage);
					actorHitResults.SetBounceCount(endpointIndex);
					if (endpointIndex > 0)
					{
						actorHitResults.SetIgnoreCoverMinDist(true);
					}
					dictionary[actorData] = actorHitResults;
				}
				dictionary[actorData].AddTechPointGainOnCaster(m_techPointGainPerLaserHit);
			}
			for (int j = 0; j < laserEndPoints.Count; j++)
			{
				Vector3 pos = laserEndPoints[j];
				if (num > 0)
				{
					pos.y += 0.1f * num;
				}
				List<NonActorTargetInfo> list5 = list2[j];
				for (int k = list5.Count - 1; k >= 0; k--)
				{
					NonActorTargetInfo nonActorTargetInfo = list5[k];
					if (nonActorTargetInfo is NonActorTargetInfo_BarrierBlock barrierBlock)
					{
						if (barrierBlock.m_barrier != null && !list.Contains(barrierBlock.m_barrier))
						{
							PositionHitResults posHitRes = new PositionHitResults(new PositionHitParameters(pos));
							barrierBlock.AddPositionReactionHitToAbilityResults(caster, posHitRes, abilityResults, true);
							list.Add(barrierBlock.m_barrier);
						}
						list5.RemoveAt(k);
					}
				}
			}
			foreach (List<NonActorTargetInfo> nonActorTargetInfo2 in list2)
			{
				abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo2);
			}
			num++;
		}
		foreach (ActorData key in dictionary2.Keys)
		{
			if (dictionary2[key] > 1)
			{
				dictionary[key].AddStandardEffectInfo(GetMultiHitEnemyEffectData());
				dictionary[key].AddStandardEffectInfo(GetAdditionalMultiHitEnemyEffectData());
			}
			else
			{
				dictionary[key].AddStandardEffectInfo(GetEnemyEffectData());
			}
		}
		foreach (ActorHitResults hitResults in dictionary.Values)
		{
			abilityResults.StoreActorHit(hitResults);
		}
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < m_numLasers; i++)
		{
			FindLaserTargets(targets[0], i, caster, out List<Vector3> laserEndPoints, out List<ActorData> orderedHitActors, null);
			list.AddRange(laserEndPoints);
			foreach (ActorData hitActor in orderedHitActors)
			{
				list.Add(hitActor.GetFreePos());
			}
		}
		foreach (AbilityTarget target in targets)
		{
			list.Add(target.FreePos);
		}
		return list;
	}

	// added in rogues
	private Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> FindLaserTargets(
		AbilityTarget targeter,
		int laserIndex,
		ActorData caster,
		out List<Vector3> laserEndPoints,
		out List<ActorData> orderedHitActors,
		List<List<NonActorTargetInfo>> nonActorTargetInfoInSegment)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
		Vector3 aimDirection = targeter.AimDirection;
		float totalAngleForLaserFan = m_totalAngleForLaserFan;
		float num = totalAngleForLaserFan / (m_numLasers - 1);
		Vector3 forwardDirection = VectorUtils.AngleDegreesToVector(VectorUtils.HorizontalAngle_Deg(aimDirection) - 0.5f * totalAngleForLaserFan + laserIndex * num);
		laserEndPoints = VectorUtils.CalculateBouncingLaserEndpoints(
			loSCheckPos,
			forwardDirection,
			GetDistancePerBounce(),
			GetMaxTotalDistance(),
			GetMaxBounces(),
			caster,
			GetLaserWidth(),
			GetMaxTargetHits(),
			true,
			caster.GetOtherTeams(),
			false,
			out var result,
			out orderedHitActors,
			nonActorTargetInfoInSegment);
		return result;
	}
#endif
}
