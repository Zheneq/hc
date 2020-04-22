using System.Collections.Generic;
using UnityEngine;

public class PosInsideChecker_Box : IPosInsideChecker
{
	private Vector3 m_startPos;

	private Vector3 m_endPos;

	private float m_radiusInSquares;

	public PosInsideChecker_Box(Vector3 startPos, Vector3 endPos, float radiusInSquares)
	{
		m_startPos = startPos;
		m_endPos = endPos;
		m_radiusInSquares = radiusInSquares;
	}

	public bool IsPositionInside(Vector3 testPos)
	{
		return AreaEffectUtils.PointInBox(testPos, m_startPos, m_endPos, Board.Get().squareSize * m_radiusInSquares);
	}

	public bool AddTestPosForBarrier(List<Vector3> testPoints, Barrier barrier)
	{
		return false;
	}
}
