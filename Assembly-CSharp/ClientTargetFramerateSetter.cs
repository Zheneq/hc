using System;
using UnityEngine;

public class ClientTargetFramerateSetter : MonoBehaviour
{
	public int m_ingameMaxFramerate = 0x78;

	public int m_frontendMaxFramerate = 0x3C;

	public int m_backgroundInGameMaxFramerate = 0x3C;

	public int m_backgroundFrontendMaxFramerate = 0xA;

	private bool m_isGameInBackground;

	private void Start()
	{
		Application.targetFrameRate = this.m_frontendMaxFramerate;
	}

	private void OnApplicationFocus(bool focusStatus)
	{
		this.m_isGameInBackground = !focusStatus;
	}

	private void Update()
	{
		if (this.m_isGameInBackground)
		{
			if (AppState.IsInGame())
			{
				Application.targetFrameRate = this.m_backgroundInGameMaxFramerate;
			}
			else
			{
				Application.targetFrameRate = this.m_backgroundFrontendMaxFramerate;
			}
		}
		else if (AppState.IsInGame())
		{
			Application.targetFrameRate = this.m_ingameMaxFramerate;
		}
		else
		{
			Application.targetFrameRate = this.m_frontendMaxFramerate;
		}
		if (HydrogenConfig.Get().TargetFrameRate >= 0)
		{
			if (Application.targetFrameRate > HydrogenConfig.Get().TargetFrameRate)
			{
				Application.targetFrameRate = HydrogenConfig.Get().TargetFrameRate;
			}
		}
	}
}
