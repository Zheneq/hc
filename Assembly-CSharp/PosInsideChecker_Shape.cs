using System.Collections.Generic;
using UnityEngine;

public class PosInsideChecker_Shape : IPosInsideChecker
{
	private AbilityAreaShape m_shape;

	private Vector3 m_freePos;

	private BoardSquare m_centerSquare;

	public PosInsideChecker_Shape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		m_shape = shape;
		m_freePos = freePos;
		m_centerSquare = centerSquare;
	}

	public bool IsPositionInside(Vector3 testPos)
	{
		return AreaEffectUtils.IsPosInShape(testPos, m_shape, m_freePos, m_centerSquare);
	}

	public bool AddTestPosForBarrier(List<Vector3> testPoints, Barrier barrier)
	{
		return false;
	}
}
