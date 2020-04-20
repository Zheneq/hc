using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class AbilityUtil_Targeter_LaserChargeReverseCones : AbilityUtil_Targeter
{
	private float m_dashWidthInSquares;

	public float m_dashRangeInSquares;

	private ConeTargetingInfo m_coneInfo;

	private int m_numCones;

	private float m_coneStartOffset;

	private float m_perConeHorizontalOffset;

	private float m_angleInBetween;

	private AbilityUtil_Targeter_LaserChargeReverseCones.GetConeInfoDelegate GetConeOrigins;

	private AbilityUtil_Targeter_LaserChargeReverseCones.GetConeInfoDelegate GetConeDirections;

	public AbilityUtil_Targeter_LaserChargeReverseCones.ConeLosCheckerDelegate m_coneLosCheckDelegate;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private SquareInsideChecker_Box m_laserLosChecker;

	private SquareInsideChecker_Path m_pathLosChecker;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_LaserChargeReverseCones.IncludeCasterDelegate m_includeCasterDelegate;

	public AbilityUtil_Targeter_LaserChargeReverseCones(Ability ability, float dashWidth, float dashRange, ConeTargetingInfo coneInfo, int numCones, float coneStartOffset, float perConeHorizontalOffset, float angleInBetween, AbilityUtil_Targeter_LaserChargeReverseCones.GetConeInfoDelegate coneOriginsDelegate, AbilityUtil_Targeter_LaserChargeReverseCones.GetConeInfoDelegate coneDirectionsDelegate) : base(ability)
	{
		this.m_dashWidthInSquares = dashWidth;
		this.m_dashRangeInSquares = dashRange;
		this.m_coneInfo = coneInfo;
		this.m_numCones = numCones;
		this.m_coneStartOffset = coneStartOffset;
		this.m_perConeHorizontalOffset = perConeHorizontalOffset;
		this.m_angleInBetween = angleInBetween;
		this.GetConeOrigins = coneOriginsDelegate;
		this.GetConeDirections = coneDirectionsDelegate;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_laserLosChecker = new SquareInsideChecker_Box(dashWidth);
		this.m_laserLosChecker.m_penetrateLos = false;
		this.m_squarePosCheckerList.Add(this.m_laserLosChecker);
		this.m_pathLosChecker = new SquareInsideChecker_Path();
		this.m_squarePosCheckerList.Add(this.m_pathLosChecker);
		for (int i = 0; i < this.m_numCones; i++)
		{
			this.m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		base.ClearActorsInRange();
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count > 1)
			{
				goto IL_BF;
			}
		}
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(1f, 1f, null));
		for (int i = 0; i < this.m_numCones; i++)
		{
			this.m_highlights.Add(HighlightUtils.Get().CreateDynamicConeMesh(this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_widthAngleDeg, false, null));
		}
		IL_BF:
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 vector = VectorUtils.GetLaserEndPoint(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, this.m_dashRangeInSquares * Board.Get().squareSize, false, targetingActor, null, true);
		float num = (vector - travelBoardSquareWorldPositionForLos).magnitude;
		BoardSquare destination;
		num = ClaymoreCharge.GetMaxPotentialChargeDistance(travelBoardSquareWorldPositionForLos, vector, currentTarget.AimDirection, num, targetingActor, out destination);
		BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, destination, targetingActor.GetCurrentBoardSquare(), true);
		List<ActorData> actorsOnPath = ClaymoreCharge.GetActorsOnPath(path, targetingActor.GetOpposingTeams(), targetingActor);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsOnPath);
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, currentTarget.AimDirection, num / Board.Get().squareSize, this.m_dashWidthInSquares, targetingActor, targetingActor.GetOpposingTeams(), false, 1, true, false, out vector, null, null, false, true);
		actorsInLaser.AddRange(actorsOnPath);
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, travelBoardSquareWorldPositionForLos);
		ActorData actorData = null;
		if (actorsInLaser.Count > 0)
		{
			actorData = actorsInLaser[0];
			Vector3 travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
			base.AddActorInRange(actorData, travelBoardSquareWorldPosition, targetingActor, AbilityTooltipSubject.Primary, false);
			Vector3 lhs = travelBoardSquareWorldPosition - travelBoardSquareWorldPositionForLos;
			lhs.y = 0f;
			Vector3 vector2 = travelBoardSquareWorldPositionForLos + Vector3.Dot(lhs, currentTarget.AimDirection) * currentTarget.AimDirection;
			vector = vector2;
			ActorHitContext actorHitContext = this.m_actorContextVars[actorData];
			actorHitContext.symbol_0015.SetInt(TargetSelect_LaserChargeWithReverseCones.s_cvarDirectChargeHit.GetHash(), 1);
		}
		this.m_laserLosChecker.UpdateBoxProperties(travelBoardSquareWorldPositionForLos, vector, targetingActor);
		Vector3 a = vector - travelBoardSquareWorldPositionForLos;
		a.y = 0f;
		float magnitude = a.magnitude;
		a.Normalize();
		Vector3 vector3 = vector - Mathf.Min(0.5f, magnitude / 2f) * a;
		HighlightUtils.Get().RotateAndResizeRectangularCursor(this.m_highlights[0], travelBoardSquareWorldPositionForLos, vector3, this.m_dashWidthInSquares);
		BoardSquare boardSquare = ClaymoreCharge.GetChargeDestinationSquare(travelBoardSquareWorldPositionForLos, vector3, actorData, null, targetingActor, false);
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
		int num2 = 0;
		base.EnableAllMovementArrows();
		num2 = base.AddMovementArrowWithPrevious(targetingActor, boardSquarePathInfo, AbilityUtil_Targeter.TargeterMovementType.Movement, num2, false);
		base.SetMovementArrowEnabledFromIndex(num2, false);
		this.m_pathLosChecker.UpdateSquaresInPath(boardSquarePathInfo);
		Vector3 freeTargetPos = vector3;
		List<Vector3> list = this.GetConeOrigins(currentTarget, freeTargetPos, targetingActor);
		List<Vector3> list2 = this.GetConeDirections(currentTarget, freeTargetPos, targetingActor);
		Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
		for (int j = 0; j < this.m_numCones; j++)
		{
			Vector3 vector4 = list[j];
			Vector3 vector5 = list2[j];
			vector5.y = 0f;
			vector5.Normalize();
			float num3 = VectorUtils.HorizontalAngle_Deg(vector5);
			List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(vector4, num3, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, true, targetingActor, base.GetAffectedTeams(), null, false, default(Vector3));
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
			if (!actorsInLaser.IsNullOrEmpty<ActorData>())
			{
				actorsInCone.Remove(actorsInLaser[0]);
			}
			if (actorsInCone.Contains(targetingActor))
			{
				actorsInCone.Remove(targetingActor);
			}
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(travelBoardSquareWorldPositionForLos, vector3);
			if (this.m_coneLosCheckDelegate != null)
			{
				for (int k = actorsInCone.Count - 1; k >= 0; k--)
				{
					if (!this.m_coneLosCheckDelegate(actorsInCone[k], targetingActor, coneLosCheckPos, null))
					{
						actorsInCone.RemoveAt(k);
					}
				}
			}
			base.AddActorsInRange(actorsInCone, vector3, targetingActor, AbilityTooltipSubject.Primary, false);
			using (List<ActorData>.Enumerator enumerator = actorsInCone.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData2 = enumerator.Current;
					if (dictionary.ContainsKey(actorData2))
					{
						Dictionary<ActorData, int> dictionary2;
						ActorData key;
						(dictionary2 = dictionary)[key = actorData2] = dictionary2[key] + 1;
					}
					else
					{
						dictionary[actorData2] = 1;
					}
				}
			}
			Vector3 position = vector4;
			position.y = HighlightUtils.GetHighlightHeight();
			this.m_highlights[1 + j].transform.position = position;
			this.m_highlights[1 + j].transform.rotation = Quaternion.LookRotation(vector5);
			SquareInsideChecker_Cone squareInsideChecker_Cone = this.m_squarePosCheckerList[2 + j] as SquareInsideChecker_Cone;
			squareInsideChecker_Cone.UpdateConeProperties(vector4, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, num3, targetingActor);
			squareInsideChecker_Cone.SetLosPosOverride(true, coneLosCheckPos, false);
		}
		if (this.m_affectsTargetingActor)
		{
			if (this.m_includeCasterDelegate != null)
			{
				if (!this.m_includeCasterDelegate(targetingActor, this.m_actorsAddedSoFar))
				{
					goto IL_6A7;
				}
			}
			base.AddActorInRange(targetingActor, travelBoardSquareWorldPositionForLos, targetingActor, AbilityTooltipSubject.Tertiary, false);
		}
		IL_6A7:
		using (Dictionary<ActorData, int>.Enumerator enumerator2 = dictionary.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<ActorData, int> keyValuePair = enumerator2.Current;
				ActorHitContext actorHitContext2 = this.m_actorContextVars[keyValuePair.Key];
				actorHitContext2.symbol_0015.SetInt(ContextKeys.symbol_0019.GetHash(), keyValuePair.Value);
				actorHitContext2.symbol_0015.SetInt(TargetSelect_LaserChargeWithReverseCones.s_cvarDirectChargeHit.GetHash(), 0);
			}
		}
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, travelBoardSquareWorldPositionForLos, vector, this.m_dashWidthInSquares, targetingActor, false, null, this.m_squarePosCheckerList, true);
			for (int l = 0; l < this.m_numCones; l++)
			{
				Vector3 coneStart = list[l];
				Vector3 vec = list2[l];
				vec.y = 0f;
				vec.Normalize();
				float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(vec);
				AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, coneStart, coneCenterAngleDegrees, this.m_coneInfo.m_widthAngleDeg, this.m_coneInfo.m_radiusInSquares, this.m_coneInfo.m_backwardsOffset, targetingActor, false, this.m_squarePosCheckerList);
			}
			base.HideUnusedSquareIndicators();
		}
	}

	public delegate List<Vector3> GetConeInfoDelegate(AbilityTarget currentTarget, Vector3 freeTargetPos, ActorData caster);

	public delegate bool ConeLosCheckerDelegate(ActorData actor, ActorData caster, Vector3 chargeEndPos, List<NonActorTargetInfo> nonActorTargetInfo);

	public delegate bool IncludeCasterDelegate(ActorData caster, List<ActorData> actorsSoFar);
}
