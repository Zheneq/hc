using System;
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
		this.m_coneStart = coneStart;
		this.m_radiusInSquares = radiusInSquares;
		this.m_coneCenterAngle = coneCenterAngle;
		this.m_coneWidthAngle = coneWidthAngle;
	}

	public bool IsPositionInside(Vector3 testPos)
	{
		return AreaEffectUtils.IsPosInCone(testPos, this.m_coneStart, this.m_radiusInSquares * Board.\u000E().squareSize, this.m_coneCenterAngle, this.m_coneWidthAngle);
	}

	public bool AddTestPosForBarrier(List<Vector3> testPoints, Barrier barrier)
	{
		bool result = false;
		if (this.m_coneWidthAngle < 180f)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PosInsideChecker_Cone.AddTestPosForBarrier(List<Vector3>, Barrier)).MethodHandle;
			}
			if (testPoints != null)
			{
				for (;;)
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
					for (;;)
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
					float num = 0.49f * this.m_coneWidthAngle;
					float d = this.m_radiusInSquares * Board.\u000E().squareSize;
					for (int i = -1; i <= 1; i++)
					{
						Vector3 vector = VectorUtils.AngleDegreesToVector(this.m_coneCenterAngle + (float)i * num);
						if (barrier.CrossingBarrier(this.m_coneStart, this.m_coneStart + vector * d))
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							bool flag;
							Vector3 lineLineIntersection = VectorUtils.GetLineLineIntersection(this.m_coneStart, vector, endPos, endPos2 - endPos, out flag);
							if (flag)
							{
								lineLineIntersection.y = endPos.y;
								testPoints.Add(lineLineIntersection);
								result = true;
							}
						}
					}
					for (;;)
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
