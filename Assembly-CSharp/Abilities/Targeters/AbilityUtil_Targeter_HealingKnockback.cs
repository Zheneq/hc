using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_HealingKnockback : AbilityUtil_Targeter_Shape
{
	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	public float m_heightOffset = 0.1f;

	public AbilityUtil_Targeter_HealingKnockback(Ability ability, AbilityAreaShape shape, bool penetrateLoS, DamageOriginType damageOriginType, bool affectsEnemies, bool affectsAllies, AffectsActor affectsCaster, AffectsActor affectsBestTarget, float knockbackDistance, KnockbackType knockbackType)
		: base(ability, shape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, affectsCaster)
	{
		m_knockbackDistance = knockbackDistance;
		m_knockbackType = knockbackType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
		int num = 0;
		EnableAllMovementArrows();
		List<ActorData> visibleActorsInRange = GetVisibleActorsInRange();
		foreach (ActorData item in visibleActorsInRange)
		{
			if (item.GetTeam() != targetingActor.GetTeam())
			{
				BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(item, m_knockbackType, currentTarget.AimDirection, centerOfShape, m_knockbackDistance);
				num = AddMovementArrowWithPrevious(item, path, TargeterMovementType.Knockback, num);
			}
		}
		SetMovementArrowEnabledFromIndex(num, false);
	}

	protected override bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin, ActorData bestTarget)
	{
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.occupant != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						{
							bool flag = potentialTarget.GetTeam() == targetingActor.GetTeam();
							if (flag)
							{
								if (boardSquareSafe.occupant == potentialTarget.gameObject)
								{
									AddActorInRange(potentialTarget, damageOrigin, targetingActor);
									goto IL_00a4;
								}
							}
							if (!flag)
							{
								AddActorInRange(potentialTarget, damageOrigin, targetingActor, AbilityTooltipSubject.Enemy);
							}
							goto IL_00a4;
						}
						IL_00a4:
						return true;
					}
				}
			}
		}
		return false;
	}
}
