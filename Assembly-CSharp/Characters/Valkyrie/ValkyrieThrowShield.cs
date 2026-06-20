// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

public class ValkyrieThrowShield : Ability
{
	[Header("-- Targeting")]
	public float m_width = 1f;
	public float m_maxDistancePerBounce = 15f;
	public float m_maxTotalDistance = 50f;
	public int m_maxBounces = 1;
	public int m_maxTargetsHit = 1;
	[Header("-- Damage")]
	public int m_damageAmount = 20;
	public int m_bonusDamagePerBounce;
	[Header("-- Knockback")]
	public float m_knockbackDistance;
	public KnockbackType m_knockbackType;
	[Header("-- Sequences")]
	public GameObject m_projectileSequence;
	
	private Valkyrie_SyncComponent m_syncComp;
	private AbilityMod_ValkyrieThrowShield m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Ricoshield";
		}
		m_syncComp = GetComponent<Valkyrie_SyncComponent>();
		SetupTargeter();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxDistancePerBounce();
	}

	public float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_widthMod.GetModifiedValue(m_width)
			: m_width;
	}

	public float GetMaxDistancePerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDistancePerBounceMod.GetModifiedValue(m_maxDistancePerBounce)
			: m_maxDistancePerBounce;
	}

	public float GetMaxTotalDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTotalDistanceMod.GetModifiedValue(m_maxTotalDistance)
			: m_maxTotalDistance;
	}

	public int GetMaxBounces()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxBouncesMod.GetModifiedValue(m_maxBounces)
			: m_maxBounces;
	}

	public int GetMaxTargetsHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsHitMod.GetModifiedValue(m_maxTargetsHit)
			: m_maxTargetsHit;
	}

	public bool BounceOnHitActor()
	{
		return m_abilityMod != null
		       && m_abilityMod.m_bounceOnHitActorMod.GetModifiedValue(false);
	}

	public int GetBaseDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetBonusDamagePerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bonusDamagePerBounceMod.GetModifiedValue(m_bonusDamagePerBounce)
			: m_bonusDamagePerBounce;
	}

	public int GetLessDamagePerTarget()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lessDamagePerTargetMod.GetModifiedValue(0)
			: 0;
	}

	public float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance)
			: m_knockbackDistance;
	}

	public float GetBonusKnockbackPerBounce()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bonusKnockbackDistancePerBounceMod.GetModifiedValue(0f)
			: 0f;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType)
			: m_knockbackType;
	}

	public int GetMaxKnockbackTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxKnockbackTargetsMod.GetModifiedValue(0)
			: 0;
	}

	public AbilityModCooldownReduction GetCooldownReductionOnLaserHitCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionOnLaserHitCaster
			: null;
	}

	public int GetExtraDamage()
	{
		return m_syncComp != null
			? m_syncComp.m_extraDamageNextShieldThrow
			: 0;
	}

	public float GetExtraKnockbackDistance(ActorData hitActor)
	{
		if (!(Targeter is AbilityUtil_Targeter_BounceLaser abilityUtil_Targeter_BounceLaser))
		{
			return 0f;
		}
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceLaser.GetHitActorContext();
		if (!hitActorContext.IsNullOrEmpty())
		{
			foreach (AbilityUtil_Targeter_BounceLaser.HitActorContext current in hitActorContext)
			{
				if (current.actor == hitActor)
				{
					return GetBonusKnockbackPerBounce() * current.segmentIndex;
				}
			}
		}
		return 0f;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ValkyrieThrowShield))
		{
			m_abilityMod = abilityMod as AbilityMod_ValkyrieThrowShield;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityUtil_Targeter_BounceLaser targeter = new AbilityUtil_Targeter_BounceLaser(
			this,
			GetLaserWidth(),
			GetMaxDistancePerBounce(),
			GetMaxTotalDistance(),
			GetMaxBounces(),
			GetMaxTargetsHit(),
			BounceOnHitActor());
		targeter.InitKnockbackData(
			GetKnockbackDistance(),
			GetKnockbackType(),
			GetMaxKnockbackTargets(),
			GetExtraKnockbackDistance);
		targeter.m_penetrateTargetsAndHitCaster = GetCooldownReductionOnLaserHitCaster() != null
		                                          && GetCooldownReductionOnLaserHitCaster().HasCooldownReduction();
		Targeter = targeter;
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
		ReadOnlyCollection<AbilityUtil_Targeter_BounceLaser.HitActorContext> hitActorContexts = (Targeters[currentTargeterIndex] as AbilityUtil_Targeter_BounceLaser).GetHitActorContext();
		for (int i = 0; i < hitActorContexts.Count; i++)
		{
			AbilityUtil_Targeter_BounceLaser.HitActorContext hitActorContext = hitActorContexts[i];
			if (hitActorContext.actor == targetActor)
			{
				dictionary[AbilityTooltipSymbol.Damage] =
					GetBaseDamage()
					+ GetBonusDamagePerBounce() * hitActorContext.segmentIndex
					+ GetExtraDamage()
					- i * GetLessDamagePerTarget();
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "MaxBounces", string.Empty, m_maxBounces);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "BonusDamagePerBounce", string.Empty, m_bonusDamagePerBounce);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Normal;
	}

#if SERVER
	//Added in rouges
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_extraDamageNextShieldThrow = 0;
		}
	}

	//Added in rouges
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		bool pierceTargetsToHitCaster = GetCooldownReductionOnLaserHitCaster() != null && GetCooldownReductionOnLaserHitCaster().HasCooldownReduction();
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets = FindLaserTargets(
			targets[0],
			caster,
			pierceTargetsToHitCaster,
			out _, // custom
			out List<Vector3> segmentPts,
			out _,
			null,
			out bool hitCaster);
		BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams
		{
			laserTargets = laserTargets,
			segmentPts = segmentPts
		};
		extraParams.segmentPts.Add(caster.GetLoSCheckPos());
		extraParams.doPositionHitOnBounce = true;
		list.Add(new ServerClientUtils.SequenceStartData(
			m_projectileSequence,
			caster.GetCurrentBoardSquare(), 
			laserTargets.Keys.ToArray(), 
			caster, 
			additionalData.m_sequenceSource,
			extraParams.ToArray()));
		if (hitCaster && pierceTargetsToHitCaster)
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				null,
				caster.GetCurrentBoardSquare(),
				new[] { caster },
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	//Added in rouges
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Dictionary<ActorData, ActorHitResults> dictionary = new Dictionary<ActorData, ActorHitResults>();
		List<Barrier> list = new List<Barrier>();
		List<List<NonActorTargetInfo>> nonActorTargetInfoInSegment = new List<List<NonActorTargetInfo>>();
		bool pierceTargetsToHitCaster = GetCooldownReductionOnLaserHitCaster() != null && GetCooldownReductionOnLaserHitCaster().HasCooldownReduction();
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> laserTargets = FindLaserTargets(
			targets[0],
			caster,
			pierceTargetsToHitCaster,
			out List<Vector3> laserEndPoints,
			out _, // custom
			out List<ActorData> orderedHitActors,
			nonActorTargetInfoInSegment,
			out bool hitCaster);
		float knockbackDistance = GetKnockbackDistance();
		int maxKnockbackTargets = GetMaxKnockbackTargets();
		for (int i = 0; i < orderedHitActors.Count; i++)
		{
			ActorData actorData = orderedHitActors[i];
			Vector3 segmentOrigin = laserTargets[actorData].m_segmentOrigin;
			int endpointIndex = laserTargets[actorData].m_endpointIndex;
			int damage = GetBaseDamage()
				+ GetBonusDamagePerBounce() * endpointIndex
				+ GetExtraDamage()
				- i * GetLessDamagePerTarget();
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, segmentOrigin));
			actorHitResults.SetBaseDamage(damage);
			actorHitResults.SetBounceCount(endpointIndex);
			if (endpointIndex > 0)
			{
				actorHitResults.SetIgnoreCoverMinDist(true);
			}
			float bonusKnockbackDistance = GetBonusKnockbackPerBounce() * endpointIndex;
			if ((knockbackDistance > 0f || bonusKnockbackDistance > 0f)
			    && (maxKnockbackTargets <= 0 || i < maxKnockbackTargets))
			{
				Vector3 aimDir = laserEndPoints[endpointIndex] - segmentOrigin;
				actorHitResults.AddKnockbackData(new KnockbackHitData(
					actorData,
					caster,
					GetKnockbackType(),
					aimDir,
					segmentOrigin,
					knockbackDistance + bonusKnockbackDistance));
			}
			dictionary[actorData] = actorHitResults;
		}
		for (int j = 0; j < laserEndPoints.Count; j++)
		{
			Vector3 pos = laserEndPoints[j];
			List<NonActorTargetInfo> nonActorTargetInfoForSegment = nonActorTargetInfoInSegment[j];
			for (int k = nonActorTargetInfoForSegment.Count - 1; k >= 0; k--)
			{
				NonActorTargetInfo nonActorTargetInfo = nonActorTargetInfoForSegment[k];
				if (nonActorTargetInfo is NonActorTargetInfo_BarrierBlock barrierBlock)
				{
					if (barrierBlock.m_barrier != null && !list.Contains(barrierBlock.m_barrier))
					{
						PositionHitResults posHitRes = new PositionHitResults(new PositionHitParameters(pos));
						barrierBlock.AddPositionReactionHitToAbilityResults(caster, posHitRes, abilityResults, true);
						list.Add(barrierBlock.m_barrier);
					}
					nonActorTargetInfoForSegment.RemoveAt(k);
				}
			}
		}
		foreach (List<NonActorTargetInfo> nonActorTargetInfo2 in nonActorTargetInfoInSegment)
		{
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo2);
		}
		foreach (ActorHitResults hitResults in dictionary.Values)
		{
			abilityResults.StoreActorHit(hitResults);
		}
		if (hitCaster && pierceTargetsToHitCaster)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetLoSCheckPos()));
			GetCooldownReductionOnLaserHitCaster().AppendCooldownMiscEvents(casterHitResults, true, 0, 0);
			abilityResults.StoreActorHit(casterHitResults);
		}
	}

	//Added in rouges
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		FindLaserTargets(
			targets[0],
			caster,
			false,
			out _, // custom
			out List<Vector3> laserEndPoints,
			out List<ActorData> orderedHitActors,
			null,
			out _);
		list.AddRange(laserEndPoints);
		foreach (ActorData hitActor in orderedHitActors)
		{
			list.Add(hitActor.GetFreePos());
		}
		foreach (AbilityTarget target in targets)
		{
			list.Add(target.FreePos);
		}
		return list;
	}

	//Added in rouges
	private Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> FindLaserTargets(
		AbilityTarget targeter, 
		ActorData caster, 
		bool pierceTargetsToHitCaster, 
		out List<Vector3> laserEndPoints, 
		out List<Vector3> laserEndPointsForAnimation, // custom
		out List<ActorData> orderedHitActors, 
		List<List<NonActorTargetInfo>> nonActorTargetInfoInSegment, 
		out bool hitCaster)
	{
		hitCaster = false;
		Vector3 loSCheckPos = caster.GetLoSCheckPos();
        laserEndPoints = VectorUtils.CalculateBouncingLaserEndpoints(
			loSCheckPos, 
			targeter.AimDirection, 
			GetMaxDistancePerBounce(), 
			GetMaxTotalDistance(), 
			GetMaxBounces(), 
			caster, 
			GetLaserWidth(), 
			GetMaxTargetsHit(), 
			true, 
			caster.GetOtherTeams(), 
			BounceOnHitActor(), 
			out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors, 
			out orderedHitActors, 
			nonActorTargetInfoInSegment, 
			pierceTargetsToHitCaster);
        laserEndPointsForAnimation = laserEndPoints; // custom
        if (pierceTargetsToHitCaster && laserEndPoints.Count > 1)
		{
			// custom
			int lastHitActor = Math.Min(GetMaxTargetsHit(), orderedHitActors.Count) - 1;
			int lastHitActorEndpoint = lastHitActor >= 0
				? bounceHitActors[orderedHitActors[lastHitActor]].m_endpointIndex
				: 0;
			List<Team> casterTeamAsList = caster.GetTeamAsList();
			for (int i = lastHitActorEndpoint; i < laserEndPoints.Count - 1; i++)
			{
				List<ActorData> hitActorsForReturn = GameWideData.Get().UseActorRadiusForLaser()
					? AreaEffectUtils.GetActorsInBoxByActorRadius(
						laserEndPoints[i],
						laserEndPoints[i + 1],
						GetLaserWidth(),
						false,
						caster,
						casterTeamAsList)
					: AreaEffectUtils.GetActorsInBox(
						laserEndPoints[i],
						laserEndPoints[i + 1],
						GetLaserWidth(),
						true,
						caster,
						casterTeamAsList);

				if (hitActorsForReturn.Contains(caster))
				{
					hitCaster = true;

					if (orderedHitActors.Count > 0)
					{
						laserEndPointsForAnimation = laserEndPoints.Take(i+1).ToList();
					}
					break;
				}
			}
			// rogues
			// float totalMaxDistanceInSquares = GetMaxTotalDistance() - (laserEndPoints[0] - loSCheckPos).magnitude / Board.Get().squareSize;
			// Vector3 normalized = (laserEndPoints[1] - laserEndPoints[0]).normalized;
			// VectorUtils.CalculateBouncingLaserEndpoints(
			// 	laserEndPoints[0],
			// 	normalized,
			// 	GetMaxDistancePerBounce(),
			// 	totalMaxDistanceInSquares,
			// 	GetMaxBounces(),
			// 	caster,
			// 	m_width,
			// 	0,
			// 	false,
			// 	caster.GetTeamAsList(),
			// 	BounceOnHitActor(),
			// 	out _,
			// 	out List<ActorData> hitActors,
			// 	null,
			// 	false,
			// 	false);
			// hitCaster = hitActors.Contains(caster);
		}
		return bounceHitActors;
	}

	//Added in rouges
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(
				FreelancerStats.ValkyrieStats.DamageDoneByThrowShieldAndKnockback,
				results.FinalDamage);
		}
	}
#endif
}
