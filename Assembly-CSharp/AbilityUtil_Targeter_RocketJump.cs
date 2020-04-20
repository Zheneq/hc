using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_RocketJump : AbilityUtil_Targeter
{
	public AbilityAreaShape m_shape;

	public bool m_penetrateLoS;

	public float m_heightOffset = 0.1f;

	private float m_knockbackDistance;

	public AbilityUtil_Targeter_RocketJump(Ability ability, AbilityAreaShape shape, bool penetrateLoS, float knockbackDistance, bool affectsAllies) : base(ability)
	{
		this.m_shape = shape;
		this.m_penetrateLoS = penetrateLoS;
		this.m_knockbackDistance = knockbackDistance;
		this.m_cursorType = HighlightUtils.CursorType.NoCursorType;
		this.m_affectsAllies = affectsAllies;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 2)
			{
				goto IL_A3;
			}
		}
		bool isForLocalPlayer = targetingActor == GameFlowData.Get().activeOwnedActorData;
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, isForLocalPlayer));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, isForLocalPlayer));
		IL_A3:
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
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
		if (this.m_knockbackDistance != 0f)
		{
			base.SetMovementArrowEnabledFromIndex(0, true);
		}
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor.GetCurrentBoardSquare(), this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
		using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				ActorData actor = actorData;
				Vector3 travelBoardSquareWorldPosition2 = targetingActor.GetTravelBoardSquareWorldPosition();
				AbilityTooltipSubject subjectType;
				if (actorData.GetTeam() == targetingActor.GetTeam())
				{
					subjectType = AbilityTooltipSubject.Ally;
				}
				else
				{
					subjectType = AbilityTooltipSubject.Primary;
				}
				base.AddActorInRange(actor, travelBoardSquareWorldPosition2, targetingActor, subjectType, false);
				if (this.m_knockbackDistance != 0f)
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, KnockbackType.AwayFromSource, currentTarget.AimDirection, targetingActor.GetTravelBoardSquareWorldPosition(), this.m_knockbackDistance);
					num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
				}
			}
		}
		if (this.m_affectsTargetingActor)
		{
			base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Self, false);
		}
		base.SetMovementArrowEnabledFromIndex(num, false);
	}
}
