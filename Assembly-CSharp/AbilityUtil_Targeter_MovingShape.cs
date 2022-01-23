using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_MovingShape : AbilityUtil_Targeter_Shape
{
	public delegate bool IsMovingShapeDelegate(ActorData targetingActor);

	public delegate BoardSquare MoveStartSquareDelegate(AbilityTarget currentTarget, ActorData targetingActor);

	public delegate Vector3 MoveStartFreePosDelegate(AbilityTarget currentTarget, ActorData targetingActor);

	public float m_moveLineWidth;

	public IsMovingShapeDelegate m_delegateIsMovingShape;

	public MoveStartSquareDelegate m_delegateMoveStartSquare;

	public MoveStartFreePosDelegate m_delegateMoveStartFreePos;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public AbilityUtil_Targeter_MovingShape(Ability ability, AbilityAreaShape shape, bool penetrateLoS, float moveLineWidth)
		: base(ability, shape, penetrateLoS)
	{
		m_moveLineWidth = moveLineWidth;
		m_squarePosCheckerList.Add(new SquareInsideChecker_Shape(m_shape));
		m_squarePosCheckerList.Add(new SquareInsideChecker_Shape(m_shape));
		m_squarePosCheckerList.Add(new SquareInsideChecker_Box(m_moveLineWidth));
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (m_highlights.Count < 4)
		{
			m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, targetingActor == GameFlowData.Get().activeOwnedActorData));
			m_highlights.Add(TargeterUtils.CreateLaserBoxHighlight(Vector3.zero, Vector3.one, m_moveLineWidth, TargeterUtils.HeightAdjustType.DontAdjustHeight));
			float value = m_moveLineWidth * 0.4f;
			value = Mathf.Clamp(value, 1.5f, 3f);
			GameObject item = AbilityUtil_Targeter_SoldierCardinalLines.CreateArrowPointerHighlight(value);
			m_highlights.Add(item);
		}
		GameObject gameObject = m_highlights[1];
		GameObject gameObject2 = m_highlights[2];
		GameObject gameObject3 = m_highlights[3];
		bool flag = IsMovingShape(targetingActor);
		if (flag)
		{
			BoardSquare moveStartSquare = GetMoveStartSquare(currentTarget, targetingActor);
			if (moveStartSquare != null)
			{
				Vector3 moveStartFreePos = GetMoveStartFreePos(currentTarget, targetingActor);
				Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, moveStartFreePos, moveStartSquare);
				Vector3 highlightGoalPos = GetHighlightGoalPos(currentTarget, targetingActor);
				List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies);
				List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_shape, centerOfShape, moveStartSquare, m_penetrateLoS, targetingActor, relevantTeams, null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
				using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						AddActorInRange(current, centerOfShape, targetingActor);
					}
				}
				List<ActorData> actors2 = AreaEffectUtils.GetActorsInRadiusOfLine(centerOfShape, highlightGoalPos, 0f, 0f, 0.5f * m_moveLineWidth, m_penetrateLoS, targetingActor, relevantTeams, null);
				TargeterUtils.RemoveActorsInvisibleToClient(ref actors2);
				using (List<ActorData>.Enumerator enumerator2 = actors2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						ActorData current2 = enumerator2.Current;
						AddActorInRange(current2, centerOfShape, targetingActor);
					}
				}
				Vector3 vector = centerOfShape;
				vector.y = HighlightUtils.GetHighlightHeight();
				Vector3 vector2 = highlightGoalPos;
				vector2.y = HighlightUtils.GetHighlightHeight();
				Vector3 forward = vector2 - vector;
				gameObject.transform.position = vector;
				TargeterUtils.RefreshLaserBoxHighlight(gameObject2, vector, vector2, m_moveLineWidth, TargeterUtils.HeightAdjustType.DontAdjustHeight);
				gameObject3.transform.position = vector;
				gameObject3.transform.rotation = Quaternion.LookRotation(forward);
				if (GameFlowData.Get().activeOwnedActorData == targetingActor)
				{
					SquareInsideChecker_Shape squareInsideChecker_Shape = m_squarePosCheckerList[0] as SquareInsideChecker_Shape;
					SquareInsideChecker_Shape squareInsideChecker_Shape2 = m_squarePosCheckerList[1] as SquareInsideChecker_Shape;
					SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[2] as SquareInsideChecker_Box;
					squareInsideChecker_Shape.UpdateShapeProperties(moveStartFreePos, moveStartSquare, targetingActor);
					squareInsideChecker_Shape2.UpdateShapeProperties(currentTarget.FreePos, GetGameplayRefSquare(currentTarget, targetingActor), targetingActor);
					squareInsideChecker_Box.UpdateBoxProperties(centerOfShape, highlightGoalPos, targetingActor);
					AreaEffectUtils.OperateOnSquaresInShape(m_indicatorHandler, m_shape, moveStartFreePos, moveStartSquare, m_penetrateLoS, targetingActor, m_squarePosCheckerList);
					AreaEffectUtils.OperateOnSquaresInRadiusOfLine(m_indicatorHandler, centerOfShape, highlightGoalPos, 0f, 0f, 0.5f * m_moveLineWidth, m_penetrateLoS, targetingActor);
					HideUnusedSquareIndicators();
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
		if (m_delegateIsMovingShape != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_delegateIsMovingShape(targetingActor);
				}
			}
		}
		return false;
	}

	private BoardSquare GetMoveStartSquare(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_delegateMoveStartSquare != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_delegateMoveStartSquare(currentTarget, targetingActor);
				}
			}
		}
		return targetingActor.GetCurrentBoardSquare();
	}

	private Vector3 GetMoveStartFreePos(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (m_delegateMoveStartFreePos != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return m_delegateMoveStartFreePos(currentTarget, targetingActor);
				}
			}
		}
		return targetingActor.GetFreePos();
	}
}
