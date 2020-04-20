using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_KnockbackRingAoE : AbilityUtil_Targeter_Shape
{
	public AbilityAreaShape m_aoeShape = AbilityAreaShape.Three_x_Three;

	public bool m_penetrateLineOfSight;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public float m_knockbackDistance = 3f;

	public bool m_knockbackAdjacentActorsIfPull = true;

	public bool m_knockbackEnemies = true;

	public AbilityAreaShape m_knockbackShape = AbilityAreaShape.Five_x_Five;

	public AbilityUtil_Targeter_KnockbackRingAoE(Ability ability, AbilityAreaShape aoeShape, bool aoePenetrateLos, AbilityUtil_Targeter_Shape.DamageOriginType aoeOriginType, bool aoeAffectEnemies, bool aoeAffectAllies, AbilityUtil_Targeter.AffectsActor aoeAffectsCaster, AbilityUtil_Targeter.AffectsActor affectsTargetOnGridposSquare, AbilityAreaShape knockbackShape, float knockbackDistance, KnockbackType knockbackType, bool knockbackAdjacentActorsIfPull, bool knockbackEnemies) : base(ability, aoeShape, aoePenetrateLos, aoeOriginType, aoeAffectEnemies, aoeAffectAllies, aoeAffectsCaster, affectsTargetOnGridposSquare)
	{
		this.m_aoeShape = aoeShape;
		this.m_penetrateLineOfSight = aoePenetrateLos;
		this.m_knockbackType = knockbackType;
		this.m_knockbackDistance = knockbackDistance;
		this.m_knockbackAdjacentActorsIfPull = knockbackAdjacentActorsIfPull;
		this.m_knockbackEnemies = knockbackEnemies;
		this.m_knockbackShape = knockbackShape;
	}

	protected virtual float GetKnockbackDistance(List<ActorData> knockbackTargets)
	{
		return this.m_knockbackDistance;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		List<ActorData> knockbackHitActors = this.GetKnockbackHitActors(currentTarget, targetingActor);
		float knockbackDistance = this.GetKnockbackDistance(knockbackHitActors);
		if (knockbackDistance > 0f)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(this.m_shape, currentTarget);
			int num = 0;
			base.SetMovementArrowEnabledFromIndex(0, true);
			using (List<ActorData>.Enumerator enumerator = knockbackHitActors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (this.m_knockbackType != KnockbackType.PullToSource || !this.m_knockbackAdjacentActorsIfPull)
					{
						goto IL_119;
					}
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_KnockbackRingAoE.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
					}
					if (!Board.Get().\u000E(actorData.GetCurrentBoardSquare(), targetingActor.GetCurrentBoardSquare()))
					{
						goto IL_119;
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
					Vector3 aimDir = targetingActor.GetTravelBoardSquareWorldPosition() - actorData.GetTravelBoardSquareWorldPosition();
					aimDir.y = 0f;
					float distance = 2f;
					if (Board.Get().\u0015(actorData.GetCurrentBoardSquare(), targetingActor.GetCurrentBoardSquare()))
					{
						distance = 2.82f;
					}
					BoardSquarePathInfo path = KnockbackUtils.BuildKnockbackPath(actorData, KnockbackType.ForwardAlongAimDir, aimDir, centerOfShape, distance);
					IL_134:
					num = base.AddMovementArrowWithPrevious(actorData, path, AbilityUtil_Targeter.TargeterMovementType.Knockback, num, false);
					continue;
					IL_119:
					path = KnockbackUtils.BuildKnockbackPath(actorData, this.m_knockbackType, currentTarget.AimDirection, centerOfShape, knockbackDistance);
					goto IL_134;
				}
				for (;;)
				{
					switch (7)
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

	private List<ActorData> GetKnockbackHitActors(AbilityTarget targeterTarget, ActorData caster)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, false, this.m_knockbackEnemies);
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(this.m_knockbackShape, targeterTarget, this.m_penetrateLineOfSight, caster, relevantTeams, null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInShape);
		return actorsInShape;
	}
}
