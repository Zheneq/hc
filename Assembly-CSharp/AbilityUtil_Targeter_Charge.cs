using System;
using System.Collections.Generic;

public class AbilityUtil_Targeter_Charge : AbilityUtil_Targeter_Shape
{
	public bool m_forceChase;

	public AbilityUtil_Targeter_Charge(Ability ability, AbilityAreaShape shape, bool shapePenetratesLoS, AbilityUtil_Targeter_Shape.DamageOriginType damageOriginType, bool affectsEnemies = true, bool affectsAllies = false) : base(ability, shape, shapePenetratesLoS, damageOriginType, affectsEnemies, affectsAllies, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible)
	{
		this.m_showArcToShape = false;
		this.AllowChargeThroughInvalidSquares = false;
	}

	public bool AllowChargeThroughInvalidSquares { get; set; }

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		this.UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		BoardSquarePathInfo path = null;
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		if (boardSquare != null)
		{
			if (currentTargetIndex != 0)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_Charge.UpdateTargetingMultiTargets(AbilityTarget, ActorData, int, List<AbilityTarget>)).MethodHandle;
				}
				if (targets != null)
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
					if (this.IsUsingMultiTargetUpdate())
					{
						goto IL_77;
					}
				}
			}
			path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, targetingActor.\u0012(), this.AllowChargeThroughInvalidSquares);
			goto IL_BB;
		}
		IL_77:
		if (boardSquare != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			BoardSquare startSquare = Board.\u000E().\u000E(targets[currentTargetIndex - 1].GridPos);
			path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare, startSquare, this.AllowChargeThroughInvalidSquares);
		}
		IL_BB:
		base.EnableAllMovementArrows();
		int fromIndex = base.AddMovementArrowWithPrevious(targetingActor, path, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
		base.SetMovementArrowEnabledFromIndex(fromIndex, false);
	}
}
