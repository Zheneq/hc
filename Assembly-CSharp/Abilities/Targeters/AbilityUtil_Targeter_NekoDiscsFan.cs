using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_NekoDiscsFan : AbilityUtil_Targeter_ThiefFanLaser
{
	private float m_aoeRadiusAtEnd;
	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;
	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();
	private bool m_showEndSquareHighlights = true;

	public AbilityUtil_Targeter_NekoDiscsFan(
		Ability ability,
		float minAngle,
		float maxAngle,
		float angleInterpMinDistance,
		float angleInterpMaxDistance,
		float rangeInSquares,
		float widthInSquares,
		float aoeRadiusAtEnd,
		int maxTargets,
		int count,
		bool penetrateLos,
		float interpStep,
		float startAngleOffset)
		: base(
			ability,
			minAngle,
			maxAngle,
			angleInterpMinDistance,
			angleInterpMaxDistance,
			rangeInSquares,
			widthInSquares,
			maxTargets,
			count,
			penetrateLos,
			false,
			false,
			false,
			false,
			0,
			interpStep,
			startAngleOffset)
	{
		m_aoeRadiusAtEnd = aoeRadiusAtEnd;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < m_count; i++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Box(widthInSquares));
		}
		for (int j = 0; j < m_count; j++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
		}
	}

	public void SetShowEndSquareHighlight(bool show)
	{
		m_showEndSquareHighlights = show;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights != null && m_highlights.Count >= 3 * m_count + 1)
		{
			m_highlights[3 * m_count].SetActive(false);
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetLoSCheckPos();
		List<BoardSquare> discSquaresFromEndPositions = NekoFanOfDiscs.GetDiscSquaresFromEndPositions(
			m_laserEndPoints, travelBoardSquareWorldPositionForLos);
		for (int i = 0; i < m_count; i++)
		{
			if (m_highlights.Count <= m_count + 2 * i)
			{
				m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(
					AbilityAreaShape.SingleSquare, 
					targetingActor == GameFlowData.Get().activeOwnedActorData));
				m_highlights.Add(HighlightUtils.Get().CreateAoECursor(
					m_aoeRadiusAtEnd * Board.SquareSizeStatic,
					targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			GameObject highlightA = m_highlights[2 * i + m_count];
			GameObject highlightB = m_highlights[2 * i + 1 + m_count];
			Vector3 centerPos = new Vector3(m_laserEndPoints[i].x, HighlightUtils.GetHighlightHeight(), m_laserEndPoints[i].z);
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(
				travelBoardSquareWorldPositionForLos,
				m_laserEndPoints[i]);
			List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(
				centerPos,
				m_aoeRadiusAtEnd,
				false,
				targetingActor,
				targetingActor.GetEnemyTeam(),
				null,
				true,
				coneLosCheckPos);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			AddActorsInRange(actors, travelBoardSquareWorldPositionForLos, targetingActor);
			int inAoeKey = ContextKeys.s_InAoe.GetKey();
			foreach (ActorData target in actors)
			{
				ActorHitContext actorHitContext = m_actorContextVars[target];
				actorHitContext.m_hitOrigin = travelBoardSquareWorldPositionForLos;
				actorHitContext.m_contextVars.SetValue(inAoeKey, 1);
			}
			Vector3 baselineHeight = discSquaresFromEndPositions[i].GetPosAtBaselineHeight();
			baselineHeight.y = HighlightUtils.GetHighlightHeight();
			highlightA.transform.position = baselineHeight;
			highlightB.transform.position = centerPos;
			if (highlightA.activeSelf != m_showEndSquareHighlights)
			{
				highlightA.SetActive(m_showEndSquareHighlights);
			}
			
		}
		if (m_interpStep > 0f)
		{
			if (m_highlights.Count < 3 * m_count + 1)
			{
				float z = (m_interpStep + m_interpMinDistanceInSquares) * Board.SquareSizeStatic;
				float lengthInSquares = 1.2f;
				GameObject highlightLineObj =
					HighlightUtils.Get().CreateDynamicLineSegmentMesh(lengthInSquares, 0.5f, false, Color.cyan);
				highlightLineObj.transform.localPosition = new Vector3(-0.5f * Board.Get().squareSize * lengthInSquares, 0f, z);
				highlightLineObj.transform.localRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
				GameObject highlightLineTransform = new GameObject
				{
					transform =
					{
						localPosition = Vector3.zero,
						localRotation = Quaternion.identity
					}
				};
				highlightLineObj.transform.parent = highlightLineTransform.transform;
				m_highlights.Add(highlightLineTransform);
			}
			GameObject highlight = m_highlights[m_highlights.Count - 1];
			Vector3 position = travelBoardSquareWorldPositionForLos;
			position.y = HighlightUtils.GetHighlightHeight();
			highlight.transform.position = position;
			highlight.transform.rotation = Quaternion.LookRotation(currentTarget.AimDirection);
		}
	}

	protected override void UpdateLaserEndPointsForHiddenSquares(Vector3 startPos, Vector3 endPos, int index, ActorData targetingActor)
	{
		for (int i = 0; i < m_count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[index] as SquareInsideChecker_Box;
			SquareInsideChecker_Cone squareInsideChecker_Cone = m_squarePosCheckerList[index + m_count] as SquareInsideChecker_Cone;
			squareInsideChecker_Box.UpdateBoxProperties(startPos, endPos, targetingActor);
			squareInsideChecker_Cone.UpdateConeProperties(endPos, 360f, m_aoeRadiusAtEnd, 0f, 0f, targetingActor);
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(startPos, endPos);
			squareInsideChecker_Cone.SetLosPosOverride(true, coneLosCheckPos, true);
		}
	}

	protected override void HandleShowHiddenSquares(ActorData targetingActor)
	{
		for (int i = 0; i < m_count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[i] as SquareInsideChecker_Box;
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(
				m_indicatorHandler,
				squareInsideChecker_Box.GetStartPos(),
				squareInsideChecker_Box.GetEndPos(),
				m_widthInSquares,
				targetingActor,
				m_penetrateLos,
				null,
				m_squarePosCheckerList);
			AreaEffectUtils.OperateOnSquaresInCone(
				m_indicatorHandler,
				squareInsideChecker_Box.GetEndPos(),
				0f,
				360f,
				m_aoeRadiusAtEnd,
				0f,
				targetingActor,
				m_penetrateLos,
				m_squarePosCheckerList);
		}
	}
}
