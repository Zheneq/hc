using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SamuraiShowdown : AbilityUtil_Targeter_ChargeAoE
{
	private float m_knockbackDist;

	private KnockbackType m_knockbackType;

	public AbilityUtil_Targeter_SamuraiShowdown(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS, float knockbackDist, KnockbackType knockbackType)
		: base(ability, radiusAroundStart, radiusAroundEnd, rangeFromDir, maxTargets, ignoreTargetsCover, penetrateLoS)
	{
		m_knockbackDist = knockbackDist;
		m_knockbackType = knockbackType;
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		Vector3 normalized = (currentTarget.GetWorldGridPos() - travelBoardSquareWorldPosition).normalized;
		int num = 1;
		EnableAllMovementArrows();
		List<ActorTarget> actorsInRange = GetActorsInRange();
		using (List<ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorTarget current = enumerator.Current;
				if (current.m_actor.GetTeam() != targetingActor.GetTeam())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(current.m_actor, m_knockbackType, normalized, travelBoardSquareWorldPosition, m_knockbackDist);
					num = AddMovementArrowWithPrevious(current.m_actor, path, TargeterMovementType.Knockback, num);
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		SetMovementArrowEnabledFromIndex(num, false);
	}
}
