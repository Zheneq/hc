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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (AppState.IsInGame())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
			switch (1)
			{
			case 0:
				continue;
			}
			if (Application.targetFrameRate > HydrogenConfig.Get().TargetFrameRate)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					Application.targetFrameRate = HydrogenConfig.Get().TargetFrameRate;
					return;
				}
			}
			return;
		}
	}
}
