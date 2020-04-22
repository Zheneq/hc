using UnityEngine;

public class AbilityUtil_Targeter_AoE_AroundActor : AbilityUtil_Targeter_AoE_Smooth
{
	private bool m_canTargetOnAlly;

	private bool m_canTargetOnEnemy;

	private bool m_canTargetOnSelf;

	private bool m_lockToGridPos = true;

	public ActorData m_lastCenterActor;

	public AbilityTooltipSubject m_allyOccupantSubject = AbilityTooltipSubject.Ally;

	public AbilityTooltipSubject m_enemyOccupantSubject = AbilityTooltipSubject.Primary;

	public AbilityUtil_Targeter_AoE_AroundActor(Ability ability, float radius, bool penetrateLoS, bool aoeAffectsEnemies = true, bool aoeAffectsAllies = false, int maxTargets = -1, bool canTargetOnEnemy = false, bool canTargetOnAlly = true, bool canTargetOnSelf = true)
		: base(ability, radius, penetrateLoS, aoeAffectsEnemies, aoeAffectsAllies, maxTargets)
	{
		m_canTargetOnEnemy = canTargetOnEnemy;
		m_canTargetOnAlly = canTargetOnAlly;
		m_canTargetOnSelf = canTargetOnSelf;
	}

	protected override Vector3 GetRefPos(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		Vector3 result = base.GetRefPos(currentTarget, targetingActor, range);
		if (m_lockToGridPos && Board.Get() != null && currentTarget != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
			if (boardSquareSafe != null)
			{
				result = boardSquareSafe.ToVector3();
			}
		}
		return result;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		m_lastCenterActor = null;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(GetRefPos(currentTarget, targetingActor, GetCurrentRangeInSquares()));
		ActorData occupantActor = boardSquare.OccupantActor;
		if (!(occupantActor != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (occupantActor == targetingActor)
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
				if (m_canTargetOnSelf)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, m_allyOccupantSubject);
							m_lastCenterActor = occupantActor;
							return;
						}
					}
				}
			}
			if (occupantActor.GetTeam() == targetingActor.GetTeam())
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
				if (m_canTargetOnAlly)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							AddActorInRange(occupantActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, m_allyOccupantSubject);
							m_lastCenterActor = occupantActor;
							return;
						}
					}
				}
			}
			if (occupantActor.GetTeam() != targetingActor.GetTeam() && m_canTargetOnEnemy)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					AddActorInRange(occupantActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, m_enemyOccupantSubject);
					m_lastCenterActor = occupantActor;
					return;
				}
			}
			return;
		}
	}
}
