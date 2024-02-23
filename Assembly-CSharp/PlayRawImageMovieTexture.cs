using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class PlayRawImageMovieTexture : MonoBehaviour
{
	public enum MovieStates
	{
		Invalid,
		Loading,
		Playing,
		Done
	}

	private MovieTexture m_movieTexture;

	private bool m_isAssetMovieTexture;

	private WWW m_streamer;

	private int m_playingStateFrameCounter;

	private MovieStates m_movieState;

	public MovieStates MovieState
	{
		get { return m_movieState; }
	}

	public void Update()
	{
		AudioSource component = GetComponent<AudioSource>();
		if (!m_movieTexture)
		{
			return;
		}
		while (true)
		{
			if (m_movieTexture.isPlaying)
			{
				m_playingStateFrameCounter++;
			}
			else
			{
				m_playingStateFrameCounter = 0;
			}
			if (m_movieState == MovieStates.Loading)
			{
				if (m_movieTexture.isReadyToPlay)
				{
					if (!m_movieTexture.isPlaying)
					{
						m_movieTexture.Play();
						RawImage component2 = GetComponent<RawImage>();
						if ((bool)component2)
						{
							component2.texture = m_movieTexture;
							component2.color = Color.white;
						}
						if ((bool)component && component.clip != null)
						{
							component.Play();
						}
					}
				}
			}
			if (m_movieState == MovieStates.Loading)
			{
				if (m_playingStateFrameCounter >= 2)
				{
					m_movieState = MovieStates.Playing;
				}
			}
			if (m_movieState != MovieStates.Playing)
			{
				return;
			}
			while (true)
			{
				if (m_movieTexture.loop)
				{
					return;
				}
				while (true)
				{
					if (!m_movieTexture.isPlaying)
					{
						m_movieState = MovieStates.Done;
					}
					return;
				}
			}
		}
	}

	public bool Play(string movieAssetName, bool loop, bool silent = false, bool useVideoAudioBus = true)
	{
		m_movieState = MovieStates.Invalid;
		Unload();
		string url = new StringBuilder().Append("file://").Append(Application.streamingAssetsPath).Append("/").Append(movieAssetName).Append(".ogv").ToString();
		m_streamer = new WWW(url);
		m_movieTexture = m_streamer.GetMovieTexture();
		if (!m_movieTexture)
		{
			return false;
		}
		m_isAssetMovieTexture = false;
		RawImage component = GetComponent<RawImage>();
		if ((bool)component)
		{
			component.color = Color.black;
		}
		AudioSource component2 = GetComponent<AudioSource>();
		if ((bool)component2)
		{
			object clip;
			if (silent)
			{
				clip = null;
			}
			else
			{
				clip = m_movieTexture.audioClip;
			}
			component2.clip = (AudioClip)clip;
			if (component2.clip != null)
			{
				if (useVideoAudioBus && AudioManager.GetMixerSnapshotManager() != null)
				{
					AudioManager.GetMixerSnapshotManager().SetMix_StartVideoPlayback();
				}
			}
		}
		m_movieTexture.loop = loop;
		m_movieState = MovieStates.Loading;
		m_playingStateFrameCounter = 0;
		return true;
	}

	public void OnDisable()
	{
		Unload();
	}

	private void Unload()
	{
		RawImage component = GetComponent<RawImage>();
		if ((bool)component)
		{
			component.texture = null;
		}
		AudioSource component2 = GetComponent<AudioSource>();
		if ((bool)component2)
		{
			component2.Stop();
			if (component2.clip != null && AudioManager.GetMixerSnapshotManager() != null)
			{
				AudioManager.GetMixerSnapshotManager().SetMix_StopVideoPlayback();
			}
			component2.clip = null;
		}
		if ((bool)m_movieTexture)
		{
			if (m_isAssetMovieTexture)
			{
				m_movieTexture.Stop();
			}
			else
			{
				Object.Destroy(m_movieTexture);
			}
			m_movieTexture = null;
		}
		if (m_streamer == null)
		{
			return;
		}
		while (true)
		{
			m_streamer.Dispose();
			m_streamer = null;
			return;
		}
	}
}
