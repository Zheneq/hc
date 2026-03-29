// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class GrydPlaceBomb : Ability
{
    [Header("-- Targeting")]
    public bool m_lockToCardinalDirs = true;
    [Header("-- Enemy direct hit")]
    public bool m_explodeThisTurnOnDirectHit;
    [Header("-- Bomb explosion")]
    public int m_bombDuration;
    public int m_damageAmount;
    public float m_explosionLaserRange;
    public float m_explosionLaserWidth;
    [Header("-- Sequences")]
    public GameObject m_castSequencePrefab;
    public GameObject m_persistentBombSequencePrefab;
    public GameObject m_explodeBombSequencePrefab;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Place Bomb";
        }

        SetupTargeter();
    }

    private void SetupTargeter()
    {
        Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false);
    }

    public override bool CustomTargetValidation(
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
        GridPos start = targetIndex == 0 ? caster.GetGridPos() : currentTargets[targetIndex - 1].GridPos;
        if (m_lockToCardinalDirs && !CardinallyAligned(start, target.GridPos))
        {
            return false;
        }

        BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
        return targetSquare != null
               && targetSquare.IsValidForGameplay()
               && base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
    }

    private bool CardinallyAligned(GridPos start, GridPos end)
    {
        return !start.CoordsEqual(end) && (start.x == end.x || start.y == end.y);
    }

#if SERVER
    // added in rogues
    public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        return new ServerClientUtils.SequenceStartData(
            m_castSequencePrefab,
            Board.Get().GetSquare(targets[0].GridPos),
            additionalData.m_abilityResults.HitActorsArray(),
            caster,
            additionalData.m_sequenceSource);
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
        BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
        PositionHitResults positionHitResults =
            new PositionHitResults(new PositionHitParameters(targetSquare.ToVector3()));
        bool explodeFirstTurn = m_explodeThisTurnOnDirectHit
                                && targetSquare.OccupantActor != null
                                && targetSquare.OccupantActor.GetTeam() != caster.GetTeam();
        positionHitResults.AddEffect(
            new GrydBombEffect(
                AsEffectSource(),
                targetSquare,
                caster,
                m_damageAmount,
                m_explosionLaserRange,
                m_explosionLaserWidth,
                explodeFirstTurn,
                m_bombDuration,
                m_persistentBombSequencePrefab,
                m_explodeBombSequencePrefab,
                0));
        abilityResults.StorePositionHit(positionHitResults);
    }
#endif
}