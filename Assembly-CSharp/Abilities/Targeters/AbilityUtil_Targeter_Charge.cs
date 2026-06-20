using System.Collections.Generic;

public class AbilityUtil_Targeter_Charge : AbilityUtil_Targeter_Shape
{
    public bool m_forceChase;

    public bool AllowChargeThroughInvalidSquares { get; set; }

    public AbilityUtil_Targeter_Charge(
        Ability ability,
        AbilityAreaShape shape,
        bool shapePenetratesLoS,
        DamageOriginType damageOriginType,
        bool affectsEnemies = true,
        bool affectsAllies = false)
        : base(ability, shape, shapePenetratesLoS, damageOriginType, affectsEnemies, affectsAllies)
    {
        m_showArcToShape = false;
        AllowChargeThroughInvalidSquares = false;
    }

    public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
    {
        UpdateTargetingMultiTargets(currentTarget, targetingActor, 0, null);
    }

    public override void UpdateTargetingMultiTargets(
        AbilityTarget currentTarget,
        ActorData targetingActor,
        int currentTargetIndex,
        List<AbilityTarget> targets)
    {
        base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
        BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
        BoardSquarePathInfo path = null;
        if (targetSquare != null
            && (currentTargetIndex == 0 || targets == null || !IsUsingMultiTargetUpdate()))
        {
            path = KnockbackUtils.BuildStraightLineChargePath(
                targetingActor,
                targetSquare,
                targetingActor.GetCurrentBoardSquare(),
                AllowChargeThroughInvalidSquares);
        }
        else if (targetSquare != null)
        {
            BoardSquare prevTargetSquare = Board.Get().GetSquare(targets[currentTargetIndex - 1].GridPos);
            path = KnockbackUtils.BuildStraightLineChargePath(
                targetingActor,
                targetSquare,
                prevTargetSquare,
                AllowChargeThroughInvalidSquares);
        }

        EnableAllMovementArrows();
        int fromIndex = AddMovementArrowWithPrevious(targetingActor, path, TargeterMovementType.Movement, 0);
        SetMovementArrowEnabledFromIndex(fromIndex, false);
    }
}