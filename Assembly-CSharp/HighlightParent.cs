using System;
using UnityEngine;

public class HighlightParent : MonoBehaviour
{
	private float m_oscillatingAlpha;

	private bool m_pulsing = true;

	private void Start()
	{
	}

	private void Update()
	{
		if (this.m_pulsing)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(HighlightParent.Update()).MethodHandle;
			}
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float num = Mathf.Sin(realtimeSinceStartup * 3f);
			this.m_oscillatingAlpha = 0.5f + Mathf.Clamp(num * 0.2f, -0.2f, 0.2f);
			foreach (Renderer renderer in base.GetComponentsInChildren<Renderer>())
			{
				Color color = renderer.material.GetColor("_TintColor");
				Color value = new Color(color.r, color.g, color.b, this.m_oscillatingAlpha);
				renderer.material.SetColor("_TintColor", value);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}
}
