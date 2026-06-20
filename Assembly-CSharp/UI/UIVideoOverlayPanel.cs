using UnityEngine;

public class UIVideoOverlayPanel : MonoBehaviour
{
	public GameObject m_videoPlacement;

	private void Start()
	{
	}

	private void Update()
	{
		PlayRawImageMovieTexture videoPlayer = GetVideoPlayer();
		if (!videoPlayer)
		{
			return;
		}
		while (true)
		{
			if (videoPlayer.MovieState == PlayRawImageMovieTexture.MovieStates.Done)
			{
				while (true)
				{
					UILandingPageFullScreenMenus.Get().SetVideoContainerVisible(false);
					return;
				}
			}
			return;
		}
	}

	public PlayRawImageMovieTexture GetVideoPlayer()
	{
		if ((bool)m_videoPlacement)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return m_videoPlacement.GetComponentInChildren<PlayRawImageMovieTexture>();
				}
			}
		}
		return null;
	}

	public void PlayVideo(string movieAssetName)
	{
		PlayRawImageMovieTexture videoPlayer = GetVideoPlayer();
		if ((bool)videoPlayer)
		{
			bool loop = false;
			videoPlayer.Play(movieAssetName, loop);
		}
	}
}
