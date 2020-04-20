﻿using System;
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

	public AbilityUtil_Targeter_ReverseStretchCone(Ability ability, float minLengthInSquares, float maxLengthInSquares, float minAngleDegrees, float maxAngleDegrees, AreaEffectUtils.StretchConeStyle stretchStyle, float coneBackwardOffsetInSquares, bool penetrateLoS) : base(ability)
	{
		this.m_minLengthSquares = minLengthInSquares;
		this.m_maxLengthSquares = maxLengthInSquares;
		this.m_minAngleDegrees = minAngleDegrees;
		this.m_maxAngleDegrees = maxAngleDegrees;
		this.m_stretchStyle = stretchStyle;
		this.m_coneBackwardOffsetInSquares = coneBackwardOffsetInSquares;
		this.m_penetrateLoS = penetrateLoS;
		this.m_indicatorHandler = new OperationOnSquare_TurnOnHiddenSquareIndicator(this);
		this.m_coneChecker = new SquareInsideChecker_Cone();
		this.m_squarePosCheckerList.Add(this.m_coneChecker);
		this.m_shouldShowActorRadius = GameWideData.Get().UseActorRadiusForCone();
	}

	public override void UpdateTargeting(AbilityTarget currentTarget, ActorData targetingActor)
	{
		base.ClearActorsInRange();
		Vector3 vector = targetingActor.GetCurrentBoardSquare().GetWorldPositionForLoS();
		Vector3 freePos = currentTarget.FreePos;
		Vector3 vector2 = vector - freePos;
		vector2.y = 0f;
		vector2.Normalize();
		float num = VectorUtils.HorizontalAngle_Deg(vector2);
		float num2;
		float coneWidthDegrees;
		AreaEffectUtils.GatherStretchConeDimensions(freePos, vector, this.m_minLengthSquares, this.m_maxLengthSquares, this.m_minAngleDegrees, this.m_maxAngleDegrees, this.m_stretchStyle, out num2, out coneWidthDegrees, this.m_discreteWidthAngleChange, this.m_numDiscreteWidthChanges, this.m_interpMinDistOverride, this.m_interpRangeOverride);
		vector -= num2 * Board.Get().squareSize * vector2;
		this.CreateConeCursorHighlights(vector, vector2, num2, coneWidthDegrees);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(vector, num, coneWidthDegrees, num2 - this.m_coneBackwardOffsetInSquares, this.m_coneBackwardOffsetInSquares, true, targetingActor, TargeterUtils.GetRelevantTeams(targetingActor, this.m_includeAllies, this.m_includeEnemies), null, false, default(Vector3));
		TargeterUtils.SortActorsByDistanceToPos(ref actorsInCone, targetingActor.GetTravelBoardSquareWorldPositionForLos() - vector2);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
		if (this.m_includeCaster)
		{
			if (!actorsInCone.Contains(targetingActor))
			{
				actorsInCone.Add(targetingActor);
			}
		}
		float num3 = GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize * (GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize);
		Vector3 a = vector - vector2 * this.m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		for (int i = 0; i < actorsInCone.Count; i++)
		{
			ActorData actorData = actorsInCone[i];
			if (this.ShouldAddActor(actorData, targetingActor))
			{
				base.AddActorInRange(actorData, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Primary, false);
				if ((a - actorData.GetTravelBoardSquareWorldPositionForLos()).sqrMagnitude <= num3)
				{
					base.AddActorInRange(actorData, targetingActor.GetTravelBoardSquareWorldPosition(), targetingActor, AbilityTooltipSubject.Far, true);
				}
			}
		}
		this.DrawInvalidSquareIndicators(targetingActor, vector, num, num2, coneWidthDegrees);
	}

	public void CreateConeCursorHighlights(Vector3 coneStartPos, Vector3 centerAimDir, float coneLengthSquares, float coneWidthDegrees)
	{
		float d = this.m_coneBackwardOffsetInSquares * Board.Get().squareSize;
		float y = 0.1f - BoardSquare.s_LoSHeightOffset;
		Vector3 position = coneStartPos + new Vector3(0f, y, 0f) - centerAimDir * d;
		if (this.m_highlights != null)
		{
			if (this.m_highlights.Count >= 1)
			{
				goto IL_A4;
			}
		}
		this.m_highlights = new List<GameObject>();
		GameObject item = HighlightUtils.Get().CreateDynamicConeMesh(coneLengthSquares, coneWidthDegrees, this.ForceHideSides, null);
		this.m_highlights.Add(item);
		IL_A4:
		HighlightUtils.Get().AdjustDynamicConeMesh(this.m_highlights[0], coneLengthSquares, coneWidthDegrees);
		this.m_highlights[0].transform.rotation = Quaternion.LookRotation(centerAimDir);
		this.m_highlights[0].transform.position = position;
	}

	private void DrawInvalidSquareIndicators(ActorData targetingActor, Vector3 coneStartPos, float forwardDir_degrees, float coneLengthSquares, float coneWidthDegrees)
	{
		if (targetingActor == GameFlowData.Get().activeOwnedActorData)
		{
			this.m_coneChecker.UpdateConeProperties(coneStartPos, coneWidthDegrees, coneLengthSquares - this.m_coneBackwardOffsetInSquares, this.m_coneBackwardOffsetInSquares, forwardDir_degrees, targetingActor);
			this.m_coneChecker.SetLosPosOverride(true, targetingActor.GetTravelBoardSquareWorldPositionForLos(), false);
			base.ResetSquareIndicatorIndexToUse();
			AreaEffectUtils.OperateOnSquaresInCone(this.m_indicatorHandler, coneStartPos, forwardDir_degrees, coneWidthDegrees, coneLengthSquares - this.m_coneBackwardOffsetInSquares, this.m_coneBackwardOffsetInSquares, targetingActor, this.m_penetrateLoS, this.m_squarePosCheckerList);
			base.HideUnusedSquareIndicators();
		}
	}

	private bool ShouldAddActor(ActorData actor, ActorData caster)
	{
		bool result = false;
		if (actor == caster)
		{
			result = this.m_includeCaster;
		}
		else
		{
			if (actor.GetTeam() == caster.GetTeam())
			{
				if (this.m_includeAllies)
				{
					result = true;
					goto IL_7F;
				}
			}
			if (actor.GetTeam() != caster.GetTeam())
			{
				if (this.m_includeEnemies)
				{
					result = true;
				}
			}
		}
		IL_7F:
		if (!this.m_penetrateLoS)
		{
			BoardSquare currentBoardSquare = actor.GetCurrentBoardSquare();
			if (!caster.GetCurrentBoardSquare().symbol_0013(currentBoardSquare.x, currentBoardSquare.y))
			{
				result = false;
			}
		}
		return result;
	}
}
