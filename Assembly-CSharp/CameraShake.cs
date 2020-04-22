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
			m_elapsedTime += Time.deltaTime;
			if (!CameraManager.Get().AllowCameraShake())
			{
				return;
			}
			while (true)
			{
				float f = Mathf.Min(1f, m_elapsedTime / m_duration);
				float d = 1f - Mathf.Abs(Mathf.Pow(f, m_dampeningPower));
				Vector3 a = base.transform.rotation * Vector3.left;
				a *= Random.Range(m_horizontalIntensity * (1f - m_ratioRange), m_horizontalIntensity);
				if (Random.value > 0.5f)
				{
					a *= -1f;
				}
				Vector3 b = base.transform.rotation * Vector3.up;
				b *= Random.Range(m_verticalIntensity * (1f - m_ratioRange), m_verticalIntensity);
				if (Random.value > 0.5f)
				{
					b *= -1f;
				}
				base.transform.position = base.transform.position + (a + b) * d;
				return;
			}
		}
	}
}
