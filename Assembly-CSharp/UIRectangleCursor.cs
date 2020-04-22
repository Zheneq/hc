using UnityEngine;

public class UIRectangleCursor : MonoBehaviour
{
	public GameObject m_start;

	public GameObject m_centerLine;

	public GameObject m_lengthLine1;

	public GameObject m_lengthLine2;

	public GameObject m_corner1;

	public GameObject m_corner2;

	public GameObject m_endWidthLine;

	public GameObject m_interior;

	public float m_distCasterToStart = 0.75f;

	public float m_distStartToCenterLine;

	public float m_distCasterToInterior = 0.75f;

	public float m_widthPerCorner = 0.1f;

	public float m_lengthPerCorner = 1.5f;

	public float m_heightOffset;

	private float m_worldWidth;

	private float m_worldLength;

	public void OnDimensionsChanged(float newWorldWidth, float newWorldLength)
	{
		m_worldWidth = newWorldWidth;
		m_worldLength = newWorldLength;
		if (!(m_worldWidth <= 0f))
		{
			while (true)
			{
				switch (7)
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
			if (!(m_worldLength <= 0f))
			{
				if (m_start != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					m_start.transform.localPosition = new Vector3(0f, m_heightOffset, m_distCasterToStart);
				}
				if (m_centerLine != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					float num = m_distCasterToStart + m_distStartToCenterLine;
					m_centerLine.transform.localPosition = new Vector3(0f, m_heightOffset, num);
					float z = m_worldLength - num;
					m_centerLine.transform.localScale = new Vector3(1f, 1f, z);
				}
				float num2 = m_worldWidth / 2f;
				if (m_lengthLine1 != null)
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
					if (m_lengthLine2 != null)
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
						float z2 = m_worldLength - m_lengthPerCorner - m_distCasterToStart;
						float z3 = m_worldLength - m_lengthPerCorner;
						m_lengthLine1.transform.localScale = new Vector3(1f, 1f, z2);
						m_lengthLine2.transform.localScale = new Vector3(1f, 1f, z2);
						m_lengthLine1.transform.localPosition = new Vector3(0f - num2, m_heightOffset, z3);
						m_lengthLine2.transform.localPosition = new Vector3(num2, m_heightOffset, z3);
					}
				}
				if (m_corner1 != null && m_corner2 != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					m_corner1.transform.localPosition = new Vector3(0f - num2, m_heightOffset, m_worldLength);
					m_corner2.transform.localPosition = new Vector3(num2, m_heightOffset, m_worldLength);
				}
				if (m_endWidthLine != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					float x = m_worldWidth - m_widthPerCorner * 2f;
					m_endWidthLine.transform.localScale = new Vector3(x, 1f, 1f);
					m_endWidthLine.transform.localPosition = new Vector3(0f, m_heightOffset, m_worldLength);
				}
				if (m_interior != null)
				{
					float worldWidth = m_worldWidth;
					float z4 = m_worldLength - m_lengthPerCorner - m_distCasterToInterior;
					m_interior.transform.localScale = new Vector3(worldWidth, 1f, z4);
					m_interior.transform.localPosition = new Vector3(0f, m_heightOffset, m_distCasterToInterior);
				}
				base.gameObject.SetActive(true);
				return;
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
		base.gameObject.SetActive(false);
	}

	public void SetRectangleEndVisible(bool visible)
	{
		m_corner1.SetActive(visible);
		m_corner2.SetActive(visible);
		m_endWidthLine.SetActive(visible);
	}

	public void SetRectangleStartVisible(bool visible)
	{
		m_start.SetActive(visible);
	}

	private void Start()
	{
	}
}
