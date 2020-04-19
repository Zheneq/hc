using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ClericConeKnockback : AbilityUtil_Targeter_StretchCone
{
	private float m_knockbackDistanceLastTargeter;

	private KnockbackType m_knockbackTypeLastTargeter;

	public AbilityUtil_Targeter_ClericConeKnockback(Ability ability, float coneLengthInSquares, float coneWidthAngle, float backwardOffsetInSquares, bool penetrateLoS, float knockbackDistance, KnockbackType knockbackType) : base(ability, coneLengthInSquares, coneLengthInSquares, coneWidthAngle, coneWidthAngle, AreaEffectUtils.StretchConeStyle.Linear, backwardOffsetInSquares, penetrateLoS)
	{
		this.m_knockbackDistanceLastTargeter = knockbackDistance;
		this.m_knockbackTypeLastTargeter = knockbackType;
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (currentTargetIndex == 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_ClericConeKnockback.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			base.InitKnockbackData(0f, KnockbackType.AwayFromSource, 0f, KnockbackType.AwayFromSource);
			base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		}
		else
		{
			base.InitKnockbackData(this.m_knockbackDistanceLastTargeter, this.m_knockbackTypeLastTargeter, 0f, KnockbackType.AwayFromSource);
			Vector3 vector = targetingActor.\u0015();
			Vector3 freePos = targets[currentTargetIndex - 1].FreePos;
			Vector3 vector2 = freePos - vector;
			vector2.y = 0f;
			vector2.Normalize();
			Vector3 vector3 = Vector3.Cross(vector2, Vector3.up);
			float num = Vector3.Dot(vector3, currentTarget.AimDirection.normalized);
			Vector3 vector4 = Vector3.RotateTowards(vector2, (num <= 0f) ? (-vector3) : vector3, this.m_maxAngleDegrees * 0.5f * 0.0174532924f, 0f);
			if (this.m_highlights != null && this.m_highlights.Count < 1)
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
				GameObject item = AbilityUtil_Targeter_SoldierCardinalLines.CreateArrowPointerHighlight(1.5f);
				this.m_highlights.Add(item);
			}
			Vector3 position = vector + vector2.normalized * 0.5f * this.m_maxLengthSquares * Board.\u000E().squareSize;
			position.y = HighlightUtils.GetHighlightHeight();
			Vector3 forward = -1f * Vector3.Cross(vector4, (num <= 0f) ? Vector3.up : (-Vector3.up));
			this.m_highlights[this.m_highlights.Count - 1].transform.position = position;
			this.m_highlights[this.m_highlights.Count - 1].transform.rotation = Quaternion.LookRotation(forward);
			int num2 = 0;
			if (this.m_knockbackDistance > 0f)
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
				base.EnableAllMovementArrows();
				List<ActorData> visibleActorsInRange = (this.m_ability.Targeters[0] as AbilityUtil_Targeter_ClericConeKnockback).GetVisibleActorsInRange();
				foreach (ActorData actorData in visibleActorsInRange)
				{
					if (actorData.\u000E() != targetingActor.\u000E())
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
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, vector4, vector, this.m_knockbackDistance);
						num2 = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num2, false);
					}
				}
			}
			base.SetMovementArrowEnabledFromIndex(num2, false);
		}
	}
}
