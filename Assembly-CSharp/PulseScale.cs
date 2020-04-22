using UnityEngine;

public class PulseScale : MonoBehaviour
{
	private float m_oscillatingScale = 0.5f;

	private void Start()
	{
	}

	private void Update()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		float num = Mathf.Sin(5f * realtimeSinceStartup);
		m_oscillatingScale = 0.8f + Mathf.Clamp(num * 0.2f, -0.2f, 0.2f);
		Transform transform = base.transform;
		float oscillatingScale = m_oscillatingScale;
		Vector3 localScale = base.transform.localScale;
		transform.localScale = new Vector3(oscillatingScale, localScale.y, m_oscillatingScale);
	}
}
