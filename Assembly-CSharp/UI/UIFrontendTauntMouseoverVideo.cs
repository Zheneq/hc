using UnityEngine;

public class UIFrontendTauntMouseoverVideo : UITooltipBase
{
	[Header("-- For Video --")]
	public PlayRawImageMovieTexture m_movieTexturePlayer;

	public void Setup(string movieAssetName)
	{
		if (!(m_movieTexturePlayer != null))
		{
			return;
		}
		if (!movieAssetName.IsNullOrEmpty())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					AudioManager.StandardizeAudioLinkages(m_movieTexturePlayer.gameObject);
					m_movieTexturePlayer.Play(movieAssetName, true, false, false);
					if (!m_movieTexturePlayer.gameObject.activeSelf)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								UIManager.SetGameObjectActive(m_movieTexturePlayer, true);
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (!m_movieTexturePlayer.gameObject.activeSelf)
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_movieTexturePlayer, false);
			return;
		}
	}
}
