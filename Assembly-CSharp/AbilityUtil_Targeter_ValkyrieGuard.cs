using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ValkyrieGuard : AbilityUtil_Targeter_Barrier
{
	public bool m_addCasterToActorsInRange;

	public float m_coverAngleLineLength = 1.5f;

	public bool m_useCone;

	private float m_coneWidthAngle;

	private float m_coneRadiusInSquares;

	private bool m_coneIgnoreLos;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	public AbilityUtil_Targeter_ValkyrieGuard(Ability ability, float width, bool snapToBorder = false, bool allowAimAtDiagonals = false, bool hideIfMovingFast = true)
		: base(ability, width, snapToBorder, allowAimAtDiagonals, hideIfMovingFast)
	{
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
	}

	public void SetConeParams(bool useCone, float coneWidthAngle, float coneRadiusInSquares, bool ignoreLos)
	{
		m_useCone = useCone;
		m_coneWidthAngle = coneWidthAngle;
		m_coneRadiusInSquares = coneRadiusInSquares;
		m_coneIgnoreLos = ignoreLos;
	}

	public override void UpdateTargetingMultiTargets(AbilityTarget currentTarget, ActorData targetingActor, int currentTargetIndex, List<AbilityTarget> targets)
	{
		ClearActorsInRange();
		base.UpdateTargetingMultiTargets(currentTarget, targetingActor, currentTargetIndex, targets);
		int num;
		if (m_snapToBorder)
		{
			num = 2;
		}
		else
		{
			num = 1;
		}
		int num2 = num;
		int num3;
		if (m_useCone)
		{
			num3 = 1;
		}
		else
		{
			num3 = 0;
		}
		int num4 = num3;
		if (m_highlights.Count <= num2 + 1 + num4)
		{
			m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(m_coverAngleLineLength, false, true));
			m_highlights.Add(HighlightUtils.Get().CreateBoundaryLine(m_coverAngleLineLength, false, false));
			if (m_useCone)
			{
				m_highlights.Add(HighlightUtils.Get().CreateConeCursor(m_coneRadiusInSquares * Board.Get().squareSize, m_coneWidthAngle));
			}
		}
		Vector3 barrierCenterPos = m_barrierCenterPos;
		float num5 = 0.5f * GameplayData.Get().m_coverProtectionAngle;
		float num6 = VectorUtils.HorizontalAngle_Deg(m_barrierOutwardFacing);
		float d = 0.5f * m_width * Board.Get().squareSize;
		Vector3 normalized = Vector3.Cross(m_barrierOutwardFacing, Vector3.up).normalized;
		m_highlights[num2].transform.position = barrierCenterPos - normalized * d;
		m_highlights[num2].transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(num6 + num5));
		m_highlights[num2 + 1].transform.position = barrierCenterPos + normalized * d;
		m_highlights[num2 + 1].transform.rotation = Quaternion.LookRotation(VectorUtils.AngleDegreesToVector(num6 - num5));
		if (m_useCone)
		{
			m_highlights[num2].SetActive(false);
			m_highlights[num2 + 1].SetActive(false);
			Vector3 vector = targetingActor.GetTravelBoardSquareWorldPositionForLos();
			if (currentTargetIndex > 0)
			{
				BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(targets[0].GridPos);
				if (boardSquareSafe != null)
				{
					vector = boardSquareSafe.ToVector3();
				}
			}
			Vector3 vector2 = -1f * m_barrierOutwardFacing;
			List<ActorData> actors = AreaEffectUtils.GetActorsInCone(vector, VectorUtils.HorizontalAngle_Deg(vector2), m_coneWidthAngle, m_coneRadiusInSquares, 0f, m_coneIgnoreLos, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, m_affectsAllies, m_affectsEnemies), null);
			TargeterUtils.RemoveActorsInvisibleToActor(ref actors, targetingActor);
			actors.Remove(targetingActor);
			for (int i = 0; i < actors.Count; i++)
			{
				AddActorInRange(actors[i], vector, targetingActor);
			}
			if (m_affectsTargetingActor)
			{
				AddActorInRange(targetingActor, vector, targetingActor);
			}
			GameObject gameObject = m_highlights[num2 + 2];
			Vector3 position = vector;
			position.y = HighlightUtils.GetHighlightHeight();
			gameObject.transform.position = position;
			gameObject.transform.rotation = Quaternion.LookRotation(vector2);
			DrawInvalidSquareIndicators(currentTarget, targetingActor, vector, vector2);
		}
		if (!m_addCasterToActorsInRange)
		{
			return;
		}
		while (true)
		{
			AddActorInRange(targetingActor, targetingActor.GetTravelBoardSquareWorldPositionForLos(), targetingActor, AbilityTooltipSubject.Self);
			return;
		}
	}

	private void DrawInvalidSquareIndicators(AbilityTarget currentTarget, ActorData targetingActor, Vector3 coneStartPos, Vector3 forwardDirection)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			ResetSquareIndicatorIndexToUse();
			float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(forwardDirection);
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, coneStartPos, coneCenterAngleDegrees, m_coneWidthAngle, m_coneRadiusInSquares, 0f, targetingActor, m_coneIgnoreLos);
			HideUnusedSquareIndicators();
		}
	}
}
