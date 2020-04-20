using System;
using UnityEngine;

public class AbilityPreview : MonoBehaviour
{
	public GameObject m_comingSoonPanel;

	public GameObject m_videoPanel;

	public void Play(string movieAssetName)
	{
		this.Stop();
		if (this.m_comingSoonPanel && this.m_videoPanel)
		{
			this.m_comingSoonPanel.SetActive(false);
			this.m_videoPanel.SetActive(true);
			PlayRawImageMovieTexture component = this.m_videoPanel.GetComponent<PlayRawImageMovieTexture>();
			if (component)
			{
				component.Play(movieAssetName, true, false, true);
			}
		}
	}

	public void Stop()
	{
		if (this.m_comingSoonPanel)
		{
			if (this.m_videoPanel)
			{
				this.m_videoPanel.SetActive(false);
				this.m_comingSoonPanel.SetActive(true);
			}
		}
	}
}
