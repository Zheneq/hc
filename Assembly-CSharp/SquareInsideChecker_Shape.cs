using System;
using UnityEngine;

public class SquareInsideChecker_Shape : ISquareInsideChecker
{
	public AbilityAreaShape m_shape;

	public Vector3 m_freePos;

	public BoardSquare m_centerSquare;

	public ActorData m_caster;

	public SquareInsideChecker_Shape(AbilityAreaShape shape)
	{
		this.m_shape = shape;
	}

	public void UpdateShapeProperties(Vector3 freePos, BoardSquare centerSquare, ActorData caster)
	{
		this.m_freePos = freePos;
		this.m_centerSquare = centerSquare;
		this.m_caster = caster;
	}

	public bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		inLos = false;
		bool flag = AreaEffectUtils.IsSquareInShape(square, this.m_shape, this.m_freePos, this.m_centerSquare, true, this.m_caster);
		if (flag)
		{
			inLos = AreaEffectUtils.IsSquareInShape(square, this.m_shape, this.m_freePos, this.m_centerSquare, false, this.m_caster);
		}
		return flag;
	}
}
