using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_GrydBomb : AbilityUtil_Targeter_Shape
{
    public bool m_lockToCardinalDirs = true;
    public bool m_showArrowHighlight = true;
    public float m_bombMoveRange;
    public float m_heightOffset = 0.1f;

    private GrydPlaceOrMoveBomb m_bombAbility;

    public AbilityUtil_Targeter_GrydBomb(Ability ability, float moveRange)
        : base(ability, AbilityAreaShape.SingleSquare, false)
    {
        m_bombMoveRange = moveRange;
        m_bombAbility = ability as GrydPlaceOrMoveBomb;
    }

    public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
    {
        if (m_bombAbility == null || !m_bombAbility.HasPlacedBomb())
        {
            SetShowArcToShape(true);
            base.UpdateTargeting(currentTarget, targetingActor);
            return;
        }

        ClearActorsInRange();
        SetShowArcToShape(false);
        Vector3 bombPos = Board.Get().GetSquare(m_bombAbility.GetPlacedBomb()).GetOccupantLoSPos();
        Vector3 vector = currentTarget.FreePos - bombPos;
        if (m_lockToCardinalDirs)
        {
            vector = VectorUtils.HorizontalAngleToClosestCardinalDirection(
                Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector)));
        }

        if (m_highlights != null && m_highlights.Count < 1)
        {
            m_highlights.Add(
                HighlightUtils.Get().CreateRectangularCursor(
                    Board.Get().squareSize * 0.75f,
                    m_bombMoveRange * Board.Get().squareSize));
        }

        Vector3 position = bombPos;
        position.y = HighlightUtils.GetHighlightHeight();
        m_highlights[0].transform.position = position;
        m_highlights[0].transform.rotation = Quaternion.LookRotation(vector);
        List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
            bombPos,
            vector,
            m_bombMoveRange,
            0.75f,
            targetingActor,
            targetingActor.GetEnemyTeamAsList(),
            false,
            1,
            false,
            false,
            out _,
            null);
        AddActorsInRange(actorsInLaser, bombPos, targetingActor);
    }
}