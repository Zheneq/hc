using System;
using UnityEngine;

public class UIWorldPing : MonoBehaviour
{
	private bool m_initialized;

	private float m_startTime;

	private const float TimeForPingToLast = 5f;

	private void Init()
	{
		if (this.m_initialized)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIWorldPing.Init()).MethodHandle;
			}
			return;
		}
		this.m_startTime = Time.time;
		this.m_initialized = true;
	}

	private void Awake()
	{
		this.Init();
	}

	private void Update()
	{
		this.Init();
		if (Time.time - this.m_startTime > 5f)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIWorldPing.Update()).MethodHandle;
			}
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemovePing(this);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
