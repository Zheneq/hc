using System;
using System.Collections.Generic;
using UnityEngine;

public class PosInsideChecker_Shape : IPosInsideChecker
{
	private AbilityAreaShape m_shape;

	private Vector3 m_freePos;

	private BoardSquare m_centerSquare;

	public PosInsideChecker_Shape(AbilityAreaShape shape, Vector3 freePos, BoardSquare centerSquare)
	{
		this.m_shape = shape;
		this.m_freePos = freePos;
		this.m_centerSquare = centerSquare;
	}

	public bool IsPositionInside(Vector3 testPos)
	{
		return AreaEffectUtils.IsPosInShape(testPos, this.m_shape, this.m_freePos, this.m_centerSquare);
	}

	public bool AddTestPosForBarrier(List<Vector3> testPoints, Barrier barrier)
	{
		return false;
	}
}
