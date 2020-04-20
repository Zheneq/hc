using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SamuraiShowdown : AbilityUtil_Targeter_ChargeAoE
{
	private float m_knockbackDist;

	private KnockbackType m_knockbackType;

	public AbilityUtil_Targeter_SamuraiShowdown(Ability ability, float radiusAroundStart, float radiusAroundEnd, float rangeFromDir, int maxTargets, bool ignoreTargetsCover, bool penetrateLoS, float knockbackDist, KnockbackType knockbackType) : base(ability, radiusAroundStart, radiusAroundEnd, rangeFromDir, maxTargets, ignoreTargetsCover, penetrateLoS)
	{
		this.m_knockbackDist = knockbackDist;
		this.m_knockbackType = knockbackType;
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		Vector3 travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
		Vector3 normalized = (currentTarget.GetWorldGridPos() - travelBoardSquareWorldPosition).normalized;
		int num = 1;
		base.EnableAllMovementArrows();
		List<AbilityUtil_Targeter.ActorTarget> actorsInRange = base.GetActorsInRange();
		using (List<AbilityUtil_Targeter.ActorTarget>.Enumerator enumerator = actorsInRange.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityUtil_Targeter.ActorTarget actorTarget = enumerator.Current;
				if (actorTarget.m_actor.GetTeam() != targetingActor.GetTeam())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_SamuraiShowdown.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
					}
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorTarget.m_actor, this.m_knockbackType, normalized, travelBoardSquareWorldPosition, this.m_knockbackDist);
					num = base.AddMovementArrowWithPrevious(actorTarget.m_actor, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
				}
			}
			for (;;)
			{
				switch (3)
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
