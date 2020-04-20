using System;
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
		this.m_widthInSquares = widthInSquares;
	}

	public void UpdateBoxProperties(Vector3 startPos, Vector3 endPos, ActorData caster)
	{
		this.m_startPos = startPos;
		this.m_endPos = endPos;
		this.m_caster = caster;
	}

	public unsafe bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		inLos = false;
		bool flag = AreaEffectUtils.IsSquareInBoxByActorRadius(square, this.m_startPos, this.m_endPos, this.m_widthInSquares);
		if (flag)
		{
			inLos = AreaEffectUtils.IsSquareInLosForBox(square, this.m_startPos, this.m_endPos, this.m_widthInSquares, this.m_penetrateLos, this.m_caster, this.m_additionalLosSources);
		}
		return flag;
	}

	public Vector3 GetStartPos()
	{
		return this.m_startPos;
	}

	public Vector3 GetEndPos()
	{
		return this.m_endPos;
	}
}
