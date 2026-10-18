// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

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
    
#if SERVER
    // added in rogues
    public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
        Vector3 loSCheckPos = caster.GetLoSCheckPos(squareAtPhaseStart);
        BoardSquare startSquare = Board.Get().GetSquareFromVec3(loSCheckPos);
        return new ServerClientUtils.SequenceStartData(
            m_sequencePrefab,
            startSquare,
            new List<ActorData>().ToArray(),
            caster,
            additionalData.m_sequenceSource);
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
    }
#endif
}