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

	public AbilityUtil_Targeter_LaserChargeReverseCones(Ability ability, float dashWidth, float dashRange, ConeTargetingInfo coneInfo, int numCones, float coneStartOffset, float perConeHorizontalOffset, float angleInBetween, GetConeInfoDelegate coneOriginsDelegate, GetConeInfoDelegate coneDirectionsDelegate)
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
		m_laserLosChecker = new SquareInsideChecker_Box(dashWidth);
		m_laserLosChecker.m_penetrateLos = false;
		m_squarePosCheckerList.Add(m_laserLosChecker);
		m_pathLosChecker = new SquareInsideChecker_Path();
		m_squarePosCheckerList.Add(m_pathLosChecker);
		for (int i = 0; i < m_numCones; i++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
		}
		while (true)
		{
			return;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		ClearActorsInRange();
		if (m_highlights != null)
		{
			if (m_highlights.Count > 1)
			{
				goto IL_00bf;
			}
		}
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f));
		for (int i = 0; i < m_numCones; i++)
		{
			m_highlights.Add(HighlightUtils.Get().CreateDynamicConeMesh(m_coneInfo.m_radiusInSquares, m_coneInfo.m_widthAngleDeg, false));
		}
		goto IL_00bf;
		IL_00bf:
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 laserEndPos = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, m_dashRangeInSquares * Board.Get().squareSize, false, targetingActor);
		float magnitude = (laserEndPos - travelBoardSquareWorldPositionForLos).magnitude;
		magnitude = ClaymoreCharge.GetMaxPotentialChargeDistance(travelBoardSquareWorldPositionForLos, laserEndPos, currentTarget.AimDirection, magnitude, targetingActor, out BoardSquare pathEndSquare);
		BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, pathEndSquare, targetingActor.GetCurrentBoardSquare(), true);
		List<ActorData> actors = ClaymoreCharge.GetActorsOnPath(path, targetingActor.GetEnemyTeams(), targetingActor);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		List<ActorData> actors2 = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, magnitude / Board.Get().squareSize, m_dashWidthInSquares, targetingActor, targetingActor.GetEnemyTeams(), false, 1, true, false, out laserEndPos, null);
		actors2.AddRange(actors);
		TargeterUtils.SortActorsByDistanceToPos(ref actors2, travelBoardSquareWorldPositionForLos);
		ActorData actorData = null;
		if (actors2.Count > 0)
		{
			actorData = actors2[0];
			Vector3 travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
			AddActorInRange(actorData, travelBoardSquareWorldPosition, targetingActor);
			Vector3 lhs = travelBoardSquareWorldPosition - travelBoardSquareWorldPositionForLos;
			lhs.y = 0f;
			Vector3 vector = travelBoardSquareWorldPositionForLos + Vector3.Dot(lhs, currentTarget.AimDirection) * currentTarget.AimDirection;
			laserEndPos = vector;
			ActorHitContext actorHitContext = m_actorContextVars[actorData];
			actorHitContext.m_contextVars.SetValue(TargetSelect_LaserChargeWithReverseCones.s_cvarDirectChargeHit.GetKey(), 1);
		}
		m_laserLosChecker.UpdateBoxProperties(travelBoardSquareWorldPositionForLos, laserEndPos, targetingActor);
		Vector3 a = laserEndPos - travelBoardSquareWorldPositionForLos;
		a.y = 0f;
		float magnitude2 = a.magnitude;
		a.Normalize();
		Vector3 vector2 = laserEndPos - Mathf.Min(0.5f, magnitude2 / 2f) * a;
		HighlightUtils.Get().RotateAndResizeRectangularCursor(m_highlights[0], travelBoardSquareWorldPositionForLos, vector2, m_dashWidthInSquares);
		BoardSquare boardSquare = ClaymoreCharge.GetChargeDestinationSquare(travelBoardSquareWorldPositionForLos, vector2, actorData, null, targetingActor, false);
		BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, targetingActor.GetCurrentBoardSquare(), true);
		bool flag = false;
		if (boardSquare != null)
		{
			if (boardSquare.OccupantActor != null)
			{
				if (boardSquare.OccupantActor != targetingActor)
				{
					if (boardSquare.OccupantActor.IsVisibleToClient())
					{
						BoardSquare chargeDestination = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(targetingActor, boardSquare, boardSquarePathInfo);
						if (chargeDestination != boardSquare)
						{
							boardSquare = chargeDestination;
							flag = true;
						}
					}
				}
			}
		}
		if (flag)
		{
			boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, targetingActor.GetCurrentBoardSquare(), true);
		}
		int arrowIndex = 0;
		EnableAllMovementArrows();
		arrowIndex = AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, TargeterMovementType.Movement, arrowIndex);
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
		m_pathLosChecker.UpdateSquaresInPath(boardSquarePathInfo);
		Vector3 freeTargetPos = vector2;
		List<Vector3> list = GetConeOrigins(currentTarget, freeTargetPos, targetingActor);
		List<Vector3> list2 = GetConeDirections(currentTarget, freeTargetPos, targetingActor);
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		for (int j = 0; j < m_numCones; j++)
		{
			Vector3 vector3 = list[j];
			Vector3 vector4 = list2[j];
			vector4.y = 0f;
			vector4.Normalize();
			float num = VectorUtils.HorizontalAngle_Deg(vector4);
			List<ActorData> actors3 = AreaEffectUtils.GetActorsInCone(vector3, num, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, true, targetingActor, GetAffectedTeams(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors3);
			if (!actors2.IsNullOrEmpty())
			{
				actors3.Remove(actors2[0]);
			}
			if (actors3.Contains(targetingActor))
			{
				actors3.Remove(targetingActor);
			}
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(travelBoardSquareWorldPositionForLos, vector2);
			if (m_coneLosCheckDelegate != null)
			{
				for (int num2 = actors3.Count - 1; num2 >= 0; num2--)
				{
					if (!m_coneLosCheckDelegate(actors3[num2], targetingActor, coneLosCheckPos, null))
					{
						actors3.RemoveAt(num2);
					}
				}
			}
			AddActorsInRange(actors3, vector2, targetingActor);
			using (List<ActorData>.Enumerator enumerator = actors3.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData current = enumerator.Current;
					if (dictionary.ContainsKey(current))
					{
						dictionary[current]++;
					}
					else
					{
						dictionary[current] = 1;
					}
				}
			}
			Vector3 position = vector3;
			position.y = HighlightUtils.GetHighlightHeight();
			m_highlights[1 + j].transform.position = position;
			m_highlights[1 + j].transform.rotation = Quaternion.LookRotation(vector4);
			SquareInsideChecker_Cone squareInsideChecker_Cone = m_squarePosCheckerList[2 + j] as SquareInsideChecker_Cone;
			squareInsideChecker_Cone.UpdateConeProperties(vector3, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, num, targetingActor);
			squareInsideChecker_Cone.SetLosPosOverride(true, coneLosCheckPos, false);
		}
		while (true)
		{
			if (m_affectsTargetingActor)
			{
				if (m_includeCasterDelegate != null)
				{
					if (!m_includeCasterDelegate(targetingActor, m_actorsAddedSoFar))
					{
						goto IL_06a7;
					}
				}
				AddActorInRange(targetingActor, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Tertiary);
			}
			goto IL_06a7;
			IL_06a7:
			using (Dictionary<ActorData, int>.Enumerator enumerator2 = dictionary.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<ActorData, int> current2 = enumerator2.Current;
					ActorHitContext actorHitContext2 = m_actorContextVars[current2.Key];
					actorHitContext2.m_contextVars.SetValue(ContextKeys.s_HitCount.GetKey(), current2.Value);
					actorHitContext2.m_contextVars.SetValue(TargetSelect_LaserChargeWithReverseCones.s_cvarDirectChargeHit.GetKey(), 0);
				}
			}
			if (targetingActor == GameFlowData.Get().activeOwnedActorData)
			{
				ResetSquareIndicatorIndexToUse();
				AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, travelBoardSquareWorldPositionForLos, laserEndPos, m_dashWidthInSquares, targetingActor, false, null, m_squarePosCheckerList);
				for (int k = 0; k < m_numCones; k++)
				{
					Vector3 coneStart = list[k];
					Vector3 vec = list2[k];
					vec.y = 0f;
					vec.Normalize();
					float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
					AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, coneStart, coneCenterAngleDegrees, m_coneInfo.m_widthAngleDeg, m_coneInfo.m_radiusInSquares, m_coneInfo.m_backwardsOffset, targetingActor, false, m_squarePosCheckerList);
				}
				while (true)
				{
					HideUnusedSquareIndicators();
					return;
				}
			}
			return;
		}
	}
}
