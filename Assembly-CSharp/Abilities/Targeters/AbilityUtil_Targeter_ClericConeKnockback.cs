using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ClericConeKnockback : AbilityUtil_Targeter_StretchCone
{
	private float m_knockbackDistanceLastTargeter;
	private KnockbackType m_knockbackTypeLastTargeter;

	public AbilityUtil_Targeter_ClericConeKnockback(
		Ability ability,
		float coneLengthInSquares,
		float coneWidthAngle,
		float backwardOffsetInSquares,
		bool penetrateLoS,
		float knockbackDistance,
		KnockbackType knockbackType)
		: base(
			ability,
			coneLengthInSquares,
			coneLengthInSquares,
			coneWidthAngle,
			coneWidthAngle,
			AreaEffectUtils.StretchConeStyle.Linear,
			backwardOffsetInSquares,
			penetrateLoS)
	{
		m_knockbackDistanceLastTargeter = knockbackDistance;
		m_knockbackTypeLastTargeter = knockbackType;
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (currentTargetIndex == 0)
		{
			InitKnockbackData(0f, KnockbackType.AwayFromSource, 0f, KnockbackType.AwayFromSource);
			base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
			return;
		}
		InitKnockbackData(m_knockbackDistanceLastTargeter, m_knockbackTypeLastTargeter, 0f, KnockbackType.AwayFromSource);
		Vector3 losCheckPos = targetingActor.GetLoSCheckPos();
		Vector3 prevTargetPos = targets[currentTargetIndex - 1].FreePos;
		Vector3 vector = prevTargetPos - losCheckPos;
		vector.y = 0f;
		vector.Normalize();
		Vector3 right = Vector3.Cross(vector, Vector3.up);
		float dot = Vector3.Dot(right, currentTarget.AimDirection.normalized);
		Vector3 vector3 = Vector3.RotateTowards(
			vector,
			dot > 0f ? right : -right,
			m_maxAngleDegrees * 0.5f * ((float)Math.PI / 180f),
			0f);
		if (m_highlights != null && m_highlights.Count < 1)
		{
			m_highlights.Add(AbilityUtil_Targeter_SoldierCardinalLines.CreateArrowPointerHighlight());
		}
		Vector3 position = losCheckPos + vector.normalized * 0.5f * m_maxLengthSquares * Board.Get().squareSize;
		position.y = HighlightUtils.GetHighlightHeight();
		Vector3 forward = -1f * Vector3.Cross(vector3, dot > 0f ? -Vector3.up : Vector3.up);
		m_highlights[m_highlights.Count - 1].transform.position = position;
		m_highlights[m_highlights.Count - 1].transform.rotation = Quaternion.LookRotation(forward);
		int arrowIndex = 0;
		if (m_knockbackDistance > 0f)
		{
			EnableAllMovementArrows();
			List<ActorData> visibleActorsInRange = (m_ability.Targeters[0] as AbilityUtil_Targeter_ClericConeKnockback).GetVisibleActorsInRange();
			foreach (ActorData target in visibleActorsInRange)
			{
				if (target.GetTeam() != targetingActor.GetTeam())
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(target, m_knockbackType, vector3, losCheckPos, m_knockbackDistance);
					arrowIndex = AddMovementArrowWithPrevious(target, path, TargeterMovementType.Knockback, arrowIndex);
				}
			}
		}
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
	}
}
