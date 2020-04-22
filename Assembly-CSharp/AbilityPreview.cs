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
			if ((bool)m_videoPanel)
			{
				while (true)
				{
					m_videoPanel.SetActive(false);
					m_comingSoonPanel.SetActive(true);
					return;
				}
			}
			return;
		}
	}
}
