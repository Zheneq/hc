using System;
using UnityEngine;

public class UIFrontendTauntMouseoverVideo : UITooltipBase
{
	[Header("-- For Video --")]
	public PlayRawImageMovieTexture m_movieTexturePlayer;

	public void Setup(string movieAssetName)
	{
		if (this.m_movieTexturePlayer != null)
		{
			if (!movieAssetName.IsNullOrEmpty())
			{
				AudioManager.StandardizeAudioLinkages(this.m_movieTexturePlayer.gameObject);
				this.m_movieTexturePlayer.Play(movieAssetName, true, false, false);
				if (!this.m_movieTexturePlayer.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(this.m_movieTexturePlayer, true, null);
				}
			}
			else if (this.m_movieTexturePlayer.gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(this.m_movieTexturePlayer, false, null);
			}
		}
	}
}
