using System.Collections.Generic;

public class TutorialDash : Ability
{
    private void Start()
    {
        Targeter = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0f, 0, false, false);
    }

    internal override ActorData.MovementType GetMovementType()
    {
        return ActorData.MovementType.Charge;
    }

    public override bool CustomTargetValidation(
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
        BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
        return KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null;
    }
}