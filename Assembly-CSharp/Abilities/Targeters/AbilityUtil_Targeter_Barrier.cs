using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_Barrier : AbilityUtil_Targeter
{
    public float m_width;
    public bool m_snapToBorder;

    private bool m_allowAimAtDiagonals;
    private Vector3 m_lastFreePos = Vector3.zero;
    private bool m_hideIfMovingFast = true;

    protected Vector3 m_barrierOutwardFacing;
    protected Vector3 m_barrierCenterPos;
    protected Vector3 m_barrierDir;

    public AbilityUtil_Targeter_Barrier(
        Ability ability,
        float width,
        bool snapToBorder = false,
        bool allowAimAtDiagonals = false,
        bool hideIfMovingFast = true)
        : base(ability)
    {
        m_width = width;
        m_snapToBorder = snapToBorder;
        m_allowAimAtDiagonals = allowAimAtDiagonals;
        m_hideIfMovingFast = hideIfMovingFast;
        m_lastFreePos.x = 10000f;
        m_lastFreePos.z = 10000f;
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
        AbilityTarget firstTarget = currentTargetIndex > 0 ? targets[0] : currentTarget;
        m_barrierCenterPos = firstTarget.FreePos;
        Vector3 vector = m_barrierCenterPos - targetingActor.GetFreePos();
        bool hasCursorMoved = false;
        bool active = false;
        Vector3 vector2 = m_barrierCenterPos;
        if (m_snapToBorder)
        {
            if ((m_barrierCenterPos - m_lastFreePos).magnitude > 0.2f)
            {
                hasCursorMoved = true;
            }

            m_lastFreePos = m_barrierCenterPos;
            BoardSquare square = Board.Get().GetSquare(firstTarget.GridPos);
            if (square != null)
            {
                active = true;
                vector2 = square.ToVector3();
                Vector3 freePos = currentTargetIndex > 0 ? currentTarget.FreePos : firstTarget.FreePos;
                vector = VectorUtils.GetDirectionAndOffsetToClosestSide(
                    square,
                    freePos,
                    m_allowAimAtDiagonals,
                    out Vector3 offset);
                m_barrierCenterPos = vector2 + offset;
            }
        }

        vector.y = 0f;
        vector.Normalize();
        m_barrierDir = vector;
        m_barrierOutwardFacing = -vector;
        if (Highlight == null)
        {
            Highlight = Object.Instantiate(
                HighlightUtils.Get().m_rectangleCursorPrefab.GetComponent<UIRectangleCursor>().m_endWidthLine);
            Highlight.transform.localScale = new Vector3(m_width * Board.Get().squareSize, 1f, 1f);
        }

        Highlight.transform.position = m_barrierCenterPos + new Vector3(0f, 0.1f, 0f);
        Highlight.transform.rotation = Quaternion.LookRotation(m_barrierOutwardFacing);
        Highlight.SetActive(!m_snapToBorder || !m_hideIfMovingFast || !hasCursorMoved);
        if (m_snapToBorder)
        {
            if (m_highlights.Count < 2)
            {
                m_highlights.Add(
                    HighlightUtils.Get().CreateShapeCursor(
                        AbilityAreaShape.SingleSquare,
                        targetingActor == GameFlowData.Get().activeOwnedActorData));
            }

            m_highlights[1].transform.position = vector2;
            m_highlights[1].SetActive(active);
        }
    }
}