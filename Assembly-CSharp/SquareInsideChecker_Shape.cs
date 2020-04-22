using UnityEngine;

public class SquareInsideChecker_Shape : ISquareInsideChecker
{
	public AbilityAreaShape m_shape;

	public Vector3 m_freePos;

	public BoardSquare m_centerSquare;

	public ActorData m_caster;

	public SquareInsideChecker_Shape(AbilityAreaShape shape)
	{
		m_shape = shape;
	}

	public void UpdateShapeProperties(Vector3 freePos, BoardSquare centerSquare, ActorData caster)
	{
		m_freePos = freePos;
		m_centerSquare = centerSquare;
		m_caster = caster;
	}

	public bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		inLos = false;
		bool flag = AreaEffectUtils.IsSquareInShape(square, m_shape, m_freePos, m_centerSquare, true, m_caster);
		if (flag)
		{
			inLos = AreaEffectUtils.IsSquareInShape(square, m_shape, m_freePos, m_centerSquare, false, m_caster);
		}
		return flag;
	}
}
