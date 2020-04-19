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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SniperPenetratingRound.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
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
							if (this.ActorMeetKnockbackConditions(actorData, targetingActor))
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
								float num2 = VectorUtils.HorizontalPlaneDistInSquares(actorData.\u0016(), targetingActor.\u0016());
								bool flag;
								if (this.m_knockbackThresholdDistance > 0f)
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
									flag = (num2 < this.m_knockbackThresholdDistance);
								}
								else
								{
									flag = true;
								}
								bool flag2 = flag;
								if (flag2)
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
									BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, currentTarget.AimDirection, targetingActor.\u0016(), this.m_knockbackDistance);
									num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
								}
							}
						}
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SniperPenetratingRound.ActorMeetKnockbackConditions(ActorData, ActorData)).MethodHandle;
			}
			return this.m_knockbackThresholdDistance <= 0f || VectorUtils.HorizontalPlaneDistInSquares(target.\u0016(), caster.\u0016()) < this.m_knockbackThresholdDistance;
		}
		return false;
	}
}
