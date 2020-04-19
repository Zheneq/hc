using System;
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
		this.m_oscillatingScale = 0.8f + Mathf.Clamp(num * 0.2f, -0.2f, 0.2f);
		base.transform.localScale = new Vector3(this.m_oscillatingScale, base.transform.localScale.y, this.m_oscillatingScale);
	}
}
