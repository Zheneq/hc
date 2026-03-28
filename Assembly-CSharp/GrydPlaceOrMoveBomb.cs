using System.Collections.Generic;
using UnityEngine;

public class GrydPlaceOrMoveBomb : Ability
{
    [Header("-- Targeting")]
    public bool m_lockToCardinalDirsForPlace = true;
    public bool m_lockToCardinalDirsForMove = true;
    public int m_placeRange = 4;
    public int m_moveRange = 4;
    public bool m_moveIsFreeAction = true;
    [Header("-- Enemy direct hit")]
    public bool m_explodeThisTurnOnDirectHit;
    public bool m_explodeImmediatelyOnMove;
    [Header("-- Bomb explosion")]
    public int m_bombDuration;
    public int m_damageAmount;
    public float m_explosionLaserRange;
    public float m_explosionLaserWidth;
    public int m_cooldownAfterExplode = 2;
    [Header("-- Anims")]
    public ActorModelData.ActionAnimationType m_moveBombAnimIndex = ActorModelData.ActionAnimationType.Ability2;
    [Header("-- Sequences")]
    public GameObject m_castSequencePrefab;
    public GameObject m_persistentBombSequencePrefab;
    public GameObject m_explodeBombSequencePrefab;
    public GameObject m_moveBombSequencePrefab;

    private Gryd_SyncComponent m_syncComp;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Place/Move Bomb";
        }

        m_syncComp = GetComponent<Gryd_SyncComponent>();
        SetupTargeter();
    }

    private void SetupTargeter()
    {
        Targeter = new AbilityUtil_Targeter_GrydBomb(this, m_moveRange);
    }

    public override bool CustomTargetValidation(
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
        if (HasPlacedBomb())
        {
            return true;
        }

        GridPos casterGridPos = caster.GetGridPos();
        if (m_lockToCardinalDirsForPlace && !CardinallyAligned(casterGridPos, target.GridPos))
        {
            return false;
        }

        BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
        return targetSquare != null
               && targetSquare.IsValidForGameplay()
               && Mathf.Abs(casterGridPos.x - target.GridPos.x) <= m_placeRange
               && Mathf.Abs(casterGridPos.y - target.GridPos.y) <= m_placeRange
               && base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
    }

    protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
    {
        List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
        AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
        return numbers;
    }

    public override bool IsFreeAction()
    {
        return HasPlacedBomb()
            ? m_moveIsFreeAction
            : base.IsFreeAction();
    }

    public override ActorModelData.ActionAnimationType GetActionAnimType()
    {
        return HasPlacedBomb()
            ? m_moveBombAnimIndex
            : base.GetActionAnimType();
    }

    private bool CardinallyAligned(GridPos start, GridPos end)
    {
        return !start.CoordsEqual(end) && (start.x == end.x || start.y == end.y);
    }

    private GridPos GetPushEndPos(Vector3 targetPos, out bool hitActor)
    {
        GridPos placedBomb = GetPlacedBomb();
        BoardSquare placedBombSquare = Board.Get().GetSquare(placedBomb);
        Vector3 placedBombPos = placedBombSquare.ToVector3();
        Vector3 dir = targetPos - placedBombPos;
        if (m_lockToCardinalDirsForMove)
        {
            dir = VectorUtils.HorizontalAngleToClosestCardinalDirection(
                Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(dir)));
        }

        hitActor = false;
        Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(
            ActorData,
            placedBombPos,
            placedBombPos + dir * m_moveRange * Board.Get().squareSize,
            out bool collision,
            out _);
        BoardSquare hitSquare = null;
        if (collision)
        {
            hitSquare = Board.Get().GetSquareFromVec3(abilityLineEndpoint);
        }

        GridPos result = placedBomb;
        if (dir.x > 0.1f)
        {
            for (int i = placedBomb.x + 1; i <= placedBomb.x + m_moveRange; i++)
            {
                BoardSquare square = Board.Get().GetSquareFromIndex(i, placedBomb.y);
                if (square == null
                    || !square.IsValidForGameplay()
                    || !placedBombSquare.GetLOS(i, placedBomb.y)
                    || hitSquare != null && hitSquare.x < i)
                {
                    break;
                }

                result = square.GetGridPos();
                if (square.OccupantActor != null && square.OccupantActor.GetTeam() != ActorData.GetTeam())
                {
                    hitActor = true;
                    break;
                }
            }
        }
        else if (dir.x < -0.1f)
        {
            for (int i = placedBomb.x - 1; i >= placedBomb.x - m_moveRange; i--)
            {
                BoardSquare square = Board.Get().GetSquareFromIndex(i, placedBomb.y);
                if (square == null
                    || !square.IsValidForGameplay()
                    || !placedBombSquare.GetLOS(i, placedBomb.y)
                    || hitSquare != null && hitSquare.x > i)
                {
                    break;
                }

                result = square.GetGridPos();
                if (square.OccupantActor != null && square.OccupantActor.GetTeam() != ActorData.GetTeam())
                {
                    hitActor = true;
                    break;
                }
            }
        }
        else if (dir.z > 0.1f)
        {
            for (int i = placedBomb.y + 1; i <= placedBomb.y + m_moveRange; i++)
            {
                BoardSquare square = Board.Get().GetSquareFromIndex(placedBomb.x, i);
                if (square == null
                    || !square.IsValidForGameplay()
                    || !placedBombSquare.GetLOS(placedBomb.x, i)
                    || hitSquare != null && hitSquare.y < i)
                {
                    break;
                }

                result = square.GetGridPos();
                if (square.OccupantActor != null && square.OccupantActor.GetTeam() != ActorData.GetTeam())
                {
                    hitActor = true;
                    break;
                }
            }
        }
        else if (dir.z < -0.1f)
        {
            for (int i = placedBomb.y - 1; i >= placedBomb.y - m_moveRange; i--)
            {
                BoardSquare square = Board.Get().GetSquareFromIndex(placedBomb.x, i);
                if (square == null
                    || !square.IsValidForGameplay()
                    || !placedBombSquare.GetLOS(placedBomb.x, i)
                    || hitSquare != null && hitSquare.y > i)
                {
                    break;
                }

                result = square.GetGridPos();
                if (square.OccupantActor != null && square.OccupantActor.GetTeam() != ActorData.GetTeam())
                {
                    hitActor = true;
                    break;
                }
            }
        }

        return result;
    }

    public bool HasPlacedBomb()
    {
        return GetPlacedBomb().x > 0 && GetPlacedBomb().y > 0;
    }

    public GridPos GetPlacedBomb()
    {
        return m_syncComp != null
            ? m_syncComp.m_bombLocation
            : GridPos.s_invalid;
    }
}