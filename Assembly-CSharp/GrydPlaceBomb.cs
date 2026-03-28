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
}