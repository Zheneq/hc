using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_SamuraiShowdown : AbilityUtil_Targeter_ChargeAoE
{
	private float m_knockbackDist;
	private KnockbackType m_knockbackType;

	public AbilityUtil_Targeter_SamuraiShowdown(
		Ability ability,
		float radiusAroundStart,
		float radiusAroundEnd,
		float rangeFromDir,
		int maxTargets,
		bool ignoreTargetsCover,
		bool penetrateLoS,
		float knockbackDist,
		KnockbackType knockbackType)
		: base(ability, radiusAroundStart, radiusAroundEnd, rangeFromDir, maxTargets, ignoreTargetsCover, penetrateLoS)
	{
		m_knockbackDist = knockbackDist;
		m_knockbackType = knockbackType;
	}

	public override void UpdateTargetingMultiTargets(
		AbilityTarget currentTarget,
		ActorData targetingActor,
		int currentTargetIndex,
		List<AbilityTarget> targets)
	{
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		Vector3 casterPos = targetingActor.GetFreePos();
		Vector3 aimDir = (currentTarget.GetWorldGridPos() - casterPos).normalized;
		int arrowIndex = 1;
		EnableAllMovementArrows();
		foreach (ActorTarget actor in GetActorsInRange())
		{
			if (actor.m_actor.GetTeam() != targetingActor.GetTeam())
			{
				BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(
					actor.m_actor,
					m_knockbackType,
					aimDir,
					casterPos,
					m_knockbackDist);
				arrowIndex = AddMovementArrowWithPrevious(actor.m_actor, path, TargeterMovementType.Knockback, arrowIndex);
			}
		}
		SetMovementArrowEnabledFromIndex(arrowIndex, false);
	}
}
