using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_KnockbackAoE : AbilityUtil_Targeter_Shape
{
	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	public bool m_lockToCardinalDirs;

	public bool m_showArrowHighlight;

	public float m_heightOffset = 0.1f;

	public AbilityUtil_Targeter_KnockbackAoE(Ability ability, AbilityAreaShape shape, bool penetrateLoS, DamageOriginType damageOriginType, bool affectsEnemies, bool affectsAllies, AffectsActor affectsCaster, AffectsActor affectsBestTarget, float knockbackDistance, KnockbackType knockbackType)
		: base(ability, shape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, affectsCaster)
	{
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, new List<AbilityTarget>());
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (m_ability.GetExpectedNumberOfTargeters() > 1)
		{
			if (currentTargetIndex >= m_ability.GetExpectedNumberOfTargeters() - 1)
			{
				goto IL_003e;
			}
		}
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		goto IL_003e;
		IL_003e:
		if (currentTargetIndex != m_ability.GetExpectedNumberOfTargeters() - 1)
		{
			return;
		}
		AbilityAreaShape shape = m_shape;
		AbilityTarget target;
		if (currentTargetIndex == 0)
		{
			target = currentTarget;
		}
		else
		{
			target = targets[0];
		}
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, target);
		Vector3 vector = currentTarget.FreePos - centerOfShape;
		if (m_lockToCardinalDirs)
		{
			vector = VectorUtils.HorizontalAngleToClosestCardinalDirection(Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector)));
		}
		int num = 0;
		if (m_knockbackDistance > 0f)
		{
			EnableAllMovementArrows();
			List<ActorData> visibleActorsInRange = GetVisibleActorsInRange();
			if (currentTargetIndex > 0)
			{
				visibleActorsInRange = (m_ability.Targeters[0] as AbilityUtil_Targeter_KnockbackAoE).GetVisibleActorsInRange();
			}
			foreach (ActorData item2 in visibleActorsInRange)
			{
				if (item2.GetTeam() != targetingActor.GetTeam())
				{
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(item2, m_knockbackType, vector, centerOfShape, m_knockbackDistance);
					num = AddMovementArrowWithPrevious(item2, path, TargeterMovementType.Knockback, num);
				}
			}
			if (m_showArrowHighlight)
			{
				if (m_highlights != null)
				{
					if (m_highlights.Count < 1)
					{
						GameObject item = AbilityUtil_Targeter_SoldierCardinalLines.CreateArrowPointerHighlight();
						m_highlights.Add(item);
					}
				}
				Vector3 position = centerOfShape;
				position.y = HighlightUtils.GetHighlightHeight();
				m_highlights[m_highlights.Count - 1].transform.position = position;
				m_highlights[m_highlights.Count - 1].transform.rotation = Quaternion.LookRotation(vector);
			}
		}
		SetMovementArrowEnabledFromIndex(num, false);
	}
}
