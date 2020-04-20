using System;
using System.Collections.Generic;

public class AbilityUtil_Targeter_SniperPenetratingRound : AbilityUtil_Targeter_Laser
{
	private bool m_knockbackNearbyEnemies;

	private float m_knockbackThresholdDistance = -1f;

	private KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	private float m_knockbackDistance;

	public AbilityUtil_Targeter_SniperPenetratingRound(Ability ability, float width, float distance, bool penetrateLos, int maxTargets, bool shouldKnockback, float knockbackThresholdDistance, KnockbackType knockbackType, float knockbackDistance) : base(ability, width, distance, penetrateLos, maxTargets, false, false)
	{
		this.m_knockbackNearbyEnemies = shouldKnockback;
		this.m_knockbackThresholdDistance = knockbackThresholdDistance;
		this.m_knockbackType = knockbackType;
		this.m_knockbackDistance = knockbackDistance;
	}

	public AbilityUtil_Targeter_SniperPenetratingRound(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets) : base(ability, width, distance, penetrateLoS, maxTargets, false, false)
	{
		this.m_knockbackNearbyEnemies = false;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (this.m_knockbackNearbyEnemies)
		{
			if (this.m_knockbackDistance > 0f)
			{
				int num = 0;
				base.EnableAllMovementArrows();
				List<ActorData> visibleActorsInRange = this.GetVisibleActorsInRange();
				using (List<ActorData>.Enumerator enumerator = visibleActorsInRange.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData actorData = enumerator.Current;
						if (actorData.GetTeam() != targetingActor.GetTeam())
						{
							if (this.ActorMeetKnockbackConditions(actorData, targetingActor))
							{
								float num2 = VectorUtils.HorizontalPlaneDistInSquares(actorData.GetTravelBoardSquareWorldPosition(), targetingActor.GetTravelBoardSquareWorldPosition());
								bool flag;
								if (this.m_knockbackThresholdDistance > 0f)
								{
									flag = (num2 < this.m_knockbackThresholdDistance);
								}
								else
								{
									flag = true;
								}
								bool flag2 = flag;
								if (flag2)
								{
									BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, currentTarget.AimDirection, targetingActor.GetTravelBoardSquareWorldPosition(), this.m_knockbackDistance);
									num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
								}
							}
						}
					}
				}
				base.SetMovementArrowEnabledFromIndex(num, false);
			}
		}
	}

	private bool ActorMeetKnockbackConditions(ActorData target, ActorData caster)
	{
		if (this.m_knockbackNearbyEnemies && this.m_knockbackDistance > 0f)
		{
			return this.m_knockbackThresholdDistance <= 0f || VectorUtils.HorizontalPlaneDistInSquares(target.GetTravelBoardSquareWorldPosition(), caster.GetTravelBoardSquareWorldPosition()) < this.m_knockbackThresholdDistance;
		}
		return false;
	}
}
