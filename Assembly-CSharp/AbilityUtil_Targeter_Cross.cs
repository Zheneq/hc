using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Cross : AbilityUtil_Targeter
{
	private float m_minDistanceInSquares;

	private float m_maxDistanceInSquares;

	private float m_crossDistanceInSquares;

	private float m_widthInSquares;

	public float m_crossLengthDecreaseOverDistance;

	private bool m_lockToCardinalDirs;

	private bool m_discreteStepsForRange;

	private bool m_penetrateLoS;

	private int m_maxTargets;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_Cross(Ability ability, float minDistanceInSquares, float maxDistanceInSquares, float crossDistanceInSquares, float widthInSquares, bool penetrateLoS, int maxTargets, bool lockToCardinalDirs, bool discreteStepsForRange, bool includeAllies = false, bool affectsCaster = false, float crossLengthDecreaseOverDistance = 0f) : base(ability)
	{
		this.m_minDistanceInSquares = minDistanceInSquares;
		this.m_maxDistanceInSquares = maxDistanceInSquares;
		this.m_crossDistanceInSquares = crossDistanceInSquares;
		this.m_widthInSquares = widthInSquares;
		this.m_crossLengthDecreaseOverDistance = crossLengthDecreaseOverDistance;
		this.m_lockToCardinalDirs = lockToCardinalDirs;
		this.m_discreteStepsForRange = discreteStepsForRange;
		this.m_maxTargets = maxTargets;
		this.m_penetrateLoS = penetrateLoS;
		this.m_affectsAllies = includeAllies;
		this.m_affectsTargetingActor = affectsCaster;
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForLaser();
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	private Vector3 GetClampedTargeterRange(AbilityTarget currentTarget, Vector3 startPos, Vector3 aimDir, ref float dist, ref float crossLength)
	{
		return GrydLaserT.GetClampedTargeterRangeStatic(currentTarget, startPos, aimDir, this.m_minDistanceInSquares, this.m_maxDistanceInSquares, this.m_discreteStepsForRange, this.m_crossLengthDecreaseOverDistance, ref dist, ref crossLength);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		float squareSize = Board.Get().squareSize;
		float num = this.m_minDistanceInSquares * squareSize;
		float num2 = this.m_crossDistanceInSquares * squareSize;
		float widthInWorld = this.m_widthInSquares * squareSize;
		if (this.m_highlights.Count < 3)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_Cross.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			this.ClearHighlightCursors(true);
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, num, null));
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, num2, null));
			this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(widthInWorld, num2, null));
		}
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		Vector3 vector = travelBoardSquareWorldPosition + new Vector3(0f, 0.1f, 0f);
		Vector3 vector2 = currentTarget.AimDirection;
		if (this.m_lockToCardinalDirs)
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
			vector2 = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector2)));
		}
		Vector3 clampedTargeterRange = this.GetClampedTargeterRange(currentTarget, vector, vector2, ref num, ref num2);
		float widthInWorld2 = (float)Mathf.CeilToInt(this.m_widthInSquares) * Board.SquareSizeStatic;
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld2, num, this.m_highlights[0]);
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld2, 0.5f * num2, this.m_highlights[1]);
		HighlightUtils.Get().ResizeRectangularCursor(widthInWorld2, 0.5f * num2, this.m_highlights[2]);
		Vector3 normalized = Vector3.Cross(vector2, Vector3.up).normalized;
		this.m_highlights[0].transform.position = vector;
		this.m_highlights[0].transform.rotation = Quaternion.LookRotation(vector2);
		this.m_highlights[1].transform.position = clampedTargeterRange;
		this.m_highlights[1].transform.rotation = Quaternion.LookRotation(normalized);
		this.m_highlights[2].transform.position = clampedTargeterRange;
		this.m_highlights[2].transform.rotation = Quaternion.LookRotation(-normalized);
		float laserRangeInSquares = 0.5f * (num2 / squareSize);
		Vector3 vector3;
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(travelBoardSquareWorldPositionForLos, vector2, num / squareSize, this.m_widthInSquares, targetingActor, base.GetAffectedTeams(), this.m_penetrateLoS, this.m_maxTargets, this.m_penetrateLoS, false, out vector3, null, null, false, true);
		clampedTargeterRange.y = travelBoardSquareWorldPositionForLos.y;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(clampedTargeterRange);
		List<ActorData> list = new List<ActorData>();
		List<ActorData> list2 = new List<ActorData>();
		if (boardSquare != null && boardSquare.height <= Board.Get().BaselineHeight)
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
			if (targetingActor.GetCurrentBoardSquare().\u0013(boardSquare.x, boardSquare.y))
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
				bool flag;
				Vector3 vector4;
				BarrierManager.Get().GetAbilityLineEndpoint(targetingActor, travelBoardSquareWorldPositionForLos, clampedTargeterRange, out flag, out vector4, null);
				if (!flag)
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
					list2 = AreaEffectUtils.GetActorsInLaser(clampedTargeterRange, normalized, laserRangeInSquares, this.m_widthInSquares, targetingActor, base.GetAffectedTeams(), this.m_penetrateLoS, this.m_maxTargets, this.m_penetrateLoS, false, out vector3, null, actorsInLaser, false, true);
					actorsInLaser.AddRange(list2);
					list = AreaEffectUtils.GetActorsInLaser(clampedTargeterRange, -1f * normalized, laserRangeInSquares, this.m_widthInSquares, targetingActor, base.GetAffectedTeams(), this.m_penetrateLoS, this.m_maxTargets, this.m_penetrateLoS, false, out vector3, null, actorsInLaser, false, true);
					actorsInLaser.AddRange(list);
					TargeterUtils.LimitActorsToMaxNumber(ref actorsInLaser, this.m_maxTargets);
				}
			}
		}
		using (List<ActorData>.Enumerator enumerator = actorsInLaser.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (!list.Contains(actorData))
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
					if (!list2.Contains(actorData))
					{
						base.AddActorInRange(actorData, travelBoardSquareWorldPosition, targetingActor, AbilityTooltipSubject.Primary, false);
						continue;
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				base.AddActorInRange(actorData, clampedTargeterRange, targetingActor, AbilityTooltipSubject.Primary, false);
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (this.m_affectsTargetingActor)
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
			base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPositionForLos(), targetingActor, AbilityTooltipSubject.Primary, false);
		}
		Vector3 crossStart = clampedTargeterRange + (0.5f * num2 - 0.1f) * normalized;
		Vector3 crossEnd = clampedTargeterRange - (0.5f * num2 - 0.1f) * normalized;
		this.DrawInvalidSquareIndicators(currentTarget, targetingActor, travelBoardSquareWorldPositionForLos, clampedTargeterRange, crossStart, crossEnd);
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 startPos, Vector3 endPos, Vector3 crossStart, Vector3 crossEnd)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, startPos, endPos, this.m_widthInSquares, targetingActor, this.m_penetrateLoS, null, null, true);
			BoardSquare boardSquare = Board.Get().GetBoardSquare(endPos);
			if (boardSquare != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_Cross.DrawInvalidSquareIndicators(AbilityTarget, ActorData, Vector3, Vector3, Vector3, Vector3)).MethodHandle;
				}
				if (boardSquare.height <= Board.Get().BaselineHeight && targetingActor.GetCurrentBoardSquare().\u0013(boardSquare.x, boardSquare.y))
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
					AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, endPos, crossStart, this.m_widthInSquares, targetingActor, this.m_penetrateLoS, null, null, true);
					AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(this.m_indicatorHandler, endPos, crossEnd, this.m_widthInSquares, targetingActor, this.m_penetrateLoS, null, null, true);
					goto IL_17F;
				}
			}
			float adjustAmount = 0f;
			int num;
			int num2;
			int num3;
			int num4;
			AreaEffectUtils.GetBoxBoundsInGridPos(crossStart, crossEnd, adjustAmount, out num, out num2, out num3, out num4);
			for (int i = num; i <= num3; i++)
			{
				for (int j = num2; j <= num4; j++)
				{
					BoardSquare boardSquare2 = Board.Get().GetBoardSquare(i, j);
					if (!(boardSquare2 == null) && AreaEffectUtils.IsSquareInBoxByActorRadius(boardSquare2, crossEnd, crossStart, this.m_widthInSquares))
					{
						this.m_indicatorHandler.OperateOnSquare(boardSquare2, targetingActor, false);
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			IL_17F:
			base.HideUnusedSquareIndicators();
		}
	}
}
