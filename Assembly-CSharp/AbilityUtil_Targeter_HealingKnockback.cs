using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_HealingKnockback : AbilityUtil_Targeter_Shape
{
	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	public float m_heightOffset = 0.1f;

	public AbilityUtil_Targeter_HealingKnockback(Ability ability, AbilityAreaShape shape, bool penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType, bool affectsEnemies, bool affectsAllies, AbilityUtil_Targeter.AffectsActor affectsCaster, AbilityUtil_Targeter.AffectsActor affectsBestTarget, float knockbackDistance, KnockbackType knockbackType) : base(ability, shape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, affectsCaster, AbilityUtil_Targeter.AffectsActor.Possible)
	{
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackType = knockbackType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, currentTarget);
		int num = 0;
		base.EnableAllMovementArrows();
		List<ActorData> visibleActorsInRange = this.GetVisibleActorsInRange();
		foreach (ActorData actorData in visibleActorsInRange)
		{
			if (actorData.\u000E() != targetingActor.\u000E())
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_HealingKnockback.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
				}
				BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, currentTarget.AimDirection, centerOfShape, this.m_knockbackDistance);
				num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
			}
		}
		base.SetMovementArrowEnabledFromIndex(num, false);
	}

	protected override bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin, ActorData bestTarget)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		if (boardSquare != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_HealingKnockback.HandleAddActorInShape(ActorData, ActorData, AbilityTarget, Vector3, ActorData)).MethodHandle;
			}
			if (boardSquare.occupant != null)
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
				bool flag = potentialTarget.\u000E() == targetingActor.\u000E();
				if (flag)
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
					if (boardSquare.occupant == potentialTarget.gameObject)
					{
						base.AddActorInRange(potentialTarget, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
						return true;
					}
				}
				if (!flag)
				{
					base.AddActorInRange(potentialTarget, damageOrigin, targetingActor, AbilityTooltipSubject.Enemy, false);
				}
				return true;
			}
		}
		return false;
	}
}
