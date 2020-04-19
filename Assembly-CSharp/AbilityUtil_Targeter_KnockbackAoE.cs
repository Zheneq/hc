using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_KnockbackAoE : AbilityUtil_Targeter_Shape
{
	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	public bool m_lockToCardinalDirs;

	public bool m_showArrowHighlight;

	public float m_heightOffset = 0.1f;

	public AbilityUtil_Targeter_KnockbackAoE(Ability ability, AbilityAreaShape shape, bool penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType, bool affectsEnemies, bool affectsAllies, AbilityUtil_Targeter.AffectsActor affectsCaster, AbilityUtil_Targeter.AffectsActor affectsBestTarget, float knockbackDistance, KnockbackType knockbackType) : base(ability, shape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible)
	{
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, new List<AbilityTarget>());
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		if (this.m_ability.GetExpectedNumberOfTargeters() > 1)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_KnockbackAoE.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
			}
			if (currentTargetIndex >= this.m_ability.GetExpectedNumberOfTargeters() - 1)
			{
				goto IL_3E;
			}
		}
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		IL_3E:
		if (currentTargetIndex == this.m_ability.GetExpectedNumberOfTargeters() - 1)
		{
			AbilityAreaShape shape = this.m_shape;
			AbilityTarget target;
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
				target = currentTarget;
			}
			else
			{
				target = targets[0];
			}
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(shape, target);
			Vector3 vector = currentTarget.FreePos - centerOfShape;
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
			int num = 0;
			if (this.m_knockbackDistance > 0f)
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
				base.EnableAllMovementArrows();
				List<ActorData> visibleActorsInRange = this.GetVisibleActorsInRange();
				if (currentTargetIndex > 0)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					visibleActorsInRange = (this.m_ability.Targeters[0] as AbilityUtil_Targeter_KnockbackAoE).GetVisibleActorsInRange();
				}
				foreach (ActorData actorData in visibleActorsInRange)
				{
					if (actorData.\u000E() != targetingActor.\u000E())
					{
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, vector, centerOfShape, this.m_knockbackDistance);
						num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
					}
				}
				if (this.m_showArrowHighlight)
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
					if (this.m_highlights != null)
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
						if (this.m_highlights.Count < 1)
						{
							GameObject item = AbilityUtil_Targeter_SoldierCardinalLines.CreateArrowPointerHighlight(1.5f);
							this.m_highlights.Add(item);
						}
					}
					Vector3 position = centerOfShape;
					position.y = HighlightUtils.GetHighlightHeight();
					this.m_highlights[this.m_highlights.Count - 1].transform.position = position;
					this.m_highlights[this.m_highlights.Count - 1].transform.rotation = Quaternion.LookRotation(vector);
				}
			}
			base.SetMovementArrowEnabledFromIndex(num, false);
		}
	}
}
