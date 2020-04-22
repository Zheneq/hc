using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private float m_horizontalIntensity = 0.3f;

	private float m_verticalIntensity = 0.1f;

	private float m_ratioRange = 0.25f;

	private float m_duration = 0.75f;

	private float m_elapsedTime;

	private float m_dampeningPower = 1f;

	private void Start()
	{
	}

	[ContextMenu("Execute")]
	public void Play()
	{
		m_elapsedTime = 0f;
	}

	public void Play(float horizonatlIntensity, float verticalIntensity, float duration)
	{
		m_horizontalIntensity = horizonatlIntensity;
		m_verticalIntensity = verticalIntensity;
		m_duration = duration;
		m_elapsedTime = 0f;
	}

	private void LateUpdate()
	{
		if (!(m_elapsedTime < m_duration))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_elapsedTime += Time.deltaTime;
			if (!CameraManager.Get().AllowCameraShake())
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				float f = Mathf.Min(1f, m_elapsedTime / m_duration);
				float d = 1f - Mathf.Abs(Mathf.Pow(f, m_dampeningPower));
				Vector3 a = base.transform.rotation * Vector3.left;
				a *= Random.Range(m_horizontalIntensity * (1f - m_ratioRange), m_horizontalIntensity);
				if (Random.value > 0.5f)
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
					a *= -1f;
				}
				Vector3 b = base.transform.rotation * Vector3.up;
				b *= Random.Range(m_verticalIntensity * (1f - m_ratioRange), m_verticalIntensity);
				if (Random.value > 0.5f)
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
					b *= -1f;
				}
				base.transform.position = base.transform.position + (a + b) * d;
				return;
			}
		}
	}
}
