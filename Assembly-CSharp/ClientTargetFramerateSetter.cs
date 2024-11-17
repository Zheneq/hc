using UnityEngine;

public class ClientTargetFramerateSetter : MonoBehaviour
{
#if EVOS
	public int m_ingameMaxFramerate = 240;
#else
	public int m_ingameMaxFramerate = 120;
#endif

	public int m_frontendMaxFramerate = 60;

	public int m_backgroundInGameMaxFramerate = 60;

	public int m_backgroundFrontendMaxFramerate = 10;

	private bool m_isGameInBackground;

	private void Start()
	{
		Application.targetFrameRate = m_frontendMaxFramerate;
#if EVOS
		QualitySettings.vSyncCount = HydrogenConfig.Get().vsync;
#endif
    }

	private void OnApplicationFocus(bool focusStatus)
	{
		m_isGameInBackground = !focusStatus;
	}

	private void Update()
	{
		if (m_isGameInBackground)
		{
			if (AppState.IsInGame())
			{
				Application.targetFrameRate = m_backgroundInGameMaxFramerate;
			}
			else
			{
				Application.targetFrameRate = m_backgroundFrontendMaxFramerate;
			}
		}
		else if (AppState.IsInGame())
		{
			Application.targetFrameRate = m_ingameMaxFramerate;
		}
		else
		{
			Application.targetFrameRate = m_frontendMaxFramerate;
		}
		if (HydrogenConfig.Get().TargetFrameRate < 0)
		{
			return;
		}
		while (true)
		{
			if (Application.targetFrameRate > HydrogenConfig.Get().TargetFrameRate)
			{
				while (true)
				{
					Application.targetFrameRate = HydrogenConfig.Get().TargetFrameRate;
					return;
				}
			}
			return;
		}
	}
}
