using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_AllVisible : AbilityUtil_Targeter
{
	public AbilityUtil_Targeter_AllVisible.ShouldAddActorDelegate m_shouldAddActorDelegate;

	private AbilityUtil_Targeter_AllVisible.DamageOriginType m_damageOriginType;

	public AbilityUtil_Targeter_AllVisible(Ability ability, bool includeEnemies, bool includeAllies, bool includeSelf, AbilityUtil_Targeter_AllVisible.DamageOriginType damageOriginType = AbilityUtil_Targeter_AllVisible.DamageOriginType.CasterPos) : base(ability)
	{
		this.m_affectsEnemies = includeEnemies;
		this.m_affectsAllies = includeAllies;
		this.m_affectsTargetingActor = includeSelf;
		this.m_damageOriginType = damageOriginType;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				List<ActorData> actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData, true);
				for (int i = 0; i < actorsVisibleToActor.Count; i++)
				{
					ActorData actorData = actorsVisibleToActor[i];
					if (!actorData.IsDead())
					{
						if (!actorData.IgnoreForAbilityHits)
						{
							if (actorData == targetingActor && this.m_affectsTargetingActor)
							{
								goto IL_11E;
							}
							if (actorData != targetingActor && actorData.GetTeam() == targetingActor.GetTeam())
							{
								if (this.m_affectsAllies)
								{
									goto IL_11E;
								}
							}
							bool flag;
							if (actorData.GetTeam() != targetingActor.GetTeam())
							{
								flag = this.m_affectsEnemies;
							}
							else
							{
								flag = false;
							}
							IL_11F:
							bool flag2 = flag;
							if (flag2)
							{
								if (this.m_shouldAddActorDelegate != null)
								{
									if (!this.m_shouldAddActorDelegate(actorData, targetingActor))
									{
										goto IL_180;
									}
								}
								Vector3 travelBoardSquareWorldPosition;
								if (this.m_damageOriginType == AbilityUtil_Targeter_AllVisible.DamageOriginType.CasterPos)
								{
									travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
								}
								else
								{
									travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
								}
								Vector3 damageOrigin = travelBoardSquareWorldPosition;
								base.AddActorInRange(actorData, damageOrigin, targetingActor, AbilityTooltipSubject.Primary, false);
								goto IL_180;
							}
							goto IL_180;
							IL_11E:
							flag = true;
							goto IL_11F;
						}
					}
					IL_180:;
				}
			}
		}
	}

	public delegate bool ShouldAddActorDelegate(ActorData potentialActor, ActorData caster);

	public enum DamageOriginType
	{
		CasterPos,
		TargetPos
	}
}
