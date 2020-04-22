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

	public AbilityUtil_Targeter_KnockbackRingAoE(Ability ability, AbilityAreaShape aoeShape, bool aoePenetrateLos, DamageOriginType aoeOriginType, bool aoeAffectEnemies, bool aoeAffectAllies, AffectsActor aoeAffectsCaster, AffectsActor affectsTargetOnGridposSquare, AbilityAreaShape knockbackShape, float knockbackDistance, KnockbackType knockbackType, bool knockbackAdjacentActorsIfPull, bool knockbackEnemies)
		: base(ability, aoeShape, aoePenetrateLos, aoeOriginType, aoeAffectEnemies, aoeAffectAllies, aoeAffectsCaster, affectsTargetOnGridposSquare)
	{
		m_aoeShape = aoeShape;
		m_penetrateLineOfSight = aoePenetrateLos;
		m_knockbackType = knockbackType;
		m_knockbackDistance = knockbackDistance;
		m_knockbackAdjacentActorsIfPull = knockbackAdjacentActorsIfPull;
		m_knockbackEnemies = knockbackEnemies;
		m_knockbackShape = knockbackShape;
	}

	protected virtual float GetKnockbackDistance(List<ActorData> knockbackTargets)
	{
		return m_knockbackDistance;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		List<ActorData> knockbackHitActors = GetKnockbackHitActors(currentTarget, targetingActor);
		float knockbackDistance = GetKnockbackDistance(knockbackHitActors);
		if (knockbackDistance > 0f)
		{
			Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, currentTarget);
			int num = 0;
			SetMovementArrowEnabledFromIndex(0, true);
			using (List<ActorData>.Enumerator enumerator = knockbackHitActors.GetEnumerator())
			{
				ActorData current;
				BoardSquarePathInfo boardSquarePathInfo;
				for (; enumerator.MoveNext(); num = AddMovementArrowWithPrevious(current, boardSquarePathInfo, TargeterMovementType.Knockback, num))
				{
					current = enumerator.Current;
					boardSquarePathInfo = null;
					if (m_knockbackType == KnockbackType.PullToSource && m_knockbackAdjacentActorsIfPull)
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
						if (Board.Get()._000E(current.GetCurrentBoardSquare(), targetingActor.GetCurrentBoardSquare()))
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							Vector3 aimDir = targetingActor.GetTravelBoardSquareWorldPosition() - current.GetTravelBoardSquareWorldPosition();
							aimDir.y = 0f;
							float distance = 2f;
							if (Board.Get()._0015(current.GetCurrentBoardSquare(), targetingActor.GetCurrentBoardSquare()))
							{
								distance = 2.82f;
							}
							boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(current, KnockbackType.ForwardAlongAimDir, aimDir, centerOfShape, distance);
							continue;
						}
					}
					boardSquarePathInfo = KnockbackUtils.BuildKnockbackPath(current, m_knockbackType, currentTarget.AimDirection, centerOfShape, knockbackDistance);
				}
				while (true)
				{
					switch (7)
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

	private List<ActorData> GetKnockbackHitActors(AbilityTarget targeterTarget, ActorData caster)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, false, m_knockbackEnemies);
		List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_knockbackShape, targeterTarget, m_penetrateLineOfSight, caster, relevantTeams, null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		return actors;
	}
}
