// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithAnvilSlam : Ability
{
	public int m_dashDamageAmount = 5;
	public float m_dashMaxDistance = 5f;
	public float m_dashWidth = 1f;
	public StandardEffectInfo m_dashEffectOnHit;
	public float m_dashRecoveryTime = 0.5f;
	[Header("-- additional bolt info now specified in NanoSmithBoltInfoComponent")]
	public int m_boltCount = 8;
	public float m_boltAngleOffset;
	public bool m_boltAngleRelativeToAim;

	private NanoSmithBoltInfo m_boltInfo;

	[Header("-- Sequences -----------------------------------------------")]
	public GameObject m_slamSequencePrefab;
	public GameObject m_boltSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Anvil Slam";
		}
		NanoSmithBoltInfoComponent component = GetComponent<NanoSmithBoltInfoComponent>();
		if (component != null)
		{
			m_boltInfo = component.m_boltInfo.GetShallowCopy();
			if (component.m_anvilSlamRangeOverride > 0f)
			{
				m_boltInfo.range = component.m_anvilSlamRangeOverride;
			}
		}
		else
		{
			Debug.LogError("No bolt info component found for NanoSmith ability");
			m_boltInfo = new NanoSmithBoltInfo();
		}
		ResetTooltipAndTargetingNumbers();
		Targeter = new AbilityUtil_Targeter_AnvilSlam(
			this,
			m_dashWidth,
			m_dashMaxDistance,
			m_boltCount,
			m_boltAngleRelativeToAim,
			m_boltAngleOffset,
			m_boltInfo);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_dashDamageAmount);
		m_dashEffectOnHit.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		if (m_boltCount > 0 && m_boltInfo != null)
		{
			m_boltInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
		}
		return numbers;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
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
		List<Vector3> endPoints;
		BoardSquare pathDestinationAndEndPoints = GetPathDestinationAndEndPoints(targets, caster, out endPoints);
		ServerEvadeUtils.ChargeSegment[] chargeSegmentForStopOnTargetHit =
			ServerEvadeUtils.GetChargeSegmentForStopOnTargetHit(caster, endPoints, pathDestinationAndEndPoints, 0.5f);
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
	private BoardSquare GetPathDestinationAndEndPoints(List<AbilityTarget> targets, ActorData caster, out List<Vector3> endPoints)
	{
		Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<Vector3> bounceEndPoints = GetBounceEndPoints(
			targets,
			caster,
			loSCheckPos,
			out var bounceTargets,
			out var orderedHitActors);
		ServerEvadeUtils.RemoveInvalidChargeEndPositions(ref bounceEndPoints);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref bounceTargets);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref orderedHitActors);
		ServerEvadeUtils.GetLastSegmentInfo(
			loSCheckPos,
			bounceEndPoints,
			out var start,
			out var vector,
			out var lastSegLength);
		float num2 = Mathf.Min(0.5f, lastSegLength / 2f);
		Vector3 end = bounceEndPoints[bounceEndPoints.Count - 1] - vector * num2;
		BoardSquare lastValidBoardSquareInLine = KnockbackUtils.GetLastValidBoardSquareInLine(start, end, true);
		endPoints = bounceEndPoints;
		return lastValidBoardSquareInLine;
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
	private List<Vector3> GetBounceEndPoints(
		List<AbilityTarget> targets,
		ActorData caster,
		Vector3 startPos,
		out Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo> bounceTargets,
		out List<ActorData> orderedHitActors)
	{
		return VectorUtils.CalculateBouncingActorEndpoints(
			startPos,
			targets[0].AimDirection,
			m_dashMaxDistance,
			m_dashMaxDistance,
			0,
			caster,
			false,
			m_dashWidth,
			caster.GetOtherTeams(),
			1,
			out bounceTargets,
			out orderedHitActors,
			true,
			null);
	}

	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		ActorData chargeHitTarget = GetChargeHitTarget(targets, caster, out var laserCoords, null);
		ActorData[] targetActorArray = chargeHitTarget != null
			? new[] { chargeHitTarget }
			: new ActorData[0];
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(
			m_slamSequencePrefab, laserCoords.end, targetActorArray, caster, additionalData.m_sequenceSource);
		list.Add(item);
		if (chargeHitTarget != null)
		{
			m_boltInfo.GetRadialBoltHitActors(
				caster,
				RunPriority,
				chargeHitTarget.GetLoSCheckPos(),
				m_boltCount,
				m_boltAngleRelativeToAim,
				targets[0].AimDirection,
				m_boltAngleOffset,
				out var sequenceActors,
				out var sequenceEndPoints,
				null);
			m_boltInfo.AddSequenceStartDataForBolts(
				m_boltSequencePrefab,
				caster,
				additionalData.m_sequenceSource,
				sequenceActors,
				sequenceEndPoints,
				ref list);
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		ActorData chargeHitTarget = GetChargeHitTarget(targets, caster, out var laserCoords, nonActorTargetInfo);
		if (chargeHitTarget != null)
		{
			ActorHitResults actorHitResults = new ActorHitResults(
				new ActorHitParameters(chargeHitTarget, caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart())));
			actorHitResults.SetBaseDamage(m_dashDamageAmount);
			actorHitResults.AddStandardEffectInfo(m_dashEffectOnHit);
			abilityResults.StoreActorHit(actorHitResults);
			List<ActorData> radialBoltHitActors = m_boltInfo.GetRadialBoltHitActors(
				caster,
				RunPriority,
				chargeHitTarget.GetLoSCheckPos(),
				m_boltCount,
				m_boltAngleRelativeToAim,
				targets[0].AimDirection,
				m_boltAngleOffset,
				out _,
				out _,
				nonActorTargetInfo);
			if (radialBoltHitActors.Contains(chargeHitTarget))
			{
				radialBoltHitActors.Remove(chargeHitTarget);
			}
			foreach (ActorData actorData in radialBoltHitActors)
			{
				ActorHitResults actorHitResults2 = new ActorHitResults(
					new ActorHitParameters(actorData, chargeHitTarget.GetLoSCheckPos()));
				if (actorData.GetTeam() != caster.GetTeam())
				{
					actorHitResults2.SetBaseDamage(m_boltInfo.damageAmount);
					actorHitResults2.AddStandardEffectInfo(m_boltInfo.effectOnEnemyHit);
				}
				else if (m_boltInfo.effectOnAllyHit.m_applyEffect)
				{
					actorHitResults2.AddStandardEffectInfo(m_boltInfo.effectOnAllyHit);
				}
				abilityResults.StoreActorHit(actorHitResults2);
			}
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private ActorData GetChargeHitTarget(
		List<AbilityTarget> targets,
		ActorData caster,
		out VectorUtils.LaserCoords endPoints,
		List<NonActorTargetInfo> nonActorTargetInfo)
	{
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			laserCoords.start,
			targets[0].AimDirection,
			m_dashMaxDistance,
			m_dashWidth,
			caster,
			caster.GetOtherTeams(),
			false,
			-1,
			false,
			true,
			out laserCoords.end,
			nonActorTargetInfo);
		ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInLaser);
		endPoints = TargeterUtils.TrimTargetsAndGetLaserCoordsToFarthestTarget(ref actorsInLaser, 1, laserCoords);
		return actorsInLaser.Count > 0
			? actorsInLaser[0]
			: null;
	}
#endif
}
