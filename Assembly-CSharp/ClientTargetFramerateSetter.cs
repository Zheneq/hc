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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientTargetFramerateSetter.Update()).MethodHandle;
			}
			if (AppState.IsInGame())
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (Application.targetFrameRate > HydrogenConfig.Get().TargetFrameRate)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Application.targetFrameRate = HydrogenConfig.Get().TargetFrameRate;
			}
		}
	}
}
