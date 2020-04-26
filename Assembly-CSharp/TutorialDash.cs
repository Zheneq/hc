using System.Collections.Generic;

public class TutorialDash : Ability
{
	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0f, 0, false, false);
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null;
	}
}
