using System.Collections.Generic;
using UnityEngine;

public class SquareInsideChecker_Box : ISquareInsideChecker
{
	public float m_widthInSquares;

	private Vector3 m_startPos;

	private Vector3 m_endPos;

	private ActorData m_caster;

	public bool m_penetrateLos;

	public List<Vector3> m_additionalLosSources;

	public SquareInsideChecker_Box(float widthInSquares)
	{
		m_widthInSquares = widthInSquares;
	}

	public void UpdateBoxProperties(Vector3 startPos, Vector3 endPos, ActorData caster)
	{
		m_startPos = startPos;
		m_endPos = endPos;
		m_caster = caster;
	}

	public bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		inLos = false;
		bool flag = AreaEffectUtils.IsSquareInBoxByActorRadius(square, m_startPos, m_endPos, m_widthInSquares);
		if (flag)
		{
			inLos = AreaEffectUtils.IsSquareInLosForBox(square, m_startPos, m_endPos, m_widthInSquares, m_penetrateLos, m_caster, m_additionalLosSources);
		}
		return flag;
	}

	public Vector3 GetStartPos()
	{
		return m_startPos;
	}

	public Vector3 GetEndPos()
	{
		return m_endPos;
	}
}
