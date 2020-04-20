using System;
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

	public AbilityUtil_Targeter_AoE_AroundActor(Ability ability, float radius, bool penetrateLoS, bool aoeAffectsEnemies = true, bool aoeAffectsAllies = false, int maxTargets = -1, bool canTargetOnEnemy = false, bool canTargetOnAlly = true, bool canTargetOnSelf = true) : base(ability, radius, penetrateLoS, aoeAffectsEnemies, aoeAffectsAllies, maxTargets)
	{
		this.m_canTargetOnEnemy = canTargetOnEnemy;
		this.m_canTargetOnAlly = canTargetOnAlly;
		this.m_canTargetOnSelf = canTargetOnSelf;
	}

	protected override Vector3 GetRefPos(AbilityTarget currentTarget, ActorData targetingActor, float range)
	{
		Vector3 result = base.GetRefPos(currentTarget, targetingActor, range);
		if (this.m_lockToGridPos && Board.Get() != null && currentTarget != null)
		{
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
		this.m_lastCenterActor = null;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(this.GetRefPos(currentTarget, targetingActor, this.GetCurrentRangeInSquares()));
		ActorData occupantActor = boardSquare.OccupantActor;
		if (occupantActor != null)
		{
			if (occupantActor == targetingActor)
			{
				if (this.m_canTargetOnSelf)
				{
					base.AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, this.m_allyOccupantSubject, false);
					this.m_lastCenterActor = occupantActor;
					return;
				}
			}
			if (occupantActor.GetTeam() == targetingActor.GetTeam())
			{
				if (this.m_canTargetOnAlly)
				{
					base.AddActorInRange(occupantActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, this.m_allyOccupantSubject, false);
					this.m_lastCenterActor = occupantActor;
					return;
				}
			}
			if (occupantActor.GetTeam() != targetingActor.GetTeam() && this.m_canTargetOnEnemy)
			{
				base.AddActorInRange(occupantActor, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, this.m_enemyOccupantSubject, false);
				this.m_lastCenterActor = occupantActor;
			}
		}
	}
}
