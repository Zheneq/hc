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
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (videoPlayer.MovieState == PlayRawImageMovieTexture.MovieStates.Done)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
