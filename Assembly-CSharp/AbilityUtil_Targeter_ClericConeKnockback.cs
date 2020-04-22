using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ClericConeKnockback : AbilityUtil_Targeter_StretchCone
{
	private float m_knockbackDistanceLastTargeter;

	private KnockbackType m_knockbackTypeLastTargeter;

	public AbilityUtil_Targeter_ClericConeKnockback(Ability ability, float coneLengthInSquares, float coneWidthAngle, float backwardOffsetInSquares, bool penetrateLoS, float knockbackDistance, KnockbackType knockbackType)
		: base(ability, coneLengthInSquares, coneLengthInSquares, coneWidthAngle, coneWidthAngle, AreaEffectUtils.StretchConeStyle.Linear, backwardOffsetInSquares, penetrateLoS)
	{
		m_knockbackDistanceLastTargeter = knockbackDistance;
		m_knockbackTypeLastTargeter = knockbackType;
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (currentTargetIndex == 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					InitKnockbackData(0f, KnockbackType.AwayFromSource, 0f, KnockbackType.AwayFromSource);
					base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
					return;
				}
			}
		}
		InitKnockbackData(m_knockbackDistanceLastTargeter, m_knockbackTypeLastTargeter, 0f, KnockbackType.AwayFromSource);
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		Vector3 freePos = targets[currentTargetIndex - 1].FreePos;
		Vector3 vector = freePos - travelBoardSquareWorldPositionForLos;
		vector.y = 0f;
		vector.Normalize();
		Vector3 vector2 = Vector3.Cross(vector, Vector3.up);
		float num = Vector3.Dot(vector2, currentTarget.AimDirection.normalized);
		Vector3 vector3 = Vector3.RotateTowards(vector, (!(num > 0f)) ? (-vector2) : vector2, m_maxAngleDegrees * 0.5f * ((float)Math.PI / 180f), 0f);
		if (m_highlights != null && m_highlights.Count < 1)
		{
			GameObject item = AbilityUtil_Targeter_SoldierCardinalLines.CreateArrowPointerHighlight();
			m_highlights.Add(item);
		}
		Vector3 position = travelBoardSquareWorldPositionForLos + vector.normalized * 0.5f * m_maxLengthSquares * Board.Get().squareSize;
		position.y = HighlightUtils.GetHighlightHeight();
		Vector3 forward = -1f * Vector3.Cross(vector3, (!(num > 0f)) ? Vector3.up : (-Vector3.up));
		m_highlights[m_highlights.Count - 1].transform.position = position;
		m_highlights[m_highlights.Count - 1].transform.rotation = Quaternion.LookRotation(forward);
		int num2 = 0;
		if (m_knockbackDistance > 0f)
		{
			EnableAllMovementArrows();
			List<ActorData> visibleActorsInRange = (m_ability.Targeters[0] as AbilityUtil_Targeter_ClericConeKnockback).GetVisibleActorsInRange();
			foreach (ActorData item2 in visibleActorsInRange)
			{
				if (item2.GetTeam() != targetingActor.GetTeam())
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(item2, m_knockbackType, vector3, travelBoardSquareWorldPositionForLos, m_knockbackDistance);
					num2 = AddMovementArrowWithPrevious(item2, path, TargeterMovementType.Knockback, num2);
				}
			}
		}
		SetMovementArrowEnabledFromIndex(num2, false);
	}
}
