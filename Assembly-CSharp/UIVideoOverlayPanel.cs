using System;
using UnityEngine;

public class UIVideoOverlayPanel : MonoBehaviour
{
	public GameObject m_videoPlacement;

	private void Start()
	{
	}

	private void Update()
	{
		PlayRawImageMovieTexture videoPlayer = this.GetVideoPlayer();
		if (videoPlayer)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIVideoOverlayPanel.Update()).MethodHandle;
			}
			if (videoPlayer.MovieState == PlayRawImageMovieTexture.MovieStates.Done)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				UILandingPageFullScreenMenus.Get().SetVideoContainerVisible(false);
			}
		}
	}

	public PlayRawImageMovieTexture GetVideoPlayer()
	{
		if (this.m_videoPlacement)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIVideoOverlayPanel.GetVideoPlayer()).MethodHandle;
			}
			return this.m_videoPlacement.GetComponentInChildren<PlayRawImageMovieTexture>();
		}
		return null;
	}

	public void PlayVideo(string movieAssetName)
	{
		PlayRawImageMovieTexture videoPlayer = this.GetVideoPlayer();
		if (videoPlayer)
		{
			bool loop = false;
			videoPlayer.Play(movieAssetName, loop, false, true);
		}
	}
}
