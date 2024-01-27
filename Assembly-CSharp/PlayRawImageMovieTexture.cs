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

	public MovieStates MovieState => m_movieState;

	public void Update()
	{
		AudioSource audioSource = GetComponent<AudioSource>();
		if (!m_movieTexture)
		{
			return;
		}
		if (m_movieTexture.isPlaying)
		{
			m_playingStateFrameCounter++;
		}
		else
		{
			m_playingStateFrameCounter = 0;
		}
		if (m_movieState == MovieStates.Loading
		    && m_movieTexture.isReadyToPlay
		    && !m_movieTexture.isPlaying)
		{
			m_movieTexture.Play();
			RawImage rawImage = GetComponent<RawImage>();
			if (rawImage != null)
			{
				rawImage.texture = m_movieTexture;
				rawImage.color = Color.white;
			}
			if (audioSource != null && audioSource.clip != null)
			{
				audioSource.Play();
			}
		}
		if (m_movieState == MovieStates.Loading
		    && m_playingStateFrameCounter >= 2)
		{
			m_movieState = MovieStates.Playing;
		}
		if (m_movieState == MovieStates.Playing
		    && !m_movieTexture.loop
		    && !m_movieTexture.isPlaying)
		{
			m_movieState = MovieStates.Done;
		}
	}

	public bool Play(string movieAssetName, bool loop, bool silent = false, bool useVideoAudioBus = true)
	{
		m_movieState = MovieStates.Invalid;
		Unload();
		string url = "file://" + Application.streamingAssetsPath + "/" + movieAssetName + ".ogv";
		m_streamer = new WWW(url);
		m_movieTexture = m_streamer.GetMovieTexture();
		if (!m_movieTexture)
		{
			return false;
		}
		m_isAssetMovieTexture = false;
		RawImage rawImage = GetComponent<RawImage>();
		if (rawImage != null)
		{
			rawImage.color = Color.black;
		}
		AudioSource audioSource = GetComponent<AudioSource>();
		if (audioSource != null)
		{
			audioSource.clip = silent ? null : m_movieTexture.audioClip;
			if (audioSource.clip != null && useVideoAudioBus && AudioManager.GetMixerSnapshotManager() != null)
			{
				AudioManager.GetMixerSnapshotManager().SetMix_StartVideoPlayback();
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
		RawImage rawImage = GetComponent<RawImage>();
		if (rawImage != null)
		{
			rawImage.texture = null;
		}
		AudioSource audioSource = GetComponent<AudioSource>();
		if (audioSource != null)
		{
			audioSource.Stop();
			if (audioSource.clip != null && AudioManager.GetMixerSnapshotManager() != null)
			{
				AudioManager.GetMixerSnapshotManager().SetMix_StopVideoPlayback();
			}
			audioSource.clip = null;
		}
		if (m_movieTexture != null)
		{
			if (m_isAssetMovieTexture)
			{
				m_movieTexture.Stop();
			}
			else
			{
				Destroy(m_movieTexture);
			}
			m_movieTexture = null;
		}
		if (m_streamer != null)
		{
			m_streamer.Dispose();
			m_streamer = null;
		}
	}
}
