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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityPreview.Play(string)).MethodHandle;
				}
				component.Play(movieAssetName, true, false, true);
			}
		}
	}

	public void Stop()
	{
		if (this.m_comingSoonPanel)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityPreview.Stop()).MethodHandle;
			}
			if (this.m_videoPanel)
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
				this.m_videoPanel.SetActive(false);
				this.m_comingSoonPanel.SetActive(true);
			}
		}
	}
}
