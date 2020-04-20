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
			if (videoPlayer.MovieState == PlayRawImageMovieTexture.MovieStates.Done)
			{
				UILandingPageFullScreenMenus.Get().SetVideoContainerVisible(false);
			}
		}
	}

	public PlayRawImageMovieTexture GetVideoPlayer()
	{
		if (this.m_videoPlacement)
		{
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
