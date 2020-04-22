using UnityEngine;

public class AbilityPreview : MonoBehaviour
{
	public GameObject m_comingSoonPanel;

	public GameObject m_videoPanel;

	public void Play(string movieAssetName)
	{
		Stop();
		if (!m_comingSoonPanel || !m_videoPanel)
		{
			return;
		}
		m_comingSoonPanel.SetActive(false);
		m_videoPanel.SetActive(true);
		PlayRawImageMovieTexture component = m_videoPanel.GetComponent<PlayRawImageMovieTexture>();
		if (!component)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			component.Play(movieAssetName, true);
			return;
		}
	}

	public void Stop()
	{
		if (!m_comingSoonPanel)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if ((bool)m_videoPanel)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					m_videoPanel.SetActive(false);
					m_comingSoonPanel.SetActive(true);
					return;
				}
			}
			return;
		}
	}
}
