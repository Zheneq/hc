using System.Collections.Generic;
using UnityEngine;

public class PosInsideChecker_Cone : IPosInsideChecker
{
	private Vector3 m_coneStart;

	private float m_radiusInSquares;

	private float m_coneCenterAngle;

	private float m_coneWidthAngle;

	public PosInsideChecker_Cone(Vector3 coneStart, float radiusInSquares, float coneCenterAngle, float coneWidthAngle)
	{
		m_coneStart = coneStart;
		m_radiusInSquares = radiusInSquares;
		m_coneCenterAngle = coneCenterAngle;
		m_coneWidthAngle = coneWidthAngle;
	}

	public bool IsPositionInside(Vector3 testPos)
	{
		return AreaEffectUtils.IsPosInCone(testPos, m_coneStart, m_radiusInSquares * Board.Get().squareSize, m_coneCenterAngle, m_coneWidthAngle);
	}

	public bool AddTestPosForBarrier(List<Vector3> testPoints, Barrier barrier)
	{
		bool result = false;
		if (m_coneWidthAngle < 180f)
		{
			while (true)
			{
				switch (3)
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
			if (testPoints != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (barrier != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 endPos = barrier.GetEndPos1();
					Vector3 endPos2 = barrier.GetEndPos2();
					float num = 0.49f * m_coneWidthAngle;
					float d = m_radiusInSquares * Board.Get().squareSize;
					for (int i = -1; i <= 1; i++)
					{
						Vector3 vector = VectorUtils.AngleDegreesToVector(m_coneCenterAngle + (float)i * num);
						if (barrier.CrossingBarrier(m_coneStart, m_coneStart + vector * d))
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
							bool intersecting;
							Vector3 lineLineIntersection = VectorUtils.GetLineLineIntersection(m_coneStart, vector, endPos, endPos2 - endPos, out intersecting);
							if (intersecting)
							{
								lineLineIntersection.y = endPos.y;
								testPoints.Add(lineLineIntersection);
								result = true;
							}
						}
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
		}
		return result;
	}
}
