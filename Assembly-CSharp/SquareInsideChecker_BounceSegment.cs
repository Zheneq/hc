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
		m_widthInSquares = widthInSquares;
	}

	public void UpdateBoxProperties(Vector3 startPos, Vector3 endPos, Vector3 collisionNormal, ActorData caster)
	{
		m_startPos = startPos;
		m_endPos = endPos;
		m_collisionNormal = collisionNormal;
		m_caster = caster;
	}

	public bool IsSquareInside(BoardSquare square, out bool inLos)
	{
		inLos = false;
		bool flag = AreaEffectUtils.IsSquareInBoxByActorRadius(square, m_startPos, m_endPos, m_widthInSquares);
		if (flag)
		{
			inLos = AreaEffectUtils.IsSquareInLosForBox(square, m_startPos, m_endPos, m_widthInSquares, false, m_caster, m_additionalLosSources);
			if (inLos)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				inLos = VectorUtils.SquareOnSameSideAsBounceBend(square, m_startPos, m_collisionNormal);
			}
		}
		return flag;
	}
}
