using System.Collections.Generic;

public class AbilityUtil_Targeter_KnockbackRingAOE_SingleTargetBoost : AbilityUtil_Targeter_KnockbackRingAoE
{
	private float m_knockbackDistanceIfSingleTarget = 2f;

	public AbilityUtil_Targeter_KnockbackRingAOE_SingleTargetBoost(Ability ability, AbilityAreaShape aoeShape, bool aoePenetrateLos, DamageOriginType aoeOriginType, bool aoeAffectEnemies, bool aoeAffectAllies, AffectsActor aoeAffectsCaster, AffectsActor affectsActorOnCurrentGridPos, AbilityAreaShape knockbackShape, float knockbackDistance, KnockbackType knockbackType, bool knockbackAdjacentActorsIfPull, bool knockbackEnemies, float knockbackDistanceIfSingleTarget)
		: base(ability, aoeShape, aoePenetrateLos, aoeOriginType, aoeAffectEnemies, aoeAffectAllies, aoeAffectsCaster, affectsActorOnCurrentGridPos, knockbackShape, knockbackDistance, knockbackType, knockbackAdjacentActorsIfPull, knockbackEnemies)
	{
		m_knockbackDistanceIfSingleTarget = knockbackDistanceIfSingleTarget;
	}

	protected override float GetKnockbackDistance(List<ActorData> knockbackTargets)
	{
		if (knockbackTargets.Count == 1)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return m_knockbackDistanceIfSingleTarget;
				}
			}
		}
		return base.GetKnockbackDistance(knockbackTargets);
	}
}
