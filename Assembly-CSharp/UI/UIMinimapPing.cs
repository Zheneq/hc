using UnityEngine;

public class UIMinimapPing : MonoBehaviour
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
				switch (1)
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
		if (Time.time - m_startTime > 5f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
