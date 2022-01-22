using System.Collections.Generic;
using UnityEngine;

public class AbilityUtil_Targeter_ReverseStretchCone : AbilityUtil_Targeter
{
	public float m_minLengthSquares;

	public float m_maxLengthSquares;

	public float m_minAngleDegrees;

	public float m_maxAngleDegrees;

	public float m_coneBackwardOffsetInSquares;

	public bool m_penetrateLoS;

	public bool m_includeEnemies = true;

	public bool m_includeAllies;

	public bool m_includeCaster;

	public AreaEffectUtils.StretchConeStyle m_stretchStyle;

	public float m_interpMinDistOverride = -1f;

	public float m_interpRangeOverride = -1f;

	public bool m_discreteWidthAngleChange;

	public int m_numDiscreteWidthChanges;

	private OperationOnSquare_TurnOnHiddenSquareIndicator m_indicatorHandler;

	private SquareInsideChecker_Cone m_coneChecker;

	private List<ISquareInsideChecker> m_squarePosCheckerList = new List<ISquareInsideChecker>();

	public bool ForceHideSides;

	public AbilityUtil_Targeter_ReverseStretchCone(Ability ability, float minLengthInSquares, float maxLengthInSquares, float minAngleDegrees, float maxAngleDegrees, AreaEffectUtils.StretchConeStyle stretchStyle, float coneBackwardOffsetInSquares, bool penetrateLoS)
		: base(ability)
	{
		m_minLengthSquares = minLengthInSquares;
		m_maxLengthSquares = maxLengthInSquares;
		m_minAngleDegrees = minAngleDegrees;
		m_maxAngleDegrees = maxAngleDegrees;
		m_stretchStyle = stretchStyle;
		m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		m_penetrateLoS = penetrateLoS;
		m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		m_coneChecker = new SquareInsideChecker_Cone();
		m_squarePosCheckerList.Add(m_coneChecker);
		m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		ClearActorsInRange();
		Vector3 worldPositionForLoS = targetingActor.GetCurrentBoardSquare().GetOccupantLoSPos();
		Vector3 freePos = currentTarget.FreePos;
		Vector3 vector = worldPositionForLoS - freePos;
		vector.y = 0f;
		vector.Normalize();
		float num = VectorUtils.HorizontalAngle_Deg(vector);
		AreaEffectUtils.GatherStretchConeDimensions(freePos, worldPositionForLoS, m_minLengthSquares, m_maxLengthSquares, m_minAngleDegrees, m_maxAngleDegrees, m_stretchStyle, out float lengthInSquares, out float angleInDegrees, m_discreteWidthAngleChange, m_numDiscreteWidthChanges, m_interpMinDistOverride, m_interpRangeOverride);
		worldPositionForLoS -= lengthInSquares * Board.Get().squareSize * vector;
		CreateConeCursorHighlights(worldPositionForLoS, vector, lengthInSquares, angleInDegrees);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(worldPositionForLoS, num, angleInDegrees, lengthInSquares - m_coneBackwardOffsetInSquares, m_coneBackwardOffsetInSquares, true, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, m_includeAllies, m_includeEnemies), null);
		TargeterUtils.SortActorsByDistanceToPos(ref actors, targetingActor.GetLoSCheckPos() - vector);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		if (m_includeCaster)
		{
			if (!actors.Contains(targetingActor))
			{
				actors.Add(targetingActor);
			}
		}
		float num2 = GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize * (GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize);
		Vector3 a = worldPositionForLoS - vector * m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		for (int i = 0; i < actors.Count; i++)
		{
			ActorData actorData = actors[i];
			if (!ShouldAddActor(actorData, targetingActor))
			{
				continue;
			}
			AddActorInRange(actorData, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor);
			if ((a - actorData.GetLoSCheckPos()).sqrMagnitude <= num2)
			{
				AddActorInRange(actorData, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Far, true);
			}
		}
		DrawInvalidSquareIndicators(targetingActor, worldPositionForLoS, num, lengthInSquares, angleInDegrees);
	}

	public void CreateConeCursorHighlights(Vector3 coneStartPos, Vector3 centerAimDir, float coneLengthSquares, float coneWidthDegrees)
	{
		float d = m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = coneStartPos + new Vector3(0f, y, 0f) - centerAimDir * d;
		if (m_highlights != null)
		{
			if (m_highlights.Count >= 1)
			{
				goto IL_00a4;
			}
		}
		m_highlights = new List<GameObject>();
		GameObject item = HighlightUtils.Get().CreateDynamicConeMesh(coneLengthSquares, coneWidthDegrees, ForceHideSides);
		m_highlights.Add(item);
		goto IL_00a4;
		IL_00a4:
		HighlightUtils.Get().AdjustDynamicConeMesh(m_highlights[0], coneLengthSquares, coneWidthDegrees);
		m_highlights[0].transform.rotation = Quaternion.LookRotation(centerAimDir);
		m_highlights[0].transform.position = position;
	}

	private void DrawInvalidSquareIndicators(ActorData targetingActor, Vector3 coneStartPos, float forwardDir_degrees, float coneLengthSquares, float coneWidthDegrees)
	{
		if (!(targetingActor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			m_coneChecker.UpdateConeProperties(coneStartPos, coneWidthDegrees, coneLengthSquares - m_coneBackwardOffsetInSquares, m_coneBackwardOffsetInSquares, forwardDir_degrees, targetingActor);
			m_coneChecker.SetLosPosOverride(true, targetingActor.GetLoSCheckPos(), false);
			ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInCone(m_indicatorHandler, coneStartPos, forwardDir_degrees, coneWidthDegrees, coneLengthSquares - m_coneBackwardOffsetInSquares, m_coneBackwardOffsetInSquares, targetingActor, m_penetrateLoS, m_squarePosCheckerList);
			HideUnusedSquareIndicators();
			return;
		}
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = m_includeCaster;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
			{
				if (m_includeAllies)
				{
					result = true;
					goto IL_007f;
				}
			}
			if (actor.GetTeam() != caster.GetTeam())
			{
				if (m_includeEnemies)
				{
					result = true;
				}
			}
		}
		goto IL_007f;
		IL_007f:
		if (!m_penetrateLoS)
		{
			BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
			if (!caster.GetCurrentBoardSquare().GetLOS(currentBoardSquare.x, currentBoardSquare.y))
			{
				result = false;
			}
		}
		return result;
	}
}
