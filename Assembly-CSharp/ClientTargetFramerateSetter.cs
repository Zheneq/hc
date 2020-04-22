using UnityEngine;

public class ClientTargetFramerateSetter : MonoBehaviour
{
	public int m_ingameMaxFramerate = 120;

	public int m_frontendMaxFramerate = 60;

	public int m_backgroundInGameMaxFramerate = 60;

	public int m_backgroundFrontendMaxFramerate = 10;

	private bool m_isGameInBackground;

	private void Start()
	{
		Application.targetFrameRate = m_frontendMaxFramerate;
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
