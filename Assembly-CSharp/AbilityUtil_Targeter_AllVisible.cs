using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_AllVisible : AbilityUtil_Targeter
{
	public delegate bool ShouldAddActorDelegate(ActorData potentialActor, ActorData caster);

	public enum DamageOriginType
	{
		CasterPos,
		TargetPos
	}

	public ShouldAddActorDelegate m_shouldAddActorDelegate;

	private DamageOriginType m_damageOriginType;

	public AbilityUtil_Targeter_AllVisible(Ability ability, bool includeEnemies, bool includeAllies, bool includeSelf, DamageOriginType damageOriginType = DamageOriginType.CasterPos)
		: base(ability)
	{
		m_affectsEnemies = includeEnemies;
		m_affectsAllies = includeAllies;
		m_affectsTargetingActor = includeSelf;
		m_damageOriginType = damageOriginType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		if (!(GameFlowData.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (!(GameFlowData.Get().activeOwnedActorData != null))
			{
				return;
			}
			while (true)
			{
				List<ActorData> actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
				for (int i = 0; i < actorsVisibleToActor.Count; i++)
				{
					ActorData actorData = actorsVisibleToActor[i];
					if (actorData.IsDead())
					{
						continue;
					}
					if (actorData.IgnoreForAbilityHits)
					{
						continue;
					}
					if (actorData == targetingActor && m_affectsTargetingActor)
					{
						goto IL_011e;
					}
					if (actorData != targetingActor && actorData.GetTeam() == targetingActor.GetTeam())
					{
						if (m_affectsAllies)
						{
							goto IL_011e;
						}
					}
					int num;
					if (actorData.GetTeam() != targetingActor.GetTeam())
					{
						num = (m_affectsEnemies ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					goto IL_011f;
					IL_011e:
					num = 1;
					goto IL_011f;
					IL_011f:
					if (num == 0)
					{
						continue;
					}
					if (m_shouldAddActorDelegate != null)
					{
						if (!m_shouldAddActorDelegate(actorData, targetingActor))
						{
							continue;
						}
					}
					Vector3 travelBoardSquareWorldPosition;
					if (m_damageOriginType == DamageOriginType.CasterPos)
					{
						travelBoardSquareWorldPosition = targetingActor.GetFreePos();
					}
					else
					{
						travelBoardSquareWorldPosition = actorData.GetFreePos();
					}
					Vector3 damageOrigin = travelBoardSquareWorldPosition;
					AddActorInRange(actorData, damageOrigin, targetingActor);
				}
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}
}
