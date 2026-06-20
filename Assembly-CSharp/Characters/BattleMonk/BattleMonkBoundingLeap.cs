// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleMonkBoundingLeap : Ability
{
	[Header("-- On Hit Damage, Effect, etc")]
	public int m_damageAmount = 20;
	public int m_damageAfterFirstHit;
	[Space(10f)]
	public StandardEffectInfo m_targetEffect;
	public int m_cooldownOnHit = -1;
	[Separator("Chase On Hit Data")]
	public bool m_chaseHitActor;
	public StandardEffectInfo m_chaserEffect;
	[Separator("Bounce")]
	public float m_width = 1f;
	public float m_maxDistancePerBounce = 15f;
	public float m_maxTotalDistance = 50f;
	public int m_maxBounces = 1;
	public int m_maxTargetsHit = 1;
	public bool m_bounceOffEnemyActor;
	[Separator("Bounce Anim")]
	public float m_recoveryTime = 0.5f;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_sequenceOnCaster;

	private const bool c_penetrateLoS = false;

	private AbilityMod_BattleMonkBoundingLeap m_abilityMod;
#if SERVER
	private Passive_BattleMonk m_passive;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Bounding Leap";
		}
#if SERVER
		PassiveData component = GetComponent<PassiveData>();
		if (component != null)
		{
			m_passive = component.GetPassiveOfType(typeof(Passive_BattleMonk)) as Passive_BattleMonk;
		}
#endif
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_BounceActor(
			this,
			m_width,
			GetMaxDistancePerBounce(),
			GetMaxTotalDistance(),
			GetMaxBounces(),
			GetMaxTargets(),
			ShouldBounceOffEnemyActors(),
			IncludeAlliesInBetween(),
			GetModdedEffectForSelf() != null && GetModdedEffectForSelf().m_applyEffect);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxDistancePerBounce();
	}

	public int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public int GetDamageAfterFirstHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageAfterFirstHitMod.GetModifiedValue(m_damageAfterFirstHit)
			: m_damageAfterFirstHit;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxHitTargetsMod.GetModifiedValue(m_maxTargetsHit)
			: m_maxTargetsHit;
	}

	public bool ShouldBounceOffEnemyActors()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bounceOffEnemyActorMod.GetModifiedValue(m_bounceOffEnemyActor)
			: m_bounceOffEnemyActor;
	}

	public bool IncludeAlliesInBetween()
	{
		return m_abilityMod != null && m_abilityMod.m_hitAlliesInBetween.GetModifiedValue(false);
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyHitEffect
			: null;
	}

	public int GetHealAmountIfNotDamagedThisTurn()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healAmountIfNotDamagedThisTurn.GetModifiedValue(0)
			: 0;
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

	public int CalcDamageForOrderIndex(int hitOrder)
	{
		int damageAfterFirstHit = GetDamageAfterFirstHit();
		if (damageAfterFirstHit > 0 && hitOrder > 0)
		{
			return damageAfterFirstHit;
		}
		return GetDamageAmount();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
		if (GetAllyHitEffect() != null)
		{
			GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		}
		AppendTooltipNumbersFromBaseModEffects(ref numbers);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			AbilityUtil_Targeter_BounceActor abilityUtil_Targeter_BounceActor = Targeter as AbilityUtil_Targeter_BounceActor;
			if (abilityUtil_Targeter_BounceActor != null)
			{
				List<AbilityUtil_Targeter_BounceActor.HitActorContext> hitActorContext = abilityUtil_Targeter_BounceActor.GetHitActorContext();
				for (int i = 0; i < hitActorContext.Count; i++)
				{
					if (hitActorContext[i].actor == targetActor)
					{
						results.m_damage = CalcDamageForOrderIndex(i);
						break;
					}
				}
				return true;
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "DamageAfterFirstHit", string.Empty, m_damageAfterFirstHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_targetEffect, "TargetEffect", m_targetEffect);
		AddTokenInt(tokens, "CooldownOnHit", string.Empty, m_cooldownOnHit);
		AbilityMod.AddToken_EffectInfo(tokens, m_chaserEffect, "ChaserEffect", m_chaserEffect);
		AddTokenInt(tokens, "MaxBounces", string.Empty, m_maxBounces);
		AddTokenInt(tokens, "MaxTargetsHit", string.Empty, m_maxTargetsHit);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_BattleMonkBoundingLeap))
		{
			m_abilityMod = abilityMod as AbilityMod_BattleMonkBoundingLeap;
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
	public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return chargeSegments[chargeSegments.Length - 1].m_pos;
	}

	// added in rogues
	public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
	{
		return ServerEvadeUtils.GetChargeBestSquareTestDirection(chargeSegments);
	}

	// added in rogues
	public override bool GetChargeThroughInvalidSquares()
	{
		return true;
	}

	// added in rogues
	public override bool CanChargeThroughInvalidSquaresForDestination()
	{
		return false;
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] ProcessChargeDodge(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerEvadeUtils.ChargeInfo charge,
		List<ServerEvadeUtils.EvadeInfo> evades)
	{
		return ServerEvadeUtils.ProcessChargeDodgeForStopOnTargetHit(
			charge.m_chargeSegments[charge.m_chargeSegments.Length - 2].m_pos,
			targets,
			caster,
			charge,
			evades);
	}

	// added in rogues
	public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare pathDestinationAndEndPoints = GetPathDestinationAndEndPoints(targets, caster, out var endPoints);
		ServerEvadeUtils.ChargeSegment[] chargeSegmentForStopOnTargetHit =
			ServerEvadeUtils.GetChargeSegmentForStopOnTargetHit(
				caster,
				endPoints,
				pathDestinationAndEndPoints,
				m_recoveryTime);
		float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(chargeSegmentForStopOnTargetHit));
		foreach (ServerEvadeUtils.ChargeSegment segment in chargeSegmentForStopOnTargetHit)
		{
			if (segment.m_cycle == BoardSquarePathInfo.ChargeCycleType.Movement)
			{
				segment.m_segmentMovementSpeed = segmentMovementSpeed;
			}
		}
		return chargeSegmentForStopOnTargetHit;
	}

	// added in rogues
	public override BoardSquare GetIdealDestination(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return GetPathDestinationAndEndPoints(targets, caster, out _);
	}

	// added in rogues
	private BoardSquare GetPathDestinationAndEndPoints(List<AbilityTarget> targets, ActorData caster, out List<Vector3> endPoints)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<Vector3> bounceEndPoints = GetBounceEndPoints(
			targets,
			caster,
			loSCheckPos,
			out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bouncingLaserInfos,
			out List<ActorData> orderedHitActors,
			null);
		ServerEvadeUtils.RemoveInvalidChargeEndPositions(ref bounceEndPoints);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref bouncingLaserInfos);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref orderedHitActors);
		ServerEvadeUtils.GetLastSegmentInfo(loSCheckPos, bounceEndPoints, out Vector3 start, out Vector3 dir, out var length);
		float num2 = Mathf.Min(0.5f, length / 2f);
		Vector3 end = bounceEndPoints[bounceEndPoints.Count - 1] - dir * num2;
		BoardSquare result;
		if (GetMaxTargets() > 0 && bouncingLaserInfos.Count >= GetMaxTargets())
		{
			result = orderedHitActors[orderedHitActors.Count - 1].GetCurrentBoardSquare();
		}
		else
		{
			result = KnockbackUtils.GetLastValidBoardSquareInLine(start, end, true);
		}
		endPoints = bounceEndPoints;
		return result;
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_passive != null)
		{
			m_passive.m_chargeLastCastTurn = GameFlowData.Get().CurrentTurn;
		}
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors = GetBounceHitActors(
			targets,
			loSCheckPos,
			caster,
			out var bounceEndPoints,
			out _,
			null);
		BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams
		{
			laserTargets = bounceHitActors,
			segmentPts = bounceEndPoints
		};
		if (IncludeAlliesInBetween())
		{
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> alliesInPath = GetAlliesInPath(loSCheckPos, bounceEndPoints, caster);
			foreach (ActorData key in alliesInPath.Keys)
			{
				if (!extraParams.laserTargets.ContainsKey(key))
				{
					extraParams.laserTargets.Add(key, alliesInPath[key]);
				}
				else
				{
					Debug.LogError("BattleMonk Leap: Trying to add actor that already added");
				}
			}
		}
		list.Add(new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			bounceEndPoints[0],
			bounceHitActors.Keys.ToArray(),
			caster,
			additionalData.m_sequenceSource,
			new Sequence.IExtraSequenceParams[]
			{
				extraParams
			}));
		if (additionalData.m_abilityResults.HitActorList().Contains(caster))
		{
			list.Add(new ServerClientUtils.SequenceStartData(
				m_sequenceOnCaster,
				caster.GetFreePos(),
				caster.AsArray(),
				caster,
				additionalData.m_sequenceSource));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<List<NonActorTargetInfo>> nonActorTargetInfos = new List<List<NonActorTargetInfo>>();
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceHitActors = GetBounceHitActors(
			targets,
			loSCheckPos,
			caster,
			out var endPoints,
			out var orderedHitActors,
			nonActorTargetInfos);
		bool flag = false;
		bool flag2 = false;
		for (int i = 0; i < orderedHitActors.Count; i++)
		{
			ActorData actorData = orderedHitActors[i];
			bool flag3 = i == orderedHitActors.Count - 1;
			int amount = CalcDamageForOrderIndex(i);
			ActorHitParameters hitParams = new ActorHitParameters(actorData, bounceHitActors[actorData].m_segmentOrigin);
			ActorHitResults actorHitResults = new ActorHitResults(amount, HitActionType.Damage, m_targetEffect, hitParams);
			if (m_cooldownOnHit >= 0 && !flag)
			{
				AbilityData.ActionType actionTypeOfAbility = caster.GetAbilityData().GetActionTypeOfAbility(this);
				actorHitResults.AddMiscHitEvent(new MiscHitEventData_OverrideCooldown(actionTypeOfAbility, m_cooldownOnHit));
				flag = true;
			}
			if (m_chaseHitActor && flag3)
			{
				actorHitResults.AddMiscHitEvent(new MiscHitEventData(MiscHitEventType.CasterForceChaseTarget));
				ActorHitParameters hitParams2 = new ActorHitParameters(caster, caster.GetFreePos());
				ActorHitResults hitResults = new ActorHitResults(m_chaserEffect, hitParams2);
				abilityResults.StoreActorHit(hitResults);
				flag2 = true;
			}
			actorHitResults.SetBounceCount(bounceHitActors[actorData].m_endpointIndex);
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (IncludeAlliesInBetween())
		{
			Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> alliesInPath = GetAlliesInPath(loSCheckPos, endPoints, caster);
			foreach (ActorData actorData2 in alliesInPath.Keys)
			{
				ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(actorData2, alliesInPath[actorData2].m_segmentOrigin));
				actorHitResults2.AddStandardEffectInfo(GetAllyHitEffect());
				abilityResults.StoreActorHit(actorHitResults2);
			}
		}
		if (!flag2)
		{
			StandardEffectInfo moddedEffectForSelf = GetModdedEffectForSelf();
			if (moddedEffectForSelf != null && moddedEffectForSelf.m_applyEffect)
			{
				ActorHitResults hitResults2 = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
				abilityResults.StoreActorHit(hitResults2);
			}
		}
		foreach (List<NonActorTargetInfo> nonActorTargetInfo in nonActorTargetInfos)
		{
			abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		}
	}

	// added in rogues
	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 freePos = caster.GetFreePos(caster.GetSquareAtPhaseStart());
		GetBounceHitActors(targets, freePos, caster, out var bounceEndPoints, out var orderedHitActors, null);
		if (bounceEndPoints != null)
		{
			list.AddRange(bounceEndPoints);
		}
		if (orderedHitActors != null)
		{
			foreach (ActorData actorData in orderedHitActors)
			{
				list.Add(actorData.GetFreePos());
			}
		}
		foreach (AbilityTarget target in targets)
		{
			list.Add(target.FreePos);
		}
		return list;
	}

	// added in rogues
	private List<Vector3> GetBounceEndPoints(
		List<AbilityTarget> targets,
		ActorData caster,
		Vector3 startPos,
		out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceTargets,
		out List<ActorData> orderedHitActors,
		List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments)
	{
		bool bounceOnActors = ShouldBounceOffEnemyActors() && GetMaxTargets() != 1;
		return VectorUtils.CalculateBouncingActorEndpoints(
			startPos,
			targets[0].AimDirection,
			GetMaxDistancePerBounce(),
			GetMaxTotalDistance(),
			GetMaxBounces(),
			caster,
			bounceOnActors,
			m_width,
			caster.GetOtherTeams(),
			GetMaxTargets(),
			out bounceTargets,
			out orderedHitActors,
			true,
			nonActorTargetInfoInSegments);
	}

	// added in rogues
	private Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> GetBounceHitActors(
		List<AbilityTarget> targets,
		Vector3 startPos,
		ActorData caster,
		out List<Vector3> bounceEndPoints,
		out List<ActorData> orderedHitActors,
		List<List<NonActorTargetInfo>> nonActorTargetInfoInSegments)
	{
		bounceEndPoints = GetBounceEndPoints(
			targets,
			caster,
			startPos,
			out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> result,
			out orderedHitActors,
			nonActorTargetInfoInSegments);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref result);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref orderedHitActors);
		return result;
	}

	// added in rogues
	private Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> GetAlliesInPath(Vector3 startPos, List<Vector3> endPoints, ActorData caster)
	{
		List<ActorData> orderedHitActors = new List<ActorData>();
		Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> result = AreaEffectUtils.FindBouncingLaserTargets(
			startPos,
			ref endPoints,
			m_width,
			caster.GetTeamAsList(),
			-1,
			true,
			caster,
			orderedHitActors);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref result);
		return result;
	}

	// added in rogues
	public override void OnDodgedDamage(ActorData caster, int damageDodged)
	{
		caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.BattleMonkStats.DamageDealtPlusDodgedByCharge, damageDodged);
	}

	// added in rogues
	public override void OnExecutedActorHit_Ability(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.BattleMonkStats.DamageDealtPlusDodgedByCharge, results.FinalDamage);
		}
	}
#endif
}
