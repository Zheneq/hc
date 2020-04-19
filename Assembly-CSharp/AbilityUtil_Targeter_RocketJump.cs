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
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 2)
			{
				goto IL_A3;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_RocketJump.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
		}
		bool isForLocalPlayer = targetingActor == GameFlowData.Get().activeOwnedActorData;
		this.m_highlights = new List<GameObject>();
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(this.m_shape, isForLocalPlayer));
		this.m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, isForLocalPlayer));
		IL_A3:
		GameObject gameObject = this.m_highlights[0];
		GameObject gameObject2 = this.m_highlights[1];
		Vector3 position = targetingActor.\u0016();
		position.y = HighlightUtils.GetHighlightHeight();
		gameObject.transform.position = position;
		if (boardSquare != null)
		{
			Vector3 position2 = boardSquare.ToVector3();
			position2.y = HighlightUtils.GetHighlightHeight();
			gameObject2.transform.position = position2;
			if (!gameObject2.activeSelf)
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
				gameObject2.SetActive(true);
			}
		}
		else
		{
			gameObject2.SetActive(false);
		}
		BoardSquarePathInfo boardSquarePathInfo = new BoardSquarePathInfo();
		boardSquarePathInfo.square = targetingActor.\u0012();
		BoardSquarePathInfo boardSquarePathInfo2 = new BoardSquarePathInfo();
		boardSquarePathInfo2.square = boardSquare;
		boardSquarePathInfo.next = boardSquarePathInfo2;
		boardSquarePathInfo2.prev = boardSquarePathInfo;
		int num = 0;
		if (this.m_knockbackDistance != 0f)
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
			base.SetMovementArrowEnabledFromIndex(0, true);
		}
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_shape, targetingActor.\u0016(), targetingActor.\u0012(), this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
		using (List<ActorData>.Enumerator enumerator = actorsInShape.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				ActorData actor = actorData;
				Vector3 damageOrigin = targetingActor.\u0016();
				AbilityTooltipSubject subjectType;
				if (actorData.\u000E() == targetingActor.\u000E())
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
					subjectType = AbilityTooltipSubject.Ally;
				}
				else
				{
					subjectType = AbilityTooltipSubject.Primary;
				}
				base.AddActorInRange(actor, damageOrigin, targetingActor, subjectType, false);
				if (this.m_knockbackDistance != 0f)
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
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, KnockbackType.AwayFromSource, currentTarget.AimDirection, targetingActor.\u0016(), this.m_knockbackDistance);
					num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
				}
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
		if (this.m_affectsTargetingActor)
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
			base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, AbilityTooltipSubject.Self, false);
		}
		base.SetMovementArrowEnabledFromIndex(num, false);
	}
}
