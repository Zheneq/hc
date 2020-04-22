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
		m_widthAngleDeg = widthAngle;
		m_radiusInSquares = radiusInSquares;
		m_ignoreLos = ignoreLos;
		m_backwardOffsetInSquares = backwardOffsetInSquares;
	}

	public TargeterPart_Cone(ConeTargetingInfo coneInfo)
	{
		m_widthAngleDeg = coneInfo.m_widthAngleDeg;
		m_radiusInSquares = coneInfo.m_radiusInSquares;
		m_ignoreLos = coneInfo.m_penetrateLos;
		m_backwardOffsetInSquares = coneInfo.m_backwardsOffset;
	}

	public void UpdateDimensions(float widthAngle, float radiusInSquares)
	{
		m_widthAngleDeg = widthAngle;
		m_radiusInSquares = radiusInSquares;
	}

	public List<ActorData> GetHitActors(Vector3 coneStartPos, Vector3 aimDir, ActorData targetingActor, List<Team> teams)
	{
		float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDir);
		List<ActorData> actors = AreaEffectUtils.GetActorsInCone(coneStartPos, coneCenterAngleDegrees, m_widthAngleDeg, m_radiusInSquares, m_backwardOffsetInSquares, m_ignoreLos, targetingActor, teams, null);
		TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
		return actors;
	}

	public GameObject CreateHighlightObject(AbilityUtil_Targeter targeter)
	{
		return HighlightUtils.Get().CreateDynamicConeMesh(m_radiusInSquares, m_widthAngleDeg, false, targeter.GetTemplateSwapData());
	}

	public void AdjustHighlight(GameObject highlightObj, Vector3 coneStartPos, Vector3 aimDir)
	{
		if (!(highlightObj != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			aimDir.Normalize();
			float d = m_backwardOffsetInSquares * Board.Get().squareSize;
			Vector3 position = coneStartPos - aimDir * d;
			position.y = HighlightUtils.GetHighlightHeight();
			HighlightUtils.Get().AdjustDynamicConeMesh(highlightObj, m_radiusInSquares, m_widthAngleDeg);
			highlightObj.transform.rotation = Quaternion.LookRotation(aimDir);
			highlightObj.transform.position = position;
			return;
		}
	}

	public void ShowHiddenSquares(OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler, Vector3 coneStartPos, float forwardAngle, ActorData targetingActor, bool ignoreLos)
	{
		AreaEffectUtils.OperateOnSquaresInCone(indicatorHandler, coneStartPos, forwardAngle, m_widthAngleDeg, m_radiusInSquares, m_backwardOffsetInSquares, targetingActor, ignoreLos);
	}
}
