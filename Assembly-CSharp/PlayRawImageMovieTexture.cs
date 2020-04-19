using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayRawImageMovieTexture : MonoBehaviour
{
	private MovieTexture m_movieTexture;

	private bool m_isAssetMovieTexture;

	private WWW m_streamer;

	private int m_playingStateFrameCounter;

	private PlayRawImageMovieTexture.MovieStates m_movieState;

	public PlayRawImageMovieTexture.MovieStates MovieState
	{
		get
		{
			return this.m_movieState;
		}
	}

	public void Update()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		if (this.m_movieTexture)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayRawImageMovieTexture.Update()).MethodHandle;
			}
			if (this.m_movieTexture.isPlaying)
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
				this.m_playingStateFrameCounter++;
			}
			else
			{
				this.m_playingStateFrameCounter = 0;
			}
			if (this.m_movieState == PlayRawImageMovieTexture.MovieStates.Loading)
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
				if (this.m_movieTexture.isReadyToPlay)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.m_movieTexture.isPlaying)
					{
						this.m_movieTexture.Play();
						RawImage component2 = base.GetComponent<RawImage>();
						if (component2)
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
							component2.texture = this.m_movieTexture;
							component2.color = Color.white;
						}
						if (component && component.clip != null)
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
							component.Play();
						}
					}
				}
			}
			if (this.m_movieState == PlayRawImageMovieTexture.MovieStates.Loading)
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
				if (this.m_playingStateFrameCounter >= 2)
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
					this.m_movieState = PlayRawImageMovieTexture.MovieStates.Playing;
				}
			}
			if (this.m_movieState == PlayRawImageMovieTexture.MovieStates.Playing)
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
				if (!this.m_movieTexture.loop)
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
					if (!this.m_movieTexture.isPlaying)
					{
						this.m_movieState = PlayRawImageMovieTexture.MovieStates.Done;
					}
				}
			}
		}
	}

	public bool Play(string movieAssetName, bool loop, bool silent = false, bool useVideoAudioBus = true)
	{
		this.m_movieState = PlayRawImageMovieTexture.MovieStates.Invalid;
		this.Unload();
		string url = string.Concat(new string[]
		{
			"file://",
			Application.streamingAssetsPath,
			"/",
			movieAssetName,
			".ogv"
		});
		this.m_streamer = new WWW(url);
		this.m_movieTexture = this.m_streamer.GetMovieTexture();
		if (!this.m_movieTexture)
		{
			return false;
		}
		this.m_isAssetMovieTexture = false;
		RawImage component = base.GetComponent<RawImage>();
		if (component)
		{
			component.color = Color.black;
		}
		AudioSource component2 = base.GetComponent<AudioSource>();
		if (component2)
		{
			AudioSource audioSource = component2;
			AudioClip clip;
			if (silent)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(PlayRawImageMovieTexture.Play(string, bool, bool, bool)).MethodHandle;
				}
				clip = null;
			}
			else
			{
				clip = this.m_movieTexture.audioClip;
			}
			audioSource.clip = clip;
			if (component2.clip != null)
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
				if (useVideoAudioBus && AudioManager.GetMixerSnapshotManager() != null)
				{
					AudioManager.GetMixerSnapshotManager().SetMix_StartVideoPlayback();
				}
			}
		}
		this.m_movieTexture.loop = loop;
		this.m_movieState = PlayRawImageMovieTexture.MovieStates.Loading;
		this.m_playingStateFrameCounter = 0;
		return true;
	}

	public void OnDisable()
	{
		this.Unload();
	}

	private void Unload()
	{
		RawImage component = base.GetComponent<RawImage>();
		if (component)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(PlayRawImageMovieTexture.Unload()).MethodHandle;
			}
			component.texture = null;
		}
		AudioSource component2 = base.GetComponent<AudioSource>();
		if (component2)
		{
			component2.Stop();
			if (component2.clip != null && AudioManager.GetMixerSnapshotManager() != null)
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
				AudioManager.GetMixerSnapshotManager().SetMix_StopVideoPlayback();
			}
			component2.clip = null;
		}
		if (this.m_movieTexture)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_isAssetMovieTexture)
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
				this.m_movieTexture.Stop();
			}
			else
			{
				UnityEngine.Object.Destroy(this.m_movieTexture);
			}
			this.m_movieTexture = null;
		}
		if (this.m_streamer != null)
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
			this.m_streamer.Dispose();
			this.m_streamer = null;
		}
	}

	public enum MovieStates
	{
		Invalid,
		Loading,
		Playing,
		Done
	}
}
