using System.Collections.Generic;
using UnityEngine;

public class TargeterPart_Laser
{
	public float m_widthInSquares = 1f;

	public float m_lengthInSquares = 1f;

	public bool m_ignoreLos;

	public int m_maxTargets = -1;

	public bool m_lengthIgnoreWorldGeo;

	public bool m_ignoreStartOffset;

	public TargeterPart_Laser(float width, float lengthInSquares, bool ignoreLos, int maxTargets)
	{
		m_widthInSquares = width;
		m_lengthInSquares = lengthInSquares;
		m_ignoreLos = ignoreLos;
		m_maxTargets = maxTargets;
	}

	public TargeterPart_Laser(LaserTargetingInfo laserInfo)
	{
		m_widthInSquares = laserInfo.width;
		m_lengthInSquares = laserInfo.range;
		m_ignoreLos = laserInfo.penetrateLos;
		m_maxTargets = laserInfo.maxTargets;
	}

	public void UpdateDimensions(float width, float lengthInSquares)
	{
		m_widthInSquares = width;
		m_lengthInSquares = lengthInSquares;
	}

	public List<ActorData> GetHitActors(Vector3 startPos, Vector3 aimDir, ActorData targetingActor, List<Team> teams, out Vector3 endPos)
	{
		return AreaEffectUtils.GetActorsInLaser(startPos, aimDir, m_lengthInSquares, m_widthInSquares, targetingActor, teams, m_ignoreLos, m_maxTargets, m_lengthIgnoreWorldGeo, false, out endPos, null, null, m_ignoreStartOffset);
	}

	public GameObject CreateHighlightObject(AbilityUtil_Targeter targeter)
	{
		return HighlightUtils.Get().CreateRectangularCursor(1f, 1f, targeter.GetTemplateSwapData());
	}

	public void AdjustHighlight(GameObject highlightObj, Vector3 startPos, Vector3 endPos, bool applyStartOffset = true)
	{
		if (!(highlightObj != null))
		{
			return;
		}
		while (true)
		{
			if (applyStartOffset)
			{
				float laserInitialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
				startPos = VectorUtils.GetAdjustedStartPosWithOffset(startPos, endPos, laserInitialOffsetInSquares);
			}
			Vector3 forward = endPos - startPos;
			forward.y = 0f;
			float magnitude = forward.magnitude;
			float widthInWorld = m_widthInSquares * Board.Get().squareSize;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, highlightObj);
			startPos.y = HighlightUtils.GetHighlightHeight();
			highlightObj.transform.position = startPos;
			highlightObj.transform.rotation = Quaternion.LookRotation(forward);
			return;
		}
	}

	public void ShowHiddenSquares(OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler, Vector3 startPos, Vector3 endPos, ActorData targetingActor, bool ignoreLos)
	{
		AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(indicatorHandler, startPos, endPos, m_widthInSquares, targetingActor, ignoreLos, null, null, !m_ignoreStartOffset);
	}
}
