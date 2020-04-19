using System;
using UnityEngine;

public class CoverRegion
{
	public Vector3 m_center;

	public float m_startAngle;

	public float m_endAngle;

	public CoverRegion(Vector3 center, float startAngle, float endAngle)
	{
		this.m_center = center;
		this.m_startAngle = startAngle;
		this.m_endAngle = endAngle;
	}

	public bool IsDirInCover(float angle_deg)
	{
		if (angle_deg > this.m_startAngle)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(CoverRegion.IsDirInCover(float)).MethodHandle;
			}
			if (angle_deg > this.m_endAngle)
			{
				angle_deg -= 360f;
				goto IL_59;
			}
		}
		if (angle_deg < this.m_startAngle && angle_deg < this.m_endAngle)
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
			angle_deg += 360f;
		}
		IL_59:
		if (this.m_startAngle <= angle_deg)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (angle_deg <= this.m_endAngle)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				return true;
			}
		}
		return false;
	}

	public bool IsInCoverFromPos(Vector3 pos)
	{
		Vector3 vec = pos - this.m_center;
		float angle_deg = VectorUtils.HorizontalAngle_Deg(vec);
		return this.IsDirInCover(angle_deg);
	}
}
