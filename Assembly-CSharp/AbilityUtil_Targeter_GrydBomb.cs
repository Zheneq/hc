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
			Vector3 vector = Board.\u000E().\u000E(this.m_bombAbility.GetPlacedBomb()).\u000E();
			Vector3 vector2 = currentTarget.FreePos - vector;
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
				vector2 = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector2)));
			}
			if (this.m_highlights != null && this.m_highlights.Count < 1)
			{
				this.m_highlights.Add(HighlightUtils.Get().CreateRectangularCursor(Board.\u000E().squareSize * 0.75f, this.m_bombMoveRange * Board.\u000E().squareSize, null));
			}
			Vector3 position = vector;
			position.y = HighlightUtils.GetHighlightHeight();
			this.m_highlights[0].transform.position = position;
			this.m_highlights[0].transform.rotation = Quaternion.LookRotation(vector2);
			Vector3 vector3;
			List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(vector, vector2, this.m_bombMoveRange, 0.75f, targetingActor, targetingActor.\u0015(), false, 1, false, false, out vector3, null, null, false, true);
			base.AddActorsInRange(actorsInLaser, vector, targetingActor, AbilityTooltipSubject.Primary, false);
		}
		else
		{
			base.SetShowArcToShape(true);
			base.UpdateTargeting(currentTarget, targetingActor);
		}
	}
}
