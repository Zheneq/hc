using System.Collections.Generic;

public class AbilityUtil_Targeter_Charge : AbilityUtil_Targeter_Shape
{
	public bool m_forceChase;

	public bool AllowChargeThroughInvalidSquares
	{
		get;
		set;
	}

	public AbilityUtil_Targeter_Charge(Ability ability, AbilityAreaShape shape, bool shapePenetratesLoS, DamageOriginType damageOriginType, bool affectsEnemies = true, bool affectsAllies = false)
		: base(ability, shape, shapePenetratesLoS, damageOriginType, affectsEnemies, affectsAllies)
	{
		m_showArcToShape = false;
		AllowChargeThroughInvalidSquares = false;
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		BoardSquarePathInfo path = null;
		BoardSquare boardSquareSafe = Board.Get().GetSquare(currentTarget.GridPos);
		if (!(boardSquareSafe != null))
		{
			goto IL_0077;
		}
		if (currentTargetIndex != 0)
		{
			if (targets != null)
			{
				if (IsUsingMultiTargetUpdate())
				{
					goto IL_0077;
				}
			}
		}
		path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe, targetingActor.GetCurrentBoardSquare(), AllowChargeThroughInvalidSquares);
		goto IL_00bb;
		IL_0077:
		if (boardSquareSafe != null)
		{
			BoardSquare boardSquareSafe2 = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
			path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe, boardSquareSafe2, AllowChargeThroughInvalidSquares);
		}
		goto IL_00bb;
		IL_00bb:
		EnableAllMovementArrows();
		int fromIndex = AddMovementArrowWithPrevious(targetingActor, path, TargeterMovementType.Movement, 0);
		SetMovementArrowEnabledFromIndex(fromIndex, false);
	}
}
