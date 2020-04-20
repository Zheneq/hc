using System;
using System.Collections.Generic;
using UnityEngine;

public class TargeterPart_Cone
{
	public float m_widthAngleDeg = 90f;

	public float m_radiusInSquares = 1f;

	public bool m_ignoreLos;

	public float m_backwardOffsetInSquares;

	public TargeterPart_Cone(float widthAngle, float radiusInSquares, bool ignoreLos, float backwardOffsetInSquares)
	{
		this.m_widthAngleDeg = widthAngle;
		this.m_radiusInSquares = radiusInSquares;
		this.m_ignoreLos = ignoreLos;
		this.m_backwardOffsetInSquares = backwardOffsetInSquares;
	}

	public TargeterPart_Cone(ConeTargetingInfo coneInfo)
	{
		this.m_widthAngleDeg = coneInfo.m_widthAngleDeg;
		this.m_radiusInSquares = coneInfo.m_radiusInSquares;
		this.m_ignoreLos = coneInfo.m_penetrateLos;
		this.m_backwardOffsetInSquares = coneInfo.m_backwardsOffset;
	}

	public void UpdateDimensions(float widthAngle, float radiusInSquares)
	{
		this.m_widthAngleDeg = widthAngle;
		this.m_radiusInSquares = radiusInSquares;
	}

	public List<ActorData> GetHitActors(Vector3 coneStartPos, Vector3 aimDir, ActorData targetingActor, List<Team> teams)
	{
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDir);
		List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(coneStartPos, coneCenterAngleDegrees, this.m_widthAngleDeg, this.m_radiusInSquares, this.m_backwardOffsetInSquares, this.m_ignoreLos, targetingActor, teams, null, false, default(Vector3));
		TargeterUtils.RemoveActorsInvisibleToClient(ref actorsInCone);
		return actorsInCone;
	}

	public GameObject CreateHighlightObject(AbilityUtil_Targeter targeter)
	{
		return HighlightUtils.Get().CreateDynamicConeMesh(this.m_radiusInSquares, this.m_widthAngleDeg, false, targeter.GetTemplateSwapData());
	}

	public void AdjustHighlight(GameObject highlightObj, Vector3 coneStartPos, Vector3 aimDir)
	{
		if (highlightObj != null)
		{
			aimDir.Normalize();
			float d = this.m_backwardOffsetInSquares * Board.Get().squareSize;
			Vector3 position = coneStartPos - aimDir * d;
			position.y = HighlightUtils.GetHighlightHeight();
			HighlightUtils.Get().AdjustDynamicConeMesh(highlightObj, this.m_radiusInSquares, this.m_widthAngleDeg);
			highlightObj.transform.rotation = Quaternion.LookRotation(aimDir);
			highlightObj.transform.position = position;
		}
	}

	public void ShowHiddenSquares(OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler, Vector3 coneStartPos, float forwardAngle, ActorData targetingActor, bool ignoreLos)
	{
		AreaEffectUtils.OperateOnSquaresInCone(indicatorHandler, coneStartPos, forwardAngle, this.m_widthAngleDeg, this.m_radiusInSquares, this.m_backwardOffsetInSquares, targetingActor, ignoreLos, null);
	}
}
