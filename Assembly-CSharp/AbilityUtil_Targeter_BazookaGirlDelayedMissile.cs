using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BazookaGirlDelayedMissile : AbilityUtil_Targeter_Shape
{
	private bool m_useInnerShape;

	private AbilityAreaShape m_innerShape;

	public AbilityUtil_Targeter_BazookaGirlDelayedMissile(Ability ability, AbilityAreaShape outerShape, bool penetrateLoS, bool useInnerShape, AbilityAreaShape innerShape, AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType = AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, bool affectsEnemies = true, bool affectsAllies = false) : base(ability, outerShape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible)
	{
		this.m_useInnerShape = useInnerShape;
		this.m_innerShape = innerShape;
	}

	protected override bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin, ActorData bestTarget)
	{
		base.HandleAddActorInShape(potentialTarget, targetingActor, currentTarget, damageOrigin, bestTarget);
		if (this.m_useInnerShape)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BazookaGirlDelayedMissile.HandleAddActorInShape(ActorData, ActorData, AbilityTarget, Vector3, ActorData)).MethodHandle;
			}
			List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_innerShape, currentTarget, this.m_penetrateLoS, targetingActor, base.GetAffectedTeams(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
			if (actorsInShape.Contains(potentialTarget))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				base.AddActorInRange(potentialTarget, damageOrigin, targetingActor, AbilityTooltipSubject.Near, true);
			}
		}
		return true;
	}
}
