using UnityEngine;

public class CoverRegion
{
	public Vector3 m_center;

	public float m_startAngle;

	public float m_endAngle;

	public CoverRegion(Vector3 center, float startAngle, float endAngle)
	{
		m_center = center;
		m_startAngle = startAngle;
		m_endAngle = endAngle;
	}

	public bool IsDirInCover(float angle_deg)
	{
		if (angle_deg > m_startAngle)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (angle_deg > m_endAngle)
			{
				angle_deg -= 360f;
				goto IL_0059;
			}
		}
		if (angle_deg < m_startAngle && angle_deg < m_endAngle)
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
			angle_deg += 360f;
		}
		goto IL_0059;
		IL_0059:
		if (m_startAngle <= angle_deg)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (angle_deg <= m_endAngle)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return false;
	}

	public bool IsInCoverFromPos(Vector3 pos)
	{
		Vector3 vec = pos - m_center;
		float angle_deg = VectorUtils.HorizontalAngle_Deg(vec);
		return IsDirInCover(angle_deg);
	}
}
