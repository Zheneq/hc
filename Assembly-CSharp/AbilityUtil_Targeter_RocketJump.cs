using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_RocketJump : AbilityUtil_Targeter
{
	public AbilityAreaShape m_shape;

	public bool m_penetrateLoS;

	public float m_heightOffset = 0.1f;

	private float m_knockbackDistance;

	public AbilityUtil_Targeter_RocketJump(Ability ability, AbilityAreaShape shape, bool penetrateLoS, float knockbackDistance, bool affectsAllies)
		: base(ability)
	{
		m_shape = shape;
		m_penetrateLoS = penetrateLoS;
		m_knockbackDistance = knockbackDistance;
		m_cursorType = HighlightUtils.CursorType.NoCursorType;
		m_affectsAllies = affectsAllies;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 2)
			{
				goto IL_00a3;
			}
		}
		bool isForLocalPlayer = targetingActor == GameFlowData.Get().activeOwnedActorData;
		m_highlights = new List<GameObject>();
		m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(m_shape, isForLocalPlayer));
		m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, isForLocalPlayer));
		goto IL_00a3;
		IL_00a3:
		GameObject gameObject = m_highlights[0];
		GameObject gameObject2 = m_highlights[1];
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		travelBoardSquareWorldPosition.y = HighlightUtils.GetHighlightHeight();
		gameObject.transform.position = travelBoardSquareWorldPosition;
		if (boardSquareSafe != null)
		{
			Vector3 position = boardSquareSafe.ToVector3();
			position.y = HighlightUtils.GetHighlightHeight();
			gameObject2.transform.position = position;
			if (!gameObject2.activeSelf)
			{
				gameObject2.SetActive(true);
			}
		}
		else
		{
			gameObject2.SetActive(false);
		}
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = targetingActor.GetCurrentBoardSquare();
		BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo();
		boardSquarePathInfo2.square = boardSquareSafe;
		boardSquarePathInfo.next = boardSquarePathInfo2;
		boardSquarePathInfo2.prev = boardSquarePathInfo;
		int num = 0;
		if (m_knockbackDistance != 0f)
		{
			SetMovementArrowEnabledFromIndex(0, true);
		}
		List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_shape, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor.GetCurrentBoardSquare(), m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				Vector3 travelBoardSquareWorldPosition2 = targetingActor.GetTravelBoardSquareWorldPosition();
				int subjectType;
				if (current.GetTeam() == targetingActor.GetTeam())
				{
					subjectType = 6;
				}
				else
				{
					subjectType = 1;
				}
				AddActorInRange(current, travelBoardSquareWorldPosition2, targetingActor, (AbilityTooltipSubject)subjectType);
				if (m_knockbackDistance != 0f)
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current, KnockbackType.AwayFromSource, currentTarget.AimDirection, targetingActor.GetTravelBoardSquareWorldPosition(), m_knockbackDistance);
					num = AddMovementArrowWithPrevious(current, path, TargeterMovementType.Knockback, num);
				}
			}
		}
		if (m_affectsTargetingActor)
		{
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self);
		}
		SetMovementArrowEnabledFromIndex(num, false);
	}
}
