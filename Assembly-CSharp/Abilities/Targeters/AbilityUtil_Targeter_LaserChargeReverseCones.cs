using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_LaserChargeReverseCones : AbilityUtil_Targeter
{
	public delegate List<Vector3> GetConeInfoDelegate(AbilityTarget currentTarget, Vector3 freeTargetPos, ActorData caster);
	public delegate bool ConeLosCheckerDelegate(ActorData actor, ActorData caster, Vector3 chargeEndPos, List<NonActorTargetInfo> nonActorTargetInfo);
	public delegate bool IncludeCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);

	private float m_dashWidthInSquares;
	public float m_dashRangeInSquares;
	private ConeTargetingInfo m_coneInfo;
	private int m_numCones;
	private float m_coneStartOffset;
	private float m_perConeHorizontalOffset;
	private float m_angleInBetween;
	private GetConeInfoDelegate GetConeOrigins;
	private GetConeInfoDelegate GetConeDirections;
	public ConeLosCheckerDelegate m_coneLosCheckDelegate;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;
	private SquareInsideChecker_Box m_laserLosChecker;
	private SquareInsideChecker_Path m_pathLosChecker;
	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();
	public IncludeCasterDelegate m_includeCasterDelegate;

	public AbilityUtil_Targeter_LaserChargeReverseCones(
		Ability ability,
		float dashWidth,
		float dashRange,
		ConeTargetingInfo coneInfo,
		int numCones,
		float coneStartOffset,
		float perConeHorizontalOffset,
		float angleInBetween,
		GetConeInfoDelegate coneOriginsDelegate,
		GetConeInfoDelegate coneDirectionsDelegate)
		: base(ability)
	{
		m_dashWidthInSquares = dashWidth;
		m_dashRangeInSquares = dashRange;
		m_coneInfo = coneInfo;
		m_numCones = numCones;
		m_coneStartOffset = coneStartOffset;
		m_perConeHorizontalOffset = perConeHorizontalOffset;
		m_angleInBetween = angleInBetween;
		GetConeOrigins = coneOriginsDelegate;
		GetConeDirections = coneDirectionsDelegate;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_laserLosChecker = new SquareInsideChecker_Box(dashWidth)
		{
			m_penetrateLos = false
		};
		m_squarePosCheckerList.Add(m_laserLosChecker);
		m_pathLosChecker = new SquareInsideChecker_Path();
		m_squarePosCheckerList.Add(m_pathLosChecker);
		for (int i = 0; i < m_numCones; i++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		ClearActorsInRange();
		if (m_highlights == null || m_highlights.Count <= 1)
		{
			m_highlights = new List<GameObject>();
			m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
			for (int i = 0; i < m_numCones; i++)
			{
				m_highlights.Add(
					HighlightUtils.Get().CreateDynamicConeMesh(
						m_coneInfo.m_radiusInSquares,
						m_coneInfo.m_widthAngleDeg,
						false));
			}
		}

		Vector3 losCheckPos = targetingActor.GetLoSCheckPos();
		Vector3 laserEndPos = VectorUtils.GetLaserEndPoint(
			losCheckPos,
			currentTarget.AimDirection,
			m_dashRangeInSquares * Board.Get().squareSize,
			false,
			targetingActor);
		float dist = (laserEndPos - losCheckPos).magnitude;
		dist = ClaymoreCharge.GetMaxPotentialChargeDistance(
			losCheckPos,
			laserEndPos,
			currentTarget.AimDirection,
			dist,
			targetingActor,
			out BoardSquare pathEndSquare);
		BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(
			targetingActor,
			pathEndSquare,
			targetingActor.GetCurrentBoardSquare(),
			true);
		List<ActorData> actorsOnPath = ClaymoreCharge.GetActorsOnPath(path, targetingActor.GetEnemyTeamAsList(), targetingActor);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsOnPath);
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
			losCheckPos,
			currentTarget.AimDirection,
			dist / Board.Get().squareSize,
			m_dashWidthInSquares,
			targetingActor,
			targetingActor.GetEnemyTeamAsList(),
			false,
			1,
			true,
			false,
			out laserEndPos,
			null);
		actorsInLaser.AddRange(actorsOnPath);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, losCheckPos);
		ActorData directHitActor = null;
		if (actorsInLaser.Count > 0)
		{
			directHitActor = actorsInLaser[0];
			Vector3 directHitActorPos = directHitActor.GetFreePos();
			AddActorInRange(directHitActor, directHitActorPos, targetingActor);
			Vector3 lhs = directHitActorPos - losCheckPos;
			lhs.y = 0f;
			laserEndPos = losCheckPos + Vector3.Dot(lhs, currentTarget.AimDirection) * currentTarget.AimDirection;
			ActorHitContext actorHitContext = m_actorContextVars[directHitActor];
			actorHitContext.m_contextVars.SetValue(TargetSelect_LaserChargeWithReverseCones.s_cvarDirectChargeHit.GetKey(), 1);
		}
		m_laserLosChecker.UpdateBoxProperties(losCheckPos, laserEndPos, targetingActor);
		Vector3 a = laserEndPos - losCheckPos;
		a.y = 0f;
		float magnitude2 = a.magnitude;
		a.Normalize();
		Vector3 destPos = laserEndPos - Mathf.Min(0.5f, magnitude2 / 2f) * a;
		HighlightUtils.Get()
			.RotateAndResizeRectangularCursor(
				m_highlights[0],
				losCheckPos,
				destPos,
				m_dashWidthInSquares);
		BoardSquare destSquare = ClaymoreCharge.GetChargeDestinationSquare(
			losCheckPos,
			destPos,
			directHitActor,
			null,
			targetingActor,
			false);
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
			targetingActor,
			destSquare,
			targetingActor.GetCurrentBoardSquare(),
			true);
		bool flag = false;
		if (destSquare != null
		    && destSquare.OccupantActor != null
		    && destSquare.OccupantActor != targetingActor
		    && destSquare.OccupantActor.IsActorVisibleToClient())
		{
			BoardSquare chargeDestination = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, destSquare, boardSquarePathInfo);
			if (chargeDestination != destSquare)
			{
				destSquare = chargeDestination;
				flag = true;
			}
		}
		if (flag)
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
				targetingActor,
				destSquare,
				targetingActor.GetCurrentBoardSquare(),
				true);
		}
		int arrowIndex = 0;
		EnableAllMovementArrows();
		arrowIndex = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, arrowIndex);
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
		m_pathLosChecker.UpdateSquaresInPath(boardSquarePathInfo);
		Vector3 freeTargetPos = destPos;
		List<Vector3> coneOrigins = GetConeOrigins(currentTarget, freeTargetPos, targetingActor);
		List<Vector3> coneDirections = GetConeDirections(currentTarget, freeTargetPos, targetingActor);
		Dictionary<ActorData, int> hitActorToNumHits = new Dictionary<ActorData, int>();
		for (int j = 0; j < m_numCones; j++)
		{
			Vector3 vector3 = coneOrigins[j];
			Vector3 vector4 = coneDirections[j];
			vector4.y = 0f;
			vector4.Normalize();
			float num = VectorUtils.HorizontalAngle_Deg(vector4);
			List<ActorData> hitActors = AreaEffectUtils.GetActorsInCone(
				vector3,
				num,
				m_coneInfo.m_widthAngleDeg,
				m_coneInfo.m_radiusInSquares,
				m_coneInfo.m_backwardsOffset,
				true,
				targetingActor,
				GetAffectedTeams(),
				null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref hitActors);
			if (!actorsInLaser.IsNullOrEmpty())
			{
				hitActors.Remove(actorsInLaser[0]);
			}
			if (hitActors.Contains(targetingActor))
			{
				hitActors.Remove(targetingActor);
			}
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(losCheckPos, destPos);
			if (m_coneLosCheckDelegate != null)
			{
				for (int i = hitActors.Count - 1; i >= 0; i--)
				{
					if (!m_coneLosCheckDelegate(hitActors[i], targetingActor, coneLosCheckPos, null))
					{
						hitActors.RemoveAt(i);
					}
				}
			}
			AddActorsInRange(hitActors, destPos, targetingActor);
			foreach (ActorData hitActor in hitActors)
			{
				if (hitActorToNumHits.ContainsKey(hitActor))
				{
					hitActorToNumHits[hitActor]++;
				}
				else
				{
					hitActorToNumHits[hitActor] = 1;
				}
			}
			Vector3 position = vector3;
			position.y = HighlightUtils.GetHighlightHeight();
			m_highlights[1 + j].transform.position = position;
			m_highlights[1 + j].transform.rotation = Quaternion.LookRotation(vector4);
			SquareInsideChecker_Cone squareInsideChecker_Cone = m_squarePosCheckerList[2 + j] as SquareInsideChecker_Cone;
			squareInsideChecker_Cone.UpdateConeProperties(
				vector3,
				m_coneInfo.m_widthAngleDeg,
				m_coneInfo.m_radiusInSquares,
				m_coneInfo.m_backwardsOffset,
				num,
				targetingActor);
			squareInsideChecker_Cone.SetLosPosOverride(true, coneLosCheckPos, false);
		}
		
		if (m_affectsTargetingActor
		    && (m_includeCasterDelegate == null || m_includeCasterDelegate(targetingActor, m_actorsAddedSoFar)))
		{
			AddActorInRange(
				targetingActor,
				losCheckPos,
				targetingActor,
				AbilityTooltipSubject.Tertiary);
		}
		foreach (KeyValuePair<ActorData, int> hitActorAndNumHits in hitActorToNumHits)
		{
			ActorHitContext actorHitContext = m_actorContextVars[hitActorAndNumHits.Key];
			actorHitContext.m_contextVars.SetValue(ContextKeys.s_HitCount.GetKey(), hitActorAndNumHits.Value);
			actorHitContext.m_contextVars.SetValue(TargetSelect_LaserChargeWithReverseCones.s_cvarDirectChargeHit.GetKey(), 0);
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(
				m_indicatorHandler,
				losCheckPos,
				laserEndPos,
				m_dashWidthInSquares,
				targetingActor,
				false,
				null,
				m_squarePosCheckerList);
			for (int i = 0; i < m_numCones; i++)
			{
				Vector3 coneStart = coneOrigins[i];
				Vector3 vec = coneDirections[i];
				vec.y = 0f;
				vec.Normalize();
				float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
				AreaEffectUtils.OperateOnSquaresInCone(
					m_indicatorHandler,
					coneStart,
					coneCenterAngleDegrees,
					m_coneInfo.m_widthAngleDeg,
					m_coneInfo.m_radiusInSquares,
					m_coneInfo.m_backwardsOffset,
					targetingActor,
					false,
					m_squarePosCheckerList);
			}
			HideUnusedSquareIndicators();
		}
	}
}
