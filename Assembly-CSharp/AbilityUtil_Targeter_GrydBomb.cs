using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_GrydBomb : AbilityUtil_Targeter_Shape
{
	public bool m_lockToCardinalDirs = true;

	public bool m_showArrowHighlight = true;

	public float m_bombMoveRange;

	public float m_heightOffset = 0.1f;

	private GrydPlaceOrMoveBomb m_bombAbility;

	public AbilityUtil_Targeter_GrydBomb(Ability ability, float moveRange) : base(ability, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible)
	{
		this.m_bombMoveRange = moveRange;
		this.m_bombAbility = (ability as GrydPlaceOrMoveBomb);
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		if (this.m_bombAbility != null && this.m_bombAbility.HasPlacedBomb())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_GrydBomb.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			base.ClearActorsInRange();
			base.SetShowArcToShape(false);
			Vector3 worldPositionForLoS = Board.Get().GetBoardSquareSafe(this.m_bombAbility.GetPlacedBomb()).GetWorldPositionForLoS();
			Vector3 vector = currentTarget.FreePos - worldPositionForLoS;
			if (this.m_lockToCardinalDirs)
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
				vector = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector)));
			}
			if (this.m_highlights != null && this.m_highlights.Count < 1)
			{
				this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(Board.Get().squareSize * 0.75f, this.m_bombMoveRange * Board.Get().squareSize, null));
			}
			Vector3 position = worldPositionForLoS;
			position.y = HighlightUtils.GetHighlightHeight();
			this.m_highlights[0].transform.position = position;
			this.m_highlights[0].transform.rotation = Quaternion.LookRotation(vector);
			Vector3 vector2;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(worldPositionForLoS, vector, this.m_bombMoveRange, 0.75f, targetingActor, targetingActor.GetOpposingTeams(), false, 1, false, false, out vector2, null, null, false, true);
			base.AddActorsInRange(actorsInLaser, worldPositionForLoS, targetingActor, AbilityTooltipSubject.Primary, false);
		}
		else
		{
			base.SetShowArcToShape(true);
			base.UpdateTargeting(currentTarget, targetingActor);
		}
	}
}
