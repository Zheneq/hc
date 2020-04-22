using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_BazookaGirlDelayedMissile : AbilityUtil_Targeter_Shape
{
	private bool m_useInnerShape;

	private AbilityAreaShape m_innerShape;

	public AbilityUtil_Targeter_BazookaGirlDelayedMissile(Ability ability, AbilityAreaShape outerShape, bool penetrateLoS, bool useInnerShape, AbilityAreaShape innerShape, DamageOriginType damageOriginType = DamageOriginType.CenterOfShape, bool affectsEnemies = true, bool affectsAllies = false)
		: base(ability, outerShape, penetrateLoS, damageOriginType, affectsEnemies, affectsAllies)
	{
		m_useInnerShape = useInnerShape;
		m_innerShape = innerShape;
	}

	protected override bool HandleAddActorInShape(ActorData potentialTarget, ActorData targetingActor, AbilityTarget currentTarget, Vector3 damageOrigin, ActorData bestTarget)
	{
		base.HandleAddActorInShape(potentialTarget, targetingActor, currentTarget, damageOrigin, bestTarget);
		if (m_useInnerShape)
		{
			while (true)
			{
				switch (1)
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
			List<ActorData> actors = AreaEffectUtils.GetActorsInShape(m_innerShape, currentTarget, m_penetrateLoS, targetingActor, GetAffectedTeams(), null);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			if (actors.Contains(potentialTarget))
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
				AddActorInRange(potentialTarget, damageOrigin, targetingActor, AbilityTooltipSubject.Near, true);
			}
		}
		return true;
	}
}
