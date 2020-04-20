using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_MovingShape : AbilityUtil_Targeter_Shape
{
	public float m_moveLineWidth;

	public AbilityUtil_Targeter_MovingShape.IsMovingShapeDelegate m_delegateIsMovingShape;

	public AbilityUtil_Targeter_MovingShape.MoveStartSquareDelegate m_delegateMoveStartSquare;

	public AbilityUtil_Targeter_MovingShape.MoveStartFreePosDelegate m_delegateMoveStartFreePos;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_MovingShape(Ability ability, AbilityAreaShape shape, bool penetrateLoS, float moveLineWidth) : base(ability, shape, penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible)
	{
		this.m_moveLineWidth = moveLineWidth;
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Shape(this.m_shape));
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Shape(this.m_shape));
		this.m_squarePosCheckerList.Add(new SquareInsideChecker_Box(this.m_moveLineWidth));
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (this.m_highlights.Count < 4)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MovingShape.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
			this.m_highlights.Add(TargeterUtils.CreateLaserBoxHighlight(Vector3.zero, Vector3.one, this.m_moveLineWidth, TargeterUtils.HeightAdjustType.DontAdjustHeight));
			float num = this.m_moveLineWidth * 0.4f;
			num = Mathf.Clamp(num, 1.5f, 3f);
			GameObject item = AbilityUtil_Targeter_SoldierCardinalLines.CreateArrowPointerHighlight(num);
			this.m_highlights.Add(item);
		}
		GameObject gameObject = this.m_highlights[1];
		GameObject gameObject2 = this.m_highlights[2];
		GameObject gameObject3 = this.m_highlights[3];
		bool flag = this.IsMovingShape(targetingActor);
		if (flag)
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
			BoardSquare moveStartSquare = this.GetMoveStartSquare(currentTarget, targetingActor);
			if (moveStartSquare != null)
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
				Vector3 moveStartFreePos = this.GetMoveStartFreePos(currentTarget, targetingActor);
				Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, moveStartFreePos, moveStartSquare);
				Vector3 highlightGoalPos = base.GetHighlightGoalPos(currentTarget, targetingActor);
				List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, this.m_affectsAllies, this.m_affectsEnemies);
				List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, centerOfShape, moveStartSquare, this.m_penetrateLoS, targetingActor, relevantTeams, null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
				using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actor = enumerator.Current;
						base.AddActorInRange(actor, centerOfShape, targetingActor, AbilityTooltipSubject.Primary, false);
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
				List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(centerOfShape, highlightGoalPos, 0f, 0f, 0.5f * this.m_moveLineWidth, this.m_penetrateLoS, targetingActor, relevantTeams, null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInRadiusOfLine);
				using (List<ActorData>.Enumerator enumerator2 = actorsInRadiusOfLine.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ActorData actor2 = enumerator2.Current;
						base.AddActorInRange(actor2, centerOfShape, targetingActor, AbilityTooltipSubject.Primary, false);
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
				Vector3 vector = centerOfShape;
				vector.y = HighlightUtils.GetHighlightHeight();
				Vector3 vector2 = highlightGoalPos;
				vector2.y = HighlightUtils.GetHighlightHeight();
				Vector3 forward = vector2 - vector;
				gameObject.transform.position = vector;
				TargeterUtils.RefreshLaserBoxHighlight(gameObject2, vector, vector2, this.m_moveLineWidth, TargeterUtils.HeightAdjustType.DontAdjustHeight);
				gameObject3.transform.position = vector;
				gameObject3.transform.rotation = Quaternion.LookRotation(forward);
				if (GameFlowData.Get().activeOwnedActorData == targetingActor)
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
					SquareInsideChecker_Shape squareInsideChecker_Shape = this.m_squarePosCheckerList[0] as SquareInsideChecker_Shape;
					SquareInsideChecker_Shape squareInsideChecker_Shape2 = this.m_squarePosCheckerList[1] as SquareInsideChecker_Shape;
					SquareInsideChecker_Box squareInsideChecker_Box = this.m_squarePosCheckerList[2] as SquareInsideChecker_Box;
					squareInsideChecker_Shape.UpdateShapeProperties(moveStartFreePos, moveStartSquare, targetingActor);
					squareInsideChecker_Shape2.UpdateShapeProperties(currentTarget.FreePos, base.GetGameplayRefSquare(currentTarget, targetingActor), targetingActor);
					squareInsideChecker_Box.UpdateBoxProperties(centerOfShape, highlightGoalPos, targetingActor);
					AreaEffectUtils.OperateOnSquaresInShape(this.m_indicatorHandler, this.m_shape, moveStartFreePos, moveStartSquare, this.m_penetrateLoS, targetingActor, this.m_squarePosCheckerList);
					AreaEffectUtils.OperateOnSquaresInRadiusOfLine(this.m_indicatorHandler, centerOfShape, highlightGoalPos, 0f, 0f, 0.5f * this.m_moveLineWidth, this.m_penetrateLoS, targetingActor);
					base.HideUnusedSquareIndicators();
				}
			}
			else
			{
				flag = false;
			}
		}
		gameObject.SetActiveIfNeeded(flag);
		gameObject2.SetActiveIfNeeded(flag);
		gameObject3.SetActiveIfNeeded(flag);
	}

	private bool IsMovingShape(ActorData targetingActor)
	{
		if (this.m_delegateIsMovingShape != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MovingShape.IsMovingShape(ActorData)).MethodHandle;
			}
			return this.m_delegateIsMovingShape(targetingActor);
		}
		return false;
	}

	private BoardSquare GetMoveStartSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_delegateMoveStartSquare != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MovingShape.GetMoveStartSquare(AbilityTarget, ActorData)).MethodHandle;
			}
			return this.m_delegateMoveStartSquare(currentTarget, targetingActor);
		}
		return targetingActor.GetCurrentBoardSquare();
	}

	private Vector3 GetMoveStartFreePos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_delegateMoveStartFreePos != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_MovingShape.GetMoveStartFreePos(AbilityTarget, ActorData)).MethodHandle;
			}
			return this.m_delegateMoveStartFreePos(currentTarget, targetingActor);
		}
		return targetingActor.GetTravelBoardSquareWorldPosition();
	}

	public delegate bool IsMovingShapeDelegate(ActorData targetingActor);

	public delegate BoardSquare MoveStartSquareDelegate(AbilityTarget currentTarget, ActorData targetingActor);

	public delegate Vector3 MoveStartFreePosDelegate(AbilityTarget currentTarget, ActorData targetingActor);
}
