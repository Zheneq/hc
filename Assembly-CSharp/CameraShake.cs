using System;
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
		this.m_elapsedTime = 0f;
	}

	public void Play(float horizonatlIntensity, float verticalIntensity, float duration)
	{
		this.m_horizontalIntensity = horizonatlIntensity;
		this.m_verticalIntensity = verticalIntensity;
		this.m_duration = duration;
		this.m_elapsedTime = 0f;
	}

	private void LateUpdate()
	{
		if (this.m_elapsedTime < this.m_duration)
		{
			this.m_elapsedTime += Time.deltaTime;
			if (CameraManager.Get().AllowCameraShake())
			{
				float f = Mathf.Min(1f, this.m_elapsedTime / this.m_duration);
				float d = 1f - Mathf.Abs(Mathf.Pow(f, this.m_dampeningPower));
				Vector3 a = base.transform.rotation * Vector3.left;
				a *= UnityEngine.Random.Range(this.m_horizontalIntensity * (1f - this.m_ratioRange), this.m_horizontalIntensity);
				if (UnityEngine.Random.value > 0.5f)
				{
					a *= -1f;
				}
				Vector3 vector = base.transform.rotation * Vector3.up;
				vector *= UnityEngine.Random.Range(this.m_verticalIntensity * (1f - this.m_ratioRange), this.m_verticalIntensity);
				if (UnityEngine.Random.value > 0.5f)
				{
					vector *= -1f;
				}
				base.transform.position = base.transform.position + (a + vector) * d;
			}
		}
	}
}
