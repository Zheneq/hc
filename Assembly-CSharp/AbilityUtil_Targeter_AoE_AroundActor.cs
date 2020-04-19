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
		if (this.m_lockToGridPos && Board.\u000E() != null && currentTarget != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_AroundActor.GetRefPos(AbilityTarget, ActorData, float)).MethodHandle;
			}
			BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
			if (boardSquare != null)
			{
				result = boardSquare.ToVector3();
			}
		}
		return result;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		this.m_lastCenterActor = null;
		BoardSquare boardSquare = Board.\u000E().\u000E(this.GetRefPos(currentTarget, targetingActor, this.GetCurrentRangeInSquares()));
		ActorData occupantActor = boardSquare.OccupantActor;
		if (occupantActor != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_AoE_AroundActor.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			if (occupantActor == targetingActor)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_canTargetOnSelf)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					base.AddActorInRange(targetingActor, targetingActor.\u0016(), targetingActor, this.m_allyOccupantSubject, false);
					this.m_lastCenterActor = occupantActor;
					return;
				}
			}
			if (occupantActor.\u000E() == targetingActor.\u000E())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_canTargetOnAlly)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					base.AddActorInRange(occupantActor, targetingActor.\u0016(), targetingActor, this.m_allyOccupantSubject, false);
					this.m_lastCenterActor = occupantActor;
					return;
				}
			}
			if (occupantActor.\u000E() != targetingActor.\u000E() && this.m_canTargetOnEnemy)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				base.AddActorInRange(occupantActor, targetingActor.\u0016(), targetingActor, this.m_enemyOccupantSubject, false);
				this.m_lastCenterActor = occupantActor;
			}
		}
	}
}
