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
}