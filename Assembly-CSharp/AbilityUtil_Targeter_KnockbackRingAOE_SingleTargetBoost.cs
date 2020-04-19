using System;
using System.Collections.Generic;

public class AbilityUtil_Targeter_KnockbackRingAOE_SingleTargetBoost : AbilityUtil_Targeter_KnockbackRingAoE
{
	private float m_knockbackDistanceIfSingleTarget = 2f;

	public AbilityUtil_Targeter_KnockbackRingAOE_SingleTargetBoost(Ability ability, AbilityAreaShape aoeShape, bool aoePenetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType aoeOriginType, bool aoeAffectEnemies, bool aoeAffectAllies, AbilityUtil_Targeter.AffectsActor aoeAffectsCaster, AbilityUtil_Targeter.AffectsActor affectsActorOnCurrentGridPos, AbilityAreaShape knockbackShape, float knockbackDistance, KnockbackType knockbackType, bool knockbackAdjacentActorsIfPull, bool knockbackEnemies, float knockbackDistanceIfSingleTarget) : base(ability, aoeShape, aoePenetrateLos, aoeOriginType, aoeAffectEnemies, aoeAffectAllies, aoeAffectsCaster, affectsActorOnCurrentGridPos, knockbackShape, knockbackDistance, knockbackType, knockbackAdjacentActorsIfPull, knockbackEnemies)
	{
		this.m_knockbackDistanceIfSingleTarget = knockbackDistanceIfSingleTarget;
	}

	protected override float GetKnockbackDistance(List<ActorData> knockbackTargets)
	{
		if (knockbackTargets.Count == 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_KnockbackRingAOE_SingleTargetBoost.GetKnockbackDistance(List<ActorData>)).MethodHandle;
			}
			return this.m_knockbackDistanceIfSingleTarget;
		}
		return base.GetKnockbackDistance(knockbackTargets);
	}
}
