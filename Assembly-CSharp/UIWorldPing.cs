using UnityEngine;

public class UIWorldPing : MonoBehaviour
{
	private bool m_initialized;

	private float m_startTime;

	private const float TimeForPingToLast = 5f;

	private void Init()
	{
		if (m_initialized)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_startTime = Time.time;
		m_initialized = true;
	}

	private void Awake()
	{
		Init();
	}

	private void Update()
	{
		Init();
		if (!(Time.time - m_startTime > 5f))
		{
			return;
		}
		while (true)
		{
			HUD_UI.Get().m_mainScreenPanel.m_offscreenIndicatorPanel.RemovePing(this);
			Object.Destroy(base.gameObject);
			return;
		}
	}
}
