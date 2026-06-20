using System.Collections.Generic;

public class AbilityUtil_Targeter_SniperPenetratingRound : AbilityUtil_Targeter_Laser
{
	private bool m_knockbackNearbyEnemies;
	private float m_knockbackThresholdDistance = -1f;
	private KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
	private float m_knockbackDistance;

	public AbilityUtil_Targeter_SniperPenetratingRound(
		Ability ability,
		float width,
		float distance,
		bool penetrateLos,
		int maxTargets,
		bool shouldKnockback,
		float knockbackThresholdDistance,
		KnockbackType knockbackType,
		float knockbackDistance)
		: base(ability, width, distance, penetrateLos, maxTargets)
	{
		m_knockbackNearbyEnemies = shouldKnockback;
		m_knockbackThresholdDistance = knockbackThresholdDistance;
		m_knockbackType = knockbackType;
		m_knockbackDistance = knockbackDistance;
	}

	public AbilityUtil_Targeter_SniperPenetratingRound(
		Ability ability,
		float width,
		float distance,
		bool penetrateLoS,
		int maxTargets)
		: base(ability, width, distance, penetrateLoS, maxTargets)
	{
		m_knockbackNearbyEnemies = false;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (m_knockbackNearbyEnemies && m_knockbackDistance > 0f)
		{
			int arrowIndex = 0;
			EnableAllMovementArrows();
			List<ActorData> visibleActorsInRange = GetVisibleActorsInRange();
			foreach (ActorData actor in visibleActorsInRange)
			{
				if (actor.GetTeam() != targetingActor.GetTeam() && ActorMeetKnockbackConditions(actor, targetingActor))
				{
					float dist = VectorUtils.HorizontalPlaneDistInSquares(actor.GetFreePos(), targetingActor.GetFreePos());
					if (m_knockbackThresholdDistance <= 0f || dist < m_knockbackThresholdDistance)
					{
						BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(
							actor,
							m_knockbackType,
							currentTarget.AimDirection,
							targetingActor.GetFreePos(),
							m_knockbackDistance);
						arrowIndex = AddMovementArrowWithPrevious(actor, path, TargeterMovementType.Knockback, arrowIndex);
					}
				}
			}

			SetMovementArrowEnabledFromIndex(arrowIndex, false);
		}
	}

	private bool ActorMeetKnockbackConditions(ActorData target, ActorData caster)
	{
		return m_knockbackNearbyEnemies
		       && m_knockbackDistance > 0f
		       && (m_knockbackThresholdDistance <= 0f
		           || VectorUtils.HorizontalPlaneDistInSquares(target.GetFreePos(), caster.GetFreePos()) < m_knockbackThresholdDistance);
	}
}
