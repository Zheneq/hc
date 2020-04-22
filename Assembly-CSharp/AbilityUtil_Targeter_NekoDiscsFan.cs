using AbilityContextNamespace;
using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_NekoDiscsFan : AbilityUtil_Targeter_ThiefFanLaser
{
	private float m_aoeRadiusAtEnd;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	private bool m_showEndSquareHighlights = true;

	public AbilityUtil_Targeter_NekoDiscsFan(Ability ability, float minAngle, float maxAngle, float angleInterpMinDistance, float angleInterpMaxDistance, float rangeInSquares, float widthInSquares, float aoeRadiusAtEnd, int maxTargets, int count, bool penetrateLos, float interpStep, float startAngleOffset)
		: base(ability, minAngle, maxAngle, angleInterpMinDistance, angleInterpMaxDistance, rangeInSquares, widthInSquares, maxTargets, count, penetrateLos, false, false, false, false, 0, interpStep, startAngleOffset)
	{
		m_aoeRadiusAtEnd = aoeRadiusAtEnd;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		for (int i = 0; i < m_count; i++)
		{
			m_squarePosCheckerList.Add(new SquareInsideChecker_Box(widthInSquares));
		}
		while (true)
		{
			for (int j = 0; j < m_count; j++)
			{
				m_squarePosCheckerList.Add(new SquareInsideChecker_Cone());
			}
			return;
		}
	}

	public void SetShowEndSquareHighlight(bool show)
	{
		m_showEndSquareHighlights = show;
	}

	public override void StartConfirmedTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.StartConfirmedTargeting(currentTarget, targetingActor);
		if (m_highlights == null)
		{
			return;
		}
		while (true)
		{
			if (m_highlights.Count >= 3 * m_count + 1)
			{
				while (true)
				{
					m_highlights[3 * m_count].SetActive(false);
					return;
				}
			}
			return;
		}
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.UpdateTargeting(currentTarget, targetingActor);
		Vector3 travelBoardSquareWorldPositionForLos = targetingActor.GetTravelBoardSquareWorldPositionForLos();
		List<BoardSquare> discSquaresFromEndPositions = NekoFanOfDiscs.GetDiscSquaresFromEndPositions(m_laserEndPoints, travelBoardSquareWorldPositionForLos);
		int num = 0;
		while (num < m_count)
		{
			if (m_highlights.Count <= m_count + 2 * num)
			{
				m_highlights.Add(HighlightUtils.Get().CreateShapeCursor(AbilityAreaShape.SingleSquare, targetingActor == GameFlowData.Get().activeOwnedActorData));
				m_highlights.Add(HighlightUtils.Get().CreateAoECursor(m_aoeRadiusAtEnd * Board.SquareSizeStatic, targetingActor == GameFlowData.Get().activeOwnedActorData));
			}
			GameObject gameObject = m_highlights[2 * num + m_count];
			GameObject gameObject2 = m_highlights[2 * num + 1 + m_count];
			Vector3 vector = m_laserEndPoints[num];
			float x = vector.x;
			float highlightHeight = HighlightUtils.GetHighlightHeight();
			Vector3 vector2 = m_laserEndPoints[num];
			Vector3 vector3 = new Vector3(x, highlightHeight, vector2.z);
			Vector3 coneLosCheckPos = AbilityCommon_LaserWithCone.GetConeLosCheckPos(travelBoardSquareWorldPositionForLos, m_laserEndPoints[num]);
			List<ActorData> actors = AreaEffectUtils.GetActorsInRadius(vector3, m_aoeRadiusAtEnd, false, targetingActor, targetingActor.GetOpposingTeam(), null, true, coneLosCheckPos);
			TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
			AddActorsInRange(actors, travelBoardSquareWorldPositionForLos, targetingActor);
			int hash = ContextKeys._001A.GetHash();
			for (int i = 0; i < actors.Count; i++)
			{
				ActorData key = actors[i];
				ActorHitContext actorHitContext = m_actorContextVars[key];
				actorHitContext._001D = travelBoardSquareWorldPositionForLos;
				actorHitContext._0015.SetInt(hash, 1);
			}
			while (true)
			{
				BoardSquare boardSquare = discSquaresFromEndPositions[num];
				Vector3 baselineHeight = boardSquare.GetBaselineHeight();
				baselineHeight.y = HighlightUtils.GetHighlightHeight();
				gameObject.transform.position = baselineHeight;
				gameObject2.transform.position = vector3;
				if (gameObject.activeSelf != m_showEndSquareHighlights)
				{
					gameObject.SetActive(m_showEndSquareHighlights);
				}
				num++;
				goto IL_023d;
			}
			IL_023d:;
		}
		while (true)
		{
			if (!(m_interpStep > 0f))
			{
				return;
			}
			while (true)
			{
				if (m_highlights.Count < 3 * m_count + 1)
				{
					float z = (m_interpStep + m_interpMinDistanceInSquares) * Board.SquareSizeStatic;
					float num2 = 1.2f;
					GameObject gameObject3 = HighlightUtils.Get().CreateDynamicLineSegmentMesh(num2, 0.5f, false, Color.cyan);
					gameObject3.transform.localPosition = new Vector3(-0.5f * Board.Get().squareSize * num2, 0f, z);
					gameObject3.transform.localRotation = Quaternion.LookRotation(new Vector3(1f, 0f, 0f));
					GameObject gameObject4 = new GameObject();
					gameObject4.transform.localPosition = Vector3.zero;
					gameObject4.transform.localRotation = Quaternion.identity;
					gameObject3.transform.parent = gameObject4.transform;
					m_highlights.Add(gameObject4);
				}
				GameObject gameObject5 = m_highlights[m_highlights.Count - 1];
				Vector3 position = travelBoardSquareWorldPositionForLos;
				position.y = HighlightUtils.GetHighlightHeight();
				gameObject5.transform.position = position;
				gameObject5.transform.rotation = Quaternion.LookRotation(currentTarget.AimDirection);
				return;
			}
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
		while (true)
		{
			return;
		}
	}

	protected override void HandleShowHiddenSquares(ActorData targetingActor)
	{
		for (int i = 0; i < m_count; i++)
		{
			SquareInsideChecker_Box squareInsideChecker_Box = m_squarePosCheckerList[i] as SquareInsideChecker_Box;
			AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(m_indicatorHandler, squareInsideChecker_Box.GetStartPos(), squareInsideChecker_Box.GetEndPos(), m_widthInSquares, targetingActor, m_penetrateLos, null, m_squarePosCheckerList);
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, squareInsideChecker_Box.GetEndPos(), 0f, 360f, m_aoeRadiusAtEnd, 0f, targetingActor, m_penetrateLos, m_squarePosCheckerList);
		}
	}
}
