using System;
using System.Collections.Generic;
using UnityEngine;

public class SquareInsideChecker_BounceSegment : ISquareInsideChecker
{
	private float m_widthInSquares;

	private Vector3 m_startPos;

	private Vector3 m_endPos;

	private Vector3 m_collisionNormal;

	private ActorData m_caster;

	private List<Vector3> m_additionalLosSources;

	public SquareInsideChecker_BounceSegment(float widthInSquares)
	{
		this.m_widthInSquares = widthInSquares;
	}

	public void UpdateBoxProperties(Vector3 startPos, Vector3 endPos, Vector3 collisionNormal, ActorData caster)
	{
		this.m_startPos = startPos;
		this.m_endPos = endPos;
		this.m_collisionNormal = collisionNormal;
		this.m_caster = caster;
	}

	public unsafe bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		inLos = false;
		bool flag = AreaEffectUtils.IsSquareInBoxByActorRadius(square, this.m_startPos, this.m_endPos, this.m_widthInSquares);
		if (flag)
		{
			inLos = AreaEffectUtils.IsSquareInLosForBox(square, this.m_startPos, this.m_endPos, this.m_widthInSquares, false, this.m_caster, this.m_additionalLosSources);
			if (inLos)
			{
				inLos = VectorUtils.SquareOnSameSideAsBounceBend(square, this.m_startPos, this.m_collisionNormal);
			}
		}
		return flag;
	}
}
