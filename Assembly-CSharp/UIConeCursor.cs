using System.Text;
using UnityEngine;

public class UIConeCursor : MonoBehaviour
{
	public GameObject m_innerArc;

	public GameObject m_outerArc;

	public float m_arcDegrees;

	public float m_heightOffset;

	private float m_worldRadius;

	public void OnRadiusChanged(float newWorldRadius)
	{
		m_worldRadius = newWorldRadius;
		if (m_worldRadius <= 0f)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					Log.Error(new StringBuilder().Append("ConeCursor with invalid radius (").Append(m_worldRadius).Append(").  Disabling...").ToString());
					UIManager.SetGameObjectActive(base.gameObject, false);
					return;
				}
			}
		}
		if (m_outerArc != null)
		{
			m_outerArc.transform.localScale = new Vector3(m_worldRadius, 1f, m_worldRadius);
		}
		UIManager.SetGameObjectActive(base.gameObject, true);
	}

	private void Start()
	{
		if (m_innerArc != null)
		{
			m_innerArc.transform.localPosition = new Vector3(0f, m_heightOffset, 0f);
		}
		if (m_outerArc != null)
		{
			m_outerArc.transform.localPosition = new Vector3(0f, m_heightOffset, 0f);
		}
	}
}
