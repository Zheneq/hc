using System;

public class AbilityUtil_Targeter_BarrierWithCharge : AbilityUtil_Targeter_Barrier
{
	public AbilityUtil_Targeter_BarrierWithCharge(Ability ability, float width, bool snapToBorder = false) : base(ability, width, snapToBorder, false, true)
	{
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		BoardSquare boardSquare = Board.\u000E().\u000E(currentTarget.GridPos);
		int fromIndex = 0;
		if (boardSquare != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityUtil_Targeter_BarrierWithCharge.UpdateTargeting(AbilityTarget, ActorData)).MethodHandle;
			}
			BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(targetingActor, boardSquare);
			fromIndex = base.AddMovementArrowWithPrevious(targetingActor, path, AbilityUtil_Targeter.TargeterMovementType.Movement, 0, false);
		}
		base.SetMovementArrowEnabledFromIndex(fromIndex, false);
	}
}
