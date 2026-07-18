// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class GrydCycleDash : Ability
{
    [Header("-- Targeting")]
    public bool m_lockToCardinalDirs = true;
    public float m_totalRange = 10f;
    public float m_legRange = 5f;
    public int m_numLegs = 2;
    [Header("-- Ground Trail")]
    public StandardGroundEffectInfo m_groundTrail;
    [Header("-- Sequences")]
    public GameObject m_castSequencePrefab;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Cycle Dash";
        }

        if (m_targetData == null || m_targetData.Length < GetExpectedNumberOfTargeters())
        {
            Debug.LogError(
                "GrydCycleDash has wrong number of Target Data entries - to match Num Legs it should be "
                + GetExpectedNumberOfTargeters());
        }

        SetupTargeter();
    }

    private void SetupTargeter()
    {
        Targeters.Clear();
        for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
        {
            AbilityUtil_Targeter_BombingRun targeter = new AbilityUtil_Targeter_BombingRun(
                this,
                AbilityAreaShape.SingleSquare,
                Mathf.RoundToInt(m_totalRange));
            targeter.SetShowArcToShape(false);
            targeter.SetUseMultiTargetUpdate(true);
            Targeters.Add(targeter);
        }
    }

    public override int GetExpectedNumberOfTargeters()
    {
        return m_numLegs;
    }

    protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
    {
        List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
        m_groundTrail.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
        return numbers;
    }

    protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
    {
        m_groundTrail.m_groundEffectData.AddTooltipTokens(tokens, "GroundTrail");
    }

    public override bool CustomTargetValidation(
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
        GridPos gridPos = targetIndex != 0 ? currentTargets[targetIndex - 1].GridPos : caster.GetGridPos();
        if (m_lockToCardinalDirs && !CardinallyAligned(gridPos, target.GridPos))
        {
            return false;
        }

        BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
        if (targetSquare == null || !targetSquare.IsValidForGameplay())
        {
            return false;
        }

        BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
            caster,
            targetSquare,
            Board.Get().GetSquare(gridPos),
            false);
        if (boardSquarePathInfo == null)
        {
            return false;
        }

        boardSquarePathInfo.CalcAndSetMoveCostToEnd();
        float moveCost = boardSquarePathInfo.FindMoveCostToEnd();
        return moveCost <= m_legRange * Board.Get().squareSize;
    }

    internal override ActorData.MovementType GetMovementType()
    {
        return ActorData.MovementType.Charge;
    }

    private bool CardinallyAligned(GridPos start, GridPos end)
    {
        return !start.CoordsEqual(end) && (start.x == end.x || start.y == end.y);
    }

#if SERVER
    // added in rogues
    public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
    {
        return chargeSegments[chargeSegments.Length - 1].m_pos;
    }

    // added in rogues
    public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
    {
        Vector3 result = chargeSegments[chargeSegments.Length - 2].m_pos.ToVector3()
                         - chargeSegments[chargeSegments.Length - 1].m_pos.ToVector3();
        result.y = 0f;
        result.Normalize();
        return result;
    }

    // added in rogues
    public override bool GetChargeThroughInvalidSquares()
    {
        return false;
    }

    // added in rogues
    public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        FindCrossedSquares(targets, caster, null, out int num, out GridPos pos);
        ServerEvadeUtils.ChargeSegment[] result = new ServerEvadeUtils.ChargeSegment[num + 1];
        result[0] = new ServerEvadeUtils.ChargeSegment
        {
            m_pos = caster.GetSquareAtPhaseStart(),
            m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
            m_end = BoardSquarePathInfo.ChargeEndType.Pivot
        };
        for (int i = 1; i <= num; i++)
        {
            result[i] = new ServerEvadeUtils.ChargeSegment();
            if (i == num && !pos.CoordsEqual(GridPos.s_invalid))
            {
                result[i].m_pos = Board.Get().GetSquare(pos);
            }
            else
            {
                result[i].m_pos = Board.Get().GetSquare(targets[i - 1].GridPos);
            }

            result[i].m_end = BoardSquarePathInfo.ChargeEndType.Pivot;
        }

        result[num].m_end = BoardSquarePathInfo.ChargeEndType.Miss;
        float segmentMovementSpeed = CalcMovementSpeed(GetEvadeDistance(result));
        for (int j = 0; j <= num; j++)
        {
            result[j].m_segmentMovementSpeed = segmentMovementSpeed;
        }

        return result;
    }

    // added in rogues
    public override BoardSquare GetIdealDestination(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        return base.GetIdealDestination(targets, caster, additionalData);
    }

    // added in rogues
    public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_castSequencePrefab,
                caster.GetCurrentBoardSquare(),
                additionalData.m_abilityResults.HitActorsArray(),
                caster,
                additionalData.m_sequenceSource)
        };
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
        List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
        List<BoardSquare> list = FindCrossedSquares(targets, caster, nonActorTargetInfo, out _, out _);
        List<StandardMultiAreaGroundEffect.GroundAreaInfo> effectAreas =
            new List<StandardMultiAreaGroundEffect.GroundAreaInfo>();
        foreach (BoardSquare boardSquare in list)
        {
            if (m_groundTrail.m_applyGroundEffect)
            {
                effectAreas.Add(
                    new StandardMultiAreaGroundEffect.GroundAreaInfo(
                        boardSquare,
                        boardSquare.ToVector3(),
                        AbilityAreaShape.SingleSquare));
            }
        }

        if (effectAreas.Count > 0)
        {
            ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
            actorHitResults.AddEffect(
                new StandardMultiAreaGroundEffect(
                    AsEffectSource(),
                    effectAreas,
                    caster,
                    m_groundTrail.m_groundEffectData));
            abilityResults.StoreActorHit(actorHitResults);
        }

        abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
    }

    // added in rogues
    private List<BoardSquare> FindCrossedSquares(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo,
        out int actualNumLegs,
        out GridPos blockedAt)
    {
        actualNumLegs = 1;
        blockedAt = GridPos.s_invalid;
        BoardSquare squareAtPhaseStart = caster.GetSquareAtPhaseStart();
        List<BoardSquare> squaresBetween = GetSquaresBetween(
            caster,
            squareAtPhaseStart.GetGridPos(),
            targets[0].GridPos,
            nonActorTargetInfo,
            out bool stopDash,
            out blockedAt);

        for (int i = 1; i < m_numLegs; i++)
        {
            if (stopDash)
            {
                break;
            }

            squaresBetween.AddRange(
                GetSquaresBetween(
                    caster,
                    targets[i - 1].GridPos,
                    targets[i].GridPos,
                    nonActorTargetInfo,
                    out stopDash,
                    out blockedAt));
            actualNumLegs++;
        }

        return squaresBetween;
    }

    // added in rogues
    private List<BoardSquare> GetSquaresBetween(
        ActorData caster,
        GridPos start,
        GridPos end,
        List<NonActorTargetInfo> nonActorTargetInfo,
        out bool stopDash,
        out GridPos blockedAt)
    {
        stopDash = false;
        blockedAt = GridPos.s_invalid;
        if (!CardinallyAligned(start, end))
        {
            blockedAt = start;
            stopDash = true;
            return new List<BoardSquare>();
        }

        Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(
            caster,
            Board.Get().GetSquare(start).ToVector3(),
            Board.Get().GetSquare(end).ToVector3(),
            out bool collision,
            out _,
            nonActorTargetInfo);
        if (collision)
        {
            GridPos gridPos = Board.Get().GetSquareFromVec3(abilityLineEndpoint).GetGridPos();
            if (!end.CoordsEqual(gridPos))
            {
                stopDash = true;
                end = gridPos;
                blockedAt = end;
            }
        }

        List<BoardSquare> result = new List<BoardSquare>();
        if (start.x == end.x)
        {
            if (start.y < end.y)
            {
                for (int i = start.y; i < end.y; i++)
                {
                    result.Add(Board.Get().GetSquareFromIndex(start.x, i));
                }
            }
            else
            {
                for (int i = start.y; i > end.y; i--)
                {
                    result.Add(Board.Get().GetSquareFromIndex(start.x, i));
                }
            }
        }
        else if (start.x < end.x)
        {
            for (int i = start.x; i < end.x; i++)
            {
                result.Add(Board.Get().GetSquareFromIndex(i, start.y));
            }
        }
        else
        {
            for (int i = start.x; i > end.x; i--)
            {
                result.Add(Board.Get().GetSquareFromIndex(i, start.y));
            }
        }

        return result;
    }
#endif
}