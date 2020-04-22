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
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(GameFlowData.Get().activeOwnedActorData != null))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				List<ActorData> actorsVisibleToActor = GameFlowData.Get().GetActorsVisibleToActor(GameFlowData.Get().activeOwnedActorData);
				for (int i = 0; i < actorsVisibleToActor.Count; i++)
				{
					ActorData actorData = actorsVisibleToActor[i];
					if (actorData.IsDead())
					{
						continue;
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
					if (actorData.IgnoreForAbilityHits)
					{
						continue;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (actorData == targetingActor && m_affectsTargetingActor)
					{
						goto IL_011e;
					}
					if (actorData != targetingActor && actorData.GetTeam() == targetingActor.GetTeam())
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
						if (m_affectsAllies)
						{
							goto IL_011e;
						}
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					int num;
					if (actorData.GetTeam() != targetingActor.GetTeam())
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
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_shouldAddActorDelegate != null)
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
						if (!m_shouldAddActorDelegate(actorData, targetingActor))
						{
							continue;
						}
					}
					Vector3 travelBoardSquareWorldPosition;
					if (m_damageOriginType == DamageOriginType.CasterPos)
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
						travelBoardSquareWorldPosition = targetingActor.GetTravelBoardSquareWorldPosition();
					}
					else
					{
						travelBoardSquareWorldPosition = actorData.GetTravelBoardSquareWorldPosition();
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
