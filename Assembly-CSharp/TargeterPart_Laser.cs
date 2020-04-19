using System;
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
		this.m_widthInSquares = width;
		this.m_lengthInSquares = lengthInSquares;
		this.m_ignoreLos = ignoreLos;
		this.m_maxTargets = maxTargets;
	}

	public TargeterPart_Laser(LaserTargetingInfo laserInfo)
	{
		this.m_widthInSquares = laserInfo.width;
		this.m_lengthInSquares = laserInfo.range;
		this.m_ignoreLos = laserInfo.penetrateLos;
		this.m_maxTargets = laserInfo.maxTargets;
	}

	public void UpdateDimensions(float width, float lengthInSquares)
	{
		this.m_widthInSquares = width;
		this.m_lengthInSquares = lengthInSquares;
	}

	public List<ActorData> GetHitActors(Vector3 startPos, Vector3 aimDir, ActorData targetingActor, List<Team> teams, out Vector3 endPos)
	{
		return AreaEffectUtils.GetActorsInLaser(startPos, aimDir, this.m_lengthInSquares, this.m_widthInSquares, targetingActor, teams, this.m_ignoreLos, this.m_maxTargets, this.m_lengthIgnoreWorldGeo, false, out endPos, null, null, this.m_ignoreStartOffset, true);
	}

	public GameObject CreateHighlightObject(AbilityUtil_Targeter targeter)
	{
		return HighlightUtils.Get().CreateRectangularCursor(1f, 1f, targeter.GetTemplateSwapData());
	}

	public void AdjustHighlight(GameObject highlightObj, Vector3 startPos, Vector3 endPos, bool applyStartOffset = true)
	{
		if (highlightObj != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TargeterPart_Laser.AdjustHighlight(GameObject, Vector3, Vector3, bool)).MethodHandle;
			}
			if (applyStartOffset)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				float laserInitialOffsetInSquares = GameWideData.Get().m_laserInitialOffsetInSquares;
				startPos = VectorUtils.GetAdjustedStartPosWithOffset(startPos, endPos, laserInitialOffsetInSquares);
			}
			Vector3 forward = endPos - startPos;
			forward.y = 0f;
			float magnitude = forward.magnitude;
			float widthInWorld = this.m_widthInSquares * Board.\u000E().squareSize;
			HighlightUtils.Get().ResizeRectangularCursor(widthInWorld, magnitude, highlightObj);
			startPos.y = HighlightUtils.GetHighlightHeight();
			highlightObj.transform.position = startPos;
			highlightObj.transform.rotation = Quaternion.LookRotation(forward);
		}
	}

	public void ShowHiddenSquares(OperationOnSquare_TurnOnHiddenSquareIndicator indicatorHandler, Vector3 startPos, Vector3 endPos, ActorData targetingActor, bool ignoreLos)
	{
		AreaEffectUtils.OperateOnSquaresInBoxByActorRadius(indicatorHandler, startPos, endPos, this.m_widthInSquares, targetingActor, ignoreLos, null, null, !this.m_ignoreStartOffset);
	}
}
