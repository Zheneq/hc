public class AbilityUtil_Targeter_BarrierWithCharge : AbilityUtil_Targeter_Barrier
{
	public AbilityUtil_Targeter_BarrierWithCharge(Ability ability, float width, bool snapToBorder = false)
		: base(ability, width, snapToBorder)
	{
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(currentTarget.GridPos);
		int fromIndex = 0;
		if (boardSquareSafe != null)
		{
			BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquareSafe);
			fromIndex = AddMovementArrowWithPrevious(targetingActor, path, TargeterMovementType.Movement, 0);
		}
		SetMovementArrowEnabledFromIndex(fromIndex, false);
	}
}
