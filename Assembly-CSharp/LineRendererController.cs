using UnityEngine;

[ExecuteInEditMode]
public class LineRendererController : MonoBehaviour
{
	private LineRenderer m_lineRenderer;

	private ParticleSystem m_parentParticleSystem;

	public Vector3[] m_positions;

	public float m_startWidth;

	public float m_endWidth;

	public Color m_startColor;

	public Color m_endColor;

	public FloatTimeEntry[] m_scaleEntries;

	public FloatTimeEntry[] m_alphaEntries;

	public float m_scale;

	public float GetTotalDuration()
	{
		float num = 0f;
		if (m_scaleEntries != null)
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
			if (m_scaleEntries.Length > 0)
			{
				num = m_scaleEntries[m_scaleEntries.Length - 1].m_time;
			}
		}
		if (m_alphaEntries != null)
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
			if (m_alphaEntries.Length > 0)
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
				num = Mathf.Max(num, m_alphaEntries[m_alphaEntries.Length - 1].m_time);
			}
		}
		return num;
	}

	private void Start()
	{
	}

	private void Update()
	{
		if ((bool)m_parentParticleSystem)
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
			if ((bool)m_lineRenderer)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
					{
						m_lineRenderer.positionCount = m_positions.Length;
						for (int i = 0; i < m_positions.Length; i++)
						{
							m_lineRenderer.SetPosition(i, m_positions[i]);
						}
						float num = GetValue(m_parentParticleSystem.time, m_scaleEntries) * m_scale;
						float value = GetValue(m_parentParticleSystem.time, m_alphaEntries);
						m_lineRenderer.startWidth = m_startWidth * num;
						m_lineRenderer.endWidth = m_endWidth * num;
						Color startColor = m_startColor;
						startColor.a *= value;
						Color endColor = m_endColor;
						endColor.a *= value;
						m_lineRenderer.startColor = startColor;
						m_lineRenderer.endColor = endColor;
						return;
					}
					}
				}
			}
		}
		m_lineRenderer = GetComponent<LineRenderer>();
		if (!base.transform.parent)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			m_parentParticleSystem = base.transform.parent.GetComponent<ParticleSystem>();
			return;
		}
	}

	private float GetValue(float curTime, FloatTimeEntry[] entries)
	{
		float result = 1f;
		if (entries != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (entries.Length > 0)
			{
				int num = entries.Length;
				int num2 = 0;
				while (true)
				{
					if (num2 < entries.Length)
					{
						if (entries[num2].m_time > curTime)
						{
							num = num2;
							break;
						}
						num2++;
						continue;
					}
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
				if (num == 0)
				{
					result = entries[num].m_value;
				}
				else if (num == entries.Length)
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
					result = entries[entries.Length - 1].m_value;
				}
				else
				{
					float time = entries[num].m_time;
					float time2 = entries[num - 1].m_time;
					float t = (curTime - time2) / (time - time2);
					result = Mathf.Lerp(entries[num - 1].m_value, entries[num].m_value, t);
				}
			}
		}
		return result;
	}
}
