using System.Collections.Generic;

public class AbilityUtil_Targeter_SniperPenetratingRound : AbilityUtil_Targeter_Laser
{
	private bool m_knockbackNearbyEnemies;

	private float m_knockbackThresholdDistance = -1f;

	private KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	private float m_knockbackDistance;

	public AbilityUtil_Targeter_SniperPenetratingRound(Ability ability, float width, float distance, bool penetrateLos, int maxTargets, bool shouldKnockback, float knockbackThresholdDistance, KnockbackType knockbackType, float knockbackDistance)
		: base(ability, width, distance, penetrateLos, maxTargets)
	{
		m_knockbackNearbyEnemies = shouldKnockback;
		m_knockbackThresholdDistance = knockbackThresholdDistance;
		m_knockbackType = knockbackType;
		m_knockbackDistance = knockbackDistance;
	}

	public AbilityUtil_Targeter_SniperPenetratingRound(Ability ability, float width, float distance, bool penetrateLoS, int maxTargets)
		: base(ability, width, distance, penetrateLoS, maxTargets)
	{
		m_knockbackNearbyEnemies = false;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		if (!m_knockbackNearbyEnemies)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_knockbackDistance > 0f)
			{
				int num = 0;
				EnableAllMovementArrows();
				List<ActorData> visibleActorsInRange = GetVisibleActorsInRange();
				using (List<ActorData>.Enumerator enumerator = visibleActorsInRange.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ActorData current = enumerator.Current;
						if (current.GetTeam() != targetingActor.GetTeam())
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (ActorMeetKnockbackConditions(current, targetingActor))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								float num2 = VectorUtils.HorizontalPlaneDistInSquares(current.GetTravelBoardSquareWorldPosition(), targetingActor.GetTravelBoardSquareWorldPosition());
								int num3;
								if (!(m_knockbackThresholdDistance <= 0f))
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									num3 = ((num2 < m_knockbackThresholdDistance) ? 1 : 0);
								}
								else
								{
									num3 = 1;
								}
								if (num3 != 0)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current, m_knockbackType, currentTarget.AimDirection, targetingActor.GetTravelBoardSquareWorldPosition(), m_knockbackDistance);
									num = AddMovementArrowWithPrevious(current, path, TargeterMovementType.Knockback, num);
								}
							}
						}
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				SetMovementArrowEnabledFromIndex(num, false);
			}
			return;
		}
	}

	private bool ActorMeetKnockbackConditions(ActorData target, ActorData caster)
	{
		if (m_knockbackNearbyEnemies && m_knockbackDistance > 0f)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_knockbackThresholdDistance <= 0f || VectorUtils.HorizontalPlaneDistInSquares(target.GetTravelBoardSquareWorldPosition(), caster.GetTravelBoardSquareWorldPosition()) < m_knockbackThresholdDistance;
				}
			}
		}
		return false;
	}
}
